# DOC-0002 — Superseded Documents

**Document ID:** `DOC-0002`
**Status:** CANONICAL
**Date:** 2026-09-29

---

## 1. Supersession policy

A document is **superseded** when a canonical `Documentation/` entry covers its
subject with better evidence. Superseded documents are **retained, never
deleted**. A minimal `SUPERSEDED` header points to the replacement.

**Superseded does not mean wrong.** It means "not authoritative; use the
replacement for current state". Historical claims remain useful for history.

---

## 2. Superseded register

| # | Document | Superseded by | Reason |
|---|---|---|---|
| S-01 | `ART_PIPELINE.md` | `ART-0001`, `ART-0002`, `ART-0003` | Stage-numbered, conflicting, predates measured data; contains a vest-experiment claim not matching the found `Коллаж тест VVG` source |
| S-02 | `AI_TOOLCHAIN_AUDIT.md` | `AI-0001`, `AI-0002`, `REL-0002` | Contains three verified false claims (no release build; character sources "in project"; stale toolchain conclusions) |
| S-03 | `AI_PRODUCTION_METHODOLOGY.md` | `AI-0002` | GitHub-only; methodology structure preserved, state claims corrected |
| S-04 | `AUDIT_SESSION_CONTEXT_2026-09-28.md` | `History/HISTORY-0001`, `ART-0001` | GitHub-only session log; still the best record of the 2026-09-22…28 period |
| S-05 | `AI_CONTEXT/` (12 files) | whole `Documentation/` tree | Multiple modified/conflicted files, overlapping scope, no single authority |
| S-06 | `CHATGPT_CHECKLIST.md` | `GAME-0001`, `GAME-0002`, `History/HISTORY-0002` | Stale Stage numbering; claims not runtime-verified |
| S-07 | `CHATGPT_CHECKLIST_before_STAGE19_2026-09-20.md` | `History/HISTORY-0002` | Historical only |
| S-08 | `CHATGPT_PROJECT_STATE.md` | `PROJECT_TRUTH.md` | GPT state claims contradicted by runtime evidence |
| S-09 | `CHATGPT_WORK_QUEUE.md` | `History/OPEN_DECISIONS.md`, `History/OPEN_ISSUES.md` | Not authoritative for sequencing |
| S-10 | `.opencode/skills/wwg-character-art/SKILL.md` — **state sections** | `ART-0001` | Asserts rig/character state that does not exist in the build. **Method sections remain valid** — see `AI-0001` §2 |
| S-11 | `AUDIT_2_RECONCILIATION_REPORT.md` — **§chest/skeleton figures** | `ART-0001` §4.1 | `Chest=141cm` and `Skeleton=62 bones` are both false. **The report is retained as audit evidence**; only those figures are corrected |

---

## 3. Not superseded — retained as evidence

| Document | Role |
|---|---|
| `AUDIT_2_RECONCILIATION_REPORT.md` | static audit evidence (with the S-11 correction) |
| `AUDIT_2_RUNTIME_REPORT.md` | runtime audit evidence — authoritative for runtime facts |
| `RECOVERY_POINT_REPORT.md` | recovery verification evidence |
| `ART_DELTA_AFTER_RECOVERY.md` | zero-delta proof |
| `RECONCILIATION_SOURCE_INVENTORY.md` | source discovery record |
| `Working/reports/*.md` (10 files) | Blender QA reports |
| Audit 1 | **conversation-only** — never written to disk; referenced from `History/HISTORY-0003` |

---

## 4. Absent documents

| Referenced | Status |
|---|---|
| `Вставленная уценка.md` | not found anywhere searched |
| Audit 1 standalone report | conversation history only |
| Authoritative game design document | **does not exist**; `GAME-0001` is the first |
| CI/build pipeline definition | does not exist |
| Test suite | does not exist |

---

## 5. Header format applied

Each superseded document receives a minimal header, inserted before its first
line, without altering any existing content:

```markdown
> **SUPERSEDED** — not authoritative. Canonical replacement:
> `Documentation/<DOMAIN>/<FILE>.md`. Retained for history; see
> `Documentation/DOC-0002-SUPERSEDED-DOCUMENTS.md` (S-NN).
> Superseded on 2026-09-29.
```

---

**End of `DOC-0002-SUPERSEDED-DOCUMENTS.md`**
