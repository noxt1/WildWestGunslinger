# PHASE 6.1 — DOCUMENTATION STRUCTURE + ARCHIVE MIGRATION

**Status:** COMPLETE — documentation only, 0 product changes
**Date:** 2026-09-29
**Commit:** see §17

---

## 1. Initial Documentation Structure

49 tracked files under `Documentation/`, plus 9 root-level `.md`:

```
Documentation/   History(7) GAME(4) UNI(4) ART(3) REL(3) AI(2) Character(1)  + 25 root files
repo root .md    PROJECT_TRUTH.md, AI_PRODUCTION_METHODOLOGY.md, AUDIT_SESSION_CONTEXT_2026-09-28.md,
                 ART_DELTA_AFTER_RECOVERY.md, AUDIT_2_*, CONSOLIDATION_VALIDATION_REPORT.md,
                 RECONCILIATION_SOURCE_INVENTORY.md, RECOVERY_POINT_REPORT.md
```

Problems: audit reports, staging manifests, recovery records and superseded registers sat **alongside**
canonical documents, so a new agent could not tell current state from history.

## 2. Final Documentation Structure

```
Documentation/
├── README.md                              ← entry point, current vs archive stated up front
├── PROJECT_STATE.md                       CANONICAL
├── ARCHITECTURE.md                        CANONICAL
├── REQUIREMENTS.md                        CANONICAL  (+ authority rule)
├── DECISIONS.md                           CANONICAL  (+ authority rule)
├── MASTER_PLAN.md                         CANONICAL  (+ authority rule)
├── DOC-0001 / DOC-0003 / DOC-0004         CANONICAL index, ID scheme, open questions
├── DOCUMENTATION_ARCHIVE_MIGRATION_MAP.md ← new, §4
├── STAGING_MANIFEST_PHASE6_1.md           ← new, §30
├── PHASE6_1_DOCUMENTATION_STRUCTURE_REPORT.md ← new, §32
├── PHASE6_GITHUB_RECONCILIATION_REPORT.md OPERATIONAL (pre-push)
├── PHASE6_PUSH_PREVIEW.md                 OPERATIONAL (pre-push)
│
├── GAME/ (4)   UNI/ (4)   ART/ (3)   REL/ (3)   AI/ (2)   Character/ (1)   History/ (7)
│
└── Archive/
    ├── README.md            ← new, §23
    ├── Legacy/      (1)  DOC-0002-SUPERSEDED-DOCUMENTS.md
    ├── Audits/     (14)  all phase reports + AUDIT_2_* + AUDIT_SESSION_CONTEXT
    ├── Historical/  (4)  AI_CONTEXT_LOSS_CHECK, RECOVERY_POINT_REPORT,
    │                      ART_DELTA_AFTER_RECOVERY, RECONCILIATION_SOURCE_INVENTORY
    ├── Superseded/  (2)  STAGING_MANIFEST_PHASE3/4
    └── Source/      (1)  AI_PRODUCTION_METHODOLOGY
```

`PROJECT_TRUTH.md` remains at **repo root** — the §6 target was deviated from deliberately; see §15.

## 3. Canonical Documents

`README.md`, `PROJECT_STATE.md`, `ARCHITECTURE.md`, `REQUIREMENTS.md`, `DECISIONS.md`,
`MASTER_PLAN.md`, `DOC-0001`, `DOC-0003`, `DOC-0004`, plus repo-root `PROJECT_TRUTH.md` and the
seven domain/History folders.

Authority rules added: `PROJECT_TRUTH` (§18), `DECISIONS` (§19), `REQUIREMENTS` (§20),
`MASTER_PLAN` (§21), `OPEN_ISSUES` (§22).

## 4. Operational Documents

`PHASE6_GITHUB_RECONCILIATION_REPORT.md` and `PHASE6_PUSH_PREVIEW.md` stay **current** — they
describe the current, unpushed Git state. They will move to `Archive/Audits/` once the push lands.

## 5. Archived Documents — 22 moves

| Class | Count | Contents |
|---|---|---|
| `Audits/` | 14 | PHASE 3, 4, 5A, 5B, 5B.1, 5C, 5D, 5R, 5-checkpoint, CONSOLIDATION_PRECOMMIT, AUDIT_2_RECONCILIATION, AUDIT_2_RUNTIME, CONSOLIDATION_VALIDATION, AUDIT_SESSION_CONTEXT |
| `Historical/` | 4 | AI_CONTEXT_LOSS_CHECK, RECOVERY_POINT_REPORT, ART_DELTA_AFTER_RECOVERY, RECONCILIATION_SOURCE_INVENTORY |
| `Superseded/` | 2 | STAGING_MANIFEST_PHASE3, STAGING_MANIFEST_PHASE4 |
| `Legacy/` | 1 | DOC-0002-SUPERSEDED-DOCUMENTS |
| `Source/` | 1 | AI_PRODUCTION_METHODOLOGY |

All 22 performed with **`git mv`** and all 22 recorded as **renames** — file history preserved.

## 6. Superseded / Historical Documents

Retained verbatim with original status headers. **No archived file was edited** — see §11.

## 8. GitHub-only Source Classification

| Document | Class | Location | Reason |
|---|---|---|---|
| `AI_PRODUCTION_METHODOLOGY.md` | SOURCE | `Archive/Source/` | 52-point methodology; specification, not yet canonicalised; zero overlap with canonical docs (0 refs to `GAME-0004`, `PROJECT_TRUTH`, `AI_CONTEXT`) |
| `AUDIT_SESSION_CONTEXT_2026-09-28.md` | AUDIT | `Archive/Audits/` | §14 recommended location; self-declares it must not replace canonical docs; security-reviewed clean |

## 9. README Changes

`Documentation/README.md` rewritten to be a true entry point:

- new **§0** stating CURRENT vs ARCHIVE, the explicit "must not be applied" rule, and the reminder
  that Archive is **not a security boundary**;
- navigation chain updated to include domains and Archive;
- **§4** rewritten as a domain-topic table (Character, Combat, Weapons, Progression, UI, Enemy,
  Environment, Art, AI, History) mapped to actual documents, with a note that `GAME-0002` covers
  several topics and is deliberately not split or duplicated;
- **§7** blocking-status table refreshed — it had gone stale ("consolidation AWAITING APPROVAL",
  "7 critical defects");
- **§10** links repaired to the new archive paths.

## 10. Link Validation

| Check | Result |
|---|---|
| markdown links to moved files | **0 broken** (re-checked after move) |
| bare-path references to old locations, current docs | **48 found → 0 remaining** |
| relative markdown links in current docs | 0 broken |
| pre-existing broken link `../Documentation/PROJECT_TRUTH.md` | **found and fixed** in 2 files |

Archived documents intentionally retain historical path references — these are statements of what
was staged at the time. Rewriting them would falsify history, so they were left alone.

## 11. Security Review

Every one of the 22 move candidates was scanned **before** movement with 9 verified pattern classes
(private key, AWS key, GitHub PAT, Slack token, OpenAI key, Google API key, JWT, connection-string
password, assigned secret):

**0 secret-pattern hits across all 22 candidates.**

- `omniroute ключи.txt` — **never read, never tracked** (0 tracked files match).
- `AUDIT_SESSION_CONTEXT_2026-09-28.md` contains operational metadata (repo ID, visibility, connector
  permission set, 4 local ports, 2 Windows paths) but **no secrets**. It remains public — archiving
  does not hide it, and the report says so explicitly.

## 12. Files Moved — 22 (see `STAGING_MANIFEST_PHASE6_1.md` §A)

## 13. Files Created — 4

`Archive/README.md` · `DOCUMENTATION_ARCHIVE_MIGRATION_MAP.md` · `STAGING_MANIFEST_PHASE6_1.md` ·
this report.

## 14. Files Not Moved

`PROJECT_TRUTH.md` (root) · the four `History/OPEN_*` live registers · all domain folders ·
`PHASE6_*` operational docs · `Working/` sources · `AI_CONTEXT/` (§8 role separation — it is the
operational layer and was **not** copied into canonical docs).

## 15. Remaining Ambiguities

1. **Three deliberate deviations from the §5 target**, each with recorded evidence in the migration map:
   `PROJECT_TRUTH.md` stays at repo root (README links `../PROJECT_TRUTH.md`); the `OPEN_*` registers
   stay in `History/` (they are **live**, not historical — the folder name is a misnomer);
   `Enemy/`, `Combat/`, `Environment/`, `Weapons/`, `Progression/`, `UI/` were **not** created, because
   §5 forbids folders made for count and their content already lives in `GAME-*`/`UNI-*` — the README
   domain map covers them without duplicating documents.
2. **`History/` is misnamed** — it holds four live registers. Renaming it is a future decision; it
   would touch many cross-references.
3. **`AI_PRODUCTION_METHODOLOGY.md` is a strong specification still not canonicalised.** Whether to
   promote it into `Art/` is a human decision.
4. **`PHASE6_PUSH_PREVIEW.md` will need archiving** after the push.
5. **`Working/` production sources remain uncommitted** (8 `.blend`, 22 QA `.md`) — deliberate, needs
   a human decision.

## 16. Git Status

Documentation only. 22 renames, 4 new, canonical-doc updates, link repairs. **0 documentation
deletions, 0 product changes.** The 75 non-`.md` worktree entries are pre-existing product work,
untouched.

## 17. Commit

`docs: finalize documentation structure and archive`

No amend, rebase, reset, clean, stash or force. **No push** — per §34 the Phase 6 push has not
happened, so this joins the final push set after full review.
