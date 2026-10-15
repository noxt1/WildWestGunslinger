> **CANONICAL LOCATION:** `Documentation/Character/CHARACTER_FOUNDATION_CONTRACT.md`
>
> This file is the **single full copy** of the Character Foundation Contract. The former
> location `AI_CONTEXT/CHARACTER_FOUNDATION_CONTRACT.md` is now a short pointer to this file and
> holds no contract text. Moved here 2026-10-15 during Phase A5 documentation remediation (conflict
> `C1`): 23 documents referenced a path that was never published to the canonical documentation
> tree. Contract content below is preserved verbatim from the source file at move time.
# CHARACTER FOUNDATION CONTRACT

> **Status:** OPERATIONAL / CANONICAL / **APPROVED** — created 2026-09-29, **approved 2026-09-30**
> **Role:** authoritative definition of the WWG Character Foundation skeleton.
>
> **APPROVED 2026-09-30** by the project owner after personal review of F3–F9:
> `WWG_Character_Foundation_Working.blend`
> (SHA-256 `B14CE5A151EFDE19026CAF6FD25A2368677B5B4A6E964288EABD7C91B9D256EF`),
> `WWG_Rig` **53 bones / 52 deform / 1 non-deform `B-root`**, vendor source
> `HumanCharacterDummy_M.blend` (`6D6DB266…`).
> Final validation: `Working\reports\new_character_foundation_final_validation_2026-09-30.md`
> (TECHNICAL PASS). Human approval:
> `Working\reports\character_foundation_final_human_approval_2026-09-30.md`.
> **This approval covers the foundation as the basis for the modular equipment pipeline. Unity
> integration remains NOT VERIFIED and no FBX has been exported.**
>
> **Supersedes:** the former `51-bone shared skeleton` contract
> (`WWG_Template_Armature`, `Doomy_measurements.md`), which is **HISTORICAL / NON-CANONICAL**.
> **Production file:** `Working\blender_src\character_foundation\WWG_Character_Foundation_Working.blend`
> **Evidence:** F2 proposal + F2 rig contract + F3 skeleton report + F4 attachment + F5 binding
> + F5 visual + F6 rest/skinning + F7 pose/deformation + F8 measurements + F9 final
> validation (see §9)

---

## 1. AUTHORITY ORDER

When sources disagree, the higher item wins:

1. **Actual state of the verified production `.blend`**
2. **This contract** and current canonical project documentation
3. **F2 Foundation Contract** (`Working\reports\new_character_foundation_rig_contract_2026-09-29.md`)
4. **F3 Skeleton Report** (`Working\reports\new_character_foundation_f3_skeleton_2026-09-29.md`)
5. **Pristine vendor source** (`HumanCharacterDummy_M.blend`, sha256 `6D6DB266…`)
6. Historical / archive material — **NON-PRODUCTION**, reference only

The former 51-bone foundation is **no longer a normative production foundation.**

---

## 2. SKELETON

**`WWG_Rig` is the 53-bone shared Character Foundation** (52 deform + 1 non-deform root).

| Property | Value |
|---|---|
| Armature object | **`WWG_Rig`** |
| **Total bones** | **53** |
| **Deform bones** | **52** |
| **Non-deform** | **1** — `B-root` |
| Constraints | 0 |
| Mechanism / IK / FK / roll bones | 0 |
| Vendor source | pristine, unchanged (`6D6DB266…`) |

Composition: 7 axial (`B-root`, `B-hips`, `B-spine`, `B-chest`, `B-neck`, `B-head`, `B-jaw`)
+ 8 arm + 8 leg + 30 finger = **53**. 23 L/R pairs, max length asymmetry 0.002 mm.

---

## 3. NAMING

Vendor `B-*` names are preserved **exactly**. No WWG renaming scheme is introduced.
Existing tooling and measurements that use `B-*` continue to apply.

---

## 4. EXCLUDED BONES

Not present in the production foundation:

* `B-handProp.L`
* `B-handProp.R`
* `B-spineProxy`
* all vendor mechanism bones (IK / FK / foot & toe roll / pole guides / prop chains)

**Rationale (measured, F2 §5.2):** the three excluded bones have **no vertex group** —
their removal loses no weight. `B-spineProxy` duplicates `B-spine`
(same length 200.329 mm) and carried a deformation-double risk.

---

## 5. ROOT

| Property | Value |
|---|---|
| `B-root` head | **`(0, 0, 0)`** |
| `B-root` parent | **null** |
| `B-root` `use_deform` | **false** |
| `B-root` `use_connect` | false |
| `B-root` length | 303.1327 mm |
| `B-hips` head | `(0, −0.016772, 0.979712)` |
| Gap `B-root` tail → `B-hips` head | **≈ 676.787 mm** |

**The ≈676.787 mm gap is INTENTIONAL and is NOT a defect.**
`B-root` is the ground-level character / root-motion pivot; the pelvis sits higher.
Do **not** connect `B-root` and `B-hips` artificially, do **not** move `B-root`,
do **not** adjust the pelvis to the root.

---

## 6. VENDOR-DERIVED BONE LENGTHS

Canonical values come **only** from the pristine vendor source:

| Bone | Canonical length |
|---|---:|
| `B-hand` | **146.4458 mm** |
| `B-pinky01` | **42.3614 mm** |
| `B-head` | **218.7954 mm** |
| `B-jaw` | **158.6917 mm** |
| `B-chest` | **270.7316 mm** |
| `B-toe` | **117.9809 mm** |

**The former 51-bone measurements must NOT be used**, in particular:
`B-hand = 130.352` (wrong by −16.094 mm), `B-head`/`B-jaw = 0.100` (degenerate),
`B-chest = 217.730`, `B-toe = 177.444`, `B-pinky01 = 880.482`.

Full 53-row table with parent / `use_connect` / deform status:
`Working\reports\new_character_foundation_rig_contract_2026-09-29.md` §3.3.

---

## 7. REST POSE

* **T-pose** (vendor rest pose)
* vendor rest preserved; F3 deviation from vendor **≤ 6 µm**
* arm span 1.822682 m · height 1.817900 m · evaluated verts 5392

Do not convert to A-pose without a concrete technical reason; that is a separate task.

---

## 8. COORDINATE SYSTEM

| Axis | Convention |
|---|---|
| `+Z` | up |
| `−Y` | forward |
| `+X` | character's left |
| Origin | `(0, 0, 0)`, ground plane, body midline |
| Unit scale | 1 Blender unit ≈ 1 m |
| Character height | ≈ 1.81826 m |
| Units in file | `METRIC`, `scale_length` 1.0, `METERS` |

**Do not rotate the character for Unity inside the foundation.**
Unity axis conversion is a separate, **NOT VERIFIED** task requiring a real
Blender → FBX → Unity test.

---

## 9. EVIDENCE

| Artifact | Path |
|---|---|
| F2 proposal | `Working\reports\new_character_foundation_proposal_2026-09-29.md` |
| F2 rig contract | `Working\reports\new_character_foundation_rig_contract_2026-09-29.md` |
| F3 skeleton report | `Working\reports\new_character_foundation_f3_skeleton_2026-09-29.md` |
| F4 attachment report | `Working\reports\new_character_foundation_f4_attachment_binding_2026-09-29.md` |
| F5 body binding report | `Working\reports\new_character_foundation_f5_body_binding_2026-09-30.md` |
| Source reset | `Working\reports\doomy_source_reset_2026-09-29.md` |
| RR-1 correction | `Working\reports\project_reconnaissance_baseline_v3_CORRECTION.md` |
| Canonical source | `…\Kevin Iglesias\WWG_Doomy_Canonical_Source\HumanCharacterDummy_M.blend` |
| Production file | `Working\blender_src\character_foundation\WWG_Character_Foundation_Working.blend` |

Production file sha256 (F3 baseline): `2920041DDCA9179EE4B564DF88D4FCEB55DBDBAFE396BF0178AB2426FAAC2A88`
Production file sha256 (current, after F4/F5): `B14CE5A151EFDE19026CAF6FD25A2368677B5B4A6E964288EABD7C91B9D256EF`
Vendor sha256: `6D6DB2666CC6214DF7E71A955A5861A8E9A460C6310246E733694EABB9727D8E`

---

## 10. ATTACHMENT BASELINE

**F4 COMPLETE — measured attachment transforms are CANONICAL.**
Evidence: `Working\reports\new_character_foundation_f4_attachment_binding_2026-09-29.md`
F4 status: **NUMERIC PASS** (user-confirmed before F5).

| Name | Parent bone | Purpose | Status |
|---|---|---|---|
| `WeaponPoint_L` | `B-hand.L` | left-hand interaction / two-handed grip | **CANONICAL (F4)** |
| `WeaponPoint_R` | `B-hand.R` | revolver / right-hand weapon | **CANONICAL (F4)** |
| `HolsterPoint` | **`B-hips`** | revolver holster | **CANONICAL (F4)** |
| `BackWeaponPoint` | **`B-chest`** | Winchester / shotgun / long back attachments | **CANONICAL (F4)** — measured, not guessed |

Rules:

* These are **attachment objects (empties)**, not bones; they are not part of the 53.
* `HolsterPoint` uses **`B-hips`**. **`B-spine` must NOT be used** — the former
  spine-parented holster was a defect and must not be reproduced.
* `matrix_parent_inverse` = identity; position expressed only via explicit `loc` / `rot`.
* Attachment names are stable and must not be renamed.
* `BackWeaponPoint` is on `B-chest`, derived from measurement: of the torso vertices on
  the back surface, **323 are `B-chest`-dominant vs 84 `B-spine`-dominant**.
* Canonical local locations (metres, bone space):

  | Object | local location |
  |---|---|
  | `WeaponPoint_L` | `(-0.0034903, -0.0613837, 0.0006913)` |
  | `WeaponPoint_R` | `(0.0034903, -0.0613835, 0.0006917)` |
  | `HolsterPoint` | `(-0.1606045, -0.0729709, -0.0020341)` |
  | `BackWeaponPoint` | `(0.0788862, -0.1293243, -0.1116658)` |

### 10.1 BONE-PARENTING LENGTH COMPENSATION — MANDATORY TECHNICAL RULE

> **In Blender 5.2.2 LTS, a bone-parented Empty under the current parent-transform scheme
> resolves its world position exactly ONE PARENT-BONE LENGTH further along the bone axis
> than a naive `bone.matrix @ local` computation predicts.**
>
> **Therefore the negative local Y values in the table above are INTENTIONAL and are the
> result of that compensation. They must NOT be "corrected", tidied, zeroed or treated as
> a faulty transform.**

Measured offsets (each equals the parent bone length exactly):

| Attachment | Parent bone | Bone length | Measured offset vs naive computation |
|---|---|---:|---:|
| `WeaponPoint_L` | `B-hand.L` | 146.4458 mm | 146.4459 mm |
| `WeaponPoint_R` | `B-hand.R` | 146.4458 mm | 146.4458 mm |
| `HolsterPoint` | `B-hips` | 83.7820 mm | 83.7821 mm |
| `BackWeaponPoint` | `B-chest` | 270.7316 mm | 270.7314 mm |

Rule for any future stage that re-derives an attachment offset:

```
local_offset = bone.matrix_local.inverted() @ target_world_position
local_offset.y -= (bone.tail_local - bone.head_local).length      # MANDATORY
```

Without the compensation every attachment is displaced by exactly one bone length
(e.g. the holster 83.8 mm too high, the back mount 270.7 mm too high).

Verified world positions (armature/world space, F4, accuracy ≤ 0.0007 mm):

| Object | world position |
|---|---|
| `WeaponPoint_L` | `(0.7141828, 0.0060580, 1.4221644)` |
| `WeaponPoint_R` | `(-0.7141829, 0.0060580, 1.4221647)` |
| `HolsterPoint` | `(-0.1606045, -0.0155893, 0.9906486)` |
| `BackWeaponPoint` | `(0.0788862, 0.0963143, 1.3972142)` |

Mirror residual of the weapon pair: **0.000316 mm** (X 0.000100, Y 0.000000, Z 0.000300).

---

## 11. HISTORICAL / NON-CANONICAL MATERIAL

The following are **HISTORICAL / NON-CANONICAL / NON-PRODUCTION** and must **not** be used as:

* a character foundation;
* a source of new bone lengths;
* a skinning / deformation authority;
* a means of restoring or "correcting" `WWG_Rig`.

| Material | Location |
|---|---|
| `Doomy_measurements.md` and related MD | quarantine archive + `WWG_Doomy_Canonical_Source` (marked non-canonical) |
| `WWG_Template_Armature`, `WWG_Foundation_Source*` | quarantine archive |
| former 51-bone rig / WWG_Player family | quarantine archive |
| `_ARCHIVE_DOOMY_PRE_RESET_2026-09-29` | `NON-PRODUCTION / DO NOT USE AS SOURCE` |

In a conflict, the **pristine vendor source** and this F2/F3 contract win.

---

## 11.5 MEASUREMENT QUALIFICATIONS

Added at the FINAL CHARACTER FOUNDATION VALIDATION stage, 2026-09-30. These are the only
measurement qualifications recorded in this contract. They do **not** alter the skeleton,
the attachment contract, or any F5/F6/F7 conclusion.

### 11.5.1 Shoulder Joint Span — NOT a garment shoulder width

| Item | Value |
|---|---|
| **Shoulder Joint Span** | **400.2625 mm** |
| Definition | `distance between B-upperArm.L and B-upperArm.R heads in exact rest` (armature space) |
| Status | **CANONICAL as a body landmark measurement** |

> **This is NOT a garment shoulder width.**
> 400.2625 mm is the skeletal acromion-to-acromion distance. **Do NOT use it as the shoulder
> width of a shirt, vest or coat panel** without a separate garment measurement; garment
> shoulder width also depends on seam placement, ease and target silhouette.

It is defined on the `B-upperArm` heads because `B-shoulder` is a clavicle-style bone whose
two heads sit on the neck midline (head span 0.0001 mm), which is not a usable width.

### 11.5.2 Toe Box Width — PROVISIONAL / LOW CONFIDENCE

| Item | Value |
|---|---|
| **Toe Box Width** | **86.4981 mm** |
| Status | **PROVISIONAL / LOW CONFIDENCE — NOT a canonical boot-last dimension** |

Derived from a sample of only **10 vertices** (within ±50 mm of `B-toe.L.tail.y`, left
foot). The sample is too small to size boot geometry against.

**Do NOT use 86.4981 mm as a final boot last / toe-box dimension.** A dedicated
higher-sample measurement is required before any boot is fitted to it.

Full dataset, methods and reproduction rules:
`Working\reports\new_character_foundation_f8_canonical_measurements_2026-09-30.md`
· validation: `Working\reports\new_character_foundation_final_validation_2026-09-30.md`

---

## 12. OPEN ITEMS
| Item | Status |
|---|---|
| Unity axis conversion | **NOT VERIFIED** — needs Blender → FBX → Unity test |
| `BackWeaponPoint` parent bone | **RESOLVED in F4** — `B-chest`, from measurement (323 vs 84) |
| Final attachment transforms | **RESOLVED in F4** — see §10, accuracy ≤ 0.0007 mm |
| Bone-parenting length compensation | **DOCUMENTED in F5** — see §10.1, mandatory |
| Body binding / skinning validation | **F5** — numeric + binding PASS; weights unchanged; see F5 report |
| Rest / skinning formal QA | **F6** — NUMERIC PASS, no blocking skinning defect |
| Pose & deformation QA | **F7** — NUMERIC PASS, no blocking deformation defect |
| Canonical measurements | **F8** — measurement data complete; see §11.5 qualifications |
| Final foundation validation | **F9** — technical pass; see F9 report |
| **Human approval** | **APPROVED 2026-09-30** — `PRODUCTION FOUNDATION / APPROVED`; see §14 |
| Rest-pose and deformation QA (F6/F7) | completed — F6 and F7 |
| Canonical measurements (F8) | completed — F8 |
| Equipment pipeline (E1 hat, clothing, weapons) | **not started** — requires separate explicit user approval |
| Torso inter-shell gaps under flexion | **OPEN (F6/F7)** — pre-existing Kevin dummy geometry, see F5 report §C.4 |

---

## 13. CHANGE CONTROL

Changes to this contract require explicit user approval.
Do not modify the skeleton, bone names, hierarchy or weights without such approval.

---

## 14. HUMAN APPROVAL

| Field | Value |
|---|---|
| Decision | **CHARACTER FOUNDATION APPROVED** |
| Date | **2026-09-30** |
| Decided by | project owner (human), after personal review of the F3–F9 results in Blender |
| Approved file | `Working\blender_src\character_foundation\WWG_Character_Foundation_Working.blend` |
| Approved SHA-256 | `B14CE5A151EFDE19026CAF6FD25A2368677B5B4A6E964288EABD7C91B9D256EF` |
| Armature | **`WWG_Rig`, 53 bones** (52 deform + 1 non-deform `B-root`) |
| Body | `HumanM_BodyMesh` — 2 754 verts / 2 756 polys, binding verified |
| Vendor source | `HumanCharacterDummy_M.blend` — `6D6DB2666CC6214DF7E71A955A5861A8E9A460C6310246E733694EABB9727D8E` |
| Status | **`PRODUCTION FOUNDATION / APPROVED`** |
| Final validation | `Working\reports\new_character_foundation_final_validation_2026-09-30.md` (TECHNICAL PASS) |
| Approval report | `Working\reports\character_foundation_final_human_approval_2026-09-30.md` |

**Scope:** approved as the production basis for the modular equipment pipeline
(shirt / vest / pants / boots / belt / holster / hat / coat). Body measurements and their
exact methods are the **F8 canonical dataset** (§11.5 qualifications apply).

**Not covered by this approval:** Unity integration (**NOT integrated / NOT VERIFIED**),
FBX export, and any clothing / weapon / equipment asset. **No equipment stage may begin
without separate explicit user approval** — next candidate stage is **E1 — HAT**.

---

## SCOPE AMENDMENT — 2026-10-07 (owner ARCHITECTURE DECISION `DEC-12`; identifier confirmed 2026-10-15)

> **This contract governs `WWG_Rig` ONLY.** It is the contract for the legacy 53-bone
> technical rig. It is **not** a project-wide character contract and **not** an
> exclusivity rule.
>
> **WWG permits multiple production character foundations:**
>
> | Foundation | Role | Governing contract |
> |---|---|---|
> | **`Bandit`** | primary production character foundation | own Humanoid rig, 118 bones; deformation repair validated 2026-10-07 (`Working/reports/bandit_deformation_repair_qa_2026-10-07.md`) |
> | **`Cowboy`** | second primary production character foundation | own Humanoid rig, 87 bones; readiness gate applies |
> | **`Doomy`** | **enemy foundation / variant basis** for enemy colour and visual variants | own rig, 51 bones `B-*` |
> | **`WWG_Rig`** | **legacy technical rig**, retained if still needed | **this contract** |
>
> **Architecture:** `multiple approved humanoid foundations -> shared gameplay/animation
> contract`. Animator, Humanoid Avatar, movement, combat animation, hit, death and weapon
> attachment remain **one shared gameplay system**. Rigs, Avatars and animation mapping are
> **per foundation**. There is no per-foundation `EnemyController` and no duplicated
> gameplay logic.
>
> **`WWG_Rig` is NOT a prerequisite for `Bandit` or `Cowboy`, and neither foundation is
> assumed to use it.** Nothing above this note changes the measurements, bone lengths,
> binding rules or attachment points of `WWG_Rig` itself; the historical record is intact.
