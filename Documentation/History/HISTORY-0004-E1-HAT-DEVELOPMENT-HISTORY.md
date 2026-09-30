# HISTORY-0004 — E1 Hat Development History

**Document ID:** `HISTORY-0004`
**Status:** CANONICAL — historical record
**Date:** 2026-09-30
**Evidence class:** measured art documents + verified file facts

> **Purpose.** A compressed *causal* record of the E1 Hat work, written so that a future
> agent can avoid the dead ends and reuse the validated methods **without reading the chat
> history**. This is not a transcript. Intermediate parameter values and superseded
> numeric metrics are intentionally omitted; the reports in `Working/reports/` hold them.

---

## 1. Outcome

E1 — Hat is closed as **`APPROVED WITH FIT CAVEAT`**. The delivered asset is a
**third-party** model (`Cowboy Hat` by MadeByYeshe, CC BY 4.0) fitted to the approved
Character Foundation. No production geometry was redesigned. See
`../ART/ART-0004-THIRD-PARTY-ASSET-CATALOG.md` §5.1 for the asset record.

---

## 2. Phase 1 — procedural crown: repeated failure

The first approach was a fully procedural Cattleman crown. It failed repeatedly:

- egg / bowler silhouette, then Fedora-like, then box / pillbox;
- flat-lid top cap, a central "nipple", crater / X / star artefacts;
- missing western shoulders;
- brim reading as horns, sombrero or taco;
- overall primitive-looking geometry.

**Root conclusion:** repeated parameter tuning could **not** reliably produce the desired
Cattleman architecture.

---

## 3. Phase 2 — reference-first correction

User reference photos were introduced as the primary source of truth
(`Working/blender_reviews/approved_reference/123/1.jpg`, `2.jpg`, `3.jpg`).

Two corrections followed, both important:

1. **Camera orientation was wrong.** Front view is **`az = 270`**. Measurements taken from
   the earlier, incorrect view orientation are **invalidated**.
2. **A documented ratio was wrong.** The previously recorded `crown_h / crown_w = 0.830`
   was superseded by pixel-based reference measurement of **≈ 0.760** (reference views
   compared at ~0.758 / ~0.763).

**Lesson:** verify the camera convention *before* trusting any derived ratio.

---

## 4. Phase 3 — tooling upgrade to stop blind iteration

To avoid further blind procedural iteration, the Blender skill set was upgraded to
**cc-blender-skill v1.3.0** (30/30 skills), adding the harmonizer, quality-refinement,
reference-to-3D, reference validation and multiview tooling, an isolated Python
environment, and verified Blender MCP connectivity.

---

## 5. Phase 4 — architecture experiments (all failed)

Superellipse crowns, angular-gate / Gaussian dents, single-point creases,
surface-of-revolution limits, height-field attempts and normal-direction displacement were
tried. Systemic causes of failure:

- excessive reliance on arbitrary parameter tuning;
- Gaussian masks creating unnatural ridges;
- angular dent gates creating X / cross / star artefacts;
- single-point crease approaches;
- **z-only deformation on near-vertical walls** (ineffective by construction);
- inappropriate global smoothing;
- top-cap construction yielding flat / nipple-like forms;
- architecture optimised for metrics rather than visual identity.

Normal-direction deformation (`P' = P - D·n̂`) was the one measurable improvement: front
spread grew **≈ 3 mm → ≈ 25.9 mm**, yielding a real `crease → shoulder → dent → outer
crown` mesh structure rather than a shader illusion.

**Methodological lesson:** *representation architecture must be validated before extensive
parameter tuning.*

---

## 6. Phase 5 — the A/B/C breakthrough (decisive)

Three diagnostic variants isolated the variable that mattered:

| Variant | Description |
|---|---|
| **A** | production crown + uniform **inner shell** |
| **B** | clean **open crown block**, no inner shell |
| **C** | **B + validated Cattleman bash**, no inner shell |

**Result: `BLOCK → BASH VALIDATED`.**

- **B** confirmed the correct clean-crown architecture: sufficient crown height,
  controlled taper, near-vertical side walls, rounded structured top, oval footprint.
- **C** confirmed the working bash architecture: broad shoulders, finite-width crease
  floor, controlled dents, no spike, no X / star artefact.

---

## 7. Phase 6 — inner-shell discovery and adaptive shell

A under-performed C because a **uniform inner-shell offset** forced the crease/shoulder
region to compress excessively: the crease could occupy too large a fraction of the crown
width and the shoulder structure was lost.

**Conclusion:** the outer crown + bash architecture was correct; the limitation was the
**inner-shell construction**.

The fix was an **adaptive** shell, thickness driven by local curvature/concavity with a
clamped range:

```
shell_thickness(u, v) = clamp(0.40 * R_local, 1.2, 4.0)
```

This preserved the external crown architecture, crease, shoulders and internal clearance.
(This method is a validated tool, **not** a claim that it solves every future fit problem.)

---

## 8. Phase 7 — third-party asset investigation

The `Cowboy Hat` (MadeByYeshe) source was downloaded and audited
(`Working/reports/third_party_cowboy_hat_source_audit_2026-09-30.md`).

**Audit finding:** the model is **not a valid Cattleman reconstruction base** — crown too
low, no true Cattleman crease, no characteristic shoulder/dent architecture, and the brim
does not match the desired western form.

**But:** the project owner accepted the overall visual style and its usability as a
*fitted visual asset*. The distinction is recorded explicitly and must be preserved:

> `NOT A VALID CATTLEMAN RECONSTRUCTION BASE` ≠ `NOT USABLE AS A PROJECT ASSET`

Licensing is valid under **CC BY 4.0** with attribution — see
`../ART/ART-0004-THIRD-PARTY-ASSET-CATALOG.md` §5.1.

---

## 9. Phase 8 — clean Foundation reset

An inconsistent Foundation transform state appeared during fitting. The approved Foundation
was **reloaded from its canonical source** and verified (`B14CE5A1…`, 53/52/1 bones,
F8 Head Top **1818.2576 mm**) before any further fitting, to exclude accumulated error.

> **Note:** the approved body transform `rotation = (90°, 0°, 0°)`, `location = (0,0,0)`,
> `scale = 1` is **legitimate** and is not an error to be "fixed".

---

## 10. Phase 9 — manual user fit as the visual baseline (methodology)

The owner **manually positioned** the hat in the viewport. That placement became the
**primary visual fit baseline**. OpenCode's mandate was restricted to technical validation,
minimal correction and intersection cleanup — **never** to rebuild the visual placement.

This "user baseline first, agent corrects minimally" pattern is retained as approved
methodology for future manually-fitted visual assets.

---

## 11. Phase 10 — BVH correction (the decisive technical fix)

The owner marked an area where the head emerged through the hat. The earlier
vertex-to-vertex clearance check had reported a false "no intersection". A
**BVH triangle-level** test (`BVHTree.overlap`) found the real result:

**162 intersecting polygon pairs** (Hat 100, Strap 46, Buttons 16) — all in the **brim**
cutting the back of the skull, none in the crown.

Each correction axis was swept independently:

| Axis | Effect |
|---|---|
| rotation Y | made it **worse** |
| position Z lift | insufficient |
| **uniform scale** | **+8 % → 0 intersections** |

Single change applied — **uniform scale 1.50368 → 1.62398**, rotation and seating
untouched. Result: **162 → 0 intersections**, 0/162 head points outside the silhouette.

**Lesson:** vertex-to-vertex distance is **not** a valid intersection proof; use
face-level tests.

---

## 12. TripoSR — experiment, not a dependency

A local TripoSR smoke test on `1.jpg` produced an OBJ, but the result was judged
**poor quality / not suitable for production**. TripoSR remains **optional tooling**;
the E1 Hat does not depend on it. The experiment was not continued.

---

## 13. Permanently failed methods — do not repeat

- primitive cylinder + dome crown;
- generic superellipse crown as a direct Cattleman solution;
- single-vertex / single-point crease;
- arbitrary coordinate-range vertex pushes;
- angular dent gates; arbitrary Gaussian height fields;
- global `smooth_vert` as shape creation;
- `Subdivide → Smooth` as architecture;
- z-only deformation on vertical crown walls;
- solving outer-shape defects through inner-shell geometry;
- extreme uniform Z-scaling of a low crown;
- treating an unrelated generic cowboy hat as an assumed Cattleman source;
- resetting imported OBJ rotation to zero;
- using vertex-to-vertex clearance as final intersection proof;
- declaring numeric PASS without visual reference confirmation.

---

## 14. Validated methods — keep

- README-first / reference-first workflow;
- verify camera orientation before deriving ratios;
- actual mesh measurement over assumed values;
- open crown block first, bash validated separately;
- normal-direction deformation for front-wall relief;
- adaptive inner shell;
- user visual baseline as authoritative;
- BVH triangle-level intersection QA;
- synchronised transform root for multi-part assets;
- checkpoint before destructive operations;
- reversible production workflow; final visual + numeric QA.

---

## 15. Artifact index (key evidence)

Reports (`Working/reports/`):
`e1_hat_real_reference_fit_2026-09-30.md` · `e1_hat_cattleman_crown_form_pass_2026-09-30.md` ·
`e1_hat_abc_diagnostic_2026-09-30.md` · `e1_hat_production_candidate_2026-09-30.md` ·
`third_party_cowboy_hat_source_audit_2026-09-30.md` ·
`e1_third_party_hat_clean_fit_final_2026-09-30.md` ·
`e1_third_party_hat_user_fit_validation_2026-09-30.md`

Checkpoints (`Working/blender_src/third_party/MadeByYeshe_CowboyHat/Working/`):
`_chk_user_manual_fit_final.blend` (current accepted state) ·
`_chk_user_manual_fit_before_microfix.blend` · `_chk_thirdparty_crown_adapted.blend` ·
`_chk_thirdparty_hat_raw_on_foundation.blend` · `_chk_clean_scene_foundation.blend`

> Superseded numeric fit metrics from earlier FIT / FIT2 / crown-adaptation passes are
> **intentionally not carried forward** here. Current truth lives in
> `../ART/ART-0004` §5.1 and the user-fit validation report.

---

## 16. Lessons learned

1. Architecture before tuning.
2. Measure the mesh; do not assume — and re-derive ratios from the correct camera.
3. Isolate variables (A/B/C) instead of multi-parameter iteration.
4. Respect the user's visual baseline; correct minimally and reversibly.
5. Use correct QA methods — face-level intersection, not vertex distance.
6. A third-party asset can be unusable as a *reconstruction base* yet fully usable as a
   *production asset*; record both facts instead of collapsing them.

---

## 17. Next stage

**E2 — Torso Clothing / Shirt.** It must treat the approved Character Foundation as
immutable. See `../ART/ART-0004` for the open third-party/licensing questions carried
forward.

---

**End of `HISTORY-0004-E1-HAT-DEVELOPMENT-HISTORY.md`**
