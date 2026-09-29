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

## Cross-references

- `DOC-0001` §1 — evidence precedence
- `OPEN_DECISIONS.md` — decisions needed
- `RECONCILIATION_SOURCE_INVENTORY.md` §13 — correction table

---

**End of `OPEN_CONFLICTS.md`**
