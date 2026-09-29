# STAGING MANIFEST — PHASE 6.1

**Status:** PREPARED
**Date:** 2026-09-29
**Purpose:** exact list of documentation-only changes staged for the Phase 6.1 commit.

`git add .` / `git add -A` were **not** used. No product file is included.

## A. Moved documents (22) — all `git mv`, all recorded as renames

| From | To | Class |
|---|---|---|
| `Documentation/PHASE3_COMMIT_REPORT.md` | `Documentation/Archive/Audits/` | AUDIT |
| `Documentation/PHASE4_AI_CONTEXT_RECONCILIATION.md` | `Documentation/Archive/Audits/` | AUDIT |
| `Documentation/PHASE5A_CORE_INTEGRITY_REPORT.md` | `Documentation/Archive/Audits/` | AUDIT |
| `Documentation/PHASE5B_GUN_DAMAGE_REPORT.md` | `Documentation/Archive/Audits/` | AUDIT |
| `Documentation/PHASE5B1_GUNCONTROLLER_IDENTITY_REPORT.md` | `Documentation/Archive/Audits/` | AUDIT |
| `Documentation/PHASE5C_HUD_XP_REPORT.md` | `Documentation/Archive/Audits/` | AUDIT |
| `Documentation/PHASE5D_MOBILE_ANDROID_REPORT.md` | `Documentation/Archive/Audits/` | AUDIT |
| `Documentation/PHASE5R_UNITY_RUNTIME_CHANNEL_REPORT.md` | `Documentation/Archive/Audits/` | AUDIT |
| `Documentation/PHASE5_CHECKPOINT_REPORT.md` | `Documentation/Archive/Audits/` | AUDIT |
| `Documentation/CONSOLIDATION_PRECOMMIT_VALIDATION.md` | `Documentation/Archive/Audits/` | AUDIT |
| `AUDIT_2_RECONCILIATION_REPORT.md` (root) | `Documentation/Archive/Audits/` | AUDIT |
| `AUDIT_2_RUNTIME_REPORT.md` (root) | `Documentation/Archive/Audits/` | AUDIT |
| `CONSOLIDATION_VALIDATION_REPORT.md` (root) | `Documentation/Archive/Audits/` | AUDIT |
| `AUDIT_SESSION_CONTEXT_2026-09-28.md` (root) | `Documentation/Archive/Audits/` | AUDIT |
| `Documentation/AI_CONTEXT_LOSS_CHECK.md` | `Documentation/Archive/Historical/` | HISTORICAL |
| `ART_DELTA_AFTER_RECOVERY.md` (root) | `Documentation/Archive/Historical/` | HISTORICAL |
| `RECONCILIATION_SOURCE_INVENTORY.md` (root) | `Documentation/Archive/Historical/` | HISTORICAL |
| `RECOVERY_POINT_REPORT.md` (root) | `Documentation/Archive/Historical/` | HISTORICAL |
| `Documentation/DOC-0002-SUPERSEDED-DOCUMENTS.md` | `Documentation/Archive/Legacy/` | LEGACY |
| `AI_PRODUCTION_METHODOLOGY.md` (root) | `Documentation/Archive/Source/` | SOURCE |
| `Documentation/STAGING_MANIFEST_PHASE3.md` | `Documentation/Archive/Superseded/` | SUPERSEDED |
| `Documentation/STAGING_MANIFEST_PHASE4.md` | `Documentation/Archive/Superseded/` | SUPERSEDED |

## B. New documents (4)

| Path | Purpose |
|---|---|
| `Documentation/Archive/README.md` | archive semantics — §23 |
| `Documentation/DOCUMENTATION_ARCHIVE_MIGRATION_MAP.md` | migration plan — §4 |
| `Documentation/STAGING_MANIFEST_PHASE6_1.md` | this file — §30 |
| `Documentation/PHASE6_1_DOCUMENTATION_STRUCTURE_REPORT.md` | phase report — §32 |

## C. Canonical documents updated

**Entry point and rules (§16–§22):**
`Documentation/README.md` · `PROJECT_TRUTH.md` · `Documentation/DECISIONS.md` ·
`Documentation/REQUIREMENTS.md` · `Documentation/MASTER_PLAN.md` ·
`Documentation/History/OPEN_ISSUES.md`

**Link repairs (§24)** — references to moved documents corrected in current docs only:
`AI_CONTEXT/` ×12 (`AI_TOOLCHAIN_AUDIT`, `ARCHITECTURE`, `ART_PIPELINE`, `CHANGELOG`,
`CONFIRMED_STATE`, `CURRENT_TASK`, `DEBUGGING`, `PROJECT_STATE`, `README`, `RULES`, `SPEC_IDEAS`,
`VERIFICATION`) · `Documentation/AI/AI-0001` · `Documentation/ART/ART-0001`, `ART-0003` ·
`Documentation/Character/CHARACTER_SOURCE_BACKUP.md` · `Documentation/DOC-0001` ·
`Documentation/ARCHITECTURE.md` · `Documentation/GAME/GAME-0004` ·
`Documentation/History/HISTORY-0001`, `HISTORY-0002`, `HISTORY-0003`, `OPEN_CONFLICTS`,
`OPEN_DECISIONS` · `Documentation/PHASE6_GITHUB_RECONCILIATION_REPORT.md`

## D. Explicitly NOT staged

| Item | Reason |
|---|---|
| `Assets/**` (scenes, C#, prefabs, materials, RP assets) | pre-existing product work — §31 |
| `Working/` (762 files) | untracked production sources; uncommitted by decision |
| `.opencode/skills/wwg-character-art/SKILL.md` | pre-existing untracked, unrelated to this phase |
| `AI_CONTEXT/CHARACTER_FOUNDATION_CONTRACT.md` | pre-existing untracked, unrelated to this phase |
| 2 × `EnemyTacticalEnvironmentScanner_TEST.cs(.meta)` deletion | pre-existing product deletion, not documentation |
| `Documentation/Archive/**` internal historical references | deliberately **not** rewritten — would falsify history |

## E. Expected diff shape

| Metric | Expected |
|---|---|
| renames | 22 (history preserved) |
| documentation deletions | **0** |
| product files | **0** |
| new documents | 4 |
