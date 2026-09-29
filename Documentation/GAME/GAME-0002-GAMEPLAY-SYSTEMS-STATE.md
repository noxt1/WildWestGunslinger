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

---

## 4. Combat systems

| System | Status | Evidence |
|---|---|---|
| `Shooter.prefab` fires | ✅ | observed |
| `Rusher.prefab` fires | ✅ | observed |
| Damage value correct | ❌ | forced to 200 at runtime, overriding 10 / 100 |
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
