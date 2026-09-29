# CONSOLIDATION VALIDATION REPORT

**Document ID:** `DOC-VALIDATION-2026-09-29`
**Phase:** 2 — Documentation Consolidation
**Status:** COMPLETE — **no commit, no push, no merge**
**Date:** 2026-09-29

---

## 1. Scope of this phase

Documentation consolidation only. This phase:

- inventoried every documentation, art, backup and build source
- proved the art delta after the recovery point
- resolved the canonical scene
- created the canonical `Documentation/` tree
- added `SUPERSEDED` headers to legacy documents
- recorded decisions, risks, conflicts and issues

**It did not** modify Unity content, C#, scenes, prefabs, materials, Blender
sources, FBX, or character art. It did not commit, push, merge, rebase, reset,
clean, or stash. It did not delete anything.

---

## 2. Constraint compliance

| # | Constraint | Result |
|---|---|---|
| 1 | No Unity/C#/Scene/Prefab/Material changes | **PASS** — 0 edits under `Assets/`, `Packages/`, `ProjectSettings/` |
| 2 | No Blender/FBX/character-art changes | **PASS** — 0 edits outside the project |
| 3 | No AI/navigation/gameplay changes | **PASS** |
| 4 | No legacy MD deleted | **PASS** — 14 files retained, 12 headers prepended |
| 5 | No `git pull` / `merge` / `rebase` / `reset` / `clean` / `stash` | **PASS** |
| 6 | No asset promoted to `APPROVED` or beyond | **PASS** — highest state remains `QA PASS` |
| 7 | `.gitignore` unchanged | **PASS** — 0 changes |
| 8 | Nothing staged | **PASS** — 0 staged entries |
| 9 | No commit / no push | **PASS** — HEAD unchanged at `7ad314c` |
| 10 | No secret read, printed, or copied | **PASS** — see §7 |

---

## 3. Files created

### 3.1 Root-level evidence (3)

| File | Bytes |
|---|---|
| `ART_DELTA_AFTER_RECOVERY.md` | 5 458 |
| `RECONCILIATION_SOURCE_INVENTORY.md` | 16 305 |
| `PROJECT_TRUTH.md` | 7 462 |

### 3.2 `Documentation/` tree (25 files, 6 folders)

```
Documentation/
  DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md      7 650
  DOC-0002-SUPERSEDED-DOCUMENTS.md               3 780
  DOC-0003-IDENTIFIER-SYSTEM.md                  3 317
  DOC-0004-OPEN-QUESTIONS.md                     3 053
  ART/  ART-0001-ART-STATE.md                    9 071
         ART-0002-ART-ASSET-REGISTER.md          4 814
         ART-0003-ART-PIPELINE-AND-QA-GATES.md   4 989
  UNI/  UNI-0001-UNITY-PROJECT-STATE.md          3 152
         UNI-0002-UNITY-DATA-AND-ASSET-REGISTER  2 881
         UNI-0003-UNITY-KNOWN-DEFECTS.md         5 155
         UNI-0004-UNITY-ENVIRONMENT-AND-TOOLCHAIN 3 021
  GAME/ GAME-0001-GAME-DESIGN-STATE.md           3 090
         GAME-0002-GAMEPLAY-SYSTEMS-STATE.md     3 484
         GAME-0003-CONTROLS-AND-PLATFORM-TARGETS 2 449
  AI/   AI-0001-AI-AGENT-STATE.md                4 074
         AI-0002-AI-TOOLING-AND-PRODUCTION-METHOD 3 609
  REL/  REL-0001-RECOVERY-AND-BACKUP.md          4 989
         REL-0002-BUILD-ARTIFACTS.md             3 240
  History/
         HISTORY-0001-TIMELINE.md                6 781
         HISTORY-0002-LEGACY-STAGE-NUMBERING.md  3 172
         HISTORY-0003-AUDIT-HISTORY.md           4 733
         OPEN_DECISIONS.md                       4 876
         OPEN_RISKS.md                           5 619
         OPEN_CONFLICTS.md                       4 458
         OPEN_ISSUES.md                          3 970
```

---

## 4. Files modified (12) — headers only

All 12 are in `AI_CONTEXT/`. Each received a prepended `SUPERSEDED` blockquote
**above its original H1**. No original line was removed or altered.

| File | Supersede ID |
|---|---|
| `AI_CONTEXT/ART_PIPELINE.md` | S-01 |
| `AI_CONTEXT/AI_TOOLCHAIN_AUDIT.md` | S-02 |
| `AI_CONTEXT/ARCHITECTURE.md` | S-05 |
| `AI_CONTEXT/CHANGELOG.md` | S-05 |
| `AI_CONTEXT/CONFIRMED_STATE.md` | S-05 |
| `AI_CONTEXT/CURRENT_TASK.md` | S-05 |
| `AI_CONTEXT/DEBUGGING.md` | S-05 |
| `AI_CONTEXT/PROJECT_STATE.md` | S-05 |
| `AI_CONTEXT/README.md` | S-05 |
| `AI_CONTEXT/RULES.md` | S-05 |
| `AI_CONTEXT/SPEC_IDEAS.md` | S-05 |
| `AI_CONTEXT/VERIFICATION.md` | S-05 |

> **Note:** 8 of these 12 were already modified before this phase. This phase
> changed 4 additional files (`ARCHITECTURE.md`, `CHANGELOG.md`,
> `CURRENT_TASK.md`, `DEBUGGING.md`) plus the 4 that were already dirty.

### 4.1 Not modified — deliberate

The four `CHATGPT_*.md` files in `~/Downloads` are **outside the
project and outside the workspace**. They are recorded as superseded
(`DOC-0002` S-06…S-09) but were **not** edited, so no user file outside the
repository was touched.

---

## 5. Git state — unchanged except for new untracked files

| Field | Value |
|---|---|
| Branch | `main` |
| HEAD | `7ad314c869e5dad1daf8e56fdf0b8f4197592969` (**unchanged**) |
| `origin/main` | `f932fcc722d22b8150ccf7e9616914324347c6e8` (unchanged) |
| Staged | **0** |
| Status entries | **90** |

### 5.1 Before vs. after this phase

| State | Before | After | Delta |
|---|---|---|---|
| total entries | 82 | 90 | **+8** |
| modified (`M`) | 15 | 19 | **+4** |
| deleted (`D`) | 2 | 2 | 0 |
| untracked (`??`) | 65 | 69 | **+4** |
| staged | 0 | 0 | 0 |

### 5.2 The +8 accounted for

| New state | Entries | Cause |
|---|---|---|
| `??` | +3 | `ART_DELTA_AFTER_RECOVERY.md`, `RECONCILIATION_SOURCE_INVENTORY.md`, `PROJECT_TRUTH.md` |
| `??` | +1 | `Documentation/` (whole new tree, git collapses it to one entry) |
| `M` | +4 | `AI_CONTEXT/{ARCHITECTURE,CHANGELOG,CURRENT_TASK,DEBUGGING}.md` — were clean, now carry headers |

**No other file changed state.** The 7 `Assets/` modified files, the 64
untracked `Assets/` files, the 2 deletions and the `Working/` entry were all
present before this phase and are untouched by it.

---

## 6. Validation checks

### 6.1 Art delta

| Check | Result |
|---|---|
| Snapshot paths present in live tree | 873/873 **PASS** |
| Size drift | 0 **PASS** |
| Deleted art files | 0 **PASS** |
| Art files modified after cutoff 2026-09-29 16:42:59 | 0 **PASS** |
| Documentation-only post-cutoff writes excluded correctly | **PASS** |
| Snapshot SHA256 | `9ABE45EE…7513` **PASS** |
| Content spot-checks | 12/12 **PASS** |
| **Declared delta** | **ZERO** |

### 6.2 Canonical scene

| Check | Result |
|---|---|
| Build Settings entry | `Assets/Scenes/TestArena.unity` **enabled** |
| Runtime load | **active scene** |
| mtime | 23:08:42 > 22:46:04 |
| GUID | distinct, no collision |
| Git | tracked vs. untracked |
| **Decision** | `Assets/Scenes/TestArena.unity` **PASS** |

### 6.3 Source discovery

| Check | Result |
|---|---|
| In-project docs inventoried | **PASS** |
| GitHub-only docs retrieved externally | **PASS** (2 files) |
| ChatGPT docs | 4 found, 1 missing — recorded **PASS** |
| OpenCode / project skills | **PASS** |
| External character art source found | **PASS** (307 files) |
| Vest/glove QA collage found | **PASS** (25 files — corrected in Phase 2.5; Phase 2 recorded 24) |
| 2026-09-22 `Z:` backup found | **PASS** (90 626 files) |
| Build artifacts found | **PASS** (APK + IL2CPP) |
| Out-of-scope sources excluded | **PASS** (VPN docs) |
| Secret-suspect file flagged, not read | **PASS** |

### 6.4 Documentation integrity

| Check | Result |
|---|---|
| Every canonical doc has an ID from `DOC-0003` | **PASS** |
| Every doc has a status and date | **PASS** |
| Evidence precedence stated in `DOC-0001` | **PASS** |
| Superseded register covers all 12 modified files | **PASS** |
| Asset state machine never exceeded by this phase | **PASS** |
| Open decisions recorded (10) | **PASS** |
| Open risks recorded (9) | **PASS** |
| Open conflicts recorded (10) | **PASS** |
| Open issues recorded (12) | **PASS** |

---

## 7. Security handling

| Item | Action |
|---|---|
| user Desktop secret-suspect file (233 B, 2026-09-18) | **Not read. Not printed. Not copied. Not hashed. Not committed.** Recorded as `RISK-02`; exact path held only in `Documentation/REL/REL-0001-RECOVERY-AND-BACKUP.md` §5, which is the security register |
| Secrets in any created document | **None.** No key, token, password, or credential value appears in any of the 28 files written |
| 2026-09-22 `Z:` manifest | Declares `SECRETS: EXCLUDED` — honoured |
| GitHub-only doc retrieval | Public repository content only |

---

## 8. Corrections issued to prior audits

| # | Prior claim | Correction | Where recorded |
|---|---|---|---|
| 1 | `WWG_Doomy_Cowboy_REFINED.blend` not found | **Exists** in `Documents\WildWestGunslinger art` | `CONFLICT-01`, `HISTORY-0003` §1 |
| 2 | Character sources "in project / in progress" | Complete, dated, outside the project | `ART-0001` §2–3 |
| 3 | No release build exists | `WildWest.apk` 46.2 MB, 2026-09-09 | `CONFLICT-04`, `REL-0002` |
| 4 | `Chest = 141 cm` | **100** (size-50 table) | `CONFLICT-02`, `ART-0001` §3.1 |
| 5 | `Skeleton = 62 bones` | **51 bones** | `CONFLICT-03`, `ART-0001` §3.1 |
| 6 | One backup (2026-09-29) | Two backups; 2026-09-22 is 11.87 GB at `Z:` | `CONFLICT-05`, `REL-0001` |
| 7 | Canonical scene ambiguous | `Assets/Scenes/TestArena.unity` | `CONFLICT-06`, `UNI-0002` §1 |

Corrections 4 and 5 invalidate the basis of Audit 2's "foundation identified"
conclusion, which must be re-derived from `ART-0001` §3.1.

---

## 9. Outstanding — requires human decision

10 decisions, 9 risks, 10 conflicts, 12 issues are open. See
`Documentation/History/OPEN_*.md`.

**Highest priority:**

| # | Item | Ref |
|---|---|---|
| 1 | Back up the character art — 307 files exist in **no** verified backup | `RISK-01` |
| 2 | Decide the canonical character foundation file | `DEC-02` |
| 3 | Enumerate the 12 broken refs and 4 missing scripts | `ISSUE-01`, `ISSUE-02` |
| 4 | Resolve Git divergence before any history operation | `DEC-07` |
| 5 | Decide the material policy | `DEC-01` |

---

## 10. Phase 2 completion statement

Phase 2 is **complete**. 28 Markdown files were written (3 root evidence + 25
canonical), 12 legacy documents received `SUPERSEDED` headers, and the art delta
after the recovery point was proven to be **zero**.

**Nothing was committed. Nothing was pushed. No product content was changed.
No legacy file was deleted. No secret was read.**

The next action requires explicit human approval (`DEC-07`) before any commit.

---

**End of `CONSOLIDATION_VALIDATION_REPORT.md`**
