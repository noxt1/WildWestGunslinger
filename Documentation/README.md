# WildWestGunslinger — Documentation

**Entry point for all project documentation.**
**Status:** CANONICAL
**Date:** 2026-09-29

If you are an agent or a new contributor, **start here.**

---

## 1. The one-paragraph truth

WildWestGunslinger is an **unreleased, pre-alpha, third-person western arena
shooter** built in Unity 6 for Windows PC with Android as a secondary target. The
arena generates at runtime (9 rooms, 71 walls, 148 cover objects), enemies spawn
and path with a custom A\* implementation, and both weapon prefabs fire. It is
**not shippable**: 7 critical defects are open, there is no character rig or
animation, no NavMesh, no Android input, and no build pipeline. Character art
exists in a complete, measured, **verified-backed-up** form but is **not
integrated** into Unity.
Two assets are **approved** (`BARREL_01`, `FENCE_01` — user-confirmed); nothing is
promoted, integrated or runtime-verified. Nothing is approved **for release**.

---

## 2. Navigation path

Follow this chain. Each step is short and each links onward.

```
  1. README.md                     ← you are here
  2. PROJECT_TRUTH.md              consolidated ground truth
  3. PROJECT_STATE.md              domain state pointers
  4. ARCHITECTURE.md               how the game is put together
  5. REQUIREMENTS.md               what must be true (not what is)
  6. DECISIONS.md                  what has been decided / left open
  7. MASTER_PLAN.md                sequencing, derived from reality
  8. OPEN_ISSUES.md                unresolved defects and gaps
  9. domains:  ART-  UNI-  GAME-  AI-  REL-  DOC-  History
```

Steps 3–7 live at `Documentation/` root. Step 8 is
`Documentation/History/OPEN_ISSUES.md`. Step 9 is the domain folders.

---

## 3. Root entry documents

| # | Document | Read it for |
|---|---|---|
| 1 | `README.md` | this page — orientation |
| 2 | `../PROJECT_TRUTH.md` | **the consolidated truth.** Scene, Git, art, backups, builds, decisions |
| 3 | `PROJECT_STATE.md` | per-domain state pointers and one-line status per system |
| 4 | `ARCHITECTURE.md` | runtime architecture, data flow, system boundaries |
| 5 | `REQUIREMENTS.md` | requirements, explicitly separated from current state |
| 6 | `DECISIONS.md` | decided items and the open decision register |
| 7 | `MASTER_PLAN.md` | dependency-ordered path from current state to shippable |
| 8 | `History/OPEN_ISSUES.md` | open defects, missing artefacts, evidence gaps |

---

## 4. Domain folders

| Folder | Prefix | Entry document |
|---|---|---|
| `ART/` | `ART-` | `ART-0001-ART-STATE.md` |
| `UNI/` | `UNI-` | `UNI-0001-UNITY-PROJECT-STATE.md` |
| `GAME/` | `GAME-` | `GAME-0001-GAME-DESIGN-STATE.md` |
| `AI/` | `AI-` | `AI-0001-AI-AGENT-STATE.md` |
| `REL/` | `REL-` | `REL-0001-RECOVERY-AND-BACKUP.md` |
| `Character/` | `CHAR-` | `CHARACTER_SOURCE_BACKUP.md` |
| `History/` | `HISTORY-`, `OPEN_` | `HISTORY-0001-TIMELINE.md` |
| root | `DOC-` | `DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md` |

---

## 5. How to read any claim in this documentation set

**Evidence precedence — highest first.** When two sources disagree, the higher
rank wins and the conflict is logged in `History/OPEN_CONFLICTS.md`.

| Rank | Evidence class |
|---|---|
| 1 | Runtime observation in Unity |
| 2 | Verified file facts (hash, GUID, size, timestamp) |
| 3 | Git history |
| 4 | Backup manifests |
| 5 | Measured art documents |
| 6 | Project documentation (this tree) |
| 7 | ChatGPT / AI prose — **advisory only** |
| 8 | Assumptions — must be labelled |

### Classification vocabulary

Every substantial claim should fall into one of these:

`VERIFIED FACT` · `VERIFIED ABSENCE` · `RUNTIME VERIFIED` · `STATIC VERIFIED` ·
`NOT VERIFIED` · `REQUIREMENT` · `DECISION` · `PLAN` · `IDEA` · `OPEN ISSUE` ·
`CONFLICTED` · `HISTORICAL` · `SUPERSEDED`

### Asset state machine

```
CREATED → QA PASS → APPROVED → PROMOTED → INTEGRATED → RUNTIME VERIFIED
```

**Nothing in this project is above `QA PASS`.** A `FINAL` or `APPROVAL_CANDIDATE`
word in a *filename* is not an approval.

---

## 6. Five distinctions that must never be blurred

These are the errors that produced every correction in the audit history.

| Never confuse | Because |
|---|---|
| **EXTERNAL SOURCE EXISTS** vs **UNITY INTEGRATED** | 307 character-art files exist and are backed up; 0 are integrated. The scene shows capsules |
| **STATICALLY EXISTS** vs **RUNTIME VERIFIED** | `WallSegment_01_FINAL.fbx` is on disk; it is not in the scene and imports at 0.01× |
| **ASSET QA PASS** vs **APPROVED** vs **PROMOTED** | `BARREL_01`/`FENCE_01` are `APPROVED` only; `CRATE_01` is `QA PASS` with **CP2 not authorized**; **nothing** is promoted, integrated or runtime-verified |
| **AI/legacy claim** vs **evidence** | Three audits produced false claims; all were corrected |
| **PLAN / REQUIREMENT** vs **IMPLEMENTATION** | 57 security/release requirements exist; 0 are implemented |

---

## 7. Current blocking status

| Item | Status |
|---|---|
| Character art preservation | **CLOSED** — 332/332 files, full SHA256 verified, restore-tested |
| Secret exposure in repo/docs | **PASS** — 0 findings across 731 files |
| Consolidation commit | **AWAITING APPROVAL** — nothing staged, nothing committed |
| Open decisions | **10** — all require human authority |
| Open critical defects | **7** |
| Ready to ship | **NO** |

---

## 8. Path policy

Canonical documents use **repository-relative paths** (`Assets/…`,
`Documentation/…`, `Working/…`).

Absolute machine paths appear **only** where they are necessary evidence:
recovery and backup locations, external source locations, and technical
diagnostics. External resources are labelled as such. Personal Desktop items are
not enumerated.

---

## 9. Standing rules

1. **Documentation never mutates product content.** Art, Unity, C#, scenes,
   prefabs, materials and Blender sources are documented, not fixed, by
   documentation work.
2. **Never promote an asset** past `APPROVED` — that is a human act.
3. **Never infer runtime behaviour.** Test it or mark it `NOT VERIFIED`.
4. **Never delete** a duplicate or legacy artefact. Mark it `REVIEW`.
5. **Never read, print, hash, copy or commit** a suspected secret. Metadata only.
6. **Never claim** a test-stage document as authority over runtime evidence.

---

## 10. Related documents outside this folder

| File | Role |
|---|---|
| `../PROJECT_TRUTH.md` | consolidated ground truth |
| `../ART_DELTA_AFTER_RECOVERY.md` | proof of zero art change after the recovery point |
| `../RECONCILIATION_SOURCE_INVENTORY.md` | every source discovered, anywhere on the machine |
| `../CONSOLIDATION_VALIDATION_REPORT.md` | Phase 2 validation |
| `CONSOLIDATION_PRECOMMIT_VALIDATION.md` | Phase 2.5 validation (risk closure + pre-commit) |
| `../RECOVERY_POINT_REPORT.md` | 2026-09-29 recovery verification |
| `../AUDIT_2_RUNTIME_REPORT.md` | runtime audit — highest-authority evidence |
| `../Working/reports/*.md` | 10 Blender QA reports |

---

**End of `Documentation/README.md`**
