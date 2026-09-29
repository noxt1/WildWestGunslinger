# ART-0003 — Art Pipeline and QA Gates

**Document ID:** `ART-0003`
**Status:** CANONICAL
**Date:** 2026-09-29

---

## 1. Intended pipeline

```
Reference (public domain / CC0 / CC BY)
        ↓  logged in _REFERENCES/SOURCES.md
Blender source  (.blend in Documents\WildWestGunslinger art)
        ↓  measurement docs + clearance profile
QA PASS  (multi-view renders: FRONT/BACK/SIDE/3Q + detail views)
        ↓  human APPROVED
FBX export  (FBX\ and Unity_Export\)
        ↓  PROMOTED
Assets\  (Characters / Environment / Props)
        ↓  INTEGRATED
Prefab / Scene
        ↓  RUNTIME VERIFIED
Play-mode confirmation
```

---

## 2. Measurement discipline (established, and canonical)

The character work uses a **measurement-first** method, documented in
`Doomy_measurements.md`, `Doomy_vest_measurements.md` and the two clearance
profiles. This is the project's most rigorous art process and is the model for
future work.

### Required conventions

| Rule | Value |
|---|---|
| Units | millimetres |
| X axis | width |
| Y axis | depth (front = −, back = +) |
| Z axis | up |
| Body basis | `WWG_Zone_*` template meshes |
| Armature | `WWG_Template_Armature`, 51 bones |

### Clearance targets (mm)

| Garment | Target clearance |
|---|---|
| Hat interior | radial 15–20, top 30–35 |
| Shirt torso | 8–12 |
| Sleeves | 6–12 |
| Pants | thigh 8–14, shin 7–12 |
| Boots | 10–18 around foot; shaft overlap 30–40 above ankle |
| Belt | 12–20 over shirt + trousers |
| Holster | 5–10 from thigh |

A measured value below target is acceptable **only** when explicitly recorded as
an intentional fitted result ("no breakthrough", "no clipping") in the clearance
profile. Undocumented intersection is a defect.

### Dead-zone rule

Garments are measured against the **outer garment surface**, never the body.
The body is a dead-zone: `Doomy_vest_measurements.md` states explicitly that the
body must not be touched while building the vest. This prevents torso deformation
from being baked into clothing.

---

## 3. QA render conventions

### Character QA (in `WWG_Doomy_Cowboy\QA\`)

Checkpoint-prefixed, ~120 renders, 2026-09-26:

| Series | Views |
|---|---|
| `CK1`–`CK3` | 3Q, ARMHOLE, BACK, COLLAR, CUFF, ELBOW, FRONT, P_ELBOW, P_FORWARD, P_NECK, P_RAISED, P_REST, SHOULDER, SIDE_L, SIDE_R, UNDERARM, UNDER_L, UNDER_R |
| `CK4`–`CK7` | SH_3Q, SH_BACK, SH_FRONT, SH_SIDE, UNDER_L, UNDER_R |
| `CK8`–`CK18` | UNDER_L / UNDER_R pairs |
| `R_*` | 3Q, ARMHOLE, BACK, ELBOW, FRONT, LEFT, RIGHT, SHOULDER, UNDERARM |
| `SHIRT_*` | 3Q, BACK, FRONT, LEFT, RIGHT, Z_ARMPIT, Z_COLLAR, Z_CUFF, Z_HEM, Z_SHOULDER |

**The standard view set is 3Q + FRONT + BACK + SIDE_L/R + UNDERARM + ARMHOLE +
SHOULDER + COLLAR + CUFF + ELBOW + HEM + under-left + under-right.** Any future
asset must ship this minimum set.

### Vest / glove QA (`Desktop\Коллаж тест VVG\01_Vest\`)

2×2 collages plus close-ups, e.g. `Vest_VisualQA_2x2`, `Vest_ShirtFit_Closeup`,
`Gloves_MarkFix_2x2`, `Vest_FinalRepair_SB.png`.

---

## 4a. Recorded 3D-asset precedents

**Added in Phase 3.** These are **recorded precedents** derived from practice on
`BARREL_01` / `FENCE_01` / `CRATE_01`, documented in `AI_CONTEXT/ART_PIPELINE.md`.
They are recorded as **evidence and precedent**, not as newly adopted design
decisions — no separate approval record exists for them as a project-wide rule.

### 4a.1 Vertex colour attribute — the `Col` finding

| Item | Value |
|---|---|
| **Precedent** | **`Col` = `FLOAT_COLOR` / `CORNER`** |
| **Prohibited** | **`BYTE_COLOR`** |
| Why | `BYTE_COLOR` stores sRGB. A value written as linear is read back roughly **12× too dark** (0.074 → 0.0063). This genuinely broke the readability of the wood surface |
| Status | **RECORDED PRECEDENT** — demonstrated on `BARREL_01`, `FENCE_01`, `CRATE_01` |
| Approval | Not separately approved as a project-wide rule. Treat as strong prior art, not as an adopted decision (`GD-07` still applies) |

**Semantics of `Col`:** RGB = base tone of the element (wood / metallic);
**A = metal mask** (0 = wood, 1 = metal), with `Metallic` taken directly from A.

### 4a.2 Unified material values

| Material | Value |
|---|---|
| Dark old iron | linear `(0.0785, 0.0794, 0.0830)` — **identical across all assets** |
| Dark oak | hue ratio ≈ `1 : 0.77 : 0.567`, linear range ≈ 0.064–0.082 |
| Wood value in render | sRGB p50 ≈ 0.41–0.49 (Barrel 0.414, Fence 0.482, Crate 0.493) |

### 4a.3 UV and slot rules

| Rule | Value |
|---|---|
| UV | world-proportional, per-face planar, **U along the element's long axis** (boards/logs along length, posts vertical) |
| Grain | must be continuous across fragment boundaries |
| Material slots | **exactly 1** slot per object (intact and each fragment) |
| Image textures | **prohibited** at the current stage. M2 = vertex attributes + simple PBR |

### 4a.4 Viewport-jitter root cause (confirmed)

The "jitter" of the wood surface in the Blender viewport was **not** a material,
geometry, or TAA problem. Confirmed cause: the **intact mesh and the reassembled
fragments were displayed simultaneously on coincident surfaces**. Fixed by
displaying one or the other.

### 4a.5 FBX export contract

`AI_CONTEXT/ART_PIPELINE.md` records the FBX export contract as **frozen** and
mandatory for all exports without deviation. It is preserved there as the
authoritative text; it is not duplicated here to avoid two sources drifting apart.

> **Status note.** These are precedents from completed work. They are **not**
> evidence that any of it is integrated or runtime verified — see
> `ART-0002-ART-ASSET-REGISTER.md` §4.1.

## 5. QA gates

| Gate | Requirement | Current status |
|---|---|---|
| G1 Reference provenance | every reference logged with licence in `_REFERENCES/SOURCES.md` | **PASS** (39 images) |
| G2 Measurement | measurement doc exists for the asset | **PASS** for character; vest measured over shirt |
| G3 Clearance | clearance profile with measured facts | **PASS** for 7 garment groups |
| G4 Multi-view QA | standard view set rendered | **PASS** for character/vest/gloves |
| G5 Human approval | explicit recorded `APPROVED` | **PASSED for `BARREL_01`, `FENCE_01`. NOT PASSED for `CRATE_01` and all other assets** |
| G6 Promotion | copy to target + record | **NOT STARTED** |
| G7 Integration | wired into prefab/scene | **NOT STARTED** (0 modular meshes) |
| G8 Runtime verification | confirmed in play mode | **NOT STARTED** |

**The pipeline is stuck at gate G4→G5.** Everything up to G4 is genuinely done;
nothing beyond it has begun.

---

## 6. Known pipeline defects

| # | Defect | Effect |
|---|---|---|
| P1 | Blender sources live outside the repository | no version control on 307 art files |
| P2 | 32 `WWG_Foundation_Source` variants with no canonical selection | impossible to tell which is current |
| P3 | 24 `.blend1` backups interleaved with sources | sources and backups not separable |
| P4 | `FBX/` and `Unity_Export/` are byte-identical duplicates | ambiguity about which is authoritative |
| P5 | `REFERENCE_INDEX.txt` states original source images could not be extracted | index-only provenance for 14 references |
| P6 | Unity FBX import resolves to 0.01× with uncompensated Z-up | **blocks G7 entirely** |
| P7 | Art work predates git initialisation (2026-09-25) | no provenance in repo history |

---

## 7. Cross-references

- `ART-0001` — art state and measured data
- `ART-0002` — asset register
- `UNI-0003` — Unity defects (P6 is `UNI-D06`)
- `History/OPEN_DECISIONS.md` — canonical variant selection

---

**End of `ART-0003-ART-PIPELINE-AND-QA-GATES.md`**
