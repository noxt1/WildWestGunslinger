# ART-0004 — Third-Party Asset Catalog

**Document ID:** `ART-0004`
**Status:** CANONICAL
**Date:** 2026-09-30
**Evidence class:** verified file facts (archive hashes on disk) + recorded user
approval + licence terms published by the source platform

---

## 1. Purpose

This is the **single canonical catalog** of third-party assets used in WildWestGunslinger
production. It exists so that any asset can be traced, in one place, through:

```
USED ASSET → CREATOR → SOURCE → LICENSE → LICENSE TERMS → ATTRIBUTION
           → WWG ASSET PATH → MODIFICATION STATUS → PRODUCTION STATUS
```

It is required for release preparation, marketplace submission, attribution obligations,
licence audit and long-term maintenance. **This document is the only licence registry for
the project.** No parallel registry may be created.

> **This is a documentation registry, not a licence store.** It records the official
> licence *reference* for each asset. Where an official licence text is bundled with the
> source, it is stored under the asset's own `Original/` folder and referenced from here
> rather than re-typed, so the legal text is never paraphrased.

---

## 2. What counts as a third-party asset

| Counts | Does not count |
|---|---|
| Any externally authored model, texture, audio or font used in production | WWG-authored assets |
| Vendor character bases (e.g. the pristine `HumanCharacterDummy_M.blend`) | Reference images used for construction only (`ART-0001` §7) |
| Assets purchased, downloaded or licensed from an external creator | Measurement/QA documents authored in-project |

A third-party asset that enters production **without a record in this catalog is a
documentation defect** (`DOC-0001` §7 maintenance rules).

---

## 3. Required fields for every record

Every entry must carry all of:

Asset Name · Creator · Original Source · Original Source URL · License · License URL ·
Date Acquired · Original Archive / File Name · Source SHA-256 (if available) ·
Current WWG Asset Path · Modification Status · Attribution Requirement ·
Attribution Text · Production Status · Notes / Restrictions

---

## 4. ORIGINAL vs DERIVATIVE — mandatory distinction

Importing, re-transforming or adapting a model **does not make the source asset WWG-owned**.
Records must state which of the two they describe:

| Term | Meaning |
|---|---|
| **ORIGINAL THIRD-PARTY ASSET** | The unmodified work of the external creator, as distributed. Belongs to the creator under its licence. |
| **WWG DERIVATIVE / MODIFIED VERSION** | A WWG-modified working copy (fitted, adapted, re-exported). Remains governed by the original licence; WWG owns only the modifications. |

**WWG does not claim ownership of any ORIGINAL THIRD-PARTY ASSET.**

---

## 5. Catalog — used third-party production assets

### 5.1 Cowboy Hat

| Field | Value |
|---|---|
| **Asset name** | Cowboy Hat |
| **Creator / author** | **MadeByYeshe** |
| **Original source** | Sketchfab |
| **Original source URL** | `https://sketchfab.com/3d-models/cowboy-hat-15fd37f4e03c447b995b5851cac52801` |
| **License** | **CC BY 4.0** |
| **License URL** | `https://creativecommons.org/licenses/by/4.0/` |
| **Date acquired** | 2026-09-30 |
| **Original archive** | `Cowboy Hat.zip` (inner) · `cowboy-hat_downloads_root.zip` (outer download) |
| **Source SHA-256 (inner)** | `AE436A37F4B1FCB3299078367FCC080EF76160517E4D3A62281DB06CD36DD972` |
| **Source SHA-256 (outer)** | `957DE226852838D7431DE892C0750B9491353DDB2397652B46652A831FD72773` |
| **Current WWG asset path** | `Working/blender_src/third_party/MadeByYeshe_CowboyHat/` |
| **Classification** | **ORIGINAL THIRD-PARTY ASSET** (source, preserved read-only in `Original/`) + **WWG DERIVATIVE** (fitted working copy) |
| **Modification status** | **Adapted / fitted for WWG.** Transform-only fitting to the approved Character Foundation, assembly synchronisation under a shared root. **No topology, sculpt, retopology or shape redesign.** Low-poly adaptation planned, **not yet performed**. |
| **Attribution requirement** | **REQUIRED** — CC BY 4.0 mandates attribution |
| **Production status** | **`APPROVED WITH FIT CAVEAT`** — visually approved by the project owner 2026-09-30 |
| **Asset-register state** | `ART-0002-ART-ASSET-REGISTER.md` §2a |
| **Notes / restrictions** | See §5.1.1 (source defects) and §5.1.2 (fit caveat) |

#### 5.1.0 Derivative lineage

The asset exists as two distinct working states. Neither is an original WWG asset.

| State | File | Status |
|---|---|---|
| **E1 — approved base** | `_chk_user_manual_fit_final.blend` | `APPROVED WITH FIT CAVEAT` (2026-09-30) |
| **E1.1 — low-poly refinement** | `WWG_ThirdParty_CowboyHat_LOWPOLY_E1_1.blend` | `READY FOR APPROVAL` (2026-09-30) |

**E1.1 modification record** (derivative of the same CC BY 4.0 original):

| Item | Value |
|---|---|
| Modification date | 2026-09-30 |
| Nature | **visual-fidelity low-poly** — silhouette / crown / brim readability preserved |
| Method | `Decimate → COLLAPSE`, ratio 0.65 (Hat) / 0.70 (Strap, Buttons), **vertex-group weighted** by visual importance; modifiers applied |
| Geometry | 4184 → **2780 tris** (−33.6 %), 2166 → **1464 verts** (−32.4 %) |
| Topology | `Hat` 0 non-manifold before and after; `Strap`/`Buttons` source defects **preserved unchanged** (64 each) |
| Fit | **0 BVH intersections**; 0/162 head points outside silhouette; min clearance 4.99 mm |
| SHA-256 | `918ED9419725884ABA7B1C821AE7964763854EFAD051BDE03D59812F843A7315` |
| Report | `Working/reports/e1_1_lowpoly_adaptation_2026-09-30.md` |
| Classification | **MODIFIED / ADAPTED THIRD-PARTY DERIVATIVE** — attribution in §5.1.3 unchanged and still required |

#### 5.1.1 Source-asset defects — inherited, not WWG-authored

The raw OBJ carries topology defects. These belong to the **source asset** and were
**deliberately not silently rewritten** in the preserved `Original/` copy:

| Object | Defect |
|---|---|
| `Strap` | 64 non-manifold edges |
| `Buttons` | 64 non-manifold edges, 8 connected components |
| `Hat` | 0 non-manifold edges |

These must **not** be recorded as WWG-authored defects. They remain relevant to any future
low-poly pass, which must record what it repairs.

#### 5.1.2 Fit caveat — must travel with the asset

The hat is **visually approved but not geometrically ideal**:

- A visible inner head-to-hat gap remains (recorded minimum inner clearance **≈ 4.21 mm**).
- Visual acceptance is based on the **intended upper / game camera** presentation.
- Intersection QA is clean: **0 body/hat intersections** (BVH triangle-level test).
- Do **not** describe this asset as `perfect fit`, `perfectly flush` or `zero gap`.

Correct wording: **`VISUALLY APPROVED WITH FIT CAVEAT` / `TECHNICALLY USABLE` /
`NON-ZERO INNER GAP REMAINS`.**

#### 5.1.3 Attribution text (ready to use)

```
Cowboy Hat by MadeByYeshe
Source: https://sketchfab.com/3d-models/cowboy-hat-15fd37f4e03c447b995b5851cac52801
License: Creative Commons Attribution 4.0 (CC BY 4.0)
License text: https://creativecommons.org/licenses/by/4.0/
Used in WildWestGunslinger in an adapted/modified form (fitted to the project
character foundation). Changes are documented in
Documentation/History/HISTORY-0004-E1-HAT-DEVELOPMENT-HISTORY.md.
```

#### 5.1.4 Unverified file inside the source archive

| File | Status |
|---|---|
| `internal_ground_ao_texture.jpeg` | **`LICENSE UNVERIFIED` — `EXCLUDED FROM PRODUCTION`** |

A file present inside a third-party archive is **not automatically covered by the licence of
the main asset**. This file's provenance and separate licensing status are **not confirmed**.

**Do not use this file in any production WWG asset without separate verification of its
origin and rights.** It is not required by the accepted hat, and is therefore excluded.

---

## 6. Vendor character base — reference lineage

| Field | Value |
|---|---|
| **Asset** | `HumanCharacterDummy_M.blend` (Kevin Iglesias human dummy, size M) |
| **Role** | **Vendor source**, pristine and read-only — historical reference only |
| **SHA-256** | `6D6DB2666CC6214DF7E71A955A5861A8E9A460C6310246E733694EABB9727D8E` |
| **Status** | Basis of the approved Character Foundation (`PROJECT_TRUTH.md` §7) |
| **Note** | The **approved** Character Foundation is a WWG production asset; this entry records only the vendor lineage. Full licence provenance for the vendor base is **not yet recorded** — see `History/OPEN_DECISIONS.md`. |

---

## 7. Rules for using this catalog

1. **One registry.** Extend this document; never create a second licence/attribution file.
2. **No paraphrasing.** Cite the official licence URL; do not retype legal text.
3. **Derivatives stay labelled.** A fitted/modified copy is a derivative, not a new original.
4. **Unverified files are quarantined.** Record them with `LICENSE UNVERIFIED` and
   `EXCLUDED FROM PRODUCTION` until provenance is confirmed.
5. **Pre-release audit.** Before any marketplace/release preparation, run a full
   third-party audit: every production asset must have creator, source, licence,
   attribution text, path and status. A third-party asset in production without a record
   here is a **documentation defect**.

---

## 8. Cross-references

- `ART-0001` — art programme state
- `ART-0002` — per-asset register (state machine)
- `ART-0003` — pipeline and QA gates
- `History/HISTORY-0004` — E1 Hat causal development history
- `History/OPEN_DECISIONS.md` — unresolved human decisions
- `PROJECT_TRUTH.md` §7 — art truth, incl. approved Character Foundation

---

**End of `ART-0004-THIRD-PARTY-ASSET-CATALOG.md`**
