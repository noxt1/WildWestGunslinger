# PROJECT_STATE

**Document ID:** `DOC-STATE-2026-09-29`
**Status:** CANONICAL
**Step 3 of the navigation chain** — see `README.md` §2.

One line per system. **Status vocabulary is strict** (`README.md` §5).

---

## 1. Overall

| Field | Value |
|---|---|
| Stage | **Pre-alpha, unreleased** |
| Shippable | **NO** — 7 critical defects open |
| Canonical scene | `Assets/Scenes/TestArena.unity` |
| Git | `main` @ `7ad314c`, **ahead 1 / behind 2** vs `origin/main`, 0 staged |
| Runtime verification channel | **AVAILABLE** | RUNTIME VERIFIED attainable | Unity MCP 3.4.7 on 127.0.0.1:8080; Play Mode + Console proven 2026-09-29 | PHASE5R |
| Asset approval state | **`BARREL_01` and `FENCE_01` = `APPROVED`** (user-confirmed). `CRATE_01` = `QA PASS`, CP2 not authorized. **Nothing is `PROMOTED`/`INTEGRATED`/`RUNTIME VERIFIED`** |
| Character art | complete, measured, **backed up (CLOSED)**, **not integrated** |
| Ready for consolidation commit | **YES**, pending human `APPROVE` |

---

## 2. Systems

| System | Status | Class | Evidence | Detail |
|---|---|---|---|---|
| Arena generation | working | `RUNTIME VERIFIED` | 9 rooms, 71 walls, 11 floors | `UNI-0001` |
| `ArenaTacticalMap` | built | `RUNTIME VERIFIED` | `IsBuilt = true` | `UNI-0001` |
| Cover objects | present | `RUNTIME VERIFIED` | 148 objects, 72 `CoverPoint` | `GAME-0001` |
| Spawn zones | present | `RUNTIME VERIFIED` | 52 | `GAME-0001` |
| Enemy spawning | working | `RUNTIME VERIFIED` | enemies instantiate | `UNI-0001` |
| Enemy FSM (`Searching`) | working | `RUNTIME VERIFIED` | state observed | `GAME-0002` |
| Enemy vision scanning | working | `RUNTIME VERIFIED` | observed | `GAME-0002` |
| Custom A\* navigation | working | `RUNTIME VERIFIED` | non-null paths, movement | `GAME-0002` |
| Weapon firing | working | `RUNTIME VERIFIED` | one canonical `GunController` (`Player#70770`); fire path == upgrade path; bullets carry current damage | `GAME-0002` |
| Gun damage value / identity | **fixed** | `RUNTIME VERIFIED` | base **100** unchanged; `FireButton` duplicate removed; controller count 2 -> 1; identity equality 70770==70770 | `UNI-D10` closed, `ISSUE-22` closed |
| NavMesh | **absent** | `VERIFIED ABSENCE` | 0 agents, 0 triangulation | `UNI-D01` |
| Character rig in Unity | **absent** | `VERIFIED ABSENCE` | 0 Animator/Avatar/Controller/SkinnedMesh | `UNI-D02` |
| Character visuals | **absent** | `VERIFIED ABSENCE` | player and enemies are capsules | `UNI-D03` |
| Broken references | **defect** | `RUNTIME VERIFIED` | 12, unidentified | `UNI-D04` |
| Missing scripts | **defect** | `RUNTIME VERIFIED` | 4, unidentified | `UNI-D05` |
| Modular FBX import | **broken** | `RUNTIME VERIFIED` | 0.01×, Z-up uncompensated | `UNI-D06` |
| Android touch controls | **broken** | `RUNTIME VERIFIED` | all refs `null` | `UNI-D07` |
| `Shooter` material | **broken** | `RUNTIME VERIFIED` | `TMP_SDF-HDRP LIT` | `UNI-D08` |
| `Rusher` material | **broken** | `RUNTIME VERIFIED` | `FrameDebuggerRenderTargetDisplay` | `UNI-D09` |
| Modular integration | **not started** | `VERIFIED ABSENCE` | 0 modular meshes | `UNI-D11` |
| Destructibles | **not started** | `VERIFIED ABSENCE` | 0 instances | `UNI-D12` |
| Equipment sockets | **absent** | `VERIFIED ABSENCE` (static) | no socket code in `Assets/Scripts` | `UNI-0001` |
| AI combat | `NOT VERIFIED` | — | untested | `GAME-0002` |
| AI investigation | `NOT VERIFIED` | — | untested | `GAME-0002` |
| AI sound reaction | `NOT VERIFIED` | — | untested | `GAME-0002` |
| AI cover-taking | `NOT VERIFIED` | — | untested | `GAME-0002` |
| AI flanking | `NOT VERIFIED` | — | untested | `GAME-0002` |
| XP / economy / cards / shop | `NOT VERIFIED` | — | no evidence at all | `GAME-0004` |
| Build from current code | **never attempted** | `VERIFIED FACT` | last build 2026-09-09 | `REL-0002` |
| CI / tests / build pipeline | **absent** | `VERIFIED ABSENCE` | none exist | `ISSUE-09` |

---

## 3. Art

| Item | Status | Class |
|---|---|---|
| Character source (307 files) | complete + **backed up** | `STATIC VERIFIED` |
| Doomy measurement docs | canonical | `STATIC VERIFIED` |
| Character QA renders (~120) | exist | `STATIC VERIFIED` |
| Vest/glove QA renders (25) | exist | `STATIC VERIFIED` |
| Wall/Floor modular FBX (6) | on disk | `STATIC VERIFIED` |
| Character FBX (5) + weapons (4) | exported, not integrated | `STATIC VERIFIED` |
| Unity integration of any of the above | **none** | `VERIFIED ABSENCE` |
| Any asset `APPROVED` | **none** | `VERIFIED ABSENCE` |
| Canonical character `.blend` | **undecided** | `OPEN DECISION` `DEC-02` |

---

## 4. Domain documents

| Domain | Entry |
|---|---|
| Art | `ART/ART-0001-ART-STATE.md` |
| Unity | `UNI/UNI-0001-UNITY-PROJECT-STATE.md` |
| Game design | `GAME/GAME-0001-GAME-DESIGN-STATE.md` |
| Gameplay systems | `GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` |
| Requirements | `REQUIREMENTS.md`, `GAME/GAME-0004-REQUIREMENTS-AND-DESIGN-DECISIONS.md` |
| AI / tooling | `AI/AI-0001-AI-AGENT-STATE.md` |
| Backups / builds / release | `REL/REL-0001-RECOVERY-AND-BACKUP.md`, `REL/REL-0002-BUILD-ARTIFACTS.md` |
| Character source | `Character/CHARACTER_SOURCE_BACKUP.md` |
| History | `History/HISTORY-0001-TIMELINE.md` |

---

**End of `PROJECT_STATE.md`**
