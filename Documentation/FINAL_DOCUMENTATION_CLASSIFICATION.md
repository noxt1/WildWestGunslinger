# FINAL DOCUMENTATION CLASSIFICATION

**Status:** CANONICAL — governing classification record
**Date:** 2026-09-29
**Rule enforced by this document:**

> **Current documentation is authoritative for active work.**
> **Archived documentation is preserved for historical context and must not be applied automatically.**

This record supersedes the Phase 6.1 placement of two documents.

---

## 1. The methodology decision — CASE A

`AI_PRODUCTION_METHODOLOGY.md` is a **CURRENT SPECIFICATION**, not historical source.

It was **not** classified by filename. Each of its 52 sections was read and assessed. The evidence
that decided it:

| Evidence | Detail |
|---|---|
| **It states binding rules, not history** | §11 *No Inference*, §13 *Protected Asset Principle*, §15 *Reversibility*, §16–17 *Git Policy*, §39 *Stop Conditions*, §41 *Human Approval Gate* — all imperative rules for work being done now |
| **It governs work currently in progress** | Art production is `IN PROGRESS` under the **ART COMPLETION GATE**; this document defines how that production must be run |
| **It directly addresses open defects** | **§26 Modular Environment Principle** ("1 mesh, 1 MeshRenderer, 1 shared material, 1 simple collider") is the target architecture that **`RT-01`** (modular FBX import broken) must satisfy. **§25 Android Performance Principle** ("avoid one decorative element = one renderer") bears directly on **`RT-09` / `UNI-D14`** (4097 renderers, Android perf unproven) |
| **It is not a state document, by its own text** | §1 states: *"It is not a project-state document and is not a replacement for the project roadmap."* So promoting it does **not** conflict with `PROJECT_TRUTH.md` |
| **It is derived from validated practice** | §1: refined using the Barrel, Fence and Crate production workflows |

**Destination:** `Documentation/AI_PRODUCTION_METHODOLOGY.md`

A new `Process/` folder was **not** created — §3 permits it only if genuinely justified, and one
cross-project document does not justify a new top-level folder. It sits at the documentation root
beside `DECISIONS.md` because it is the same class of document: binding, cross-domain, and it must
be visible to a new agent immediately.

**No duplicate was left behind** (§4). Exactly one copy exists, verified by filesystem scan.
History is preserved: the move was recorded as `R100` (100% similarity), so Git shows a rename.

## 2. Audit finding — a second misplacement (§7)

While auditing `Archive/` for current documents, exactly **one** misplacement was found:

| Document | Finding | Evidence | Action |
|---|---|---|---|
| `DOC-0002-SUPERSEDED-DOCUMENTS.md` | was in `Archive/Legacy/`, but is **current** | self-declares `Status: CANONICAL`; defines the **live supersession policy**; cited by the canonical index `DOC-0001`, by `GAME-0004`, and by 4 `AI_CONTEXT` files | promoted to `Documentation/DOC-0002-SUPERSEDED-DOCUMENTS.md` (`R100`) |

A register *about* superseded material is itself current policy, not legacy content. Leaving it in
`Archive/` would have made current documents point into the Archive for an active rule.

A status sweep of all 23 archived documents found **no other** self-declared current document
(all others are `COMPLETE`, `FINAL`, `PREPARED`, `CHECKPOINT`, or carry no status header).

## 3. Final classification table

| Document | Current/Archive | Authority | Destination | Reason |
|---|---|---|---|---|
| `AI_PRODUCTION_METHODOLOGY.md` | **CURRENT** | **APPLY** | `Documentation/AI_PRODUCTION_METHODOLOGY.md` | binding production rules; governs work in progress; §25/§26 map to `RT-09`/`RT-01` |
| `DOC-0002-SUPERSEDED-DOCUMENTS.md` | **CURRENT** | **APPLY** | `Documentation/DOC-0002-SUPERSEDED-DOCUMENTS.md` | self-declared CANONICAL; live supersession policy; cited by the canonical index |
| `README.md` | CURRENT | entry point | `Documentation/README.md` | states the CURRENT/ARCHIVE rule |
| `../PROJECT_TRUTH.md` | CURRENT | **current reality** | repo root | cannot be overridden by Archive |
| `PROJECT_STATE.md` | CURRENT | APPLY | `Documentation/PROJECT_STATE.md` | domain state |
| `ARCHITECTURE.md` | CURRENT | APPLY | `Documentation/ARCHITECTURE.md` | system architecture |
| `REQUIREMENTS.md` | CURRENT | APPLY | `Documentation/REQUIREMENTS.md` | current requirements only |
| `DECISIONS.md` | CURRENT | APPLY | `Documentation/DECISIONS.md` | approved decisions only |
| `MASTER_PLAN.md` | CURRENT | APPLY | `Documentation/MASTER_PLAN.md` | current roadmap only |
| `History/OPEN_ISSUES.md` (+ `OPEN_DECISIONS`, `OPEN_CONFLICTS`, `OPEN_RISKS`) | CURRENT | APPLY | `Documentation/History/` | live registers, despite the folder name |
| `DOC-0001`, `DOC-0003`, `DOC-0004` | CURRENT | APPLY | `Documentation/` | index, ID scheme, open questions |
| `GAME/`, `UNI/`, `ART/`, `REL/`, `AI/`, `Character/` | CURRENT | APPLY | `Documentation/<domain>/` | active domain documentation |
| `PHASE6_PUSH_PREVIEW.md`, `PHASE6_GITHUB_RECONCILIATION_REPORT.md` | CURRENT *(temporary)* | current unpushed state | `Documentation/` | move to `Archive/Audits/` **after** a successful push (§17) |
| `Archive/Audits/*` (13) | ARCHIVE | history/context | `Documentation/Archive/Audits/` | completed phase + audit evidence |
| `Archive/Historical/*` (4) | ARCHIVE | history/context | `Documentation/Archive/Historical/` | point-in-time snapshots |
| `Archive/Superseded/*` (2) | ARCHIVE | history/context | `Documentation/Archive/Superseded/` | completed staging manifests |
| `Archive/Source/`, `Archive/Legacy/` | ARCHIVE *(empty)* | — | with `README.md` explaining the promotion | kept so the structure survives a fresh clone |

## 4. Archive leakage check

Every reference from a **current** document into `Archive/` was classified. Result:
**0 cases** where an archived document is cited as active authority. All such citations are
evidence, history, or supersession lookups.

## 5. Security

Both promotion candidates were scanned with 9 verified pattern classes before promotion
(control confirmed the scanner was live). **0 secret-pattern hits.** The forbidden
`Desktop\omniroute ключи.txt` was never read and is not tracked.

No full history re-scan: the Phase 6 scan covered 1125 blobs with 0 hits, and no blob has been
rewritten since — only renames were performed.

## 6. Product protection

**0 product files touched.** No C#, Unity scene, prefab, material, FBX, Blender or product asset
was modified. The 10 pre-existing `Assets/` worktree modifications remain untouched and unstaged.
