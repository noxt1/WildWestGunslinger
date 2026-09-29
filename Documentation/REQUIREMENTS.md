# REQUIREMENTS

**Document ID:** `DOC-REQ-2026-09-29`
**Status:** CANONICAL
**Step 5 of the navigation chain** — see `README.md` §2.

---

## 1. What this document is

A **requirement** states what must be true. It is **not** a statement of what is
true. Current state lives in `PROJECT_STATE.md` and `ARCHITECTURE.md`.

Every row carries an explicit implementation status so a requirement is never
mistaken for progress.


> **Current requirements are maintained here.** Historical and superseded requirements are preserved in ``Documentation/Archive/`` and are **not** automatically active.

| Status | Meaning |
|---|---|
| `MET` | verified in the running build |
| `PARTIAL` | some part works; stated precisely |
| `NOT MET` | verified absent or verified broken |
| `NOT VERIFIED` | no evidence either way |
| `NOT STARTED` | no work exists |

---

## 2. Foundational requirements

| # | Requirement | Status | Evidence |
|---|---|---|---|
| FR-01 | The game runs in Unity 6 on Windows PC | **MET** | `RUNTIME VERIFIED` |
| FR-02 | A single canonical playable scene exists | **MET** | `Assets/Scenes/TestArena.unity` resolved on build + runtime + GUID evidence |
| FR-03 | The arena is procedurally generated from replaceable prefab pools (`DD-05`) | **MET** for generation; **NOT MET** for pool replaceability — 0 modular meshes | `UNI-D11` |
| FR-04 | No duplicate scene files remain | **NOT MET** | `Assets/TestArena.unity` duplicate, `REVIEW` (`DEC-09`) |
| FR-05 | The project is reproducible from a clean clone | **NOT MET** | scene depends on untracked prefabs/materials (`RISK-03`) |

---

## 3. Character requirements

| # | Requirement | Status | Evidence |
|---|---|---|---|
| CR-01 | A technical character foundation is fixed | **MET** | Human Character Dummy asset 178395 (`DD-02`); 1818 mm M-size; 51-bone armature |
| CR-02 | Character geometry exists in an editable source | **MET** | 43 `.blend` in the external source, **backed up (CLOSED)** |
| CR-03 | Character measurements are recorded | **MET** | 4 measurement/clearance documents |
| CR-04 | Character multi-view QA is produced | **MET** | ~120 renders, `CK1`–`CK18` + view sets |
| CR-05 | **One** canonical character file is designated | **NOT MET** | 10 `Doomy` candidates + 32 `Foundation` variants (`DEC-02`) |
| CR-06 | A character asset is `APPROVED` | **NOT MET** | no character asset has an approval record (note: `BARREL_01`/`FENCE_01` props **are** approved - `ART-0002` §4) |
| CR-07 | The character is imported into Unity | **NOT MET** | no character FBX in `Assets/` (`ISSUE-06`) |
| CR-08 | A skinned representation exists in the scene | **NOT MET** | 0 `SkinnedMeshRenderer` (`UNI-D02`) |
| CR-09 | The character is animated | **NOT MET** | 0 `Animator`, 0 `Avatar`, 0 `AnimatorController` |
| CR-10 | Equipment attachment points function | **NOT VERIFIED** | no socket/attach code in `Assets/Scripts`; named points exist only as unconfirmed Blender bones |
| CR-11 | Clothing respects measured garment clearance | **MET** (art side) | 7 garment groups documented; measured values within or intentionally-fitted |
| CR-12 | Reference provenance is recorded and licence-clean | **MET** (character references) | `_REFERENCES/SOURCES.md`, 39 CC0/public-domain images |

---

## 4. Environment requirements

| # | Requirement | Status | Evidence |
|---|---|---|---|
| ER-01 | Modular wall and floor geometry exists | **MET** | 6 FBX (`WallSegment_01_{FINAL,r2,r3}`, `FloorSegment_01_{FINAL,r2,r3}`) |
| ER-02 | A FINAL wall/floor revision is designated | **NOT MET** | `DEC-04`; `_FINAL` in a filename is not an approval |
| ER-03 | Modular FBX import is correct | **NOT MET** | 0.01× scale, Z-up uncompensated (`UNI-D06`) |
| ER-04 | Modular geometry is integrated into the scene | **NOT MET** | 0 modular meshes (`UNI-D11`) |
| ER-05 | Destructible props are present | **NOT MET** | 0 instances (`UNI-D12`) |
| ER-06 | Required western prop set exists (rocks, cacti, trees, fences, lanterns, wagons, crates, barrels, covers) — `DD-04` | **PARTIAL** | Crate/Cover present; the rest unaudited |
| ER-07 | Cities and buildings stay out of scope — `DD-03` | **MET** (scope honoured) | no city assets |

---

## 5. Gameplay requirements

| # | Requirement | Status | Evidence |
|---|---|---|---|
| GR-01 | The player can move | **MET** | `RUNTIME VERIFIED` |
| GR-02 | Weapons can be fired | **MET** | both prefabs fire |
| GR-03 | Weapon damage is authorable from data | **NOT MET** | forced to 200 at runtime, overriding Inspector values (`UNI-D10`) |
| GR-04 | Enemies spawn and move | **MET** | spawn + custom A\* verified |
| GR-05 | Enemies perceive the player | **PARTIAL** | vision scanning verified; alerting/combat not tested |
| GR-06 | Enemies take cover, flank, investigate, react to sound | **NOT VERIFIED** | 5 behaviours untested |
| GR-07 | NavMesh navigation is available | **NOT MET** | 0 agents, 0 triangulation (`UNI-D01`) |
| GR-08 | No broken object references | **NOT MET** | 12 (`UNI-D04`) |
| GR-09 | No missing scripts | **NOT MET** | 4 (`UNI-D05`) |
| GR-10 | XP, economy, cards, shop, run progression exist | **NOT VERIFIED** | no evidence of any kind |
| GR-11 | Character/enemy classes are visually distinct | **NOT MET** | capsules |

---

## 6. Platform requirements

| # | Requirement | Status | Evidence |
|---|---|---|---|
| PR-01 | The game builds from current code | **NOT VERIFIED** | never attempted; last build 2026-09-09 from 2026-09-02 code |
| PR-02 | Android touch controls function | **NOT MET** | all `MobileTouchControls` refs `null` (`UNI-D07`) |
| PR-03 | The exact Unity version is pinned | **NOT MET** | not recorded anywhere (`ISSUE-05`) |
| PR-04 | The package manifest is inventoried | **NOT MET** | not captured (`ISSUE-07`) |
| PR-05 | CI, tests and a build script exist | **NOT MET** | none exist (`ISSUE-09`) |
| PR-06 | The game runs on a real Android device | **NOT VERIFIED** | cannot pass while `PR-02` fails |

---

## 7. Security and release requirements

**Full 57-item set with per-item status: `REL/REL-0003-SECURITY-AND-RELEASE-REQUIREMENTS.md`.**

| # | Requirement | Status |
|---|---|---|
| SR-01 | No secrets in `Assets`, `Resources`, `StreamingAssets` or the shipped build | **NOT VERIFIED** — working tree clean (0 findings / 731 files); Git history and APK not swept |
| SR-02 | Development tooling and credentials are development-only | **NOT MET** — no build pipeline to enforce it |
| SR-03 | Release signing is configured | **NOT MET** |
| SR-04 | Release build hardening is applied | **NOT MET** |
| SR-05 | Final security sign-off before marketplace submission | **NOT MET** — **hard gate, `REL-0003` 19.9.7** |

---

## 8. Data integrity requirements (governance)

| # | Requirement | Status |
|---|---|---|
| DR-01 | No asset is promoted past `APPROVED` by an agent | **MET** |
| DR-02 | No legacy document is deleted | **MET** — 12 retained with `SUPERSEDED` headers |
| DR-03 | No product content is modified by documentation work | **MET** |
| DR-04 | Untested behaviour is never described as working | **MET** after Phase 2.5 review |
| DR-05 | Filenames never substitute for approval state | **MET** — explicit rule in `ART-0002` §0 |
| DR-06 | Suspected secrets are never read, printed, hashed or committed | **MET** |
| DR-07 | Every material claim carries an evidence class | **MET** |
| DR-08 | Machine-specific paths are limited to legitimate evidence uses | **MET** after Phase 2.5 sanitization |

---

## 9. Summary

| Group | Requirements | MET | PARTIAL | NOT MET | NOT VERIFIED / NOT STARTED |
|---|---|---|---|---|---|
| Foundational | 5 | 2 | 1 | 2 | 0 |
| Character | 12 | 6 | 0 | 5 | 1 |
| Environment | 7 | 2 | 1 | 4 | 0 |
| Gameplay | 11 | 3 | 1 | 4 | 3 |
| Platform | 6 | 0 | 0 | 3 | 3 |
| Security/release | 5 | 0 | 0 | 4 | 1 |
| Data integrity | 8 | 8 | 0 | 0 | 0 |
| **Total** | **54** | **21** | **3** | **18** | **8** |

Plus **57** detailed security/release requirements in `REL-0003`, of which
**0 are implemented**.

---

## 10. Cross-references

- `PROJECT_STATE.md` — current status per system
- `ARCHITECTURE.md` — how it fits together
- `DECISIONS.md` — what is decided
- `MASTER_PLAN.md` — sequencing
- `History/OPEN_ISSUES.md` — unresolved gaps
- `GAME/GAME-0004-REQUIREMENTS-AND-DESIGN-DECISIONS.md` — design decisions
- `REL/REL-0003-SECURITY-AND-RELEASE-REQUIREMENTS.md` — full release set

---

**End of `REQUIREMENTS.md`**
