---
type: roadmap
status: active
created: "2026-10-15"
updated: "2026-10-15"
project: WildWestGunslinger
scope: Unity integration — environment and characters
---

# INTEGRATION ROADMAP — интеграция окружения и персонажей в Unity

> **This file is the persistent working document of the integration route.**
> It records the plan, current stage, decisions, blockers and the exact next step.
> Update it after every stage. Reports with evidence live in `../Working/reports/`.
>
> Authority order: Git canonical docs → current verifiable reports → actual
> files/scenes/prefabs/Unity → agent memory. **When documentation contradicts
> measured reality, the conflict is recorded here, not silently resolved.**

## 0. Role, scope and operating rules of this document

**Purpose.** This is the single persistent working document of the Unity
integration route: it sequences the remaining integration work and keeps stage
status, blockers, owner decisions and the exact next step recoverable across
sessions. It is a **route**, not a second copy of the documentation set.

**Position in the hierarchy.** `Documentation/README.md` outranks this file. So do
the profile contracts and standing rules it references — `AI_PRODUCTION_METHODOLOGY.md`
(**APPLY**), `Character/CHARACTER_FOUNDATION_CONTRACT.md`, `README.md` §9 standing
rules, `ARCHITECTURE.md`, the domain registers. This roadmap **never** overrides
them; it sequences work *inside* them and points at them. Where this file and a
canonical document disagree, the canonical document wins and the disagreement is
recorded here as a conflict, not resolved here.

**When an agent must read it.** Whenever the task touches Unity integration, the
environment kit, character integration, weapon attach points, or continues work
from an earlier session in this route. In those cases reading it is mandatory,
not optional.

**Continue, never restart.** On resuming: read §4 (stage register), §7 (open owner
decisions), §8 (blockers) and §10 (exact next step), then continue from the
recorded stage. Stages marked `PASS` are not re-run without a concrete technical
reason, and any such reason is written into §9 before the stage is repeated.

**Choosing the next safe stage.** After a stage closes: run its checks, compare
against its exit criteria, write the evidence report, update this file, then pick
the next stage that (a) depends on no unresolved owner decision and (b) is safe
within the granted permissions. Start it immediately — routine progression does
not wait for a new instruction. Stages that are `BLOCKED` or `NOT READY` are
skipped, not forced.

**Mandatory stop points.** Stop and ask the owner when the next step requires:
choosing between ambiguous asset revisions · authorising an export, import or
promotion · a new product or architecture decision · editing scenes, code,
prefabs or import settings beyond the approved scope · deleting, replacing or
overwriting data · any action whose consequences are not covered by these rules.
A blocked stage may still have its independent, safe checks executed first; only
the dependent part waits.

**Where evidence lives.** Detailed reports go to `Working/reports/`, using the
project's existing report conventions (dated filename, descriptive stem). This
file keeps only state, decisions and pointers. Working-tree changes are
preserved; Git checkpoint actions (add/commit/push) require separate owner
permission and are never implied by updating this file.

**Conflict resolution rule.** Measured reality outranks a stale canonical claim
for the purpose of *continuing work*, but the stale claim is **not** deleted — it
is listed in §6 with the measurement that contradicts it, and correcting the
canonical document is a separate, explicitly approved action.

## 1. Goal

Bring already-produced assets to a **verified Unity integration** without
repeating finished production work, ending with: a working game environment,
a confirmed enemy basis, an integrated player character, a verified weapon
system, and a documented project state.

**Completion criteria (all must hold before the goal may be declared met):**
1. Environment kit (floor, wall, barrel, fence) present in
   `Assets/Scenes/TestArena.unity` at verified scale, orientation and collision.
2. Enemy spawn confirmed in a real Unity **runtime** session of the canonical scene.
3. Player character selected by owner decision, integrated, and driving the game.
4. Weapon attach points verified and firing/reload confirmed in runtime.
5. Every asset carrying `RUNTIME VERIFIED` backed by a runtime observation.

`APPROVED` ≠ `PROMOTED`. Static audit ≠ runtime test. Asset readiness ≠ scene integration.

## 2. Working principles and constraints

- Never repeat a stage that passed without a proven technical reason.
- Never treat a filename, an existing prefab, or a sandbox test as proof.
- Never overwrite, delete, or restore user files on assumption.
- No `git add .`; no commit/push/pull/fetch/reset/clean/checkout without permission.
- Product assets, scenes, prefabs, code and import settings are frozen outside an
  explicitly approved stage.
- Stop only at report checkpoints, mandatory owner decisions, or proven blockers.
- Status vocabulary: `PASS` · `FAIL` · `UNVERIFIED` · `BLOCKED` · `NOT READY`.

## 3. Baseline

| Field | Value |
|---|---|
| Branch | `main` |
| HEAD | `12cd312860eed0f6ddc85c8f44752df0900d35a4` (`docs: consolidate canonical documentation and DEC-12`) |
| local `origin/main` ref | same SHA — **no `fetch` performed**, remote not independently verified |
| Last check | 2026-10-17 (D-16E) |
| Working tree | 464 entries · staged **0** · modified 76 · deleted 2 · untracked 386 (measured after the D-16E prefab/scene edits and before this stage's report was written; the report adds one more untracked entry) |
| User changes present | **YES** — includes uncommitted Phase D1 edit to `Assets/Settings/DefaultVolumeProfile.asset` (21 305 B, SHA-256 `4519C964F9FC48BA870BB7D22E22444F71468102CFC6B630D6F586471FDC72FE`) and untracked production file `Assets/Scripts/Enemies/BanditAnimatorDriver.cs` |
| Canonical scene | `Assets/Scenes/TestArena.unity` |
| Duplicate scene | `Assets/TestArena.unity` — **do not substitute for the canonical scene** |
| Unity | 6000.3.23f1; Unity MCP available (read-only use in read-only stages) |
| Unity version gate | Inspector checks that used `FindObjectsByType` see only the **active scene** (currently a sandbox QA scene), not `TestArena` |

## 4. Stage register

| Stage | Scope | Status | Evidence |
|---|---|---|---|
| **E1A** | FBX import scale + environment readiness to integrate | `PASS` | `Working/reports/E1A_import_scale_environment_readiness_2026-10-15.md` |
| **E1B** | Revision comparison, `PROMOTED` criteria check, exact scene change plan, working-tree classification | `PASS` | `Working/reports/E1B_revision_comparison_promotion_and_scene_plan_2026-10-15.md` |
| **E1C** | Documentary reconciliation of D-01/D-02/D-08/D-09; verify recommendations against the source reports | `PASS` | `Working/reports/E1C_decision_reconciliation_2026-10-15.md` |
| **E1D** | Read-only evidence pack for a future isolated evaluation of the modular kit (geometry/collision comparison vs the procedural arena) | `PASS` | `Working/reports/E1D_isolated_kit_evaluation_evidence_2026-10-15.md` |
| E1E | Adopt the D-08 promotion-target proposal; freeze the revision choices | `NOT READY` | blocked by D-02 owner choice and D-08 adoption |
| **E1E** | Decision lock (D-07, D-09…D-13), isolated sandbox scene with the two decided revisions, P-1 Git-compatibility check, exact checkpoint inventory | `PASS` (sandbox static QA); runtime QA `UNVERIFIED` | `Working/reports/E1E_decision_lock_sandbox_checkpoint_2026-10-15.md` |
| **E1F** | Decision closure (D-12, D-13, D-14, D-15), `.gitattributes` read-only diagnosis, P-1 compatibility re-check, D-17 texture investigation | `PASS` (documentary + diagnosis); sandbox fix `BLOCKED` | `Working/reports/E1F_decision_closure_git_diagnostics_texture_investigation_2026-10-15.md` |
| **E1G** | Blender source texture audit — establish where the wood look is stored in `WallSegment_01` and `FloorSegment_01` | `PASS` — both are `PROCEDURAL_MATERIAL` + `VERTEX_COLOR_ONLY` (`MIXED`), 0 images in either `.blend` | `Working/reports/E1G_blender_source_texture_audit_2026-10-15.md` |
| **E1H** | Bake plan: prerequisites, UV feasibility, texture set, resolution/padding, `ART-0003` compliance | `BLOCKED` — plan complete; execution needs an owner decision on the route and on `ART-0003` §4a.3 | `Working/reports/E1H_bake_plan_wall_floor_2026-10-15.md` |
| **E1I** | Sandbox vertex-colour material test — can the existing `Col` from the FBX give an acceptable look without bake or UV change | `PASS` (wall) · `PASS` (floor) — a sandbox-only URP shader reading `Col` with a measured sRGB decode yields a recognisable wood tone on both | `Working/reports/E1I_vertex_colour_material_test_2026-10-16.md` |
| E1J | Owner decision: accept route A, or continue toward route B (bake) | `NOT READY` | blocked by D-17 / D-18 |
| E1G | Owner decision on the D-17 presentation fix (new sandbox material) and on D-13's commit precondition | `NOT READY` | blocked by the two decisions below |
| **E2-VC** | Extended vertex-colour audit of the **whole** Modular Kit — barrel, cover, wall, floor plus every analogous asset | `PASS` — all 11 kit FBX (5 Blender sources) share one `Col` + procedural method with 0 image textures; route A extends with no exceptions. 8 legacy/third-party FBX `EXCLUDED` as `DIFFERENT_MATERIAL_METHOD` | `Working/reports/E2_modular_kit_vertex_colour_audit_2026-10-16.md` |
| **E2-VC-F** | Follow-up: D-21 (`BYTE_COLOR` origin/impact), D-24 (cover revision comparison), D-18 (Bake scope), D-16 (integration plan, not executed) | `PASS` — see report; D-21 mechanism proven, D-24 recommends the already-live `r5`, D-18 partially unblocked **for the verified kit only**, D-16 plan drafted. No production change | `Working/reports/E2_VC_followup_D21_D24_D18_D16_2026-10-16.md` |
| **E2-VCF** | Visual validation of the four destructible fragment sets — barrel, fence, crate, cover r5 — using the unmodified E2-V2 shader | `PASS` — all four sets `PASS` (**corrected 2026-10-16**: crate moved from `PARTIAL` after the formal diagnostic disproved the coplanarity hypothesis; its metal is 22 × 22 mm pyramidal rivets, base sunk 3 mm / apex proud 7 mm, rivet count matching rendered mark count). Historical `PARTIAL` claim preserved and marked `DISPROVED` in the report | `Working/reports/E2_VCF_fragment_visual_validation_2026-10-16.md` (§7.3, §13) + `Working/reports/E2_VCF_crate_coplanar_geometry_diagnostic_2026-10-16.md` |
| **D-16A** | Controlled production-material integration — production shader + 6 standalone `.mat` + assignment in `Assets/Scenes/TestArena.unity` | `PASS` — `WWG/VertexColorWoodMetal` compiles; 52 renderers across all 6 approved families assigned; visual check under the scene's own lighting; scene reopens cleanly; Play Mode without errors. Canonical scene turned out to be **fully procedural** (0 kit models), so an isolated, collider-free, **inactive** overview section `D16A_MaterialOverview` at y = −60 (58 m below the arena) was created inside the scene | `Working/reports/D16A_production_material_integration_2026-10-16.md` |
| **D-16A-RV** | Runtime Visual Verification — production shader + 6 `.mat` in Play Mode | `PASS` — section activated **at runtime only**, all six families and the metal details rendered correctly through **Scene View**; shader and materials load at runtime with no errors; scene restored exactly (section inactive, `isDirty = False`, SHA-256 unchanged). Camera-coverage and Android performance remain `UNVERIFIED` | `Working/reports/D16A_runtime_visual_validation_2026-10-16.md` |
| **D-16A-MF** | Material-fidelity investigation (sandbox): grain/rust do not survive FBX export | `PASS (sandbox)` — wood grain, rust, PBR metal, roughness and colour space traced to the six Blender graphs; R1/R2/R3 prototypes built; no production change during the investigation | `Working/reports/D16A_material_fidelity_wood_rust_2026-10-16.md`, `..._followup_R2_2026-10-16.md` |
| **D-16A-MF-R3** | Colour space, PBR metal, rust and grain contrast (sandbox) | `PASS` with one known limitation — **R3-A resolved the E1I vs R2 contradiction: there is NO colour-space discrepancy** (Blender linear → FBX linear → Unity stores sRGB → single `SRGBToLinear` recovers the authored value; measured on wall and barrel). Metal now uses URP PBR (`UniversalFragmentPBR`). Grain contrast tunable (`_GrainContrast = 0.6`). **MF-F2 remains `PARTIAL`**: rust is correctly transferred but is visually less prominent than the Blender reference because the source limits it to a 0.45 blend on roughness-0.90 metal. Android `UNVERIFIED` | `Working/reports/D16A_material_fidelity_followup_R3_2026-10-16.md` |
| **D-16B** | Production promotion of the verified R3 material implementation + prefab material assignment | `PASS` — production shader `WWG/VertexColorWoodMetal` replaced with the R3 implementation (GUID/path unchanged, compiles clean); six production `.mat` updated (GUIDs preserved, `textureRefs = 2`); materials assigned to existing `Barrel_01`, `Fence_01`, `Cover_01` (materials only — colliders, rigidbodies, `DestructibleObject` and hierarchy untouched); new minimal prefabs `WallSegment_01`, `FloorSegment_01`, `Crate_01`, `Crate_01_Fragments`; `Assets/Scenes/TestArena.unity` and `Assets/TestArena.unity` **bit-identical**; `D16A_MaterialOverview` intact and still inactive; Play Mode clean. **`MF-F2` remains `PARTIAL`** (rust prominence is limited by the source graph, measured in report §6) — promotion proceeded with that limitation explicitly recorded, not waived. Final audit (report §13): shader has **0 messages** of any severity, all six materials have **0 texture dependencies**, the metallic mask was verified on the production meshes (wall/floor exactly 0 metal vertices; barrel 504, fence 78, crate 96, cover 320 metal vertices), and **`_DielectricF0` was found inert** in URP's metallic workflow — zero visual impact, deliberately not changed because a real fix requires migrating to `_SPECULAR_SETUP`, which would invalidate the verified R3-A colour chain and needs owner approval | `Working/reports/D16B_production_prefab_material_integration_2026-10-16.md` |
| **D-16E** | Full remediation & integration pass — executes CH-1…CH-14 of the D-16D plan | `PARTIAL PASS` — **6 of 14 executed, 1 partial, 7 `BLOCKED`**, none `FAIL`. Executed: `CH-4`/`CH-5`/`CH-7`/`CH-8` colliders + wall Z-up rotation (wall now `4.000 × 2.000 × 0.200 m`, collider AABB matches renderer AABB exactly), `CH-10` filled six empty environment prefab arrays with the project's own `Default*PrefabPaths` so Editor and Player agree, `CH-12` assigned `fencePrefab`/`coverPrefab`/`cratePrefab`/`barrelPrefab` and set `useCoverClusters = true`, `CH-13` removed `DIAG_TEMP_White`/`DIAG_TEMP_Gray`/`QA_Capsule`, `CH-1` registered the seven kit prefabs in `ART-0002` **without assigning any status**, `CH-2` synced this baseline. `CH-14` kept the `D16A_MaterialOverview` section (least-risk, non-destructive). **`BLOCKED`:** `CH-3` (Git commit prohibited by the same brief), `CH-6` (crate fragments are CP2 scope, CP2 NOT AUTHORIZED), `CH-9` (**premise invalid** — `minimumWallHeight`/`minimumWallThickness` are used only by `BuildRoomGeometry()`/`CreateCorridor()` for procedural primitive walls; the kit wall is never placed by that path, so lowering them would shrink every arena wall while fixing nothing), `CH-11` (brief §3.E forbids changing the RP asset), `CH-14`-deletion option, plus `CH-7/8` wall *status* questions. No wall status assigned — D-14 intact. Android stays `UNVERIFIED`; `MF-F2` stays `PARTIAL`; `_DielectricF0` untouched | `Working/reports/D16E_full_remediation_implementation_2026-10-17.md` |
| E2 | Controlled integration of floor/wall/barrel/fence into canonical scene | **`NOT READY`** | materials and prefab assignments now exist (`D-16B`, `D-16E`), but **no runtime verification has been performed**: the scene was never entered in Play Mode with clusters enabled, so arena generation, A* obstacle handling, bullet blocking and enemy cover use remain unproven. D-16 (scene write) was exercised only for the bounded D-16E change list |
| E3 | Runtime verification of enemy spawn (Doomy) | `NOT READY` | depends on E2, but independently investigable |
| E4 | Integrate player character (Bandit) | `BLOCKED` | owner decision D-03 (`DEC-12` per-role foundation) |
| E5 | Weapon attach points and weapon behaviour | `BLOCKED` | depends on E4; prior repair report ended `STOP` |
| E6 | Cover (`DEC-06` + export authorisation) and Crate (`DEC-05`/CP2) | `BLOCKED` | owner decisions D-04, D-05 |
| E7 | Final integration verification across environment, player, enemies, weapon | `NOT READY` | depends on E2–E6 |

## 5. Stage E1A — result

**Status: `PASS`.** Criteria met; one blocker found and *removed* (not raised).

### 5.1 UNI-D06 — measured, not reproduced

Primary source: `Documentation/Engineering/UNI-0003-UNITY-KNOWN-DEFECTS.md` §`UNI-D06`
(published in `12cd312`). Recorded state: *"modular environment FBX import scale
resolves to 0.01×; Z-up is not compensated; State: unfixed; import settings are wrong;
source-side `.meta`/importer problem"*. It currently **blocks G7 entirely**
(`ART-0003` P6) and *"blocks all environment art"* (`ART-0002` §248).

Measured facts (read-only, Unity + `.meta` inspection):

| Fact | Measurement |
|---|---|
| `ModelImporter.globalScale` | `1` |
| `ModelImporter.useFileScale` / `fileScale` | `True` / `0.01` |
| `ModelImporter.bakeAxisConversion` | `False` |
| Modular vs known-good props | **identical importer settings** — modular FBX are not special |
| Mesh bounds in Unity units | wall `0.04 × 0.002 × 0.02`, floor `0.04 × 0.04 × 0.0008` |
| FBX root / child transform | `localRotation = (270, 0, 0)`, `localScale = (100, 100, 100)` |
| Effective world size | wall **4.0 × 0.2 × 2.0 m**, floor **4.0 × 4.0 × 0.08 m** |
| Blender source dimensions | wall `4.000 × 0.200 × 2.000 m`, floor `4.000 × 4.000 × 0.08 m` |

**Conclusion:** the 0.01× figure describes raw mesh data, which Blender's
centimetre export compensates with a ×100 child scale **and** a −90° X root
rotation. Both halves of the recorded defect (scale and Z-up) are compensated
in the current imported assets. The same compensation is already present in
`Barrel_01.prefab` and `Fence_01.prefab` (`localRot (270,0,0)`, scale 100,
`BoxCollider` per child sized to the mesh).

**Residual risk:** the finding is at asset/prefab level. A scene-level verification
(place an instance, measure world bounds) is still required before `UNI-D06` may be
formally closed — that is E1B/E2 work and needs scene-write permission.

### 5.2 Revision ambiguity (Wall / Floor) — owner decision required

| Asset | Revision | verts | Evidence | Note |
|---|---|---|---|---|
| Wall | `_FINAL` | 480 | `wallsegment_01_qa_20261004.md`: `PASS_NUMERIC` / `PASS_VISUAL` 7/7 | strongest automated + visual QA |
| Wall | `_r2` | 2156 | `ART-0002`: `QA PASS` | heaviest; r2 grain was built with geometry |
| Wall | `_r3` | 360 | `wood_language_correction_r3_final_qa.md`: "corrected candidate, awaits your visual sign-off, NOT promoted" | lightest, unapproved |
| Floor | `_FINAL` | 574 | `floorsegment_qa_20261004.md`: 5/5 PASS (4.000×4.000×0.08) | "awaiting user visual confirmation" |
| Floor | `_r2` | 3078 | `ART-0002`: `QA PASS` | heaviest |
| Floor | `_r3` | 572 | `wood_language_correction_r3_final_qa.md` | unapproved |

All six FBX share identical importer settings and the same scale/rotation
compensation. None is referenced by the canonical scene.

### 5.3 Environment readiness measured

| Asset | Unity prefab | Physics | Canonical scene |
|---|---|---|---|
| `Barrel_01` | `Assets/Prefabs/Environment/Barrel_01.prefab` exists | 13 `BoxCollider`, 12 `Rigidbody` (destructible), `isStatic=false` | **0 instances** |
| `Fence_01` | `Assets/Prefabs/Environment/Fence_01.prefab` exists | 14 `BoxCollider`, 13 `Rigidbody`, `isStatic=false` | **0 instances** |
| Wall / Floor | no prefab | none yet | **0 instances** |

`BARREL_01` and `FENCE_01` are `APPROVED` (user-confirmed) and are **ready to place**
without any re-export or re-production.

## 6. Documentation vs measured reality — recorded conflicts

| # | Canonical claim | Measured reality | Handling |
|---|---|---|---|
| C-1 | `UNI-0003` `UNI-D06`: import `0.01×`, Z-up uncompensated, "source-side `.meta` problem" | importer settings identical to working props; scale ×100 and −90° X rotation applied by export | Conflict recorded; formal close deferred to a scene-level check (E1B/E2) |
| C-2 | `ART-0003` P6 "blocks G7 entirely"; `ART-0002` "blocks all environment art" | gate precondition appears already satisfied | Do not re-block E1B on this without re-verification |
| C-3 | `PROJECT_STATE.md`: "Broken references — 12, unidentified" | Phase C measured 4 unresolved asset refs in gameplay assets, 0 missing Mono Script in scenes/prefabs | Numbers stale |
| C-4 | `MASTER_PLAN.md` C1/C2/C3 and `DOC-0004` Q9/Q10/Q11 | contradicted by Phase C measurements | Stale; must not be used as gate input |
| C-5 | `PROJECT_STATE.md` Git row: `e631b5d`, 344 untracked | HEAD `12cd312`; 343 untracked | Stale dated snapshot |
| C-6 | `wallsegment_01_qa_20261004.md` header: "Verdict: `PASS_NUMERIC` / `PASS_VISUAL` (7/7 covered)" | same file's own verdict block: "`PASS_NUMERIC` + **`VISUAL QA = PARTIAL`**", and "`PASS / FINAL` … **не подтверждён** этим прогоном"; Asset State stays `CREATED` | Internal contradiction inside one report. Wall has **no** clean visual signature. Recommendation for `_FINAL` upheld, reason corrected |
| C-7 | E1B recommendation to use `FloorSegment_01_FINAL.fbx` | `floorsegment_qa_20261004.md` L175–L190 records a **user decision of 2026-10-04** binding the export to `FloorSegment_01_r3.fbx` and marking `_FINAL`/`r2` **HISTORICAL**; `_FINAL` is geometrically identical to `r3` | E1B recommendation **withdrawn**; the floor choice is `r3` unless the owner overrides their own recorded decision |
| C-8 | `wood_language_correction_r3_final_qa.md` binds Wall r3 to blend `dc8778a606679770` | current `WallSegment_01_Working.blend` hashes to `1D125174918DC4C5` — **the wall source changed after r3 was exported** | Wall `_r3` is stale against its source; it cannot be treated as current |
| C-9 | E0/E1A reported `ArenaModule_Start` 14 + `ArenaModule_Straight` 13 instances in the canonical scene | The scene has exactly **3 `PrefabInstance` blocks**, all `ArenaModule_Straight`; the extra GUID matches were `m_Modifications` entries inside them | E1D corrected the count. "Would double the serialised wall/floor geometry" no longer holds — the overlap risk is against **runtime-generated** geometry and is `UNVERIFIED` |
| C-10 | E1C proposal P-1 for the promotion target | cannot be adopted by the agent (D-08) | awaiting owner decision D-08 |
| C-11 | **D-17 — "no textures" on wall and floor in the sandbox.** Measured cause: **texture was never authored**. `ART-0003` §4a.3 L132 — "Image textures **prohibited** at the current stage. M2 = vertex attributes + simple PBR"; wall QA L103/L238 records "Image textures = 0 — PASS"; floor QA L120 "none". Both meshes carry **UV + vertex colours**; materials are **embedded in the FBX** (`materialLocation=InPrefab`), shader **URP/Lit** (compatible), **0 bound textures**, `_BaseColor = 0.906` grey | The wood tone lives in vertex colour (RGB ≈ 0.28–0.31, **alpha = 0 everywhere** = pure wood, metal mask 0). URP/Lit does not consume vertex colour by default, so the mesh renders near-white. **Nothing was lost** | Cause proven; a fix needs a **new material**, which D-17 forbids → sandbox fix `BLOCKED`, owner decision needed |
| C-12 | **E1G closes the D-17 cause at source level.** Both source `.blend` contain **0 image datablocks** and **0 `ShaderNodeTexImage`** nodes; the pattern is `Noise → ColorRamp` procedural mix + the **`Col`** attribute (`FLOAT_COLOR`/CORNER) + the canonical constant colours of `ART-0003` §4a.2 (iron `0.0785/0.0794/0.0830`, oak `0.115/0.062/0.038`) | Viewport captures of both `.blend` **do show** the pattern (log courses on the wall, plank seams on the floor), so the source is visually complete; Unity loses the **node graph**, while the `Col` data itself **does arrive** in the FBX | Sandbox may be made representative either by (A) a vertex-colour material or (B) a material Bake — **both require owner permission** and are recorded in the E1G report §5 |

## 7. Owner decisions register

### 7.1 Decisions received (chronological — not back-dated)

| ID | Decision | Date received | Basis | Scope of the authorisation | Evidence | Remaining conditions |
|---|---|---|---|---|---|---|
| **D-01** | `WallSegment_01`: recommendation is **`WallSegment_01_FINAL.fbx`**. Verify reports and exact paths before recording; do not hide conflicting evidence. | 2026-10-15 (E1C request) | completed visual QA exists for this revision, whereas `r3` waits for a visual signature | Recommendation to be **verified**, then recorded as the working choice | Verified in E1C §3.1 — **recommendation upheld with two corrections** (the QA verdict is `VISUAL QA = PARTIAL`, and `r3` is stale against the current `.blend`) | Owner confirms; a wall visual signature is still missing |
| **D-02** | `FloorSegment_01`: recommendation is **`FloorSegment_01_FINAL.fbx`** by the same reasoning. Verify the source reports; do not pass a measurement check off as visual QA. | 2026-10-15 (E1C request) | as D-01 | Recommendation to be **verified**, then recorded | Verified in E1C §3.2 — **CONFLICT: recommendation contradicted by a recorded user decision of 2026-10-04 that binds `FloorSegment_01_r3.fbx` and marks `_FINAL`/`r2` HISTORICAL** | Owner must choose between the recorded decision (`_r3`) and the stated preference (`_FINAL`); the two differ by metadata only |
| **D-06** | `BARREL_01` / `FENCE_01` may become `PROMOTED` only after every project promotion criterion is checked and evidenced. The authorisation excludes placing objects, editing a scene, or changing unrelated files. Do not declare `PROMOTED` if criteria are not met. | 2026-10-15 (E1B request) | `README.md` §9.2 — promotion is a human act | Status recording **only** | Criteria check in E1B §4 — **not verifiable** | Blocker B-7 |
| **D-07** | Classify all working-tree changes and propose a safe checkpoint plan. Nothing committed, indexed, or Git-state-changing without separate permission. | 2026-10-15 (E1B request) | a dirty tree makes future scene diffs unverifiable | Classification and recommendation **only** | E1B §6 — 423 entries classified, 7 proposed groups | Owner approves a grouping |
| **D-08** | Do **not** declare `PROMOTED` yet. Investigate the canonical promotion rules and find an admissible definition of *promotion target* and *promotion record*. Do not create a folder or introduce a new standard on your own. If a new project rule is required, prepare a concrete option for approval. | 2026-10-15 (E1C request) | gap B-7 — G6 cannot be executed | Investigation and a **proposal only**; asset status unchanged | Investigation in E1C §4 — proposal **P-1** prepared, not adopted | Owner adopts or amends the proposal |
| **D-09** | Isolate the modular kit from the procedural arena as a **separate verification candidat
| **D-07** | Prepare an exact file list and dependency map for future checkpoint groups. **No Git operation is to be performed.** | 2026-10-15 (E1E request) | a dirty tree makes every later diff unverifiable | Inventory and recommendation **only** | Exact grouping with paths, tracked state, dependencies, protected sources and risks — E1E report §8 | Owner approves specific groups |
| **D-09** | Evidence conflict: the **2026-10-04 decision is authoritative for the floor**. For the **wall**, check the primary evidence separately and record the result; do not substitute an assumption for missing evidence. | 2026-10-15 (E1E request) | two `r3` records conflict | Resolving evidence precedence only | Verified in E1E §3 — floor `r3` authoritative; **wall: no recorded signature exists** (absence of evidence, not evidence of absence) | Wall handling → new decision **D-14** |
| **D-10** | A **separate isolated sandbox scene** with the selected wall and floor plus the necessary colliders is authorised | 2026-10-15 (E1E request) | a visual/physical check needs an isolated scene, and the canonical scene must stay untouched | Sandbox scene + sandbox-only colliders and objects. **No** change to FBX, `.meta`, production prefabs or project settings | Created `Assets/_Sandbox/ModularKit_2026-10-15/ModularKit_Isolated.unity`; verified in E1E §4–6 | Sandbox placement is **not** promotion and **not** production integration |
| **D-11** | **Wall:** `WallSegment_01_FINAL.fbx` is the selected candidate. Visual QA is **partial** and must not be declared complete | 2026-10-15 (E1E request) | revision selection had to rest on evidence, not preference | Revision selection only | Recorded; the QA report's own verdict remains `VISUAL QA = PARTIAL` | D-14 |
| **D-12** | **Floor:** `FloorSegment_01_r3.fbx` selected, per the owner decision of 2026-10-04. Keep conflict C-7 and check the wall separately | 2026-10-15 (E1E request) | honours a recorded owner decision over a later preference | Revision selection only | Recorded; `_FINAL` remains marked HISTORICAL | C-7 stays open as a documentation conflict |
| **D-13** | Adopt promotion standard P-1 **only after** checking compatibility with the existing Git rules. Do **not** perform promotion and do **not** commit assets | 2026-10-15 (E1E request) | D-08 produced a proposal, not a standard | Compatibility check **only** | Verified in E1E §5 — **compatible on Git rules**, blocked on one precondition | Precondition + `.gitattributes` anomaly → **D-13**, **D-15** |
| **D-12** | **Floor confirmed:** keep `FloorSegment_01_r3.fbx` per the owner decision of **2026-10-04**. Keep conflict C-7 if the primary evidence cannot close it; do not substitute a dated owner decision with an assumption based on a filename | 2026-10-15 (E1F request) | C-7 remains open between two `r3` records | Revision selection only; **unchanged** from E1E | `floorsegment_qa_20261004.md` L175–L190 is explicit and asset-specific | C-7 stays open as a documentation conflict |
| **D-14** | **Wall confirmed:** `WallSegment_01_FINAL.fbx` is the working candidate. Visual QA stays **`PARTIAL`**, no owner visual signature exists. Never assign `VISUAL QA PASS`, `PROMOTED`, `INTEGRATED` or `RUNTIME VERIFIED` to the wall, and never treat the missing signature as evidence of rejection | 2026-10-15 (E1F request) | E1E §3.1 found no dated owner decision for the wall | Revision selection **and wording restrictions** only | `wallsegment_01_qa_20261004.md` verdict block + a full scan of `Working/reports/**` | none — the decision is closed with its caveat attached |
| **D-15** | `.gitattributes`: **read-only diagnosis only — editing is forbidden.** Perform the §4 checks and record the result | 2026-10-15 (E1F request) | the file shows ` M` with an empty textual diff | Diagnosis **only** | Cause established in E1F §4 — stale index stat entry, benign | Refreshing the index entry needs a Git write → **owner permission required** |
| **D-13 (closed)** | **Promotion standard P-1 accepted by the owner**, subject to Git-compatibility verification: target `Assets/Art/<Category>/…`; record in `ART-0002` with date, path, author, QA-report reference and SHA-256; promotion confirmed only when the file at the target path matches the recorded SHA-256; promotion and Git operations need separate permission | 2026-10-15 (E1F request) | owner adoption of the E1C proposal | Standard definition **only** — **no promotion executed in E1F**; `BARREL_01`/`FENCE_01` stay **`APPROVED`** | Compatibility re-verified in E1F §5 — **no conflict**; conditions met except the commit precondition | Target files are still untracked → commit is a D-07 decision |
| **D-17** | **Missing textures on wall and floor.** Determine at which stage the link is broken. Do not assume textures were lost, that materials are missing, or that URP is the cause | 2026-10-15 (E1F request) | owner observation after placing the objects in the sandbox | Diagnosis **and** a sandbox-only fix **only** if the cause is proven, the fix stays inside the sandbox, needs no new material/texture/re-export/import-setting change, and is reversible | **Cause established in E1F §3** and **completed in E1G**: both `.blend` contain **0 images** and **0 Image Texture nodes**; the pattern is **procedural Noise+ColorRamp mixed with the `Col` vertex colour** (`PROCEDURAL_MATERIAL` + `VERTEX_COLOR_ONLY` = `MIXED`) | Sandbox fix **not applied**: it would require a new material, which is explicitly out of scope → owner must choose route A or B, see E1G §5 |e**. This is permission to *prepare a plan*, not to create or modify a scene. Do **not** place modules in `Assets/Scenes/TestArena.unity` and do **not** replace the existing procedural arena. | 2026-10-15 (E1C request) | duplication risk: the scene already generates 9 rooms / 71 walls / 11 floors | Planning and read-only evidence **only**; **no scene write of any kind** | Comparison method in E1C §5; evidence pack prepared in `E1D` | E2 remains blocked until the owner separately authorises any scene write |

### 7.2 Decisions still open

| ID | Question | Options | Consequence |
|---|---|---|---|
| **D-01** | **CLOSED by D-11** — wall revision resolved to `WallSegment_01_FINAL.fbx` | — | — |
| **D-02** | **CLOSED by D-12** — floor revision resolved to `FloorSegment_01_r3.fbx` | — | — |
| **D-08 / D-13** | **CLOSED** — P-1 adopted by the owner and verified compatible with the current Git rules in E1F §5. Promotion stays unauthorised and unexecuted | — | see `E1F` report §5 |
| **D-14** | **CLOSED** — wall is the working candidate with visual QA recorded as `PARTIAL`; no signature exists and none is implied | — | wording restriction stays in force permanently |
| **D-15** | **CAUSE ESTABLISHED, ACTION `BLOCKED`** — the `.gitattributes` ` M` is a **stale index stat entry** (index size 1116 vs actual 1109 = the old CRLF version), not a content change. Refreshing it requires `git update-index --refresh` / `git add`, which are forbidden without permission | a) authorise the index refresh · b) leave as-is | any future commit touches a file git believes is modified |
| **D-17** | Sandbox presentation fix (wall and floor render near-white). Needs a **new material** that consumes vertex colour — forbidden by the task scope | a) authorise a sandbox-only material · b) accept the current look and record it as expected at M2 | until decided, the sandbox cannot show the wood tone |
| **D-03** | Player foundation under `DEC-12` (Bandit vs Cowboy) | a) Bandit · b) Cowboy — blocked on licence proof (`ART-0004`, `ISSUE-27` weapon attach FAIL) | unblocks E4; **cannot be inferred** |
| **D-04** | `DEC-06` Cover revision status + export authorisation | a) defer Cover from base environment · b) authorise r5 export and integrate | unblocks E6 |
| **D-05** | `DEC-05` / Crate CP2 authorisation | a) keep CP2 unauthorised · b) authorise CP2 | unblocks E6 |
| **D-07** | Approve the checkpoint grouping in the E1E report §8 (7 groups) and authorise a specific Git checkpoint | a) approve group by group · b) amend · c) withhold | no commit may happen until answered |
| **D-16** | Permission to write to `Assets/Scenes/TestArena.unity` for `E2` | a) grant a bounded scene change list · b) withhold | `E2` cannot start |
| **D-16A** | Bounded scene-write authorisation actually granted on 2026-10-16 for **materials only** | **Executed** — production shader + 6 `.mat` + assignment in `TestArena.unity`. Does **not** cover prefab assets or gameplay wiring, so **D-16 stays open** | see D-16A report; owner decisions still required (§12 of that report) |

### 7.3 Dated records added by E2-VC-F (history preserved)

**2026-10-16 — D-18, partial lift (from E2-VC-F §5).** For the verified Modular
Kit only — **11 FBX from 5 Blender sources** — Bake is **no longer practically
required**: route A (vertex colour + `Col.a` as metal mask, no image textures)
is confirmed by measurement and by visual check of the intact meshes. The
historical `BLOCKED` records (E1H, `ART-0003` §4a.3, unusable UV) **remain in
force** for every other asset. Fragments were verified numerically but **not
visually**. Characters, weapons, vegetation, rocks and ground patches were not
examined. **No project-wide lift is granted**; `GD-07` still applies.
Decision itself stays with the owner.

**2026-10-16 — D-21 mechanism proven, not fixed (E2-VC-F §3).**
`Barrel_01_Working.blend` stores `Col` as `BYTE_COLOR`; the FBX exporter
applies linear→sRGB, so Unity is currently self-consistent and the ~12×
darkening does not appear there, while Blender's own shader path decodes the
same bytes as sRGB. The violation is **not** remediated — the source was not
modified. Interim measure: freeze the FBX as reference and run the pre-export
colour check defined in E2-VC-F §3.7. A fix is a separate owner-authorised task
because it changes the barrel's shipped tone.

**2026-10-16 — D-24 evidence (E2-VC-F §4).** Recommended cover revision is
`Cover_Western_Wooden_r5.fbx` + `Cover_Western_Wooden_Fragments_r5.fbx`, which
is **already the live revision** via `Cover_01.prefab`, so no prefab change is
required. `Cover_Western_Wooden_FINAL.fbx` is an orphaned revision: no prefab
references it, no matching fragments set exists, and it is absent from
`ART-0002`. `Fragments_r2` has degenerate colour (R min = 0.000) and
`Fragments_r3` is an empty export. The name `FINAL` is not evidence
(`OPEN_DECISIONS` DEC-04). Owner decision still pending under D-04.

**2026-10-16 — E2-VC measurement erratum (E2-VC-F §0).** The E2-VC fragment
rows for barrel, fence and cover fragments were measured on only the first
top-level FBX object and are corrected in the E2-VC report §8. Consequence:
fragments **do** contain metal (`A` up to 1.0), so the metal-mask requirement
applies to all 11 FBX, not only the intact ones. The overall E2-VC verdict is
unaffected.

**2026-10-16 — E2-VCF fragment visual validation (report §0, §7)
— SUPERSEDED IN PART, see the `SUPERSEDED` note immediately below; the crate
verdict in this entry is `DISPROVED` and the overall stage status is `PASS`.**
All 46
renderers across the four fragment FBX were inspected; every Mesh has vertex
colour, exactly one material slot, no image textures, and **zero mixed
triangles**, so the `Col.a` mask is clean and per-triangle. Reopening the
sandbox scene preserved all 46 renderers, Meshes, submeshes and material
assignments (`PASS`). Barrel, fence and cover r5 are `PASS`. Crate is
`PARTIAL`: on `Frag_09_Lid` and `Frag_05_Wall_Front` the metal triangles are
coplanar with the wood faces, so the metal area does not read visually — a
**source-geometry** defect that the shader cannot fix. Measured and
reproducible across scene reload. Additionally, cover strap vertices are
marked metal (`a = 1`) but carry wood-coloured RGB, which E2-V2 masks by
overriding metal albedo; the underlying data is inconsistent with
`ART-0003` §4a.2. E2-V2 was used **unmodified**; no shader copy was created.
This entry registers results only — **D-16, D-18, D-21 and D-24 statuses are
unchanged**, and no owner decision is implied.

> **SUPERSEDED 2026-10-16.** The crate `PARTIAL` verdict above is
> **DISPROVED** by `Working/reports/E2_VCF_crate_coplanar_geometry_diagnostic_2026-10-16.md`
> (read-only). There are **zero** parallel metal/wood plane pairs in
> `Frag_09_Lid`, `Frag_05_Wall_Front` or `Fence_01_Frag_06_Board_1`; the metal
> is 22 × 22 mm pyramidal rivets, 10 mm tall, base sunk 3 mm into the wood and
> apex proud 7 mm, and rivet counts match the rendered mark counts (2↔2,
> 4↔4, 2↔2). The original `PARTIAL` claim rested on comparing the *span of
> metal triangle centroids* with the wood face extent, which spans the gap
> between the two rivets, not a single rivet. **E2-VCF is `PASS` for all four
> sets** and no geometry fix is required. The text above is retained as
> history. The cover wood-coloured metal RGB observation above **still stands**.

**2026-10-16 — D-16A, controlled production-material integration (report §13).**
`WWG/VertexColorWoodMetal` was created as a production shader (derived from
the verified E2-V2, RGB decoded exactly once, alpha never gamma-decoded and
used as the metal mask per `ART-0003` §4a.1, canonical dark-old-iron metal
`(0.0785, 0.0794, 0.0830)` per §4a.2), plus six standalone materials in the
new `Assets/Materials/ModularKit/` folder. All six approved families were
assigned in `Assets/Scenes/TestArena.unity` (52 renderers, 0 empty slots).
**Scope that matters:** the canonical scene was found to be **fully
procedural** and contained **zero** Modular Kit models, so the assignment
lives in an isolated, collider-free, **inactive** overview section 58 m below
the arena. Consequence: materials are integrated and verified, but **not**
wired into gameplay. **Prefab assets and gameplay generators are unchanged,
so D-16 is not closed and E2 stays `NOT READY`.** Runtime visual evidence for
the kit materials is `UNVERIFIED` by design (the section is inactive). Only
`Assets/Scenes/TestArena.unity` changed among protected files; HEAD unchanged;
staged count 0. No Git write operations were performed. D-18, D-21 and D-24
statuses are unchanged.

**2026-10-16 — D-18, scope fixed for the verified Modular Kit (E2-VCF §13.3).**
Route A (vertex colour + `Col.a` as metal mask, no image textures) is confirmed
for the checked assets and **does not require Bake** to obtain the wood tone and
the metal mask: 11 kit FBX from 5 Blender sources (E2-VC) plus all 46 fragment
renderers across the four destructible sets (E2-VCF), with zero mixed triangles
and zero parallel metal/wood plane pairs. **Scope limits:** the decision covers
only the verified set; it does **not** lift `ART-0003` §4a.3 (image textures)
for any other asset; it does **not** authorise changes to production materials,
sources or prefabs; it does **not** mean the project is production-ready;
historical `BLOCKED` records (E1H, `ART-0003` §4a.3, unusable UV) remain in
force for everything else; `GD-07` still applies. Characters, weapons,
vegetation, rocks and ground patches were never checked, and the Blender
procedural grain is still not reproduced — that is route A's known ceiling, not
a defect. **No owner decision is implied by this entry.**

## 8. Blockers and removal conditions

| Blocker | Blocks | Removed when |
|---|---|---|
| B-1 revision ambiguity D-01/D-02 | E1B, E2 (wall/floor) | owner names the revision |
| B-2 scene-write permission not granted | E2 | explicit permission for a bounded scene change list |
| B-3 `UNI-D06` formal close requires a scene-level measurement | `PASS` for `UNI-D06` | one instance placed and measured (needs B-2) |
| B-4 `DEC-12` player foundation undecided | E4, E5 | owner decision D-03 |
| B-5 licence proof for Cowboy absent | E5 (if Cowboy chosen) | owner supplies proof of purchase |
| B-6 weapon repair report ended `STOP` | E5 | `Working/reports/cowboy_weapon_repair_2026-10-08.md` §6 blocker resolved |
| B-7 **promotion target undefined** — `DOC-0001` §4 requires `PROMOTED` = "copied into the promotion target", but no canonical document names a target path and no such folder exists in `Assets/` | `PROMOTED` for `BARREL_01`/`FENCE_01` (D-06 stays blocked) | owner decision D-08 |

### 8.1 Android runtime defects reported by the owner (QA-ANDROID-2026-10-18)

Источник — личная установка и проверка APK владельцем на Android 2026-10-18.
Фиксированы без искажения. Полный текст наблюдений — `Working/reports/QA_ANDROID_RUNTIME_DEFECTS_2026-10-18.md`;
Obsidian — `00-Inbox/QA_ANDROID_RUNTIME_DEFECTS_2026-10-18.md` (доступен).

APK проверки: `Builds/Android/WildWestGunslinger.apk`, 498 433 234 B,
SHA-256 `8FAECE444FF798C2E8D94BFD3A5E0FD90135680D2DC912B5578AF9C07082CADC`
(`Working/reports/Android_apk_build_burst_temp_2026-10-18.md`).

| ID | Наблюдение владельца | Статус (2026-10-18, вторая сессия) |
|---|---|---|
| **QA-A** | Стрельба в бегу: переход в стойку стрельбы, скольжение вместо движения, скорость не замедляется ожидаемым образом, отдача не заметна | **`FIXED`** (Editor verified) — `Aim → Walk`/`Run` с гейтом `Speed`; Android-прогон `UNVERIFIED` |
| **QA-B** | Отсутствует работающее управление AIM справа; стрельба есть, но прицеливаться нельзя; пули на неправильной высоте, вертикаль не работает | **`FIXED`** (Editor verified) — видимый AIM-джойстик создан, вертикаль проходит; высота точки выстрела `BLOCKED` (`D24` §10.1) |
| **QA-C** | Doomy не интегрирован в ожидаемом игровом виде; в предыдущих версиях были повреждённые текстуры | **`BLOCKED`** — без изменений (`D23` §3, B1–B5); безопасная диагностика выполнена |
| **QA-D** | Красный квадрат на основном игровом персонаже; причина неизвестна | **`FIXED`** (Editor verified) — маркер скрыт без прицела, при прицеле кольцевой ретикл в точке прицела |

**Связь с существующей документацией (не новые наблюдения):**

- **QA-A** совпадает с `D21` §6.2: у `Aim` нет переходов в `Walk`/`Run` — скольжение при
  захваченной цели зафиксировано ранее и **не исправлялось**. Отдача подтверждена в Editor
  (`D21` §4.2), но не на Android — требуется проверка.
- **QA-B** частично пересекается с `D24` §3.1 (высота траектории 1.500 м при укрытиях ≤ 1.210 м,
  статус `BLOCKED`) и с `D25` (ручное прицеливание реализовано, но Android не проверялся,
  видимого UI джойстика нет — только маркер-точка).
- **QA-C** совпадает с `D23` §2.3: игра спавнит старый `Doomy.fbx` (`minY = −500`), болванка
  `WWG_Doomy_Foundation.fbx` не используется; интеграция `BLOCKED` по 5 барьерам.
- **QA-D** — первичная гипотеза: UI-маркер `AimCrosshair` (красный квадрат 34×34) из `D25` §3.3,
  проецируемый через `Camera.WorldToScreenPoint`. **Требует проверки до исправления.**

**Общий гейт (обновлён 2026-10-19, шестая сессия):** устранена причина «вялости»
AIM — `ManualAimControls` (−200) выполнялся раньше слоя ввода и интегрировал углы
из значения стика **предыдущего кадра**. Зафиксирован порядок
ввод (−300) → интеграция (−100) → камера (+100). Aim Assist разделён на два
независимых механизма по материалу Insomniac (friction / tracking); полный
захват цели не применялся, так как в исследовании он отмечен как слишком
заметный. Проверено: при нейтральном стике и максимальной помощи `aimYaw` =
0.0000 — перехвата управления нет; переключение между целями исключено
гистерезисом. Добавлена левая FIRE-кнопка при AIM (298, 729), не пересекающаяся
с зоной MOVE (круг начинается с 625 при верхней границе зоны 594); проверено
одновременное MOVE + AIM + FIRE с разными touch ID. Реализована Auto-Fire on
Target как отдельная механика с попаданием луча и допуском 0.22 с;
**по умолчанию выключена**, так как `D25` §246 и `U-06` определяют иной
контракт — конфликт вынесен в отчёт для решения владельца. При отдаче найден
дефект накопления: 310 выстрелов уводили взгляд на 12.9° без возврата; отдача
переведена во временный угол, 30 выстрелов подряд дают насыщение 1.0° при
`aimPitch` = 0.000. Отчёт:
`Working/reports/QA_ANDROID_AIM_FEEL_ASSIST_AUTOFIRE_2026-10-19.md`.

**Общий гейт (обновлён 2026-10-19, пятая сессия):** бег со стрельбой устранён
структурно. Инвентаризация всех 43 клипов Quaternius UAL1 показала отсутствие
клипа бега со стрельбой; риг `BanditAvatar` гуманоидный, поэтому применён
разрешённый заданием путь — второй слой Animator `UpperBody` с новой маской
`WWG_UpperBodyMask` (таз и ноги исключены). Базовый слой теперь несёт только
локомоцию, верхний — прицел, стрельбу и перезарядку. Замеры: при удержании
FIRE базовый слой остаётся в `Run` 90 % времени, ход ног 3897 мм, отдача руки
сохранена (802 мм). Сокращение `Fire` проблему не решало и более не применяется
как средство. AIM: устранён жёсткий срез мёртвой зоны (16.81 °/с скачком за
кадр) в пользу непрерывной монотонной кривой; добавлены регулируемые
`GameSettings.AimSensitivity` и отключаемый `AimAssistStrength`. Камера:
переключение привязано к удержанию пальца, а не к отклонению стика — устранены
самопроизвольные переходы в изометрию. Статусы: P0 `PASS`, P1 `PASS`, P2 `PASS`
(Editor), Android `UNVERIFIED`. Отчёт:
`Working/reports/QA_ANDROID_RUN_AIM_CAMERA_2026-10-19.md`.

**Общий гейт (обновлён 2026-10-19, четвёртая сессия):** повторный тест владельца на
устройстве подтвердил, что AIM остаётся неуправляемым. Устранена корневая причина:
правый джойстик вращал луч из камеры, но не саму камеру и не персонажа. Ориентация
взгляда перенесена в `ManualAimControls` как единственный источник; камера в AIM
орбитирует пивот по этой же ориентации, персонаж поворачивается по ней же, прицел
фиксирован в центре, луч прицеливания = ось камеры. Дополнительно устранено попадание
луча в коллайдер игрока (ломало ручной AIM) и конкуренция авто-фасинга с ручным AIM.
Измерения: `angle(камера, AIM)` = 0.000°, yaw камеры == yaw персонажа, скорость
69.96 °/с при 70.00 ожидаемых, накопленный yaw 1813° (круг не ограничен). QA-2A,
QA-2B, QA-2C — **`PASS` (Editor)**, Android `UNVERIFIED`. QA-1 — **`PARTIAL`**:
локомоция 33 % → 53 % при сохранённой отдаче; полное устранение невозможно в одном
слое (цикл шага 0.667 с против интервала 0.4 с) и требует решения владельца.
Новый отчёт: `Working/reports/QA_ANDROID_AIM_CAMERA_SHOOTING_FIX2_2026-10-19.md`.

**Общий гейт (обновлён 2026-10-19, третья сессия):** повторный Android-тест владельца
выявил, что QA-A и QA-B **не устранены** на устройстве. Установлена причина QA-A:
состояние `Fire` удерживалось дольше интервала между выстрелами (клип 0.633 с при
`fireRate` 0.4 с), поэтому ноги не анимировались — скольжение. Переходы `Fire`
перестроены с приоритетом локомоции; локомоция 11 % → 33 %. Причина QA-B: канвас
выбирался через `FindFirstObjectByType<Canvas>()` и оказывался WorldSpace-канвасом
полоски здоровья; AIM захватывал весь экран; `yawOffset` не ограничивался.
Реализованы OTS-переход камеры, AIM только справа, видимая кнопка стрельбы,
ограничение и recenter прицела. Оба дефекта — **`PARTIAL`** (Editor verified),
Android `UNVERIFIED`. Отчёт:
`Working/reports/QA_ANDROID_AIM_CAMERA_SHOOTING_2026-10-19.md`.

**Общий гейт (2026-10-18, вторая сессия):** QA-A, QA-B (AIM-джойстик + вертикаль) и QA-D
исправлены и проверены в Editor Play Mode; QA-C остаётся `BLOCKED` (решение владельца, `D23` §3).
Высота точки выстрела (`D24` §10.1) — `BLOCKED`. Новый APK собран и проверен как артефакт;
установка на устройство не выполнялась (`adb devices` пуст) — Android runtime `UNVERIFIED`.
Отчёты: `Working/reports/QA_ANDROID_FIXES_2026-10-18.md` (+ evidence).

## 9. Checks already performed — do not repeat without cause

- Phase C defect inventory (read-only): 4 unresolved asset refs, 0 missing Mono
  Script in scenes/prefabs, 5 missing scripts removed from `DefaultVolumeProfile` (D1).
- Phase D1 edit of `Assets/Settings/DefaultVolumeProfile.asset` — verified, uncommitted.
- Bandit animator QA (Aim/Fire/Reload/Hit/Dead) — passed in sandbox
  (`Working/reports/bandit_animator_dead_state_qa_2026-10-15.md`); **do not rerun**.
- Wall/Floor numeric and visual QA (`*_qa_20261004.md`) — passed for `_FINAL`.
- E1A importer measurements in §5.1 — **do not re-measure without a change to
  FBX or importer settings**.

## 11. Documentation work log

| Date | Action | Result |
|---|---|---|
| 2026-10-15 | `Documentation/INTEGRATION_ROADMAP.md` created as the persistent route document | created |
| 2026-10-15 | `Documentation/README.md` — added **MANDATORY PROCEDURE** (7 steps), **task routing map** (9 verified task types), entry in §3 root-entry table and §4 CORE table; documented the missing general agent-rules file as a gap | updated, no competing rules created |
| 2026-10-15 | `.opencode/agents/reviewer.md` inspected — **deliberately not modified**: it runs with filesystem read access denied and cannot read canonical documents, so a routing rule there would be inert | gap recorded in README |

This log records documentation work only. It does **not** assert that any
integration stage has been performed in the project.

## 10. Exact next step

`E1A`–`E1F` are closed. Revisions are locked: **wall `WallSegment_01_FINAL.fbx`**
(visual QA `PARTIAL`), **floor `FloorSegment_01_r3.fbx`** (owner decision of
2026-10-04). Promotion standard **P-1** is adopted by the owner and verified
Git-compatible; **no promotion has been performed** and `BARREL_01`/`FENCE_01`
remain `APPROVED`. The `.gitattributes` anomaly is diagnosed (stale index stat
entry — benign). The D-17 texture investigation is closed with a proven cause and
**no fix applied**.

**D-16E changed the next step (2026-10-17).** The kit now has colliders, an
upright wall, a filled environment prefab configuration, and a live
`ArenaGenerator` prefab assignment with `useCoverClusters = true`. The gating item
is therefore no longer *wiring* but **evidence**: nothing has been run.

1. **Runtime verification of the wired integration (new, first priority).** Enter
   `Assets/Scenes/TestArena.unity` in Play Mode with the current serialised state and
   record: room count vs `minimumRooms=3`/`maximumRooms=6`; whether cover clusters
   instantiate; whether `CoverSystem` registers the kit `CoverPoint`s; whether
   `ArenaTacticalMap.IsPathAvailable` is blocked by the new wall colliders; whether
   `Bullet.OnCollisionEnter` fires on them. This is the only remaining item that
   closes `E2`, and it is safe (read-only observation, no file change).
2. **D-16 (scene write) — exercised only for the D-16E list.** Any further scene
   change needs a fresh bounded grant.
3. **Android — L3 (build) now `PASS`; L2 and L4 still open.** `activeBuildTarget =
   Android` while `PC_RPAsset` + quality level `PC` are active. The kit shader has 2
   variants and requests no shadows/additional lights/reflections on any platform.
   **L3 (build) closed 2026-10-18:** IL2CPP `arm64-v8a` APK built, Burst AOT linked
   after redirecting `TEMP`/`TMP` for the Editor process away from
   `%LOCALAPPDATA%\Temp` — `Working/reports/Android_apk_build_burst_temp_2026-10-18.md`.
   **Rebuild 2026-10-18 (вторая сессия):** после исправлений QA-A/B/D собран новый APK
   `Builds/Android/WildWestGunslinger_QAfix_2026-10-18.apk` (SHA-256
   `8A3E76167F661A72C0364297D03D4B2DE34B883CF1EC2DF825B762A43F91653C`, 0 ошибок) —
   `Working/reports/QA_ANDROID_FIXES_2026-10-18.md`.
   **L2 (variant compile)** and **L4 (device install/run)** remain open.
4. **Still open, not gating E2:** `D-03` player foundation (Bandit vs Cowboy),
   `D-04` `DEC-06` Cover, `D-05` Crate CP2, `D-17` route B, `D-07` checkpoint group,
   `D-21` `BYTE_COLOR`, and the wall asset-state conflict (`ART-0002` `QA PASS` vs
   roadmap C-6 `CREATED`) plus the `README.md` commit, which requires a separate
   Git permission.

`E2` remains **`NOT READY`** until item 1 produces runtime evidence.

---

## 12. Session entry — 2026-10-19 (seventh session, direction set A–E)

**Scope.** Recon + diagnosis for five owner-requested directions: mobile aiming
quality, hand/gun shake during aim, Doomy model/textures/height and dummy
integration, Blender→Unity mismatch for crates/boxes/covers, and meaningful
procedural room placement. Full evidence:
`Working/reports/RECON_DIAGNOSIS_ABCDE_2026-10-19.md`.

**No product file was changed in this session** (scene, prefabs, code, import
settings untouched). Measured in Play Mode only; **no Git operation** performed.

| Direction | Established cause | Owner decision required |
|---|---|---|
| **A** aim | Not a missing-mechanics defect: a continuous monotone curve, per-axis scaling and friction+tracking assist already exist. Missing are a **dual-zone/boost** region and independent per-axis sensitivity. Applicability of each candidate mechanic was checked against public sources (gamedeveloper.com, gamedev.stackexchange.com, salivity.github.io, gpadtester.org, Overwatch ramp docs) | No — files are outside the protected list |
| **B** shake | **Not reproduced at idle aim**: right-hand bone moved **0.226 mm/frame** (max 4.725 mm) over 149 frames, layer weight stable at 1.000, clip `Armature\|Pistol_Aim_Neutral`. Run-aim and fire-aim samples were **contaminated** (player died, base layer stuck in `Bandit_Death_Grounded`) and are **not** used as evidence | No, but a controlled sandbox harness is needed |
| **C** Doomy | Float measured exactly: **root Y = 1.000, `LeftToes` 1.166, `LeftFoot` 1.262**, collider `center (0,0,0) height 2` → the body physically rests 1 m high. **No textures exist for any Doomy material**, so "torn textures" cannot be a texture defect. Dummy = `WWG_Doomy_Foundation.fbx` (sandbox, unused); barriers B1–B5 from `D23` still stand | **Yes** — B1–B5 / `D23` step 1 first |
| **D** props | **Two asset families are mixed.** `TestArena.unity:23845-23848` points `coverPrefab`/`cratePrefab` at the *Environment* family (8-vert boxes, no vertex colours, shared fence texture, destruction is a no-op) while `fencePrefab`/`barrelPrefab` use the *Destructible* family (real meshes, vertex colour, working destruction) — which is exactly why the **barrel** is the correct control. Importer settings and world scale are equivalent and **not** the cause | **Yes** — family choice + `CP2`/`DEC-05` |
| **E** placement | `clusterMargin = 3.5` **pushes clusters away from the perimeter** — the opposite of rule R1; yaw is a full 0–360° instead of ±30° (R7); cluster composition is hard-coded (R5); `minimumDistanceBetweenCovers` is a **dead field**; `useRandomSeed = true` makes `seed` inert → generation is **not reproducible** | **Yes** — `ArenaGenerator.cs` is in the protected list |

**Reference image (E) does not exist** in the repository or the vault — it survives
only as the textual description and rules R1–R8 in `D22` §3. Pixel-accurate
composition is therefore not recoverable without the owner re-supplying it.

**Burst build workaround (operational, unchanged).** The `TEMP`/`TMP` redirect
described in `Working/reports/Android_apk_build_burst_temp_2026-10-18.md` was
superseded on 2026-10-19 by a better-supported diagnosis and a different fix:
processes launched from the project directory can only write inside the project,
while `bcl.exe` must write to the system `%TEMP%`. The Burst runtime is now
copied to `<user>\AppData\Local\Unity\com.unity.burst@2f3d916873ea\.Runtime`
and `UNITY_BURST_RUNTIME_PATH` is set (user level + Editor process). The path
**must** reproduce the `com.unity.burst@<hash>\.Runtime` structure, otherwise Burst
extracts version `Unknown`, treats `Library\BurstCache\JIT\*.dll` as stale and tries
to delete it — impossible, because the Editor holds those files as loaded modules
and `Library` inherits `DENY Everyone → DeleteSubdirectoriesAndFiles`. APK built
after this: `Builds/Android/WildWestGunslinger_AutoFireMultiTouch_2026-10-19.apk`,
475.4 MB, 158 s, 0 errors, SHA-256
`A2B6B1DFCA452A09961B07B48E1A030C27E01FCDD666A0BE9C471F38A22EC3C2`.
**The environment variable is machine state, not a project file — removing it will
break Android builds again on this machine.**

---

## 13. Session entry — 2026-10-19 (eighth session, directions A–E implemented)

**Scope.** Owner-requested five directions: mobile aiming quality, hand/gun shake
during aim, Doomy model/textures/height and dummy integration, Blender→Unity
mismatch for crates/boxes/covers, meaningful procedural room placement. Phase 1
(RECON + DIAGNOSIS) is `Working/reports/RECON_DIAGNOSIS_ABCDE_2026-10-19.md`;
phase 2 (implementation + QA) is
`Working/reports/QA_ABCDE_RECON_DIAGNOSIS_IMPLEMENTATION_2026-10-19.md`.

**Owner decisions received:** C1 (measure the dummy first, then fix the pivot) ·
D3 (diagnostics only for the asset families, no changes) · E1 (bounded change to
`ArenaGenerator.cs`) · B1 (temporary diagnostic files under `Assets/_Sandbox/`) ·
E2 (reference image to be supplied by the owner).

**Canonical scene was NOT modified. No Git operation was performed (staged = 0).**

> **Protected-scene note (2026-10-19).** The bytes of `Assets/Scenes/TestArena.unity`
> changed during this session because **Unity itself re-saved the scene when the
> Android build started** (`mtime` equals the build start; the builder saves a
> dirty scene before building). No existing serialized value was altered — all
> generator settings and the four prop-prefab references are byte-identical to
> the recorded baseline — but Unity wrote the defaults of the three new
> generator fields (`clusterWallInset 0.35`, `clusterWallRun 9`,
> `clusterYawJitter 30`) into the file. Additive and behaviour-neutral. The
> scene was already modified relative to `HEAD` before this session.

| Direction | Result | Evidence |
|---|---|---|
| **A** aim | Dual-zone profile added: boost from 0.85 deflection to ×2, independent yaw/pitch sensitivity, tunable dead zone. Curve measured **monotonic**, 180° in 0.64 s at full stick (was 1.29 s). Files `GameSettings.cs`, `MobileTouchControls.cs`, `ManualAimControls.cs` | `PASS` (Editor) |
| **B** shake | **Not reproduced**: with the animation frozen the hand bone moves 0.0000 mm/frame; the UpperBody layer adds only ×1.3 over the base. Logic does not shake the hands (`recoilKickRemaining` = 0). Most likely device-side cause: continuous recoil with Auto-Fire now ON by default | `NOT REPRODUCED` |
| **C** Doomy float | Measured root Y 1.000 / toes 1.166 → caused by `CapsuleCollider.center (0,0,0)` with `height 2`. **Fixed in the 4 prefabs** (`center (0,1,0)`, `Doomy_Model.y` 0.131 → 0.064). After: root 0.000, toes 0.095, **`FirePoint` 2.000 → 1.000 m** (partially unblocks `D24` §10.1) | `PASS` (Editor) |
| **C** Doomy mesh | **Diagnosed**: no Doomy texture exists at all, so "torn textures" are not a texture defect; in Play Mode the torso Z-extent grows 0.65 → 1.21 m, i.e. the retargeted UAL1 clip shears the skinning. The approved dummy (`WWG_Doomy_Foundation` + `Smoke_UAL1`) renders **intact**, but has identical bone offsets (so it would not fix the float) and only one body mesh (no costume). Still an owner decision: barriers B1–B5 of `D23` | `BLOCKED` |
| **D** props | **No change by decision D3.** Cause recorded: the scene mixes two asset families — `coverPrefab`/`cratePrefab` point at the *Environment* family (8-vert boxes, no vertex colours, shared fence texture, destruction is a no-op) while the barrel/fence use *Destructible*. Importer and world scale verified equivalent | `PASS` (diagnosis) |
| **E** placement | `ArenaGenerator.cs`: R1 (clusters now hug the walls instead of being pushed to the centre), R5 (per-template along-wall extent), R7 (wall-aligned ±30° instead of a full 360°), density derived from wall length, and the serialized `seed` is now authoritative (was overridden by `TickCount`) with the effective seed published in `activeSeed`. Measured: 60 clusters / 127 props, **mean distance to the nearest wall 0.35 m**, centre empty, **identical layout for the same seed** | `PASS` (Editor) |

**Reference image (E):** supplied by the owner in chat on 2026-10-19 — wooden
arena, plank floor and walls, all mass along the perimeter, empty centre,
2–4 item groups, fences parallel to the walls. **The file itself is not in the
repository**, so pixel-accurate comparison still requires the owner to store it.

**Still open / not covered:** Android device runtime (`UNVERIFIED` for every
direction), the Doomy mesh rewrite (owner decision), `DEC-05`/`CP2` and `DEC-06`
(blocking the crate and cover assets), and the reference image file.

---

## 14. Session entry — 2026-10-19 (ninth session: AIM re-tune, Doomy dummy, cover models, sections, hands matrix)

**Scope.** Owner-requested: AIM feel (no jolt, no lag), Doomy model/textures/height
plus the approved dummy, hand shake while shooting, cover placement matching a
supplied reference, and the Blender→Unity cover mismatch. Evidence:
`Working/reports/QA_AIM_DOOMY_HANDS_COVERS_REFERENCE_2026-10-19.md`.

**Reference image.** Supplied by the owner and now stored in the repository:
`Working/blender_reviews/approved_reference/1792221347 (1).png` (2 134 457 B) —
wooden arena, plank floor and walls, all mass along the perimeter, empty centre,
2–4 item groups, picket fences bounding the sections.

**Protected scene.** The owner-approved Doomy dummy and the approved cover models
were integrated **without changing the scene's prefab references for the Doomy**
(the prefab now nests the dummy). The scene was edited in exactly two places:
`cratePrefab` → `Crate_01`, `coverPrefab` → `Cover_01`. Additionally the
runtime leftover `GeneratedArena` (2 240 objects, 6.5 MB → 585 KB scene) and the
leftover `PROBE_Doomy_Repaired(Clone)` were removed. `QA_Doomy` was already gone.
`D16A_MaterialOverview` and `ArenaModule_Start/_Straight` were **kept**: they are
documented evidence placements (D-16A/D-16C/D-16E/D-17).

| Direction | Result | Evidence |
|---|---|---|
| **AIM** | The jolt came from the boost engaging at 0.85 with a linear blend (max speed step 95.3 °/s) and from a 0.12 dead zone that made small deflections produce **no** motion. Re-tuned: dead zone 0.06, exponent 1.25, boost zone 0.60, multiplier 1.6, boost blended with **smoothstep** (continuous slope). Max step 95.3 → **28.6 °/s**, 180° turn 0.64 → 0.80 s, curve still monotonic. One-time migration `AimTuningV2` replaces the stored values on existing installs | `PASS` (Editor) |
| **Doomy** | The deformation was **not** a texture fault: no Doomy texture exists. Cause = the production `Doomy.fbx` Avatar **Rig Configuration mis-match** (2 errors) making the retargeted UAL1 clip shear the mesh (torso Z-extent 0.65 → 1.21 m). The approved `WWG_Doomy_Foundation.fbx` (0 rig errors, 1 mesh, no costume) was moved into production **with its GUID preserved** and wired into the four Doomy prefabs. Root Y 1.000 → **0.000**, feet on the floor, mesh intact, **`FirePoint` 2.000 → 1.000 m** (partially unblocks `D24` §10.1) | `PASS` (Editor) |
| **Hands** | 7-mode diagnostic stand (`Assets/_Sandbox/HandShake_2026-10-19/`) measured: idle/aim 10.4 mm/frame (one 153 mm pose snap on aim engage), firing 4.1–4.3 mm/frame, aim+fire+moving 19.9 mm/frame (run sway). **No high-frequency jitter in any mode**; the recoil is not a contributor | `PARTIAL` — not reproduced in Editor, phone test pending (5-step instruction in the report) |
| **Placement** | Sections replaced: 4 crates in two rows (second row offset +0.18 m), or 3 crates + 1 barrel, or 4 barrels, each bounded by a picket fence, placed against a wall with the cluster yaw at wall ± 30°. Measured 42 sections, **42/42 with a fence**, mean distance to the nearest wall 0.35 m, centre empty, **identical layout for the same seed** | `PASS` (Editor) |
| **Cover models** | The visible mismatch was the generator pointing at the **blockout "Environment" family** (8-vert boxes, no vertex colours, one shared fence texture) while the barrel/fence used the approved "Destructible" family. `Crate_01` was lying on its side (missing −90° X, `minY = −0.375`); the conversion was moved **inside** the prefab because the generator overwrites the member's root rotation. The `D22F` "degenerate collider" premise was **disproved** by measurement (`Cover_01` collider AABB = renderer AABB, `minY = 0`). `cratePrefab`/`coverPrefab` now point at the approved models; the crate keeps its gameplay role (`DestructibleObject` + 2 `CoverPoint` at proper height) | `PASS` (Editor) |

**Not closed by this session:** `CP2`/`DEC-05` (crate fragments — the crate still
destroys without debris), `DEC-06` (cover promotion status), `D-24` (trajectory
height, partially), and the **Doomy revolver** (barrier B5) — the dummy carries no
weapon, so the enemy now needs a separate weapon object. The owner must confirm
that a bare mannequin is acceptable in combat.



---

## 15. Session entry — 2026-10-20 (tenth session: Cowboy Elderly replaces Doomy, crate destruction chain, reference placement, build)

**Scope.** Owner-requested: replace the enemy visual model with the supplied
Cowboy Elderly package, integrate the approved cover/crate Blender sources with
the existing destruction system, match the placement to the approved reference,
verify AIM and hands, and clean conflicting asset variants. Evidence:
`Working/reports/QA_COWBOY_COVERS_REFERENCE_2026-10-20.md`.

**Sources used (verified, not assumed).**
- Cover: `Working/blender_src/cover/Cover_Western_Wooden_Working.blend` (METRIC 1.0;
  `Cover_Fragments` 11, `Cover_Western_Wooden` intact, `QA_Rig`;
  `MAT_cover_western_wood_metal`, no image textures).
- Crate: `Working/blender_src/crate/Crate_01_Working.blend` (`Crate_Fragments` 10,
  `Crate_Intact`, `QA_Setup`; `MAT_crate_01_wood_metal`).
  The existing FBX exports were already produced from these blends (source path is
  embedded), so **no re-export was required** — the defect was the assignment.
- Character: `cowboyelderly.unitypackage` (508.8 MB) from the owner's Desktop —
  Asset Store `cowboy-elderly-226819` (matches `ART-0004`); the character only was
  imported (59 assets / 109 files), the demo arena and TMP fonts were excluded.

| Direction | Result | Evidence |
|---|---|---|
| **Enemy model** | The four `Doomy_*` prefabs now nest `cowboy_character_v2` as `Enemy_Model`. Root components, `Doomy.controller`, `CapsuleCollider` (center 0,1,0 / h 2) and `FirePoint` (1.000 m) preserved; avatar `cowboy_characterAvatar` (valid human, 87 bones). Model Y offset calibrated to **0.2327** from the measured minimum toe contact over a walk cycle (`toesY 0.016`, `headY 1.64`) | `PASS` (Editor) |
| **Cowboy materials** | All 12 package materials pointed at a shader GUID **absent from the package** → magenta (`Hidden/InternalErrorShader`). Mask layout measured directly from the TGA: **R = metallic, A = smoothness**. Clean `Universal Render Pipeline/Lit` materials `WWG_cowboy_*` created and assigned; the character now renders fully textured (vest with buttons, plaid trousers, boots, revolver in holster) | `PASS` (Editor) |
| **Crate destruction** | `Crate_01` now carries the full chain: `State_Intact` + `State_Destroyed` (10 fragment instances), `hideOnDestroyed` (1), `showOnDestroyed` (10), `debrisBodies` (10), `coverPoints` (2, at body height), per-fragment `BoxCollider` + `Rigidbody` (mass 2). Measured in Play Mode: `Intact` → `Destroyed` after `TakeDamage(999)`, 10 fragments active, `isKinematic = 0` | `PASS` (Editor) |
| **Placement** | Perimeter sections against a wall (mean 0.35 m), cluster yaw wall ±30° with wall-clearance compensation, 4 crates in two offset rows / 3+1 / 4 barrels, picket fence on a random side, serialised `seed` is now authoritative (`activeSeed` published). Measured 43 sections / 215 props / 0 sunk / 0 floating for seed 12345 | `PARTIAL` — 38 prop-to-prop overlaps between neighbouring sections remain open |
| **AIM** | Unchanged from session 9 and re-verified: dead zone 0.06, exponent 1.25, boost 0.60/1.6 with smoothstep, max speed step 95.3 → **28.6 °/s** | `PASS` (Editor) |
| **Hands** | 7-mode matrix re-measured; the control run with frozen animation shows **0.0000 mm/frame** hand motion → the hand logic is static; the 10.4 mm figure is a one-off pose snap on aim engage | `PASS` (logic) / phone test pending |
| **Build** | `Builds/Android/WildWestGunslinger_CowboyCovers_2026-10-20.apk`, 509.5 MB (previous 474.3 MB; +35.2 MB = Cowboy textures at 2048² ASTC), SHA-256 `386E084FFCFBF5E087E9FAB10648C32E53CC845F429E9613E89E69565FB17543`, 195.98 s, 0 errors | `BUILD VERIFIED` |

**Protected scene.** The scene was edited in exactly two serialized references
(`cratePrefab` → `Crate_01`, `coverPrefab` → `Cover_01`) plus the removal of the
runtime leftovers `GeneratedArena` (2 240 objects) and `PROBE_Doomy_Repaired(Clone)`.
Nothing was lost against the Git baseline (0 removed objects); the 167 added objects
are the owner's Bandit character, the documented `D16A_MaterialOverview` section and
the environment generator. Four new serialized fields (`clusterWallInset`,
`clusterWallRun`, `clusterYawJitter`, `activeSeed`) are defaults written by Unity's
own re-save of the file.

**Not closed by this session:** asset-variant cleanup (a reference map is required
before any archive move), the 38 section overlaps, `CP2`/`DEC-05` and `DEC-06`
registry statuses (deliberately untouched), the `ART-0004` licence record for
Cowboy Elderly, and the Android device verification (no device available).

---

## 16. Session entry - 2026-10-20 (eleventh session: Cowboy materials/animation, two-tier sections, asset cleanup, licence)

**Scope.** Owner-requested: transfer the real crate/cover materials, fix the Cowboy Elderly skin and the run-while-shooting animation, build two-tier cover sections with a fence, remove duplicate assets, verify the Cowboy licence, update documentation, rebuild the APK.

| Direction | Result | Evidence |
|---|---|---|
| **Crate/cover materials** | The approved Blender sources were measured in background Blender: both MAT_crate_01_wood_metal and MAT_cover_western_wood_metal are **14-node procedural graphs** (UV path, Col vertex-colour attribute, no image textures) - structurally identical to the fence graph, and the project already carries the agreed pipeline: vertex colours + WWG/VertexColorWoodMetal. No baking was needed and none was done. Verified in Unity: every FBX carries vertex colours and UVs, every prefab uses MAT_WWG_WoodMetal_{Crate,Cover} on the canonical shader, 90 production materials checked with 0 problems | PASS (Editor) |
| **Cowboy skin** | The "white skin" was **not** a material loss: the package textures import correctly (a swatch render of the leather holster shows brown, not blue). The one real defect found was a colour-channel repair attempt that was **reverted** after the swatch test disproved it. Final state: natural skin, beard, eyes and hair (QA_Enemy_head_front_2026-10-20.png) | PASS (Editor) |
| **Run while shooting** | Root cause: Doomy.controller was **single-layer** and IsAiming is driven by "player visible", so a running enemy left locomotion for a standing aim pose and the legs froze. Fix: a second UpperBody layer (mask Assets/Animations/Enemies/WWG_UpperBody_Mask.asset, humanoid body parts + the Cowboy spine chain, **hips excluded** so the legs stay on the locomotion layer). Measured over the full behaviour cycle: foot swing stays 0.29-0.43 m while the upper body enters Aim/Fire/Reload | PASS (Editor) |
| **Two-tier sections** | Sections are now two-tier: lower 4 crates / upper 4 barrels (or 3 crates + 1 barrel, or 4 barrels on 4 barrels), enclosed by a three-sided picket fence with the room side open for approach. Real prefab dimensions were measured and used (crate 1.000 x 0.757 x 0.750, barrel 0.604 x 0.900 x 0.574, fence 1.960 x 1.200 x 0.160). 44 sections, 4-6 per room, 172 upper-tier props | PASS (Editor) |
| **Overlaps** | The 38 inter-section overlaps are gone: **0 real overlaps** (measured with Physics.ComputePenetration, resting contacts excluded). Also fixed an **inverted sign** in the wall-clearance compensation, which had pushed sections into the wall; the passage along the wall is now 1.22-1.72 m | PASS (Editor) |
| **Crate destruction** | Re-verified after the fix: Intact -> Destroyed, enabled colliders 1 -> 11, 10 fragment renderers visible, 10 rigidbodies switched to dynamic. The crate's 10 fragment colliders were **enabled** (unlike cover/barrel/fence) and have been disabled, matching the other destructibles | PASS (Editor) |
| **Asset cleanup** | Reference map built over 263 scenes/prefabs/materials/controls. 34 unreferenced assets archived to Working/archive_assets/2026-10-20/ **with their .meta** (Doomy legacy model/prefab/materials, old Cover r2-r4, old Barrel exports, unused Wall/Floor revisions, cowboy v3, placeholder prefab). After cleanup: 0 missing components, 90 production materials with 0 problems, all production assets present. Third-party libraries (ADG plank textures, Innerverse skybox, TMP fonts) are left in place as REVIEW - they are libraries, not duplicate versions | PASS (Editor) |
| **Licence** | Fab listing checked: **Free**, Fab Standard License, AI use allowed, credit not required, both pricing tiers grant identical rights (so the revenue threshold does not restrict use), commercial distribution of the game allowed, standalone redistribution of the asset prohibited. Recorded in ART-0004 section 10 with the remaining evidence gap named | PASS (documentation) |

**Not closed by this session:** the client's detailed "License terms" panel could not be captured as text (documented in ART-0004 10.2); the single-sided hair cards need a two-sided shader if the author notes are to be honoured; CP2/DEC-05 and DEC-06 remain open; no Android device was available, so device verification stays NOT VERIFIED.
---

## 17. Session entry - 2026-10-20 (twelfth session: enemy death, aim/bullet alignment, progressive barrel destruction)

**Scope.** Owner-reported Android defects: (A) the Cowboy enemy corpse flies away or moves in an unknown direction on death, (B) the visual aim does not match the bullet direction, (C1) barrel debris blocks further shots, (C2) one shot destroys the whole barrel. One limited QA pass, no navigation work, **no Android build**, no commit.

| Direction | Root cause | Fix | Evidence |
|---|---|---|---|
| **A - death** | The AI turns the enemy by writing 	ransform.rotation on a **non-kinematic Rigidbody** (a teleport), which the physics resolves as a spin - up to **18 rad/s** accumulated. While the AI kept writing, the spin was corrected every frame; death stopped the writes, so the corpse unwound and was flung (measured 160-360 deg, 2-5 m, 14.5 m/s peaks). The capsule then stayed inside the arena contact envelope and the solver pushed it out at a constant 2.1 m/s | EnemyController: zero the angular velocity and freeze FreezeRotationY on death, zero the horizontal velocity **every** physics tick while dead, and put the body in kinematic mode (only when grounded, so an airborne death still falls) | probe_death_baseline.csv -> probe_death_v3/v4.csv: displacement 2.145 m -> **0.001-0.015 m**, rotation 160.6 deg -> **0.03-9.62 deg**, angular velocity 18 -> **0** |
| **B - aim vs bullet** | The aim pose is a **static local-space animation**, the bullet always goes from irePoint to the player, FacePlayer() ran before the movement code (which turned the body again), and a 	ransform.rotation write is not authoritative on a dynamic Rigidbody (measured **94 deg** mismatch) | FaceDirectionToward now writes 
b.rotation; the facing to the player is re-applied after the state update in FixedUpdate | probe_aim_baseline.csv (94.0 deg at the shot) vs probe_aim_v4.csv: steady-state median **32.3 deg**, last frames **1.3-15.3 deg**. The shot-moment probe value is taken in Update before the next FixedUpdate, so it is not a valid before/after metric - a natural firing cycle was not measured |
| **C1 - debris blocks shots** | The player bullet swept with the mask ~0 and stopped at the first hit, so a debris fragment ate the bullet; the enemy bullet had the same behaviour | Bullet sweeps with SphereCastAll and skips colliders of an **already destroyed** DestructibleObject; EnemyBullet got the same filter. No layer or collision-matrix change, debris physics untouched | probe_barrel_final.csv: shot 3 passes the debris of the destroyed barrel A and damages barrel B |
| **C2 - barrel dies in one hit** | Barrel maxHealth 50 vs the player bullet damage **200** -> instant Destroyed; the Damaged stage was empty | Barrel prefab only: maxHealth **400** (2 x 200, derived from the measured damage), hideOnDamaged = [intact body], showOnDamaged = [12 hoops], disableColliderOnDestroyed = true, plus a permanent Collision node so the damaged barrel stays hittable | probe_barrel_final.csv: shot 1 -> Damaged 200/400, shot 2 -> Destroyed. Crate and cover untouched |

**Not closed:** the shot-moment aim metric in a natural firing cycle; the corpse still blocks bullets and the mesh falls inside the standing capsule (no ragdoll in the project, no new assets allowed); crate and cover still break in one hit (out of scope by instruction); other 	ransform.rotation writes in the AI remain (full conversion to 
b.rotation would change AI behaviour); EnemyHearing.FixedUpdate has no death gate (harmless in the measurement, left alone). **No APK was built** (forbidden in this pass) - there are no build-level or device-level results.
---

## 18. Session entry - 2026-10-20 (thirteenth session: APS v2.06 research - study only, no implementation)

**Scope.** Research the purchased Fab plugin *AdaptivePathfindingSystem: AI Navigation Plugin v2.06* (NordVader) and prepare a grounded plan for the WWG defect where enemies run onto crates/barrels/covers and appear to jump. **Research stage only: no WWG gameplay code, A*, NavMesh, ArenaTacticalMap.cs or EnemySpawner.cs was changed, no APK was built, no commit.** Evidence: Working/reports/APS_PLUGIN_ANALYSIS_2026-10-20.md.

**Plugin located.** C:\Program Files\Epic Games\UE_5.8\Engine\Plugins\Marketplace\Adaptive64768b7cb9a5V14 - Unreal Engine **5.8.3**, .uplugin VersionName **2.06**, one copy, the Fab build for UE 5.8. The package ships **complete C++ sources** (no decompilation needed) but **no Content/**: no demo map and no Blueprint examples, so no Unreal experiment was possible; no Unreal MCP is available in this session (only blenderMCP and unityMCP).

**What the plugin actually is.** AAPS_NavigationData extends ARecastNavMesh and overrides the engine's FindPath delegate; the navigation surface stays the stock Recast/Detour. Five modes exist in the installed build (EngineOnly, APS_Arc, APS_LowLevelTiles, APS_LowLevelArc, APS_Experimental - the last one is not mentioned on the Fab page). Per-agent parameters come from IAPS_MoveToInterface (ArcStrength, NumMidPoints, MidpointMode, LaneOffset) and are read on every path request.

**Core finding.** The curved route is **post-processing of an already found path**, not an alternative obstacle search: the longest edge is replaced by two halves that are each a **real path query**, chosen from 9 off-axis candidates, projected onto the navmesh, scored by clearance/angle/backward/side penalties, and rejected if the path would stretch beyond 60 % (first step) or 25 % (later steps). Lane Offset shifts the route laterally at corners and, above a threshold of 180, can inject a genuinely different side route. FindCustomPath performs **no validation** - the game code owns that.

**Applicability to the WWG defect - two levels.**
- **Level 1 (the actual defect, code-confirmed in WWG):** the nav grid is 2D with cellSize 1.5 while gentRadius 0.65 and the walkability probe uses radius  .55 at height +0.8, so a path may pass ~0.15 m from a crate surface and the physics lifts the capsule onto it; when A* fails the enemy falls back to MoveDirectlyToTarget and walks **into** the obstacle; the cover goal is the cover point itself. The AI never commands vertical motion (Y is zeroed in the movement code), so the "jumping" is a physics artefact. **APS does not solve any of this.**
- **Level 2 (route shape - optional):** curvature, lane spreading, per-state profiles and side-route injection are transferable as **ideas**, implemented independently on the Unity API. APS itself does not fix obstacle avoidance.

**Licence.** Fab Standard License: commercial use and commercial distribution of the game are allowed, standalone redistribution of the asset is not, no attribution required, and the Personal/Professional tiers grant identical rights. The licence does **not** grant the right to port the proprietary implementation into another engine. Therefore: study the plugin as a behaviour reference, implement the ideas from scratch, and do **not** copy code, names, comments or assets into WWG.

**Recommended next stage (not started).** Mandatory correctness first: remove the direct fallback into obstacles, make the agent clearance honest (radius, not a single cell), separate the cover goal from the cover surface, and stop the vertical lift. Then optional route shaping (fillet, lane offset, state profiles, side route) plus a measuring stand (clearance, penetration count, vertical speed). Items 1-2 of the mandatory list touch the protected ArenaTacticalMap.cs and need an explicit permission.
---

## 19. Session entry - 2026-10-20 (fourteenth session: navigation correctness - direct-move fallback, clearance, cover goal)

**Scope.** Fix the confirmed navigation defects of the AI: direct movement into an obstacle when A* fails, routes too close to covers, the movement goal coinciding with the cover point, and the physics lifting the capsule onto crates. Permission was given for **targeted** changes in ArenaTacticalMap.cs and the existing A*. **No APK was built**, no commit, no navigation rewrite, no APS features.

**Baseline measured** (real map, real props, Working/reports/nav_probe_baseline.csv): the path to a goal inside a cover **ended inside the prop in 8 of 8 cases** (the earlier "9 of 9" figure belonged to a superseded run of the stand - see section 21), the minimum clearance by the baseline methodology (to any collider except the measured prop) was **0.00-0.55 m**, and a capsule pressed into a prop was lifted by the solver by up to **0.568 m** (|vy| 6.83 m/s). 1660 walkable cells.

**Root causes confirmed in code:** (1) MoveTowardsKnownTarget moves **directly** to the goal when it is closer than 5 m - the cover goal sits ~0.9 m from a crate, so the agent was sent straight into it; (2) when A* fails the same code moved **directly at the goal** through the obstacle; (3) TryFindPathInsideRoom **unconditionally overwrote** the last path point with the raw goal, so a goal inside a prop became the path's destination; (4) the node probe and the path-simplification line-of-sight test used radius 0.55 m while the navigation agent radius is 0.65 m and the enemy capsule radius is 0.5 m, and the edges between cells were never validated.

**Fixes (targeted):** ArenaTacticalMap - the walkability probe and the simplification LOS test now use max(agentRadius, obstacleCheckRadius); the raw goal is only used as the last path point when the segment to it is clear for the agent volume (IsSegmentClear); the A* tracks the closest visited cell and returns its path as a best effort when the search fails; new TryFindPathToNearestReachable. EnemyController - the short-distance direct move requires a clear segment, a failed search falls back to the nearest reachable point, and if even that is impossible the agent **stops** (only the horizontal velocity is zeroed, gravity is untouched); diagnostic counters 
avFallbackToNearestCount / 
avStoppedUnreachableCount were added. The A* itself (search, heuristic, scoring, 4-neighbour grid, maxPathNodes) was **not** rewritten.

**After** (
av_probe_after.csv): paths ending inside a cover **0 of 8** (was 8 of 8), points inside a prop **0** in total (was 8 in total), minimum clearance **0.68-1.09 m** by the after-methodology (only to destructible props; the baseline methodology measured to any collider and gives 0.00-0.55 m, so the two rows are **not** a before/after pair), walkable cells 1660 -> 1420 (-14 %, the cost of correctness), path build 1.00-6.47 ms, functional 8 s run: **0 prop contacts, 0.000 m vertical rise, 0 unsafe fallbacks, 0 stops**. The capsule lift under a **direct** push into a prop is unchanged - that is a property of Unity Physics, not of the route, and was deliberately not "fixed" by blocking vertical motion. Re-verified against the saved CSVs in section 21.

**Not closed:** the functional run never reached a cover, so the runtime "takes a position at the cover" behaviour is not confirmed; routes through the dense two-tier sections are rarer (the agent now stops next to a section it cannot squeeze through) and need observation; the unreachable-target case stops the agent and relies on the existing path-refresh mechanism instead of a dedicated wait state; no build/device verification.
---

## 20. Session entry - 2026-10-20 (fifteenth session: documentation audit and state reconciliation - documentation only)

**Scope.** Reconcile the project state across code, QA reports, AI_CONTEXT, this roadmap, the Obsidian vault, local Git and GitHub, and publish the documentation only. **No game code, scene, prefab or asset was changed; no APK was built; no commit of game code.** Evidence: Working/reports/PROJECT_DOCUMENTATION_RECONCILIATION_2026-10-20.md; transfer document AI_CONTEXT/CHATGPT_HANDOFF_CURRENT.md (created).

### 20.1 Canonical source and model

- The canonical plan of record is **this file**. The Obsidian roadmap note is a linked working representation, not a second plan.
- State model used from now on: *done and confirmed / implemented but partially verified / awaiting editor check / awaiting build / awaiting Android QA / in progress / planned / blocked by external dependency / outdated or cancelled*.
- State of the world on 2026-10-20: branch main, HEAD 12cd312, origin/main identical (0/0), staged = 0 **but 645 working-tree entries (86 modified tracked + 559 untracked)**, including 40 changed game scripts that are **not** published.

### 20.2 Confirmed state of the main directions

| Direction | State |
|---|---|
| Procedural arena, two-tier cover sections | done and confirmed (Editor) - 44 sections, 4-6 per room, 1.22-1.72 m wall passage, 0 real overlaps |
| Player, controls, aiming | implemented, partially verified (Editor) - AIM re-tuned; device QA of the old list remains historical |
| Weapons, projectiles, hit resolution | implemented, partially verified (Editor) - bullets now pass through the debris of a destroyed destructible |
| Enemy death | done and confirmed (Editor) - corpse displacement 2.145 m -> 0.001 m, rotation 160.6 deg -> 0.03 deg |
| Enemy aiming vs bullet direction | implemented, partially verified (Editor) - steady-state mismatch median 32 deg; the shot-moment metric in a natural firing cycle is still unmeasured |
| Destructible barrel | done and confirmed (Editor) - 1 shot -> Damaged, 2 -> Destroyed |
| Destructible crate | implemented, partially verified (Editor) - 10 physical fragments; CP2/DEC-05 remain **formally open** (owner act) |
| Navigation correctness | editor verified (geometry/probe checks) - paths ending inside a cover **8/8 -> 0/8** (re-verified from the saved CSVs, section 21); clearance 0.00-0.55 m (baseline methodology) vs 0.68-1.09 m (after methodology) - **not a before/after pair**; **in-game cover approach still pending**; build and device not verified |
| Character art / materials | done and confirmed (Editor) - Cowboy Elderly in the four enemy prefabs, working URP/Lit materials, 34 duplicate assets archived |
| External research (APS v2.06) | documented only - no integration, and the licence does not permit porting the implementation |
| Audio, weapon-model integration, balance, mobile performance | planned / not started |
| Android build and device QA | awaiting build and device - the last APK (CowboyAnim_TwoTier_2026-10-20.apk, SHA-256 66D231D8...) **predates** the death, aiming, barrel and navigation fixes |

### 20.3 Documentation discrepancies found (not silently fixed)

| # | Where | Claim | Fact |
|---|---|---|---|
| D1 | Documentation/PROJECT_STATE.md (CANONICAL, 2026-10-15) | character art "not integrated", visuals only on the Bandit prefab | the enemy visual is Cowboy Elderly in four prefabs, two-layer controller - the canonical status document is **outdated**; changing the canonical status is the owner's act, so it is recorded here and in the handoff |
| D2 | AI_CONTEXT/CURRENT_TASK.md | "Stage 4.4.5 FROZEN / PAUSED", "CRATE_01 CP1 COMPLETE" | the project is far beyond that stage; the file already declares itself superseded as canonical |
| D3 | Documentation/History/OPEN_ISSUES.md | Cowboy needs a recorded purchase proof | the asset was acquired, the Fab licence is verified (ART-0004 section 10); the issue still needs an owner decision to close |
| D4 | AI_CONTEXT/CONFIRMED_STATE.md, AI_CONTEXT/README.md | historical / superseded | consistent with reality - left unchanged by design |
| A1-A3 | QA_NAVIGATION_CORRECTNESS_2026-10-20.md vs its CSVs | "9 of 9", "0.75-1.23 m", "0.9-8.2 ms" | the saved CSVs contain **8** `target_inside` cases (8/8 -> 0/8), clearance **0.68-1.09 m** (after methodology) vs **0.00-0.55 m** (baseline methodology - not a pair), build **1.00-6.47 ms**. **Re-verified and corrected on 2026-10-20 - see section 21 and the metrics reconciliation report** |
| A4 | both navigation CSVs | 9-column header | rows carry 8 fields (no pathFound), the functional row 9, and decimals use a comma - tolerant parsing is required; the conclusions are unaffected |

### 20.4 Next steps (a plan, not an authorisation)

1. Confirm the navigation result in game: the agent actually taking a cover position, and the behaviour in the dense two-tier sections (routes there are now rarer - 1660 -> 1420 walkable cells).
2. Close the remaining death / aiming / destruction findings on evidence; CP2/DEC-05/DEC-06 are owner decisions.
3. Integrate the selected weapon models and visuals **after** a licence check.
4. Wire the prepared audio and check the game events.
5. Build the APK (owner permission) and run the Android QA session.
6. Stabilise the game loop, the balance and the mobile performance.
7. Optional and separate from APS: implement the route-shape ideas on the Unity API.

### 20.5 Integration point for future reports

Any later QA report (navigation cover approach, Android session, weapon/audio integration) is integrated into this roadmap as a new numbered section and into WWG_CURRENT_STATE.md / AI_CONTEXT/CHATGPT_HANDOFF_CURRENT.md as an addition - the existing context is never overwritten or delayed until a report appears.

---

## 21. Session entry - 2026-10-20 (sixteenth session: navigation QA metrics reconciliation - documentation only)

**Scope.** The documentation audit found that the numbers in the navigation QA report did not match the saved measurement files. The CSVs were re-read from bytes, re-parsed with two independent implementations, the metrics were recomputed, and every derived document was corrected. **No game code, scene, prefab, asset, APK or stand was changed or re-run.** Evidence: `Working/reports/NAVIGATION_QA_METRICS_RECONCILIATION_2026-10-20.md`.

### 21.1 What the files actually contain

| Fact | Value |
|---|---|
| Delimiter | `;` - a comma is the **decimal separator** inside numeric fields |
| Encoding / line endings | UTF-8 with BOM, CRLF, trailing newline |
| Header | 9 names, including `pathFound` |
| Data rows | **8** fields (prop scenarios), **10** (info rows: the count is the last field), **9** (functional enemy row) |
| Why the lengths differ | the header is a hard-coded literal, the rows are written by a format string that **never emits `pathFound`** - the header describes the schema but is **not a positional map** for any row type. `pathFound` cannot be recovered (it is absent from every row of both files and was used by no conclusion) |
| Parsing rules applied | 8 fields -> scenario, result, pathPoints, lastPointInProp, minClearance, pointsInProp, buildMs, detail; 10 fields -> count in the last field; 9 fields -> scenario, result, -, -, -, minClearance, contacts, buildMs, detail; decimals parsed by replacing `,` with `.`; the free-text `detail` column keeps dots (invariant `Vector3` formatting) |

### 21.2 Recomputed metrics (two independent parsers, identical results)

| Metric | Baseline | After | Comparable |
|---|---|---|---|
| paths ending **inside** a cover | **8 of 8** | **0 of 8** | yes - the primary result |
| sum of path points inside a cover | 8 | **0** | yes |
| clearance, baseline methodology (to any collider) | 0.00-0.55 m | - | **no** |
| clearance, after methodology (to destructible props only) | - | 0.68-1.09 m | **no** - the two rows use different definitions |
| path build time, `target_inside` | 1.66-4.97 ms | 1.00-6.47 ms | yes |
| walkable cells | 1660 | 1420 | yes |
| functional 8 s run | - | 0 prop contacts, 0.000 m rise, 0 fallbacks, 0 stops | yes |
| capsule lift under a direct push | max 0.568 m | max 1.482 m | physics characterisation, **not** a navigation metric |

**Sample comparability:** both runs contain 8 `target_inside` scenarios but over different props (baseline `Intact`x4/`Collision`x3/`Crate`x1; after `Crate`x3/`Intact`x3/`Collision`x2), and 5 vs 2 `far_side` scenarios. Aggregate metrics are comparable; per-prop clearance values are not.

### 21.3 Correction applied

The report's first edition claimed **9/9**, **0.75-1.23 m** and **0.9-8.2 ms** - figures from a superseded run of the stand whose CSV was later overwritten. The QA report now carries the verified numbers plus a dedicated correction section (5.2) stating the old values, what the re-check showed, the method, the CSV format facts and the non-comparability of the clearance rows. The same corrections were propagated to this roadmap (19, 20), to `AI_CONTEXT/CHATGPT_HANDOFF_CURRENT.md`, to the documentation reconciliation report (A1-A7) and to the vault notes. The original CSVs were **not** modified.

**Unchanged open items:** the in-game cover approach is still unconfirmed; routes in the dense two-tier sections may be rarer; the solver can still lift the capsule under a direct push; no dedicated wait state; no APK, no device - `BUILD VERIFIED` and `DEVICE VERIFIED` are not claimed.
