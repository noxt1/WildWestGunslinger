# PHASE 5R — UNITY RUNTIME VERIFICATION CHANNEL

**Document ID:** `DOC-P5R-2026-09-29`
**Status:** COMPLETE — **RUNTIME CHANNEL PASS**
**Date:** 2026-09-29
**Base commit:** `bbd8f9a` (Phase 5A)

---

## 1. Initial state

| Item | State before |
|---|---|
| Canonical docs | current in Git (`bbd8f9a`) |
| Unity Editor | **running** — 6000.3.23f1 |
| MCP for Unity server | **running**, listening on `127.0.0.1:8080` |
| OpenCode `unityMCP` config | present and `enabled: true` |
| Agent's own Unity tool channel | **absent** — no Unity tools in the session toolset |
| Consequence | every runtime claim was `NOT VERIFIED`; Phase 5A could only reach STATIC |

---

## 2. Unity state

| Field | Value |
|---|---|
| Version | **6000.3.23f1** (matches `ProjectVersion.txt`) |
| Editor PID | 11320 |
| Command line | `-projectpath C:\Users\cyril\WildWestGunslinger` — **correct project** |
| Started | 2026-09-29 16:10:14 |
| Asset workers | 2 (PIDs 28224, 33412) |
| Active scene | `TestArena` → `Assets/Scenes/TestArena.unity` |
| Build index | 1 |
| `isDirty` | **false** (no unsaved changes at session start) |
| Root objects | 20 |
| Play Mode | available and functional |

---

## 3. MCP state

| Field | Value |
|---|---|
| Process | `mcp-for-unity.exe` (python 3.11, uv cache) |
| PID | 25076, started 2026-09-29 16:12:32 |
| Command | `--transport http --http-url http://127.0.0.1:8080 --project-scoped-tools` |
| Server | **`mcp-for-unity-server` version 3.4.7** |
| Endpoint | `http://127.0.0.1:8080/mcp` (`GET /` correctly returns 404) |
| Transport | Streamable HTTP, `Accept: application/json, text/event-stream` |
| Protocol | `2024-11-05` |
| Session | established via `initialize` → `Mcp-Session-Id` header |
| `tools/list` | **48 tools** |
| `notifications/initialized` | HTTP 202 |

Key tools confirmed present: `manage_editor` (play/pause/stop), `read_console`,
`manage_scene`, `find_gameobjects`, `manage_components`,
`manage_scriptable_object`, `execute_code`, `manage_prefabs`, `manage_material`,
`refresh_unity`, `run_tests`, `set_active_instance`.

---

## 4. OpenCode state

`~/.config/opencode/opencode.json` — `mcp` block:

| Key | Value | Correct? |
|---|---|---|
| `unityMCP.type` | `remote` | ✅ |
| `unityMCP.url` | `http://127.0.0.1:8080/mcp` | ✅ matches the running server |
| `unityMCP.enabled` | `true` | ✅ |
| `blenderMCP` | `local`, port 9876 | ✅ working (its tools are present in-session) |

**No configuration change was required or made.** The configuration was already
correct in every respect.

---

## 5. Root cause

**Evidence-based conclusion: this was never a server, port, or configuration fault.**

| Candidate cause | Verdict | Evidence |
|---|---|---|
| MCP server not running | **REJECTED** | PID 25076 alive since 16:12:32 |
| Port unavailable | **REJECTED** | `127.0.0.1:8080` in `Listen` state |
| Configuration mismatch | **REJECTED** | configured URL matches the server's `--http-url` exactly |
| Stale registry | **NOT APPLICABLE** | the server is a stateless remote endpoint; no registry |
| Transport problem | **REJECTED** | `initialize` returned HTTP 200 with a valid session id |
| Protocol initialisation problem | **REJECTED** | `tools/list` returned 48 tools |
| **OpenCode integration problem** | **CONFIRMED** | the Unity tools are **absent from this session's toolset** while `blenderMCP` tools are present |

**Confirmed cause:** the `unityMCP` server was healthy and correctly configured,
but its tools were never registered into the agent's toolset for this session.
The `blenderMCP` server is `type: local` and its tools loaded normally; `unityMCP`
is `type: remote`. The failure is therefore on the **OpenCode-side MCP client
session initialisation for remote servers**, not in Unity, not in the Unity MCP
package, and not in configuration.

> **Consequence for future work:** a session may again come up without Unity tools.
> The reliable path is the direct JSON-RPC fallback, now fully documented in §6.

---

## 6. Recovery

**No configuration or project file was changed.** Per §5, since the штатный
in-toolset path was unavailable, the documented **direct read-only JSON-RPC
path** (the same approach used in Audit 2) was validated and fixed as the
fallback.

### Working procedure (documented for reuse)

1. `POST http://127.0.0.1:8080/mcp`
   Headers: `Accept: application/json, text/event-stream`, `Content-Type: application/json`
   Body: `{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2024-11-05","capabilities":{},"clientInfo":{"name":"...","version":"1.0"}}}`
2. Read the **`Mcp-Session-Id`** response header. It is **required** on every later call.
3. `POST` `{"jsonrpc":"2.0","method":"notifications/initialized"}` → expect HTTP 202.
4. Call tools: `{"jsonrpc":"2.0","id":N,"method":"tools/call","params":{"name":"<tool>","arguments":{...}}}`
5. The response is SSE-framed: extract the line beginning `data: `.

**Two gotchas found and fixed during this phase:**

| Gotcha | Symptom | Resolution |
|---|---|---|
| `Accept` header must include `text/event-stream` | server may not frame correctly | use both types |
| `Mcp-Session-Id` is mandatory after `initialize` | `-32602 Invalid request parameters` on every call | carry the header on all requests |
| PowerShell `ConvertTo-Json` nested-hashtable serialization produced a body the server rejected | `-32602 Invalid request parameters` | send **raw JSON strings**, not converted hashtables |

---

## 7. Runtime smoke test

All steps passed. Read-only; **nothing was saved**.

| # | Step | Tool | Result |
|---|---|---|---|
| 1 | Connect | `initialize` | ✅ HTTP 200, session issued |
| 2 | Read editor state | `manage_editor` → `telemetry_ping` | ✅ `{"success":true}` |
| 3 | Read loaded scenes | `manage_scene` → `get_loaded_scenes` | ✅ 1 scene: `TestArena`, buildIndex 1, `isDirty: false` |
| 4 | Read active scene | `manage_scene` → `get_active` | ✅ path, buildIndex, 20 roots |
| 5 | Inspect hierarchy | `manage_scene` → `get_hierarchy` | ✅ all 20 roots with component types |
| 6 | Clear console | `read_console` → `clear` | ✅ |
| 7 | **Enter Play Mode** | `manage_editor` → `play` | ✅ `Entered play mode.` |
| 8 | Read Console | `read_console` → `get` | ✅ 446 entries |
| 9 | Inspect runtime objects | `find_gameobjects` → `by_component` | ✅ **3 live `EnemyController`** |
| 10 | Check removed component | `find_gameobjects` → `by_component` `EnemyTacticalEnvironmentScanner` | ✅ **0 objects** |
| 11 | **Exit Play Mode** | `manage_editor` → `stop` | ✅ `Exited play mode.` |

**RUNTIME CHANNEL PASS** — every operation completed with no manual bypass other
than the documented JSON-RPC fallback.

---

## 8. Console

446 entries read during a full play cycle.

| Finding | Count | Assessment |
|---|---|---|
| `The referenced script (Unknown) on this Behaviour is missing!` | **0** | ✅ **RT-06 fix confirmed at runtime** |
| `Missing (Mono Script)` | **0** | ✅ |
| `NoSubscription` from `generators.ai.unity.com` | 5 | **Unity AI service**, unrelated to the project — no subscription, not a code defect |
| `Editor is not in automated mode` | 1 | informational — the Editor was not started with `-automated`; modal dialogs may interrupt automation |

No project-origin errors were observed.

---

## 9. Product findings — observations only, nothing fixed

Per §8, no product issue was touched. Recorded for the next remediation task:

| # | Observation | Evidence |
|---|---|---|
| 1 | **`DIAG_TEMP_White` / `DIAG_TEMP_Gray` confirmed live in the scene**, `activeInHierarchy: true`, both `Transform + MeshFilter + MeshRenderer`, both with unresolvable material GUIDs. `ISSUE-20` is real, not a static-only artifact | hierarchy read, instanceIDs 67714 / 67722 |
| 2 | `Player` carries **`XPManager` and `UpgradeManager`** — confirms the Phase 4 finding that progression code exists, and that the missing `xpBar`/`levelText` are wiring/UI, not missing logic | `Player` component list |
| 3 | 3 enemies spawned and are live — enemy spawning is functioning in the current build | `find_gameobjects` by_component |
| 4 | `EnemyTacticalEnvironmentScanner` component is **completely absent** from the runtime scene — the Phase 5A prefab fix took effect | 0 objects |
| 5 | `Ground`, `ArenaModule_Start`, `ArenaModule_Straight`, `ArenaVisualStyle` are **`activeSelf: false`** in the hierarchy — expected for generator output not yet instantiated, recorded as observation only | hierarchy read |

---

## 10. Documentation

| Document | Update |
|---|---|
| `Documentation/UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md` | `UNI-D05` / `RT-06` **→ `RUNTIME VERIFIED`** with console evidence |
| `Documentation/History/OPEN_ISSUES.md` | `ISSUE-13` **CLOSED (runtime verified)**; new `ISSUE-21` — no Unity tool channel in session, with the working fallback documented |
| `Documentation/PROJECT_STATE.md` | runtime-verification capability row added |
| `Documentation/PHASE5A_CORE_INTEGRITY_REPORT.md` | §6/§7/§9 upgraded from `NOT PERFORMED` to the achieved result |
| `Documentation/UNI/UNI-0001-UNITY-PROJECT-STATE.md` | §9 runtime verification channel documented |
| `Documentation/PHASE5R_UNITY_RUNTIME_CHANNEL_REPORT.md` | this report |

---

## 11. Git

| Field | Value |
|---|---|
| Message | `chore: restore Unity runtime verification channel` |
| Scope | documentation only — **0 product files** |
| Config files changed | **none** — the existing configuration was already correct |
| Prohibited operations | none — no `add .`, `amend`, `merge`, `rebase`, `reset`, `clean`, `stash`, `push` |

---

## 12. Outcome

The runtime verification channel is **restored and proven**. Future remediation
clusters can now reach `RUNTIME VERIFIED` instead of stopping at `STATIC VERIFIED`.

**One bounded limitation remains:** the Unity tools are still not injected into the
agent's toolset automatically, so the documented JSON-RPC fallback remains the
working path until OpenCode's remote-MCP session initialisation is fixed
server-side. The fallback is fully specified in §6.

---

**End of `PHASE5R_UNITY_RUNTIME_CHANNEL_REPORT.md`**
