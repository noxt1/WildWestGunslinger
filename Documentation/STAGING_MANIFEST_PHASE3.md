# STAGING MANIFEST — PHASE 3 (REVISED)

**Document ID:** `DOC-STAGING-P3-2026-09-29-r2`
**Status:** PREPARED — accurate to the real diff
**Date:** 2026-09-29
**Supersedes:** the r1 manifest, which incorrectly described 8 `AI_CONTEXT`
files as "header-only"

---

## 1. Why this manifest was rewritten

The r1 manifest claimed all 12 `AI_CONTEXT` files carried **headers only**.
Diffing them proved otherwise: **8 of the 12 also contain 763 lines of
pre-existing project documentation work** dated 2026-09-27/28, authored before
Phase 2. Committing them would have buried that work inside a commit authorised
on a false premise.

Per the Phase 2.5 rule *"pre-existing entries must not be swept into the
consolidation commit"*, those **8 files are excluded**. Their working-tree
content is **not modified** by this phase — it stays uncommitted for its own
deliberate commit.

## 2. Gate status

| Item | Status |
|---|---|
| `DEC-07` | **✅ CONFIRMED 2026-09-29** — land documentation commit as-is; divergence deferred; GitHub-only MD not merged |
| Pre-staging staged count | **0** (all 52 from the aborted attempt were unstaged via `git restore --staged`; working tree untouched) |

---

## 3. Candidates — 44 files

### 3.1 New canonical documentation — 36 files

| # | File | Reason | Type | Approved |
|---|---|---|---|---|
| 1 | `Documentation/README.md` | **Canonical entry point** | new | YES |
| 2 | `Documentation/PROJECT_STATE.md` | Navigation step 3 | new | YES |
| 3 | `Documentation/ARCHITECTURE.md` | Navigation step 4 | new | YES |
| 4 | `Documentation/REQUIREMENTS.md` | Navigation step 5 | new | YES |
| 5 | `Documentation/DECISIONS.md` | Navigation step 6 | new | YES |
| 6 | `Documentation/MASTER_PLAN.md` | Navigation step 7 | new | YES |
| 7 | `Documentation/CONSOLIDATION_PRECOMMIT_VALIDATION.md` | Phase 2.5 validation | new | YES |
| 8 | `Documentation/STAGING_MANIFEST_PHASE3.md` | This manifest | new | YES |
| 9 | `Documentation/DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md` | Index + precedence | new | YES |
| 10 | `Documentation/DOC-0002-SUPERSEDED-DOCUMENTS.md` | Supersession register | new | YES |
| 11 | `Documentation/DOC-0003-IDENTIFIER-SYSTEM.md` | Identifier scheme | new | YES |
| 12 | `Documentation/DOC-0004-OPEN-QUESTIONS.md` | Open questions | new | YES |
| 13 | `Documentation/ART/ART-0001-ART-STATE.md` | Art state; **§3 `Working/blender_src/` (7 working `.blend`)**; **§4.1–4.4 character** | new | YES |
| 14 | `Documentation/ART/ART-0002-ART-ASSET-REGISTER.md` | **§4 `BARREL_01`/`FENCE_01` APPROVED; §5 `CRATE_01` CP1/CP2 not authorized** | new | YES |
| 15 | `Documentation/ART/ART-0003-ART-PIPELINE-AND-QA-GATES.md` | **§4a `Col = FLOAT_COLOR` precedent and material rules** | new | YES |
| 16 | `Documentation/UNI/UNI-0001-UNITY-PROJECT-STATE.md` | Runtime-verified Unity state | new | YES |
| 17 | `Documentation/UNI/UNI-0002-UNITY-DATA-AND-ASSET-REGISTER.md` | Data/asset register | new | YES |
| 18 | `Documentation/UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md` | **§1.0 FIX-01/FIX-02 2026-09-27 runtime evidence; 14 defects; §1.1 RT→UNI map** | new | YES |
| 19 | `Documentation/UNI/UNI-0004-UNITY-ENVIRONMENT-AND-TOOLCHAIN.md` | Editor, platforms, Git | new | YES |
| 20 | `Documentation/GAME/GAME-0001-GAME-DESIGN-STATE.md` | Implemented design state | new | YES |
| 21 | `Documentation/GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` | ✅/❌/❓ per system | new | YES |
| 22 | `Documentation/GAME/GAME-0003-CONTROLS-AND-PLATFORM-TARGETS.md` | Controls, Android blocker | new | YES |
| 23 | `Documentation/GAME/GAME-0004-REQUIREMENTS-AND-DESIGN-DECISIONS.md` | 35 recovered requirements + `DD-01`…`DD-06` | new | YES |
| 24 | `Documentation/AI/AI-0001-AI-AGENT-STATE.md` | Trust model, skill verification | new | YES |
| 25 | `Documentation/AI/AI-0002-AI-TOOLING-AND-PRODUCTION-METHOD.md` | Reconciled method | new | YES |
| 26 | `Documentation/REL/REL-0001-RECOVERY-AND-BACKUP.md` | Backup topology | new | YES |
| 27 | `Documentation/REL/REL-0002-BUILD-ARTIFACTS.md` | Build artifacts, staleness | new | YES |
| 28 | `Documentation/REL/REL-0003-SECURITY-AND-RELEASE-REQUIREMENTS.md` | 57 recovered STAGE-19 requirements | new | YES |
| 29 | `Documentation/Character/CHARACTER_SOURCE_BACKUP.md` | `RISK-01` closure evidence | new | YES |
| 30 | `Documentation/History/HISTORY-0001-TIMELINE.md` | Timeline | new | YES |
| 31 | `Documentation/History/HISTORY-0002-LEGACY-STAGE-NUMBERING.md` | Legacy Stage history | new | YES |
| 32 | `Documentation/History/HISTORY-0003-AUDIT-HISTORY.md` | Audit history + corrections | new | YES |
| 33 | `Documentation/History/OPEN_DECISIONS.md` | **`DEC-05` corrected; `DEC-07` CONFIRMED** | new | YES |
| 34 | `Documentation/History/OPEN_RISKS.md` | 9 risks | new | YES |
| 35 | `Documentation/History/OPEN_CONFLICTS.md` | 10 conflicts | new | YES |
| 36 | `Documentation/History/OPEN_ISSUES.md` | **RT-01…RT-09 mapping** | new | YES |

### 3.2 New root evidence — 4 files

| # | File | Reason | Type | Approved |
|---|---|---|---|---|
| 37 | `PROJECT_TRUTH.md` | **Consolidated ground truth** | new | YES |
| 38 | `ART_DELTA_AFTER_RECOVERY.md` | Zero art-delta proof | new | YES |
| 39 | `RECONCILIATION_SOURCE_INVENTORY.md` | Source discovery record | new | YES |
| 40 | `CONSOLIDATION_VALIDATION_REPORT.md` | Phase 2 validation | new | YES |

### 3.3 Modified `AI_CONTEXT` — 4 files, **header-only, verified**

These 4 were **clean in Git before Phase 2**. Their only change is the prepended
`SUPERSEDED` blockquote. Their diffs were verified to contain **0 pre-existing
content**.

| # | File | Reason | Change | Pre-existing content | Approved |
|---|---|---|---|---|---|
| 41 | `AI_CONTEXT/AI_TOOLCHAIN_AUDIT.md` | Header (S-02) + false-claim warning | modified | **0** | YES |
| 42 | `AI_CONTEXT/DEBUGGING.md` | Header | modified | **0** | YES |
| 43 | `AI_CONTEXT/README.md` | Header | modified | **0** | YES |
| 44 | `AI_CONTEXT/SPEC_IDEAS.md` | Header | modified | **0** | YES |

---

## 4. EXCLUDED — 8 pre-existing-dirty `AI_CONTEXT` files

**Not staged. Not committed. Working-tree content left exactly as it is.**

| File | added lines | my header | **pre-existing** |
|---|---|---|---|
| `AI_CONTEXT/ART_PIPELINE.md` | 225 | 8 | **217** |
| `AI_CONTEXT/CHANGELOG.md` | 181 | 5 | **176** |
| `AI_CONTEXT/CURRENT_TASK.md` | 172 | 7 | **165** |
| `AI_CONTEXT/PROJECT_STATE.md` | 92 | 5 | **87** |
| `AI_CONTEXT/ARCHITECTURE.md` | 77 | 6 | **71** |
| `AI_CONTEXT/RULES.md` | 28 | 5 | **23** |
| `AI_CONTEXT/CONFIRMED_STATE.md` | 24 | 7 | **17** |
| `AI_CONTEXT/VERIFICATION.md` | 14 | 7 | **7** |
| **Total** | **814** | **50** | **763** |

### 4.1 What that pre-existing content contains

It is **not** superseded filler. It is substantive, and Phase 3 has already
reconciled it into canonical documentation:

| Source content | Reconciled into |
|---|---|
| `BARREL_01` / `FENCE_01` **FINAL / APPROVED** (user-confirmed) | `ART-0002` §4 |
| `CRATE_01` CP1 complete, **CP2 NOT AUTHORIZED** | `ART-0002` §5, `DEC-05` |
| Blender asset registry + working-`.blend` paths | `ART-0001` §3.1 |
| `Col = FLOAT_COLOR` material precedent, metal/wood values, UV rules, FBX contract | `ART-0003` §4a |
| 2026-09-27 HUD overlay fix, FPS 60, 0 console errors | `UNI-0003` §1.0 FIX-01 |
| 2026-09-27 aliasing A/B tests | `UNI-0003` §1.0 FIX-02 |
| 3D-ring / world-space enemy direction indicator | recorded in `ART_PIPELINE` / `ARCHITECTURE` (still excluded from this commit) |

> These 8 files remain **modified and uncommitted**. They should be reviewed and
> committed separately by the owner. Their `SUPERSEDED` headers are already in
> the working tree, so the canonical documentation is self-consistent whether or
> not they are committed.

---

## 5. EXCLUDED — 74 PRODUCT STATE entries

| Category | Count | Notes |
|---|---|---|
| Modified scenes | 2 | `Assets/Scenes/TestArena.unity`, `TestArena_Preview.unity` |
| Modified C# | 5 | includes `EnemyTacticalPlanner.cs`, `ArenaGenerator.cs`, `EnemyDirectionIndicator.cs` |
| Modified render settings | 2 | `Assets/Settings/Mobile_RPAsset.asset`, `PC_RPAsset.asset` |
| Deleted product files | 2 | `EnemyTacticalEnvironmentScanner_TEST.cs` (+ `.meta`) |
| Untracked `Assets/Art/**` | — | modular FBX, props FBX (`DEC-04`, `DEC-05`) |
| Untracked `Assets/Materials/WWG_*` | — | `DEC-01`, `DEC-10` |
| Untracked `Assets/Prefabs/Environment/WWG_*` | — | `DEC-10` |
| Untracked `Assets/Screenshots/**` | — | review separately |
| Untracked `Assets/TestArena.unity` (+ meta) | — | duplicate scene, `DEC-09` |
| Untracked `Working/**` | — | includes the 7 production working `.blend` |
| `.gitignore` | 0 changes | must remain unchanged |
| `Packages/`, `ProjectSettings/` | 0 changes | must remain unchanged |

---

## 6. Prohibitions observed

| Prohibition | Verified |
|---|---|
| `git add .` | **not used** — every file added individually by path |
| `git add -A` | **not used** |
| Directory-wide staging | **not used** |
| Staging a file absent from this manifest | **not done** |
| `git commit` before cached-diff verification | **not done** |
| `git push` / `merge` / `rebase` / `reset` / `clean` / `stash` / force ops | **none performed** |
| `amend` / `squash` | **not used** |

> `git restore --staged` was used **once**, to clear the aborted 52-file staging.
> It affects the **index only** — no working-tree file was altered, and no Git
> history was touched.

---

## 7. Expected commit

| Property | Value |
|---|---|
| Message | `docs: consolidate canonical project documentation` |
| Files | **44** (40 new + 4 modified) |
| Product files | **0** |
| Non-`.md` files | **0** |
| Deletions in AI_CONTEXT diffs | **0** (headers are pure insertions) |

---

**End of `STAGING_MANIFEST_PHASE3.md`**
