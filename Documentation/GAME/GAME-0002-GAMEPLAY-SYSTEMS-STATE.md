# GAME-0002 — Gameplay Systems State

**Document ID:** `GAME-0002`
**Status:** CANONICAL
**Date:** 2026-09-29

Every row is either **runtime-verified** or explicitly **unverified**. Nothing
is inferred from documentation.

---

## 1. Verification legend

| Symbol | Meaning |
|---|---|
| ✅ | Runtime-verified on 2026-09-29 |
| ❌ | Runtime-verified as broken/absent |
| ❓ | **Not tested — do not claim as working** |

---

## 2. Player systems

| System | Status | Evidence |
|---|---|---|
| Player exists | ✅ | instantiated at runtime |
| Player representation | ❌ | **Capsule**, not a character |
| Player movement | ✅ | movement observed |
| Player weapon use | ✅ | firing confirmed |
| Player health / damage | ❓ | not tested |
| Player animation | ❌ | 0 Animator, 0 Avatar, 0 Controller, 0 SkinnedMeshRenderer |
| Touch controls | ❌ | `MobileTouchControls` refs all `null` (`UNI-D07`) |
| Aim / camera | ❓ | not tested |

---

## 3. Enemy systems

| System | Status | Evidence |
|---|---|---|
| Enemy spawning | ✅ | enemies instantiate at runtime |
| Enemy representation | ❌ | **Capsule** |
| FSM reaches `Searching` | ✅ | observed state |
| Vision scanning | ✅ | observed |
| Navigation (custom A\*) | ✅ | non-null paths, movement observed |
| Navigation (NavMesh) | ❌ | 0 agents, 0 triangulation |
| **Combat behaviour** | ❓ | **not tested** |
| **Investigation behaviour** | ❓ | **not tested** |
| **Sound propagation / reaction** | ❓ | **not tested** |
| **Cover-taking** | ❓ | **not tested** |
| **Flanking** | ❓ | **not tested** |
| Enemy health / death | ❓ | not tested |
| Enemy animation | ❌ | no rig integrated |

> The five ❓ rows are the single largest evidence gap in the project. The
> design documents imply a tactical AI using `ArenaTacticalMap` and `CoverPoint`,
> but **none of that tactical behaviour has been observed**.

### 3.1 Progression systems — corrected in Phase 4

| System | Status | Evidence |
|---|---|---|
| XP accumulation | **EXISTS, user-confirmed (earlier session)** | ⚠️ see note |
| Level up | **EXISTS, user-confirmed (earlier session)** | ⚠️ see note |
| `UpgradeManager` / `UpgradeUI` | **EXISTS, user-confirmed (earlier session)** | ⚠️ see note |
| XP **readout** | ❌ **broken** | `UNI-D13` — `HUDController.xpBar` / `levelText` are null |
| Economy (currency, shop, cards) | ❓ | **not tested** — no runtime evidence |
| Run progression | ❓ | **not tested** |

> **⚠️ CORRECTION (Phase 4).** Phase 2 recorded progression as having "**no
> evidence of any kind**". That was wrong. **Five scripts exist** —
> `XPManager.cs`, `XPOrb.cs`, `XPBarController.cs`, `UpgradeManager.cs`,
> `UpgradeUI.cs` in `Assets/Scripts/Player/` — and
> `AI_CONTEXT/CONFIRMED_STATE.md` lists XP, Level Up and UpgradePanel as
> **user-CONFIRMED** in an earlier session.
>
> The U-12 audit (2026-09-29) did not exercise them, so current behaviour is
> still `NOT VERIFIED`, and the HUD readout is definitely broken (`UNI-D13`).
> Recorded as `CONFLICT-12`.

### 3.2 User-confirmed systems (historical confirmations)

`AI_CONTEXT/CONFIRMED_STATE.md` records these as explicitly confirmed by the
project owner in earlier sessions. They are **historical confirmations**, not
2026-09-29 runtime evidence, and are kept separate from the tables above.

| System | Recorded status |
|---|---|
| `EnemyController` state machine | CONFIRMED (Stage 1 + 2) |
| `EnemyTacticalPlanner` (roles, groups) | CONFIRMED (Stage 1) |
| A\* navigation | CONFIRMED (Stage 2) |
| `WaveManager`, `EnemySpawner` | CONFIRMED |
| Main menu, pause, restart | CONFIRMED |
| Player movement, player shooting | CONFIRMED |
| `Bullet` | CONFIRMED |
| `EnemyTacticalVision`, `EnemyHearing`, `NoiseSystem`, `CoverSystem`, `ArenaTacticalMap` | EXISTS (unchanged) |
| Stage 3 group target distribution | **NOT CONFIRMED** — Implemented/Compiled/Tested = YES, Confirmed = **NO** |
| Stage 4.4.4 Trees + Bushes + Grass | **CONFIRMED** (runtime, `TestArena_Preview.unity`) |
| Mobile UI (joystick, touch controls) | CONFIRMED — ⚠️ **conflicts with `UNI-D07`**, see `CONFLICT-13` |

> **Dated combat evidence.** Stage 3 was practically tested in Unity; 13 numbered
> observations are recorded, including item 12: *"Боевой тест: самостоятельный
> выход в центр, пропуск игрока вне взгляда, детект и атака"* — advance to centre,
> miss the player when out of view, then detect and attack.
>
> This is **dated evidence that combat behaviour exists**, but it was never
> user-confirmed and predates the 2026-09-29 audit. AI combat therefore remains
> `NOT VERIFIED` in the current-state table, with this history preserved rather
> than deleted.

---

## 4. Combat systems

| System | Status | Evidence |
|---|---|---|
| `Shooter.prefab` fires | ✅ | observed |
| `Rusher.prefab` fires | ✅ | observed |
| Damage value correct | ✅ **RUNTIME VERIFIED** | authored values honoured: Player **100**, FireButton **10**; upgrades ×1.20 cumulative (`UNI-D10` fixed Phase 5B) |
| Damage model / hit detection | ❓ | not tested |
| Hit feedback | ❓ | not tested |
| Destructibles | ❌ | 0 instances in scene |

---

## 5. AI support systems

| System | Status | Evidence |
|---|---|---|
| `ArenaTacticalMap` built | ✅ | `IsBuilt = true` |
| `CoverPoint` registry | ✅ | 72 registered |
| Spawn zone registry | ✅ | 52 registered |
| Cover objects present | ✅ | 148 in scene |
| Pathfinding | ✅ | custom A\* functional |

**The support layer exists and is populated. The behaviours that consume it
(cover-taking, flanking) are untested.**

---

## 6. Blocked logic

| # | Defect | Blocks |
|---|---|---|
| `UNI-D04` | 12 broken references | unknown — must be enumerated first |
| `UNI-D05` | 4 missing Mono Scripts | unknown — must be enumerated first |
| `UNI-D01` | no NavMesh | NavMesh-dependent AI |
| `UNI-D02`/`D03` | no character rig | all animation, all visual character identity |

> `UNI-D04` and `UNI-D05` are the highest-priority unknowns: a missing script or
> a null reference can silently disable an entire gameplay system, and may
> already explain why several systems are untested.

---

## 7. Cross-references

- `UNI-0003` — full defect list with severity
- `GAME-0001` — design state
- `History/OPEN_ISSUES.md` — open questions

---

**End of `GAME-0002-GAMEPLAY-SYSTEMS-STATE.md`**
