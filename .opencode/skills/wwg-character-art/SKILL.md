---

name: wwg-character-art
description: Professional stylized western character art workflow for WildWestGunslinger. Preserve Kevin Iglesias as the anatomical source of truth, create natural human silhouettes, fitted clothing, believable western accessories, clean low-poly forms, controlled proportions, and mandatory multiview visual QA. Use for any WWG character modeling, revision, clothing, silhouette, hat, weapon attachment, or visual-quality task in Blender.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

# WildWestGunslinger Character Art Skill

## PURPOSE

This skill defines the visual quality bar for WildWestGunslinger characters.

The target is:

PROFESSIONAL STYLIZED LOW-POLY WESTERN GAME CHARACTER

The target is NOT:

* blocky primitive character
* cube-built human
* toy-like geometry
* crude placeholder
* random collection of geometric shapes
* clothing made from obvious boxes
* exaggerated proportions without artistic reason

LOW-POLY DOES NOT MEAN BLOCKY.

A successful model must still read as a believable human at first glance.

---

## SOURCE OF TRUTH

The Kevin Iglesias Human Character Dummy is the primary anatomical reference.

The WWG character must preserve the underlying human anatomy of Kevin unless a specific visual requirement explicitly justifies a change.

Preserve:

* overall height
* torso proportions
* shoulder width
* arm length
* leg length
* head scale
* neck position
* human silhouette
* natural body relationships

Do not rebuild the body from primitive cubes when an existing Kevin body can be retained.

Prefer modifying, clothing, accessorizing, and styling the existing anatomical foundation.

---

## VISUAL TARGET

The character should look like:

"the same human wearing stylized western clothing"

not:

"a new creature assembled from boxes"

Required qualities:

* readable shoulders
* readable chest
* natural waist
* believable pelvis
* human arms
* human legs
* clean hands
* believable feet/boots
* natural head-to-body ratio
* coherent silhouette from front, side, and 3/4 views

Geometry must be simple but intentional.

Use enough topology to preserve the silhouette.

Do not aggressively reduce geometry merely to chase a very low polygon count.

---

## CLOTHING RULES

Clothing must follow the underlying anatomy.

Do not make clothing as detached rectangular shells unless the reference intentionally demands a rigid object.

Preferred approach:

* fitted shell
* controlled thickness
* tapered forms
* clean edge definition
* small, deliberate overlaps
* silhouette-preserving folds
* readable material boundaries

Vest:

* follows chest
* follows shoulder width
* narrows toward waist
* does not behave like a rectangular box

Shirt:

* follows torso
* sleeves follow arms
* shoulders remain natural

Coat:

* follows torso
* controlled silhouette
* readable hem
* no giant rectangular slab

Trousers:

* follow legs
* preserve knee and ankle readability
* avoid cylindrical toy-leg appearance

Boots:

* follow actual foot shape
* readable toe and ankle
* not giant blocks

Shoulder protection:

* small and integrated
* must not turn into oversized cubes

---

## HEAD AND HAT

The head is a high-priority visual region.

The head must:

* remain human
* have natural scale
* connect correctly to the neck
* maintain the original Kevin proportions
* remain visually integrated with the body

The western hat must sit naturally on the head.

Check:

* crown size
* crown height
* brim width
* vertical placement
* side profile
* front profile
* clearance around forehead
* clearance around ears
* face visibility

Never allow the head to visibly protrude through the hat in an accidental way.

Never hide a bad fit by changing camera angle.

---

## WESTERN IDENTITY

Western identity should come from:

* hat
* shirt
* vest
* coat
* belt
* holster
* bandana/scarf
* boots
* weapon
* restrained accessories
* coherent western colors

The design should remain readable at gameplay distance.

Do not add decorative complexity merely to make the character look detailed.

---

## CHARACTER FAMILY

All five WWG characters belong to one visual family:

WWG_Player
WWG_Bandit
WWG_Rusher
WWG_Shooter
WWG_Tactical

They share:

* anatomical base
* visual quality level
* proportions
* shading language
* material language
* polygon philosophy

Differences should primarily come from:

* clothing
* color
* hat
* accessories
* weapon
* restrained role-specific silhouette accents

Do not accidentally make each character look like a different game's asset.

---

## LOW-POLY QUALITY RULE

Prioritize silhouette quality over raw triangle minimization.

Good low-poly:

* planar but intentional surfaces
* controlled bevels
* readable transitions
* clean proportions
* consistent facets
* no obvious primitive-box construction

Bad low-poly:

* cubes for body parts
* excessive angularity
* square limbs
* huge flat torso blocks
* rectangular clothing floating around the body
* crude head
* crude hands/feet

When choosing between slightly more geometry and visibly worse anatomy, preserve the better silhouette.

Current character scale around 10–11k triangles is acceptable for the current WWG foundation unless later profiling proves otherwise.

---

## MATERIAL LANGUAGE

Use clean stylized materials.

Preferred:

* matte or semi-matte
* simple color blocks
* controlled roughness
* subtle material separation
* coherent western palette

Avoid:

* noisy procedural textures
* excessive surface noise
* muddy texture overlays
* strong yellow filters
* fake cinematic grading
* overly realistic shaders

Geometry and silhouette must carry the character design.

---

## RIG PROTECTION

The existing character rig is protected.

Current valid foundation:

* 51-bone shared skeleton
* valid skinning
* valid head weights
* valid deformation
* valid attachment points

Do NOT rebuild or replace the skeleton unless explicitly instructed.

Do NOT casually change:

* bone names
* hierarchy
* armature structure
* vertex groups
* attachment point names

Protected attachments:

* WeaponPoint_R
* WeaponPoint_L
* HolsterPoint
* BackWeaponPoint

If geometry changes, preserve compatibility with the existing rig.

---

## VISUAL QA IS MANDATORY

Never declare a character visually finished after inspecting only one viewport angle.

At minimum inspect:

1. front
2. side
3. 3/4 front
4. 3/4 rear

Compare against the original Kevin reference.

Check:

* head/body ratio
* shoulder width
* torso taper
* arm proportions
* leg proportions
* hat placement
* clothing fit
* silhouette
* weapon placement
* obvious intersections
* floating geometry
* accidental clipping
* primitive-looking forms

---

## QUALITY GATE

Before declaring a character READY:

The model must pass this test:

"When placed beside Kevin, does this clearly look like the same human wearing a professionally designed stylized western outfit?"

If the answer is no:

DO NOT declare READY.

Do another refinement pass.

---

## REFINEMENT LOOP

When a visual result is rejected as poor, do NOT blindly repeat the same modeling operation.

First classify the problem:

* anatomy
* silhouette
* clothing fit
* head/hat
* proportions
* materials
* accessory placement
* topology
* overall visual language

Then:

1. preserve working parts
2. identify the weakest visual area
3. improve that area
4. re-render
5. compare again
6. repeat only when improvement is measurable

Do not introduce unrelated changes.

---

## IMPORTANT WWG RULE

A character can be technically correct and still visually unacceptable.

Technical validation is NOT equivalent to visual acceptance.

Passing:

* rig
* weights
* FBX
* attachments

does not automatically mean:

* good character design
* good silhouette
* good clothing
* good proportions
* professional appearance

Visual quality has its own acceptance gate.

---

## FINALIZATION

Do not overwrite the validated master destructively until a refinement pass has been visually verified.

When modifying WWG characters:

* preserve a recoverable baseline
* make controlled changes
* render proof views
* validate the result
* only then update the delivery FBX

For a multi-character task, perfect one character first.

Use WWG_Player as the visual master.

Only after Player reaches the approved visual quality should the same visual language be propagated to Bandit, Rusher, Shooter, and Tactical.
