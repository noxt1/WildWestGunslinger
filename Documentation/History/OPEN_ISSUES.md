# OPEN ISSUES

**Status:** CANONICAL register of open defects, missing artefacts, and gaps
**Date:** 2026-09-29

Unity gameplay/engine defects live in `UNI-0003` with severities. This file
tracks **missing artefacts, unidentified defects, and documentation gaps**.

---

## Runtime issues (audit refs preserved)

The U-12 runtime audit numbered its findings `RT-01`…`RT-09`. Those identifiers
are **preserved** here and mapped to the canonical `UNI-D*` register. None is
closed. `RT-*` items must never be silently dropped when editing
`UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md`.

| Audit ref | Issue | Canonical | Status |
|---|---|---|---|
| **`RT-01`** | Modular FBX import at 0.01×, Z-up uncompensated (4 m wall → 4 cm tile) | `UNI-D06` | **OPEN** — blocks all environment promotion |
| **`RT-02`** | `MobileTouchControls` all 3 refs null — inert on PC, **activates on Android** | `UNI-D07` | **OPEN** — Android blocker |
| `RT-03` | `FireButton`/`GunController` duplicate; `weaponPoint` → Player root; damage 10→200 | `UNI-D10` (partial) | **OPEN** — the duplicate/`weaponPoint` element has no dedicated entry |
| **`RT-04`** | `GunController.Awake` forces `damage ≥ 200`, discarding Inspector values | `UNI-D10` | **OPEN** — code confirmed at `GunController.cs:37` |
| **`RT-05`** | `Rusher.prefab` = `FrameDebuggerRenderTargetDisplay`; `Shooter.prefab` = `TMP_SDF-HDRP LIT` | `UNI-D08`, `UNI-D09` | **OPEN** |
| `RT-06` | 9 missing-script warnings per spawn cycle; scales with enemy count | `UNI-D05` | **OPEN** |
| `RT-07` | `HUDController.xpBar` / `levelText` null — level never displayed | **`UNI-D13`** | **OPEN** — added in Phase 3 |
| `RT-08` | 12 broken references, all runtime-confirmed | `UNI-D04` | **OPEN** — unidentified |
| `RT-09` | 4097 renderers / 1552 objects at runtime — Android perf unproven | **`UNI-D14`** | **OPEN** — added in Phase 3 |

> **Phase 3 gap closed.** `RT-07` and `RT-09` were absent from the Phase 2
> register. They are now `UNI-D13` and `UNI-D14`. All nine audit refs are mapped.

### AI runtime not verified

| Behaviour | Status |
|---|---|
| AI combat | **NOT VERIFIED** |
| AI investigation | **NOT VERIFIED** |
| AI sound propagation / reaction | **NOT VERIFIED** |
| AI cover-taking | **NOT VERIFIED** |
| AI flanking | **NOT VERIFIED** |

`UNI-D04`/`UNI-D05` may already explain this — see `ISSUE-01`/`ISSUE-02`.

### Absent systems that must stay listed as open

| Item | Status |
|---|---|
| Character Foundation in Unity | **ABSENT** — `UNI-D02` |
| Missing Animator / Avatar / Controller | **ABSENT** — `UNI-D02` |
| Modular environment integration | **ABSENT** — `UNI-D11`, 0 meshes |
| NavMesh | **ABSENT** — `UNI-D01`, 0 agents / 0 triangulation |

### Release security items

| Item | Status |
|---|---|
| `REL-0003` §19.1.3 — search Unity project **and Git history** for secrets | **PARTIAL** — working tree only (0 findings / 731 files); **Git history not swept** |
| `REL-0003` §19.1.4 — check the final APK/AAB for development credentials | **NOT STARTED** — no current APK exists |

---

## Unidentified defects — must be enumerated first

### ISSUE-01 — 12 broken object references, unidentified
- **Severity:** Critical
- **Known:** 12 references resolve to `None` at runtime
- **Unknown:** which GameObjects, which fields, which systems they disable
- **Why first:** a nulled reference can silently disable a whole system and may explain several "untested" AI behaviours
- **Ref:** `UNI-D04`

### ISSUE-02 — 4 missing Mono Script references, unidentified
- **Severity:** Critical
- **Known:** 4 components show `Missing (Mono Script)`
- **Unknown:** the owning GameObjects and the lost logic
- **Approach:** identify by script GUID against the repository before any repair
- **Ref:** `UNI-D05`

---

## Missing artefacts

### ISSUE-03 — `Вставленная уценка.md` not found
- **Severity:** Low
- Referenced as a project document; absent from `Downloads` and everywhere searched
- Either it was never created, or it lives somewhere not yet searched

### ISSUE-04 — Vest reference source images missing
- **Severity:** Medium
- `Desktop\Коллаж тест VVG\00_Vest_Reference\REFERENCE_INDEX.txt` catalogues **14 reference images** and states the originals **could not be extracted** (no tool access to chat attachments)
- Only the index survives. Provenance for those 14 items is descriptive only
- **Note:** this does not affect the vest itself, whose measurements are recorded in `Doomy_vest_measurements.md`

### ISSUE-05 — Unity version not pinned — ✅ RESOLVED 2026-09-29 (Phase 4)
- **Severity:** Medium → **RESOLVED**
- The version **is** recorded: `ProjectSettings/ProjectVersion.txt` contains `m_EditorVersion: 6000.3.23f1` and `m_EditorVersionWithRevision: 6000.3.23f1 (09d2ecc7fb28)`
- Phase 2/2.5 wrongly recorded this as "not recorded in any project document" — the check was run against documentation, not against `ProjectSettings/`
- Now documented in `../UNI/UNI-0001-UNITY-PROJECT-STATE.md` §1
- **Residual:** whether the version should additionally be *asserted* (e.g. a CI guard against accidental Editor upgrade) is still undecided

### ISSUE-06 — Character FBX not present in `Assets/`
- **Severity:** High
- Five character FBX and four weapon FBX exist in the art source (`Documents\…`)
- No integrated character FBX is present in `Assets/`, and the scene has 0 `SkinnedMeshRenderer`
- This is expected given `UNI-D02`, but means the promotion target is undefined

### ISSUE-07 — Package manifest not captured
- **Severity:** Low
- The audit reports contain no package inventory. URP is inferred from the material name `TMP_SDF-HDRP LIT`
- Capture `Packages/manifest.json` into `UNI-0004`

### ISSUE-08 — No authoritative game design document
- **Severity:** Medium
- Design intent is scattered across `CHATGPT_*`, `ART_PIPELINE.md`, `AI_CONTEXT/*`
- `GAME-0001` is the first consolidated statement, built from runtime evidence rather than design intent
- True design intent still needs to be authored by a human

### ISSUE-09 — No CI, no tests, no build pipeline
- **Severity:** High
- Nothing automated exists: no CI, no test suite, no build script, no release config
- Every defect in `UNI-0003` must therefore be found and verified manually

### ISSUE-10 — Audit 1 has no artefact on disk
- **Severity:** Low
- Audit 1 exists only in conversation history. Its findings survive only through this documentation set and `HISTORY-0003`
- No report file was ever written

---

## Evidence gaps

### ISSUE-11 — Five AI behaviours untested
Combat, investigation, sound propagation, cover-taking, flanking.
**Status:** must not be reported as working. See `GAME-0002` §3

### ISSUE-12 — 146 of 307 art files unaccounted for in the 2026-09-22 backup
- The `Z:` art copy holds 161 files; the live source holds 307
- The manifest does not record the exclusion criteria
- **Ref:** `CONFLICT-10` / `RISK-01`

---

## Phase 4 discoveries — new open issues

### ISSUE-13 — 4 enemy prefabs hold a dangling script reference
- **Severity:** High (console noise) — **✅ CLOSED, RUNTIME VERIFIED 2026-09-29 (Phase 5R)**
- **Root cause of:** `UNI-D05` (4 missing Mono Scripts) and `RT-06` (9 warnings per spawn cycle)
- **Detail:** `Assets/Scripts/Enemies/EnemyTacticalEnvironmentScanner_TEST.cs` was deleted as temporary diagnostic cleanup. Its `.meta` is gone, but `Bandit.prefab`, `Rusher.prefab`, `Shooter.prefab` and `Tactical.prefab` still reference script GUID `809b48f6d9f34f34d90ca85975a9c332` in the component slot between `EnemyHealth` and `EnemyTacticalPlanner`
- **Symptom:** `The referenced script (Unknown) on this Behaviour is missing!` — 7 warnings on one `Bandit`; scales with enemy count
- **Not a compile error:** all code references to the deleted type were already removed; project compiles with 0 errors
- **✅ FIXED 2026-09-29 (Phase 5A):** the orphaned component block and its m_Component entry were removed from all 4 prefabs. GUID occurrences 4 -> 0. Root-cause investigation showed the capability was absorbed into EnemyController (obstacleProbeRadius, obstacleRouteProbeDistance, obstacleStuckTime, obstacleRouteActive); the deleted component had 0 callers
- **Verified:** 2026-09-29 against the filesystem

### ISSUE-14 — Shadow Aliasing is an open, unfixed rendering problem
- **Severity:** Medium (visual quality)
- **Detail:** blocky/aliased shadows observed. **Not fixed** in the 2026-09-27 phase
- **Current settings:** Main Light shadow resolution **1024**, cascade count **1**, shadow distance **50**, PCF filter
- **Next test discipline:** change **one** shadow setting at a time; first candidate `1024 → 2048`
- **Must not** be reported as resolved

### ISSUE-15 — Square/blocky shadow diagnosis queued behind the arrow A/B test
- **Severity:** Low
- **Recorded by:** the project owner as a separate observation
- **Sequencing:** performed as a separate step **after** the arrow-position A/B test is decided

### ISSUE-16 — `EnemyDirectionIndicator` arrow radial position is an open visual question
- **Severity:** Low (cosmetic)
- **Detail:** arrows are placed at `worldRingRadius = 0.835` but visually read as sitting on the ring's *inner* contour. An A/B test must choose: inner contour / mid-thickness (`0.785`) / outer contour (`0.835`)
- **Explicitly NOT a bug:** direction logic is mathematically verified (`dot = 1.000000`, angular error `0.0000°`). Do not change direction logic, ring size, or `worldRingHeightOffset = 0.02` without cause

### ISSUE-17 — Duplicate scene root cause now known
- **Severity:** Low, but it caused data loss
- **Root cause:** an MCP scene save was written to `Assets/TestArena.unity` instead of `Assets/Scenes/TestArena.unity`. Some object deletions saved into the duplicate **never reached the real scene**
- **Status:** duplicate still present, untracked, **cleanup forbidden** pending `DEC-09`
- **Consequence to remember:** edits made through MCP may have silently gone to the wrong file. Any past "missing object" report should be re-checked against this

### ISSUE-18 — `AI_CONTEXT/ARCHITECTURE.md` contains a corrupted token
- **Severity:** Low (documentation)
- **Detail:** line reads `Другие EnemyController блокируют视线` — Chinese characters embedded in Russian text
- **Action:** corrected in the Phase 4 rewrite of `AI_CONTEXT/ARCHITECTURE.md`

### ISSUE-19 — `AI_CONTEXT` was never reconciled with canonical docs
- **Severity:** High (process)
- **Detail:** Phase 2/2.5 prepended `SUPERSEDED` headers to `AI_CONTEXT` files **without reading their content**. As a result the canonical set was wrong on: asset approvals, `CRATE_01` CP1/CP2, Unity version, script inventory, XP/progression status, and the `UNI-D05` root cause — while 763 lines of real project documentation sat uncommitted and unreconciled
- **Lesson:** superseding a document requires **reading** it first
- **Status:** corrected in Phase 4 — see `../../PHASE4_AI_CONTEXT_RECONCILIATION.md`

---

### ISSUE-20 — Two `DIAG_TEMP_*` diagnostic debris objects with missing materials
- **Severity:** Low (visual debris; no functional impact)
- **Detail:** `DIAG_TEMP_White` (missing material GUID `5e0759d7869747ad89fefa597cdcd781`) and `DIAG_TEMP_Gray` (missing material GUID `e0b90311a8d04983a42b7fa1b1d8977b`) sit in `Assets/Scenes/TestArena.unity`. Both use hand-authored fileIDs (`910000001`), the Unity built-in Quad mesh, are `m_Enabled: 1` and cast shadows
- **Origin:** leftovers from a washed-out-scene diagnosis session; the name prefix is explicit
- **These are the only 2 genuinely broken asset references in the scene** (Phase 5A reclassified the former ""12 broken references"")
- **Action:** removal is a destructive scene edit and requires a user decision. **Not removed**
- **Status:** REVIEW - awaiting decision
### ISSUE-21 - Unity MCP tools not injected into the agent session
- **Severity:** Medium (blocks `RUNTIME VERIFIED`; workaround exists)
- **Detail:** `mcp-for-unity-server` v3.4.7 runs correctly on `127.0.0.1:8080` and `unityMCP` is correctly configured in `opencode.json` (`type: remote`, `url: http://127.0.0.1:8080/mcp`, `enabled: true`). However the Unity tools are **absent from the agent's toolset** for the session, while the `local`-type `blenderMCP` tools load normally
- **Rejected causes (with evidence):** server not running, port unavailable, config mismatch, transport failure, protocol init failure - all disproved by a successful `initialize` + `tools/list` (48 tools)
- **Confirmed cause:** OpenCode-side MCP client session initialisation for **remote** type servers
- **Workaround (documented and proven):** direct JSON-RPC to `http://127.0.0.1:8080/mcp`. See `../PHASE5R_UNITY_RUNTIME_CHANNEL_REPORT.md` section 6 for the exact procedure, including the mandatory `Mcp-Session-Id` header and the SSE `data:` frame extraction
- **Status:** **MITIGATED** - runtime verification is fully possible via the fallback. The underlying OpenCode integration issue remains open upstream
### ISSUE-22 - Duplicate `GunController`: firing and upgrades bound to different instances
- **Severity:** ~~High~~ -> **RESOLVED**
- **Status:** **CLOSED - FIXED, RUNTIME VERIFIED 2026-09-29 (Phase 5B.1)**
- **BEFORE:** fire controller `67492` (`MobileUI/HUD/FireButton`) != upgrade controller `67864` (`Player`); `SPLIT = True`
- **ROOT CAUSE:** `MobileTouchControls.gunController` was never assigned in the scene (`fileID: 0`); the class compensated with unordered `FindFirstObjectByType<GunController>()`, which returned a `GunController` handle that had been attached to the `FireButton` UI control. That duplicate was mis-configured (weaponPoint -> Player root, `playerController` = NULL). The real weapon controller on `Player` already existed but was referenced by nothing in the fire path
- **AFTER:** one canonical instance `Player#70770` for both fire and upgrade paths; `FireButton` holds no `GunController`; `FindFirstObjectByType<GunController>()` removed from the codebase
- **VERIFICATION:** `TEST4_IDENTITY_EQUAL = True` (70770 == 70770), controller count 2 -> 1, 0 project-origin console errors
- **Damage values:** Player `100` unchanged; the duplicate's `10` was deleted together with the obsolete component. Final balance value remains **`DEC-11` (still OPEN)**
- **Report:** `Documentation/PHASE5B1_GUNCONTROLLER_IDENTITY_REPORT.md`
## Cross-references

- `UNI-0003` — Unity defects with severity
- `OPEN_DECISIONS.md` — decisions
- `OPEN_RISKS.md` — risks
- `OPEN_CONFLICTS.md` — source conflicts

---

**End of `OPEN_ISSUES.md`**
