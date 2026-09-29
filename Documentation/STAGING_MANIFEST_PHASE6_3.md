# STAGING MANIFEST — PHASE 6.3

**Status:** PREPARED
**Date:** 2026-09-29
**Scope:** documentation only. `git add .` / `git add -A` **not** used.

## A. Moved documentation (12) — all `git mv`, all recorded `R100` (content unchanged)

| From | To |
|---|---|
| `Documentation/GAME/GAME-0003-CONTROLS-AND-PLATFORM-TARGETS.md` | `Documentation/UI/` |
| `Documentation/ART/ART-0001-ART-STATE.md` | `Documentation/Art/` |
| `Documentation/ART/ART-0002-ART-ASSET-REGISTER.md` | `Documentation/Art/` |
| `Documentation/ART/ART-0003-ART-PIPELINE-AND-QA-GATES.md` | `Documentation/Art/` |
| `Documentation/UNI/UNI-0001-UNITY-PROJECT-STATE.md` | `Documentation/Engineering/` |
| `Documentation/UNI/UNI-0002-UNITY-DATA-AND-ASSET-REGISTER.md` | `Documentation/Engineering/` |
| `Documentation/UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md` | `Documentation/Engineering/` |
| `Documentation/UNI/UNI-0004-UNITY-ENVIRONMENT-AND-TOOLCHAIN.md` | `Documentation/Engineering/` |
| `Documentation/REL/REL-0001-RECOVERY-AND-BACKUP.md` | `Documentation/Engineering/` |
| `Documentation/REL/REL-0002-BUILD-ARTIFACTS.md` | `Documentation/Engineering/` |
| `Documentation/REL/REL-0003-SECURITY-AND-RELEASE-REQUIREMENTS.md` | `Documentation/Engineering/` |
| `Documentation/PHASE6_PUSH_PREVIEW.md` | `Documentation/Archive/Audits/` (Phase 6 housekeeping) |

Empty directory shells `Documentation/UNI/` and `Documentation/REL/` removed (git never tracked
empty directories).

## B. New index files (6) — §18

`Documentation/Art/README.md` · `Documentation/AI/README.md` · `Documentation/Character/README.md` ·
`Documentation/UI/README.md` · `Documentation/Engineering/README.md` · `Documentation/GAME/README.md`

## C. New reports (2)

`Documentation/PHASE6_3_FINAL_STRUCTURE_REPORT.md` · this manifest

## D. Modified (4)

| Path | Change |
|---|---|
| `Documentation/README.md` | CORE / DOMAINS / ARCHIVE architecture block (§17); link-defect fixes; full domain paths; push-preview reference repointed to the archive |
| `AI_CONTEXT/AI_TOOLCHAIN_AUDIT.md` | domain path links |
| `AI_CONTEXT/DEBUGGING.md` | domain path links |
| `AI_CONTEXT/ARCHITECTURE.md` | domain path links |

## E. Explicitly NOT staged

| Item | Reason |
|---|---|
| `Assets/**` | pre-existing product work — §26 |
| `Working/` | untracked production sources, uncommitted by decision |
| 17 unstaged `Documentation/` files | foreign art-branch work — deliberately left out |
| `Documentation/STAGING_MANIFEST_PHASE6_1.md`, `Documentation/DOCUMENTATION_ARCHIVE_MIGRATION_MAP.md` | historical Phase 6.1 records; old domain paths deliberately **not** rewritten (§20) |
| `.opencode/skills/wwg-character-art/SKILL.md`, `AI_CONTEXT/CHARACTER_FOUNDATION_CONTRACT.md` | pre-existing untracked, unrelated |
| `Documentation/Archive/**` internal historical references | not rewritten |

## F. Expected diff shape

| Metric | Expected |
|---|---|
| renames | 12 (`R100`) |
| new files | 8 |
| modified files | 4 |
| documentation deletions | **0** |
| product files | **0** |
| secrets | **0** |