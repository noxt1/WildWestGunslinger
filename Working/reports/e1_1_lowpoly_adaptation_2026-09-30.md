# E1.1 — Low-Poly Adaptation of Approved E1 Hat

**Date:** 2026-09-30
**Status:** **`E1.1 LOW-POLY — READY FOR APPROVAL`**
**Nature:** refinement of an approved asset — **not** redesign

> `E1` = approved base (unchanged, still the approved visual truth)
> `E1.1` = low-poly refinement (this pass, awaiting human approval)

---

## 1. CURRENT E1 SOURCE

| Item | Value |
|---|---|
| Approved E1 source (BEFORE) | `Working/blender_src/third_party/MadeByYeshe_CowboyHat/Working/_chk_user_manual_fit_final.blend` |
| E1.1 output (AFTER) | `Working/blender_src/third_party/MadeByYeshe_CowboyHat/Working/WWG_ThirdParty_CowboyHat_LOWPOLY_E1_1.blend` |
| E1.1 SHA-256 | `918ED9419725884ABA7B1C821AE7964763854EFAD051BDE03D59812F843A7315` |
| Size | 381 101 B |
| BEFORE preservation copy | `_chk_e1_approved_before_lowpoly.blend` (approved E1 kept intact) |
| Work method | **interactive Blender** (Blender MCP, live session) — no headless modeling |

Fit transform was **not** re-derived and **not** changed: root
`WWG_ThirdParty_Hat_Root`, uniform scale `1.62398`, rotation
`(−1.278°, 7.332°, 80.083°)`, location `(−3.538, −42.303, 1702.868) mm`.

---

## 2. LOW-POLY METHOD

### 2.1 Measured first (§16)

Current geometry was measured before any target was chosen. No arbitrary polygon count
was imposed. The density distribution showed:

| Zone (radius) | Polys | Reading |
|---|---:|---|
| 0–20 mm | 8 | crown top / crease apex |
| 20–40 mm | 28 | crown upper |
| 40–60 mm | 190 | crown wall |
| 60–80 mm | 440 | **highest density** — crown/band transition |
| 80–100 mm | 274 | band / inner brim root |
| 100–120 mm | 64 | brim |
| 120–140 mm | 300 | brim curl |
| 140–160 mm | 300 | brim outer field |
| 160–180 mm | 68 | brim edge |

### 2.2 Visual-importance weighting (§8)

Blind decimation was **not** used. A vertex group `LP_Keep` was built so the polygon
budget follows visual importance rather than uniform thinning:

| Zone | Weight | Rationale |
|---|---:|---|
| `crease_top` (crown, r < 72 mm) | **1.00** | crease + crown top are silhouette-critical and the approved western identity |
| `brim_edge` (r ≥ 86 % of r_max) | **1.00** | defines the outer silhouette and the brim curl |
| `band` (local Y 40–61 mm) | **0.95** | visible band + buckle readability |
| `crown` (wall) | **0.92** | crown readability |
| `brim_flat` (low-curvature field) | **0.42** | safe to thin — flat, low curvature |
| `brim_under` (underside, y < 22 mm) | **0.30** | largely hidden; thinned hardest |

`Strap` and `Buttons` are visible band/buckle elements and were weighted by radius
(outer edge 1.00, inner 0.55) rather than thinned blindly.

### 2.3 Ratio selection by visual validation (§7)

`Decimate → COLLAPSE` with `use_collapse_triangulate` was used as an **intermediate
tool**, then validated. Ratios were tested and the crown was inspected close-up at each:

| Ratio | Tris | Crown close-up | Verdict |
|---|---:|---|---|
| 0.55 | 1596 | **dark blotches / non-deliberate faceting** | **REJECTED** |
| 0.60 | 1742 | clean | acceptable |
| **0.65** | **1886** | **clean** | **SELECTED** |
| 0.72 | 2090 | clean | accepted, minimal gain |

`use_collapse_triangulate` also guarantees a **consistent triangulation** (no
triangulation artefacts, §20), at the cost of converting quads to tris — which is why
the *polygon* count rises while the *triangle* count falls. This is reported honestly
rather than hidden.

Modifiers were **applied**, so the result is real production geometry, not a live stack.

---

## 3. BEFORE / AFTER

| Metric | BEFORE | AFTER | Change |
|---|---:|---:|---|
| **Vertices** | 2166 | **1464** | **−32.4 %** |
| **Triangles** | 4184 | **2780** | **−33.6 %** |
| Polygons | 2124 | 2780 | +30.9 % (all-quads → all-tris, deliberate) |
| Material slots | 3 (1 per object) | 3 (1 per object) | unchanged |
| Live modifiers | 0 | 0 | unchanged |

### Per object

| Object | Verts before → after | Tris before → after | Non-manifold before → after | Shells |
|---|---|---|---|---|
| `Hat` | 1454 → **945** | 2904 → **1886** | 0 → **0** | 1 → 1 |
| `Strap` | 320 → **233** | 576 → **402** | 64 → **64** | 1 → 1 |
| `Buttons` | 392 → **286** | 704 → **492** | 64 → **64** | 8 → 8 |

**Geometry reduction achieved while silhouette, crown readability, brim readability and
western curvature were preserved** — the stated objective, not "lowest tri count".

---

## 4. SOURCE-DEFECT HANDLING (recorded, not hidden)

The low-poly pass was required by `ART-0004` §5.1.1 to *record what it repairs*.

| Object | Defect | E1.1 outcome |
|---|---|---|
| `Strap` | 64 non-manifold / 64 boundary edges | **NOT repaired — preserved unchanged** |
| `Buttons` | 64 non-manifold / 64 boundary edges, 8 components | **NOT repaired — preserved unchanged** |
| `Hat` | 0 non-manifold | stayed **0** |

The decimation **neither fixed nor worsened** the source defects: the counts are
identical before and after. They remain open items, correctly attributed to the
**source asset** (`ART-0004` §5.1.1), not to WWG. Repairing them would be a separate,
explicitly scoped task.

---

## 5. VISUAL QA

BEFORE and AFTER rendered from an **identical camera rig, framing, pose and lighting**
(`LP_BEFORE_*` / `LP_AFTER_*`, same `LP_Cam`, same EEVEE settings), so differences are
attributable to the low-poly refinement rather than camera variation.

Views captured for both sets: **front, side, 3/4 front, 3/4 rear, rear, top, underside,
close-up** — plus tight crown close-ups at ratios 0.55 / 0.60 / 0.65 / 0.72.

| Check | Result |
|---|---|
| Silhouette (front / side / rear) | **preserved** |
| Crown readability + crease ridge | **preserved** — crease still reads frontally and in 3/4 |
| Brim curl / wave / wing | **preserved** — no flattening, no taco/sombrero |
| Band + buckle readability | **preserved** — centre buckle and two side buckles read |
| Brim thickness | **preserved** — physical, not paper-thin |
| Underside | **clean** — no black faces, no collapsed brim |
| Shading | **no broken normals, no seams, no unintended smoothing** |
| Triangulation artefacts | none (uniform triangulation by construction) |
| Black / collapsed faces | none |
| Crown pinching | none observed |

Faceting present is limited, deliberate and confined to low-curvature fields.

---

## 6. FIT QA

Re-run with **BVH triangle-level** intersection testing (the method validated in E1 —
vertex-to-vertex distance is **not** used as intersection proof).

| Check | Expected | Actual | Result |
|---|---|---|---|
| `Hat` intersections | 0 | **0** | PASS |
| `Strap` intersections | 0 | **0** | PASS |
| `Buttons` intersections | 0 | **0** | PASS |
| **Total intersections** | **0** | **0** | **PASS** |
| Head points outside hat silhouette | 0 | **0 / 162** | PASS |
| Head points above hat top | 0 | **0** | PASS |
| Hat world Z | — | 1673.8 → 1869.9 mm | — |
| Head top | 1818.26 mm | 1818.26 mm | — |
| Min internal clearance | caveat retained | **4.99 mm** | see caveat |

**No new intersection was introduced by the low-poly conversion.** The fit was **not**
"fixed" by moving or scaling the character, and the Foundation was **not** altered.

> **FIT CAVEAT (unchanged, §19).** Minimum internal clearance is now **4.99 mm**
> (was ≈ 4.21 mm before low-poly; the small increase is a consequence of vertex
> collapse, not a fit change). It remains accepted for the intended camera.
> **Not** `perfect fit`, **not** `perfectly flush`, **not** `zero gap`.

---

## 7. TOPOLOGY QA

| Check | `Hat` | `Strap` | `Buttons` |
|---|---|---|---|
| Non-manifold edges | **0** | 64 *(source)* | 64 *(source)* |
| Boundary edges | **0** | 64 *(source)* | 64 *(source)* |
| Zero-area faces | **0** | **0** | **0** |
| Loose vertices | **0** | **0** | **0** |
| Degenerate normals | **0** | **0** | **0** |
| Live modifiers left | 0 | 0 | 0 |
| Material slots | 1 | 1 | 1 |

PASS is stated **only** against these measurements, not self-asserted.

---

## 8. LICENSE

| Field | Value |
|---|---|
| Asset | **Cowboy Hat** |
| Creator | **MadeByYeshe** |
| License | **CC BY 4.0** |
| License URL | `https://creativecommons.org/licenses/by/4.0/` |
| Source URL | `https://sketchfab.com/3d-models/cowboy-hat-15fd37f4e03c447b995b5851cac52801` |
| E1.1 classification | **MODIFIED / ADAPTED THIRD-PARTY DERIVATIVE** — **not** reclassified as an original WWG asset |
| Original attribution | **unchanged**, retained in `ART-0004` §5.1.3 |
| `internal_ground_ao_texture.jpeg` | **`LICENSE UNVERIFIED` / `EXCLUDED FROM PRODUCTION`** — not introduced, status unchanged |

---

## 9. FOUNDATION SAFETY

| Item | State |
|---|---|
| `HumanM_BodyMesh` | **UNCHANGED** |
| `WWG_Rig` (53 / 52 / 1) | **UNCHANGED** |
| Weights, bones, transforms | **UNCHANGED** |
| Foundation `.blend` / hash `B14CE5A1…` | **UNTOUCHED** — never opened for writing in this pass |
| Decimate / remesh / retopo / smoothing applied to Foundation | **NONE** |

The hat adapted to the Foundation; the reverse never happened.

---

## 10. CHECKPOINTS

| File | Role |
|---|---|
| `_chk_user_manual_fit_final.blend` | approved E1 (untouched) |
| `_chk_e1_approved_before_lowpoly.blend` | BEFORE copy taken at start of E1.1 |
| `WWG_ThirdParty_CowboyHat_LOWPOLY_E1_1.blend` | **E1.1 result** |
| `_chk_user_manual_fit_before_microfix.blend` | earlier E1 checkpoint (historical) |

Renders: `Working/renders/ThirdParty_CowboyHat/LP_BEFORE_*.png`, `LP_AFTER_*.png`,
`LP_CROWN_r55/r60/r65/r72.png` (ratio-selection evidence).

---

## 11. NOT PERFORMED

No geometry redesign · no crease/dent/sculpt work · no retopology · no voxel/aggressive
remesh · no subdivision cycles · no material or UV change · no texture change · no new
material slots · no change to texture provenance · no unverified file introduced · no
Character Foundation change · no Unity integration · no FBX export.

---

# `E1.1 LOW-POLY — READY FOR APPROVAL`

`E1` remains the approved base. `E1.1` is a low-poly refinement awaiting human approval
and does not supersede E1 until that approval is recorded.
