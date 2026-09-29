# AI-0001 — AI Agent and Skill State

**Document ID:** `AI-0001`
**Status:** CANONICAL
**Date:** 2026-09-29

---

## 1. Trust model for AI output

AI-generated claims are **advisory** and rank lowest in the evidence hierarchy
(`DOC-0001` §1). An AI statement becomes canonical only when corroborated by a
file fact or a runtime observation.

| Source class | Trust | Handling |
|---|---|---|
| Runtime observation | **Authoritative** | becomes canonical directly |
| File/hash/GUID facts | **Authoritative** | becomes canonical directly |
| Git history | **Authoritative** | becomes canonical directly |
| Measured art docs | High | canonical, with the measurement as the source |
| AI prose (`CHATGPT_*`, skills) | **Advisory** | must be corroborated before canonical |
| AI assumptions | **None** | must be labelled as assumption |

---

## 2. Project skills

### `.opencode/skills/wwg-character-art/SKILL.md`

| Property | Assessment |
|---|---|
| QA view-set and measurement discipline | **VALID** — matches `ART-0003` practice |
| Clearance-profile methodology | **VALID** — matches `Doomy_*_profile.md` |
| Multi-view render requirement | **VALID** — matches the `QA/` render sets |
| **Claims about the current rig** | **INVALID** — the file asserts rig state that does not match the build (no rig is integrated) |
| **Claims about current character stage** | **INVALID** — conflicts with `ART-0001` |

**Disposition:** the *method* sections are usable as process guidance. The
*state* sections are wrong and must not be relied on.

### 2.1 Phase 2.5 line-by-line verification of the skill's state claims

| Skill line(s) | Claim | Verdict | Evidence |
|---|---|---|---|
| 289 | `51-bone shared skeleton` | **CORRECT** | `Doomy_measurements.md` — `WWG_Template_Armature` = 51 bones |
| 285 | "The **existing** character rig is protected" | **INVALID as a Unity claim** | 0 `Animator`, 0 `Avatar`, 0 `AnimatorController`, 0 `SkinnedMeshRenderer` (`UNI-D02`) |
| 291–294 | "valid skinning / head weights / deformation / attachment points" | **NOT VERIFIED** | No Unity rig exists to be valid. These describe a Blender-side intent, not a running system |
| 305–308 | Protected attachments `WeaponPoint_R`, `WeaponPoint_L`, `HolsterPoint`, `BackWeaponPoint` | **NOT VERIFIED in Unity** | No socket/attach/equip code in `Assets/Scripts`; 0 SkinnedMeshRenderer. May exist as **Blender bones** in the source — unconfirmed either way |
| 254 | "current character scale around 10–11k triangles is acceptable for the current WWG foundation" | **NOT VERIFIED** | Triangle count of the current canonical `.blend` is unmeasured, and no canonical `.blend` is selected (`DEC-02`) |
| 51–53, 92, 152–153 | Body/clothing construction rules ("retain the existing Kevin body", "not detached shells", "maintain original Kevin proportions") | **VALID** as process guidance | Consistent with `ART-0003` method and `DD-02` (`GAME-0004`) |

### 2.2 Rule arising from this review

> The skill describes a **Blender-side art workflow** and its state sections
> implicitly assume a Unity rig that **does not exist**. Treat every "existing"
> in the skill as *"existing in the external Blender source"*, never as
> *"existing in the Unity project"*.
>
> The canonical Unity reality is `UNI-0001` and `UNI-0003`, **not** the skill.

### 2.3 Operational instruction vs. project state

| Skill content type | Handling |
|---|---|
| Measurement method, clearance profiles, QA view sets, silhouette/anatomy rules | **Operational instruction** — valid, follow it |
| Triangle budget, rig protection, attachment points, "existing" anything | **Project state assertion** — unverified or false; verify against `UNI-0001`/`ART-0001` before acting |
| Any "current"/"existing" claim | Must be re-verified. Canonical documentation wins (`DOC-0001` §1) |

---

## 3. External skill libraries

`~/.config/opencode/skill-libraries/` contains a large generic
Blender/Unity/3D skill set. These are **general-purpose and not project-specific**.
They are not evidence about WildWestGunslinger and must never be cited as such.

Notable generic skills present include modeling, materials, lighting, cameras,
rendering, animation, export, and rig/QA workflows. Use them as technique
references only.

---

## 4. ChatGPT-sourced documents

| File | Status |
|---|---|
| `CHATGPT_CHECKLIST.md` | advisory; uses stale Stage numbering |
| `CHATGPT_CHECKLIST_before_STAGE19_2026-09-20.md` | historical |
| `CHATGPT_PROJECT_STATE.md` | advisory; several claims contradicted by runtime |
| `CHATGPT_WORK_QUEUE.md` | advisory; not authoritative |
| `Вставленная уценка.md` | **referenced but absent** |

---

## 5. AI context documents in the repository

`AI_CONTEXT/` contains 12 Markdown files, many of them modified or conflicted in
the working tree. They are **superseded** by this `Documentation/` tree
(see `DOC-0002`).

---

## 6. Known AI errors corrected in this phase

| # | AI claim | Reality |
|---|---|---|
| 1 | `WWG_Doomy_Cowboy_REFINED.blend` NOT FOUND | **Exists** — search was scoped to the repo only |
| 2 | Character blend sources are "in the project / in progress" | Real sources are in `Documents\WildWestGunslinger art`, dated and complete |
| 3 | No release build exists | **`WildWest.apk` 46.2 MB exists** (2026-09-09) |
| 4 | `Chest=141cm` | Chest is **100** (size 50) |
| 5 | `Skeleton=62 bones` | Armature has **51 bones** |
| 6 | Only one backup exists (2026-09-29) | A **12.16 GB dated backup** exists at `Z:` from 2026-09-22 |

Full detail in `RECONCILIATION_SOURCE_INVENTORY.md` §13.

---

## 7. Standing rules for AI-assisted work in this project

1. **Never modify Unity content, C#, scenes, prefabs, materials, Blender sources,
   FBX, or character art** to "fix" documentation drift. Document the drift.
2. **Never promote an asset** past `APPROVED`. Promotion is a human act.
3. **Never infer runtime behaviour** from documentation. Test it or mark it ❓.
4. **Never delete** a duplicate or legacy artefact. Mark it REVIEW and record it.
5. Every new claim must cite its evidence class.

---

## 8. Cross-references

- `AI-0002` — production methodology
- `DOC-0001` §1 — evidence precedence
- `DOC-0002` — superseded documents
- `History/HISTORY-0003` — audit history

---

**End of `AI-0001-AI-AGENT-STATE.md`**
