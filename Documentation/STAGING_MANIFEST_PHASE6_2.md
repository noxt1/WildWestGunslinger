# STAGING MANIFEST — PHASE 6.2

**Status:** PREPARED
**Date:** 2026-09-29
**Scope:** documentation only. `git add .` / `git add -A` **not** used.

## A. Promotions (2) — `git mv`, recorded as `R100` (content unchanged)

| From | To | Reason |
|---|---|---|
| `Documentation/Archive/Source/AI_PRODUCTION_METHODOLOGY.md` | `Documentation/AI_PRODUCTION_METHODOLOGY.md` | CASE A — current specification, must be APPLIED |
| `Documentation/Archive/Legacy/DOC-0002-SUPERSEDED-DOCUMENTS.md` | `Documentation/DOC-0002-SUPERSEDED-DOCUMENTS.md` | §7 audit — self-declared `CANONICAL`, live supersession policy |

## B. New documents (3)

| Path | Purpose |
|---|---|
| `Documentation/FINAL_DOCUMENTATION_CLASSIFICATION.md` | §11 governing classification record |
| `Documentation/Archive/Source/README.md` | keeps the empty `Source/` folder in a fresh clone; explains the promotion |
| `Documentation/Archive/Legacy/README.md` | keeps the empty `Legacy/` folder in a fresh clone; explains the promotion |

## C. Modified documents (3)

| Path | Change |
|---|---|
| `Documentation/AI_PRODUCTION_METHODOLOGY.md` | **authority header only** (Status/Role/Authority/Note) — 52 sections of body text untouched |
| `Documentation/README.md` | §0 governing rule stated verbatim; methodology + DOC-0002 added to the Current table; Archive table corrected; §10 links updated |
| `AI_CONTEXT/AI_TOOLCHAIN_AUDIT.md`, `AI_CONTEXT/DEBUGGING.md`, `AI_CONTEXT/README.md`, `AI_CONTEXT/SPEC_IDEAS.md` | link repair: old archive path → new current path (4 files) |

## D. Explicitly NOT staged

| Item | Reason |
|---|---|
| `Assets/**` — 10 tracked modifications | pre-existing product work |
| 66 untracked entries (art, prefabs, materials, `Working/`) | pre-existing / uncommitted by decision |
| 17 unstaged `Documentation/` files | foreign art-branch work (51→53 bone corrections etc.) — deliberately left out |
| `.opencode/skills/wwg-character-art/SKILL.md`, `AI_CONTEXT/CHARACTER_FOUNDATION_CONTRACT.md` | pre-existing untracked, unrelated to this phase |
| `Documentation/Archive/**` internal historical references | not rewritten — would falsify history |

## E. Expected diff shape

| Metric | Expected |
|---|---|
| renames | 2 (`R100`) |
| new documents | 3 |
| modified documents | 6 |
| documentation deletions | **0** |
| product files | **0** |
