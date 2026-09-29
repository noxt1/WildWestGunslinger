# WildWestGunslinger — Documentation

**Entry point for all project documentation.**
**Status:** CANONICAL
**Date:** 2026-09-29

If you are an agent or a new contributor, **start here.**

---

## 0. CURRENT PROJECT DOCUMENTATION vs ARCHIVE

This documentation set has **two strictly separate layers**.

### CURRENT / CANONICAL

The documents in sections 2–4 below. They are the **operational source of truth** for ChatGPT,
OpenCode, technical tasks, architectural decisions and current project state. Use these.

> **Current documentation is authoritative for active work.**

### ARCHIVE

`Archive/` — preserved history and source material. It is **not** part of current instructions.

> **Archived documentation is preserved for historical context and must not be applied automatically.**

> **Explicit rule:** Archived documentation is preserved for history and context only. It is not an
> operational source of truth and **must not be applied to the current project** unless a canonical
> document explicitly references it as historical evidence.

A new agent reads CURRENT documentation for instructions, and consults `Archive/` only to understand
a past decision, a previous state, or the origin of a current rule.

**`Archive/` is not a security boundary.** This repository is public: anything moved there remains
readable on GitHub with its full history. Archiving is about *meaning*, not about hiding anything.

The plan that decided every placement is `DOCUMENTATION_ARCHIVE_MIGRATION_MAP.md`.
Archive rules are explained in `Archive/README.md`.

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
  9. domains:  Character  GAME  UNI  ART  AI  REL  History
 10. Archive/                      history and source material only
```

Steps 3–7 live at `Documentation/` root. Step 8 is
`Documentation/History/OPEN_ISSUES.md`. Step 9 is the domain folders. Step 10 is **not** current
instruction — see section 0.

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
| 8 | `AI_PRODUCTION_METHODOLOGY.md` | **APPLY** — binding production and agent-execution rules (recon → baseline → approval → controlled production → checkpoint → numeric QA → visual QA → export → re-import → final audit → human approval → git) |
| 9 | `DOC-0002-SUPERSEDED-DOCUMENTS.md` | **APPLY** — supersession policy and the register of superseded documents |
| 10 | `History/OPEN_ISSUES.md` | open defects, missing artefacts, evidence gaps |

> **Scope rule.** `AI_PRODUCTION_METHODOLOGY.md` defines **how** work is produced;
> `PROJECT_TRUTH.md` defines **what is true**. Neither replaces the other, and neither replaces
> `MASTER_PLAN.md`.

---

## 4. Domain folders

Domain documents use the project's identifier scheme (`DOC-0003-IDENTIFIER-SYSTEM.md`).
The folders are named by that scheme, not by generic topic names, so that the `GAME-0002` /
`UNI-0003` style references used throughout the documentation keep working.

| Domain topic | Folder | Prefix | Entry document | Read it for |
|---|---|---|---|---|
| Art pipeline, QA gates, asset lifecycle | `ART/` | `ART-` | `ART-0001-ART-STATE.md` | art state, asset register, pipeline and QA gates |
| Unity project, tooling, defects | `UNI/` | `UNI-` | `UNI-0001-UNITY-PROJECT-STATE.md` | Unity runtime state, asset register, known defects, toolchain |
| **Combat**, **Weapons**, **Progression**, **UI**, **Enemy**, **Environment** | `GAME/` | `GAME-` | `GAME-0001-GAME-DESIGN-STATE.md` | design state, gameplay systems (damage, firing, projectile, XP, upgrades, arena, enemy spawning), controls and platform targets, requirements |
| AI architecture and agent tooling | `AI/` | `AI-` | `AI-0001-AI-AGENT-STATE.md` | AI agent state, tooling and production method |
| **Character** | `Character/` | — | `CHARACTER_SOURCE_BACKUP.md` | character source lineage, rig, backup state |
| Recovery, builds, security & release | `REL/` | `REL-` | `REL-0001-RECOVERY-AND-BACKUP.md` | backups, build artefacts, security/release requirements |
| Timeline, audit history, open registers | `History/` | `HISTORY-`, `OPEN_` | `HISTORY-0001-TIMELINE.md` | timeline, legacy numbering, audit history, and the **live** `OPEN_ISSUES` / `OPEN_DECISIONS` / `OPEN_CONFLICTS` / `OPEN_RISKS` registers |
| Cross-project index | root | `DOC-` | `DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md` | canonical document index |
| **History and source material only** | `Archive/` | — | `Archive/README.md` | archived audits, historical snapshots, superseded manifests — **not** current instruction |

> **Note on domain folders.** `GAME-0002` covers several of the topics above (combat, weapons,
> progression, UI, enemy, environment). It is deliberately **not** split or duplicated across
> multiple folders; read it as the single gameplay-systems document. The mapping is recorded in
> `DOCUMENTATION_ARCHIVE_MIGRATION_MAP.md`.

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
| Canonical documentation consolidation | **COMMITTED** — see `Archive/Audits/PHASE5_CHECKPOINT_REPORT.md` |
| Technical baseline (Phases 5A–5D) | **FROZEN** — art production pause for integration |
| GitHub reconciliation | **RECONCILED LOCALLY, NOT PUSHED** — `PHASE6_GITHUB_RECONCILIATION_REPORT.md` |
| Secret exposure in repo/docs | **PASS** — 0 findings; scan method independently validated |
| Weapon damage decision (`DEC-11`) | **OPEN** — deliberately undecided |
| Android device runtime (`RT-02`) | **NOT VERIFIED** — no device available |
| Level readout UI (`RT-07`) | **OPEN** — absent; XP bar itself works |
| Character Foundation | **NOT integrated** into Unity |
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

| File | Role | Layer |
|---|---|---|
| `../PROJECT_TRUTH.md` | consolidated ground truth | CURRENT |
| `../AI_CONTEXT/` | operational layer for agent working rules | CURRENT (operational) |
| `../Working/reports/*.md` | 22 Blender QA reports | CURRENT (source, uncommitted) |
| `Archive/Audits/AUDIT_2_RUNTIME_REPORT.md` | runtime audit — historical highest-authority evidence | ARCHIVE |
| `Archive/Historical/ART_DELTA_AFTER_RECOVERY.md` | proof of zero art change after the recovery point | ARCHIVE |
| `Archive/Historical/Documentation/Archive/Historical/RECONCILIATION_SOURCE_INVENTORY.md` | every source discovered on the machine | ARCHIVE |
| `Archive/Historical/RECOVERY_POINT_REPORT.md` | 2026-09-29 recovery verification | ARCHIVE |
| `Archive/Audits/AUDIT_2_RECONCILIATION_REPORT.md` | Audit 2 documentation reconciliation | ARCHIVE |
| `Archive/Audits/CONSOLIDATION_PRECOMMIT_VALIDATION.md` | Phase 2.5 validation (risk closure + pre-commit) | ARCHIVE |
| `AI_PRODUCTION_METHODOLOGY.md` | 52-point production methodology — **APPLY** | CURRENT |
| `Archive/Audits/AUDIT_SESSION_CONTEXT_2026-09-28.md` | audit/session preservation record | ARCHIVE |
| `DOC-0002-SUPERSEDED-DOCUMENTS.md` | supersession policy + superseded register — **APPLY** | CURRENT |
| `FINAL_DOCUMENTATION_CLASSIFICATION.md` | final CURRENT/ARCHIVE classification record | CURRENT |
| `DOCUMENTATION_ARCHIVE_MIGRATION_MAP.md` | what moved where, and why | CURRENT |
| `STAGING_MANIFEST_PHASE6_1.md` | Phase 6.1 staging manifest | CURRENT |
| `PHASE6_1_DOCUMENTATION_STRUCTURE_REPORT.md` | Phase 6.1 report | CURRENT |
| `PHASE6_PUSH_PREVIEW.md` | pre-push preview (**push not performed**) | CURRENT |

---

**End of `Documentation/README.md`**
