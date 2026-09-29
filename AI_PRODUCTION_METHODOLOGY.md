WildWestGunslinger — AI Production Methodology

Document type: Production / Agent Execution Methodology
Version: 1.0
Date: 2026-09-28
Origin: ChatGPT project orchestration + validated OpenCode/Blender workflow
Execution model: ChatGPT = orchestrator / reviewer; OpenCode = executor
Primary tools: OpenCode + Blender MCP + Unity MCP when available

---

1. PURPOSE

This document defines the reusable methodology for AI-assisted production work in the WildWestGunslinger project.

It is not a project-state document and is not a replacement for the project roadmap.

Its purpose is to define how OpenCode must safely investigate, modify, validate, document and finalize project assets and systems.

The methodology is intended to be reusable across:

- Character Foundation
- Human Character Dummy
- Character Equipment
- Weapons
- Enemies
- Environment
- Destructible Assets
- Blender Assets
- Unity Integration
- Future production pipelines

The methodology was developed through practical project work and subsequently refined using successful Barrel, Fence and Crate production workflows.

---

2. CORE PRODUCTION MODEL

The intended production model is:

«Understand once → establish a verified baseline → work consistently within that baseline → verify each result → audit everything → obtain human approval → record the approved result in Git.»

The system deliberately avoids two extremes:

Extreme A — Blind Agent Execution

The agent begins modifying the project based on incomplete context or assumptions.

This is prohibited.

Extreme B — Full Re-Read Before Every Small Operation

The agent repeatedly rereads the entire project and all documentation before every stage.

This wastes context and increases the possibility of inconsistent reinterpretation.

Preferred Model

Use:

- Initial Recon
- Project Knowledge Baseline
- Stage Readiness Check
- Targeted Re-Verification when necessary
- Controlled production stages
- Checkpoints
- Numeric QA
- Visual QA
- Final Audit
- Human Approval
- Separate Git Commit

---

3. EXECUTION ROLES

3.1 ChatGPT

ChatGPT acts as:

- orchestrator;
- specification author;
- reviewer;
- decision-support layer;
- approval-gate coordinator;
- requirements interpreter;
- final human-facing report reviewer.

ChatGPT does not replace factual project inspection.

If the current project state contradicts remembered context:

«Actual project state takes priority.»

---

3.2 OpenCode

OpenCode acts as:

- project executor;
- repository inspector;
- Blender operator;
- Unity operator when MCP is available;
- report generator;
- validation executor.

OpenCode must not invent missing information.

When a required fact cannot be established:

«UNKNOWN»

or

«NOT VERIFIED»

If the uncertainty can affect safety or correctness:

«STOP»

---

3.3 User

The user is the final authority for:

- visual approval;
- acceptance of design variants;
- scope changes;
- destructive operations;
- final asset approval;
- Git commit approval;
- remote push approval.

The user may explicitly approve or reject a checkpoint.

---

4. SOURCE OF TRUTH

Use the following priority:

1. Actual current project state
2. Current local project documentation
3. Project Knowledge Baseline
4. Verified tool/MCP data
5. Git recovery/reference information
6. Agent memory/context

Git is not automatically the source of truth for current project documentation.

If local documentation represents the currently accepted state and Git contains an older version:

«Local current documentation has priority.»

If the correct state cannot be determined:

«STOP — DOCUMENTATION CONFLICT»

---

5. INITIAL PROJECT RECON

Before a major production pipeline begins, perform one full Initial Recon.

Initial Recon must establish:

- project state;
- documentation;
- roadmap;
- current stage;
- Character Foundation;
- relevant assets;
- Blender;
- Unity;
- MCP;
- OpenCode;
- skills;
- Git state;
- protected systems;
- naming conventions;
- paths;
- visual baseline;
- production constraints.

Initial Recon happens once per working session unless a Targeted Re-Verification becomes necessary.

---

6. PROJECT KNOWLEDGE BASELINE

After Initial Recon, create a verified working baseline when appropriate.

Recommended path:

"Working/reports/project_reconnaissance_baseline.md"

The Baseline must contain only verified facts.

It may include:

- project version;
- Unity version;
- Blender version;
- relevant packages;
- documentation discovered;
- Character Foundation state;
- Dummy state;
- rig;
- skeleton;
- scale;
- orientation;
- geometry;
- materials;
- skills;
- MCP capabilities;
- Git recovery state;
- protected systems;
- naming/path rules;
- visual baseline;
- current task;
- known conflicts;
- unknowns.

Important facts should include their source.

The Baseline is a session working reference.

It does not replace the actual project or permanent project documentation.

---

7. CONTEXT PERSISTENCE

Related stages should preferably be executed within the same OpenCode session/context.

Benefits:

- Initial Recon is not repeated unnecessarily;
- verified facts remain available;
- MCP discovery is not repeated;
- skill inventory is not repeated;
- Dummy reconnaissance is not repeated;
- context consumption is reduced;
- contradictory reinterpretations are reduced.

However:

«Agent context is never the sole source of truth.»

If context is lost, compacted or becomes unreliable:

1. consult the Baseline;
2. consult current local documentation;
3. inspect the actual project if required;
4. perform Targeted Re-Verification.

Never reconstruct a critical technical fact from memory.

---

8. STAGE READINESS CHECK

Before each production stage, do not repeat the entire Initial Recon.

Perform a short readiness check.

Verify:

- Baseline exists;
- required requirements are known;
- required skills are available;
- required skill documentation has been read;
- required MCP capability is available;
- required Dummy/Character Foundation facts are known;
- output paths are available;
- target files are safe to create;
- previous checkpoint has required approval;
- no new conflict exists;
- recovery state is still valid;
- current stage is within approved scope.

Result:

"READY FOR E#"

or

"STOP — TARGETED VERIFICATION REQUIRED"

---

9. TARGETED RE-VERIFICATION

Full Recon should not be repeated unless necessary.

Targeted verification is required when:

- project state changed;
- Blender changed;
- Unity changed;
- MCP changed;
- a capability disappeared;
- context was lost;
- an important fact cannot be confirmed;
- documentation conflict appeared;
- a new asset type introduces unknown requirements;
- an existing file unexpectedly changed;
- Git recovery state became uncertain;
- a technical fact requires direct confirmation.

Only the necessary area should be rechecked.

---

10. EVIDENCE-BASED EXECUTION

Important technical facts require evidence.

Acceptable evidence includes:

- file path;
- MD heading;
- object name;
- Blender data path;
- Unity component;
- concrete measurement;
- MCP capability result;
- Git commit hash;
- screenshot;
- render;
- direct inspection result.

Examples:

Bad:

«"The model probably uses the correct rig."»

Good:

«"Armature object X contains Y bones and the verified Head bone is Z."»

If the fact cannot be verified:

«UNKNOWN / NOT VERIFIED»

---

11. NO INFERENCE

Do not convert assumptions into facts.

Examples of prohibited assumptions:

- assuming an armature is humanoid because its name says Humanoid;
- assuming a socket exists because another asset uses one;
- assuming an MCP capability exists because the MCP theoretically supports it;
- assuming FBX settings behave identically across Blender versions;
- assuming an asset is approved because it looks finished.

The correct response is:

«VERIFY»

or

«UNKNOWN»

or

«STOP»

---

12. EXISTING FILE PROTECTION

Existing project files are protected by default.

"CREATE" means:

«create a new file that does not already exist.»

If a target already exists:

«it is no longer a simple CREATE operation.»

Modification or overwrite requires explicit authorization.

Do not silently:

- overwrite;
- replace;
- rename;
- move;
- delete;
- regenerate;
- re-export over an existing approved file.

---

13. PROTECTED ASSET PRINCIPLE

Approved or integrated assets must be treated as protected.

They should not be:

- modified during experiments;
- used as disposable working copies;
- replaced without authorization;
- renamed;
- moved;
- deleted;
- used as uncontrolled bases for unrelated experiments.

New experiments should use explicitly authorized working files.

This principle is especially important for:

- approved character assets;
- approved environment props;
- integrated prefabs;
- final FBX assets;
- approved Blender source files.

---

14. NO UNAUTHORIZED COPIES

Do not create arbitrary backup or duplicate copies of protected assets.

Prohibited unless explicitly authorized:

- duplicate ".blend" files;
- duplicate FBX files;
- duplicate Unity assets;
- temporary protected-asset copies;
- renamed backup variants.

Allowed:

- read-only inspection;
- in-memory analysis;
- explicitly authorized new working files;
- explicitly authorized validation scenes.

---

15. REVERSIBILITY

Every modification must be reversible.

Before modifying a project:

- a verified recovery point should exist;
- the change scope should be known;
- the expected modified files should be known.

If no verified recovery point exists:

«STOP — NO VERIFIED RECOVERY POINT»

A future final Git commit does not replace a pre-change recovery point.

---

16. GIT POLICY

Git has two separate purposes:

Recovery Checkpoint

Created or verified before destructive/modifying work.

Purpose:

«restore the state before modifications.»

Final Git Commit

Created after:

- production;
- validation;
- Final Audit;
- human approval.

Purpose:

«record the approved result.»

These are not the same operation.

---

17. GIT IS NOT AUTOMATIC

OpenCode must not automatically:

- commit;
- push;
- amend commits;
- reset;
- force push;
- stage unrelated files;
- delete unrelated files.

Git should be a separate task after human approval.

Remote push requires separate explicit authorization.

---

18. CHECKPOINT PRODUCTION MODEL

Major assets should be developed through controlled checkpoints.

A generic asset pipeline can use:

CP1 — Construction / Blockout / Foundation

Establish:

- dimensions;
- construction;
- silhouette;
- object structure;
- major topology;
- material concept;
- technical constraints.

No final export unless explicitly authorized.

CP2 — Production / Controlled Finalization

After CP1 approval:

- final geometry;
- topology;
- materials;
- UV;
- deformation;
- controlled separation where required;
- final QA;
- export.

CP3 — Validation / Final

Where required:

- re-import;
- reassembly;
- strict separation;
- measurements;
- visual QA;
- final reports.

Not every asset requires identical checkpoints.

The checkpoint structure must match the asset.

---

19. VISUAL EXPLORATION CHECKPOINT

For visually ambiguous assets, use a dedicated exploration checkpoint.

Example:

1. Create 2–3 blockout variants.
2. Show front/side/top views.
3. Do not finalize topology.
4. Do not perform final UV.
5. Do not export.
6. Stop.
7. User selects a variant.

Only after selection should production continue.

This is particularly useful for:

- hats;
- clothing;
- weapons;
- character accessories;
- visual design variants.

---

20. CONSTRUCTION-FIRST MODELING

For physical props and destructible assets, model according to real or logically constructed parts.

Do not generate arbitrary geometry merely because it looks correct from one camera.

Examples:

A barrel should be understood as:

- staves;
- bands;
- heads;
- rivets.

A fence should be understood as:

- posts;
- rails;
- boards;
- nails.

A crate should be understood as:

- posts;
- boards;
- lid;
- base;
- hardware.

This allows controlled destruction, reassembly, collision logic and future gameplay integration.

---

21. CONTROLLED DESTRUCTION

When an asset is intended to be destructible:

«Use construction-faithful controlled separation.»

Do not use arbitrary fragmentation merely to produce many pieces.

Preferred approach:

- separate real construction components;
- preserve logical relationships;
- keep hardware attached to its host where appropriate;
- create meaningful fragments;
- avoid excessive fragment count;
- avoid unnecessary physics objects.

Cell Fracture/Voronoi-style arbitrary fragmentation is not the default methodology.

If no dedicated fracture capability exists:

«use controlled component separation / BMesh / Boolean / Bisect / Separate / Join only where technically justified.»

---

22. MODULAR CHARACTER EQUIPMENT

Character equipment should be treated as modular systems rather than isolated meshes.

Typical layers:

- body;
- shirt;
- vest;
- jacket/coat;
- trousers;
- boots;
- hat;
- belt;
- holster;
- gloves;
- weapons;
- accessories;
- sockets.

Each layer should have a clearly documented relationship to:

- skeleton;
- attachment point;
- skinning;
- materials;
- scale;
- Character Foundation.

Do not redesign the Character Foundation inside an equipment task unless explicitly authorized.

---

23. ATTACHMENT METHOD MUST BE VERIFIED

Do not assume an attachment method in advance.

Possible methods include:

- parent to Head;
- parent to bone;
- armature deformation;
- single-bone influence;
- SkinnedMeshRenderer-compatible workflow;
- another verified project-compatible approach.

The selected method must be based on the actual Character Foundation.

Document:

- chosen method;
- technical reason;
- verified source.

---

24. SKINNING AND DEFORMATION

For clothing and character equipment:

Verify:

- skeleton;
- bone hierarchy;
- relevant bones;
- vertex groups;
- weights;
- deformation;
- clipping;
- extreme poses.

Do not assume that visually fitting the clothing in the rest pose is sufficient.

At minimum, test relevant movement such as:

- idle;
- walk;
- head rotation;
- weapon pose where applicable;
- expected gameplay deformation.

---

25. ANDROID PERFORMANCE PRINCIPLE

Visual quality must not be achieved by uncontrolled object proliferation.

Prefer:

- low object count;
- low renderer count;
- shared materials where appropriate;
- modular meshes;
- reusable prefabs;
- instancing/batching where beneficial;
- simple colliders;
- controlled texture resolution;
- reasonable triangle budgets.

Avoid:

«one plank = one GameObject»

or:

«one small decorative element = one renderer»

unless there is a concrete gameplay requirement.

Visual richness should come from:

- silhouette;
- modular construction;
- material variation;
- controlled vertex attributes;
- reusable variants.

---

26. MODULAR ENVIRONMENT PRINCIPLE

For modular walls/floors and similar environment systems:

Do not build the final procedural environment from hundreds of separate primitive GameObjects.

Preferred architecture:

Wall Segment

Approximately one reusable segment containing multiple horizontal logs in one mesh.

Target runtime representation:

- 1 mesh;
- 1 MeshRenderer;
- 1 shared material;
- 1 simple collider.

Floor Segment

One reusable mesh containing multiple floorboards/planks.

Target runtime representation:

- 1 mesh;
- 1 MeshRenderer;
- 1 shared material;
- 1 simple collider.

Corridors and rooms should reuse these modules.

---

27. MATERIAL STRATEGY

For mobile assets, use controlled material complexity.

Where the project specification requires M2:

- one material slot;
- simple PBR;
- vertex attributes for controlled variation;
- wood/metal differentiation through vertex color and/or masks where appropriate.

For exported Blender assets:

"Col" should use:

"FLOAT_COLOR / CORNER"

rather than:

"BYTE_COLOR"

when preserving intended linear values is important.

Do not introduce procedural Blender shader complexity when the target pipeline cannot reliably preserve it through FBX.

Unity-side production materials may be authored later as a separate integration step.

---

28. UV PRINCIPLES

UVs must be validated rather than assumed.

Check:

- coverage;
- orientation;
- scale;
- seams;
- appropriate grain direction;
- no accidental overlap where prohibited;
- compatibility with the intended material workflow.

For wood:

«grain direction should follow the physical construction.»

Examples:

- barrel staves → vertical grain;
- wall logs → longitudinal grain;
- floorboards → longitudinal grain;
- crate boards → appropriate board direction.

---

29. EXPORT CONTRACT

Export settings must be treated as a technical contract.

Current project methodology prefers:

- explicit selection;
- scale 1.0;
- FBX scale none;
- no unnecessary bake-space transformation;
- no embedded textures when the project material pipeline does not require them;
- EEVEE-compatible workflow;
- no unnecessary add-ons;
- verified Blender version behavior.

Do not blindly assume a setting.

Verify actual Blender/MCP behavior when needed.

---

30. FBX RE-IMPORT VALIDATION

A successful export does not prove a successful asset.

Where applicable, validate the exported FBX by re-importing it.

Check:

- scale;
- orientation;
- object count;
- mesh integrity;
- material slot count;
- UV;
- vertex attributes;
- triangle count;
- separation;
- attachment;
- visual appearance.

If re-import behavior differs from the source Blender scene:

«document the difference and determine whether it affects the production contract.»

---

31. FLAT SHADING / SMOOTHING

When flat shading is required by the project specification:

- verify source mesh shading;
- verify export behavior;
- verify re-import behavior.

Do not silently change the frozen export contract merely to compensate for an unresolved re-import behavior.

If FBX re-import changes shading behavior:

«document the issue and defer pipeline-specific correction to the appropriate validation stage.»

---

32. NUMERIC QA

Numeric QA verifies measurable technical requirements.

Typical checks:

- triangle count;
- object count;
- renderer count where applicable;
- dimensions;
- origin;
- scale;
- bounding box;
- material slots;
- UV coverage;
- non-manifold geometry;
- loose geometry;
- duplicate geometry;
- unwanted intersections;
- bone count;
- vertex groups;
- weight validity;
- fragment count;
- fragment dimensions;
- reassembly deviation.

A numeric requirement is either:

"PASS"

or

"FAIL"

or

"NOT VERIFIED"

Do not convert an approximate result into PASS without evidence.

---

33. VISUAL QA

Visual QA verifies:

- silhouette;
- proportions;
- construction;
- style;
- material appearance;
- fit;
- clipping;
- attachment;
- readability;
- consistency with approved references;
- absence of unintended visual artifacts.

Required views depend on the asset.

Typical views:

- front;
- side;
- rear;
- top;
- close-up;
- assembled;
- separated;
- gameplay/context view where appropriate.

---

34. PASS MODEL

For production assets:

«Numeric PASS + Visual PASS = Production PASS»

Neither is sufficient alone.

A model can be technically valid but visually wrong.

A model can look correct but contain technical failures.

Both must pass.

---

35. FINAL STATE IS NOT "EXPORT SUCCESS"

An asset is not considered complete merely because:

- Blender generated a mesh;
- the file was saved;
- FBX exported successfully;
- the object looks good in one viewport.

Completion requires the applicable combination of:

- approved design;
- valid construction;
- numeric QA;
- visual QA;
- material/UV validation;
- export validation;
- re-import validation where required;
- correct reports;
- correct file scope;
- human approval.

---

36. BATCH UNITY INTEGRATION

When multiple related Blender assets are being developed:

«Unity integration should normally occur as a batch after the planned Blender asset set is complete.»

Do not repeatedly integrate every intermediate model into Unity unless the task explicitly requires an early integration test.

Benefits:

- fewer import cycles;
- less intermediate cleanup;
- consistent material setup;
- consistent prefab setup;
- easier final validation;
- clearer Git changes.

Early integration is allowed only when a technical question cannot be answered without it.

---

37. CHECKPOINT REPORT STANDARD

Every checkpoint report should contain:

1. STATUS
2. ACTIONS
3. CREATED FILES
4. MODIFIED FILES
5. DELETED FILES
6. SOURCE / EVIDENCE
7. MEASUREMENTS
8. CAPABILITIES
9. VALIDATION
10. PROBLEMS
11. UNKNOWN / NOT VERIFIED
12. NEXT GATE

Possible statuses:

- READY
- STOP
- APPROVED
- PASS
- FAIL
- UNKNOWN
- NOT VERIFIED
- FINAL

---

38. CHANGE SCOPE

Every production stage should define its expected scope.

At minimum:

- allowed directories;
- expected new files;
- expected modified files;
- prohibited files;
- protected assets;
- out-of-scope systems.

After execution, compare actual changes against expected scope.

Unexpected changes require investigation.

---

39. STOP CONDITIONS

OpenCode must stop when:

- documentation conflict appears;
- required parameter is unknown;
- required MCP capability is unavailable;
- required skill documentation is missing;
- destructive ambiguity appears;
- target file unexpectedly exists;
- protected asset would need modification;
- Git recovery point cannot be verified;
- scope expands;
- operation cannot be safely reversed;
- checkpoint approval is missing;
- actual project state contradicts the Baseline;
- context loss makes a critical fact unverifiable.

Do not improvise around a STOP condition.

---

40. FINAL AUDIT

After a major pipeline:

perform a separate read-only Final Audit.

The audit must verify:

Documentation

- reports exist;
- reports correspond to actual work;
- current documentation is consistent.

Assets

- only approved assets were created;
- protected assets remain unchanged;
- no unexpected duplicates exist.

Blender

- source files are valid;
- geometry is valid;
- materials are valid;
- UV is valid;
- export is valid.

Unity

Where applicable:

- imports are valid;
- prefabs are valid;
- components are correct;
- no unrelated systems changed.

Scope

- no unrelated changes;
- no unauthorized deletions;
- no unauthorized replacements.

Git

- initial recovery state is recorded;
- final state is accurately identified;
- no unrelated changes are included.

After Final Audit:

«STOP»

Human review follows.

---

41. HUMAN APPROVAL GATE

ChatGPT + user review:

- checkpoint reports;
- screenshots;
- renders;
- numeric QA;
- visual QA;
- FBX validation;
- Final Audit;
- change scope.

The user decides:

- approved;
- changes required;
- files to retain;
- files to reject;
- whether the asset is FINAL.

No final Git commit occurs before this decision.

---

42. FINAL GIT TASK

The Git task is separate.

It receives an explicitly approved file list.

It must:

1. verify current Git state;
2. show changes;
3. stage only approved files;
4. create one descriptive commit;
5. report commit hash;
6. stop.

It must not automatically:

- stage unrelated files;
- delete files;
- amend previous commits;
- reset;
- force push;
- push remotely.

Remote push requires separate explicit approval.

---

43. CHARACTER EQUIPMENT PIPELINE

The methodology was initially formalized for:

- E0 — Initial Project Recon
- E1 — Hat
- E2 — Torso
- E3 — Legs / Feet
- E4 — Belt / Holster / Sockets
- E5 — Equipment Pipeline Validation

The same structure can be adapted to other production areas.

Example:

"E0 Recon → Baseline → Approval → E1 → E2 → E3 → E4 → E5 → Final Audit → Approval → Git"

The exact stages may change.

The methodology does not require every asset to use identical technical steps.

---

44. EXAMPLE CHARACTER EQUIPMENT GATES

E1 Hat

Possible checkpoints:

1. silhouette exploration;
2. user-selected variant;
3. final geometry/fit;
4. attachment validation;
5. UV/material;
6. export/re-import;
7. final approval.

E2 Torso

Possible checkpoints:

1. blockout;
2. clothing fit;
3. deformation;
4. final geometry;
5. UV/material;
6. export/re-import;
7. approval.

E3 Legs / Feet

Possible checkpoints:

1. blockout;
2. fit;
3. deformation;
4. final geometry;
5. validation;
6. approval.

E4 Belt / Holster / Sockets

Possible checkpoints:

1. placement;
2. attachment architecture;
3. socket verification;
4. weapon compatibility;
5. validation;
6. approval.

E5 Pipeline Validation

Verify:

- equipment replacement;
- shared skeleton;
- attachment;
- scale;
- material expectations;
- naming;
- folders;
- export;
- performance assumptions.

Goal:

«prove a reusable equipment pipeline rather than a collection of unrelated one-off models.»

---

45. APPLICATION TO ENEMIES

The same methodology applies to enemy character production.

Separate:

Character Asset

- body;
- clothing;
- equipment;
- rig;
- animations.

from:

Enemy Gameplay Systems

- EnemyController;
- tactical vision;
- hearing;
- searching;
- investigation;
- cover;
- flanking;
- group roles;
- waves;
- combat.

Do not silently combine character-production work with AI-system changes.

A model task should not modify gameplay AI unless explicitly authorized.

---

46. APPLICATION TO ENVIRONMENT

The same methodology applies to environment assets.

Example:

"Asset Recon → CP1 Construction → Approval → CP2 Production → Numeric QA → Visual QA → Export → Re-import → Final Audit"

For modular environment assets:

- preserve low GameObject count;
- preserve reusable construction;
- use shared materials where appropriate;
- use simple colliders;
- create reusable variants;
- avoid uncontrolled primitive proliferation.

---

47. APPLICATION TO DESTRUCTIBLES

Destructible assets require additional validation:

- construction-faithful fragments;
- fragment count;
- physical meaning;
- separation;
- strict separation;
- reassembly;
- reassembly bounding-box comparison;
- fragment visibility state;
- viewport/render visibility;
- export;
- re-import.

A common failure mode is simultaneous display of intact and reassembled fragment geometry.

If coincident surfaces produce shimmer/z-fighting:

«identify the visibility-state cause rather than changing geometry unnecessarily.»

Normal workflow:

- intact visible;
- fragments hidden in viewport;
- fragments may remain render-enabled when required by the production setup.

---

48. DOCUMENTATION POLICY

Project documentation must distinguish between:

Current State

What currently exists.

Specification

What should be created.

Methodology

How the work should be performed.

History / Changelog

What changed and why.

Do not mix these categories unnecessarily.

The existence of a successful workflow does not automatically mean the resulting asset is FINAL.

---

49. CRITICAL DISTINCTION: CP1 VS FINAL

A common production mistake is treating a completed construction checkpoint as a finished asset.

For example:

"Crate_01 — CP1 COMPLETE"

means:

- intact model exists;
- construction is validated;
- visual QA for CP1 may have passed;
- future controlled fracture was considered.

It does not mean:

"Crate_01 — FINAL DESTRUCTIBLE ASSET"

unless CP2/finalization and the applicable final QA have actually been completed and approved.

Documentation must preserve this distinction.

---

50. APPROVED VS COMPLETE

These terms must not be treated as interchangeable.

APPROVED

A human has explicitly accepted the current checkpoint/result.

COMPLETE

All required technical stages for that asset/task have been completed.

FINAL

The complete result has passed the required final validation and has been explicitly accepted as final.

A model may be:

"CP1 COMPLETE / AWAITING APPROVAL"

or:

"CP1 APPROVED / CP2 NOT DONE"

or:

"FINAL / APPROVED"

These states must remain explicit.

---

51. SUCCESS CRITERIA

A production pipeline is successful only when:

- Initial Recon completed;
- relevant documentation identified;
- relevant skills identified;
- required skill files read;
- required MCP capabilities verified;
- project baseline established;
- recovery point verified;
- protected systems identified;
- unknowns documented;
- scope defined;
- user approved production;
- each required stage completed;
- checkpoints passed;
- numeric QA passed;
- visual QA passed;
- export validated;
- re-import validated where required;
- Final Audit passed;
- user approved final result;
- Git records only the approved result.

---

52. FINAL PRINCIPLE

The most important rule is:

«Do not optimize for speed of generation. Optimize for verified, reversible, reproducible production.»

OpenCode should be able to explain:

- what it knew;
- where that information came from;
- what it changed;
- why it changed it;
- what it verified;
- what remains unknown;
- what is protected;
- what requires approval;
- what exactly will be committed to Git.

The production pipeline is therefore:

RECON → BASELINE → APPROVAL → CONTROLLED PRODUCTION → CHECKPOINT → NUMERIC QA → VISUAL QA → EXPORT → VALIDATION → FINAL AUDIT → HUMAN APPROVAL → GIT COMMIT

No step silently substitutes for another.
