# ART-0001 — Art State

**Document ID:** `ART-0001`
**Status:** CANONICAL
**Date:** 2026-09-29
**Evidence class:** verified file facts + measured art documents

---

## 1. Headline

**Two assets are `APPROVED` (`BARREL_01`, `FENCE_01` — recorded user
confirmation). One is at `QA PASS` awaiting approval (`CRATE_01`, CP1 complete,
CP2 not authorized). Nothing is `PROMOTED`, `INTEGRATED`, or `RUNTIME VERIFIED`.**

The scene contains 294 `WWG_*` objects, but the playable character forms are
Capsules and the modular environment kit is not integrated. Art production is
**ahead of Unity integration by a wide margin**.

---

## 2. Where the art actually lives

There are **three** art locations. None is versioned, none is designated
canonical, and none is covered by the 2026-09-29 recovery snapshot.

| # | Location | Files | Size | Newest | Nature |
|---|---|---|---|---|---|
| 1 | `~/Documents/WildWestGunslinger art` | 307 | 180.2 MB | 2026-09-26 | **Character art source of record** |
| 2 | `~/Desktop/Коллаж тест VVG` | 25 | ~13.5 MB | 2026-09-26 | Vest/glove QA collage |
| 3 | `Working/` (repo) | 747 | 226.1 MB | 2026-09-29 16:34 | Blender sources + QA reports |

A fourth, read-only copy exists at
`Z:\Мой диск\WWG_RECOVERY\2026-09-22_FULL\WildWestGunslinger_art` (161 files,
96.9 MB) as part of the 2026-09-22 backup.

---

## 3. In-repository art source — `Working/blender_src/`

**Added in Phase 3.** This is a **fourth** art location: inside the repository,
tracked-eligible but currently **untracked**.

| Metric | Value |
|---|---|
| Path | `Working/blender_src/` |
| Production working `.blend` files | **7** (exact count established by audit) |
| Git status | `?? Working/blender_src/` — **untracked** |

### 3.1 Exact inventory (verified on disk)

| Working `.blend` | Bytes | Asset |
|---|---|---|
| `barrel/Barrel_01_Working.blend` | 867 831 | `BARREL_01` — **`APPROVED`** |
| `fence/Fence_01_Working.blend` | 891 960 | `FENCE_01` — **`APPROVED`** |
| `crate/Crate_01_Working.blend` | 130 858 | `CRATE_01` — CP1 complete, **CP2 not authorized** |
| `cover/Cover_Western_Wooden_Working.blend` | 145 240 | Cover — state recorded in `ART-0002` |
| `doomy/Doomy_Character_Production_Working.blend` | **2 568 092** | **Doomy character production source** |
| `wall_segment/WallSegment_01_Working.blend` | 116 315 | Wall module — `DEC-04` |
| `floor_segment/FloorSegment_01_Working.blend` | 101 021 | Floor module — `DEC-04` |

> **`Doomy_Character_Production_Working.blend` (2 568 092 B) is a character
> production source inside the repository.** It is distinct from the external
> character lineage in §4 and was not enumerated before Phase 3.

### 3.2 The four states must not be conflated

| Question | Answer for `Working/blender_src/` |
|---|---|
| Does the source **exist**? | **Yes** — 7 working `.blend`, verified |
| Is it **backed up**? | **Yes, partially** — inside the repo, therefore inside the 2026-09-29 recovery point (`REL-0001`). The **external** character source in §4 has its own verified backup |
| Is it **Unity-integrated**? | **No** — `UNI-D11` (0 modular meshes), `UNI-D12` (0 destructibles), `UNI-D02` (0 SkinnedMesh) |
| Is it **runtime verified**? | **No** — none of these assets appears in the runtime scene census |

---

## 4. External character art — `Kevin Iglesias / WildWest`

Path: `~/Documents/WildWestGunslinger art\art\Characters\Kevin Iglesias\WildWest`

### 4.1 Canonical body data (measured, not estimated)

| Measurement | Value |
|---|---|
| Basis | Kevin Iglesias human character dummy, size **M** |
| Height to crown | **1818 mm** |
| Torso Z range | 926–1579 |
| Legs Z range | 119–1023 |
| Head Z range | 1537–1818 |
| Head height | 281.1 mm |
| Head width (X) | 198.2 mm |
| Head depth (Y) | 255.4 mm (front −164.4 / back +91.0) |
| Chest | **100** (size-50 table, chest width 21) |
| Armature | `WWG_Template_Armature`, **51 bones** |
| Source lineage | `WWG_Zone_*` template meshes + armature in `WWG_Foundation_Source.blend` |

Axes convention: **X = width**, **Y = depth (front − / back +)**, **Z = up**. All
values in millimetres.

> **CORRECTION:** `AUDIT_2_RECONCILIATION_REPORT.md` claims `Chest=141cm` and
> `Skeleton=62 bones`. Both are false. Use 100 chest and 51 bones.

### 4.2 `WWG_Doomy_Cowboy` — confirmed present

Audit 1 reported `WWG_Doomy_Cowboy_REFINED.blend` as **NOT FOUND**. That was a
**search-scope error** — the file exists outside the project directory.

| File | Bytes | Modified | Note |
|---|---|---|---|
| `WWG_Doomy_Cowboy_WORK.blend` | 1 704 329 | 2026-09-21 17:00 | base work |
| `WWG_Doomy_Cowboy_APPROVAL_CANDIDATE.blend` | 2 309 505 | 2026-09-22 15:48 | approval candidate |
| `WWG_Doomy_Cowboy_REFINED.blend` | 2 360 988 | 2026-09-22 16:16 | **Audit 1 target — exists** |
| `WWG_Doomy_Cowboy_VEST_APPROVAL_CANDIDATE.blend` | 2 364 062 | 2026-09-22 17:12 | vest candidate |
| `WWG_Doomy_Cowboy_VEST_WORK.blend` | 2 551 057 | 2026-09-25 12:05 | vest work |
| `WWG_Doomy_Cowboy_VEST_MARK1_REPAIR_SB_WORK.blend` | 2 552 003 | 2026-09-25 23:33 | mark repair |
| `WWG_Doomy_Cowboy_VEST_FULL_REPAIR_SB_WORK.blend` | 2 567 279 | 2026-09-26 00:10 | vest full repair |
| `WWG_Doomy_Cowboy_FINAL_REPAIR_SB_WORK.blend` | 2 568 092 | 2026-09-26 00:33 | final repair |
| `WWG_Doomy_Cowboy_FINAL_REPAIR_SB_WORK_PRESAVE_20260926_053821.blend` | 16 196 184 | 2026-09-26 05:38 | presave |
| `WWG_Doomy_Cowboy_CLOTHING_REBUILD_SB_WORK.blend` | 2 179 296 | 2026-09-26 12:02 | **newest work file** |

Plus 24 `.blend1` backups, a `QA/` folder with **~120 PNG renders**
(checkpoints `CK1`–`CK18`, plus `R_*` and `SHIRT_*` view sets), and
`WWG_Doomy_Cowboy_Vest_Inspection_Collage.png`.

**No file in this set is designated canonical.** Latest-in-time is
`..._CLOTHING_REBUILD_SB_WORK.blend` (2026-09-26 12:02), but "latest" is not the
same as "approved". See `History/OPEN_DECISIONS.md`.

> **Filename warning.** Names containing `FINAL`, `FINAL_REPAIR`,
> `APPROVAL_CANDIDATE` are **filenames, not states**. `FINAL` in a name is
> necessary but not sufficient. `APPROVAL_CANDIDATE` is a *candidate*, i.e.
> earlier than approved. Approval requires a **recorded human confirmation**.
> See `ART-0002-ART-ASSET-REGISTER.md` §0 and §4.

### 4.3 `WWG_Foundation_Source` lineage — 32 variants, none canonical

2026-09-21 → 2026-09-22. Variants include `_baseline`, `_baseline_DesignAssets_20260922`,
`_baseline_FinalCorrective_20260922`, `_baseline_FiveChar_20260921`,
`_baseline_PatternTest_20260922`, `_baseline_PlayerPass_20260921`,
`_baseline_Polish_20260921`, `_baseline_VestAssembly_20260922`,
`_baseline_VestNEW2_20260922`, `_DesignAssets`, `_FiveChar`, `_PlayerPass1`,
`_PlayerPass2`, `_PlayerPass2_protected`, `_PlayerVestRepair`, `_Polish`,
`_VestAssembly`, `_VestClean`, `_VestOpusRepair`, `_VestOpusRepair_V2`,
`_VestOpusRepair_V3`, `_VestPatternPatch`, plus the base
`WWG_Foundation_Source.blend`.

### 4.4 Unity export sets

| Set | Assets |
|---|---|
| `FBX/` | `WWG_Player`, `WWG_Bandit`, `WWG_Rusher`, `WWG_Shooter`, `WWG_Tactical`; weapons `WWG_Carbine`, `WWG_Revolver`, `WWG_Shotgun`, `WWG_Winchester` |
| `Unity_Export/Characters/` | byte-identical duplicates of the 5 character FBX |
| `Unity_Export/Weapons/` | byte-identical duplicates of the 4 weapon FBX |

**None of these FBX are integrated in Unity** — the runtime shows Capsules and
0 modular meshes.

---

## 5. Garment clearance — measured profile

From `Doomy_clearance_profile.md` (target clearance vs. measured fact, mm):

| Garment | Target | Measured fact |
|---|---|---|
| Hat interior | radial 15–20, top 30–35 | radial 16 / depth 17 / top 32 — **within target** |
| Shirt torso | 8–12 | min Torso↔Shirt **1.2** — **tight in shoulders, no clipping** |
| Sleeves | 6–12 | Hands↔Shirt **4.3**; elbow bridges close the Kevin zone gap |
| Pants | thigh 8–14, shin 7–12 | Legs↔Pants **3.2** — fitted, no breakthrough |
| Boots | 10–18 around foot, shaft overlap 30–40 | Feet↔Boots **5.9**; shafts tucked into trousers |
| Belt | 12–20 over shirt+trousers | Shirt↔Strap min **25.5** — rigid oval, contacting front/back/sides |
| Holster | 5–10 from thigh | **8.5** — within target |

> Several garments measure **below** their nominal target clearance but are
> documented as intentional "fitted, no breakthrough / no clipping". This is
> an art-direction choice, not a defect.

---

## 6. Vest measurements

Measured over the **SHIRT** surface, with the body explicitly treated as a
dead-zone (not touched). Chest section, by z-slice (mm):

| z | width | depth | circumference ≈ |
|---|---|---|---|
| 1.42 | 435.1 | 262.2 | 1112 |
| 1.35 | 407.8 | 277.6 | 1086 |
| 1.26 | 393.6 | 257.8 | 1034 |
| 1.17 | 320.6 | 226.9 | 866 |
| 1.08 | 311.4 | 222.3 | 844 |

### Vest reference design (from `00_Vest_Reference/REFERENCE_INDEX.txt`)

Brown suede western vest: moderate V-neck; snap buttons down the centre front;
left chest welt pocket; two lower welt pockets; pointed front hems overlapping
the belt; dark back with a cinch strap and buckle. Reference 07 (size-50 table)
gives chest 100, neck width 8, armhole height 25, back length 46, back width
20.5, armhole width 13, chest width 21 — "our torso ~100", consistent with
`Doomy_measurements.md`.

> **Open gap:** the original source images referenced by the index **could not be
> extracted** from the original chat attachments. `REFERENCE_INDEX.txt` is a
> catalogue only. See `History/OPEN_ISSUES.md`.

---

## 7. Reference policy

`_REFERENCES/SOURCES.md` (39 images) states: all references are
**public domain / CC0 / CC BY**, used **only** as construction and silhouette
reference for 3D modelling. No image is copied into geometry, used as a texture,
or transferred into Unity.

This policy is **canonical and binding**. Any new reference must be recorded in
`_REFERENCES/SOURCES.md` with its licence before use.

---

## 8. QA collage (`Desktop\Коллаж тест VVG`)

25 files, 2026-09-23 → 2026-09-26 — vest and glove QA renders, invisible to the
project documentation:

- `01_Vest/`: `Vest_VisualQA_2x2`, `Vest_VisualQA_REVISION_2x2`,
  `Vest_TechnicalQA_REVISION_2x2`, `Vest_ShirtFit_Closeup`,
  `Vest_ShirtFit_FinalQA_2x2`, `Vest_ShirtFit_TopQA_2x2`,
  `Vest_ReferenceAccurate_LowPoly_Closeup`, `Vest_ReferenceAccurate_LowPoly_VisualQA_2x2`,
  `Vest_UpperShoulder_TopQA_2x2`, `Vest_FinalCorrection_2x2`,
  `Vest_FinalCorrection_Closeup`, `Vest_FinalCorrection_TopQA_2x2`
- `01_Vest/Vest_Repair/`: `Vest_Repair_2x2`, `Vest_Repair_Closeup`,
  `Vest_Repair_TopQA`, `Vest_MarkFix_2x2`
- `01_Vest/Gloves_Repair/`: `Gloves_Repair_2x2`, `Gloves_MarkFix_2x2`
- `01_Vest/FinalRepair_SB/`, `01_Vest/Gloves_FinalRepair_SB/`
- `WesternShootingGloves_VisualQA_2x2.png`, `WesternShootingGloves_FinalCorrection_2x2.png`
- `wildwestgunslinger-directory.json` (11.94 MB directory export)
- `2026-09-26_13-44-51.png`

These are **QA PASS evidence** for vest and glove work — they are why the art
state is `QA PASS` and not merely `CREATED`.

---

## 9. Art delta

**Zero art changes after the 2026-09-29 16:42:59 recovery point.**
See `ART_DELTA_AFTER_RECOVERY.md`.

---

## 10. Cross-references

- `ART-0002` — asset register with per-asset state
- `ART-0003` — pipeline and QA gates
- `UNI-0003` — Unity defects blocking integration
- `History/OPEN_DECISIONS.md` — character foundation identity

---

**End of `ART-0001-ART-STATE.md`**
- **`ART-0004` — third-party asset catalog (single canonical licence/attribution registry; E1 Hat record in §5.1)**
- `History/HISTORY-0004` — E1 Hat causal development history

---

## 11. E1 — Hat (2026-09-30)

**`APPROVED WITH FIT CAVEAT`** — third-party asset (`Cowboy Hat` by MadeByYeshe,
**CC BY 4.0**) fitted to the approved Character Foundation. Assembly synchronised under
`WWG_ThirdParty_Hat_Root`; **0** BVH triangle-level intersections; a non-zero inner gap
(**≈ 4.21 mm**) remains as a recorded caveat against intended-camera presentation.
Not `PROMOTED` / `INTEGRATED` / `RUNTIME VERIFIED`. Low-poly deferred.

Full record — `ART-0004` §5.1 · register entry — `ART-0002` §2a · history — `History/HISTORY-0004`.
