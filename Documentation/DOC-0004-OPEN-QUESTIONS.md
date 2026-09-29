# DOC-0004 — Open Questions

**Document ID:** `DOC-0004`
**Status:** CANONICAL
**Date:** 2026-09-29

A short, human-actionable list. Detail lives in `History/OPEN_DECISIONS.md`,
`OPEN_RISKS.md`, `OPEN_CONFLICTS.md` and `OPEN_ISSUES.md`.

---

## 1. Questions that block progress

| # | Question | Why it blocks | Ref |
|---|---|---|---|
| Q1 | Which of the 32 `WWG_Foundation_Source` variants is canonical? | Every downstream character asset depends on it | `DEC-02` |
| Q2 | Which `WWG_Doomy_Cowboy` file is canonical — `CLOTHING_REBUILD_SB_WORK` (newest) or an approval candidate? | "Newest" ≠ "approved" | `DEC-02` |
| Q3 | Which `Wall` / `Floor` FBX is FINAL? | Blocks environment promotion | `DEC-04` |
| Q4 | What is the repository material policy? | Blocks every material decision | `DEC-01` |
| Q5 | Is `CRATE_01` CP1 approved, thereby authorizing CP2? (CP1 = COMPLETE, **CP2 NOT AUTHORIZED**) | Blocks crate fragmentation | `DEC-05` |
| Q6 | What is the status of `Cover` r4 / r5? | Blocks cover promotion | `DEC-06` |
| Q7 | How should the Git divergence (ahead 1 / behind 2, 82 uncommitted) be resolved? | Blocks any clean commit | `DEC-07` |
| Q8 | Does the legacy Stage numbering have any remaining authority? | Affects every legacy doc | `DEC-08` |

---

## 2. Questions that block a release

| # | Question | Ref |
|---|---|---|
| Q9 | Which 12 objects hold the broken references? | `UNI-D04`, `ISSUE-01` |
| Q10 | Which 4 GameObjects have missing Mono Scripts? | `UNI-D05`, `ISSUE-02` |
| Q11 | Why is `MobileTouchControls` fully null-referenced? | `UNI-D07` |
| Q12 | What is the exact Unity version, and is it pinned? | `ISSUE-05` |
| Q13 | Where is the `Assets/` copy of the character FBX? | `ART-0001` §4.4, `ISSUE-06` |

---

## 3. Questions about evidence

| # | Question | Ref |
|---|---|---|
| Q14 | Are the five untested AI behaviours (combat, investigation, sound, cover, flanking) functional? | `GAME-0002` §3 |
| Q15 | Why do 148 cover objects map to only 72 `CoverPoint`? Decorative, or a registration bug? | `GAME-0001` §3 |
| Q16 | Which 146 of the 307 art files were excluded from the 2026-09-22 `Z:` art copy? | `REL-0001` §2 |
| Q17 | Is the `Z:` backup actually uploaded to Google Drive? | `RISK-04` |
| Q18 | Do the vest reference images referenced by `REFERENCE_INDEX.txt` still exist anywhere? | `ISSUE-04` |

---

## 4. Questions that must be answered by a human, never by an agent

1. Anything requiring an **`APPROVED`** asset decision (`GATE G5`).
2. Anything requiring **deletion** of a duplicate or legacy artefact.
3. Anything requiring **commit, push, merge, rebase, or reset**.
4. Anything touching **secrets** — including `Desktop\omniroute ключи.txt`.
5. The **material policy** (`DEC-01`), because it governs many other decisions.

---

## 5. Cross-references

- `History/OPEN_DECISIONS.md` — full decision register
- `History/OPEN_RISKS.md` — active risks
- `History/OPEN_CONFLICTS.md` — source conflicts
- `History/OPEN_ISSUES.md` — open defects and missing artefacts
- `UNI-0003` — defect list

---

**End of `DOC-0004-OPEN-QUESTIONS.md`**
