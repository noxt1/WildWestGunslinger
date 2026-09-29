# DOCUMENTATION ARCHIVE MIGRATION MAP

**Status:** CANONICAL — planning record for the Phase 6.1 restructure
**Date:** 2026-09-29

## Classification vocabulary

| Class | Meaning |
|---|---|
| `CANONICAL` | authoritative cross-project truth — operational source of truth |
| `ACTIVE DOMAIN` | current domain knowledge, identified by the project's ID scheme |
| `OPERATIONAL` | current working state of an in-flight process |
| `AUDIT` | completed audit / verification evidence, retained as history |
| `HISTORICAL` | superseded snapshot kept for provenance |
| `SUPERSEDED` | officially replaced by a canonical document |
| `SOURCE` | preserved source material, not current operational documentation |
| `LEGACY` | fully replaced legacy documentation |

## Migration table

| Current Path | Document | Classification | Destination | Reason | History Required |
|---|---|---|---|---|---|
| `PROJECT_TRUTH.md` *(repo root)* | Consolidated project truth | CANONICAL | **stays at repo root** | referenced as `../PROJECT_TRUTH.md` by `Documentation/README.md`; root is the first thing an agent sees | yes |
| `Documentation/README.md` | Entry point | CANONICAL | stays | §16 makes it the entry point | yes |
| `Documentation/PROJECT_STATE.md` | Domain state pointers | CANONICAL | stays | cross-project | yes |
| `Documentation/ARCHITECTURE.md` | System architecture | CANONICAL | stays | cross-project | yes |
| `Documentation/REQUIREMENTS.md` | Requirements register | CANONICAL | stays | cross-project | yes |
| `Documentation/DECISIONS.md` | Approved decisions | CANONICAL | stays | cross-project | yes |
| `Documentation/MASTER_PLAN.md` | Current roadmap | CANONICAL | stays | cross-project | yes |
| `Documentation/DOC-0001-…INDEX.md` | Canonical index | CANONICAL | stays | cross-project index | yes |
| `Documentation/DOC-0003-IDENTIFIER-SYSTEM.md` | ID scheme definition | CANONICAL | stays | defines `GAME-`, `UNI-`, `ART-`… prefixes used in 30+ cross-references | yes |
| `Documentation/DOC-0004-OPEN-QUESTIONS.md` | Open questions | CANONICAL | stays | live register | yes |
| `Documentation/PHASE6_GITHUB_RECONCILIATION_REPORT.md` | Reconciliation result | OPERATIONAL | stays | describes current unpushed state | yes |
| `Documentation/PHASE6_PUSH_PREVIEW.md` | Push preview | OPERATIONAL | stays | describes current unpushed state | yes |
| `Documentation/GAME/*` (4) | Game design + systems + controls + requirements | ACTIVE DOMAIN | stays | identifier scheme; cited by ID | yes |
| `Documentation/UNI/*` (4) | Unity state / assets / defects / toolchain | ACTIVE DOMAIN | stays | identifier scheme; `UNI-D13` etc. cited widely | yes |
| `Documentation/REL/*` (3) | Recovery, build artefacts, security/release | ACTIVE DOMAIN | stays | identifier scheme | yes |
| `Documentation/ART/*` (3) | Art state / asset register / pipeline+QA gates | ACTIVE DOMAIN | stays | identifier scheme | yes |
| `Documentation/AI/*` (2) | AI agent state, tooling method | ACTIVE DOMAIN | stays | identifier scheme | yes |
| `Documentation/Character/*` (1) | Character source backup | ACTIVE DOMAIN | stays | domain dir already correct | yes |
| `Documentation/History/*` (7) | Timeline, legacy numbering, audit history, `OPEN_ISSUES` / `OPEN_DECISIONS` / `OPEN_CONFLICTS` / `OPEN_RISKS` | ACTIVE DOMAIN | stays | the four `OPEN_*` registers are live working documents, not history | yes |
| `AI_PRODUCTION_METHODOLOGY.md` *(root)* | 52-point art production methodology | SOURCE | `Documentation/Archive/Source/` | GitHub-only doc (§15); specification not yet canonicalised; no canonical overlap | yes |
| `AUDIT_SESSION_CONTEXT_2026-09-28.md` *(root)* | Audit/session preservation record | AUDIT | `Documentation/Archive/Audits/` (§14 recommended location) | GitHub-only; self-declares it must not replace canonical docs; security-reviewed clean | yes |
| `ART_DELTA_AFTER_RECOVERY.md` *(root)* | Art delta after recovery | HISTORICAL | `Documentation/Archive/Historical/` | one-shot recovery snapshot, superseded by `ART-0002` register | yes |
| `AUDIT_2_RECONCILIATION_REPORT.md` *(root)* | Audit 2 reconciliation | AUDIT | `Documentation/Archive/Audits/` | completed audit, marked HISTORICAL | yes |
| `AUDIT_2_RUNTIME_REPORT.md` *(root)* | Audit 2 runtime | AUDIT | `Documentation/Archive/Audits/` | completed audit, marked HISTORICAL | yes |
| `CONSOLIDATION_VALIDATION_REPORT.md` *(root)* | Consolidation validation | AUDIT | `Documentation/Archive/Audits/` | completed validation | yes |
| `RECONCILIATION_SOURCE_INVENTORY.md` *(root)* | Source inventory | HISTORICAL | `Documentation/Archive/Historical/` | point-in-time inventory | yes |
| `RECOVERY_POINT_REPORT.md` *(root)* | Recovery point report | HISTORICAL | `Documentation/Archive/Historical/` | one-shot recovery record | yes |
| `Documentation/DOC-0002-SUPERSEDED-DOCUMENTS.md` | Superseded register | LEGACY | `Documentation/Archive/Legacy/` | a register *about* superseded docs; keeps superseded material discoverable | yes |
| `Documentation/AI_CONTEXT_LOSS_CHECK.md` | AI_CONTEXT loss check | HISTORICAL | `Documentation/Archive/Historical/` | completed check, `COMPLETE` | yes |
| `Documentation/CONSOLIDATION_PRECOMMIT_VALIDATION.md` | Pre-commit validation | AUDIT | `Documentation/Archive/Audits/` | completed validation | yes |
| `Documentation/PHASE3_COMMIT_REPORT.md` | Phase 3 commit report | AUDIT | `Documentation/Archive/Audits/` | completed phase evidence | yes |
| `Documentation/PHASE4_AI_CONTEXT_RECONCILIATION.md` | Phase 4 reconciliation | AUDIT | `Documentation/Archive/Audits/` | completed phase evidence | yes |
| `Documentation/PHASE5A_CORE_INTEGRITY_REPORT.md` | 5A core integrity | AUDIT | `Documentation/Archive/Audits/` | completed phase evidence | yes |
| `Documentation/PHASE5B_GUN_DAMAGE_REPORT.md` | 5B gun damage | AUDIT | `Documentation/Archive/Audits/` | completed phase evidence | yes |
| `Documentation/PHASE5B1_GUNCONTROLLER_IDENTITY_REPORT.md` | 5B.1 controller identity | AUDIT | `Documentation/Archive/Audits/` | completed phase evidence | yes |
| `Documentation/PHASE5C_HUD_XP_REPORT.md` | 5C HUD/XP | AUDIT | `Documentation/Archive/Audits/` | completed phase evidence | yes |
| `Documentation/PHASE5D_MOBILE_ANDROID_REPORT.md` | 5D mobile/Android | AUDIT | `Documentation/Archive/Audits/` | completed phase evidence | yes |
| `Documentation/PHASE5R_UNITY_RUNTIME_CHANNEL_REPORT.md` | 5R runtime channel | AUDIT | `Documentation/Archive/Audits/` | completed phase evidence | yes |
| `Documentation/PHASE5_CHECKPOINT_REPORT.md` | Phase 5 checkpoint | AUDIT | `Documentation/Archive/Audits/` | completed checkpoint | yes |
| `Documentation/STAGING_MANIFEST_PHASE3.md` | Phase 3 staging manifest | SUPERSEDED | `Documentation/Archive/Superseded/` | manifest for a completed, superseded staging | yes |
| `Documentation/STAGING_MANIFEST_PHASE4.md` | Phase 4 staging manifest | SUPERSEDED | `Documentation/Archive/Superseded/` | manifest for a completed, superseded staging | yes |

## Deliberate deviations from the §5 target structure

§5 permits deviation where the inventory shows objective necessity. Three deviations, each with evidence:

| §5 target | Actual | Reason |
|---|---|---|
| `Documentation/PROJECT_TRUTH.md` | stays at **repo root** | `Documentation/README.md` links it as `../PROJECT_TRUTH.md`; moving it breaks that link and removes the first document a new agent sees. |
| `Documentation/OPEN_ISSUES.md` (+ `OPEN_DECISIONS`, `OPEN_CONFLICTS`, `OPEN_RISKS`) | stay in `Documentation/History/` | these are **live working registers**, not history. The `History/` prefix is a misnomer; the documents themselves are canonical. Moving them would misrepresent live state as historical. |
| `Enemy/`, `Combat/`, `Environment/`, `Weapons/`, `Progression/`, `UI/` domain dirs | **not created** | §5 forbids creating folders for count. Their content already lives in `GAME-0001/0002/0003/0004` and `UNI-*`; splitting would duplicate documents (§15). The README provides an explicit **domain map** instead. |

`ART/`, `AI/`, `Character/` already exist and match §7 naming, so they are kept as-is.

## Domain map (§7 coverage without duplication)

| Domain | Authoritative document(s) |
|---|---|
| Character | `Character/CHARACTER_SOURCE_BACKUP.md` |
| Enemy | `GAME/GAME-0001-GAME-DESIGN-STATE.md`, `GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md`, `AI/*` |
| Combat | `GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` (damage, firing, projectile) |
| Environment | `GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` (arena), `UNI/UNI-0002-UNITY-DATA-AND-ASSET-REGISTER.md` |
| Weapons | `GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md`, `Archive/Source/AI_PRODUCTION_METHODOLOGY.md` |
| Progression | `GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` (XP, upgrades, levels) |
| UI | `GAME/GAME-0003-CONTROLS-AND-PLATFORM-TARGETS.md`, `GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` |
| Art | `ART/ART-0001..0003`, `Archive/Source/AI_PRODUCTION_METHODOLOGY.md` |
| AI | `AI/AI-0001`, `AI/AI-0002`, `GAME/GAME-0002` |

## Move method

`git mv` only (§25) — preserves file history. No delete + recreate. No deletion of any document
in this phase.
