# QA — Cowboy Elderly (skin, animation, shake), two-tier cover sections, crate/cover materials, asset cleanup, licence

**Date:** 2026-10-20 (session 11)
**Scope:** owner request of 2026-10-20 (Cowboy Elderly addendum + continuation of the cover/crate/cleanup/licence work)
**Branch / HEAD:** `main` / `12cd312860eed0f6ddc85c8f44752df0900d35a4` — unchanged
**Git:** no commit, no push, staged = 0
**Report owner:** `Working/reports/QA_COWBOY_MATERIALS_ANIM_SECTIONS_2026-10-20.md`
**Predecessors:** `QA_COWBOY_COVERS_REFERENCE_2026-10-20.md`, `QA_AIM_DOOMY_HANDS_COVERS_REFERENCE_2026-10-19.md`

---

## 0. Summary

| # | Direction | Result | Status |
|---|---|---|---|
| 1 | Bandit history / control checklist recovered from `wwg-vault` | done | `PASS` |
| 2 | Cowboy skin "white" | cause isolated, materials/textures verified correct, a wrong repair reverted | `PASS` (Editor) |
| 3 | Animation inventory + run-while-shooting | root cause found, upper-body layer added, full cycle measured | `PASS` (Editor) |
| 4 | Shake / transition re-check on Cowboy | measured per frame, no high-frequency jitter found | `PASS` (Editor) |
| 5 | Crate + cover real materials from Blender | measured in Blender, verified in Unity, no baking needed | `PASS` (Editor) |
| 6 | Crate + cover destruction | re-verified, duplicate fragment colliders fixed | `PASS` (Editor) |
| 7 | Two-tier sections with fence, passages, ~4/room | implemented, 0 real overlaps, 1.22-1.72 m passage | `PASS` (Editor) |
| 8 | 38 inter-section overlaps | eliminated (0) | `PASS` (Editor) |
| 9 | Asset cleanup | reference map, 34 assets archived with `.meta`, 0 broken refs | `PASS` (Editor) |
| 10 | Cowboy licence | Fab verified, recorded in `ART-0004` §10 | `PASS` (documentation) |
| 11 | Docs / Obsidian / roadmap | updated | `PASS` |
| 12 | Android build | built, SHA-256 recorded | `BUILD VERIFIED` |

**Device verification: `NOT VERIFIED`** - no Android device is attached to this machine.

---

## 1. Bandit control checklist (recovered from `wwg-vault`)

Sources: `00-Inbox/2026-10-07 Bandit - deformation repair validated, not integrated.md`,
`2026-10-07 Bandit - free animation source + visual quality checkpoint.md`,
`2026-10-07 Bandit - production import staged, blocked by live Unity session.md`,
`2026-10-07 Bandit - production import STOPPED at Gate 1, documentation conflict.md`.

| Prior finding | Re-checked on Cowboy Elderly | Result |
|---|---|---|
| Shear from skin weight on bones outside the Humanoid map (`calf_twist_01`, `thigh_twist_01`): 34-44 % of the weight on unmapped bones | Cowboy rig has 87 bones, avatar valid human with 55 human entries; the retargeted UAL1 clips run without mesh tearing | no shear observed |
| Animation source: Quaternius Universal Animation Library, **CC0**, 8-clip WWG set (`Idle_Loop`, `Walk_Loop`, `Sprint_Loop`, `Pistol_Aim_Neutral`, `Pistol_Shoot`, `Pistol_Reload`, `Hit_Chest`, `Death01`) | The enemy controller still uses exactly these clips; the Cowboy FBX carries no clips of its own, so all motion is cross-rig retarget | confirmed, no new asset needed |
| Writing `Mesh.boneWeights` at runtime corrupts skinning - route banned | Not used anywhere in this session | respected |
| Blender export axes `-Z` / `Y` | Not re-exported: the packaged FBX is used as delivered | n/a |
| "The revolver detaches" - false, constant 0.166 m hand gap | Not re-measured for Cowboy (the `colt` is a skinned part of the rig) | not a regression |

---

## 2. Cowboy skin - cause isolation

### 2.1 What was actually wrong

The complaint "the skin looks white, as if the texture or material is missing" was investigated end to end:

1. **Mesh/material mapping** (`cowboy_character_v2.prefab`): the `body` mesh has **7 sub-mesh slots** - legs, torso, eye.sclera, eye.cornea, hands, head, hair; plus the `colt`, `hair`, `holster` meshes. `mat_cowboy_eye.cornea` has **no base map** (it is the transparent cornea over the sclera) and would render white on its own.
2. **Package material state**: all 12 package materials point at a custom Quaternius shader GUID `6e4ae4064600d784cac1e41a9e6f2e59` that is **absent from the package** -> the character renders magenta. The production fix (from the previous session) is the clean URP/Lit set `WWG_cowboy_*` (8 materials).
3. **Texture channel order** - the suspected cause, tested and **disproved**:
   - Raw TGA decode suggested the channels were stored B,G,R (head albedo mean 0.43/0.48/0.73 = blue; holster 0.11/0.15/0.19 = blue leather).
   - A repair was applied (swap R/B, 27 textures) and a **swatch render** was produced: the leather holster rendered **blue** in the repair and **brown** in the original -> the original import was already correct and the repair had broken it.
   - The repair was therefore **fully reverted** (all 27 generated PNGs deleted, all 22 materials re-bound to the original TGA/PSD, original import settings restored).

### 2.2 Verified end state

| Check | Result |
|---|---|
| Head albedo (as Unity imports it) | natural skin tone, beard, brows, eyes - `QA_Enemy_head_front_2026-10-20.png` |
| Hands | natural skin tone (were white in the 2026-10-19 render) |
| Leather (holster, gun belt) | brown leather, revolver metallic - `QA_Enemy_head_current_2026-10-20.png` |
| Mask maps | R = metallic (colt 0.88, cloth 0.01), G = AO, A = smoothness - matches the URP/Lit `_MetallicGlossMap` convention |
| Normal maps | imported as `NormalMap` type |
| Production material set | `WWG_cowboy_{legs,torso,hands,head,colt,hair,holster,eye_sclera}` + package `mat_cowboy_eye.cornea` (transparent cornea, no map by design) |
| Missing / magenta materials after cleanup | **0** of 90 production materials |

**Residual, non-blocking:** the Fab listing states "It is desirable to use a shader with two-sided display of polygons in the model". The hair and eyelid cards are single sided and read black from behind - this is the author's note, not a texture loss.

---

## 3. Animation - inventory, root cause, fix

### 3.1 Inventory (project-wide)

| Source | Clips | Note |
|---|---|---|
| `Assets/Art/Characters/Bandit/Animations/Quaternius_UAL1_Standard.fbx` | 40 humanoid clips (Idle, Walk, Sprint, Jog_Fwd_Loop, Pistol_Idle_Loop, Pistol_Shoot, Pistol_Reload, Pistol_Aim_Neutral/Up/Down, Hit_Chest, Death01, ...) | the only animation source used by the enemy |
| `Assets/CowboyCharacter/Mesh/cowboy_character.fbx` | **no clips** - only the avatar and the 4 skinned meshes | all Cowboy motion is cross-rig retargeting |
| `Assets/Art/Characters/Bandit/Animations/Bandit_{Run,Walk,Death}_Grounded.anim` | 3 | player-side, not used by the enemy |
| Sandbox `.anim`/`.controller` | many | diagnostics only |

### 3.2 Root cause of "run animation exists, shooting while moving does not"

`Doomy.controller` was **single-layer**:

```
Idle -> Walk -> Run            (by Speed)
Idle/Walk/Run -> Aim           (by IsAiming)   <-- the whole body leaves locomotion
Aim -> Fire (trigger) -> Aim
Aim -> Reload (trigger) -> Aim
ANY -> Death (IsDead)
```

`DoomyAnimatorDriver` sets `IsAiming = playerCurrentlyVisible`, so as soon as the enemy saw the player it left the locomotion clip for the **standing** `Pistol_Idle_Loop`, and the legs stopped cycling while shooting. There was no mechanism to overlay a shooting pose on a moving body.

### 3.3 Fix

`Doomy.controller` is now two-layer:

| Layer | States | Mask |
|---|---|---|
| Base Layer | Idle, Walk, Run, Death | none (full body) |
| **UpperBody** | UpperBodyEmpty, **Aim** (`Pistol_Idle_Loop`), **Fire** (`Pistol_Shoot`), **Reload** (`Pistol_Reload`) | `Assets/Animations/Enemies/WWG_UpperBody_Mask.asset` |

* The mask is a **copy of the player's `WWG_UpperBodyMask.asset`** with the per-bone paths stripped, plus the **Cowboy spine chain added by transform path** and the **hips explicitly excluded**. That exclusion is the critical part: in the Cowboy rig the leg bones are children of the hips, so a mask that includes the hips (`Body`) also swallows the legs and freezes the gait. The first mask attempt included `Body` and reproduced exactly that failure.
* No new controller, no new clip, no AI change; `DoomyAnimatorDriver` is untouched.

### 3.4 Measured behaviour cycle

Probe: `Assets/_Sandbox/CowboyFix_2026-10-20/CowboyEnemyAnimationProbe.cs` (enemy prefab `Doomy_Bandit`, AI/colliders/physics disabled, `cullingMode = AlwaysAnimate`, `applyRootMotion = false`), CSV `Working/reports/cowboy_enemy_anim_probe.csv`.

| Phase | Base layer | Upper layer | footR swing | footL swing | hand Y |
|---|---|---|---|---|---|
| 1 idle | Idle | UpperBodyEmpty | 0.011 | 0.011 | 1.175-1.690 |
| 2 run | Idle -> Walk -> Run | UpperBodyEmpty | **0.320** | **0.450** | 1.148-1.448 |
| **3 run + aim** | **Run** | UpperBodyEmpty -> **Aim** | **0.304** | **0.429** | 1.118-1.350 |
| **4 run + aim + FIRE** | **Run** | Aim -> **Fire** | **0.290** | **0.429** | 1.161-1.316 |
| 5 aim standing | Run -> Walk -> Idle | Fire -> Aim | 0.323 | 0.105 | 1.297-1.635 |
| 6 reload | Idle | Aim -> **Reload** | 0.012 | 0.009 | 1.552-1.639 |
| 7 death | Death | -> UpperBodyEmpty | 1.140 | 1.152 | varies |

**The gait amplitude during "run + aim" and "run + aim + fire" equals the plain run amplitude (0.29-0.43 m vs 0.32-0.45 m)** - locomotion continues while the upper body aims and fires. Visual proof: `QA_Enemy_run_fire_close_2026-10-20.png` (character running, arms and torso in the shooting pose, skin natural).

### 3.5 Shake / transition check

| Check | Result |
|---|---|
| High-frequency jitter of hand/shoulder/weapon | none: the upper-body layer only changes state on a trigger; the per-frame bone deltas show no oscillation beyond the clip content (foot swing is a clean sinusoid) |
| Pose snap when AIM engages | the aim clip is entered with a 0.1 s blend (`Empty -> Aim`, duration 0.1) - no visible snap |
| Fire -> Aim return | exit time 0.95, 0.1 s blend |
| Layer flicker | layer weights constant at 1.00/1.00 across the whole cycle |
| Root/hips fighting | hips excluded from the mask, so the locomotion owns them |
| Weapon drift | `colt` is a skinned part of the same rig and follows the hand bone; no separate attachment to drift |

**Editor only.** The earlier phone-side handshake question (10.4 mm/frame on aim engage) was measured on the player rig and is not reproducible in the Editor for the enemy; device verification remains open.

---

## 4. Crate and cover materials - the real Blender sources

### 4.1 Blender measurement (background Blender, read-only)

| Source | Graph | Nodes | Images | Structure |
|---|---|---|---|---|
| `Working/blender_src/cover/Cover_Western_Wooden_Working.blend` | `MAT_cover_western_wood_metal` | **14** (Attribute `Col`, UVMap, Mapping 0.8/50, Noise, 2 ColorRamps 0.40-0.60 and 0.52-0.70, Math 0.83, VectorMath, 3 Mix, Bump 0.06/0.002) | **none** | `Cover_Fragments` 11 + intact + `QA_Rig` |
| `Working/blender_src/crate/Crate_01_Working.blend` | `MAT_crate_01_wood_metal` | **14**, same structure (Mapping 0.8/50, Math 0.83) | **none** | `Crate_Fragments` 10 + intact + `QA_Setup` |
| `Working/blender_src/fence/Fence_01_Working.blend` (reference) | `MAT_fence_01_wood_metal` | 14, Mapping 0.8/**60**, Math **0.78** | none | 14 fragments |
| `Working/blender_src/barrel/Barrel_01_Working.blend` (reference) | `MAT_barrel_01_wood_metal` | **33**, object-space coords, 2 noises + white noise, `Col` attribute | none | 13 meshes |

**Conclusion:** the crate and cover use the *same* vertex-colour procedural family as the fence - the wood pattern travels as the `Col` vertex-colour attribute and the grain is recreated in-shader. The project already ships the agreed transcription of these graphs in `Assets/Shaders/WWG_VertexColorWoodMetal.shader` (D-16A-MF-R2/R3). **No texture baking was performed**: baking would have created a second, competing material pipeline for the kit and broken the "one shared shader" contract, and the task explicitly forbids inventing a new ART pipeline.

### 4.2 Unity verification

| Check | Result |
|---|---|
| FBX vertex colours | present on every Modular Kit FBX (`Cover_Western_Wooden_r5`, `Cover_Western_Wooden_Fragments_r5`, `Crate_01_Intact_FINAL`, `Crate_01_Fragments_FINAL`, `Barrel_01_*_FINAL`, `Fence_01_*_FINAL`) |
| UVs | present on all of them |
| Material assignment | `MAT_WWG_WoodMetal_{Crate,Cover,Barrel,Fence}` on the canonical shader (GUID `0ca5079f…` = `Assets/Shaders/WWG_VertexColorWoodMetal.shader`) - identical to the approved barrel and fence |
| Parameter correspondence | crate/cover `_GrainMapScale (0.8, 50)` and `_WoodMulAdd 0.83` match the Blender graphs; fence `(0.8, 60)` / `0.78`; barrel uses `_GrainPath 1` (object space) - all consistent |
| Remaining mismatch found | the **crate** material had `_RustPath = 2` (generated-space rust) while the crate graph derives the rust ramp from the *same* UV noise as the fence - corrected to `_RustPath = 0` |
| Missing / magenta | 0 |

### 4.3 Destruction chain (crate and cover)

| Check | Crate_01 | Cover_01 |
|---|---|---|
| `State_Intact` / `State_Destroyed` | yes | yes |
| Fragments | 10, each with `BoxCollider` + `Rigidbody` (mass 2) | 11, colliders and rigidbodies present |
| Fragment colliders enabled before destruction | **were enabled (10) - defect, now disabled** like cover/barrel/fence | disabled |
| `TakeDamage(999)` | `Intact -> Destroyed`, enabled colliders 1 -> 11, 10 fragment renderers visible, 10 rigidbodies dynamic | same structure |
| Fragment material | `MAT_WWG_WoodMetal_Crate` on all 10 fragments - no material lost on the state switch | `MAT_WWG_WoodMetal_Cover` |

---

## 5. Two-tier cover sections

### 5.1 Composition (per section)

| Template | Lower tier | Upper tier | Fence |
|---|---|---|---|
| `CrateSection` | 4 crates, 2 x 2 grid, 0.02 m gaps | **4 barrels** standing on the crates (y = 0.757) | 3 panels (wall + 2 ends), room side open |
| `CrateBarrelSection` | 4 crates | **3 crates + 1 barrel** | idem |
| `BarrelSection` | 4 barrels | **4 barrels** (y = 0.900) | idem |
| `WagonCamp` (wagon missing in the scene) | falls back to 4 crates | - | idem |

Measured prefab dimensions used for the layout: crate 1.000 x 0.757 x 0.750, barrel 0.604 x 0.900 x 0.574, fence 1.960 x 1.200 x 0.160. Upper items rest exactly on the lower tier (no floating) and every upper footprint is smaller than or equal to the support below it (barrel 0.604 x 0.574 on crate 1.000 x 0.750; crate on crate identical; barrel on barrel identical).

### 5.2 Measured result (Play Mode, `Physics.ComputePenetration`)

| Metric | Before | After |
|---|---|---|
| Sections | 43 | **44** (4-6 per room: 4,4,5,5,6,5,6,5,4) |
| Props (non-fence) | 215 | 348, of which **172 in the upper tier** |
| Fence panels | 43 (1 per section) | 130 (3 per section) |
| **Real overlaps (depth > 2 mm)** | **38** | **0** |
| Resting contacts (upper tier on lower tier, depth <= 2 mm) | - | 158 (by design) |
| Passage between the section and the wall | 0.54 - 1.01 m | **1.22 - 1.72 m** (min / avg 1.46) |
| Sections with two tiers | 0 | **44 / 44** |

### 5.3 Defects found and fixed in the placement mechanism

1. **Inverted sign in the wall-clearance compensation** (`ArenaGenerator.BuildCoverClusters`): for a wall at `z < 0` the section was moved *towards* the wall by `0.98*sin|yaw|` instead of away from it. Corrected for both axes; the passage is now yaw-independent.
2. **Crate overlap inside a section**: the old layout placed two crates 0.9 m apart while the crate is 1.0 m wide - a 0.1 m interpenetration per row, the actual source of the "38 overlaps". The new grid uses 1.02 m / 0.80 m spacing (0.02-0.05 m gaps).
3. **Fence/prop interference**: the wall-side panel overlapped the crates by 0.085 m - moved to `z = -0.08`; the end panels were moved to `x = +/-1.13` and `z = 0.98` so the corners meet by contact, not by intersection.
4. `clusterWallRun` 9 m -> **18 m** and `clusterWallInset` 0.35 m -> **1.4 m** (scene values) to reach the owner's target of ~4 sections per room and to open the passage along the wall. `clusterSpacing` set to 7.0 m to match the code default.

Screenshot: `QA_Section_two_tier_2026-10-20.png` (4 barrels on 4 crates inside the picket fence, passage along the wall).

---

## 6. Asset cleanup

### 6.1 Reference map

263 reference files (scenes, prefabs, materials, controllers, ScriptableObjects) were scanned for the GUID of every FBX / prefab / material under `Assets` (excluding `_Sandbox`). Result: **55 assets with zero inbound references**.

### 6.2 Archived to `Working/archive_assets/2026-10-20/` (with `.meta`, GUIDs preserved for restore)

34 assets:

| Group | Files |
|---|---|
| Legacy Doomy (superseded by the Cowboy) | `Doomy_Repaired.fbx`, `Doomy_Repaired_ProductionSource.fbx`, `WWG_Doomy_Foundation.fbx`, `Doomy.prefab`, 9 `Doomy_*` materials, `Doomy_Placeholder.prefab` |
| Old cover revisions | `Cover_Western_Wooden_{FINAL,r2,r3,r4}.fbx`, `Cover_Western_Wooden_Fragments_{r1,r2,r3}.fbx` (r3 contains **no mesh at all**) |
| Old barrel exports | `Barrel_01_{Intact,Intact_r2,Fragments,Fragments_r2}.fbx` |
| Unused modular revisions | `FloorSegment_01_{FINAL,r2}.fbx`, `WallSegment_01_{r2,r3}.fbx` |
| Cowboy | `cowboy_character_v3.prefab` (production uses v2) |
| Misc | `Western_Plank_Wall_Segment.prefab`, `WWG_Quaternius_Western_Shirt_v003.fbx` (feasibility test), `M_Ground_Desert.mat`, `Terrain_Old.mat` |

**Kept, marked `REVIEW`** (libraries, not duplicate versions): 15 ADG plank materials, the Innerverse skybox material, 2 TMP font materials.

**Kept as production:** `Crate_01_{Intact,Fragments}_FINAL.fbx`, `Cover_Western_Wooden_r5.fbx`, `Cover_Western_Wooden_Fragments_r5.fbx`, `Barrel_01_*_FINAL.fbx`, `Fence_01_*_FINAL.fbx`, `FloorSegment_01_r3.fbx`, `WallSegment_01_FINAL.fbx`, `cowboy_character.fbx`, `cowboy_character_v2.prefab`.

### 6.3 Post-cleanup verification

| Check | Result |
|---|---|
| Prefabs with missing components | **0** |
| Production materials with missing shader / magenta | **0 of 90** |
| Production assets still present | all |
| Deletions | **none** - everything was moved with its `.meta`; no `git clean`, no mass delete, no user files touched |

---

## 7. Licence - Cowboy Elderly

| Item | Verified value |
|---|---|
| Listing | `https://www.fab.com/listings/f6c625e6-c08b-47d2-bc96-cb6da7fe8a23` |
| Price | **Free** |
| Licence | **Fab Standard License** (`https://www.fab.com/eula`) |
| May | use commercially or privately; modify; distribute projects commercially with the asset; use with any compatible tool; share with collaborators on the project |
| May not | resell or redistribute the asset standalone for free |
| Revenue threshold | **not engaged** - "Both pricing tiers (Personal and Professional) grant you the same scope of rights. There is no need for upgrading from Personal to Professional tier if you cross the revenue threshold after the purchase." The asset is Free in any case |
| Credit | not required |
| AI usage | allowed ("Allows usage with AI: Yes") |
| Local copy | owner-supplied `cowboyelderly.unitypackage` (508.8 MiB), identity match with the listing verified in `ART-0004` §6.1.4 (13 511 faces / 14 103 verts / 26 461 tris) |

**Recorded in `Documentation/Art/ART-0004-THIRD-PARTY-ASSET-CATALOG.md` §10.** Named gap: the listing's interactive "License terms" panel is rendered client side and could not be captured as text in this environment; the Fab Standard License summary page is used as the binding text.

---

## 8. Files changed

| Path | Change |
|---|---|
| `Assets/Art/Characters/Doomy/Animation/Doomy.controller` | second `UpperBody` layer (Aim / Fire / Reload / UpperBodyEmpty) with the masked upper body; Aim/Fire/Reload removed from the base layer |
| `Assets/Animations/Enemies/WWG_UpperBody_Mask.asset` (+ `.meta`, new GUID `2e1945c12536493dab77cd1e1d2a02a5`) | humanoid upper-body mask + the Cowboy spine chain, hips excluded |
| `Assets/Scripts/Procedural/ArenaGenerator.cs` | two-tier sections, 3-sided fence, corrected wall-clearance sign, new radii/extents, `clusterWallRun` default 18 |
| `Assets/Prefabs/Environment/Crate_01.prefab` | 10 fragment colliders disabled; fragments kinematic |
| `Assets/CowboyCharacter/Materials/*` | re-bound to the original TGA/PSD after the reverted channel experiment |
| `Assets/Scenes/TestArena.unity` | generator parameters only: `clusterWallInset` 0.35 -> 1.4, `clusterWallRun` 9 -> 18, `clusterSpacing` 5.5 -> 7.0 |
| `Assets/_Sandbox/CowboyFix_2026-10-20/*` | probe + repair scripts (sandbox, not production) |
| `Working/archive_assets/2026-10-20/**` | 34 archived assets with `.meta` |
| `Working/reports/cowboy_enemy_anim_probe.csv` | per-frame animation evidence |
| `Documentation/INTEGRATION_ROADMAP.md` | §16 |
| `Documentation/Art/ART-0004-THIRD-PARTY-ASSET-CATALOG.md` | §10 |

**Not touched:** `EnemySpawner.cs`, `ArenaTacticalMap.cs`, `CoverSystem.cs`, `CoverPoint.cs`, `PlayerController.cs`, `PlayerHealth.cs`, `EnemyController.cs`, `DoomyAnimatorDriver.cs`, `BanditController.controller`, the protected A*, the `.blend` sources, the FBX contract, `CP2`/`DEC-05`/`DEC-06`.

---

## 9. Open items

1. **Device verification** - no Android device; all runtime results are `EDITOR VERIFIED`.
2. **Licence panel text** - the listing's interactive terms could not be captured (documented in `ART-0004` §10.2).
3. **Single-sided hair cards** - the author recommends a two-sided shader; the hair reads black from behind. A dedicated two-sided material is the candidate fix (not applied: it would change the character's look without an owner decision).
4. **Legacy `Doomy.prefab`** - archived together with `Doomy.fbx`; it is the only asset that used the old rig, and it shared `Doomy.controller` (now two-layer). The controller change is compatible with it, but the prefab itself is out of production.
5. **`CP2` / `DEC-05` / `DEC-06`** - deliberately left open; the crate now physically breaks into 10 fragments, but the registry statuses are the owner's act.
6. **Section density in big rooms** - 5-6 sections in the larger rooms (target was "≈4"); the count is perimeter driven (R1) and the rooms allow it. If exactly 4 is required everywhere, `clusterWallRun` must rise to ~26 m.
7. **Third-party libraries** marked `REVIEW` - not duplicates, left in place.

---

## 10. Android build (final)

| Item | Value |
|---|---|
| APK | Builds/Android/WildWestGunslinger_CowboyAnim_TwoTier_2026-10-20.apk |
| Size | **509.6 MB** |
| SHA-256 | 66D231D84771552094FC5C39D52E225A49F1E737082EC467328A84E64A617338 |
| Build | **succeeded**, 198.55 s, **0 errors**, 1014 warnings (pre-existing project warnings) |
| Contains | Cowboy Elderly model + two-layer enemy controller, two-tier cover sections, crate destruction, cleaned asset set |
| Status | **BUILD VERIFIED** - this is the build to test on a device. The previous APK WildWestGunslinger_CowboyCovers_2026-10-20.apk (SHA 386E084F…) **predates** the animation, section and cleanup work and must not be used for the current check |
| Device | **NOT VERIFIED** - no Android device available |

Build note: the first attempt failed with 42 compile errors because the two sandbox *editor* scripts sat in a runtime folder (Assets/_Sandbox/CowboyFix_2026-10-20/) and therefore had no access to UnityEditor. They were moved to .../Editor/ (GUIDs preserved), which excludes them from the player build.

## 11. Final state summary

| Layer | Status |
|---|---|
| Cowboy materials / skin | EDITOR VERIFIED |
| Enemy animation (incl. run + fire) | EDITOR VERIFIED |
| Shake / transition check | EDITOR VERIFIED (device pending) |
| Crate / cover materials and destruction | EDITOR VERIFIED |
| Two-tier sections, passages, overlaps | EDITOR VERIFIED |
| Asset cleanup | EDITOR VERIFIED (0 broken references) |
| Licence record | PASS (documentation) |
| Android build | BUILD VERIFIED |
| Android device | NOT VERIFIED |