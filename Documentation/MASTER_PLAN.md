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
  D7  DEC-02   pick canonical char  ─┐
  D8  import + integrate character  ─┴─→ UNI-D02/D03 resolved
  D9  animation setup               ───→ first real animation in game
  D10 UNI-D01  NavMesh              ───→ only if a design actually needs it
  D11 UNI-D07  touch controls       ───→ Android viable
  D12 UNI-D12  destructibles        ───→ optional, lowest priority
```

### Hard constraints on Phase D

1. **D1/D2 first.** Everything else may be downstream of them.
2. **D5 before D6.** Integration is impossible while import is at 0.01×.
3. **D7 before D8.** No character work should start before a canonical file is chosen.
4. **D10 is conditional.** The game currently navigates via custom A\*. Baking a
   NavMesh is only justified if a specific design requires it. Do not do it by default.
5. **No asset may pass `APPROVED` without a human** (`GD-07`).

---

## 6. Phase E — First consolidation commit scope *(the boundary)*

**In scope — documentation only:**

- `Documentation/**`
- `ART_DELTA_AFTER_RECOVERY.md`
- `RECONCILIATION_SOURCE_INVENTORY.md`
- `PROJECT_TRUTH.md`
- `CONSOLIDATION_VALIDATION_REPORT.md`
- `CONSOLIDATION_PRECOMMIT_VALIDATION.md`
- The 12 `AI_CONTEXT/*.md` `SUPERSEDED` header edits

**Explicitly NOT in scope:**

| Excluded | Count | Why |
|---|---|---|
| Pre-existing modified tracked files | 15 | unrelated to documentation; reviewed individually first |
| Pre-existing deletions | 2 | never auto-included |
| Untracked `Assets/**` | ~64 | product content; needs `DEC-10` |
| Untracked `Working/**` | 1 | product content |
| Anything outside the repository | — | including the character source |

> **Documentation ready ≠ project ready to commit.** These are two different
> operations. The first is ready now; the second is not.

---

## 7. Phase F — Later phases (not authorised, listed for completeness)

| Phase | Content | Gate |
|---|---|---|
| F1 | Character integration + animation | `DEC-02` |
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
