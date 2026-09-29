AUDIT — SYSTEM DOCUMENTATION AND ACCESS CONTEXT

Session record — 2026-09-28

«Purpose: preserve the context and decisions from the current documentation/system audit session in a dedicated repository area.
This document records facts, decisions, conflicts, and the next safe actions. It does not claim unverified project states as current.»

---

1. Repository and access state

Repository:

- "noxt1/WildWestGunslinger"
- Default branch: "main"
- GitHub repository ID: "1386927322"
- Current visibility at the time of this record: Public
- GitHub connector currently has repository access and administrator-level repository permissions.
- Repository contents can currently be read through the connected GitHub integration.
- Repository write access through the current ChatGPT GitHub integration was attempted and rejected with HTTP 403.

Visibility test performed during this session

The repository was temporarily changed by the user from Public to Private.

Result:

- GitHub API access through the ChatGPT connector returned HTTP 404.
- Repository search through the connected GitHub integration returned no repository.
- This demonstrated that the current ChatGPT GitHub connection did not retain access after the repository became Private.

The user then changed the repository back to Public.

Current verification after returning to Public:

- Repository lookup succeeds.
- Repository is reported as "visibility: public".
- Repository permissions available through the connector metadata include "admin: true", "maintain: true", "pull: true", "push: true", "triage: true".

Important operational conclusion

GitHub repository visibility and ChatGPT connector authorization are separate.

Current safe assumption:

- Public repository: ChatGPT GitHub connector can access it.
- Private repository: current connector configuration cannot presently access it.
- OpenCode on the user's PC is a separate local Git/GitHub authentication path and should be configured independently.

No repository visibility change should be treated as an indication that OpenCode authentication is configured.

---

2. User requested preservation of the current audit context

The user explicitly requested that the context of the current audit conversation be preserved in Markdown inside the repository.

Dedicated folder requested:

"Audit всей системы документации"

This document belongs inside that folder.

The folder is intentionally separate from the canonical "AI_CONTEXT" documentation until reconciliation is complete.

---

3. Hard audit conclusion established before this session

The documentation audit compared the available project documentation sources and established the following hierarchy:

1. Actual Unity/Git working tree = factual implementation state.
2. GitHub "main" / "AI_CONTEXT" = current project memory/documentation available to OpenCode.
3. Explicit user confirmations = only authoritative source for "CONFIRMED".
4. ChatGPT Library "CHATGPT_CHECKLIST.md" = roadmap/checklist.
5. Character/Asset/Contract methodology developed later in conversation = specification that has not yet been fully canonicalized into GitHub.
6. Older Library Markdown files = historical/reference material.
7. Old chat snippets = secondary reference only.

Critical rule

Do not bulk synchronize, rewrite, or merge the documentation until factual reconciliation is completed.

No mass documentation rewrite was authorized by this audit.

---

4. Current reconciliation status

Safe global status established by the audit:

PROJECT STATE: RECONCILIATION REQUIRED

WRITE ACTIONS: BLOCKED for project-state correction until the factual comparison is complete.

DATA LOSS DETECTED: NO

GITHUB ACCESS: WORKS while repository is Public

LIBRARY FILES: AVAILABLE

SINGLE SOURCE OF TRUTH: NOT YET FORMED

The present document is a preservation record, not a declaration that reconciliation has been completed.

---

5. Main documentation sources found

Relevant GitHub "AI_CONTEXT" documents include:

- "README.md"
- "PROJECT_STATE.md"
- "ARCHITECTURE.md"
- "RULES.md"
- "CURRENT_TASK.md"
- "CONFIRMED_STATE.md"
- "CHANGELOG.md"
- "ART_PIPELINE.md"
- "VERIFICATION.md"
- "DEBUGGING.md"
- "SPEC_IDEAS.md"
- "AI_TOOLCHAIN_AUDIT.md"

There are additional repository files; the exact total should not be asserted without a fresh listing.

Relevant ChatGPT Library Markdown sources found during the audit include:

- "CHATGPT_CHECKLIST.md"
- "CHATGPT_PROJECT_STATE.md"
- "CHATGPT_WORK_QUEUE.md"
- "CHATGPT_CHECKLIST_before_STAGE19..."
- "Вставленная ​​уценка.md"

The Library sources have different historical status and must not be treated as equally current.

---

6. Important documentation conflicts

Stage 4.4.5

A major conflict exists.

GitHub documentation records Stage 4.4.5 as:

- "FROZEN / PAUSED"
- implementation not established as complete in the corresponding current project memory.

The ChatGPT Library checklist contains a different state indicating technical implementation/compilation/testing with user confirmation still pending.

Safe status:

CONFLICTED STATE — requires factual verification in the actual Unity project.

Do not mark Stage 4.4.5 as confirmed based on either documentation source alone.

Stage 3

GitHub changelog records a working baseline involving:

- individual target assignment;
- "visitedRooms" with unvisited priority;
- exclusion of another active "targetRoom";
- leader existence/re-election;
- directed room adjacency;
- non-destructive A* refresh;
- hysteresis;
- waypoint/leaderless fallback;
- fast empty-room search;
- gates against active-target overwrite;
- continuation after enemy death;
- runtime detection and navigation behavior.

The record says implemented/compiled/tested, but confirmation state has historical ambiguity.

Safe treatment:

- technical working baseline appears documented;
- user confirmation must not be inferred;
- current factual Unity state should be checked before treating it as canonical.

Stage 4.4.4

Documentation records Stage 4.4.4 as confirmed.

This should still be distinguished from the conflicting Stage 4.4.5 state.

Stage 5

Not safely closed by the audit.

DestructibleObject

A destructibility foundation exists, but this does not mean Stage 8 is complete.

Documented foundation:

- one core "DestructibleObject";
- HP;
- state flow INTACT → DAMAGED → DESTROYED;
- arrays for hide/show;
- UnityEvents;
- no Update loop;
- Bullet/EnemyBullet damage routed through "GetComponentInParent<DestructibleObject>";
- "CoverSystem" guard;
- controlled partial destruction;
- only limited kinematic debris for fence use;
- Android-performance considerations.

Treat this as foundation work, not automatic Stage 8 completion.

---

7. Actual architecture facts preserved by the audit

Important documented systems include:

- "EnemyController"
- "EnemyTacticalPlanner"
- "EnemyTacticalVision"
- "EnemyHearing"
- "NoiseSystem"
- "ArenaTacticalMap"
- "CoverSystem"
- "CoverPoint"
- "WildWestEnvironmentGenerator"
- "DestructibleObject"
- "EnemySpawner"
- "WaveManager"

Architectural principle:

- "EnemyTacticalVision" should handle vision/local obstacle scanning rather than becoming the pathfinding system.
- Tactical planning, hearing/noise, room navigation, cover, and pathfinding are separate concerns.

Environment source pipeline documented in GitHub:

- source assets under "Documents\WildWestGunslinger art\art\Environment\<Name>\";
- FBX + "_Source.blend" + textures;
- Unity uses FBX + textures;
- FBX Z-up is compensated at prefab Model level with approximately -90° X rotation;
- minY target is 0.00;
- shared URP Lit materials are used;
- realtime lights are minimized;
- colliders are kept minimal.

---

8. Procedural environment constraint

The current design direction explicitly rejects building procedural western walls and floors from many independent Cube/GameObjects.

Target modular kit:

WallSegment

- approximately 4 m;
- one mesh;
- multiple horizontal logs contained in that mesh;
- one MeshRenderer;
- one shared material;
- one collider;
- reusable variants.

FloorSegment

- approximately 4 × 4 m;
- one mesh;
- multiple floorboards contained in that mesh;
- one MeshRenderer;
- shared material.

This is a project design constraint and should be preserved during future procedural-environment work.

---

9. Character / asset production framework established before this record

A separate production methodology was developed and should not be silently mixed into the old roadmap.

Package structure:

A. Character Package
B. Equipment Package
C. Weapon Package
D. Enemy Character Package
E. Animation Package

Then:

- Cross-Package Audit
- Final Package Audit
- Unity Import & Integration

Unity is treated as the assembly/integration environment, not as the first place where asset compatibility is discovered.

Initial Project Recon

Required first:

- read-only scan;
- establish Project Knowledge Baseline;
- no modifications during recon.

Baseline rule:

- baseline is treated as immutable during the session;
- if a new fact affects later stages and is absent from the baseline, STOP;
- user decides whether/how the baseline is updated;
- updates should create a versioned baseline or receive explicit overwrite approval.

Reuse of baseline in later OpenCode sessions

A prior baseline may be reused only when:

- baseline is available;
- user confirms the project has not changed;
- Git HEAD matches the recorded HEAD.

Otherwise perform a new recon.

---

10. Git safety framework

Before write operations:

1. verify repository;
2. verify branch;
3. verify HEAD;
4. verify working tree;
5. establish recovery checkpoint/hash;
6. establish accepted pre-production state.

If no verified recovery point exists:

- STOP;
- unless the user explicitly approves another reversible mechanism.

Git commit is a separate operation after final audit and user approval.

No automatic push should be assumed.

---

11. Global change scope

Default working area:

"Working/"

Existing files are protected.

Rules:

- no deletion;
- CREATE must not overwrite an existing file;
- Unity "Assets/" changes require explicit allowance/separate approval;
- existing gameplay systems are protected unless explicitly authorized.

Protected systems include:

- Unity gameplay;
- PlayerController;
- EnemyController;
- combat/GunController/Bullet;
- Health/HUD;
- MobileTouchControls;
- procedural arena;
- AI/cover;
- existing assets;
- existing Character Foundation;
- Unity prefabs;
- unrelated documentation.

---

12. Character package and contract framework

Character E0 recon must establish facts about:

- source;
- skeleton;
- Head;
- scale/height;
- orientation;
- topology;
- materials;
- bounds;
- origin;
- forward axis;
- triangles;
- LODs;
- textures;
- skinning;
- attachments.

No inference is allowed where factual inspection is possible.

Character Contract

The contract includes:

- skeleton hierarchy/naming;
- orientation;
- scale;
- height;
- forward axis;
- Head;
- Hand_L / Hand_R;
- weapon attachment points;
- humanoid bones;
- base pose;
- deformation constraints;
- material/texture expectations;
- naming and attachment conventions.

Lifecycle:

DRAFT → VERIFICATION → REPORT → STOP → USER APPROVAL → FROZEN

Contract Freeze happens before dependent Equipment/Weapon/Enemy/Animation work.

Breaking changes include:

- skeleton hierarchy/name changes;
- orientation;
- scale;
- height;
- forward axis;
- hand positions;
- attachment points;
- humanoid bones;
- base pose;
- deformation assumptions.

Such changes require:

- impact assessment;
- dependent package review;
- explicit user approval;
- new contract version/freeze.

State marker:

CONTRACT BREAK

---

13. Equipment framework

Stages:

- E0 Dummy Recon
- E1 Hat
- E2 Torso
- E3 Legs/Feet
- E4 Belt/Holster
- E5 Equipment Package Audit

E1 must be approved before E2.

Equipment modularity rule:

Replacing an E1–E4 component must not require modification of:

- Character Package;
- Dummy/Foundation;
- unrelated components.

Failure state:

STOP — EQUIPMENT MODULARITY FAILURE

Hat v1.2

Documented constraints:

- Blender + FBX only;
- working location "Working/blender_src/hat_01/";
- "hat_01_work.blend";
- "hat_01_Intact.fbx";
- review PNGs;
- report;
- no overwrite;
- checkpoint flow from silhouette variants through approved variant and final geometry/fit/attachment to production/export/reimport;
- target approximately 250–500 tris;
- maximum 800 tris;
- one material slot;
- stylized low-poly western;
- broad brim/high crown;
- attachment method determined after E0;
- no Unity changes in E1.

---

14. Weapon compatibility framework

Weapon contract covers:

- origin;
- scale;
- orientation;
- grip position/orientation;
- muzzle;
- magazine;
- bolt/slide;
- hand alignment;
- attachment points;
- animation interface;
- naming;
- materials;
- export;
- performance.

Grip is a three-way interface:

Character skeleton + Weapon geometry/grip + Animation hand pose

This must be checked before Unity integration.

---

15. Enemy package

Enemy package covers:

- body;
- variants;
- class visuals;
- equipment variants;
- materials;
- LOD/performance;
- animation compatibility.

Preferred direction:

- shared player skeleton.

If an enemy requires a separate skeleton:

- STOP;
- perform compatibility impact assessment;
- user decides.

---

16. Animation package

Required areas:

- Idle;
- Walk;
- Run;
- Combat;
- Shooting;
- Reload;
- Hit;
- Death;
- Interaction.

Animation contract includes:

- skeleton version;
- bones;
- base pose;
- clips;
- root motion;
- attachment/weapon grip;
- equipment compatibility;
- player/enemy applicability;
- export/version.

---

17. Cross-Package Audit

Performed after Character, Equipment, Weapon, Enemy and Animation packages are ready.

Checks:

- common bone/socket names;
- scale/orientation;
- equipment/weapon attachment;
- animation interfaces;
- grip;
- versioning;
- breaking changes.

Failure:

AUDIT FAILED

No automatic fix.

No Git commit after a failed audit.

User must receive source-backed report and decide next action.

Backward incompatibility:

STOP — BACKWARD INCOMPATIBILITY

---

18. Unity integration sequence

Separate Unity sequence:

- U0 Unity Import Recon
- U1 Character Foundation
- U2 Equipment Integration
- U3 Weapon Integration
- U4 Player Animation Integration
- U5 Enemy Animation Integration
- U6 Prefab/Runtime Assembly
- U7 Unity Integration Audit

The Character Foundation must not be treated as complete merely because a Blender asset exists.

---

19. Project roadmap mapping

Previously established mapping:

Stage 6.8 Character Foundation

- production → Character/Equipment/Weapon/Enemy/Animation packages;
- Unity → U1–U7;
- gameplay → Stage 6/7.

Stage 7 Weapon System

- foundation → Weapon Package;
- attachment → Weapon Package + U3;
- presentation → Package + U3;
- feedback → Stage 18;
- balance → Stage 20;
- Android performance → U7/Stage 20.

Stages 9–11

- gameplay and AI;
- enemy visual variants depend on Enemy Character Package.

Stage 17

- animation production → Animation Package;
- Animator transitions/integration → U4/U5;
- performance → U7/Stage 20.

Stage 18

- polish after framework and Unity integration.

Stage 19

SECURITY, PRIVACY AND MARKETPLACE RELEASE

Stage 20

BALANCE

---

20. Stage status discipline

Allowed status vocabulary documented for the project:

- Implemented
- Compiled
- Tested
- Confirmed
- PENDING for intermediate states where appropriate

Important:

- "CONFIRMED" is not an AI inference.
- User confirmation is the authoritative confirmation event.
- A report saying a system compiled or tested does not automatically mean the user confirmed it.

---

21. OpenCode / toolchain context

Known environment:

- OpenCode 1.18.31
- Node 24.21.0
- Python 3.14.7
- Git 2.55.0
- Unity 6000.3.23f1
- Blender 5.1.2
- OmniRoute: "http://localhost:20128/v1"
- LM Studio: "http://127.0.0.1:1234/v1"
- Blender MCP: localhost port 9876, previously verified alive
- Unity MCP: "http://127.0.0.1:8080/mcp", previously configured but unavailable when Unity Editor is not running
- Windows build: 26200
- PowerShell 5.1

The project has Git initialized on branch "main".

Historical audit note:

- an older section of "AI_TOOLCHAIN_AUDIT.md" said the project was not a Git repository;
- later Phase 2 results documented "git init -b main" on 2026-09-25;
- the later record supersedes the older snapshot.

Do not interpret the entire "AI_TOOLCHAIN_AUDIT.md" as a single current-state snapshot.

---

22. OpenCode operating model

Established division:

ChatGPT = orchestrator / planner

OpenCode = executor

Codex is explicitly excluded from the user's current remote automation plan.

Desired workflow:

АНАЛИЗ → ПЛАН → СОГЛАСОВАНИЕ → ИЗМЕНЕНИЕ → COMPILE → UNITY TEST → ПОДТВЕРЖДЕНИЕ

User confirmation is the only true "CONFIRMED".

No unattended destructive changes.

---

23. GitHub / OpenCode / ChatGPT access separation

The current audit demonstrated three separate access paths.

ChatGPT GitHub connector

- Access depends on the repository being visible to the connector.
- Current Public repository is accessible.
- Current Private repository was not accessible.

OpenCode on PC

- Uses local Git/GitHub authentication.
- It does not depend on ChatGPT's connector authentication.
- Recommended authentication mechanism: GitHub CLI.

Expected PC verification:

"gh auth status"

If needed:

"gh auth login"

Then verify repository operations from:

"C:\Users\cyril\WildWestGunslinger"

Expected checks:

- "git remote -v"
- "git fetch origin"
- "git status"
- "git branch --show-current"

Do not assume OpenCode is authenticated merely because ChatGPT can access the repository.

---

24. Current recommended reconciliation task

The next major task remains a HARD RECON across four sources:

1. GitHub "main"
2. local "C:\Users\cyril\WildWestGunslinger"
3. factual Unity project state
4. ChatGPT Library

Output structure:

FACT → SOURCE → DATE → STATUS → CONFLICT → ACTION

At minimum verify:

- Git HEAD;
- working tree;
- ".gitignore";
- AI_CONTEXT;
- Assets;
- Stage 4.4.5;
- Stage 3;
- environment;
- DestructibleObject;
- Character Foundation;
- actual C# files;
- OpenCode configuration;
- ".opencode";
- Blender pipeline;
- GitHub main vs local HEAD.

Do not mass-update MD files before this comparison.

---

25. Safety rule for future documentation changes

A new tool, skill, MCP capability, or workflow that materially changes an approved decision requires targeted re-verification.

If it changes an approved decision:

STOP — NEW CAPABILITY REQUIRES REVIEW

Reports must describe work that was actually performed, not planned work.

---

26. What this document does NOT claim

This record does not claim that:

- Stage 4.4.5 is complete;
- Stage 3 is user-confirmed;
- Stage 8 is complete;
- Stage 5 is complete;
- Character Foundation is complete;
- the new Character/Equipment/Weapon/Enemy/Animation framework has already been fully integrated into GitHub canonical documentation;
- OpenCode GitHub authentication is configured;
- private-repository access for ChatGPT is configured;
- the local working tree is identical to GitHub "main".

Those facts require separate verification.

---

27. Immediate state after this session

Repository visibility: PUBLIC

ChatGPT GitHub access: VERIFIED

GitHub repository metadata access: VERIFIED

Repository content read capability: VERIFIED

Repository content write capability through current ChatGPT integration: FAILED — HTTP 403 "Resource not accessible by integration"

Private repository access through current ChatGPT connector: NOT AVAILABLE with current connector permissions

OpenCode PC authentication: SEPARATE / not verified by this record

Documentation reconciliation: NOT COMPLETE

Mass synchronization: NOT AUTHORIZED

Data-loss indication: NONE

Next major operation: HARD RECON before canonical documentation rewrite

---

28. Preservation intent

This document exists as a session/audit preservation record.

It should not replace:

- "PROJECT_STATE.md";
- "CONFIRMED_STATE.md";
- "CURRENT_TASK.md";
- "CHANGELOG.md";
- the canonical project checklist.

Those documents should only be updated after the factual reconciliation process establishes which statements are current.

---

29. Audit session event log

Event 1 — Repository access verification

The repository was accessible through the connected GitHub integration.

Event 2 — Public → Private test

The user changed repository visibility to Private.

The ChatGPT connector immediately lost repository access and returned HTTP 404.

Event 3 — Private → Public restoration

The user changed the repository back to Public.

Repository metadata became accessible again.

Event 4 — Write capability test

A dedicated audit document was prepared for:

"Audit всей системы документации/AUDIT_SESSION_CONTEXT_2026-09-28.md"

The GitHub integration attempted to create the file on "main".

GitHub rejected the operation with:

"403 Resource not accessible by integration"

No file was created by that failed operation.

No existing repository file was modified by that operation.

Event 5 — Manual preservation

The user requested the complete Markdown content in ChatGPT so the file can be created manually and uploaded to the repository.

This document is that preservation content.

---

30. Verification requirements after manual upload

Once the user uploads this file to GitHub, the following checks should be performed:

1. Verify exact path:
   "Audit всей системы документации/AUDIT_SESSION_CONTEXT_2026-09-28.md"

2. Verify the file can be read through the GitHub connector.

3. Verify the uploaded content matches this preservation record.

4. Verify the file does not accidentally contain:
   
   - credentials;
   - API keys;
   - passwords;
   - tokens;
   - private personal information.

5. Verify that creation of the audit file did not modify unrelated files.

6. Verify the commit containing the file is understood and accepted by the user before treating it as part of the canonical documentation state.

The audit folder is a preservation/archive area until the reconciliation process explicitly promotes any document to canonical status.

---

31. Final preservation state

AUDIT CONTEXT PRESERVED FOR MANUAL REPOSITORY UPLOAD

Repository:

"noxt1/WildWestGunslinger"

Branch:

"main"

Requested folder:

"Audit всей системы документации"

Requested file:

"AUDIT_SESSION_CONTEXT_2026-09-28.md"

Repository visibility at record time:

PUBLIC

ChatGPT GitHub read access:

VERIFIED

ChatGPT GitHub write access:

FAILED — HTTP 403

Documentation reconciliation:

PENDING

Canonical documentation rewrite:

BLOCKED UNTIL HARD RECON

User approval remains the final authority for "CONFIRMED" state and for any breaking or destructive project changes.

---

Record status: PRESERVED
Record type: Audit / Documentation Reconciliation Context
Repository: "noxt1/WildWestGunslinger"
Branch: "main"
Date: 2026-09-28
