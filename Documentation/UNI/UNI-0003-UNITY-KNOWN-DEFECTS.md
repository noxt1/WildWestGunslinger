# UNI-0003 — Unity Known Defects

**Document ID:** `UNI-0003`
**Status:** CANONICAL
**Date:** 2026-09-29
**Evidence class:** runtime observation (U-12, 2026-09-29)

Every defect below was **observed in play mode**, not inferred from documentation.

---

## 1.0 Runtime fixes verified on 2026-09-27 (pre-existing, recorded)

**Added in Phase 3.** These fixes were made and runtime-verified on 2026-09-27
and are recorded in `AI_CONTEXT/CHANGELOG.md`. They are listed here as **resolved
runtime evidence**, kept separate from the open defects below and from later
audit assumptions.

### FIX-01 — White HUD overlay in `TestArena` (permanent fix)

| Field | Value |
|---|---|
| Symptom | Persistent white haze over the playable scene after an earlier washed-out-scene fix |
| Cause | Object `TestArena → MobileUI → HUD` held a fullscreen `Image` component, semi-transparent white `RGBA(1, 1, 1, 0.392)`, `anchorMin 0,0` / `anchorMax 1,1` / `sizeDelta 0×0` — stretched across the whole Canvas |
| Fix | `m_Enabled: 0` on that `Image` component, saved via `EditorSceneManager.MarkSceneDirty` + `SaveScene`; recorded in `Assets/Scenes/TestArena.unity` at fileID `2096943043` |
| Preserved | `HUD`, `MobileUI`, the `Canvas`, and all children — `HealthBar`, `XPBar`, `JoystickBackground`, `FireButton`, `WaveText`, `PauseButton`. `MobileUI` was **not** disabled wholesale; `MobileTouchControls` was **not** modified |
| Runtime verification | `Main Menu → Play → TestArena`, **two runs**, including a scene reload from disk. `Image.enabled = False` both times |
| Measured result | No white overlay; HP Bar, XP Bar, WaveText, Joystick, Fire Button, Menu Button all display; **FPS 60.0; Console errors = 0** |
| Status | Implemented YES · Compiled YES (0 errors) · Tested YES (runtime, 2 runs) · **Confirmed NO** — awaiting user visual confirmation |

> **`Confirmed = NO` is the recorded status.** The runtime evidence is real and
> reproducible, but the user has not visually signed it off. This is the correct
> distinction between *tested* and *confirmed*.

### FIX-02 — Geometry aliasing A/B tests (renderScale, MSAA)

Diagnostic A/B testing of render scale and MSAA against Shadow Aliasing and
Geometry/Edge Aliasing, recorded 2026-09-27. Relevant to `Assets/Settings/Mobile_RPAsset.asset`
and `PC_RPAsset.asset`, which remain **modified and uncommitted** in the working
tree.

> Scope note: these are **2026-09-27 records**. They are not superseded by the
> 2026-09-29 U-12 audit, which did not evaluate render settings. Both are
> retained; neither overrides the other.

---

## 1. Open defects

### Severity scale

| Severity | Meaning |
|---|---|
| **Critical** | Blocks release, or blocks a target platform |
| **High** | Visible wrong behaviour in the core loop |
| **Medium** | Feature absent or degraded but not blocking |

---

## Critical

### UNI-D01 — Zero NavMesh
- **Observed:** 0 agents, 0 triangulation.
- **Impact:** any NavMesh-based agent is non-functional. Current enemy movement depends entirely on the custom A\* pathfinder, which means the baked navigation asset is missing, not merely unconfigured.
- **State:** unfixed.

### UNI-D02 — No character rig integrated
- **Observed:** 0 `Animator`, 0 `Avatar`, 0 `AnimatorController`, 0 `SkinnedMeshRenderer`.
- **Impact:** characters cannot be animated. No animation state is reachable in game.
- **State:** unfixed. Character source work exists (see `ART-0001`) but nothing is integrated.

### UNI-D03 — Player and enemies are Capsules
- **Observed:** both actor types use primitive capsule geometry.
- **Impact:** no visual character in the playable build.
- **State:** unfixed, downstream of `UNI-D02`.

### UNI-D04 — Scene reference integrity — ✅ RECLASSIFIED in Phase 5A
- **Previous claim:** "12 broken object references" — `None` at runtime.
- **Phase 5A finding (static enumeration of `Assets/Scenes/TestArena.unity`):** the figure of 12 counted **unassigned serialized fields** that are *not* broken references. Full enumeration:

| Group | Count | Actual behaviour | Broken? |
|---|---|---|---|
| `ArenaGenerator.fencePrefab / coverPrefab / cratePrefab / barrelPrefab / wagonPrefab / lanternPrefab / logWallPrefab` | 7 | Gated by `HaveClusterPrefabs()` (requires fence+cover+crate+barrel). All 7 null → generator **falls back to `BuildLegacyCovers()`** and builds covers as primitives. Graceful by design | **No** |
| `MobileTouchControls.playerController / gunController / playerCamera` | 3 | Self-healed at runtime: `FindFirstObjectByType<PlayerController>()`, `FindFirstObjectByType<GunController>()`, `Camera.main` | **No** (see `UNI-D07` caveat) |
| `HUDController.xpBar / levelText` | 2 | **Dead fields** — `HUDController.cs` (53 lines) never reads them. `UpdateHealth()` touches only `healthBar` and `healthText`. No XP UI objects exist in the scene | **No** — feature simply not implemented |

- **Genuinely broken references found instead:** **2**, not 12. Both are unresolvable **material** GUIDs on temporary diagnostic objects:

| Object | Missing material GUID | Note |
|---|---|---|
| `DIAG_TEMP_White` | `5e0759d7869747ad89fefa597cdcd781` | hand-authored fileID `910000001`, Unity built-in Quad mesh, `m_Enabled: 1`, casts shadows |
| `DIAG_TEMP_Gray` | `e0b90311a8d04983a42b7fa1b1d8977b` | same pattern |

  Named `DIAG_TEMP_*` — leftovers from a washed-out-scene diagnosis session. All 57 GUID references in the scene were resolved; the other 15 are Unity built-ins or package assets (URP, uGUI, Input System, TMP).
- **State:** reclassified. The 2 diagnostic debris objects are **REVIEW — removal requires a user decision** (scene edit, destructive). Not removed by Phase 5A.

### UNI-D05 — 4 missing Mono Script references — ✅ FIXED 2026-09-29 (Phase 5A)
- **Observed:** 4 components show `Missing (Mono Script)`.
- **Impact:** the owning GameObjects have lost their logic entirely; behaviour is silently absent.
- **ROOT CAUSE IDENTIFIED in Phase 4** (verified against the filesystem, 2026-09-29): the diagnostic script `Assets/Scripts/Enemies/EnemyTacticalEnvironmentScanner_TEST.cs` was deleted as temporary cleanup. Its `.meta` is gone, but **4 enemy prefabs still hold a serialized reference to script GUID `809b48f6d9f34f34d90ca85975a9c332`**:

  | Prefab | Dangling reference |
  |---|---|
  | `Assets/Prefabs/Enemies/Bandit.prefab` | ✅ confirmed |
  | `Assets/Prefabs/Enemies/Rusher.prefab` | ✅ confirmed |
  | `Assets/Prefabs/Enemies/Shooter.prefab` | ✅ confirmed |
  | `Assets/Prefabs/Enemies/Tactical.prefab` | ✅ confirmed |

  The component slot sits **between `EnemyHealth` and `EnemyTacticalPlanner`**. All references in `EnemyTacticalPlanner.cs` to the deleted type were already removed, so **the project compiles with 0 errors** — only the orphaned prefab slots remain.
- **Historical symptom (before the fix):** Play Mode printed `The referenced script (Unknown) on this Behaviour is missing!` for spawned enemies — measured **7 warnings on one `Bandit`**. This was the source of `RT-06`. **Now 0** (Phase 5R).
- **What the component was for (Phase 5A investigation):** `EnemyTacticalEnvironmentScanner_TEST.cs` (435 lines, GUID confirmed as `809b48f6d9f34f34d90ca85975a9c332`) was a diagnostic obstacle-scanner exposing `ScanNow()`, `TryGetBestRoute(out RouteCandidate)`, `IsDirectionClear(Vector3, float)`, with `showDebug = true` and `OnDrawGizmos()`. Recovered from git history (`f9f4899`) and the 2026-09-22 `Z:` backup.
- **Was its functionality replaced?** **Yes — absorbed into `EnemyController.cs`** as first-class fields, not via a component:

  | Deleted scanner field | `EnemyController` equivalent |
  |---|---|
  | `bodyRadius = 0.35` | `obstacleProbeRadius = 0.35` — **identical** |
  | `routeProbeDistance = 2.5` | `obstacleRouteProbeDistance = 2.2` |
  | `sideProbeAngle = 70` | `obstacleSideProbeDistance = 1.6` |
  | — | `obstacleStuckTime`, `obstacleRouteCommitTime`, `obstacleEscapeDistance`, `obstacleRouteActive` (newer, developed system) |

  `TryGetBestRoute`, `IsDirectionClear`, `RouteCandidate`, `ObstacleInfo` and `visibleObstacles` have **0 references** anywhere in current code. No architectural replacement is needed — the capability lives in `EnemyController` directly.
- **No state was lost:** all 16 serialized fields were still present in the prefabs and were readable.
- **Decision: REMOVE obsolete reference.**
- **Fix applied (2026-09-29):** the orphaned component block (28 lines) and its `m_Component` entry were removed from all 4 prefabs — `Bandit`, `Rusher`, `Shooter`, `Tactical`. Each prefab lost exactly 29 lines; component count 10 → 9.
- **Static verification:** GUID occurrences 4 → **0**; `EnemyTacticalEnvironmentScanner` name **0**; orphaned `m_Component` references **0**; `EnemyHealth`, `EnemyTacticalPlanner`, `EnemyController` all intact; diff is **116 deletions, 0 insertions** across 4 files; orphan-block count identical to `HEAD` (2 = the GameObject headers themselves) → **no regression**.
- **✅ RUNTIME VERIFIED (2026-09-29, Phase 5R).** Via the restored Unity MCP channel: Console cleared → Play Mode entered → 16 s wait for spawn cycles → **3 live `EnemyController` instances** → **446 console entries scanned, 0 `The referenced script (Unknown) on this Behaviour is missing!`** → Play Mode exited. `find_gameobjects` for `EnemyTacticalEnvironmentScanner` returns **0 objects**. No regressions: enemies spawned and pathed normally. Full evidence: `../PHASE5R_UNITY_RUNTIME_CHANNEL_REPORT.md`.
- **State:** **CLOSED — RUNTIME VERIFIED.**

### UNI-D06 — Modular FBX imported at 0.01× scale
- **Observed:** modular environment FBX import scale resolves to `0.01×`; Z-up is not compensated.
- **Impact:** imported modular geometry is effectively invisible (1 cm scale). Any attempt to integrate the modular kit will place geometry at the wrong scale and orientation.
- **State:** unfixed. Import settings are wrong; this is a source-side `.meta`/importer problem.

### UNI-D07 — Android touch controls — ⚠️ RECLASSIFIED in Phase 5A
- **Previous claim:** "`MobileTouchControls` object references are all `null`" — critical for Android.
- **Phase 5A finding:** the 3 null fields are **self-healed at runtime** in `MobileTouchControls.cs` (`Start()`):
  - `playerController` → `FindFirstObjectByType<PlayerController>()`
  - `gunController` → `FindFirstObjectByType<GunController>()`
  - `playerCamera` → `Camera.main`

  Additional `playerController == null` guards exist at usage sites. **Therefore the nulls are not a functional break on PC** — they are unassigned serialized fields with runtime fallback.
- **Latent risks that remain real:**
  1. **Instance ambiguity — ✅ RESOLVED (Phase 5B.1).** `FindFirstObjectByType<GunController>()` returned the *first* match; the duplicate `FireButton`/`GunController` could bind the wrong weapon on device. All `FindFirstObjectByType<GunController>()` lookups were removed and replaced with a deterministic `PlayerController → GetComponent<GunController>()` resolution.
  2. **Initialisation order** — if the Player or its `GunController` does not exist yet when `Start()` runs, the lookup returns `null` and there is no retry.
  3. `Camera.main` requires a camera tagged `MainCamera`.
- **Android status: NOT VERIFIED.** No physical Android device is available in this environment. Per §12, **PC PASS ≠ Android PASS**; this remains an open Android blocker until a device run confirms touch input.
- **Suppressing the nulls in code is explicitly rejected** per the task rules — the fallback already exists; adding null-guards would hide the ambiguity rather than fix it.
- **State:** reclassified; not a code defect to patch. **Android validation OPEN.**

---

## High

### UNI-D08 — `Shooter.prefab` has a font material as its surface
- **Observed:** material `TMP_SDF-HDRP LIT` (a TextMeshPro font material) applied to the mesh renderer.
- **Impact:** the Shooter renders as a font atlas surface, not a character texture.
- **State:** unfixed.

### UNI-D09 — `Rusher.prefab` has a debug material
- **Observed:** material `FrameDebuggerRenderTargetDisplay` applied.
- **Impact:** the Rusher renders with a frame-debugger display material. Indicates a material that was never replaced before the prefab was saved.
- **State:** unfixed.

### UNI-D10 — Gun damage overwritten to 200 — ✅ FIXED, RUNTIME VERIFIED (Phase 5B)

- **Was:** `Awake()` contained `damage = Mathf.Max(damage, 200f)` — a hard-coded **floor** that silently discarded every authored value below 200.
- **Why it was a defect:** no design documentation referenced it, and a search of all 42 scripts found the literal `200` **nowhere else** in the codebase. It had zero dependents and no justification.
- **Fix:** the 6-line clamp was deleted from `Awake()`. The field default `= 200f` (line 14) was deliberately **left unchanged** — that is authored configuration for newly added components, not a defect.
- **Runtime evidence (2026-09-29, Phase 5B, via Unity MCP):**

  | Instance | Authored | Runtime **before** | Runtime **after** |
  |---|---|---|---|
  | `Player` | 100 | 200 | **100** |
  | `FireButton` | 10 | 200 | **10** |

  - Upgrade matrix confirmed: 100 → ×1.20 → **120** → **144** → **172.8** (cumulative, multiplicative); a `0` multiplier is correctly ignored.
  - Firing confirmed: bullets spawned continuously carrying `damage = 10`; after 4 s of fire an enemy died (3 → 2). `10 damage × 10 hits = 100 = EnemyHealth.maxHealth`.
  - **0 project-origin errors in 500 console entries.**
- **✅ Subsequent defect found and now also fixed (Phase 5B.1, `ISSUE-22` / `RT-03`):** removing the clamp exposed that the weapon that *fires* and the object that receives *upgrades* were **different `GunController` instances** (`FindFirstObjectByType` → `FireButton`; `UpgradeManager` → `Player`). Unified onto one canonical `Player` instance (runtime-verified `70770 == 70770`); the `FireButton` duplicate was removed.
- **Balance consequence (accepted, needs a decision):** after unification the firing weapon uses its authored `100` damage. The final gameplay value is `DEC-11` — **still OPEN**, deliberately not decided in Phase 5B.1.
- **State:** **✅ FIXED — RUNTIME VERIFIED.**
### UNI-D11 — Zero modular environment meshes integrated
- **Observed:** 0 modular meshes in the scene despite the kit existing in `Assets/Art/Environment/Modular/`.
- **Impact:** the environment kit is unusable. Interacts with `UNI-D06` — integration cannot proceed until import is fixed.
- **State:** unfixed, blocked by `UNI-D06`.

---

## Medium

### UNI-D12 — Zero destructible instances
- **Observed:** 0 destructible objects in the scene, although controlled-fracture assets exist in `Assets/Art/Props/`.
- **Impact:** a designed feature is entirely absent from the playable build.
- **State:** unfixed.

### UNI-D13 — HUD level/XP fields are null
- **Observed:** `HUDController.xpBar` and `HUDController.levelText` are null at runtime; the level is never displayed.
- **Static evidence:** `Assets/Scripts/UI/HUDController.cs` lines 12–13 declare both as `[SerializeField]`, and both are unassigned in the scene.
- **Impact:** the level/XP readout is permanently absent. Also means the XP system has **no UI surface**, reinforcing that XP is unimplemented (`GAME-0004` §3).
- **State:** unfixed. Runtime audit ref `RT-07`.
- **Note:** this is a *separate* null reference from the 12 in `UNI-D04`; it was not counted in that total.

### UNI-D14 — Android performance unproven
- **Observed:** 4097 renderers across 1552 objects at runtime.
- **Impact:** Android frame rate, memory and thermal behaviour are **unknown**. No device test has been performed (`ISSUE-11`, `GAME-0003` §3).
- **State:** unfixed / unverified.
- **Constraint:** this must be measured on a physical device. It cannot be closed on desktop editor numbers.

---

## 1.1 Runtime-audit ref mapping (`RT-*` → `UNI-D*`)

The U-12 runtime audit numbered issues `RT-01`…`RT-09`. Those identifiers are
retained for traceability to `AUDIT_2_RUNTIME_REPORT.md`.

| Audit ref | Defect | Canonical ID |
|---|---|---|
| `RT-01` | Modular FBX import 0.01×, Z-up uncompensated | `UNI-D06` |
| `RT-02` | `MobileTouchControls` refs null (Android-critical) | `UNI-D07` |
| `RT-03` | Duplicate `GunController`; firing/upgrades on **different instances** — **✅ FIXED, RUNTIME VERIFIED 2026-09-29** | `ISSUE-22` **CLOSED** | Phase 5B.1: one canonical `Player#70770`; `FireButton` duplicate removed |
| `RT-04` | `GunController.Awake` forced `damage ≥ 200` — **✅ FIXED, RUNTIME VERIFIED 2026-09-29** | `UNI-D10` **CLOSED** | Phase 5B: Player 200→100, FireButton 200→10 |
| `RT-05` | `Rusher`/`Shooter` prefab materials wrong | `UNI-D08`, `UNI-D09` |
| `RT-06` | 9 missing-script warnings per spawn cycle — **RUNTIME VERIFIED: 0 warnings** (Phase 5R) | `UNI-D05` **CLOSED** |
| `RT-07` | `HUDController.xpBar` / `levelText` null | **`UNI-D13`** |
| `RT-08` | 12 broken references | `UNI-D04` |
| `RT-09` | 4097 renderers / 1552 objects, Android perf unproven | **`UNI-D14`** |

> `UNI-D13` and `UNI-D14` were **added in Phase 3** because the Phase 2 register
> omitted `RT-07` and `RT-09`. The gap is closed; all nine audit refs are now mapped.

---

## Untested — must not be reported as working

These systems were **not** exercised at runtime and must not be described as
functional in any document:

- AI combat behaviour
- AI investigation behaviour
- AI sound propagation / reaction
- AI cover-taking
- AI flanking

---

## Repair ordering guidance

Blocking dependencies, in order:

```
UNI-D05 (missing scripts)  ─┐
UNI-D04 (broken refs)      ─┴─→ restore logic ─→ UNI-D02/D03 (character) ─→ UNI-D01 (NavMesh)
UNI-D06 (import scale)     ───→ UNI-D11 (modular integration)
UNI-D07 (touch)            ───→ Android target
UNI-D10 (damage)           ───→ weapon balance
UNI-D08/D09 (materials)    ───→ visual correctness
```

`UNI-D04` and `UNI-D05` should be enumerated first, because both silently
disable behaviour and may account for other apparent defects.

---

**End of `UNI-0003-UNITY-KNOWN-DEFECTS.md`**
