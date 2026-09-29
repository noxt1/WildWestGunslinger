# HISTORY-0002 — Legacy Stage Numbering

**Document ID:** `HISTORY-0002`
**Status:** CANONICAL (as history)
**Date:** 2026-09-29

---

## 1. Status

The legacy `Stage 1` … `Stage 20+` numbering is **retired as an identifier
system**. It is preserved here for traceability only.

**New work must not introduce new Stage numbers.** Use the `ART-` / `UNI-` /
`GAME-` / `AI-` / `DOC-` / `REL-` / `HISTORY-` scheme in `DOC-0003`.

---

## 2. Why it was retired

| Problem | Effect |
|---|---|
| Stage numbers are **global and unordered by domain** | "Stage 20" does not say whether it is art, code, or docs |
| Stage completion is **asserted in prose, never verified** | completed stages include work that does not exist in the build |
| The **same number is reused across documents** | `CHATGPT_CHECKLIST.md` and `ART_PIPELINE.md` disagree about what a stage means |
| **No evidence link** from a stage to a file, hash, or runtime observation | unverifiable |
| Stage numbering is **chronologically frozen** while work continued | new work has no stage |

---

## 3. Where legacy stages appear

| Source | Use |
|---|---|
| `CHATGPT_CHECKLIST.md` | primary stage tracker (superseded — `DOC-0002` S-06) |
| `CHATGPT_CHECKLIST_before_STAGE19_2026-09-20.md` | pre-Stage-19 snapshot (S-07) |
| `ART_PIPELINE.md` | stage-numbered art pipeline (S-01) |
| `AI_CONTEXT/*` | mixed use |
| `AUDIT_SESSION_CONTEXT_2026-09-28.md` | refers to "Stage 19/20" work |

---

## 4. Known stage claims that are contradicted by evidence

These legacy "completed" claims are **false or unverifiable** and must not be
carried forward:

| Legacy claim | Reality | Ref |
|---|---|---|
| Character rig / skeleton stage complete | 0 Animator, 0 Avatar, 0 SkinnedMeshRenderer in the scene | `UNI-D02` |
| Chest 141 cm | canonical measurement is **100** (size 50) | `ART-0001` §4.1 |
| Skeleton 62 bones | `WWG_Template_Armature` has **51 bones** | `ART-0001` §4.1 |
| Environment modular kit integrated | 0 modular meshes in scene | `UNI-D11` |
| Character source "in progress in project" | sources are complete but live **outside** the project | `ART-0001` §2 |
| No release build | `WildWest.apk` exists (2026-09-09) | `REL-0002` |

---

## 5. Migration mapping

Legacy stages are not mechanically mappable onto canonical IDs, because the
legacy scheme mixed domains. Use these pointers instead:

| Legacy concern | Canonical home |
|---|---|
| Art pipeline / QA | `ART-0001`, `ART-0002`, `ART-0003` |
| Scene / prefab / material work | `UNI-0001`, `UNI-0002`, `UNI-0003` |
| Gameplay / AI / controls | `GAME-0001`, `GAME-0002`, `GAME-0003` |
| Tooling / agent method | `AI-0001`, `AI-0002` |
| Backups / builds | `REL-0001`, `REL-0002` |
| Dated history | `HISTORY-0001` |

---

## 6. Open question

Does the legacy Stage numbering retain **any** authority — e.g. contractual
milestones referenced elsewhere? Recorded as `DEC-08` in `OPEN_DECISIONS.md`.
Until answered, treat it as **history only**.

---

## 7. Cross-references

- `DOC-0003` — identifier system
- `DOC-0002` — superseded register
- `HISTORY-0001` — timeline

---

**End of `HISTORY-0002-LEGACY-STAGE-NUMBERING.md`**
