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

## 6.1 EXTERNAL CHARACTER ASSET AUDIT — 2026-10-07 (NOT ADMITTED)

> **Scope.** A separate, read-only audit of character assets downloaded by the
> project owner and stored **outside Unity**, in a personal desktop folder
> (external location, deliberately not recorded as an absolute path — see
> §6.1.1). This is **not** a continuation of the
> Quaternius feasibility experiment recorded in `PROJECT_TRUTH.md` — "QUATERNIUS
> FINAL VERDICT" and **not** a production import.
>
> **Admission status: `NOT ADMITTED`.** Nothing in §6.1 has been copied into
> `Assets/`, imported into Unity, modified, or added to Git. The source files
> remain byte-for-byte unchanged.
>
> This section is the licence registry entry required by §3 for assets that were
> evaluated and **rejected or deferred**. Per §7 rule 1, the audit result is
> recorded here rather than in a parallel registry.

### 6.1.1 Source location (external resource, not project-relative)

| Property | Value |
|---|---|
| Kind | **External Resource** — a personal desktop folder outside the repository and outside Unity |
| Publisher / origin | Quaternius character asset pack (vendor), as downloaded by the project owner |
| Absolute path | **Deliberately not recorded.** It contains the owner's user name and local disk layout, so it is not published. The path exists only in the owner's local environment. |
| Locator for internal use | Quaternius character pack folder, desktop root, audit date 2026-10-07 |

Per `../DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md` §8 this is an **External
Resource**. Nothing under this source is in the repository, is tracked by Git, or
is a Unity project folder. Every file below is identified by name and SHA-256, so
the audit is reproducible without disclosing the local path.

### 6.1.2 Full inventory — 8 files, 3 entries, SHA-256 verified

Folder root contains 3 entries; subfolder `надо проверить\` contains 5 files.

| # | File | Ext | Bytes | Modified | SHA-256 (first 16) |
|---|---|---|---:|---|---|
| 1 | `Universal Base Characters[Standard].zip` | zip | 128 968 391 | 2026-10-06 20:14:10 | `FDBF1804C90DFC1E` |
| 2 | `Modular Character Outfits - Fantasy[Standard].zip` | zip | 294 347 394 | 2026-10-06 20:14:38 | `C3468B18871CC8C8` |
| 3 | `надо проверить\Stylized Nature MegaKit[Standard].zip` | zip | 104 088 529 | 2026-10-06 20:35:43 | `298F6732B872E4CF` |
| 4 | `надо проверить\bandit (1).fbx` | fbx | 6 534 672 | 2026-10-06 23:03:25 | `6FE6644556FE6BB0` |
| 5 | `надо проверить\bandit (1).mb` | mb | 13 798 000 | 2026-10-06 23:03:20 | `EDEF076F163A6863` |
| 6 | `надо проверить\bandittextures (1).zip` | zip | 523 344 912 | 2026-10-06 23:05:57 | `7C1AB8901572A572` |
| 7 | `надо проверить\banditunity (1).unitypackage` | unitypackage | 534 657 293 | 2026-10-06 23:06:53 | `67BC807323C4F93A` |
| 8 | `надо проверить\cowboyelderly.unitypackage` | unitypackage | 533 500 312 | 2026-10-06 22:57:43 | `080B05B84D238FE5` |

Archive contents census (read-only listing, no extraction to `Assets/`):

| Archive | Entries | Character content | Environment content |
|---|---:|---|---|
| `Universal Base Characters[Standard].zip` | 128+ | 2 FBX (`Superhero_Male/Female_FullBody`), 8 hairstyle FBX sets, 4 glTF variants | — |
| `Modular Character Outfits - Fantasy[Standard].zip` | 161+ | 22 FBX (Ranger/Peasant Body/Arms/Legs/Feet/Acc) | — |
| `Stylized Nature MegaKit[Standard].zip` | 459 | — | 79 FBX + OBJ/glTF (trees, rocks, grass, flowers) |
| `bandittextures (1).zip` | 70 | 68 `.tga` (Ban* + Colt texture sets) | — |
| `banditunity (1).unitypackage` | 466 tar members / **96 assets** | 3 FBX, 7 prefabs, 19 `.mat`, 58 `.tga`, 1 `.unity`, 1 `.shader` | — |
| `cowboyelderly.unitypackage` | 383 tar members / **85 assets** | 2 FBX, 4 `.mesh`, 3 prefabs, 15 `.mat`, 29 `.tga`, 1 `.psd`, 2 `.unity` | 1 arena FBX + 7 arena `.mat` |

**No README / LICENSE / COPYING / EULA file exists inside any `.unitypackage`
or inside the Bandit ZIPs.** Licence files are present **only** in the two
Quaternius ZIPs (`License_Standard.txt`).

### 6.1.3 BANDIT — AlexN

| Field | Value |
|---|---|
| Asset name | **Bandit** |
| Creator / author | **AlexN** (Unity Asset Store publisher id 20710) |
| Original source | Unity Asset Store |
| Original source URL | `https://assetstore.unity.com/packages/3d/characters/humanoids/bandit-185326` |
| Publisher URL | `https://assetstore.unity.com/publishers/20710` |
| License | **Standard Unity Asset Store EULA** |
| License URL | `https://unity.com/legal/as-terms` (Terms last updated 2024-12-04) |
| Declared license type | `Extension Asset` (page field "License type: Extension Asset") |
| Store file size (declared) | 537.9 MB — local `.unitypackage` 534 657 293 B = 509.9 MiB |
| Store version / date | 1.0 · Aug 25, 2025 · original Unity 2021.3.45 |
| Price at audit | **FREE** |
| Local artefact set | `banditunity (1).unitypackage`, `bandit (1).fbx`, `bandit (1).mb`, `bandittextures (1).zip` |
| Attribution requirement | **NONE** under the Standard EULA (no credit clause). Voluntary credit permitted. |
| Production status | **`NOT ADMITTED`** |

##### Identity — content-based, not filename

Verified from content, three independent ways:

1. **Package path namespace**: `Assets/Bandit/**`, 96 assets, 3 FBX
   (`BanditMain`, `RevolverRig`, `RevolverBoneless`), 19 materials all prefixed
   `Ban*`/`RevolverMtl`, 58 `.tga` all `T_Ban*`/`T_Revolver_*`, 7 prefabs
   (`BanditMain`, `BanditMask`, `BanditNH`, `+WithWeapon` variants,
   `RevolverRig`, `RevolverBoneless`).
2. **Geometry matches the store listing**: store declares *verts 63 367 · faces
   62 568 · tris 112 857*. Measured on the packaged `BanditMain.fbx`:
   **82 866 verts / 82 011 faces / 140 355 tris across 39 mesh objects**
   (`fbx_audit.json`). The packaged FBX contains the character **plus the
   revolver rig mesh set**; excluding the 8 revolver objects
   (`Ammo`, `Cylinder`, `Ejector`, `Hammer`, `Main`, `Reload`, `RevolverDecor`,
   `Trigger`) leaves **77 856 verts / 76 781 faces / 130 579 tris across 31
   objects** (`agg_stats.json` → `bandit_character_only`). The store figure sits
   between the two — the store count is a subset selection, not the whole FBX.
3. **Store description matches the asset's own description text**:
   *"Wild west female character … Mecanim Humanoid Rig. Face rig … The pack has
   a revolver peacemaker realistic model with its own rig and moving parts."*
   — and the package ships exactly that: `RevolverRig.fbx` (12 bones:
   `coltHandle`, `cylin`, `ammo`, `bullet1..6`, `hammer`, `trigger`, `reload`).

The loose `bandit (1).fbx` (6 534 672 B) and the packaged `BanditMain.fbx`
(6 583 840 B) are **byte-different but geometry-identical**: measured
`39 mesh objects / 82 866 verts / 140 355 tris / 118 bones` on both, bbox
differs by ≤1e-6 m. They are two FBX export versions of the same asset, 49 168
bytes apart. Both are the AlexN Bandit.

`bandit (1).mb` is the **Maya binary authoring source** for the same character,
not a third asset: header `MayaFOR8`, `HEADVERS` = **Maya 2020**, timestamp
`Fri, Sep 23, 2022`, and its node table contains the identical mesh and bone
names (`BanBody`, `BanHaircards`, `pelvis`, `spine_01…03`, `clavicle_l`,
`upperarm_l`, `weapon_r`, `coltHandle`, `teeth_lower`, `tongue1`, eye and eyelid
bones) plus `*ShapeOrig` blend-shape nodes and `hairSystems`.

##### Licence evaluation (per EULA clause)

| # | Question | Verdict | EULA basis |
|---|---|---|---|
| 1 | Commercial use | **PERMITTED** | §2.2.1(a),(b),(d) — incorporate into a Licensed Product, distribute it, monetise |
| 2 | Modification | **PERMITTED** | §2.2.1(e) + §6 "END-USER may modify Assets" |
| 3 | Redistribution inside a commercial game | **PERMITTED** as embedded component | §2.2.1(a),(b) |
| 4 | Redistribution of the raw asset | **PROHIBITED** | §2.2.1.1(d), §3.5, §5.1 — no copies except one installed copy + archive backup |
| 5 | Attribution | **NOT REQUIRED** | No credit clause in the Standard EULA |
| 6 | Derivatives | **PERMITTED** and shippable inside the Product | §2.2.1(e) |
| 7 | AI-related restrictions | **PROHIBITED** to train AI/ML; permits gathering/scraping/dissemination; **no** AI-training prohibition on *using* the asset in a game | §3.8(v), §2.2.1.1(g) |
| 8 | Platform restrictions | **NONE** stated. Store page lists no platform limit. **Android not blocked** (relevant: Android is a WWG target) | Terms §8.4 lists safety-critical exclusions only |
| 9 | License applies to the actual downloaded file | **YES** — `.unitypackage` is Unity's native Asset Store download container; no repackaging, no mirror, no `.unitypackage` re-zip; 96/96 asset paths resolve to `Assets/Bandit/**` | file evidence |
| 10 | Author and source unambiguous | **YES** — publisher name, publisher id 20710, package id 185326, all three present on the store page and matching the file contents | store page + package contents |

**LICENSE STATUS = PASS**

Residual item: `Extension Asset` designation means §2.3.2 applies —
**per-seat licence, maximum 2 computers**. If WWG is ever developed by a
contractor without their own seat, §2.4 requires that contractor to hold a
licence. Single seat is adequate for the current single-owner project.

##### Geometry audit (measured, `agg_stats.json`)

| Metric | Character (31 obj) | Whole FBX (39 obj) |
|---|---:|---:|
| Vertices | 77 856 | 82 866 |
| Faces | 76 781 | 82 011 |
| Triangles | 130 579 | 140 355 |
| Quads | 53 798 | 58 340 |
| Triangles-as-polygons | 22 983 | 23 669 |
| **n-gons (>4)** | **0** | **2** (`Reload` 1, `RevolverDecor` 1) |
| Non-manifold edges | 22 551 | 22 747 |
| Duplicate verts | 1 834 | 1 834 |
| Disconnected shells | 1 299 | 1 337 |
| Degenerate faces | **0** | **0** |
| Loose verts | **0** | **0** |
| Hidden-from-render objects | **0** | **0** |

Non-manifold / shell counts are dominated by **hair cards**, not the body:
`BanHaircards` alone = 9 972 non-manifold edges, 583 shells, 913 duplicate
verts; `BanHaircardsHat` = 9 916 / 581 / 913; `BanEyelashes` = 1 104 non-manifold
edges / 92 shells. These are **intentionally flat card geometry** (alpha-cut
hair/lash strips). Excluding the three card meshes: **body + clothing
non-manifold = 22 551 − 20 992 = 1 559 edges**, of which `BanBody` alone is 50
and `BanBracers` 220.

Transforms: armature `root` — location `(0,0,0)`, rotation
`(1.570796, 0, 0)` rad = 90° X, **scale `(0.01, 0.01, 0.01)`** (FBX authored in
cm, 0.01 m-per-unit). Character height **1.914390 m** (world Z 0 → 1.912321).
BanditMain.fbx.meta records `useFileUnits: 1`, `useFileScale: 1`,
`globalScale: 1`, `previousCalculatedGlobalScale: 0.01` — so Unity applies the
0.01 file-scale correction and the character imports at **1.91 m**, not at
0.019 m. This is the exact mechanism behind the historic `RT-01` / `UNI-D06`
0.01× bug, and here it is **already handled in the importer**.

##### Rig / weights audit (`rig_audit.json`)

| Item | Value |
|---|---|
| Armature object / data name | `root` |
| Bone count | **118** |
| Deform bones | **118** (all bones `use_deform: true`; **zero** non-deform bones) |
| Bone-naming convention | **UE4/UE5 style** — `pelvis`, `spine_01/02/03`, `neck_01`, `head`, `clavicle_l/r`, `upperarm_l/r`, `lowerarm_l/r`, `hand_l/r`, `thigh_r/l`, `calf_r/l`, `foot_r/l`, `ball_r/l` |
| Twist bones | `upperarm_twist_01_*`, `lowerarm_twist_01_*`, `thigh_twist_01_*`, `calf_twist_01_*` (8) |
| Finger bones | **30** (5 digits × 3 joints × 2 hands) |
| Face bones | **32** under `faceAttach`: brows ×5, cheeks ×5, lips ×5, nostrils ×2, `C_nose_bridge`, `R_eye`/`L_eye` + 4 eyelid bones, `C_jaw` + `teeth_lower`/`teeth_upper`/`tongue`/`tongue1` |
| Hair bones | `hat`, `hair_curls_A/B_l/r_01` ×4, `ponytail_01…04` |
| **Weapon bones** | **`weapon_l`**, **`weapon_r`** (both children of the hand bones) + `weapon_r → coltHandle → {cylin → ammo → bullet1..6, hammer, trigger, reload}` |
| **Holster bone** | **`holster`** (child of `pelvis`) |
| IK / helper bones | **NONE** — no IK, no pole, no twist-follow, no aim bones |
| Animator object constraints | **NONE** |
| Orphan vertex groups | **0** on all 39 meshes |
| Vertices with no weights | **0** on all 39 meshes |
| Max influences / vertex | **9** (`BanBody`, 1 079 verts > 4); 5 on `BanHaircardsHat` (115 verts) and `BanShirt1` (2 verts) |
| Weight normalisation | **PASS** — every mesh min weight-sum ∈ [0.9993, 1.0], max 1.0 |

##### Unity Humanoid feasibility

`BanditMain.fbx.meta` — shipped **inside the package**:

| Field | Value | Meaning |
|---|---|---|
| `animationType` | **3** | `ModelImporterAnimationType.Human` |
| `humanDescription.human` | **`[]` — EMPTY** | **no human-bone mapping is stored** |
| `humanDescription.skeleton` | `[]` — EMPTY | |
| `avatarSetup` | **key absent** | falls back to importer default |
| `lastHumanDescriptionAvatarSource` | `{instanceID: 0}` | no source Avatar |
| `animationType` on `RevolverRig.fbx.meta` | **2** | `Generic` |

The `.meta` declares Human **mode** with an **empty** bone map. The store
listing claims *"Mecanim Humanoid Rig"* and *"compatible with UE4 Epic
Skeleton"*, which is consistent — UE4 naming maps to Unity Humanoid by
heuristic. `BanditMain.prefab` binds `m_Avatar: {fileID: 9000000, guid:
3ce16b0c2ae4ac84fa450432c1f64f35, type: 3}` — i.e. the Avatar sub-asset **of the
FBX itself**, and `m_Controller: {fileID: 0}` (**no Animator Controller**).

Structural feasibility is verifiable from the bone list: `pelvis`,
`spine_01/02/03`, `neck_01`, `head`, `clavicle_l/r`, `upperarm_l/r`,
`lowerarm_l/r`, `hand_l/r`, `thigh_l/r`, `calf_l/r`, `foot_l/r` all exist as a
connected chain from a single root. Unity's required 15 + optional 21 slots are
satisfiable, including `UpperChest` (`spine_03`), `Left/RightToes` (via
`ball_l/r`), `Left/RightEye` (`L_eye`/`R_eye`) and `Jaw` (`C_jaw`).

**Classification: `HUMANOID LIKELY`** — *not* `HUMANOID READY`.

Reasoning, stated as fact: the bone hierarchy is complete and conventional; but
the shipped importer metadata contains **no stored human-bone mapping**, and
this audit is read-only and explicitly **did not import into Unity**. Unity's
automatic UE-convention mapping has therefore **not been executed and observed
on this project**. The gate that would prove `HUMANOID READY` — import the FBX,
open the Avatar Configuration, confirm no mandatory bone is unmapped and no
muscle error appears — has **not** been run. `HUMANOID LIKELY` is the highest
class the evidence supports.

##### Animation audit

| Item | Value |
|---|---|
| Embedded animation clips in `BanditMain.fbx` | **0** — `AnimationStack` = 0, `AnimationLayer` = 0, `AnimationCurveNode` = 0, `AnimationCurve` = 0, `KeyTime` = 0 (raw FBX byte scan **and** Blender import both 0) |
| Embedded clips in `RevolverRig.fbx` | **0** |
| External `.anim` / `.controller` files in package | **0** (no such pathname in 96 assets) |
| Animator Controller referenced by `BanditMain.prefab` | **`{fileID: 0}` — none assigned** |
| Animator Controllers in the package | **0** |
| Store statement on animation | *"There are no animations, only three unreal maniq animations for testing"* (third-party listing text; **the three test animations are NOT in the shipped package**) |
| Scene FPS | 24 (Blender import default for FBX without `GlobalSettings` time data) |
| T-pose / A-pose | **A-pose** (arms ~45° down; confirmed in render `bandit_FRONT.png`) |
| Root motion / constraints / NLA | none present |

Animation **potential** for `Idle → locomotion → aim → fire → reload → hit →
death`:

| Requirement | Foundation supports it? |
|---|---|
| Idle / locomotion | **Yes** — full standard humanoid bone set, A-pose, normalised weights |
| Aim / fire | **Yes, better than Doomy** — dedicated `weapon_l` / `weapon_r` bones, plus a fully rigged revolver (`coltHandle → cylin/ammo/hammer/trigger/reload`, 6 `bullet` bones) that can be animated for a **visible cylinder-and-hammer cycle** |
| Reload | **Yes** — `reload` bone plus `ammo` + `bullet1..6` chain lets bullets be driven |
| Upper-body separation | **Yes** — Humanoid Animator layers split at UpperChest |
| Hit / death | **Yes** — humanoid retarget target |
| Facial animation | **Available, not configured** — 32 face bones exist; no Animator Controller and no clips, so blendshapes/lip-sync wiring is unbuilt. The `.mb` contains `*ShapeOrig` blendshape nodes; `importBlendShapes: 1` is set, so blendshapes import if present. |

**No compatible-skeleton reuse across the two audited characters.** Bandit uses
UE4 naming (118 bones); Cowboy Elderly uses a different convention (87 bones);
Doomy uses the project-internal `B-*` scheme (51 bones). Animation reuse across
them requires **per-target retargeting**, not clip sharing. This is a
materiality fact for the `Idle → …` pipeline and is scored below.

##### Visual / western fit QA (read-only Workbench renders)

| Observation | Evidence |
|---|---|
| Height 1.914 m, adult female proportions | `bandit_FRONT.png`, bbox Z 0 → 1.912321 |
| **Full western silhouette present**: wide-brim cowboy hat, bandana face mask (`BanMaskdown`/`BanMaskup`), fitted buttoned shirt/vest (`BanJacket` 1 659 v), gun belt with buckle (`BanGunbelt` 3 476 v), thigh holster + holstered revolver (`BanGunbeltHolster`), leather chaps (`BanChaps` 5 539 v), flared trousers (`BanPants`), boots (`BanBoots`), bracers, ponytail | `bandit_FRONT.png`, `bandit_THREEQ.png`, `def_bandit_posed.png` |
| Hat is **native to the asset**, not a fit-in | `BanHat` 1 464 v + `BanHat2` 450 v, both bound to the `hat` bone |
| Weapon carried in hand, holstered at thigh | `def_bandit_posed.png`, `bandit_THREEQ.png` |
| Deformation under load | `defect_bandit_weights.png` — 95° knee, 140° elbow, arm raise: **no volume collapse, no explosion, no detachment at belt/holster/hat** |
| Silhouette readable at gameplay distance | `read_bandit_GAMEPLAY_256.png` — hat brim, mask, gunbelt read as distinct shapes at 256 px |
| Texture appearance | **NOT EVALUATED** — 4K `.tga` sets are not bound in FBX materials (FBX diffuse = white 1,1,1 for all 15 `Ban*` materials except `BanHatMtl` 0.231/0/1 blue). Appearance depends entirely on the 4 `.tga` sets per material |

**WESTERN FIT SCORE = 88 / 100**

Deductions from 100: −6 thin facial/hair detail at mid distance (visible in
`bandit_HEAD_CLOSE.png`: 583+581 hair-card shells, no modelled face planes
behind the cards); −3 no boots-vs-heel distinction (single low boot form);
−3 hat, mask and holster are **rigid single-weight bindings** (1 vertex group
each on `BanHat`, `BanHat2`) so they will not follow head/neck micro-motion.

##### Already usable / human polish / major conversion

| Bucket | Items |
|---|---|
| **Already usable** | Body, hat, bandana, jacket/vest, gun belt, holster, chaps, trousers, boots, bracers, hair cards, full 118-bone rig, revolver rig |
| **Human polish** | Rename UE4 bone names to project convention or fix the Avatar mapping table; normalise `BanBody` 1 079 verts from 9 → 4 influences (mobile-perf); add texture-set QA per material (4K `.tga` × 15 materials = 58 files, 1.64 GB uncompressed); trim 1 834 duplicate verts; build Animator Controller + `Idle/locomotion/aim/fire/reload/hit/death` set; reduce 1 559 body non-manifold edges |
| **Major conversion** | Nothing identified |

##### Final scorecard

| Category | Score |
|---|---:|
| License | **100** |
| Geometry | **72** |
| Rig | **92** |
| Weights / Deformation | **86** |
| Animation | **70** |
| Unity Humanoid | **58** |
| Western Fit | **88** |
| Production Effort | **70** |
| Scalability | **72** |
| **Overall** | **79** |

### 6.1.4 COWBOY ELDERLY — ART_I_SHOC

| Field | Value |
|---|---|
| Asset name | **Cowboy Elderly** |
| Creator / author | **ART_I_SHOC** (Unity Asset Store publisher id 70620) |
| Original source | Unity Asset Store |
| Original source URL | `https://assetstore.unity.com/packages/3d/characters/cowboy-elderly-226819` |
| Publisher URL | `https://assetstore.unity.com/publishers/70620` |
| License | **Standard Unity Asset Store EULA** |
| License URL | `https://unity.com/legal/as-terms` (last updated 2024-12-04) |
| Declared licence tier | **Single Entity** · `License type: Extension Asset` |
| Store file size (declared) | 539.3 MB — local `.unitypackage` 533 500 312 B = 508.8 MiB |
| Store version / date | 1.0 · Jul 25, 2022 · original Unity 2021.3.4f1 |
| Price at audit | **€18.40** — **PAID asset** |
| Pipeline declaration | Built-in **Not compatible** · URP **Not compatible** · **HDRP Compatible** |
| Local artefact set | `cowboyelderly.unitypackage` **only** |
| Attribution requirement | **NONE** under the Standard EULA |
| Production status | **`NOT ADMITTED`** |

##### Identity — content-based

1. Package namespace `Assets/CowboyCharacter/**`, 85 assets, character FBX
   `cowboy_character.fbx`, prefabs `cowboy_character_v1/v2/v3.prefab`,
   materials `mat_cowboy_{torso,legs,holster,head,hands,hair,colt,eye.*}.mat`.
2. **Third-party mirror of the store listing text** quotes *"Persons: 13511 ·
   Vert: 14103 · Tris: 26461 · 40 textures all 4k · Additional bones: jaw, gun,
   eyelids, eyes, eyebrows, smile, holster, pigtail"*. Measured on the packaged
   FBX: **13 511 faces / 14 103 verts / 26 461 tris** (`fbx_audit.json`,
   `agg_stats.json` → `cowboy_all`) — **exact match on all three figures**. The
   additional bones named there all exist: `c_jawbone.x` (jaw), `cc_gun` (gun),
   `eyelid_top/bot`, `c_eye`, `c_eyebrow_*`, `c_lips_smile`, `cc_holster`,
   `c_tail_00…03` (pigtail).
3. Secondary confirmation that the file was obtained from the Asset Store:
   the package contains a `TextMesh Pro/` tree, `LiberationSans` fonts and a
   `SkyandFogSettingsProfile.asset` under `Assets/Settings/` — i.e. a snapshot
   of the seller's own project.

> Note on the third-party mirror: the "Persons: 13511" wording is a mislabelling
> by the mirror — **13 511 is the face count**, not a person count. The figure
> still matches the measured mesh exactly, so it is used as identity evidence.

##### Licence evaluation

Identical clause-by-clause evaluation to §6.1.3, with these differences:

| # | Question | Verdict | Basis |
|---|---|---|---|
| 1–4, 6 | commercial / modification / in-game redistribution / raw redistribution / derivatives | Same as Bandit: PERMITTED / PERMITTED / PERMITTED / **PROHIBITED** / PERMITTED | §2.2.1, §2.2.1.1(d), §5.1 |
| 5 | Attribution | **NOT REQUIRED** | Standard EULA |
| 7 | AI restrictions | Same as Bandit | §3.8(v), §2.2.1.1(g) |
| 8 | Platform restrictions | **NONE stated.** Note the store declares Built-in and URP **incompatible**, HDRP compatible | store page |
| 9 | Licence applies to the actual file | **YES** — native `.unitypackage`, no repackaging | file evidence |
| 10 | Author / source unambiguous | **YES** — publisher name, id 70620, package id 226819 | store page |

**LICENSE STATUS = PASS (tier-conditional)**

Two conditions attach, and they are the reason this is not an unconditional
PASS:

1. **Single Entity tier.** The licence is scoped to one legal entity. WWG is a
   single-owner project, so this is satisfied — **provided the entity holding
   the Asset Store account is the same entity shipping the game.** **UNKNOWN**:
   the audit cannot see the purchasing account.
2. **`Extension Asset`** → §2.3.2 per-seat, max 2 computers; §2.4 requires any
   contractor to hold their own seat.

Additionally **paid** (€18.40). Unlike the free Bandit, a paid asset requires a
**proof-of-purchase record** before it can enter production. **No invoice,
receipt, order ID or account identifier exists on disk** — that evidence is
outside the audited folder and outside the repository.

**LICENSE STATUS = CONDITIONAL**
(→ PASS on licence terms; **BLOCKED** until proof of purchase is recorded for
this specific entity.)

##### Geometry audit

| Metric | Whole FBX (4 obj) | Body only (`body`) |
|---|---:|---:|
| Vertices | 14 103 | 7 410 |
| Faces | 13 511 | 7 229 |
| Triangles | 26 461 | 14 134 |
| Quads | 12 950 | 6 905 |
| Triangles-as-polygons | 561 | 324 |
| **n-gons** | **0** | **0** |
| Non-manifold edges | 1 597 | 630 |
| Duplicate verts | 29 | 24 |
| Disconnected shells | 95 | 34 |
| Degenerate faces | **0** | **0** |
| Loose verts | **0** | **0** |
| Hidden-from-render objects | **0** | **0** |
| Mesh objects | **4** (`body`, `colt`, `hair`, `holster`) | 1 |
| Materials on every object | all 9 slots on all 4 objects (**excessive** — see below) | |

Geometry quality is **materially better** than Bandit: 0 n-gons, 0 degenerate
faces, 0 loose verts, 29 duplicate verts total, 12 950 of 13 511 polys are quads.

Transforms: armature `root` — location `(0,0,0)`, rotation `(0,0,0)`,
**scale `(0.01, 0.01, 0.01)`**. Character height **1.840331 m**
(world Z −0.002659 → 1.837672). `cowboy_character.fbx.meta` records
`useFileUnits: 1`, `useFileScale: 1`, `globalScale: 1` → same 0.01 cm authoring
convention as Bandit, also handled by the importer.

##### Rig / weights audit

| Item | Value |
|---|---|
| Armature object / data name | `root` |
| Bone count | **87** |
| Deform bones | **87** (all deform; zero non-deform) |
| Bone-naming convention | **Custom, `.x`-suffixed** — `root.x`, `spine_01.x`, `neck.x`, `head.x`, `arm_stretch.l`, `forearm_stretch.l`, `hand.l`, `thigh_stretch.l`, `foot.l`, `toes_01.l` |
| Twist / stretch bones | `arm_twist.*`, `forearm_twist.*`, `thigh_twist.*`, `leg_twist.*` (8) |
| Finger bones | **30** (5 digits × 3 joints × 2 hands), named `c_index1.l` … `c_pinky3.r` |
| Face bones | 24: `c_eye.l/r`, `c_eye_offset.l/r`, `c_eyebrow_full/01/01_end/02/03` ×2, `c_jawbone.x`, `c_lips_smile.l/r`, `eyelid_top/bot.l/r` |
| Hair bones | `c_tail_00.x` … `c_tail_03.x` (4) |
| **Weapon bones** | **`cc_gun`** (child of `thigh_stretch.r`) |
| **Holster bone** | **`cc_holster`** (child of `thigh_stretch.r`) |
| IK / helper bones | **NONE** |
| Animator object constraints | **NONE** |
| Orphan vertex groups | **0** |
| Vertices with no weights | **0** |
| **Max influences / vertex** | **40** (`body`, 4 274 of 7 410 verts > 4) |
| **Weight normalisation** | **FAIL — 4 881 of 7 410 `body` verts unnormalised**; observed weight-sum range **0.0686 … 1.8337** |

This is the **single most serious technical defect** found in this audit.
Blender reports `holster`: 5 155 of 5 224 verts unnormalised, sum range
**0.1206 … 1.2837**; `hair`: 160 of 468 unnormalised, sum **0.0671 … 2.0873**;
`colt`: all 1 001 verts at sum **0.5** exactly (uniformly half-weighted).

Rendered consequence, `defect_cowboy_weights.png` — 140° elbow, 95° knee: knees
and elbows **hold** (no explosion, no collapse, no mesh tearing), but the
deformation is visibly **soft and inflated** at the shoulder and thigh-joint
creases, and the holster visibly **separates** from the belt/leg. The defect is
real and reproducible; it is a quality defect, not a catastrophic failure.

Unity import behaviour is relevant and documented: `cowboy_character.fbx.meta`
sets `skinWeightsMode: 0` (**Standard**) and `maxBonesPerVertex: 4`. Per Unity's
`ModelImporter` behaviour, vertices with more than 4 influences have the
**lowest-weighted bones discarded**, and `QualitySettings.skinWeights` /
`SkinnedMeshRenderer.quality` further limit influences at skinning time. So
**4 274 of 7 410 body vertices lose 90 % of their influence data at import**
unless the importer is switched to `Custom` + `maxBonesPerVertex: 40`.

##### Unity Humanoid feasibility — **VERIFIED FROM SHIPPED METADATA**

`cowboy_character.fbx.meta`, shipped inside the package, contains a **complete,
explicitly-authored Humanoid bone map**:

| Field | Value | Meaning |
|---|---|---|
| `animationType` | **3** | `Human` |
| `avatarSetup` | **1** | `CreateFromThisModel` |
| `autoGenerateAvatarMappingIfUnspecified` | **1** | enabled |
| `humanDescription.human` | **55 entries, fully populated** | see below |

The 55 stored human-bone assignments:

`root.x`→Hips · `spine_01.x`→Spine · `spine_02.x`→Chest · `spine_03.x`→UpperChest ·
`neck.x`→Neck · `head.x`→Head · `shoulder.l/r`→Left/RightShoulder ·
`arm_stretch.l/r`→Left/RightUpperArm · `forearm_stretch.l/r`→Left/RightLowerArm ·
`hand.l/r`→Left/RightHand · `thigh_stretch.l/r`→Left/RightUpperLeg ·
`leg_stretch.l/r`→Left/RightLowerLeg · `foot.l/r`→Left/RightFoot ·
`toes_01.l/r`→Left/RightToes · `c_eye.l/r`→Left/RightEye ·
`c_jawbone.x`→Jaw · `c_thumb1..3`→Left/Right Thumb Proximal/Intermediate/Distal ·
`c_index1..3`, `c_middle1..3`, `c_ring1..3`, `c_pinky1..3` → corresponding
Left/Right {Index, Middle, Ring, Little} Proximal/Intermediate/Distal

Every mandatory Unity Humanoid slot is mapped, plus UpperChest, both Eyes, Jaw
and all 30 finger bones. `humanDescription.skeleton` stores **92 transform
records** with local position/rotation/scale for each node.

**Classification: `HUMANOID LIKELY`**

Stated plainly: the *authored mapping is complete and present in the shipped
file*, which is stronger evidence than Bandit has. But `HUMANOID READY` is a
claim about how Unity **evaluates** that mapping on import — whether Unity 6
flags a muscle error, whether the A-pose/`hasExtraRoot: 1` combination passes
validation. This audit did not import into Unity, so that evaluation has **not
been executed**. The classification cannot go higher on the evidence gathered.

Additional decisive risk: `skinWeightsMode: 0` / `maxBonesPerVertex: 4`
truncates 4 274 body vertices (§ above). A humanoid Avatar can be perfectly
mapped and still deform badly once 36 of 40 influences are discarded at import.

##### Animation audit

| Item | Value |
|---|---|
| Embedded animation clips | **0** — `AnimationStack`/`AnimationLayer`/`AnimationCurveNode`/`AnimationCurve`/`KeyTime` all 0 by raw byte scan |
| `.anim` / `.controller` in package | **0** |
| `cowboy_character_v1.prefab` Animator | `m_Avatar` bound to the FBX's own Avatar sub-asset; **`m_Controller: {fileID: 0}` — none** |
| Animator Controllers in package | **0** |
| `referencedClips` in FBX meta | **`[]`** |
| Scene FPS | 24 (Blender import default) |
| T-pose / A-pose | **A-pose** (near-horizontal arms, `cowboy_FRONT.png`) |
| Root motion / constraints / NLA | none |

For `Idle → locomotion → aim → fire → reload → hit → death`: the humanoid bone
set supports all of it, and `cc_gun` / `cc_holster` / `c_tail_*` give
weapon- and hair-specific control. The revolver `colt` mesh is **bound 100 % to
a single vertex group** (`max_influences: 1`), so it is a rigid attachment to
`cc_gun` — it cannot articulate a cylinder-and-hammer cycle the way the Bandit's
revolver rig can. Reload animation on this character is therefore
**single-object**, not mechanical.

##### Visual / western fit QA

| Observation | Evidence |
|---|---|
| Height 1.840 m, elderly male — **bald, grey ponytail** (`c_tail_00…03`), long grey beard | `cowboy_FRONT.png`, `cowboy_SIDE_L.png`, `cowboy_HEAD_CLOSE.png` |
| Western items present: **buttoned waistcoat/vest** (`mat_cowboy_torso`, 2 745 faces), **gun belt with buckle**, **thigh holster with revolver**, boots | `cowboy_FRONT.png` |
| **NO cowboy hat** — `cowboy_character.fbx` contains no hat mesh. Head is bare except hair and beard. The E1 hat would have to be fitted separately | `cowboy_object_names` = `['body','colt','hair','holster','root']`; `cowboy_FRONT.png` |
| **No separate shirt/pants/boots meshes** — `body` is one mesh carrying head + torso + arms + hands + legs + eyes; boots are integrated into the leg, not a separate object | `cowboy_all` mesh count 4 |
| Deformation under load | `defect_cowboy_weights.png` — holds, but soft/inflated at joints, holster separates |
| Gameplay-distance readability | `read_cowboy_GAMEPLAY_256.png` — **highest of the three**: the tall hatless grey-haired silhouette, bright torso colour block and red holster read distinctly at 256 px |
| Face detail | `cowboy_HEAD_CLOSE.png`: heavy brow cards, prominent nasolabial folds, **cornea/sclera two-material eye** (`mat_cowboy_eye.cornea` alpha 0, `mat_cowboy_eye.sclera`), cheek/eyelid/brow bones present |
| Texture appearance | **NOT EVALUATED** — FBX diffuse colours are placeholders (`hands` pure green 0/1/0, `legs` 1/1/0 yellow, `torso` 0/1/1 cyan, `holster` 1/0/0 red, `colt` dark blue). Real look depends on the 4K sets. **Placeholder colours do not indicate the shipped look.** |

**WESTERN FIT SCORE = 74 / 100**

Deductions from 100: −10 no hat in the asset (the single most recognisable
western signifier must be fitted, and it must survive Unity Humanoid import); −6
**no modular clothing separation** (head+torso+arms+hands+legs are one mesh —
this is the finding that matters most for the garment pipeline, see §6.1.6); −5
unnormalised weights cause soft joint deformation that would need re-rig work to
look authored rather than acceptable; −5 boots are integrated into the leg mesh,
so boots cannot be replaced or recoloured independently.

##### Already usable / human polish / major conversion

| Bucket | Items |
|---|---|
| **Already usable** | Body mesh + skin, head + face, 87-bone rig, all 15 materials, all 27 character texture files, revolver mesh, holster mesh, hair mesh, band + holster geometry |
| **Human polish** | Re-normalise 4 881 `body` + 5 155 `holster` + 160 `hair` vertices and reduce 40 → 4 influences (or set importer to `Custom`/`40`); verify Humanoid Avatar in Unity (`avatarSetup: 1` is correct but unverified on this project); reduce the 9-material-slot-on-every-object waste to per-object slots; verify HDRP Lit binding; author Animator Controller + clip set; trim 29 duplicate verts and 1 597 non-manifold edges |
| **Major conversion** | **Separate the monolithic `body` mesh** into head / torso / arms / hands / legs / boots so western garments can be fitted and swapped — this is the item that decides whether the asset can serve as a WWG character foundation at all; author a hat; author boots as a separate object; build a rigid-vs-articulated decision for `colt` |

##### Final scorecard

| Category | Score |
|---|---:|
| License | **70** |
| Geometry | **80** |
| Rig | **78** |
| Weights / Deformation | **35** |
| Animation | **58** |
| Unity Humanoid | **65** |
| Western Fit | **74** |
| Production Effort | **45** |
| Scalability | **42** |
| **Overall** | **60** |

### 6.1.5 Comparison with the Doomy enemy foundation

**Doomy measured, not assumed.** `Working/blender_src/doomy/Doomy_Character_Production_Working.blend`
opened read-only (2 571 546 B), `Doomy_`-prefixed objects only, so the file's
179 objects (6 player/enemy variants + library) do not inflate the figure:

| Metric | Doomy (measured) | Bandit | Cowboy Elderly |
|---|---:|---:|---:|
| Verts | **12 167** | 82 866 | 14 103 |
| Faces | 21 266 | 82 011 | 13 511 |
| Triangles | 23 101 | 140 355 | 26 461 |
| Mesh objects | 62 | 39 | 4 |
| Non-manifold edges | **993** | 22 747 | 1 597 |
| Max influences / vertex | **6** | 9 | **40** |
| Armature bones | **51** (`B-*` names) | 118 (UE4) | 87 (`.x`) |
| Finger bones | 30 | 30 | 30 |
| Armature objects in file | 8 (one 51-bone rig shared) | 1 | 1 |
| Actions in file | **0** | 0 | 0 |
| Weapon bones | **none** | `weapon_l/r` + 12-bone revolver rig | `cc_gun` |
| Hat | **yes** (`Doomy_Hat_*`) | yes | **no** |
| Vest | yes | yes (jacket) | yes (torso) |
| Separate boots | yes (`Doomy_Boots`, 344 v) | yes (`BanBoots`) | **no** (integrated) |
| Separate holster | yes | yes | yes |
| Materials | 44 in file | 19 | 15 |
| Triangles per enemy on screen | **23 101** | 140 355 (6.1×) | 26 461 (1.1×) |

| Criterion | Bandit | Cowboy Elderly | Doomy |
|---|---|---|---|
| License safety | **PASS** (free, PASS) | **CONDITIONAL** (paid, proof of purchase missing) | **WWG-owned** — vendor lineage only, no full licence record (`ART-0004` §6, `OPEN_DECISIONS.md`) |
| Rig | 118 bones, UE4 naming, `weapon_l/r`, holster bone, 0 orphan groups | 87 bones, custom naming, `cc_gun`/`cc_holster`, 0 orphan groups | 51 bones, `B-*`, **no weapon bones**, shared rig across 8 armatures |
| Deformation | clean (sum 0.9993–1.0), survives 140° elbow / 95° knee | **4 881+5 155+160 verts unnormalised**, sums 0.068–1.834, soft joints, holster separates | max 6 influences, body parts are 2 346-vert blocks — lowest resolution, simplest deformation |
| Animation potential | best: revolver rig with `bullet1..6`, `hammer`, `trigger`, `reload`; 32 face bones | good: humanoid complete, `cc_gun`/`cc_holster`, 24 face bones; revolver rigid | weakest: **no weapon bones**, no facial rig, no clips anywhere |
| Western identity | **88** — hat, mask, vest, chaps, holster, boots all native | 74 — vest/belt/holster native, **hat absent**, boots integrated | already western, in-project, art-directed |
| Unity readiness | `animationType: 3` but **empty** `human` map; no Animator Controller | `animationType: 3` + **55-entry authored map**, `avatarSetup: 1`; no Animator Controller | **not in Unity at all** (`UNI-D02`) |
| Manual work | moderate: 4K textures × 15 mats, influence normalisation, Animator build, avatar mapping | **high**: weight re-normalisation, monolithic-body separation, hat, boots, Animator build | already spent; remaining work is integration, not construction |
| Scalability | 31 separable clothing/body objects → reusable parts | 4 objects, 1 monolithic body → **poor** | shared 51-bone rig → best for many enemy instances |
| Production risk | medium | **high** | low (already owned, already validated) |
| Overall | **79** | **60** | — |

### 6.1.6 Answer to the stated project decision

> **Question:** are these two characters actually more useful than Doomy for
> production?

**BANDIT: YES, for the player / named-character slot. NO, for enemies.**

- For the **player / named character**, Bandit is a genuine upgrade over any
  current option. Evidence: native western silhouette with hat, bandana,
  buttoned shirt, gun belt, chaps, holster, boots; a complete 118-bone rig with
  32 face bones and **dedicated `weapon_l`/`weapon_r` bones plus a 12-bone
  articulated revolver**; `LICENSE STATUS = PASS`; `animationType: 3` Human.
  The project's approved foundation (`WWG_Rig`, 53 bones) has **no weapon
  bones** and no face rig, so Bandit is materially more capable for a
  revolver-driven hero.
- For **enemies**, Bandit is **worse than Doomy on cost and better only on
  quality**: 140 355 tris vs Doomy's 23 101 (**6.1×**) for an enemy the player
  sees repeatedly and at distance; 9 SkinnedMeshRenderers on a shared rig vs
  Doomy's simpler parts; and it costs a Unity license seat either way. Enemies
  do not need 32 face bones, an articulated revolver or 4K-per-material
  textures.

**COWBOY ELDERLY: NO for both slots, in its current form.**

- Licence is **CONDITIONAL**, not PASS — a paid asset with **no proof of
  purchase on disk** (`§6.1.4`).
- Its decisive defect is structural: **one monolithic `body` mesh** carrying
  head, torso, arms, hands and legs. WWG's whole approach to characters is
  garment fitting onto a body foundation (`wwg-garment-construction`, E1/E2
  stages, `wwg-garment-construction` clearance rules). Separating that mesh is
  not polish — it is major conversion work, and it re-opens the topology rather
  than reusing it.
- 4 881 unnormalised body vertices with sums down to 0.0686 and up to 1.8337
  require re-rig work before the model is presentable in a hero shot.
- Its **one clear win** is that its Humanoid mapping is **already authored and
  stored** (55 entries), whereas Bandit's shipped metadata has an empty map.

**RECOMMENDATION — recorded as an audit conclusion, not an action**

| Character | Verdict |
|---|---|
| **BANDIT — AlexN** | **`FALLBACK`** as the character foundation decision input; **top-ranked candidate for `PRIMARY PLAYER CANDIDATE` / `SECONDARY CHARACTER`** if a Western hero slot is opened |
| **COWBOY ELDERLY — ART_I_SHOC** | **`REJECT`** for both player and enemy roles in current form; retain only as a **reference/dossier item** |

`PRODUCTION CANDIDATE` is **not** awarded to either: neither has entered Unity,
neither Humanoid mapping has been verified on this project, and neither has an
Animator Controller. That claim would require evidence this audit deliberately
did not generate.

**The project decision stands unchanged.** Enemies continue on **Doomy**, with
class differentiation via visual features and colour variants. This audit found
nothing that justifies replacing Doomy for enemies. Bandit is a candidate for
the *player* slot, which is a separate decision the project owner has not made.

### 6.1.7 Anti-repeat record

Do not re-derive: tar/gz member layout of `.unitypackage`; `.meta` GUID
resolution across a package; FBX binary node-name byte-scan as an
animation-presence test; Workbench material-colour rendering for silhouette
QA. The scripts that produced every number above are outside the repository and
were run against temporary copies only.

---

## 7. Rules for using this catalog

1. **One registry.** Extend this document; never create a second licence/attribution file.
   Audited-and-rejected external assets are also recorded here — see
   §6.1 EXTERNAL CHARACTER ASSET AUDIT 2026-10-07, which carries `NOT ADMITTED`
   records for `Bandit (AlexN)` and `Cowboy Elderly (ART_I_SHOC)` held in an
   **external** location outside Unity and outside the repository (see §6.1.1).
2. **No paraphrasing.** Cite the official licence URL; do not retype legal text.
3. **Derivatives stay labelled.** A fitted/modified copy is a derivative, not a new original.
4. **Unverified files are quarantined.** Record them with `LICENSE UNVERIFIED` and
   `EXCLUDED FROM PRODUCTION` until provenance is confirmed.
5. **Pre-release audit.** Before any marketplace/release preparation, run a full
   third-party audit: every production asset must have creator, source, licence,
   attribution text, path and status. A third-party asset in production without a record
   here is a **documentation defect**.

---

## 8. FREE ANIMATION SOURCE — SELECTED 2026-10-07 (technical evaluation, NOT admitted)

> Separate from the §6.1 asset audit. This records only a **verified technical
> evaluation** performed in a disposable test environment. No production integration,
> no Animator Controller, no asset import into `Assets/`.

### 8.1 Selected source

| Field | Value |
|---|---|
| Name | **Quaternius — Universal Animation Library (Standard)** |
| URL | `https://quaternius.com/packs/universalanimationlibrary.html` / `https://quaternius.itch.io/universal-animation-library` |
| Author | Quaternius |
| License | **CC0 1.0 Universal (Public Domain Dedication)** |
| Commercial use | **ALLOWED** — CC0 imposes no restriction |
| Redistribution | **PERMITTED**, including raw files inside a shipped game and inside this repository. No attribution required. |
| Download | Anonymous, no account. `UAL1_Standard.fbx` 22.7 MB, `UAL1_Standard_RM.fbx` 22.7 MB (root motion baked), `.glb` 7.3 MB |
| Consistency with existing art | **YES** — stylised lowpoly, same family as the Quaternius kits already catalogued in §5 |

License verified twice: from the itch.io listing
(`Asset license: Creative Commons Zero v1.0 Universal`) and from the `License.txt`
shipped inside the downloaded archive.

### 8.2 Rejected alternative

| Source | License | Reason not selected |
|---|---|---|
| **Mixamo (Adobe)** | Adobe General Terms §2.5 — perpetual royalty-free commercial use **permitted**; raw-file redistribution **restricted**; team members may share within one project | Legally usable, but strictly worse than CC0 for the same permitted use, requires an Adobe account, and its generic mocap style does not match the project's stylised-lowpoly art direction |

`UNKNOWN LICENSE = DO NOT USE` — no source with unclear terms was accepted.

### 8.3 Verified technical result on the Bandit Avatar

| Check | Result |
|---|---|
| Library Avatar | `isValid=True`, `isHuman=True`, 52 human entries |
| AnimationClips | **43**, all with `isHumanMotion=True` |
| WWG 8-clip coverage | **COMPLETE from one source, one style** |
| Retarget `source → Bandit` | **PASS** — cross-rig (52-entry source rig → 55-entry Bandit Avatar) |
| Foot placement over walk | `hips.y` 0.9542–0.9610 m, stable. `LeftFoot.y` 0.0997→0.2904 and `RightFoot.y` 0.2689→0.1205 correctly alternate — **no foot skating** |
| Right hand raises for Aim/Fire/Reload | **YES** — `hand_r.y` 1.1594 → 1.4536 (**+0.294 m**) |
| Revolver stays in the hand | **YES** — `|gunMesh − hand_r| = 0.166 m`, constant in every clip and frame (authored rig offset, not a retarget artefact) |
| Mesh collapse / popping / explosion | None |

Selected 8-clip mapping:

| WWG slot | UAL clip | Length |
|---|---|---|
| Idle | `Idle_Loop` | 2.50 s |
| Walk | `Walk_Loop` | 1.33 s |
| Run | `Sprint_Loop` | 0.67 s |
| Aim | `Pistol_Aim_Neutral` | 0.17 s |
| Fire | `Pistol_Shoot` | 0.63 s |
| Reload | `Pistol_Reload` | 1.67 s |
| Hit | `Hit_Chest` | 0.33 s |
| Death | `Death01` | 2.40 s |

### 8.4 Visual quality verdict on Bandit — `B`

Overall **B — usable with minor cleanup**. Per clip (technical / visual / gameplay):

| Clip | Technical | Visual | Gameplay |
|---|---|---|---|
| Idle | PASS | A | A |
| Walk | PASS | B | B |
| Run | PASS | **C** | **C** |
| Aim | PASS | A | A |
| Fire | PASS | A | A |
| Reload | PASS | A | A |
| Hit | PASS | B | A |
| Death | PASS | B | A |

Gameplay-camera suitability: **A** at combat and medium framing, **B/C** at close framing.

### 8.5 Defect found — Bandit rig, not the animation source

`BanBoots` and `BanChaps` **shear away from the legs** under strong knee/hip flexion
(visible in Walk, severe in Sprint). Cause: both meshes carry heavy weight on the twist
bones, which are **not** part of the generated Humanoid map.

| Mesh | Bone | Share | In Human map |
|---|---|---|---|
| `BanBoots` | `calf_twist_01_l` / `_r` | 18.6 % each | **NO** |
| `BanChaps` | `calf_twist_01_l` / `_r` | **dominant** (1035 / 996 weight units vs 572 / 571 for the mapped `calf_l` / `calf_r`) | **NO** |
| `BanPants` | `thigh_twist_01_l` / `_r` | 313 / 310 weight units | **NO** |

This is an **Avatar-mapping limitation of the Bandit rig**. Any retargeted Humanoid clip
set will show it. Remediation options (each requires separate approval): map the twist
bones into the Avatar human description, or re-weight the three meshes in Blender to
drop the twist bones, or author the missing locomotion in the rig's native pose.

Secondary, close-range only: hat/hair-card clipping at Sprint and Hit; the weapon sits a
constant 0.166 m from the hand bone (authored, acceptable at gameplay distance).

### 8.6 Status

- **Source selected:** YES (technical evaluation only)
- **Bandit visual animation quality:** `B`
- **Production integration:** **NO**
- **Human approval for foundation change:** **NOT REQUESTED / NOT GRANTED**
- **Full report:** `Working/reports/bandit_free_animation_source_and_quality_2026-10-07.md`
- **Visual + log evidence:** `Working/reports/bandit_anim_quality_evidence_2026-10-07/`

---

## 9. BANDIT DEFORMATION REPAIR — VALIDATED 2026-10-07 (test environment only, NOT integrated)

> This records a **deformation-repair validation**, not a production integration.
> Full report: `Working/reports/bandit_deformation_repair_qa_2026-10-07.md`
> Evidence: `Working/reports/bandit_deformation_repair_evidence_2026-10-07/`

### 9.1 Defect (VERIFIED FACT)

`BanBoots`, `BanChaps` and `BanPants` carried **37.15 %**, **44.38 %** and **34.83 %** of
their skin weight on twist bones that are **not** in the generated Humanoid map:
`calf_twist_01_l/r` and `thigh_twist_01_l/r`. Under retargeted Humanoid clips the boots and
lower-leg garments detached from the legs, visible at **front**, **side**, **medium** and
**combat** camera distance across `Walk`, `Run`, `Aim`, `Fire`, `Reload`, `Hit` and `Death`.

`calf_twist_01_l` is a child of `calf_l` with a local rotation only ~0.1 deg different from
`calf_l`, i.e. functionally the same bone in the native rig.

### 9.2 Rejected repair route — must not be reused

Writing `UnityEngine.Mesh.boneWeights` at runtime in Unity 6 **corrupts the skinning on its
own.** A NOOP control that rewrote the weights with completely unchanged values reproduced
the same catastrophic collapse as the real repair. Two `Bandit` weight-rewrite bugs were also
found and fixed in that investigation (stale vertex-group lookup creating `calf_l.00x` groups;
skipping zeroed groups on `REPLACE` which doubled every vertex weight sum).

### 9.3 Accepted repair — Option B, Blender

Fold twist weight onto the mapped parent bone, renormalise to 1.0, export from Blender with
Unity axis convention `axis_forward="-Z"` / `axis_up="Y"`:

| From | To | Fallback |
|---|---|---|
| `calf_twist_01_l/r` | `calf_l/calf_r` | `foot_l/foot_r` on `BanBoots`, which has no `calf_l` group |
| `thigh_twist_01_l/r` | `thigh_l/thigh_r` | — |

Armature untouched (118 bones), no bone renamed/moved/re-parented, no material, texture,
weapon rig or topology change.

**Blender export-axis trap:** exporting with `axis_forward="-Y"` / `axis_up="Z"` rotates the
rig 90 deg about X and destroys the humanoid retarget. Any future Bandit export must use
`-Z` / `Y`.

### 9.4 Result

| Metric | Before | After |
|---|---|---|
| twist-bone weight, 3 meshes | 37.15 / 44.38 / 34.83 % | **0.00 / 0.00 / 0.00 %** |
| bones outside Human map | 2 / 4 / 4 | **0 / 0 / 0** |
| max influences | 3 / 4 / 4 | 2 / 4 / 4 |
| weight-sum range | 1.0000 | 1.0000 |
| unnormalised / zero-weight verts | 0 / 0 | **0 / 0** |
| Avatar | valid, human, 55 / 164 | **valid, human, 55 / 164** |
| transforms / skinned meshes | 164 / 39 | **164 / 39** |

Pose metrics identical before and after for all 8 clips — the repair changes skinning only,
never a bone. Weapon `gunGap` constant at 0.084 m, no foot skating, no popping, no regression
in `Aim` / `Fire` / `Reload`.

### 9.5 Attribution caveat

A clean re-export through the corrected axis convention removes the visible tearing on its
own, with the twist weights untouched. The A/B therefore **cannot** attribute the visual
improvement to the weight change alone. The weight repair is still the recommended file
because it produces the same visual result while removing the 37-44 % structural dependency
on bones Unity does not drive, at zero measured cost.

### 9.6 Status

- Deformation: **`FIXED`** · Technical: **`PASS`** · Visual: **`A`** · Gameplay: **`A`**
- `Walk` and `Run` after repair: **`A`**
- Production integration: **NO** — repaired FBX exists only in a disposable workspace
- Human approval: **NOT GRANTED**

---

## 10. Cross-references

- `ART-0001` — art programme state
- `ART-0002` — per-asset register (state machine)
- `ART-0003` — pipeline and QA gates
- `History/HISTORY-0004` — E1 Hat causal development history
- `History/OPEN_DECISIONS.md` — unresolved human decisions
- `PROJECT_TRUTH.md` §7 — art truth, incl. approved Character Foundation

---

**End of `ART-0004-THIRD-PARTY-ASSET-CATALOG.md`**

---

## 10. COWBOY ELDERLY - LICENCE VERIFICATION AND PRODUCTION PROMOTION (2026-10-20)

This addendum updates section 6.1.4. It does not delete the 2026-10-07 audit record.

| Field | Verified value (2026-10-20) |
|---|---|
| Asset | Cowboy Elderly, ART_I_SHOC |
| Fab listing (current) | https://www.fab.com/listings/f6c625e6-c08b-47d2-bc96-cb6da7fe8a23 |
| Price on Fab | **Free** (Download / Add to My Library, no purchase) |
| License | **Fab Standard License** (https://www.fab.com/eula) |
| Attribution | **Not required** ("You are not required to give credit to the publisher") |
| AI usage | **Allowed** ("Allows usage with AI: Yes" on the listing) |
| Revenue threshold | **Not engaged.** The asset is Free, and the Fab summary states: "Both pricing tiers (Personal and Professional) grant you the same scope of rights. There is no need for upgrading from Personal to Professional tier if you cross the revenue threshold after the purchase." |
| Commercial use of the game | **Allowed** - "Commercially distribute your Projects with the Fab assets incorporated into it" |
| Standalone redistribution of the asset | **Not allowed** - "Resell or redistribute the asset for free on a standalone basis" |
| Local copy | cowboyelderly.unitypackage, 508.8 MiB, supplied by the owner on 2026-10-19/20; imported as Assets/CowboyCharacter/** (character only, 109 files) |
| Identity proof | Unchanged from 6.1.4: measured 13 511 faces / 14 103 verts / 26 461 tris match the listing text exactly; the additional bones (jaw, gun, eyelids, eyes, eyebrows, smile, holster, pigtail) all exist in the packaged FBX |
| Production status | **INTEGRATED** - the model is the active enemy visual in Assets/Prefabs/Enemies/Doomy_{Bandit,Rusher,Shooter,Tactical}.prefab as nested prefab Enemy_Model |

### 10.1 Recorded technical deviations from the package

1. The package materials referenced a custom Quaternius shader GUID 6e4ae4064600d784cac1e41a9e6f2e59 that is **absent from the package**; the character rendered magenta. They were rebuilt as Universal Render Pipeline/Lit materials WWG_cowboy_* (8), with the mask layout measured from the TGA files (R = metallic, A = smoothness, G = AO).
2. The listing states "It is desirable to use a shader with two-sided display of polygons in the model" - the hair/eyelid cards are single sided, so they show black from the back. Not a texture defect; documented as a rendering note.

### 10.2 Remaining gap

The **interactive "License terms" panel of the listing is rendered client side and could not be captured as text** in this environment. The binding text used above is the Fab Standard License summary page (https://www.fab.com/eula), which is the licence stated by the listing. Evidence of origin for the local copy is the owner-supplied package plus the identity match in 6.1.4. No claim is made beyond what is written here.