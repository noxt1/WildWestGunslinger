# PHASE 5 CHECKPOINT — TECHNICAL BASELINE FREEZE

**Status:** CHECKPOINT — project intentionally frozen before art completion
**Date:** 2026-09-29
**Commit:** see §8
**Nature:** documentation only. **No C#, scene, prefab, material, FBX, Blender or AI change was made.**

---

## 1. Verified Technical Baseline

Verified by runtime evidence or by a clean build, in Phases 5A–5D.

| Area | Status | Evidence |
|---|---|---|
| Scene / reference integrity | improved | orphaned scanner refs removed, duplicate controller removed, refs serialized |
| `GunController` architecture | corrected | one canonical instance; fire path == upgrade path |
| Gun damage model | corrected | authored values honoured; no forced clamp |
| XP / level progression | **RUNTIME VERIFIED** | XP source, pickup, level-up, `UpgradeManager` integration |
| XP bar UI | **RUNTIME VERIFIED** | fill tracks XP exactly |
| Upgrade application | fixed | one click = one upgrade |
| Mobile reference serialization | corrected | all 3 `MobileTouchControls` refs point at canonical `Player` |
| Android build | **BUILD VERIFIED** | IL2CPP APK, **0 errors** |
| Runtime verification channel | **VERIFIED** | OpenCode → direct Unity MCP JSON-RPC fallback |

## 2. Closed Issues

| Ref | Issue | Closed by |
|---|---|---|
| `A-02` / `RT-06` | Orphaned deleted-scanner references on 4 enemy prefabs | Phase 5A (+5R runtime) |
| `A-01` | Original "12 broken references" claim reclassified — the real unresolved remainder is 2 | Phase 5A |
| `RT-04` / `UNI-D10` | `GunController.Awake` forced `damage ≥ 200` via `Mathf.Max(damage, 200f)` | Phase 5B |
| `ISSUE-22` / `RT-03` | Duplicate `GunController`; fire and upgrade paths on different instances | Phase 5B.1 |
| `ISSUE-23` | Every upgrade choice applied twice (`x1.44` instead of `x1.20`) | Phase 5C |
| `ISSUE-05` | Unity version not pinned | Phase 4 (earlier) |

`RT-04` is a **correctness** fix, not a balance decision. Authored base damage remains `100`.
**`DEC-11` stays OPEN** and is not settled by this checkpoint.

## 3. Partially Verified Issues

| Ref | Issue | Verified | Not verified |
|---|---|---|---|
| `RT-02` / `UNI-D07` | Mobile / Android input | PC runtime; Android build | **Android device runtime** |

Status is deliberately split: **PC RUNTIME VERIFIED** + **ANDROID BUILD VERIFIED** +
**ANDROID DEVICE NOT VERIFIED**. A successful APK is not a verified touch screen.

## 4. Open Issues

None of the following were closed by this checkpoint.

| Ref | Issue |
|---|---|
| `RT-01` / `UNI-D06` | Modular FBX import at 0.01×, Z-up uncompensated — blocks environment integration |
| `RT-05` / `UNI-D08`,`D09` | `Rusher`/`Shooter` prefab materials wrong |
| `RT-07` / `UNI-D13` | Level readout absent (XP **bar** is fine) |
| `RT-09` / `UNI-D14` | 4097 renderers / 1552 objects — Android performance unproven |
| `ISSUE-20` | `DIAG_TEMP_White` / `DIAG_TEMP_Gray` debris with 2 missing material GUIDs |
| `ISSUE-21` | Unity MCP tools not injected into the agent session (OpenCode remote-MCP limitation) |
| `ISSUE-11` | AI combat / investigation / sound / cover behaviours untested |
| `ISSUE-14` | Shadow aliasing, unfixed |
| — | **Android ABI**: build is `arm64-v8a` only; 32-bit `armeabi-v7a` unsupported |
| — | 68 `kinematic body` warnings from `EnemyController.cs:2297` (ambient, pre-existing) |
| `DEC-11` | Weapon base damage value — **OPEN, untouched** |

## 5. Deferred Integration

Deliberately deferred, not cancelled. All remain roadmap items.

- Modular environment integration
- Character Foundation integration
- Final clothing integration
- Final weapons / art integration
- Enemy model / art replacement
- Full AI runtime verification
- Android device verification

**Not** to be started while art production is in flight: Character Foundation, clothing,
weapon art, enemy placeholder replacement, modular environment integration, `ArenaGenerator`
rewrites to accommodate transitional assets.

## 6. Art Production In Progress

Each item stays `IN PROGRESS` until its art QA / approval gate declares otherwise. A working
model does **not** earn `FINAL`.

| Item | State |
|---|---|
| Environment wagon | `IN PROGRESS` |
| Player Character | `IN PROGRESS` |
| Character clothing | `IN PROGRESS` |
| Weapons | `IN PROGRESS` |
| Enemy characters / assets | `IN PROGRESS` |

**Character status, stated precisely:**

- *Source* — character source exists and is backed up.
- *Production* — character and clothing assets still being developed.
- *Unity* — **Character Foundation is NOT integrated.**
- *Runtime* — Animator / Avatar / equipment integration is future work.

The 51-bone rig is **not** claimed to be integrated into Unity.

**Weapons status:** `GunController` technical integrity is fixed and the damage architecture works;
actual weapon art assets are still in production; final integration deferred. `DEC-11` unchanged.

**Enemy status:** controller and runtime infrastructure exist; the orphaned scanner reference is
fixed; enemy character/art replacement pending; full tactical AI verification incomplete. No AI
rewrite is to begin.

## 7. Runtime Verification Status

Working path:

```
OpenCode  ->  direct Unity MCP JSON-RPC fallback  ->  Unity Editor
```

`ISSUE-21` remains **OPEN**: the OpenCode remote-MCP injection limitation is not resolved merely
because the JSON-RPC fallback works. The fallback is a workaround, not a fix.

## 8. Git State

| Field | Value |
|---|---|
| Baseline commit | `131a0fd` — `fix: harden mobile controller resolution` |
| This checkpoint | `docs: checkpoint technical baseline before art completion` |
| `origin/main` | `f932fcc` — behind 2 / ahead 9 |
| Push | **NO** |
| Staging | exact manifest, documentation only; no `git add .` |
| Untouched | 94 pre-existing product-state entries, unfinished art, `Working/`, Unity artifacts |
| Recovery refs | branch and tag `040651b` intact |

## 9. Next Integration Gate

Scheduling state: **ART COMPLETION GATE** — complete current production models and pass their
asset QA / approval gates before large-scale Unity integration resumes. This is a temporary
execution state, **not** a new permanent stage number; no existing stage was renumbered.

When the current models are reported complete, the next task is:

**ART FINALIZATION + INTEGRATION READINESS AUDIT**

followed by: art QA → approval → integration plan → Unity integration → runtime verification →
documentation update → Git checkpoint.
