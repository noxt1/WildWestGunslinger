# OPEN CONFLICTS

**Status:** CANONICAL register of unresolved contradictions between sources
**Date:** 2026-09-29

A conflict is **resolved** when a higher-ranked evidence class settles it
(`DOC-0001` §1) or a human decides it. Until then it is recorded here.

**Status key:** `RESOLVED` (settled by evidence) · `OPEN` (needs a human)

---

## CONFLICT-01 — Character file: missing vs. present

| Field | Value |
|---|---|
| Sources | Audit 1: "NOT FOUND" · Filesystem: present |
| Resolution | **RESOLVED** — the file exists in `Documents\WildWestGunslinger art`. Audit 1 was scoped to the repository only |
| Ref | `HISTORY-0003` §1 |

---

## CONFLICT-02 — Chest measurement: 141 cm vs. 100

| Field | Value |
|---|---|
| Sources | `AUDIT_2_RECONCILIATION_REPORT.md`: `Chest=141cm` · `Doomy_measurements.md` + vest size-50 reference: **100** |
| Resolution | **RESOLVED** — 100 (chest width 21 on the size-50 table, consistent with "our torso ~100"). 141 is a unit/scale error |
| Consequence | Audit 2's "foundation identified" conclusion must be re-derived |
| Ref | `ART-0001` §4.1 |

---

## CONFLICT-03 — Bone count: 62 vs. 51

| Field | Value |
|---|---|
| Sources | `AUDIT_2_RECONCILIATION_REPORT.md`: `Skeleton=62 bones` · `Doomy_measurements.md`: `WWG_Template_Armature` = **51 bones** |
| Resolution | **RESOLVED** — 51 |
| Ref | `ART-0001` §4.1 |

---

## CONFLICT-04 — Release build: none vs. exists

| Field | Value |
|---|---|
| Sources | `AI_TOOLCHAIN_AUDIT.md`: "no release build" · Filesystem: `Desktop\WildWest.apk` 46.2 MB, 2026-09-09 |
| Resolution | **RESOLVED** — the APK exists |
| Ref | `REL-0002` |

---

## CONFLICT-05 — Backups: one vs. two

| Field | Value |
|---|---|
| Sources | All three audits: recovery picture = the 2026-09-29 snapshot · Filesystem: `Z:\Мой диск\WWG_RECOVERY\2026-09-22_FULL` (11.87 GB) |
| Resolution | **RESOLVED** — two dated backups exist, covering different scopes |
| Ref | `REL-0001` |

---

## CONFLICT-06 — Canonical scene: which `TestArena`?

| Field | Value |
|---|---|
| Sources | Two files, same name, different GUID |
| Resolution | **RESOLVED** — `Assets/Scenes/TestArena.unity`. Build Settings enabled, loaded at runtime, later mtime, tracked, and 2 lines differ |
| Residual | Disposition of the duplicate remains `DEC-09` |
| Ref | `UNI-0002` §1 |

---

## CONFLICT-07 — APK existence vs. Android being broken

| Field | Value |
|---|---|
| Sources | `WildWest.apk` exists (2026-09-09) · `MobileTouchControls` fully null-referenced in the current build (`UNI-D07`) |
| Resolution | **RESOLVED** — no contradiction: the APK is **stale**. `Assembly-CSharp.dll` is dated 2026-09-02, three weeks before the current state. The APK predates the current touch-control code and does not prove Android works |
| Ref | `GAME-0003` §3, `REL-0002` §3 |

---

## CONFLICT-08 — Cover count: 148 objects vs. 72 `CoverPoint`

| Field | Value |
|---|---|
| Sources | Runtime: 148 cover objects, 72 registered `CoverPoint`, 52 spawn zones |
| Resolution | **OPEN** — may be decorative objects, or a registration gap. Not a proven defect |
| Needs | A human/agent pass enumerating cover objects vs. registered points |
| Ref | `GAME-0001` §3, `Q15` |

---

## CONFLICT-09 — `AI_CONTEXT/` internal consistency

| Field | Value |
|---|---|
| Sources | 12 files in `AI_CONTEXT/`, many modified or conflicted in the working tree, with overlapping scope and no single authority |
| Resolution | **RESOLVED for authority** — all superseded by `Documentation/` (`DOC-0002` S-05). Retained as history |
| Ref | `DOC-0002` |

---

## CONFLICT-10 — GitHub-only docs vs. local tree

| Field | Value |
|---|---|
| Sources | `origin/main` (2 commits ahead) contains `AI_PRODUCTION_METHODOLOGY.md` and `AUDIT_SESSION_CONTEXT_2026-09-28.md`; neither is in the local working tree |
| Resolution | **OPEN** — requires `DEC-07`. Retrieved copies exist externally in `WildWestGunslinger_RECOVERY\github_only\` |
| Note | Retrieved copies use LF line endings (29 300 / 23 269 B) vs. GitHub metadata (30 625 / 24 322 B); the difference equals the line count. Compare against `git cat-file blob` for byte-exact verification |
| Ref | `UNI-0004` §6 |

---

## CONFLICT-11 — `AI_CONTEXT` script inventory is stale

| Field | Value |
|---|---|
| Sources | `AI_CONTEXT/PROJECT_STATE.md` claims "**41 .cs files in 8 subdirectories**" and its folder tree **omits `Environment/`** · measured filesystem 2026-09-29 |
| Resolution | **RESOLVED** — **42 `.cs` files in 10 subdirectories**: Core 5 · Cover 2 · Enemies 9 · Environment 2 · Level 4 · Performance 1 · Player 10 · Procedural 3 · UI 4 · Weapons 2 |
| Also stale | `AI_CONTEXT/ARCHITECTURE.md` line counts: `EnemyTacticalPlanner` claims 1469 → **measured 1 416**; `EnemySpawner` 1498 → **1 623**; `WaveManager` 149 → **162**; `CoverSystem` 510 → **514** |
| Correct | `EnemyController` 3 738 · `EnemyTacticalVision` 2 268 · `EnemyHearing` 342 · `NoiseSystem` 557 · `ArenaTacticalMap` 1 770 · `CoverPoint` 486 |
| Ref | `../UNI/UNI-0001-UNITY-PROJECT-STATE.md` §5.1 |

---

## CONFLICT-12 — XP / progression: "no evidence" vs. confirmed-and-implemented

| Field | Value |
|---|---|
| Sources | Phase 2 canonical docs recorded XP / level-up / upgrades as "**no evidence of any kind**" · `AI_CONTEXT/CONFIRMED_STATE.md` lists them as **user-CONFIRMED** · 5 scripts exist in `Assets/Scripts/Player/` |
| Resolution | **PARTIALLY RESOLVED** — the *code exists* and was *historically confirmed*; current behaviour is still `NOT VERIFIED` because U-12 (2026-09-29) did not exercise it |
| Phase 2 error | The "no evidence of any kind" claim was **too strong** and is corrected in `../GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` §3.1 |
| Residual | XP accumulation, level-up and upgrade application are untested; the HUD readout is broken (`UNI-D13`) |
| Ref | `../GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` §3.1 |

---

## CONFLICT-13 — Mobile UI: "CONFIRMED" vs. null touch-control references

| Field | Value |
|---|---|
| Sources | `AI_CONTEXT/CONFIRMED_STATE.md`: "Мобильное UI (joystick, touch controls) — **CONFIRMED**" · U-12 runtime audit 2026-09-29: all `MobileTouchControls` references are **`null`** (`UNI-D07`) |
| Resolution | **OPEN** — the two records are from different sessions |
| Hypotheses (none verified) | (a) a **regression** after the confirmation; (b) the confirmation covered the **joystick only**, not `MobileTouchControls`; (c) the `MobileUI` object exists (2 references in the canonical scene) but was never fully wired |
| Evidence | `AI_CONTEXT/PROJECT_STATE.md` records that on 2026-09-27 `MobileUI` was **not** disabled wholesale and `MobileTouchControls` was **not** modified — so the object survived that edit |
| Why it matters | Android viability depends on this. `UNI-D07` remains the current-state truth |
| Ref | `../UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md` `UNI-D07` |

---

## CONFLICT-14 — Spawn zone count 53 vs 52

| Field | Value |
|---|---|
| Sources | `AI_CONTEXT/CURRENT_TASK.md` expected-behaviour chain: `SpawnZones=53` after generation · U-12 runtime measured **52** |
| Resolution | **OPEN** — one-off discrepancy, immaterial to gameplay |
| Note | Not a defect. Recorded so a future comparison is not confused by it |
| Ref | `../GAME/GAME-0001-GAME-DESIGN-STATE.md` §7.1 |

---

## CONFLICT-15 — Platform priority: Android vs Windows

| Field | Value |
|---|---|
| Sources | `AI_CONTEXT/RULES.md`: "**Android** — основная платформа. **Windows** — вторичная" · canonical `UNI-0001`/`GAME-0003` treat **Windows as primary dev target**, Android secondary |
| Resolution | **OPEN** — no `DEC-` entry exists for platform priority |
| Impact | Android is blocked (`UNI-D07`, `UNI-D14`), so the recorded primary platform is the **less** developed one. This is a real planning conflict |
| Ref | `../UNI/UNI-0001-UNITY-PROJECT-STATE.md` §1 |

---

## CONFLICT-16 — Protected files are already modified

| Field | Value |
|---|---|
| Sources | `AI_CONTEXT/RULES.md` lists `EnemyTacticalPlanner.cs`, `ArenaGenerator.cs`, `EnemyDirectionIndicator.cs` (and 11 others) as **not changeable without explicit permission** · `git status` shows all three **modified in the working tree**, uncommitted |
| Resolution | **OPEN** — the modifications are dated 2026-09-27/28 and were evidently made under a different authorisation path (Blender/3D art branch and visual-fix phase) |
| Risk | The protection rule and the actual working state disagree. A future agent reading only `RULES.md` would be misled about what is safe to touch |
| Ref | `../GAME/GAME-0004-REQUIREMENTS-AND-DESIGN-DECISIONS.md` §5.2 |

---

## Cross-references

- `DOC-0001` §1 — evidence precedence
- `OPEN_DECISIONS.md` — decisions needed
- `Documentation/Archive/Historical/RECONCILIATION_SOURCE_INVENTORY.md` §13 — correction table

---

**End of `OPEN_CONFLICTS.md`**
