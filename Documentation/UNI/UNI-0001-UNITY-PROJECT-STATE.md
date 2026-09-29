# UNI-0001 — Unity Project State

**Document ID:** `UNI-0001`
**Status:** CANONICAL
**Date:** 2026-09-29
**Evidence class:** runtime observation + verified file facts

---

## 1. Project identity

| Field | Value |
|---|---|
| Engine | **Unity 6000.3.23f1** (Unity 6) — verified in `ProjectSettings/ProjectVersion.txt` (`m_EditorVersionWithRevision: 6000.3.23f1 (09d2ecc7fb28)`) |
| Project root | repository root (relative: `.`) |
| Active scene | `Assets/Scenes/TestArena.unity` |
| Stage | Pre-alpha, unreleased |
| Primary platform | Windows PC |
| Secondary platform | Android (blocked — see `UNI-0003`) |
| Render pipeline | **URP** — `Mobile_RPAsset` (Deferred, `m_RendererType: 1`), `PC_RPAsset` (HighDynamicRange color grading) |
| Platform note | `AI_CONTEXT/RULES.md` states Android is the **primary** platform and Windows secondary — a project-level statement, not yet reflected in a canonical decision |

---

## 2. Scene

| Field | Value |
|---|---|
| Canonical path | `Assets/Scenes/TestArena.unity` |
| GUID | `bcc94342232f0d0468763dc03a0ec36d` |
| Size | 223 870 B |
| Modified | 2026-09-26 23:08:42 |
| Build Settings | **enabled** |
| Runtime | **loaded as active scene** |
| Git | tracked |

Duplicate `Assets/TestArena.unity` (GUID `92227d062a096334b8edf7335614c361`,
untracked, 2 differing lines) exists. **REVIEW — do not delete.**

---

## 3. Runtime-confirmed working systems

| System | Measured result |
|---|---|
| Arena generator | 9 rooms, 71 walls, 11 floors |
| Tactical map | `ArenaTacticalMap.IsBuilt = true` |
| Cover | 148 cover objects, 52 spawn zones, 72 `CoverPoint` |
| Enemy spawning | enemies instantiate at runtime |
| Enemy FSM | reaches `Searching`; vision scanning active |
| Navigation | custom A\* returns non-null paths; movement observed |
| Weapons | both `GunController` prefabs fire |

---

## 4. Runtime-confirmed absent systems

| System | Measured result |
|---|---|
| NavMesh | 0 agents, 0 triangulation |
| Character rig | 0 Animator, 0 Avatar, 0 Controller, 0 SkinnedMeshRenderer |
| Player body | Capsule |
| Enemy bodies | Capsule |
| Destructibles | 0 instances |
| Modular environment | 0 meshes integrated |
| `WWG_*` objects in scene | 294 |
| Animator / Avatar / AnimatorController / SkinnedMeshRenderer | 0 / 0 / 0 / 0 |
| **Equipment sockets / bone attachment points** | **none** — no `socket`, `attach`, `BoneAttach` or `EquipPoint` code exists in `Assets/Scripts` (**VERIFIED ABSENCE**, static) |

---

## 5. Script / code state

| Item | Value |
|---|---|
| Assembly | `Assembly-CSharp` (managed DLL of 2026-09-02 exists in the IL2CPP build) |
| **Total `.cs` files** | **42** in **10** subdirectories of `Assets/Scripts/` |
| Missing Mono Scripts | **4** — root cause identified, see `UNI-D05` |
| Broken object references | **12** — still unidentified, see `UNI-D04` |
| Custom pathfinding | A\*-based, functional, not NavMesh |
| Enemy FSM states verified | `Searching` only |
| Enemy FSM states unverified | combat, investigation, sound, cover, flanking |

### 5.1 Verified script inventory

Line counts are **measured on 2026-09-29**, not copied from any document.

| Script | Lines | Folder |
|---|---|---|
| `EnemyController.cs` | 3 738 | `Enemies/` |
| `EnemyTacticalVision.cs` | 2 268 | `Enemies/` |
| `EnemyDirector`/`EnemyTacticalPlanner.cs` | **1 416** | `Enemies/` |
| `EnemyHearing.cs` | 342 | `Enemies/` |
| `NoiseSystem.cs` | 557 | `Enemies/` |
| `ArenaTacticalMap.cs` | 1 770 | `Procedural/` |
| `ArenaGenerator.cs`, `ArenaModule.cs` | — | `Procedural/` |
| `CoverSystem.cs` | 514 | `Enemies/Cover/` |
| `CoverPoint.cs` | 486 | `Enemies/Cover/` |
| `EnemySpawner.cs` | 1 623 | `Level/` |
| `WaveManager.cs` | 162 | `Level/` |
| `DestructibleObject.cs` | 376 | `Environment/` |
| `WildWestEnvironmentGenerator.cs` | 1 431 | `Environment/` |
| `EnemyDirectionIndicator.cs` | 2 052 | `UI/` |
| `HUDController.cs`, `DeathUIController.cs`, `SettingsUIManager.cs` | — | `UI/` |
| `GunController.cs`, `Bullet.cs` | — | `Weapons/` |
| `XPManager.cs`, `XPOrb.cs`, `XPBarController.cs`, `UpgradeManager.cs`, `UpgradeUI.cs` | — | `Player/` |
| `PlayerController.cs`, `PlayerHealth.cs`, `CameraFollow.cs`, `MobileJoystick.cs`, `MobileTouchControls.cs` | — | `Player/` |
| `GameSettings.cs`, `DifficultyManager.cs`, `MainMenuController.cs`, `PauseMenuController.cs`, `SettingsController.cs` | — | `Core/` |
| `MobilePerformanceTest.cs` | — | `Performance/` |

> Folder counts: Core 5 · Cover 2 · Enemies 9 · Environment 2 · Level 4 ·
> Performance 1 · Player 10 · Procedural 3 · UI 4 · Weapons 2 = **42**.
>
> **`AI_CONTEXT/PROJECT_STATE.md` claims "41 .cs in 8 subdirectories" and omits
> the `Environment/` folder entirely. Both figures are stale** — see
> `../History/OPEN_CONFLICTS.md` `CONFLICT-11`.

### 5.2 Systems present in code but absent from the scene

| System | Script exists | In scene |
|---|---|---|
| XP / level up | ✅ 5 scripts in `Player/` | ❌ `UNI-D13` — `HUDController.xpBar` / `levelText` null |
| Destructibles | ✅ `DestructibleObject.cs` (376 lines) | ❌ 0 instances (`UNI-D12`) |
| `WildWestEnvironment` | ✅ 1 431 lines | ⚠️ confirmed working in `TestArena_Preview.unity` (`Stage 4.4.4` CONFIRMED) |
| `EnemyDirectionIndicator` | ✅ 2 052 lines | ✅ runtime-created via `[RuntimeInitializeOnLoadMethod]`, not serialized |

**Never claim the unverified FSM states as working.**

---

## 6. Untracked scene dependencies

The following are required by the scene but are **untracked in Git** — a clean
clone would not reproduce the scene:

| Path | Note |
|---|---|
| `Assets/Prefabs/Environment/WWG_*.prefab` | critical scene dependencies |
| `Assets/Materials/WWG_*.mat` | critical scene dependencies |

---

## 7. What is NOT established

- No Git history before 2026-09-25, so pre-git work has no provenance in-repo.
- `origin/main` is 2 commits ahead and contains 2 Markdown files absent locally.
- No release configuration, no CI, no build pipeline definition is documented.

---

## 8. Cross-references

- `UNI-0002` — data and asset register
- `UNI-0003` — known defects
- `UNI-0004` — environment and toolchain
- `PROJECT_TRUTH.md` — consolidated truth
- `AUDIT_2_RUNTIME_REPORT.md` — raw runtime evidence

---

**End of `UNI-0001-UNITY-PROJECT-STATE.md`**
