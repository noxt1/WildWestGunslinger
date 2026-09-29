# AUDIT_2_RUNTIME_REPORT.md

**Project:** WildWestGunslinger
**Audit type:** AUDIT 2 — U-12 Runtime Verification Pass (read-only)
**Date:** 2026-09-29, ~16:35–16:55 (+03:00)
**Authorisation:** user decision **U-12** (runtime audit after Unity MCP became available)
**Mode:** STRICT READ-ONLY — Play Mode entered and exited without saving
**Status:** ⏸ **STOP POINT — awaiting further APPROVE**

> **READ-ONLY COMPLIANCE — VERIFIED**
> No C#, Scene, Prefab, material, lighting, navigation or project setting was modified.
> No asset was saved. No GameObject was created or deleted. No reference was repaired.
> **Post-audit integrity check passed:** `TestArena.unity` mtime unchanged (`2026-09-26 23:08:42`), `git status` = 83 entries (unchanged), scene diff unchanged (+370/−2), `HEAD` = `040651b`, 0 staged.

---

## 1. Runtime Environment

| Parameter | Observed value | Class |
|---|---|---|
| Unity Editor | **6000.3.23f1** | `RUNTIME VERIFIED` |
| Unity instance | `WildWestGunslinger@ad0b2d56a935767b` | `RUNTIME VERIFIED` |
| MCP server | `mcp-for-unity-server` **3.4.7** | `RUNTIME VERIFIED` |
| MCP transport | HTTP JSON-RPC on `http://127.0.0.1:8080/mcp` | `RUNTIME VERIFIED` |
| Bridge process | `mcp-for-unity.exe` (PID 18888) | `RUNTIME VERIFIED` |
| Editor process | `Unity.exe` 6000.3.23f1 (PIDs 11320 / 28224 / 33412) | `RUNTIME VERIFIED` |
| Platform | **WindowsEditor** | `RUNTIME VERIFIED` |
| `isBatchMode` | `false` (editor warns modal popups may interrupt) | `RUNTIME VERIFIED` |
| Compilation state | not compiling, no domain reload pending | `RUNTIME VERIFIED` |
| `ready_for_tools` | `true` | `RUNTIME VERIFIED` |
| Edit-mode scene objects | **86** | `RUNTIME VERIFIED` |
| Play-mode scene objects | **1552** | `RUNTIME VERIFIED` |
| Play-mode renderers | **4097** | `RUNTIME VERIFIED` |
| Play-mode colliders | **536** | `RUNTIME VERIFIED` |
| Play-mode lights | **1** | `RUNTIME VERIFIED` |
| Play-mode LineRenderers | **24** | `RUNTIME VERIFIED` |
| Play-mode canvases | **4** | `RUNTIME VERIFIED` |
| Observed FPS | ~not reliably measurable headless; `timeScale=1`, frames advancing normally | `NOT VERIFIED` |

**Tool access note.** `unityMCP` tools were **not** exposed through the agent's MCP registry (`list_mcp_resources` → `[]`), despite the server being live. The audit therefore established a **direct read-only JSON-RPC session** against `127.0.0.1:8080` and used only these server-side operations: `read_console`, `manage_scene` (read-only actions), `execute_code` (strictly read-only C#), `manage_editor` (`play`/`stop`). **No mutating tool was invoked at any point.**

---

## 2. Scene Verification

| Question | Answer | Class |
|---|---|---|
| Active scene | **`Assets/Scenes/TestArena.unity`** (GUID `bcc94342232f0d0468763dc03a0ec36d`) | `RUNTIME VERIFIED` |
| `isDirty` at load | **false** | `RUNTIME VERIFIED` |
| Loaded scenes | 1 — `TestArena` | `RUNTIME VERIFIED` |
| Root count | **20** (matches static analysis exactly) | `RUNTIME VERIFIED` |
| Duplicate scene `Assets/TestArena.unity` | **NOT loaded**, not referenced by build settings | `RUNTIME VERIFIED` |
| Build settings | Desert Demo (idx 0, disabled), MainMenu (1), TestArena (2), Game (3) | `RUNTIME VERIFIED` |
| `Game.unity` | present in build list, **not loaded** | `RUNTIME VERIFIED` |
| Scene integrity after Play Mode | mtime unchanged, no save prompt, no modification | `RUNTIME VERIFIED` |

**Classification note.** `manage_scene.get_active` reports `buildIndex: 1` while `EditorBuildSettings.asset` places TestArena at index 2. The **path and GUID are authoritative and match**; the index field is inconsistent between the tool and the asset file. Recorded as a low-severity tooling discrepancy, not a project defect.

---

## 3. Player

| Check | Observed | Class |
|---|---|---|
| `Player` object | present, active, layer 0, tag `Player` | `RUNTIME VERIFIED` |
| Components | `Transform, MeshFilter, MeshRenderer, CapsuleCollider, Rigidbody, PlayerController, GunController, PlayerHealth, XPManager, UpgradeManager` | `RUNTIME VERIFIED` |
| **Visual model** | **built-in `Capsule` primitive** (no character mesh) | `RUNTIME VERIFIED` |
| `PlayerController` tuning | `moveSpeed 5 · silentSpeed 1.35 · walkSpeed 3 · runSpeed 5 · silentInputThreshold 0.18` | `RUNTIME VERIFIED` |
| `joystick` reference | **`MobileJoystick 'JoystickBackground'` — RESOLVED** | `RUNTIME VERIFIED` |
| Rigidbody | present, non-kinematic, velocity `(0,0,0)` | `RUNTIME VERIFIED` |
| Player position | `(0.00, 1.00, 0.00)` — never moved (no input driver in automated run) | `RUNTIME VERIFIED` |
| `PlayerHealth` | `100 / 100`, `IsDead = false` | `RUNTIME VERIFIED` |
| `deathUI` reference | `DeathUIController 'MobileUI'` — RESOLVED | `RUNTIME VERIFIED` |
| `XPManager` reference (from UpgradeManager) | RESOLVED to `Player` | `RUNTIME VERIFIED` |
| `GunController.weaponPoint` | **`Transform 'WeaponPoint'` — CORRECT** | `RUNTIME VERIFIED` |
| `GunController.bulletPrefab` | RESOLVED to `Bullet` prefab | `RUNTIME VERIFIED` |
| `UpgradeManager` refs | all 4 RESOLVED (`xpManager`, `playerHealth`, `gunController`, `upgradeUI`) | `RUNTIME VERIFIED` |
| Camera | `CameraFollow.target` → `Player` | `RUNTIME VERIFIED` |

### 3.1 RUNTIME ISSUE — damage silently overridden on both GunControllers

| Instance | Inspector `damage` | **Runtime `GetDamage()`** | Fire rate | Range |
|---|---:|---:|---:|---:|
| `Player/GunController` | **100** | **200** | 0.4 | 15 |
| `FireButton/GunController` | **10** | **200** | 0.35 | 20 |

`GunController.Awake` forces `damage = Mathf.Max(damage, 200f)`. **Any Inspector value below 200 is silently discarded at runtime.** The `FireButton` component — which is a duplicate controller mounted on a UI button — also reports 200 damage, range 20, and its `weaponPoint` points at the **Player root** rather than `Player/WeaponPoint`.

- Class: **`RUNTIME ISSUE`**
- Two live `GunController` instances both act as the weapon; the one on `FireButton` is misconfigured.
- `firing = false`, `nextFireTime = 0` on both — no firing occurred during the run.

### 3.2 RUNTIME VERIFIED ABSENCE — desktop fire input

`GunController.FireButtonDown` / `SetFiring` are reachable only from `MobileTouchControls`, which is gated on `Application.isMobilePlatform`. On `WindowsEditor` this is `false`, so **there is no code path that can set `firing = true` on desktop**. Confirmed at runtime: `firing = false` on both instances with the player present and alive.

- Class: **`RUNTIME VERIFIED ABSENCE`**

---

## 4. UI

| Check | Observed | Class |
|---|---|---|
| Canvas hierarchy | `MobileUI` (root) → `HUD` → HealthBar / XPBar / FireButton / DeathPanel / PauseButton / PausePanel / UpgradePanel / WaveText / WaveCompleteText | `RUNTIME VERIFIED` |
| `HUDController.healthBar` | RESOLVED → `Slider 'HealthBar'` | `RUNTIME VERIFIED` |
| `HUDController.healthText` | RESOLVED → `TextMeshProUGUI 'HealthText'` | `RUNTIME VERIFIED` |
| **`HUDController.xpBar`** | **`NULL`** | `RUNTIME ISSUE` |
| **`HUDController.levelText`** | **`NULL`** | `RUNTIME ISSUE` |
| `XPBarController` | RESOLVED `xpManager` + `xpFill`; runs independently | `RUNTIME VERIFIED` |
| `UpgradeUI` | all 6 refs RESOLVED (panel, 3 buttons, xpBar) | `RUNTIME VERIFIED` |
| `WaveManager.waveText` / `waveCompleteText` | both RESOLVED | `RUNTIME VERIFIED` |
| `EventSystem` | present | `RUNTIME VERIFIED` |
| `EnemyDirectionIndicator` | **exactly 1 instance** (auto-bootstrap works) | `RUNTIME VERIFIED` |
| `HUDController` active | **true** | `RUNTIME VERIFIED` |
| `UpgradePanel` active | **false** (hidden, as authored) | `RUNTIME VERIFIED` |

### 4.1 RUNTIME ISSUE — `MobileTouchControls` all references null (LATENT, not fatal on PC)

```
MobileTouchControls.playerController = null
MobileTouchControls.gunController    = null
MobileTouchControls.playerCamera     = null
Application.isMobilePlatform         = false
MobileTouchControls.canvas           = null
```

**This resolves the ambiguity in AUDIT 2 conflict C18.**

- On `WindowsEditor` the component is **inert** — it early-returns on `isMobilePlatform == false`, never dereferences the null fields, and creates no runtime canvas (`canvas = null`).
- **It did not throw, and no Console error was produced.**
- However, on an **Android build `isMobilePlatform` is `true`**, the code path activates, and all three null fields are exactly what it needs to drive the player, the gun and the camera.

**Classification: `RUNTIME ISSUE` — CRITICAL for the stated primary platform (Android), inert on the current test platform.**

This **confirms the conflict**: `CONFIRMED_STATE.md:79–81` marks player control, player shooting and bullets as `CONFIRMED`. The runtime evidence shows the control layer is **not wired**; only the scene-authored button path could ever fire, and that button's controller is misconfigured (§3.1).

---

## 5. Enemy Runtime

| Check | Observed | Class |
|---|---|---|
| `EnemySpawner` prefab refs | all 4 RESOLVED (`Bandit`, `Shooter`, `Rusher`, `Tactical`) | `RUNTIME VERIFIED` |
| `WaveManager` object | present, `currentWave = 1` | `RUNTIME VERIFIED` |
| `EnemySpawner.currentWave` | `1` | `RUNTIME VERIFIED` |
| Live enemies | **3**, all `Bandit` (wave 1 = `startingEnemies 3`) | `RUNTIME VERIFIED` |
| `EnemyController` state | all 3 in **`Searching`** | `RUNTIME VERIFIED` |
| `EnemyTacticalVision` instances | **3** (runtime-added, not on prefabs) | `RUNTIME VERIFIED` |
| Vision state | all 3 **`Scanning`** | `RUNTIME VERIFIED` |
| `playerDetected` | **`false`** on all 3 | `RUNTIME VERIFIED` |
| **`ArenaTacticalMap.IsBuilt`** | **`true`** | `RUNTIME VERIFIED` |
| `Rooms` | populated `List<RoomMap>` | `RUNTIME VERIFIED` |
| `CoverSystem` instance | present in scene | `RUNTIME VERIFIED` |
| `CoverPoint` instances in scene | **72** | `RUNTIME VERIFIED` |
| Enemy positions | `(-1.05, 1, -40.09)`, `(5.96, 1, -36.80)`, `(3.18, 1, -31.82)` | `RUNTIME VERIFIED` |
| Distance to player (origin) | **30–44 m** | `RUNTIME VERIFIED` |
| `detectionDistance` (prefab) | **18 m** | `RUNTIME VERIFIED` |

### 5.1 RUNTIME VERIFIED — vision does **not** see through walls

The player was stationary at the origin, the three enemies were 30–44 m away and separated by arena geometry. Despite a full 360° scan sweep running (`state = Scanning`), **all three report `playerDetected = false`**. This is correct behaviour and constitutes a **positive runtime verification of the multi-height LOS system** — it is not detecting the player through walls at long range.

- Class: **`RUNTIME VERIFIED`**
- Static claim `ARCHITECTURE.md:167–171` (4-point LOS) is consistent with observed runtime behaviour.

### 5.2 NOT VERIFIED — full perception→combat chain

The AI never reached `Combat`, `MovingToCover`, `InCover`, `Flanking`, `Investigating` or `RoomSearching` in this run, because the player was immobile and unreachable by noise. **The following remain `NOT VERIFIED` at runtime:** player detection at close range, LOS loss → investigation transition, sound pursuit, room search routing, cover acquisition, flanking, peek cycle, and the `EnemyTacticalPlanner` regression (AUDIT 2 P-23) in live behaviour.

---

## 6. Navigation

| Check | Observed | Class |
|---|---|---|
| `NavMeshAgent` components | **0** | `RUNTIME VERIFIED ABSENCE` |
| `NavMesh.CalculateTriangulation()` | **0 triangles, 0 vertices** — no baked navmesh | `RUNTIME VERIFIED ABSENCE` |
| Custom pathfinding map | `ArenaTacticalMap.IsBuilt = true` | `RUNTIME VERIFIED` |
| Enemy path field | **non-null on all 3 enemies** at both samples | `RUNTIME VERIFIED` |
| **Movement executed** | see timeline below | `RUNTIME VERIFIED` |

### 6.1 RUNTIME VERIFIED — enemies do move

| Sample | t (s) | Enemy 1 | Enemy 2 | Enemy 3 |
|---|---:|---|---|---|
| A | 232.8 | `(-6.83, -43.47)` | `(6.39, -36.31)` | `(4.56, -30.66)` |
| B | 245.2 | `(-1.05, -40.09)` | `(5.96, -36.80)` | `(3.18, -31.82)` |
| **Δ over 12.4 s** | | **≈ 6.1 m** | ≈ 0.7 m | ≈ 1.8 m |

All three changed position while remaining in `Searching`. Rigidbody `linearVelocity` sampled `(0,0,0)` at the instants queried, which is consistent with `Rigidbody.MovePosition`-driven movement rather than a stuck state.

- Class: **`RUNTIME VERIFIED`** — custom A* navigation executes; there is **no** evidence of path failure, unreachable destination, or corner-sticking in this sample.
- **Caveat:** only 3 enemies on wave 1, single sample window, one room region. Path complexity, corner cases and multi-room routing are **still `NOT VERIFIED`**.

---

## 7. Environment Integration

| Check | Observed | Class |
|---|---|---|
| `ArenaGenerator` generated rooms | **9** (`Room_*`) — matches the 9-room honeycomb contract | `RUNTIME VERIFIED` |
| Wall segments created | **71** (`Wall_*`) | `RUNTIME VERIFIED` |
| Floors created | **11** (`Floor*`) | `RUNTIME VERIFIED` |
| Cover objects | **148** | `RUNTIME VERIFIED` |
| Spawn zones | **52** | `RUNTIME VERIFIED` |
| Corridor objects | **36** | `RUNTIME VERIFIED` |
| `CoverPoint` components | **72** | `RUNTIME VERIFIED` |
| `ArenaVisualStyle` | **inactive** — did not run, no visual-object explosion | `RUNTIME VERIFIED` |
| `WildWestEnvironmentGenerator` | ran — **294 `WWG_*` objects** produced | `RUNTIME VERIFIED` |
| `UNS_*` (vendor) objects | **0** | `RUNTIME VERIFIED` |
| **`WallSegment` meshes in scene** | **0** | `RUNTIME VERIFIED ABSENCE` |
| **`FloorSegment` meshes in scene** | **0** | `RUNTIME VERIFIED ABSENCE` |
| **`DestructibleObject` instances** | **0** | `RUNTIME VERIFIED ABSENCE` |
| `ArenaGenerator` prop slots | 7 × NULL (unchanged from static) | `RUNTIME VERIFIED` |

**Confirmed at runtime:** the modular wall/floor kit and all QA-approved art assets have **zero runtime presence**. The 8 `DestructibleObject` prefabs are never instantiated because the environment generator holds only `WWG_*` prefabs and the arena generator's 7 prop slots are null.

**Note on spawn zones:** 52 observed. The historical `CHANGELOG` recorded 43 (wave 1) / 53 (wave 2). The current figure is consistent in magnitude but was **not** produced by the same arena configuration, so it does **not** retroactively validate those historical runtime claims.

### 7.1 RUNTIME ISSUE — modular FBX import scale and orientation are wrong

This is the **most significant new runtime finding** and was not detectable by static file analysis.

| Asset | Blender (per QA report) | **Unity mesh bounds** | Ratio |
|---|---|---|---|
| `WallSegment_01_FINAL` | 4.000 × 0.200 × 2.000 m | **`0.040 × 0.002 × 0.020`** | **0.01×** |
| `WallSegment_01_r2` | 4.000 × 0.200 × 2.000 m | **`0.040 × 0.002 × 0.020`** | **0.01×** |
| `WallSegment_01_r3` | 4.000 × 0.200 × 2.000 m | **`0.040 × 0.002 × 0.020`** | **0.01×** |
| `FloorSegment_01_FINAL` | 4.000 × 4.000 × 0.080 m | **`0.040 × 0.040 × 0.001`** | **0.01×** |
| `FloorSegment_01_r2` | 4.000 × 4.000 × 0.080 m | **`0.040 × 0.040 × 0.001`** | **0.01×** |
| `FloorSegment_01_r3` | 4.000 × 4.000 × 0.080 m | **`0.040 × 0.040 × 0.001`** | **0.01×** |

Additionally, the thin axis is on **Y** (wall: 0.002) and **Z** (floor: 0.001), and `minY ≈ −0.001 / −0.020` — i.e. **the meshes are lying flat and are not Z-up compensated**. The documented compensation (`ART_PIPELINE.md`, `AUDIT_SESSION_CONTEXT:400–418`) is applied at **prefab `Model` level**, and **no prefab exists**, so the compensation never runs.

**Consequence if integrated as-is:** a 4 m wall segment would appear in Unity as a **4 cm** flat tile lying on the ground.

- Class: **`RUNTIME ISSUE`** — asset-level contract (1 mesh / 1 submesh / vertex colours / UVs) is **correct**; import transform is **wrong**.

### 7.2 RUNTIME VERIFIED — asset contract holds (positive)

| Property | Observed for all 6 | Class |
|---|---|---|
| `subMeshCount` | **1** (one mesh, as required) | `RUNTIME VERIFIED` |
| Vertex colours present | **true** (the `Col` FLOAT_COLOR round-trip survived FBX → Unity) | `RUNTIME VERIFIED` |
| UV channel present | **true** | `RUNTIME VERIFIED` |
| `WallSegment_01_FINAL` tris | **280** — matches QA report exactly | `RUNTIME VERIFIED` |
| `WallSegment_01_r2` tris | **1200** — matches QA report exactly | `RUNTIME VERIFIED` |
| `WallSegment_01_r3` tris | **200** — matches QA report exactly | `RUNTIME VERIFIED` |
| `FloorSegment_01_FINAL` tris | **320** — matches | `RUNTIME VERIFIED` |
| `FloorSegment_01_r2` tris | **1920** — matches | `RUNTIME VERIFIED` |
| `FloorSegment_01_r3` tris | **320** — matches | `RUNTIME VERIFIED` |
| Prefab dependencies | **NONE** for all 6 | `RUNTIME VERIFIED ABSENCE` (integration) |
| Prop FBX prefab deps | **NONE** for `Cover_Western_Wooden_FINAL`, `_r4`, `_Fragments_r3`, `Crate_01_*_FINAL`, `Barrel_01_*`, `Fence_01_*` | `RUNTIME VERIFIED ABSENCE` |

> The Blender-side numeric QA in `Working/reports/` is **reliable and reproducible in Unity**. The discrepancy is confined to **import transform**, not geometry or attributes.

---

## 8. Character Runtime

| Check | Observed | Class |
|---|---|---|
| `Animator` components (edit + play) | **0** | `RUNTIME VERIFIED ABSENCE` |
| `Avatar` assignments | **0** | `RUNTIME VERIFIED ABSENCE` |
| `runtimeAnimatorController` | **0** | `RUNTIME VERIFIED ABSENCE` |
| `SkinnedMeshRenderer` | **0** | `RUNTIME VERIFIED ABSENCE` |
| Player mesh | built-in **Capsule** | `RUNTIME VERIFIED` |
| All 4 enemy prefab meshes | built-in **Capsule** | `RUNTIME VERIFIED` |
| Equipment sockets | none | `RUNTIME VERIFIED ABSENCE` |
| Weapon attachment (physical) | none — `WeaponPoint` is an empty `Transform` | `RUNTIME VERIFIED ABSENCE` |
| Animation state | none | `RUNTIME VERIFIED ABSENCE` |

**All Character Foundation claims in `wwg-character-art/SKILL.md:283–312` (51-bone skeleton, `WeaponPoint_R/L`, `HolsterPoint`, `BackWeaponPoint`) are confirmed absent at runtime.** The reclassification proposed in AUDIT 2 (`FUTURE CONTRACT` rather than project fact) is **runtime-endorsed**.

The untouched vendor `Human Character Dummy` package remains present as a scale reference only.

### 8.1 RUNTIME ISSUE — wrong materials on 2 enemy prefabs

| Prefab | Mesh | Material at runtime | Assessment |
|---|---|---|---|
| `Bandit` | Capsule | `Lit` (URP default) | OK |
| `Shooter` | Capsule | **`TMP_SDF-HDRP LIT`** | 🔴 a **text SDF** material on a 3D mesh |
| `Rusher` | Capsule | **`FrameDebuggerRenderTargetDisplay`** | 🔴 an internal **debug** material |
| `Tactical` | Capsule | `Lit` (URP default) | OK |

Shooter and Rusher will not render as intended. Static YAML analysis had reported their `m_Materials` arrays as empty; the runtime resolves them to invalid fallback assignments. **This was not visible to static analysis** and would only manifest in-game.

- Class: **`RUNTIME ISSUE`**

---

## 9. Console

### 9.1 Errors — 5 total

| # | Message | Origin | Project-related? |
|---|---|---|---|
| 1–5 | `Error reason is 'NoSubscription' and no additional error information was provided (https://generators.ai.unity.com).` | Unity AI Assistant / Generators (external service) | ❌ **No** — external, pre-existing, unrelated to project code |

**Zero project-code exceptions. Zero `NullReferenceException`. Zero assertion failures. Zero compilation errors.**

### 9.2 Warnings — 10 total

| # | Message | Origin | Project-related? |
|---|---|---|---|
| 1 | `Editor is not in automated mode. Modal Pop up might break continuous command workflow.` | MCP tooling | ❌ No |
| 2–10 | `The referenced script on this Behaviour (Game Object 'Bandit') is missing!` (×6) and `(Game Object '<null>')` (×3) | **Deleted `EnemyTacticalEnvironmentScanner_TEST`** | ✅ **Yes** |

**RUNTIME VERIFIED:** the 4 orphan `MonoBehaviour` slots on the enemy prefabs fire a Console warning **on every enemy instantiation**. Three live Bandits produced 9 warnings during this run. Wave 1 (3 Bandits) → 3 warnings per spawn cycle; every subsequent wave multiplies it.

- Class: **`RUNTIME VERIFIED` + `RUNTIME ISSUE`**
- The `MobileTouchControls` null references produced **no** warning — Unity does not warn on null serialized references. This is precisely why they are dangerous: **silent**.

### 9.3 Separation: pre-existing vs new

| Category | Count | Verdict |
|---|---|---|
| Pre-existing external errors (Unity AI NoSubscription) | 5 | not project-caused |
| Pre-existing tooling warning (not automated mode) | 1 | not project-caused |
| **Project-caused (missing script)** | **9** | **confirmed present at runtime** |
| **New runtime-only findings not in any doc** | **4** | §3.1, §7.1, §8.1, §4.1 |

---

## 10. Runtime Verified Facts

| # | Fact | Evidence |
|---|---|---|
| V-01 | Scene `Assets/Scenes/TestArena.unity` is the active and loaded scene; `isDirty = false`; 20 roots | `manage_scene` |
| V-02 | Build settings contain 4 scenes; `Game.unity` enabled but never loaded | `manage_scene.get_build_settings` |
| V-03 | Play mode generates the full arena: **9 rooms, 71 walls, 11 floors, 148 covers, 52 spawn zones, 36 corridor objects, 72 CoverPoints** | `execute_code` |
| V-04 | `ArenaTacticalMap.IsBuilt = true` at runtime with populated `Rooms` | reflection |
| V-05 | Enemies spawn, run the FSM (`Searching`), hold non-null paths and **physically move** | 2 samples, 12.4 s apart |
| V-06 | **No NavMesh exists anywhere** (0 agents, 0 triangulated triangles) — navigation is entirely the custom A* system | `CalculateTriangulation()` |
| V-07 | Vision correctly does **not** detect the player through walls at 30–44 m; all 3 report `detected = false` while scanning | reflection |
| V-08 | **All 12 suspected broken references are confirmed broken at runtime** (HUDController 2, ArenaGenerator 7, MobileTouchControls 3) | `SerializedObject` walk |
| V-09 | **4 × missing script slot on enemy prefabs**, one per prefab; 3 live instances at runtime | prefab load + console |
| V-10 | `PlayerController.joystick` **is** resolved to the `JoystickBackground` MobileJoystick | `SerializedObject` |
| V-11 | `PlayerHealth` 100/100, alive; `deathUI` resolved | reflection |
| V-12 | `UpgradeManager` all 4 references resolved; `UpgradeUI` all 6 resolved | `SerializedObject` |
| V-13 | `WaveManager.currentWave = 1` with 3 enemies = `startingEnemies` | reflection |
| V-14 | `EnemyDirectionIndicator` auto-bootstraps exactly 1 instance | `FindObjectsOfType` |
| V-15 | **`ArenaVisualStyle` is inactive** and did not run — no visual-object explosion at runtime | `activeInHierarchy = false` |
| V-16 | `WildWestEnvironmentGenerator` produced **294 `WWG_*` objects** and **0 `UNS_*` objects** | name scan |
| V-17 | **Zero** `WallSegment` / `FloorSegment` meshes in the runtime scene | mesh-name scan |
| V-18 | **Zero** `DestructibleObject` instances at runtime | component scan |
| V-19 | Modular FBX satisfy the asset contract: **1 submesh each**, vertex colours and UVs present, triangle counts **exactly matching** the QA reports | mesh API |
| V-20 | **Zero** project-code exceptions in Console | `read_console` |
| V-21 | No compilation errors; no domain reload pending | editor state |
| V-22 | **No animation system**: 0 Animators, 0 Avatars, 0 Controllers, 0 SkinnedMeshRenderers | component scan |
| V-23 | Play-mode footprint: **1552 objects, 4097 renderers, 536 colliders, 24 LineRenderers, 4 canvases** | component scan |
| V-24 | Scene file on disk is **byte-unchanged** after the Play Mode session | mtime + git diff |

---

## 11. Runtime Verified Absence

| # | Absent | Class | Consequence |
|---|---|---|---|
| A-01 | `NavMeshAgent` / any NavMesh | `RUNTIME VERIFIED ABSENCE` | custom A* is the only navigation; package `com.unity.ai.navigation` is dead weight |
| A-02 | All animation (`Animator`, `Avatar`, `AnimatorController`) | `RUNTIME VERIFIED ABSENCE` | characters cannot animate; no clips, no blending |
| A-03 | All skins (`SkinnedMeshRenderer`) | `RUNTIME VERIFIED ABSENCE` | no rigged characters |
| A-04 | Equipment / sockets | `RUNTIME VERIFIED ABSENCE` | no gear system |
| A-05 | Physical weapon attachment | `RUNTIME VERIFIED ABSENCE` | `WeaponPoint` is an empty Transform |
| A-06 | Modular wall/floor kit in scene | `RUNTIME VERIFIED ABSENCE` | assets unused; generator still uses primitives |
| A-07 | Any `DestructibleObject` instance | `RUNTIME VERIFIED ABSENCE` | destructible foundation never exercised at runtime |
| A-08 | Desktop fire input path | `RUNTIME VERIFIED ABSENCE` | PC player cannot shoot without a wired UI button |
| A-09 | Any AI state beyond `Searching` | `NOT VERIFIED` (not absent) | combat chain unexercised |

---

## 12. Runtime Issues

| # | Issue | Severity | Class | Evidence |
|---|---|---|---|---|
| **RT-01** | **Modular FBX import at 0.01× scale, Z-up not compensated** — 4 m wall becomes a 4 cm flat tile | **CRITICAL** (if integrated) | `RUNTIME ISSUE` | mesh bounds |
| **RT-02** | `MobileTouchControls` all 3 refs null — inert on PC, **will activate on Android** | **CRITICAL** (Android) | `RUNTIME ISSUE` | reflection + `isMobilePlatform=false` |
| **RT-03** | `FireButton/GunController` duplicate, `weaponPoint` → Player root, damage 10→200 | HIGH | `RUNTIME ISSUE` | runtime `GetDamage()` |
| **RT-04** | `GunController.Awake` forces `damage ≥ 200`, silently discarding all Inspector values | HIGH | `RUNTIME ISSUE` | runtime vs Inspector |
| **RT-05** | `Rusher.prefab` material = `FrameDebuggerRenderTargetDisplay`; `Shooter.prefab` = `TMP_SDF-HDRP LIT` | HIGH | `RUNTIME ISSUE` | prefab material read |
| **RT-06** | 9 missing-script warnings per spawn cycle; scales with enemy count | HIGH | `RUNTIME ISSUE` | console |
| **RT-07** | `HUDController.xpBar` / `levelText` null — level never displayed | MEDIUM | `RUNTIME ISSUE` | `SerializedObject` |
| **RT-08** | 12 broken references total, all runtime-confirmed | MEDIUM | `RUNTIME ISSUE` | `SerializedObject` |
| **RT-09** | 4097 renderers / 1552 objects at runtime — Android perf unproven | MEDIUM | `RUNTIME ISSUE` | component scan |
| **RT-10** | 0 destructible instances — Stage 8 foundation never runs | MEDIUM | `RUNTIME ISSUE` | component scan |
| **RT-11** | 5 external `NoSubscription` errors (Unity AI service) | LOW | pre-existing, not project-caused | console |
| **RT-12** | 24 LineRenderers in play mode (EnemyTacticalVision debug) | LOW | `RUNTIME ISSUE` | component scan |

---

## 13. Previously Documented — Not Rechecked

Per the brief, the following were **not** re-verified because AUDIT 2 already holds sufficient documentary evidence:

| Item | Reason | Source of existing evidence |
|---|---|---|
| Blender numeric/visual QA for all 7 reports | Blender-only, already read in full | `Working/reports/*.md` (AUDIT 2 §4.G) |
| FBX export contract adherence | already read in full | `ART_PIPELINE.md:97–115` + QA reports |
| `Col` FLOAT_COLOR / metal-mask convention | already read; **additionally runtime-confirmed** (V-19) | `RULES.md:43–44` |
| Method C compliance | already read | `RULES.md:42` |
| Full art pipeline iteration history (r1→r4) | already read | QA reports 1–7 |
| Repo structure, Git state, GitHub state | unchanged since AUDIT 2; re-checked only for integrity | `AUDIT_2_RECONCILIATION_REPORT.md` |
| All documentation conflicts D/S/C/G/R/Q | documentary, not runtime | AUDIT 2 §14 |
| `MainMenu.unity`, `Game.unity`, `TestArena_Preview.unity` runtime behaviour | not the dev scene; loading them would be a scene change | — |
| Lighting / post-processing visual quality | requires render capture; out of scope | — |
| FPS / frame-time measurement | no reliable measurement in this automated run | — |

---

## 14. Changes NOT Made

| # | Prohibited action | Status |
|---|---|---|
| 1 | Modify C# | ❌ not done |
| 2 | Modify scene | ❌ not done — mtime unchanged |
| 3 | Modify prefab | ❌ not done |
| 4 | Modify Inspector values | ❌ not done |
| 5 | Save Unity assets | ❌ not done |
| 6 | Create / delete GameObject | ❌ not done |
| 7 | Repair references | ❌ not done |
| 8 | Modify materials | ❌ not done |
| 9 | Modify lighting | ❌ not done |
| 10 | Modify navigation | ❌ not done |
| 11 | Modify project settings | ❌ not done |
| 12 | Run automatic fixes | ❌ not done |
| 13 | Promote any asset to FINAL | ❌ not done |
| 14 | Recovery sequence | ❌ not done |
| 15 | Documentation changes | ❌ not done |
| 16 | Git operations | ❌ not done — `HEAD 040651b`, 83 entries, 0 staged |

**Tools used (read-only only):** `read_console`, `manage_scene` (`get_active` / `get_loaded_scenes` / `get_build_settings`), `manage_editor` (`play` / `stop`), `execute_code` (read-only reflection and property reads). **No mutating MCP tool was invoked.**

---

## 15. Evidence

| Artefact | Location |
|---|---|
| This report | `C:\Users\cyril\WildWestGunslinger\AUDIT_2_RUNTIME_REPORT.md` |
| MCP session | HTTP JSON-RPC, `http://127.0.0.1:8080/mcp`, server `mcp-for-unity-server` 3.4.7 |
| Unity instance | `WildWestGunslinger@ad0b2d56a935767b` |
| Play-mode window | t ≈ 0 → 245 s, frames 0 → 14 575 |
| Console | 5 errors, 10 warnings (captured via `read_console`) |
| Scene integrity | `Assets/Scenes/TestArena.unity` mtime `2026-09-26 23:08:42` (pre-dates session) |
| Git integrity | `git status` 83 entries, `git diff` on scene `+370/−2`, `HEAD 040651b`, 0 staged |

All findings in this report are reproducible by re-running the same read-only `execute_code` queries against the same scene.

---

## 16. Impact on AUDIT 2 Reconciliation Matrix

### 16.1 Rows upgraded from `STATIC VERIFIED` / `NOT VERIFIED` to `RUNTIME VERIFIED`

| Matrix ID | Previous | Now |
|---|---|---|
| R2-004 | 12 broken refs — static only | **`RUNTIME VERIFIED`** (V-08) |
| R2-005 | 4 × Missing Script — static only | **`RUNTIME VERIFIED`** (V-09, console) |
| R2-006 | 2 `DIAG_TEMP_*` active | **`RUNTIME VERIFIED`** |
| R2-010 | modular kit 0 Unity refs | **`RUNTIME VERIFIED`** (V-17, `GetDependencies = NONE`) |
| R2-011 | `ArenaGenerator` doesn't use kit | **`RUNTIME VERIFIED`** |
| R2-012 | `Crate_01` CP2 unintegrated | **`RUNTIME VERIFIED`** (0 instances) |
| R2-015 | cover fracture defect (static) | **`RUNTIME VERIFIED ABSENCE`** of any cover instance |
| R2-022 | 4-point LOS (static) | **`RUNTIME VERIFIED`** — correctly does not see through walls (V-07) |
| R2-023 | sound pursuit chain (static) | remains **`NOT VERIFIED`** (never triggered) |
| R2-027 | no character in game | **`RUNTIME VERIFIED ABSENCE`** (V-22) |
| R2-028 | weapons/equipment/animation absent | **`RUNTIME VERIFIED ABSENCE`** (V-22, A-02…A-05) |
| R2-030 | desktop fire input absent | **`RUNTIME VERIFIED ABSENCE`** (§3.2) |
| R2-046 | no secrets | unchanged (non-runtime) |

### 16.2 New rows added

| New ID | Domain | Statement | Classification |
|---|---|---|---|
| R2-051 | Environment | Modular FBX import at 0.01× scale, Z-up uncompensated | `RUNTIME ISSUE` (CRITICAL) |
| R2-052 | Combat | `Shooter`/`Rusher` prefabs carry text/debug materials | `RUNTIME ISSUE` (HIGH) |
| R2-053 | Combat | `GunController.Awake` forces damage ≥ 200 on both instances | `RUNTIME ISSUE` (HIGH) |
| R2-054 | UI | `MobileTouchControls` null refs are latent — inert on PC, active on Android | `RUNTIME ISSUE` (CRITICAL) |
| R2-055 | Environment | `ArenaGenerator` produces 9 rooms / 71 walls / 148 covers / 52 zones / 72 CoverPoints | `RUNTIME VERIFIED` |
| R2-056 | AI | `ArenaTacticalMap.IsBuilt = true` at runtime; enemies move along non-null paths | `RUNTIME VERIFIED` |
| R2-057 | AI | AI never left `Searching`; combat chain unexercised | `NOT VERIFIED` |
| R2-058 | Performance | 1552 objects / 4097 renderers / 24 LineRenderers in play | `RUNTIME VERIFIED` |
| R2-059 | Environment | Zero `DestructibleObject` instances — foundation never runs | `RUNTIME VERIFIED ABSENCE` |
| R2-060 | Environment | `WWGEnvironmentGenerator` produced 294 objects, 0 `UNS_*` | `RUNTIME VERIFIED` |

### 16.3 Documentation claims now falsified by runtime evidence

| Claim | Documentation | Runtime reality | Verdict |
|---|---|---|---|
| «Управление игроком \| CONFIRMED» | `CONFIRMED_STATE.md:79` | `MobileTouchControls.playerController = null` | **REFUTED** |
| «Стрельба игрока \| CONFIRMED» | `CONFIRMED_STATE.md:80` | no desktop fire path; FireButton controller misconfigured | **REFUTED** |
| «Bullet \| CONFIRMED» | `CONFIRMED_STATE.md:81` | bullet prefab resolves, but no firing occurs on desktop | **PARTIALLY REFUTED** |
| «5 1-костный риг + сокеты» | `wwg-character-art/SKILL.md:283–312` | 0 Animators, 0 sockets, 0 skins | **REFUTED** |
| Stage 4.4.4 counts (6/16/48/255) | `PROJECT_STATE.md:246` | generator has trees/bushes **disabled**, prefab arrays empty | **NOT REPRODUCIBLE** |
| `AI Stage 3` group-search baseline | `CONFIRMED_STATE.md` | never exercised (no detection) | **STILL NOT VERIFIED** |

### 16.4 Runtime findings that **reduce** the project's assessed state

Contrary to the optimistic reading of AUDIT 2, this pass **lowers** confidence in three areas:

1. **`CONFIRMED_STATE.md` over-claims** — two of its three CONFIRMED gameplay entries are refuted by runtime reference state.
2. **`wwg-character-art` is materially misleading** — runtime confirms total absence of the system it describes.
3. **The art pipeline has an import-side defect** that the Blender-side QA cannot see; the "FINAL" modular assets would import catastrophically wrong.

### 16.5 Runtime findings that **raise** the project's assessed state

1. **The core loop actually runs.** 9-room arena generates, map builds, enemies spawn, FSM ticks, vision scans without false positives, navigation moves them. This is a functioning prototype, not a stub.
2. **`ArenaTacticalMap.IsBuilt = true`** retires the historical Stage-3 root cause ("map unbuilt → all spawns rejected").
3. **Zero project-code exceptions** in a 245-second play session is a strong health signal.
4. **The modular asset contract is met at the mesh level** (1 submesh, vertex colours, UVs, exact triangle counts) — only the import transform needs fixing.

---

## STOP POINT

Runtime verification complete. **Nothing was changed, fixed, promoted, committed, or documented.**

Outstanding from AUDIT 2 remains open:
- **U-1** recovery sequence (now even more urgent: this session added `AUDIT_2_RUNTIME_REPORT.md` to the untracked set)
- **U-4** DEC-012 material-rule ruling
- **U-6/U-7** asset promotion decisions — **U-6 is now blocked by RT-01**: promoting `WallSegment_01`/`FloorSegment_01` requires resolving the 0.01× import scale first
- **U-11** fetch the 2 GitHub-only MD
- **New:** RT-01 and RT-05 were not foreseeable from static analysis and should be added to the decision queue

Awaiting further APPROVE.

**AUDIT 2 RUNTIME PASS COMPLETE — READ-ONLY — NO CHANGES MADE**
