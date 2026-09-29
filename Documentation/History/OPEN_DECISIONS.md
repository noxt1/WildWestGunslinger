# OPEN DECISIONS

**Status:** CANONICAL register of decisions that require a human
**Date:** 2026-09-29

**No agent may resolve any item in this file.** Each requires explicit human
authority, because each one is irreversible or affects many downstream items.

---

## DEC-01 — Repository material policy

| Field | Value |
|---|---|
| Question | What is the canonical material policy for the repository? |
| Why it matters | Governs `UNI-D08`/`UNI-D09` (font and debug materials on character prefabs), the untracked `WWG_*.mat` set, and every future material change |
| Options | (a) committed `.mat` in `Assets/Materials/`; (b) generated at import; (c) addressables; (d) per-asset exception list |
| Current state | Undecided. Untracked `WWG_*.mat` files are required by the canonical scene |
| Blocks | `Q4`, all material repair, scene reproducibility |
| Owner | human |

---

## DEC-02 — Character foundation identity

| Field | Value |
|---|---|
| Question | Which `WWG_Foundation_Source` variant (1 base + 31) **and** which `WWG_Doomy_Cowboy` file are canonical? |
| Candidates (Doomy) | `_WORK` (2026-09-21), `_APPROVAL_CANDIDATE` (09-22 15:48), `_REFINED` (09-22 16:16), `_VEST_APPROVAL_CANDIDATE` (09-22 17:12), `_VEST_WORK` (09-25 12:05), `_VEST_MARK1_REPAIR_SB_WORK` (09-25 23:33), `_VEST_FULL_REPAIR_SB_WORK` (09-26 00:10), `_FINAL_REPAIR_SB_WORK` (09-26 00:33), `_FINAL_REPAIR_SB_WORK_PRESAVE_20260926_053821` (09-26 05:38), `_CLOTHING_REBUILD_SB_WORK` (09-26 12:02) |
| Note | Names containing `APPROVAL_CANDIDATE` are **candidates, not approvals**. Newest ≠ approved |
| Also required | A canonical `FBX/` vs `Unity_Export/` selection (currently byte-identical duplicates) |
| Blocks | Every character asset, `UNI-D02`, `Q1`, `Q2` |
| Owner | human |

---

## DEC-03 — Character source location policy

| Field | Value |
|---|---|
| Question | Should the character art source stay in `Documents\WildWestGunslinger art`, or move into the repository? |
| Why it matters | 307 files, 180.2 MB of source of record, currently **unversioned**. `Working/blender_src/` is tracked but is a different set |
| Options | (a) move into repo; (b) keep outside and back up on a schedule; (c) symlink/submodule |
| Blocks | `RISK-01`, `RISK-03` |
| Owner | human |

---

## DEC-04 — Wall / Floor FINAL selection

| Field | Value |
|---|---|
| Question | Which `Wall` and which `Floor` FBX revision is FINAL? |
| Candidates (STATIC VERIFIED on disk) | `WallSegment_01_FINAL.fbx`, `WallSegment_01_r2.fbx`, `WallSegment_01_r3.fbx`, `FloorSegment_01_FINAL.fbx`, `FloorSegment_01_r2.fbx`, `FloorSegment_01_r3.fbx` — all in `Assets/Art/Environment/Modular/` |
| Note | Files named `_FINAL` **exist**, but a filename alone is **not** an approval. `BARREL_01` and `FENCE_01` are `APPROVED` on recorded user confirmation; the Wall/Floor modules are **not** |
| Why it matters | `ART_PIPELINE.md` referenced a FINAL selection; the only `_FINAL` designations available are in filenames and are unverified |
| Blocked by | `UNI-D06` (import at 0.01×) must be fixed before any selection can be validated in Unity |
| Owner | human |

---

## DEC-05 — `CRATE_01` CP1 approval — ✅ STATE ESTABLISHED, DECISION PENDING

| Field | Value |
|---|---|
| **Correction (Phase 3)** | This decision is **not** "should CP2 be promoted". That framing was wrong and implied CP2 was ever authorized |
| **Canonical state** | **`CRATE_01` — CP1 = COMPLETE. CP2 = NOT AUTHORIZED / AWAITING APPROVAL** |
| State | `QA PASS` — CP1 complete, user approval not yet given |
| Working `.blend` | `Working/blender_src/crate/Crate_01_Working.blend` (130 858 B) |
| Current content | `Crate_01_Intact` (1 mesh); **`Crate_Fragments` is EMPTY** |
| Geometry QA | 0 ngons / 0 loose / 0 duplicate verts / 0 non-manifold / **0 wood-to-wood intersections** |
| Intact tris | 740 (budget 400–800) |
| FBX on disk | `Crate_01_Intact_FINAL.fbx` (29 052 B), `Crate_01_Fragments_FINAL.fbx` (57 404 B) |
| **CP2** | **NOT AUTHORIZED.** Recorded stop point: `CRATE_01 CP1 COMPLETE — AWAITING USER APPROVAL` |
| Open question | Does the user approve CP1, thereby authorizing CP2? |
| CP2 sketch (not executed) | ~13–14 fragments: front ×2, rear ×2, left ×2, right ×2, lid ×2, base ×1, corner posts ×4 or pairs |
| Blocker | **CP2 must not begin without explicit user authorization** |
| Owner | human |
| Ref | `../ART/ART-0002-ART-ASSET-REGISTER.md` §5 |

> A `_FINAL` filename on the Crate FBX does **not** authorize CP2. The
> `Crate_Fragments` collection is empty in the working blend.

---

## DEC-06 — `Cover` r4 / r5 status

| Field | Value |
|---|---|
| Question | What is the status of `Cover` revisions r4 and r5? |
| Related | 148 cover objects exist in scene but only 72 `CoverPoint` are registered (`GAME-0001` §3) |
| Owner | human |

---

## DEC-07 — Git divergence resolution — ✅ CONFIRMED 2026-09-29

| Field | Value |
|---|---|
| Question | How to resolve `main` being **ahead 1 / behind 2** with uncommitted product changes? |
| Specifics | `origin/main` carries `AI_PRODUCTION_METHODOLOGY.md` and `AUDIT_SESSION_CONTEXT_2026-09-28.md`, both **absent locally** |
| **Status** | **✅ CONFIRMED — 2026-09-29, by project owner (explicit APPROVE)** |
| **Decision** | **Land the documentation commit on `main` as-is. Documentation only — no product files.** |
| **Divergence** | **DEFERRED.** The `origin/main` divergence (local ahead 1 / behind 2) is **NOT** resolved in Phase 3. It remains an open item for a separate, deliberately planned operation |
| **GitHub-only MD** | **NOT** pulled in via merge. `AI_PRODUCTION_METHODOLOGY.md` and `AUDIT_SESSION_CONTEXT_2026-09-28.md` stay unmerged; external reconciled copies remain in `~/WildWestGunslinger_RECOVERY/github_only/` and are already reflected in `Documentation/AI/*` and `Documentation/History/*` |
| Prohibited operations | `git pull`, `merge`, `rebase`, `reset`, `clean`, `stash`, `push`, force operations — **none performed** |
| Product changes | All 74 pre-existing product entries remain **uncommitted and unstaged** |
| Owner | human — **closed** |

### Rationale

Committing documentation on a dirty, behind branch is safe **as long as the
commit contains no product content and no history operation is performed**.
A commit is additive; it does not alter the working tree of other files. The
divergence is a separate concern from recording canonical documentation, and
coupling them would have required a prohibited history operation.

### Consequences accepted

1. `main` will be **ahead 2 / behind 2** after this commit.
2. The two GitHub-only Markdown files remain unmerged — accepted, since their
   content is already reconciled into canonical documentation.
3. Resolving the divergence later will require a planned operation; it must not
   be improvised.

---

## DEC-08 — Legacy Stage numbering authority

| Field | Value |
|---|---|
| Question | Does the legacy `Stage N` scheme retain any authority (milestones, contracts, external references)? |
| Default until answered | **History only** (`HISTORY-0002`) |
| Owner | human |

---

## DEC-09 — Duplicate scene disposition

| Field | Value |
|---|---|
| Question | What happens to `Assets/TestArena.unity` (untracked duplicate, GUID `92227d062a096334b8edf7335614c361`)? |
| Current instruction | **REVIEW — DO NOT DELETE** |
| Note | 2 lines differ from the canonical scene. Resolve whether it holds unique work before any action |
| Owner | human |

---

## DEC-10 — Untracked Unity asset policy

| Field | Value |
|---|---|
| Question | Should `Assets/Prefabs/Environment/WWG_*.prefab` and `Assets/Materials/WWG_*.mat` be committed? |
| Why it matters | A clean clone currently **cannot** reproduce the playable scene (`RISK-03`) |
| Owner | human |

---

## Cross-references

- `DOC-0004` — plain-language question list
- `UNI-0004` §6 — Git divergence detail
- `ART-0001` §3 — character candidates

---

**End of `OPEN_DECISIONS.md`**
