# ART-0002 — Art Asset Register

**Document ID:** `ART-0002`
**Status:** CANONICAL
**Date:** 2026-09-29

State machine (from `DOC-0001` section 4):
`CREATED → QA PASS → APPROVED → PROMOTED → INTEGRATED → RUNTIME VERIFIED`

**Two assets are `APPROVED` (`BARREL_01`, `FENCE_01`). One is at `QA PASS` awaiting
approval (`CRATE_01`). E1 — Hat is `APPROVED WITH FIT CAVEAT` (§2a). Nothing is
`PROMOTED` or `INTEGRATED`.**

> ### Filename semantics — mandatory reading
> Several source files contain the words `FINAL`, `FINAL_REPAIR`,
> `FinalCorrection`, `APPROVAL_CANDIDATE` or `FINAL_REPAIR_SB_WORK` in their
> **names**. Per `../DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md` §4, the state
> machine is driven by a **recorded approval**, not by a filename.
>
> - A `FINAL` in a name is **necessary but not sufficient**. `FINAL` alone
>   proves nothing.
> - `APPROVAL_CANDIDATE` is a **candidate**, one step *earlier* than approved.
> - An asset is `APPROVED` **only** when a human confirmation is recorded, as in
>   §4 below. `BARREL_01` and `FENCE_01` meet that bar; nothing else does.

## 4. Approved assets — recorded user confirmation

These two assets carry a **recorded user confirmation** ("пользователь
подтвердил") in `AI_CONTEXT/ART_PIPELINE.md` → "Blender asset registry", and
their `_FINAL` FBX exports were verified present on disk during Phase 3.

| Asset | State | Working `.blend` | Final FBX (verified on disk) |
|---|---|---|---|
| **`BARREL_01`** | **`APPROVED`** (user-confirmed) | `Working/blender_src/barrel/Barrel_01_Working.blend` (867 831 B) | `Barrel_01_Intact_FINAL.fbx` (31 132 B), `Barrel_01_Fragments_FINAL.fbx` (76 860 B) |
| **`FENCE_01`** | **`APPROVED`** (user-confirmed) | `Working/blender_src/fence/Fence_01_Working.blend` (891 960 B) | `Fence_01_Intact_FINAL.fbx` (29 260 B), `Fence_01_Fragments_FINAL.fbx` (82 156 B) |

Recorded properties (from the same registry):

| Property | `BARREL_01` | `FENCE_01` |
|---|---|---|
| Style | stylized low-poly western, dark oak, dark old iron, rivets | stylized low-poly western, dark oak, dark old iron, nails |
| Method | Method C, 12 independent fragments | Method C, 13 independent fragments |
| Intact / Fragments tris | 580 / 900 | 668 / 1500 |
| FBX separation / reimport / reassembly | PASS / PASS / PASS | PASS / PASS / PASS |

**A confirmed root cause is recorded** for the "jitter" of the wood surface: the
intact mesh and the reassembled fragments were displayed **simultaneously on
coincident surfaces** in the Blender viewport. Not a material, geometry, or TAA
cause.

### 4.1 Approval is not integration

`APPROVED` means a human confirmed the **asset**. It does **not** mean:

| Not implied | Reality |
|---|---|
| promoted to a release target | not done |
| integrated into a Unity scene | **0 destructible instances in scene** (`UNI-D12`) |
| runtime verified | not verified |

`APPROVED` sits **before** `PROMOTED → INTEGRATED → RUNTIME VERIFIED` in the
state machine (`../DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md` §4).

## 5. `CRATE_01` — CP1 complete, approval pending

| Field | Value |
|---|---|
| Asset | **`CRATE_01`** |
| State | **`QA PASS` — CP1 COMPLETE, AWAITING APPROVAL** |
| CP2 | **NOT AUTHORIZED** |
| Working `.blend` | `Working/blender_src/crate/Crate_01_Working.blend` (130 858 B) |
| Current object | `Crate_01_Intact` (1 intact mesh) |
| Collections | `Crate_Intact` (1 object), `Crate_Fragments` (**empty**) |
| Dimensions | 1.000 × 0.750 × 0.757 m (height +0.007 = nail-head protrusions) |
| Intact tris | 740 (budget 400–800) |
| Geometry QA | 0 ngons / 0 loose / 0 duplicate verts / 0 non-manifold / **0 wood-to-wood intersections** |
| Material | `MAT_crate_01_wood_metal`, 1 slot, `Col` FLOAT_COLOR/CORNER, UVMap |

> **CP2 IS NOT AUTHORIZED.** The recorded stop point is
> `CRATE_01 CP1 COMPLETE — AWAITING USER APPROVAL`. CP2 must not begin without
> explicit user authorization.
>
> Note: `Crate_01_Intact_FINAL.fbx` (29 052 B) and `Crate_01_Fragments_FINAL.fbx`
> (57 404 B) exist on disk, but **`Crate_Fragments` is empty in the working
> blend** and approval is pending. The presence of a `_FINAL` filename does not
> authorize CP2. See `../History/OPEN_DECISIONS.md` `DEC-05`.

---

## 1. Characters

| Asset | Location | State | Evidence |
|---|---|---|---|
| `WWG_Foundation_Source` (base) | `Documents\...\WildWest\` | `QA PASS` | 32-variant lineage, measurement docs |
| `WWG_Foundation_Source` — 31 variants | same | `QA PASS` | `.blend` files 2026-09-21…22 |
| `WWG_Doomy_Cowboy_WORK` | `...\WWG_Doomy_Cowboy\` | `QA PASS` | QA renders exist |
| `WWG_Doomy_Cowboy_APPROVAL_CANDIDATE` | same | `CREATED` | named candidate, not approved |
| `WWG_Doomy_Cowboy_REFINED` | same | `QA PASS` | `WWG_Doomy_Cowboy_Vest_Inspection_Collage.png` |
| `WWG_Doomy_Cowboy_VEST_APPROVAL_CANDIDATE` | same | `CREATED` | candidate, not approved |
| `WWG_Doomy_Cowboy_VEST_WORK` | same | `QA PASS` | `Gloves_Repair/*`, `Vest_Repair/*` |
| `WWG_Doomy_Cowboy_VEST_MARK1_REPAIR_SB_WORK` | same | `QA PASS` | MarkFix renders |
| `WWG_Doomy_Cowboy_VEST_FULL_REPAIR_SB_WORK` | same | `QA PASS` | QA renders |
| `WWG_Doomy_Cowboy_FINAL_REPAIR_SB_WORK` | same | `QA PASS` | QA renders |
| `..._FINAL_REPAIR_SB_WORK_PRESAVE_20260926_053821` | same | `CREATED` | 16.2 MB presave, no QA |
| `WWG_Doomy_Cowboy_CLOTHING_REBUILD_SB_WORK` | same | `QA PASS` | newest, 2026-09-26 12:02 |
| Character FBX ×5 (Player/Bandit/Rusher/Shooter/Tactical) | `FBX/` + `Unity_Export/Characters/` | `QA PASS` | export exists, **not integrated** |
| Weapon FBX ×4 (Carbine/Revolver/Shotgun/Winchester) | `FBX/` + `Unity_Export/Weapons/` | `QA PASS` | export exists, **not integrated** |

> "QA PASS" here means QA renders/documentation exist — **not** that a human
> approved the asset. `APPROVED` is a human act and has not occurred for any
> character asset.

---

## 2a. E1 — Hat (third-party, approved with fit caveat)

> **Added 2026-09-30.** Registered as `APPROVED WITH FIT CAVEAT` — a human visual
> approval was recorded by the project owner. State per `../DOC-0001` §4
> (`CREATED → QA PASS → APPROVED → PROMOTED → INTEGRATED → RUNTIME VERIFIED`);
> this asset is at **`APPROVED`**, not yet `PROMOTED` / `INTEGRATED` / `RUNTIME VERIFIED`.

| Field | Value |
|---|---|
| Asset | **E1 — Hat** (third-party asset used as an adapted project asset) |
| **State** | **`APPROVED WITH FIT CAVEAT`** (user-confirmed, 2026-09-30) |
| Source asset | **Cowboy Hat** by **MadeByYeshe** — **CC BY 4.0** (third-party; see `ART-0004`) |
| Working `.blend` | `Working/blender_src/third_party/MadeByYeshe_CowboyHat/Working/_chk_user_manual_fit_final.blend` |
| Assembly root | `WWG_ThirdParty_Hat_Root` — parts `Hat`, `Strap`, `Buttons`, **synchronised** |
| Final uniform scale | `1.62398` |
| Final rotation | `(−1.278°, 7.332°, 80.083°)` |
| Final location | `(−3.538, −42.303, 1702.868) mm` |
| Geometry QA | **0 BVH triangle-level intersections**; 0 non-manifold; 0 zero-area; 0 duplicate (checked state) |
| Head coverage | 0 / 162 sampled head points outside the hat silhouette |
| Low-poly | **NOT performed** (deferred to a separate pass) |
| Full asset record | **`ART-0004-THIRD-PARTY-ASSET-CATALOG.md` §5.1** |
| Causal history | `../History/HISTORY-0004-E1-HAT-DEVELOPMENT-HISTORY.md` |

> **FIT CAVEAT (must travel with this asset).** The hat is **visually approved but not
> geometrically ideal**: a visible inner head-to-hat gap remains (recorded minimum inner
> clearance **≈ 4.21 mm**), and visual acceptance is based on the **intended upper / game
> camera**. Do **not** describe it as `perfect fit`, `perfectly flush` or `zero gap`.
> Correct wording: **`VISUALLY APPROVED WITH FIT CAVEAT` / `TECHNICALLY USABLE` /
> `NON-ZERO INNER GAP REMAINS`.**
>
> Intersection QA is clean (**0**); the caveat concerns the inner gap, not a collision.

---

## 2b. E1.1 — Hat low-poly refinement

> **Added 2026-09-30.** `E1` (§2a) remains the **approved base**. `E1.1` is a
> **refinement of that approved asset, not a redesign**, and is
> **`READY FOR APPROVAL`**. It does **not** supersede E1 until human approval is recorded.

| Field | Value |
|---|---|
| Asset | **E1.1 — Hat (low-poly refinement of E1)** |
| **State** | **`READY FOR APPROVAL`** (awaiting human approval) |
| Base asset | E1 — Hat (§2a), `APPROVED WITH FIT CAVEAT` |
| Working `.blend` | `Working/blender_src/third_party/MadeByYeshe_CowboyHat/Working/WWG_ThirdParty_CowboyHat_LOWPOLY_E1_1.blend` |
| SHA-256 | `918ED9419725884ABA7B1C821AE7964763854EFAD051BDE03D59812F843A7315` |
| Source / licence | unchanged — Cowboy Hat by MadeByYeshe, **CC BY 4.0** (`ART-0004` §5.1.0) |
| Geometry | 4184 → **2780 tris** (−33.6 %); 2166 → **1464 verts** (−32.4 %) |
| Method | vertex-group-weighted `Decimate → COLLAPSE` (0.65 / 0.70), applied |
| Fit QA | **0 BVH intersections**; 0/162 head points outside silhouette; min clearance 4.99 mm |
| Topology QA | 0 non-manifold / 0 zero-area / 0 loose / 0 degenerate normals; source defects in `Strap`/`Buttons` preserved |
| Report | `Working/reports/e1_1_lowpoly_adaptation_2026-09-30.md` |

> Fit caveat continues to apply — see §2a. Low-poly did **not** introduce any
> intersection and the Character Foundation was **not** modified.

---

## 2. Garments (vest / gloves)

| Asset | Location | State | Evidence |
|---|---|---|---|
| Vest body | `WWG_Doomy_Cowboy` | `QA PASS` | `Vest_VisualQA_2x2` and 11 further renders |
| Vest shirt-fit pass | same | `QA PASS` | `Vest_ShirtFit_FinalQA_2x2`, `_TopQA_2x2`, `_Closeup` |
| Vest technical QA | same | `QA PASS` | `Vest_TechnicalQA_REVISION_2x2` |
| Vest reference-accurate low-poly | same | `QA PASS` | `Vest_ReferenceAccurate_LowPoly_*` |
| Vest final correction | same | `QA PASS` | `Vest_FinalCorrection_2x2`, `_Closeup`, `_TopQA_2x2` |
| Vest upper shoulder | same | `QA PASS` | `Vest_UpperShoulder_TopQA_2x2` |
| Vest repair pass | `Desktop\Коллаж тест VVG\01_Vest\Vest_Repair\` | `QA PASS` | 4 renders |
| Vest final repair (SB) | `…\FinalRepair_SB\` | `QA PASS` | 1 render |
| Western shooting gloves | `Desktop\…\01_Vest\` | `QA PASS` | `WesternShootingGloves_VisualQA_2x2`, `_FinalCorrection_2x2` |
| Gloves repair | `…\Gloves_Repair\` | `QA PASS` | `Gloves_Repair_2x2`, `Gloves_MarkFix_2x2` |
| Gloves final repair (SB) | `…\Gloves_FinalRepair_SB\` | `QA PASS` | 1 render |

---

## 3. Environment — modular kit

| Asset | Location | State | Blocker |
|---|---|---|---|
| `WallSegment_01_FINAL.fbx` | `Assets/Art/Environment/Modular/` | `QA PASS` | `UNI-D06` import at 0.01× |
| `WallSegment_01_r2.fbx` | same | `QA PASS` | `UNI-D06` |
| `WallSegment_01_r3.fbx` | same | `QA PASS` | `UNI-D06` |
| `FloorSegment_01_FINAL.fbx` | same | `QA PASS` | `UNI-D06` |
| `FloorSegment_01_r2.fbx` | same | `QA PASS` | `UNI-D06` |
| `FloorSegment_01_r3.fbx` | same | `QA PASS` | `UNI-D06` |
| **FINAL selection (Wall/Floor)** | — | **OPEN DECISION `DEC-04`** | see `../History/OPEN_DECISIONS.md` |
| Modular integration in scene | — | **not started** | 0 modular meshes in scene |

> **STATIC VERIFIED:** the three wall revisions and three floor revisions listed
> above are **STATICALLY CONFIRMED PRESENT on disk** (exact filenames, 6 FBX +
> 6 `.meta`). Each carries `_FINAL` in its name.
>
> **That is a filename, not an approval.** No `APPROVED` record exists for any
> of them, and none is integrated (`UNI-D11`). The `FINAL` suffix is precisely
> why `DEC-04` must be decided by a human rather than inferred.

---

## 4. Props

| Asset | Location | State | Note |
|---|---|---|---|
| Controlled-fracture props (FBX) | `Assets/Art/Props/` | `QA PASS` | 0 instances in scene (`UNI-D12`) |
| `CRATE_01` | `Working/blender_src/crate/` | `QA PASS` — CP1 complete | **CP2 NOT AUTHORIZED** - see §5 and `DEC-05` |
| `Cover` r4 / r5 | same | `QA PASS` | **status is an OPEN DECISION** |

---

## 5. Unity art dependencies (untracked)

| Path | Count | Git | Note |
|---|---|---|---|
| `Assets/Prefabs/Environment/WWG_*.prefab` | 14 in art source | **untracked** | required by scene |
| `Assets/Materials/WWG_*.mat` | 7 in art source | **untracked** | required by scene |
| `Assets/TestArena.unity` | 1 | **untracked** | duplicate scene, REVIEW |

---

## 6. Environment / prop materials in scene

| Prefab | Material | Problem |
|---|---|---|
| `Shooter.prefab` | `TMP_SDF-HDRP LIT` | font material on a mesh renderer (`UNI-D08`) |
| `Rusher.prefab` | `FrameDebuggerRenderTargetDisplay` | debug material never replaced (`UNI-D09`) |

---

## 7. Promotion blockers summary

Nothing can be promoted beyond `QA PASS` until:

1. `UNI-D06` (FBX import scale/orientation) is fixed — blocks all environment art.
2. `UNI-D04` / `UNI-D05` (broken refs, missing scripts) are enumerated.
3. A human `APPROVED` decision exists per asset.
4. The repository material policy is decided.

---

**End of `ART-0002-ART-ASSET-REGISTER.md`**
