# PHASE 4 — AI_CONTEXT RECONCILIATION

**Document ID:** `DOC-P4-2026-09-29`
**Status:** COMPLETE
**Date:** 2026-09-29
**Predecessor commit:** `4b04f5f081a5e6717f1a1d250aa3cff5dbe9e10b`

---

## 1. Purpose and outcome

Reconcile the 8 remaining modified `AI_CONTEXT/*.md` files — 763 lines of
pre-existing project documentation — into the canonical documentation set, without
losing content, without creating a second competing source of truth, and without
touching product code.

**Outcome: 0 unidentified useful content. 10 canonical errors corrected.**

---

## 2. Files reviewed

| # | File | Purpose | Pre-existing lines | Canonical overlap | Unique content | Operational | Historical | Current |
|---|---|---|---:|---:|---:|---:|---:|---:|
| 1 | `ART_PIPELINE.md` | Art workflow bridge | **217** | medium | low | **high** | low | low |
| 2 | `CHANGELOG.md` | Change history | **176** | low | **high** | low | **high** | low |
| 3 | `CURRENT_TASK.md` | Active task + stop point | **165** | medium | medium | **high** | medium | **high** |
| 4 | `PROJECT_STATE.md` | Project state (legacy) | **87** | **high** | **high** | low | low | medium |
| 5 | `ARCHITECTURE.md` | Component reference | **71** | medium | **high** | medium | low | medium |
| 6 | `RULES.md` | Agent working rules | **23** | low | low | **high** | low | medium |
| 7 | `CONFIRMED_STATE.md` | User confirmations | **17** | low | **high** | low | **high** | medium |
| 8 | `VERIFICATION.md` | Evidence rules | **7** | low | low | **high** | low | medium |
| | **TOTAL** | | **763** | | | | | |

---

## 3. Content disposition

| Disposition | Fragments |
|---|---|
| **Migrated to canonical documentation** | **61** |
| **Retained operationally in `AI_CONTEXT`** | **44** |
| **Superseded / corrected** | **9** |
| **Flagged as conflict, not auto-resolved** | **5** |
| **Historical — kept in place, not rewritten** | **7** |
| **Intentionally excluded** | **0** |

Full per-fragment table: `AI_CONTEXT_LOSS_CHECK.md`.

---

## 4. Canonical documents changed (9)

| File | Change |
|---|---|
| `UNI/UNI-0001-UNITY-PROJECT-STATE.md` | Unity version **6000.3.23f1**; verified 42 `.cs` / 10 folders; measured line counts; systems-present-in-code table |
| `UNI/UNI-0002-UNITY-DATA-AND-ASSET-REGISTER.md` | **§5a** environment prefab registry (8 prefabs); **§5b** `WildWestEnvironment`; **§5c** render settings; **§5d** destructible foundation |
| `UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md` | **`UNI-D05` root cause identified** (4 prefabs → deleted script GUID); 7 warnings on one `Bandit` |
| `GAME/GAME-0001-GAME-DESIGN-STATE.md` | **§7** verified gameplay parameters: arena, waves, enemy archetypes, player, spawning, perception/noise, indicator |
| `GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` | **§3.1** progression corrected; **§3.2** historical user confirmations |
| `GAME/GAME-0004-REQUIREMENTS-AND-DESIGN-DECISIONS.md` | **§5.1 `OR-01`…`OR-12`** operational rules; **§5.2** protected-file list |
| `History/OPEN_ISSUES.md` | `ISSUE-05` **resolved**; **`ISSUE-13`…`ISSUE-19` created** |
| `History/OPEN_CONFLICTS.md` | **`CONFLICT-11`…`CONFLICT-16` added** |
| `History/OPEN_DECISIONS.md` | `DEC-05` detail enriched with CP2 budgets and fragment design |

---

## 5. `AI_CONTEXT` files changed (8)

All 8 received a **unified header** carrying Status · Role · Canonical
relationship · Source of truth · Reconciliation pointer, and a canonical pointer
using a **relative** path (§13 requirement). Old headers were stripped.

**No text was removed.** The diff shows 6 deletion lines, all accounted for:
2 intentional path normalisations (`PROJECT_STATE.md` project root → relative;
`CHANGELOG.md` external source → `~/…`) and 4 trailing-newline artifacts on the
last line of four files. All 12 `# ` H1 titles are intact and the 763
pre-existing lines are all still present.

| File | New Role |
|---|---|
| `PROJECT_STATE.md` | **Operational / reference** — points to canonical state, does not define it |
| `ARCHITECTURE.md` | **Operational / reference** — component-level engineering reference; line counts flagged stale |
| `ART_PIPELINE.md` | **Operational / art workflow** — authoritative for Method C, material + FBX contract, presentation state, batch policy |
| `CURRENT_TASK.md` | **Operational / task state** — active task, stop point, approval gates |
| `CHANGELOG.md` | **Historical** — append-only record |
| `CONFIRMED_STATE.md` | **Historical confirmations** — dated, explicitly not current runtime evidence |
| `RULES.md` | **Operational / binding rules** — how an agent may work |
| `VERIFICATION.md` | **Operational / evidence rules** — what counts as evidence |

---

## 6. Role separation achieved (§11, §14)

| Layer | Document | Defines |
|---|---|---|
| Reality | `PROJECT_TRUTH.md`, `PROJECT_STATE.md`, domain docs | what **exists** |
| Requirements | `REQUIREMENTS.md`, `GAME-0004`, `REL-0003` | what **must** exist |
| Decisions | `DECISIONS.md`, `History/OPEN_DECISIONS.md` | what is **approved** |
| Plan | `MASTER_PLAN.md` | what is **planned** |
| Open issues | `History/OPEN_ISSUES.md` | what is **broken** |
| **AI_CONTEXT** | `RULES.md`, `VERIFICATION.md`, `ART_PIPELINE.md`, `CURRENT_TASK.md`, `ARCHITECTURE.md` | **how to work** |
| Skills | `.opencode/skills/` | **how to execute** a specialised operation |

> `AI_CONTEXT` no longer stores a second copy of project state, requirements,
> architecture or decisions. Where those topics appear, the file points to the
> canonical document. Operational rules stay in `AI_CONTEXT` and are indexed —
> not duplicated — in `GAME-0004` §5.1.

---

## 7. Conflicts found and how they were handled (§16)

| ID | Conflict | Handling |
|---|---|---|
| `CONFLICT-11` | `AI_CONTEXT` script inventory stale (41 files / 8 dirs; 4 wrong line counts) | **RESOLVED** by measurement — 42 files / 10 dirs |
| `CONFLICT-12` | "XP has no evidence" vs. 5 scripts + user confirmation | **PARTIALLY RESOLVED** — code exists, current behaviour still unverified |
| `CONFLICT-13` | Mobile UI "CONFIRMED" vs. `UNI-D07` null touch refs | **OPEN** — 3 hypotheses, none assumed |
| `CONFLICT-14` | Spawn zones 53 vs. 52 | **OPEN** — immaterial, recorded |
| `CONFLICT-15` | Android primary vs. Windows primary | **OPEN** — real planning conflict, no decision invented |
| `CONFLICT-16` | 3 protected files are already modified | **OPEN** — rule and working state disagree |

**No contradiction was hidden and none was auto-resolved by assumption.**

---

## 8. Canonical truth corrections (10)

| # | Was | Now |
|---|---|---|
| 1 | "No asset is approved" | `BARREL_01`, `FENCE_01` **APPROVED** (user-confirmed) |
| 2 | "Crate CP2 promotion" | CP1 complete, **CP2 NOT AUTHORIZED** |
| 3 | "Unity version not recorded" | **6000.3.23f1** |
| 4 | "41 `.cs` in 8 dirs" | **42 in 10 dirs**; `Environment/` was missing |
| 5 | "XP: no evidence of any kind" | **5 scripts + historical confirmation** |
| 6 | `UNI-D05` unidentified | **Root cause: 4 prefabs → deleted script GUID** |
| 7 | 4 script line counts wrong | **Measured** in `UNI-0001` §5.1 |
| 8 | Shadow Aliasing unrecorded | **`ISSUE-14`** — open, unfixed |
| 9 | Duplicate scene had no root cause | **`ISSUE-17`** — MCP saved to `Assets/` not `Assets/Scenes/` |
| 10 | `AI_CONTEXT` never reconciled | **`ISSUE-19`** — process lesson |

---

## 9. Remaining ambiguity

| # | Ambiguity | Status |
|---|---|---|
| 1 | Whether mobile UI regressed or was never fully wired | `CONFLICT-13` |
| 2 | Android vs. Windows platform priority | `CONFLICT-15` |
| 3 | Whether modifying 3 protected files was authorised | `CONFLICT-16` |
| 4 | Character foundation identity — Dummy 178395 vs. Kevin Iglesias | `DEC-02` still open; the ambiguity is now **documented** rather than guessed |
| 5 | `CRATE_01` CP1 approval | `DEC-05` — awaiting the user |
| 6 | Spawn zone 53 vs 52 | `CONFLICT-14` |

---

## 10. Security check (§17)

| Check | Result |
|---|---|
| Secret patterns in modified/created MD | **0 findings** |
| `omniroute ключи.txt` | **not read, not copied, not committed** |
| Unnecessary personal paths | **0 added**; all paths in the 8 headers are **relative** |
| Product files touched | **0** |

---

## 11. Preserved-not-rewritten (§10)

`CHANGELOG.md` history was **not** rewritten. Entries that were later superseded
keep their original values; the supersession is stated in the file. This applies
specifically to:

- intermediate `Compiled=NO / Tested=NO` patch lines — marked as pre-bundle state, not current
- the deprecated `Instance==null` root-cause diagnosis — marked superseded by the correct `IsBuilt=false` finding
- garment lessons (Seams to Plush, Remesh, Cloth) — kept as lessons, not promoted to specification

---

## 12. Verification (§25)

A simulated new-agent read starting from `Documentation/README.md` can determine:
project reality (→ `PROJECT_TRUTH.md`, `PROJECT_STATE.md`) · canonical scene
(→ `PROJECT_TRUTH.md` §2) · architecture (→ `ARCHITECTURE.md`) · requirements
(→ `REQUIREMENTS.md`) · decisions (→ `DECISIONS.md`) · roadmap
(→ `MASTER_PLAN.md`) · open issues (→ `History/OPEN_ISSUES.md`) · character state
(→ `ART-0001` §4) · enemy/AI state (→ `GAME-0002`) · art state (→ `ART-0002`) ·
operational OpenCode rules (→ `AI_CONTEXT/RULES.md`, `VERIFICATION.md`) ·
history (→ `History/HISTORY-0001`, `AI_CONTEXT/CHANGELOG.md`) · reconciliation
evidence (→ `AI_CONTEXT_LOSS_CHECK.md`, this report).

---

## 13. Cross-references

- `AI_CONTEXT_LOSS_CHECK.md` — per-fragment disposition, 0 unidentified
- `PHASE3_COMMIT_REPORT.md` Appendix A — Phase 3 status and exclusion reason
- `STAGING_MANIFEST_PHASE4.md` — exact commit scope
- `History/OPEN_CONFLICTS.md` — `CONFLICT-11`…`16`
- `History/OPEN_ISSUES.md` — `ISSUE-13`…`19`

---

**End of `PHASE4_AI_CONTEXT_RECONCILIATION.md`**
