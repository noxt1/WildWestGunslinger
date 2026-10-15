# MASTER_PLAN

**Document ID:** `DOC-PLAN-2026-09-29`
**Status:** CANONICAL — **this is a PLAN, not a commitment and not a schedule**
**Step 7 of the navigation chain** — see `README.md` §2.

**No agent may execute this plan.** Every step below that touches product
content requires explicit human authorization, and most are additionally blocked
on an open decision.

---

## 1. Derivation

This plan is derived **only** from `PROJECT_STATE.md` and `REQUIREMENTS.md`.
It contains no new aspirations. Ordering is by **hard dependency**, not by
preference.

---

## 2. Phase A — Documentation consolidation *(ready now)*

> **This is the current roadmap.** Historical roadmaps and old Stage numbering are preserved in `Documentation/Archive/` (see `Archive/Historical/HISTORY-LEGACY-STAGE-NUMBERING`) and must **not** be treated as current planning identifiers.

| Step | Action | State |
|---|---|---|
| A1 | Consolidate canonical documentation | **DONE** — Phase 2 |
| A2 | Close `RISK-01` (character source backup) | **DONE** — Phase 2.5, 332/332 verified |
| A3 | Close `RISK-02` / run the secret scan | **DONE** — Phase 2.5, 0 findings / 731 files |
| A4 | Validate all canonical docs | **DONE** — Phase 2.5 |
| **A5** | **First consolidation commit** | **BLOCKED — awaiting human `APPROVE`; needs `DEC-07`** |

> **A5 is the only step in this plan that is purely documentation and could
> proceed immediately after approval.** It must contain documentation only —
> see §6.

---

## 3. Phase B — Restore integrity *(blocked on `DEC-07`)*

| Step | Action | Blocked by |
|---|---|---|
| B1 | Decide Git divergence handling | `DEC-07` |
| B2 | Decide whether untracked `WWG_*` prefabs/materials are committed | `DEC-10` |
| B3 | Decide the material policy | `DEC-01` |
| B4 | Decide the duplicate-scene disposition | `DEC-09` |
| B5 | Confirm `Z:` cloud sync, or mirror backups to a second medium | `RISK-04` |

> **B2 is the highest-value step in the whole plan.** Until untracked scene
> dependencies are committed, a clean clone cannot reproduce the playable scene.

---

## 4. Phase C — Enumerate before repairing *(read-only, no approval needed to start)*

| Step | Action | Why first |
|---|---|---|
| C1 | Enumerate the **12 broken object references** (`ISSUE-01`) | A nulled reference silently disables behaviour and may explain five `NOT VERIFIED` AI systems |
| C2 | Enumerate the **4 missing Mono Scripts** by GUID (`ISSUE-02`) | Same |
| C3 | Determine why `MobileTouchControls` references are all `null` (`UNI-D07`) | May be a symptom of C1/C2 |
| C4 | Pin the exact Unity version and capture the package manifest (`ISSUE-05`, `ISSUE-07`) | Precondition for any reliable repair |

> **C is read-only investigation.** It changes nothing and requires no product
> modification. It should be the next work item after A5, because it is cheap
> and it determines the true repair scope.

---

## 5. Phase D — Repair order (dependency-driven)

Derived from `UNI-0003` and `ARCHITECTURE.md` §6.

```
  D1  UNI-D05  missing scripts      ─┐
  D2  UNI-D04  broken references    ─┴─→ restore lost logic
  D3  UNI-D10  gun damage override  ───→ weapon balance authorable
  D4  UNI-D08/D09 wrong materials   ───→ visual correctness (needs D1+B2/B3)
  D5  UNI-D06  FBX import 0.01×     ───→ prerequisite for ALL environment art
  D6  UNI-D11  integrate modular    ───→ 6 WallSegment/FloorSegment FBX
  D7  DEC-02   pick canonical char  ─┐   **REWORDED 2026-10-15:** the single-foundation
  D8  import + integrate character  ─┴─→ premise of `DEC-02` is SUPERSEDED by `DEC-12`.
                                    D7 now means: **select the foundation for a given role**
                                    (human act). D8 still means: import + integrate that
                                    chosen foundation (human act, and NOT authorised by `DEC-12`)
  D9  animation setup               ───→ first real animation in game
  D10 UNI-D01  NavMesh              ───→ only if a design actually needs it
  D11 UNI-D07  touch controls       ───→ Android viable
  D12 UNI-D12  destructibles        ───→ optional, lowest priority
```

### Hard constraints on Phase D

1. **D1/D2 first.** Everything else may be downstream of them.
2. **D5 before D6.** Integration is impossible while import is at 0.01×.
3. **D7 before D8.** `DEC-12` settled the architecture (multiple foundations allowed), but no
   foundation is integrated before one is **selected for its role** by the owner. Selection and
   integration both remain human acts.
4. **D10 is conditional.** The game currently navigates via custom A\*. Baking a
   NavMesh is only justified if a specific design requires it. Do not do it by default.
5. **No asset may pass `APPROVED` without a human** (`GD-07`).
6. **This plan does not authorise Phase D.** Repairing documentation consistency (including this
   file) removes a documentation obstacle only. Phase D steps stay blocked on their own gates:
   D1/D2 project-integrity repair first, then human approval for any asset promotion or character
   integration.

---

## 6. Phase E — First consolidation commit scope *(the boundary)*

**In scope — documentation only:**

### A5 allowlist — exact scope (REWRITTEN 2026-10-15, aligned at Final Scope Alignment and again after the `DEC-12` migration)

> **This section replaces both earlier scope definitions.** The pre-2026-10-15 wording used the
> blanket rule `Documentation/**` and named three repository-root paths that no longer exist (they
> moved under `Documentation/Archive/`). The intermediate 26-file allowlist was replaced after the
> content review showed that several listed files carry large pre-existing changes that were never
> reviewed as part of this commit. After the `DEC-12` migration and the corrected selective
> staging the **index actually holds 14 unique paths**. The scope below now distinguishes what is
> staged from what remains deferred, so that no broader claim is made than the index supports.
> Everything not named here is out of scope, including any other file under `Documentation/`.

#### A5.1 Staged whole — 8 files (every staged change is A5 work)

| # | Path | Staged content |
|---|---|---|
| 1 | `Documentation/MASTER_PLAN.md` | `DEC-02` / `DEC-12` alignment in D7 and F1; Phase D hard constraints incl. "this plan does not authorise Phase D"; §6 |
| 2 | `Documentation/PROJECT_STATE.md` | Git row as a dated measured snapshot; `UNI-D02` / `UNI-D03` split; Art table integration row |
| 3 | `Documentation/DOC-0004-OPEN-QUESTIONS.md` | Q1 / Q2 re-worded — per-role selection and `Cowboy` admission |
| 4 | `Documentation/DOC-0003-IDENTIFIER-SYSTEM.md` | path-case correction + new §5.1 on the `DEC-nn` / `DEC-0nn` series |
| 5 | `Documentation/DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md` | path-case correction |
| 6 | `Documentation/DOCUMENTATION_ARCHIVE_MIGRATION_MAP.md` | path-case correction |
| 7 | `Documentation/Character/CHARACTER_FOUNDATION_CONTRACT.md` | **NEW** — canonical Character Foundation Contract, `DEC-12` scope amendment |
| 8 | `AI_CONTEXT/CHARACTER_FOUNDATION_CONTRACT.md` | **NEW** — short pointer, no contract text |

#### A5.2 Staged selectively — 6 files (only the listed fragments are in the index)

| # | Path | Staged fragments | Confirmed absent from the index |
|---|---|---|---|
| 9 | `Documentation/DECISIONS.md` | `DEC-12` registration; identifier note; scope reminder; `DEC-02` → `SUPERSEDED by DEC-12`; §2a | `PD-04` / `PD-05` / `PD-12`; preamble backtick cleanup; trailing `**End of DECISIONS.md**` |
| 10 | `Documentation/History/OPEN_DECISIONS.md` | `DEC-02` `Blocks` row; architectural `Status` row with `DEC-12`; identifier note | weapon `DEC-11` record — byte-identical to `HEAD` |
| 11 | `Documentation/History/OPEN_CONFLICTS.md` | complete self-contained `CONFLICT-17`; contract reference in the CONFLICT-02/03 banner | the `Resolution` annotation of CONFLICT-03 |
| 12 | `Documentation/REQUIREMENTS.md` | `CR-05` only — re-worded with `DEC-12`, status still `NOT MET` | `CR-01` 51→53 change; preamble backtick cleanup |
| 13 | `AI_CONTEXT/ART_PIPELINE.md` | only the path-case fix in the "Canonical art state" line | the four Russian-text hunks from a previous session |
| 14 | `PROJECT_TRUTH.md` | §6 Git baseline as a dated measured snapshot + new §6a (5 hunks) | `QUATERNIUS FINAL VERDICT` block; `MULTIPLE CHARACTER FOUNDATIONS` section; `Bandit integration state` table; E1 Hat; measurement supersession; the approval block |

**Index total: 8 whole + 6 selective = 14 unique paths.**

#### A5.3 Deferred — not staged, because the required fragment is inseparable from excluded content

| Path | Wanted fragment | Why it stays out |
|---|---|---|
| `Documentation/Character/README.md` | contract references repointed | the references sit **inside** the excluded `MULTIPLE PRODUCTION CHARACTER FOUNDATIONS` block (+104 lines) |
| `Documentation/Art/ART-0001-ART-STATE.md` | two contract references | the same physical table lines also carry the pre-existing 51→53 bone correction |
| `Documentation/AI/AI-0001-AI-AGENT-STATE.md` | one contract reference | same line also carries the 51→53 bone correction |
| `Documentation/Art/ART-0003-ART-PIPELINE-AND-QA-GATES.md` | one contract reference | same line also carries the 51→53 bone correction |
| `Documentation/History/HISTORY-0002-LEGACY-STAGE-NUMBERING.md` | one contract reference | same table row also carries the superseded-51-bones marking |
| `Documentation/History/HISTORY-0003-AUDIT-HISTORY.md` | one contract reference | same table row also carries the superseded-51-bones marking |
| `Documentation/History/OPEN_ISSUES.md` | the two `DEC-12` pointers | both sit **inside** the newly added `ISSUE-27` block (+40 lines); isolating them would leave orphaned bullets |

These remain available for a separate, owner-approved pass. They are **not** staged as whole files
for convenience.

#### A5.3 What this preparation does **not** authorise

Preparing A5 is **not** a decision to publish the pre-existing changes listed below. They remain
uncommitted working-tree content and need their own human decision:

- the `ART-0004` external character asset audit, including its `REJECT` / `DEFER` admission verdicts;
- the `PROJECT_TRUTH.md` Quaternius feasibility verdict block;
- the `ART-0001` §11 E1 Hat section;
- the `AI-0002` agent tooling state audit;
- the `Character/README.md` Cowboy production-candidate block;
- `OPEN_ISSUES.md` `ISSUE-26` / `ISSUE-27`;
- `ARCHITECTURE.md`, `CHARACTER_SOURCE_BACKUP.md`, `AI-0001` 51→53-bone correction,
  `ART_PIPELINE.md` tooling block, and every other pre-existing change not reviewed above.

#### A5.4 Explicitly NOT in scope

| Excluded | Detail | Why |
|---|---|---|
| `Documentation/Archive/**` | 5 modified tracked files under `Archive/Audits/` and `Archive/Historical/` | Archive is history, not current instruction (`README.md` §0). Their local modifications are **preserved untouched** and **not** included. Separate human approval required. |
| `Documentation/History/OPEN_ISSUES.md` | `ISSUE-26` + `ISSUE-27`, +40 lines | pre-existing only, no A5 content |
| `Documentation/ARCHITECTURE.md` | 51→53-bone correction | pre-existing only, no A5 content; file still carries a stale divergence claim at L173 |
| `Documentation/Character/CHARACTER_SOURCE_BACKUP.md` | `WWG_Rig` 53-bone rewrite | pre-existing only, no A5 content |
| `Documentation/Art/ART-0004-THIRD-PARTY-ASSET-CATALOG.md` | owner absolute path removal + the full external audit | the audit itself is not approved; the path fix sits inside the same hunks |
| `Documentation/Art/ART-0001-ART-STATE.md` | E1 Hat section | pre-existing only |
| `Documentation/AI/AI-0002-…`, `Documentation/GAME/GAME-0004-…`, `AI_CONTEXT/CONFIRMED_STATE.md` | pre-existing blocks | pre-existing only |
| Other `Documentation/**` | `STAGING_MANIFEST_PHASE6_1/2/3.md`, `DOC-0004-OPEN-QUESTIONS.md`, `PHASE6_GITHUB_RECONCILIATION_REPORT.md`, `Engineering/**` | historical phase records, open-question registers and engineering docs not reviewed in this commit |
| `Assets/**` | all code, prefabs, scenes, materials, art, **the Bandit animator production fix and the QA harness** | product content; needs `DEC-10`; not documentation |
| `Packages/**`, `ProjectSettings/**` | `manifest.json`, `packages-lock.json`, `TimeManager.asset`, `*_RPAsset.asset` | project configuration, not documentation |
| `Working/**` | 944 untracked entries, including the existing QA and sync reports and all Blender sources | product/working material, uncommitted by decision |
| `WWG_Quaternius_Feasibility/**` | 517 untracked entries | locked-baseline experiment, read-only per its own lock |
| `.opencode/**` | skills and plugin | agent procedure, not canonical project documentation |
| Obsidian vault | HermesVault, its `master` branch and its git repo | separate memory store, deliberately isolated, no remote |
| extracted/misc debris | `_dsh_extracted/`, `_dsh_*.js`, `view_polys.py`, `0bf4b2d8-….json`, `arg1`, `--fbx` | tooling debris |
| Deleted files | `Assets/Scripts/Enemies/EnemyTacticalEnvironmentScanner_TEST.cs` + `.meta` (2 deletions) | never auto-included |
| Anything outside the repository | character source, external assets | out of project scope |

#### A5.5 Future technical operation — **not performed**

Staging the agreed hunks is a **future operation to be performed only after explicit human
`APPROVE`**. It has deliberately **not** been run; the Git index is empty.

1. Stage the 10 core files whose whole change is A5 content.
2. For the 4 hunk-limited core files (`REQUIREMENTS.md`, `OPEN_DECISIONS.md`,
   `AI-0001-AI-AGENT-STATE.md`, `ART_PIPELINE.md`) stage only the A5 hunks.
3. For the 7 mixed files stage only the fragments listed in A5.2; where an A5 line shares a hunk
   with a non-A5 line, split the hunk.
4. Stage the 2 new files (`Documentation/Character/CHARACTER_FOUNDATION_CONTRACT.md`,
   `AI_CONTEXT/CHARACTER_FOUNDATION_CONTRACT.md`).
5. Verify the resulting staged set equals this section exactly, then stop for a separate commit
   decision.

> **Documentation ready ≠ project ready to commit.** These are two different operations.
> Documentation may be ready for human review; the project is not ready to be committed wholesale.

---

## 7. Phase F — Later phases (not authorised, listed for completeness)

| Phase | Content | Gate |
|---|---|---|
| F1 | Character integration + animation | `DEC-12` satisfied (architecture) + **human selection of the foundation per role** + human integration approval. `DEC-12` alone does **not** authorise integration |
| F2 | Environment integration | D5, D6, `DEC-04` |
| F3 | AI behaviour verification (combat, investigation, sound, cover, flanking) | C1, C2 |
| F4 | Android platform bring-up | D11 |
| F5 | Progression / economy / cards / shop | design not authored |
| F6 | Security and release (`REL-0003`, 57 items) | core gameplay stable |
| F7 | Marketplace submission | **`REL-0003` 19.9.7 sign-off — hard gate** |

---

## 8. What this plan deliberately does not contain

- No dates, estimates or durations. There is no basis for them.
- No new features. `F5` items come from preserved requirements only.
- No assumption that legacy Stage numbers indicate readiness. They do not
  (`HISTORY-0002`).
- No claim that anything in Phases B–F is scheduled or approved.

---

**End of `MASTER_PLAN.md`**