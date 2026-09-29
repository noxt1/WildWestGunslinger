# UNI-0002 — Unity Data and Asset Register

**Document ID:** `UNI-0002`
**Status:** CANONICAL
**Date:** 2026-09-29

---

## 1. Scenes

| Path | GUID | Bytes | Modified | Build Settings | Runtime | Git |
|---|---|---|---|---|---|---|
| `Assets/Scenes/TestArena.unity` | `bcc94342232f0d0468763dc03a0ec36d` | 223 870 | 2026-09-26 23:08:42 | **enabled** | **active** | tracked |
| `Assets/TestArena.unity` | `92227d062a096334b8edf7335614c361` | 223 870 | 2026-09-26 22:46:04 | no | not loaded | **untracked** |

The two differ by 2 lines. `Assets/Scenes/TestArena.unity` is canonical.
The root copy is a **DUPLICATE — REVIEW, DO NOT DELETE**.

---

## 2. Prefabs

| Prefab | Issue | Git |
|---|---|---|
| `Shooter.prefab` | material `TMP_SDF-HDRP LIT` (`UNI-D08`) | tracked |
| `Rusher.prefab` | material `FrameDebuggerRenderTargetDisplay` (`UNI-D09`) | tracked |
| `Assets/Prefabs/Environment/WWG_*.prefab` | required by scene | **untracked** |

Both confirmed-firing weapon prefabs carry a **broken surface material**:
a TextMeshPro font material on Shooter, a frame-debugger display material on
Rusher. Neither has a valid character texture applied.

---

## 3. Materials

| Path / material | Issue | Git |
|---|---|---|
| `TMP_SDF-HDRP LIT` | font material misapplied to a mesh renderer | — |
| `FrameDebuggerRenderTargetDisplay` | debug material never replaced | — |
| `Assets/Materials/WWG_*.mat` | required by scene | **untracked** |

---

## 4. Environment art — modular kit

| Path | State | Blocker |
|---|---|---|
| `Assets/Art/Environment/Modular/` — Wall FBX revisions | present | import at 0.01× (`UNI-D06`) |
| `Assets/Art/Environment/Modular/` — Floor FBX revisions | present | import at 0.01× (`UNI-D06`) |
| FINAL Wall / Floor selection | **OPEN DECISION** | — |
| Scene integration | **0 modular meshes** | `UNI-D06` |

---

## 5. Props art

| Path | State |
|---|---|
| `Assets/Art/Props/` — FBX revisions | present |
| `Assets/Art/Props/` — controlled-fracture assets | present, **0 instances in scene** (`UNI-D12`) |
| `CRATE_01` | `QA PASS` — CP1 complete; **CP2 NOT AUTHORIZED** (`DEC-05`). `BARREL_01`/`FENCE_01` are `APPROVED` but **0 destructible instances** exist in the scene |
| `Cover` r4 / r5 | `QA PASS`; **status is an OPEN DECISION** |

---

## 5a. Environment prefab registry

**Added in Phase 4.** Recovered from `AI_CONTEXT/PROJECT_STATE.md`; integration
dated 2026-09-21, recorded as *prepared, not user-confirmed*.

| Prefab (`Assets/Prefabs/Environment/`) | Parts | HP | CoverPoints |
|---|---|---|---|
| `Fence_Western_Wooden.prefab` — states INTACT 12 / DAMAGED 4 / DESTROYED 2 debris | 18 (~504 v) | 50 | 2 |
| `Lantern_Post_Western.prefab` — Glass/Bulb/frame, **no light** | 15 (338 v / 160 t) | 40 | 0 (decor) |
| `Barrel_Western_Wooden.prefab` — Staves/Hoops/Lids | 6 (456 v / 268 t) | 30 | 2 |
| `Crate_Western_Wooden.prefab` — Corners/Panels | 10 (230 v / 110 t) | 40 | 2 |
| `Cover_Western_Wooden.prefab` — Posts/Feet/Planks/TopLog | 8 (221 v / 113 t) | 50 | 2 |
| `Wagon_Western_Wooden.prefab` — Wheels/Axles/Bed/Walls/Bench/Shafts, 2 BoxCollider | 14 (568 v / 320 t) | 80 | 2 |
| `Western_Log_Wall_Segment.prefab` — **PREPARED, not scattered** | 8 (416 v / 236 t) | 60 | 2 |
| `Western_Plank_Wall_Segment.prefab` — **PREPARED, not scattered** | 14 (322 v / 154 t) | 60 | 2 |

**Integration facts (2026-09-21):**

- All 7 source FBX were **Z-up** → corrected at prefab level with `Model` rotation **−90° X**; `minY = 0.00` on all
- Albedo byte-identical across props; normal maps byte-identical; normal importers set to NormalMap
- 4 shared URP Lit materials in `Art/Environment/Shared/Materials/`
- No Camera/Light inside any FBX; **realtime lights = 0**
- Colliders minimal for Android: 1 Box per prop (wagon 2); Rigidbody only on 2 fence-debris (kinematic, parked)
- Generator pools: Fences max 12 · Props max 10 · Lanterns max 6 · Wagons max 4, all outside rooms
- **No per-frame destruction logic**
- FPS ~30 intermittent, **NOT confirmed**; CoverPoint-marker hypothesis unconfirmed
- Manual Play test (visual, shooting, cover-AI, wall seams) **awaits the user**

> These prefabs exist and are wired, but **0 destructible instances and 0 modular
> meshes** appear in the runtime scene census (`UNI-D11`, `UNI-D12`). Wall
> segments are explicitly **PREPARED, not DONE** — preparation for a future
> environment stage, not completion of one.

## 5b. `WildWestEnvironment` — confirmed working in the Preview scene

| Field | Value |
|---|---|
| Script | `Assets/Scripts/Environment/WildWestEnvironmentGenerator.cs` (1 431 lines) |
| Root | `WildWestEnvironment` (sibling of `GeneratedArena`) |
| Seed | own `System.Random`, deterministic |
| Data source (read-only) | `ArenaTacticalMap.Rooms`, `RoomMap.Module.GetWorldBounds()`, `RoomMap.Exits` |
| Ordering | created after `ArenaTacticalMap.Build()`; objects lie **outside** room bounds; **does not affect navigation** |
| Preview scene | `Assets/Scenes/TestArena_Preview.unity` — the working `TestArena` was not modified |

**Runtime composition (Stage 4.4.4, user-CONFIRMED):** Ground 1
(`M_Ground_Desert`, 152×156 m, y = −0.05) · Cliffs 6 (`UNS_Rock_Cliff_*`) ·
Rocks 32 · Trees 16 (`UNS_Spruce_01/02`) · Bushes 48 (`UNS_Bush`) ·
Grass 255 (`UNS_Grass`, collider disabled on instances). Vegetation sampled from
the bounds boundary, independent per-category spacing. `rockMaxDistance`
fixed 45 → 75.

> Background is **not implemented** (Stage 4.4.5 FROZEN / PAUSED).
> Desert Pack is dormant pending URP conversion of `Mat_01`.

## 5c. Render settings (set 2026-09-27, still uncommitted)

| Asset | Setting | Value | Note |
|---|---|---|---|
| `Mobile_RPAsset` | renderer type | Deferred (`m_RendererType: 1`) | |
| `Mobile_RPAsset` | `m_RenderScale` | **1** (was 0.8) | A/B: 0.8 → 1 reduced stair-stepping |
| `Mobile_RPAsset` | `m_MSAA` | **4** (was 1) | A/B: 1 → 4 improved Cube/Capsule AA |
| `PC_RPAsset` | `m_ColorGradingMode` | **1** HighDynamicRange (was 0) | set during washed-out-scene diagnosis |
| Quality settings | `antiAliasing` | 0 on both levels | **not an error**; URP MSAA provides AA |
| — | FPS | ~56.4 (renderScale 0.8) → ~60 after | no regression |

> **Shadow Aliasing remains OPEN** — shadow resolution 1024, 1 cascade, distance
> 50. See `../History/OPEN_ISSUES.md` `ISSUE-14`. Not fixed in that phase.

## 5d. Destructibles — foundation exists, zero instances

| Field | Value |
|---|---|
| Script | `Assets/Scripts/Environment/DestructibleObject.cs` (376 lines) |
| Scope | the single destructibility core: HP, INTACT → DAMAGED → DESTROYED, hide/show arrays, UnityEvents, **no `Update()`** |
| Integration | `Bullet` / `EnemyBullet` call `TakeDamage` via `GetComponentInParent<DestructibleObject>()`; `CoverSystem` has an `isActiveAndEnabled` guard |
| Instances in scene | **0** (`UNI-D12`) |
| Intent | controlled partial destruction; separate parts may detach; physics only locally; Android performance considered |
| Status | **foundation prepared — Stage 8 NOT performed** |

> This refines `UNI-D12`: the *system* is implemented, the *placement* is not.
> The defect is zero instances, not a missing implementation.

## 6. Characters

| Path | State |
|---|---|
| Character FBX in `Assets/` | **not integrated** |
| SkinnedMeshRenderer in scene | **0** |
| Animator / Avatar / Controller in scene | **0 / 0 / 0** |

---

## 7. Git tracking gaps

The following are required by the canonical scene but are **untracked**:

- `Assets/Prefabs/Environment/WWG_*.prefab`
- `Assets/Materials/WWG_*.mat`
- `Assets/TestArena.unity` (the duplicate scene)

**A clean clone cannot reproduce the playable scene.** This is `RISK-03`.

---

## 8. Cross-references

- `UNI-0001` — project state
- `UNI-0003` — known defects
- `ART-0002` — art asset register

---

**End of `UNI-0002-UNITY-DATA-AND-ASSET-REGISTER.md`**
