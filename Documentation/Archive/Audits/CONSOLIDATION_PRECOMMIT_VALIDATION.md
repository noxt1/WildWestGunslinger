# CONSOLIDATION PRE-COMMIT VALIDATION

**Document ID:** `DOC-PRECOMMIT-2026-09-29`
**Phase:** 2.5 — Risk Closure + Pre-Commit Validation
**Status:** COMPLETE
**Date:** 2026-09-29
**Commit performed:** **NONE** · **Push performed:** **NONE** · **Staged files:** **0**

---

## A. Character Source

### A.1 Source verified

**External resource:** `~/Documents/WildWestGunslinger art`
**Absolute:** `C:\Users\cyril\Documents\WildWestGunslinger art`

| Metric | Value |
|---|---|
| File count | **307** |
| Total size | **180.2 MB** |
| Newest file | 2026-09-26 12:02 |
| `.blend` sources | 43 |
| `.fbx` exports | 28 |
| `.png` QA renders | 117 |
| `.jpg` reference images | 36 |
| Measurement `.md` docs | 4 |
| Reference-provenance `.md` | 1 |

### A.2 Backup verified

| Field | Value |
|---|---|
| Date/time | **2026-09-29 17:06:06** |
| Archive | `~/WildWestGunslinger_RECOVERY/character_source_20260929_170606/wwg_character_source_20260929_170606.zip` |
| Archive size | 178 198 663 B (169.94 MB) |
| Archive SHA256 | `FE53758A0B92D27D399C5CED2243F8D0CD4F674B544EE52E98E16C61615B63EF` |
| Full manifest | `character_source_manifest_20260929_170606.csv` — path, size, SHA256, source, mtime for **all** files |
| Contents | `Characters/` 307 + `Collage/` 25 = **332** entries |
| Uncompressed | 213 482 332 B (203.6 MB) |

**Independence:** outside the Git repository · separate from Recovery Point
2026-09-29 · separate from the 2026-09-22 `Z:` backup.

**Non-destructive:** originals were read only. Nothing moved, renamed, modified
or deleted.

### A.3 Verification result

Method: SHA256 computed for every source file, archive created, archive
**re-opened read-only**, every entry **streamed back out** and re-hashed from
the archived bytes, then compared per file on presence + size + hash.

| Check | Expected | Actual | Result |
|---|---|---|---|
| Source file count | 332 | 332 | **PASS** |
| Backup entry count | 332 | 332 | **PASS** |
| Verified (SHA256 + size) | 332 | **332** | **PASS** |
| Missing | 0 | 0 | **PASS** |
| Extra | 0 | 0 | **PASS** |
| Hash mismatches | 0 | 0 | **PASS** |
| Size mismatches | 0 | 0 | **PASS** |
| Source total bytes | 213 482 332 | 213 482 332 | **PASS** |
| Backup uncompressed bytes | 213 482 332 | 213 482 332 | **PASS** |

> **BACKUP VERIFIED** — full 332/332 verification, not a sample.

**Restore test:** archive fully extracted to a temporary directory, 5 critical
files re-hashed — **5/5 PASS**. Temporary directory removed.

### A.4 Critical hashes

| File | Bytes | SHA256 |
|---|---|---|
| `WWG_Doomy_Cowboy_REFINED.blend` | 2 360 988 | `47332B9B06953CBC9786C85262E06BC649E247371A6F923440C3AC88CA5D9269` |
| `WWG_Foundation_Source.blend` | 1 704 329 | `6B0EC5D3AE837586C2AAE3CB4412A6B15316EAD46226B841DCF67B399BBEC7FF` |
| `Doomy_measurements.md` | 2 705 | `481B46FB67D948DF36800A90C2F5771B51587774CCDB39800464C47616386F34` |
| `_REFERENCES/SOURCES.md` | 11 205 | `D975465DC447D9F55D536C7B6A0144CD101A409ED57B8374C6605D85418D71` |
| `Unity_Export/Characters/WWG_Player.fbx` | 1 230 860 | `0772946CA4A4DA17337C3F9BFC4F6A4005DDD70E2EFE466FAC68AE91F5399A06` |

Full 64-char hashes for all 332 files are in the CSV manifest.

**`WWG_Template_Armature`:** no standalone file exists. It is an object inside
the `WWG_Foundation_Source.blend` lineage, which is backed up and hash-verified.
**Consequence:** a backup excluding `.blend` would have lost the rig.

### A.5 Remaining risk

| # | Limitation | Status |
|---|---|---|
| L1 | Archive is on the same machine as the source (`C:` → `C:`) | Protects against deletion/edit, **not** machine or disk failure |
| L2 | Not on `Z:`, not cloud-confirmed | `RISK-04` remains **OPEN** |
| L3 | Canonical character variant still undecided | `DEC-02` — a *selection* problem, not preservation |
| L4 | `.blend1` and duplicate `FBX/`/`Unity_Export/` preserved as-is | Deliberate; pruning is a human decision |
| L5 | Git still does not track character art | `DEC-03` |

> **`RISK-01`: CLOSED.**

---

## B. Security

### B.1 Secret-suspect file

| Field | Value |
|---|---|
| Existence | **PRESENT** |
| Size | 233 B |
| Last modified | 2026-09-18 |
| Location | user Desktop — exact path held only in `REL-0001-RECOVERY-AND-BACKUP.md` §5, the security register |
| **Contents accessed** | **NO — never read, never printed, never copied, never hashed, never quoted, never indexed** |
| Remediation | Metadata-only handling applied; recorded as `RISK-02`; permanent handling rule in `DECISIONS.md` `GD-08` |

> **No secret value appears in this report or in any documentation file.**

### B.2 Secret exposure scan

15 pattern classes: cloud access keys, secret access keys, VCS personal/fine-grained
tokens, LLM API keys, OAuth/slack tokens, private-key blocks, JWTs, bearer tokens,
URL-embedded credentials, connection-string credentials, generic key/token/secret
assignments, password assignments, key-file extensions.

| Scope | Files scanned | High-confidence findings |
|---|---|---|
| Project Markdown (all, incl. 34 canonical docs) | 57 | **0** |
| Git-tracked text files | 674 | **0** |
| Staged files | 0 | **0** |
| **Total** | **731** | **0** |

**Result: PASS.**

### B.3 Honest scope limit

The scan covered the **working tree**. It did **not**:

- walk full **Git history** (requirement `REL-0003` 19.1.3 remains **not fully closed**)
- unpack or inspect the 2026-09-09 **APK** (requirement 19.1.4 **not started**)
- inspect the 12.16 GB `Z:` backup contents

### B.4 Remediation result

| Action | Result |
|---|---|
| Secrets found in documentation | **none** — no remediation needed |
| Machine-specific secret path minimised | **done** — removed from `CONSOLIDATION_VALIDATION_REPORT.md` and `RECONCILIATION_SOURCE_INVENTORY.md`; retained only in the security register |
| Unrelated personal Desktop items | **removed** from the canonical inventory (previously itemised 4 personal/unrelated paths) |
| Security scanning | **performed** — recorded as `REL-0003` §3 with explicit partial-closure wording |

---

## C. Documentation

### C.1 Files reviewed — 34 canonical documents

| Group | Files | Corrections applied |
|---|---|---|
| `Documentation/` root | 8 (`README`, `PROJECT_STATE`, `ARCHITECTURE`, `REQUIREMENTS`, `DECISIONS`, `MASTER_PLAN`, + 6 `DOC-`) | 5 new, 3 edited |
| `ART/` | 3 | 3 edited |
| `UNI/` | 4 | 2 edited |
| `GAME/` | 4 | 1 new, 1 edited |
| `AI/` | 2 | 1 edited |
| `REL/` | 3 | 2 new, 2 edited |
| `Character/` | 1 | new |
| `History/` | 7 | 1 edited |
| Root evidence docs | 4 | 4 edited |

### C.2 Corrections applied in Phase 2.5

| # | File | Correction |
|---|---|---|
| 1 | `ART-0002` | **Added §0 "Filename semantics"** — `FINAL`/`APPROVAL_CANDIDATE` in a filename is not an approval state |
| 2 | `ART-0001` | **Added filename warning**; listed the 6 `WallSegment`/`FloorSegment` FBX as `STATIC VERIFIED` |
| 3 | `ART-0002` | Corrected a truncated filename (vest inspection collage) to its full name |
| 4 | `UNI-0001` | Project root → relative; **added equipment sockets as `VERIFIED ABSENCE`** |
| 5 | `UNI-0004` | Project root → relative; "works in editor" → **`RUNTIME VERIFIED`** with date |
| 6 | `PROJECT_TRUTH` | Project root → relative; §3 heading → `RUNTIME VERIFIED, 2026-09-29` |
| 7 | `DOC-0001` | "generator works" → "generator runs and builds its layout at runtime" |
| 8 | `RECONCILIATION_SOURCE_INVENTORY` | **Removed 4 personal/unrelated Desktop paths**; secret reference reduced to metadata; added scan results; **collage 24 → 25** |
| 9 | `CONSOLIDATION_VALIDATION_REPORT` | **Removed secret path**; Downloads path normalised; collage 24 → 25 |
| 10 | `AI-0001` | **Added §2.1 line-by-line skill verification** and §2.2 rule |
| 11 | `REL-0001` | Character/collage coverage rows updated to point at the new backup; Desktop shortcut detail trimmed |
| 12 | `HISTORY-0001` | 2026-09-26 heading + explicit "filename is not approval" note |
| 13 | `AI-0001`, `RECONCILIATION_SOURCE_INVENTORY`, `AI-0002` | `~/.config/opencode/...` normalised |

### C.3 Path sanitization

| Metric | Value |
|---|---|
| Machine-specific absolute paths before | 56 |
| Replaced with relative or `~/` form | 29 |
| Unrelated/personal Desktop paths removed | 4 |
| Secret location minimised to the security register | 1 |
| Remaining absolute paths | **15 — all `Z:\Мой диск\WWG_RECOVERY`** |
| Justification | Legitimate: recovery/backup evidence, explicitly permitted by §5 |

### C.4 Classification verification

| Check | Result |
|---|---|
| Classification vocabulary defined | **PASS** — 13 classes, `README.md` §5 |
| Runtime claims carry evidence | **PASS** |
| Untested behaviour marked `NOT VERIFIED` | **PASS** — 5 AI behaviours, progression, build |
| `QA PASS` never equated with `APPROVED` | **PASS** — enforced in `ART-0002` §0 |
| `EXTERNAL SOURCE EXISTS` never equated with `UNITY INTEGRATED` | **PASS** — `README.md` §6, `ARCHITECTURE.md` §4 |
| `STATICALLY EXISTS` never equated with `RUNTIME VERIFIED` | **PASS** |
| PLAN/REQUIREMENT never presented as implementation | **PASS** — `REL-0003`, `GAME-0004` |

Unqualified-implementation-claim scan: 65 candidate lines, **28 reviewed**, 6
tightened, 22 confirmed correct as written (state-machine definitions, open
decisions, or filename glosses).

### C.5 Link check

| Metric | Value |
|---|---|
| Backticked file references resolved | 87 296 filenames indexed across project + all external roots |
| Unresolvable references | **8** |
| — intentional prose abbreviations | 4 |
| — real defects | **1 fixed** (one truncated filename restored to its full name) |
| — files created this phase | 2 (now resolve) |
| — remaining | 1 → `CONSOLIDATION_PRECOMMIT_VALIDATION.md` (this file) |
| `Documentation/README.md` | **CREATED** — was missing |
| Navigation chain README → … → domains | **PASS** — all 9 steps present |

### C.6 Conflicts remaining

| Conflict | Status |
|---|---|
| `CONFLICT-01`…`07` | **RESOLVED** by evidence |
| `CONFLICT-08` 148 cover objects vs 72 `CoverPoint` | **OPEN** |
| `CONFLICT-10` 2 files on `origin/main` absent locally | **OPEN** — needs `DEC-07` |

---

## D. ChatGPT

### D.1 Source documents reviewed

| Document | Size | Content |
|---|---|---|
| `~/Downloads/CHATGPT_CHECKLIST.md` | 15 052 B | 51 headings, 20 stages, design decisions, security set |
| `~/Downloads/CHATGPT_CHECKLIST_before_STAGE19_2026-09-20.md` | 9 337 B | 41 headings, historical snapshot |
| `~/Downloads/CHATGPT_PROJECT_STATE.md` | 8 864 B | 14 headings, state claims |
| `~/Downloads/CHATGPT_WORK_QUEUE.md` | 3 360 B | 10 headings, sequencing |
| `~/Downloads/Вставленная уценка.md` | — | **NOT FOUND** (`ISSUE-03`) |

### D.2 Requirements preserved — **material gap found and closed**

Phase 2 had **no home** for these. They existed only in a superseded external
document. Recovered:

| Preserved | Into | Count |
|---|---|---|
| **STAGE 19 security/privacy/release requirements** | **`REL/REL-0003-SECURITY-AND-RELEASE-REQUIREMENTS.md`** | **57** |
| Fixed asset/style decisions (`DD-01`…`DD-06`) | `GAME/GAME-0004-REQUIREMENTS-AND-DESIGN-DECISIONS.md` | 6 |
| Progression / XP / economy / cards / shop requirements | `GAME-0004` §3 | 9 |
| Content design requirements (character, enemy, AI, environment, weapons, destructibles, aim) | `GAME-0004` §4 | 15 |
| Workflow constraints + status vocabulary (`WC-01`…`WC-04`) | `GAME-0004` §5 | 4 |
| Historical checkpoint record (last confirmed 4.4.1–4.4.4, current 4.4.5, Stage 3 open, Desert Pack dormant) | `GAME-0004` §6 | 1 |

### D.3 Decisions preserved

| Decision | Content |
|---|---|
| `DD-01` | Quaternius / «Konteri» temporary enemy model |
| `DD-02` | **Human Character Dummy (Asset Store 178395)** = technical character foundation — this is the *origin* of the Kevin Iglesias / 51-bone lineage |
| `DD-03` | Cities/buildings out of western scope |
| `DD-04` | Required western prop set (9 categories) |
| `DD-05` | Environment from replaceable prefab pools |
| `DD-06` | Do not mix incompatible low-poly styles |

### D.4 Historical content preserved

Nothing was deleted. All 4 `CHATGPT_*` files remain on disk untouched
(outside the repository — deliberately not edited). The `CHANGELOG` equivalent,
the stage history, and the checkpoint record are carried in
`History/HISTORY-0001` and `History/HISTORY-0002`.

### D.5 False claims corrected in canonical documentation

| Legacy claim | Canonical | Where |
|---|---|---|
| `Chest=141cm` | 100 | `CONFLICT-02` |
| `Skeleton=62 bones` | 51 | `CONFLICT-03` |
| `WWG_Doomy_Cowboy_REFINED.blend` missing | exists | `CONFLICT-01` |
| no release build | APK exists (stale) | `CONFLICT-04` |
| one backup | two | `CONFLICT-05` |
| character sources "in project" | external, backed up | `CONFLICT-01`, `ART-0001` |
| Stage numbers indicate readiness | history only | `HISTORY-0002` |

> Legacy implementation claims were **corrected in canonical docs and preserved
> historically** where the history matters. No legacy content was deleted merely
> to remove a contradiction.

---

## E. OpenCode

### E.1 AI_CONTEXT reviewed

12 files. All carry a `SUPERSEDED` header; **all original H1 lines intact**;
no original content removed or altered.

| File | Header | Original H1 intact |
|---|---|---|
| `AI_TOOLCHAIN_AUDIT.md` | yes | yes |
| `ARCHITECTURE.md` | yes | yes |
| `ART_PIPELINE.md` | yes | yes |
| `CHANGELOG.md` | yes | yes |
| `CONFIRMED_STATE.md` | yes | yes |
| `PROJECT_STATE.md` | yes | yes |
| `CURRENT_TASK.md` | yes | yes |
| `DEBUGGING.md` | yes | yes |
| `README.md` | yes | yes |
| `RULES.md` | yes | yes |
| `SPEC_IDEAS.md` | yes | yes |
| `VERIFICATION.md` | yes | yes |

### E.2 Skills reviewed — `wwg-character-art`

Line-by-line verification of state claims recorded in `AI-0001` §2.1:

| Claim | Verdict |
|---|---|
| `51-bone shared skeleton` | **CORRECT** |
| "The **existing** character rig is protected" | **INVALID as a Unity claim** — 0 Animator/Avatar/Controller/SkinnedMesh |
| "valid skinning / head weights / deformation / attachment points" | **NOT VERIFIED** — no Unity rig exists |
| `WeaponPoint_R/L`, `HolsterPoint`, `BackWeaponPoint` | **NOT VERIFIED in Unity** — no socket code; possibly Blender bones, unconfirmed |
| "10–11k triangles acceptable" | **NOT VERIFIED** — unmeasured, and no canonical `.blend` selected |
| Body/clothing construction rules | **VALID** as process guidance |

Rule recorded: *"existing"* in the skill means **existing in the external Blender
source**, never **existing in the Unity project**. Canonical documentation wins
(`DECISIONS.md` `GD-10`).

### E.3 Contradictions remaining

| # | Contradiction | Status |
|---|---|---|
| 1 | Skill asserts an existing rig; Unity has none | **Recorded** in `AI-0001` §2.1. Skill file itself not modified (outside Phase 2.5 documentation scope) |
| 2 | Skill attachment points vs. absent socket code | **Recorded** as `NOT VERIFIED` |
| 3 | Legacy `AI_CONTEXT` state claims vs. runtime | **Superseded** by `PROJECT_TRUTH` |

---

## F. Canonical

| Item | Value | Status |
|---|---|---|
| Canonical scene | `Assets/Scenes/TestArena.unity` | **RESOLVED** — build settings + runtime + mtime + GUID + tracking |
| Duplicate scene | `Assets/TestArena.unity` | `REVIEW — DO NOT DELETE` (`DEC-09`) |
| Canonical documentation root | `Documentation/` | **34 files, 9 folders** |
| Canonical entry point | `Documentation/README.md` | **CREATED this phase** |
| Truth | `PROJECT_TRUTH.md` | **PASS** |
| Requirements | `REQUIREMENTS.md` + `GAME-0004` + `REL-0003` | **PASS** — separated from state |
| Decisions | `DECISIONS.md` + `History/OPEN_DECISIONS.md` | **PASS** — decided vs. open clearly split |
| Plan | `MASTER_PLAN.md` | **PASS** — labelled PLAN; dependency-derived; no dates |
| State | `PROJECT_STATE.md` | **PASS** |
| Architecture | `ARCHITECTURE.md` | **PASS** — describes as-built, lists absences |
| Open issues | `History/OPEN_ISSUES.md` | **PASS** |

**Truth / Requirements / Decisions / Plan separation: PASS.** Each is a distinct
document with a distinct classification, and no requirement is stated as
implemented.

---

## G. Git

| Field | Value |
|---|---|
| Branch | `main` |
| HEAD | `7ad314c869e5dad1daf8e56fdf0b8f4197592969` (**unchanged by Phase 2.5**) |
| `origin/main` | `f932fcc722d22b8150ccf7e9616914324347c6e8` (unchanged) |
| Divergence | **ahead 1 / behind 2** |
| Recovery branch | `recovery/checkpoint-2026-09-29-audit2` → `040651b1d8963231b814217319b099b7ab5ef16d` |
| Recovery tag | `recovery-2026-09-29-audit2` → annotated tag object `7bc19c7aa0b…`, resolves to commit `040651b1d896…` |
| **Staged** | **0** |
| Working tree | **91 entries** = 19 M + 2 D + 70 untracked |
| `.gitignore` | **0 changes** |
| `Packages/`, `ProjectSettings/` | **0 changes** |

### G.1 Delta during Phase 2.5

| State | Before | After | Delta |
|---|---|---|---|
| total | 90 | 91 | +1 |
| untracked | 69 | 70 | +1 — new `Documentation/Character/` subfolder surfaced as a separate entry |

No tracked file was added, removed, or committed. No product file was modified.

### G.2 DOCUMENTATION COMMIT CANDIDATES

**New (7 entries, 34 files):**

- `Documentation/` — entire tree (34 files)
- `PROJECT_TRUTH.md`
- `ART_DELTA_AFTER_RECOVERY.md`
- `RECONCILIATION_SOURCE_INVENTORY.md`
- `CONSOLIDATION_VALIDATION_REPORT.md`
- `CONSOLIDATION_PRECOMMIT_VALIDATION.md` (this file)

**Modified (12):** all `AI_CONTEXT/*.md` — `SUPERSEDED` headers only

> A **selective** `git add` is required. **`git add .` must not be used.**

### G.3 NOT READY FOR COMMIT

| Excluded | Count | Reason |
|---|---|---|
| `Assets/Scenes/TestArena.unity` | 1 M | pre-existing scene change; needs review |
| `Assets/Scenes/TestArena_Preview.unity` | 1 M | pre-existing |
| `Assets/Scripts/Enemies/EnemyTacticalPlanner.cs` | 1 M | pre-existing |
| `Assets/Scripts/Procedural/ArenaGenerator.cs` | 1 M | pre-existing |
| `Assets/Scripts/UI/EnemyDirectionIndicator.cs` | 1 M | pre-existing |
| `Assets/Settings/Mobile_RPAsset.asset` | 1 M | pre-existing |
| `Assets/Settings/PC_RPAsset.asset` | 1 M | pre-existing |
| `Assets/Scripts/Enemies/EnemyTacticalEnvironmentScanner_TEST.cs` (+`.meta`) | 2 D | pre-existing deletions — **never auto-included** |
| `Assets/Art/Environment/Modular/**` | untracked | `DEC-04` |
| `Assets/Art/Props/**` | untracked | `DEC-05`, `DEC-06` |
| `Assets/Materials/WWG_*` (6 + metas) | untracked | `DEC-01`, `DEC-10` |
| `Assets/Prefabs/Environment/WWG_*` (6 + metas) | untracked | `DEC-10` |
| `Assets/Screenshots/**` | untracked | review separately |
| `Assets/TestArena.unity` (+`.meta`) | untracked | `DEC-09` |
| `Working/**` | 1 | product content |

> **Documentation ready ≠ project ready to commit.** These are two different
> operations. The 22 pre-existing product changes require individual review and,
> for the untracked assets, open decisions.

---

## H. Remaining risks

| # | Risk | Status | Note |
|---|---|---|---|
| 1 | `RISK-01` character art unprotected | **CLOSED** | 332/332 verified + restore test |
| 2 | `RISK-02` secret-suspect file | **MITIGATED** | never accessed; permanent handling rule; **not eliminated** — the file still exists outside the project |
| 3 | `RISK-03` clean clone cannot reproduce the scene | **OPEN** | needs `DEC-10` |
| 4 | `RISK-04` `Z:` cloud sync unconfirmed | **OPEN** | new backup is on `C:` only |
| 5 | `RISK-05` 82+ uncommitted changes | **OPEN** | needs `DEC-07` |
| 6 | `RISK-06` 12 broken refs / 4 missing scripts unidentified | **OPEN** | first repair step; may hide other failures |
| 7 | `RISK-07` unversioned `.blend1`, duplicate FBX sets | **OPEN** | `DEC-02` |
| 8 | `RISK-08` stale APK may be mistaken for working | **MITIGATED** | labelled in `REL-0002` |
| 9 | `RISK-09` systematic documentation drift | **MITIGATED** | 34-doc canonical set + supersession |
| 10 | **Git history not swept for secrets** | **OPEN** | new finding; `REL-0003` 19.1.3 not fully closed |
| 11 | **APK not inspected for credentials** | **OPEN** | `REL-0003` 19.1.4 not started |
| 12 | **Exact Unity version not pinned** | **OPEN** | `ISSUE-05` |
| 13 | **`WWG_Template_Armature` exists only inside `.blend`** | **MITIGATED** | backup is `.blend`-complete; single point of failure remains |

---

## Final status classification

| Item | Status |
|---|---|
| `RISK-01` character source backup | **CLOSED** |
| `RISK-02` secret-suspect file | **MITIGATED** |
| `RISK-03`…`RISK-09` | **OPEN** (5) / **MITIGATED** (2) |
| Secret exposure scan | **PASS** — 0 findings / 731 files |
| Character backup verification | **VERIFIED** — 332/332 |
| 34-document review | **PASS WITH CORRECTIONS** — 13 corrections applied |
| ChatGPT reconciliation | **PASS** — 57 security + 35 other requirements recovered |
| OpenCode reconciliation | **PASS** — skill state claims recorded, contradictions logged |
| Legacy document review | **PASS** — 12/12 headers correct, all original H1 intact |
| Canonical links | **PASS** — 1 defect fixed, 0 remaining |
| Path sanitization | **PASS** — 29 normalised, 4 personal paths removed |
| Git safety | **PASS** — 0 staged, HEAD unchanged, no history operation |
| Unity / C# / prefab / art integrity | **PASS** — 0 changes |
| **Ready for consolidation commit** | **YES — after explicit human APPROVE** |

### BLOCKED items requiring a human decision

| # | Item |
|---|---|
| 1 | `DEC-07` — Git divergence; must be resolved before staging |
| 2 | `DEC-10` — untracked `WWG_*` prefabs/materials |
| 3 | `DEC-01` — material policy |
| 4 | `DEC-02` — canonical character file |
| 5 | `DEC-09` — duplicate scene disposition |
| 6 | Explicit `APPROVE` to commit |

### NOT VERIFIED items

| # | Item |
|---|---|
| 1 | Git history secret sweep |
| 2 | APK credential inspection |
| 3 | Exact Unity version |
| 4 | 5 AI behaviours (combat, investigation, sound, cover, flanking) |
| 5 | Whether a build from current code succeeds |
| 6 | `Z:` cloud sync completion |

---

## Phase 2.5 compliance

| Constraint | Result |
|---|---|
| No Unity / C# / Scene / Prefab / Material change | **PASS** |
| No FBX / Blender `.blend` change | **PASS** |
| No `UNI-D01`/`D02`/`D04`/`D05` fix attempted | **PASS** — recorded only |
| No asset promoted | **PASS** — ceiling remains `QA PASS` |
| No project file deleted | **PASS** |
| No `merge` / `rebase` / `reset` / `clean` / `stash` | **PASS** |
| No `push` | **PASS** |
| No `git add` performed | **PASS** — 0 staged |
| No commit | **PASS** |
| Backup created outside the project | **PASS** |
| Secret contents never accessed | **PASS** |
| Only documentation files modified | **PASS** |

---

**End of `CONSOLIDATION_PRECOMMIT_VALIDATION.md`**
