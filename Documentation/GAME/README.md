# GAME - cross-domain gameplay, design and requirements registers

> **Layer:** DOMAIN. **Apply** for gameplay, design and requirements work.

These documents deliberately **span domains**. They are not split or duplicated, because splitting
a single register would create divergent sources of truth.

## Current documents

| Document | Role | Domains covered |
|---|---|---|
| `GAME-0001-GAME-DESIGN-STATE.md` | design intent and verified gameplay parameters | Environment (arena, cover), Weapons, Character, level content |
| `GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` | runtime state of gameplay systems | Player, Enemy, Progression (XP, upgrades, levels), Combat, AI support, UI |
| `GAME-0004-REQUIREMENTS-AND-DESIGN-DECISIONS.md` | requirements and design-decision register | progression and economy, content, art and workflow constraints |

`GAME-0003-CONTROLS-AND-PLATFORM-TARGETS.md` moved to `../UI/GAME-0003-CONTROLS-AND-PLATFORM-TARGETS.md` because it is a single-domain
input document.

## Domains represented here rather than in their own folder

Following the "no empty folders" rule, these have **no standalone document** and are therefore
covered here and by the root canonical documents:

| Domain | Where it is documented |
|---|---|
| **Combat** | `GAME-0002` section 4; `../ARCHITECTURE.md` |
| **Progression** | `GAME-0002` section 3.1; `GAME-0004` section 3 |
| **Enemy** | `GAME-0002` section 3; `GAME-0001` sections 2-4 |
| **Environment** | `GAME-0001` sections 2-3; `../Art/ART-0002-ART-ASSET-REGISTER.md`; `RT-01` in `../History/OPEN_ISSUES.md` |
| **Weapons** | `GAME-0001` section 5; `GAME-0002` section 4; `DEC-11` in `../History/OPEN_DECISIONS.md` |

## Source-of-truth rule

- Gameplay system state: `GAME-0002`
- Design state: `GAME-0001`
- Requirements and decisions: `GAME-0004` plus the root `REQUIREMENTS.md` and `DECISIONS.md`
- Cross-project reality: `../../PROJECT_TRUTH.md`