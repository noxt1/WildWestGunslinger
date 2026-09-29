# GAME-0004 — Requirements and Design Decisions Register

**Document ID:** `GAME-0004`
**Status:** CANONICAL
**Date:** 2026-09-29
**Preserves content from:** `~/Downloads/CHATGPT_CHECKLIST.md` (legacy Stage scheme), `~/Downloads/CHATGPT_PROJECT_STATE.md`, `~/Downloads/CHATGPT_WORK_QUEUE.md`
**Phase 2.5 purpose:** §8 reconciliation — preserve approved requirements and design decisions that Phase 2 had no home for.

---

## 1. Classification of this document

Everything in this document is **REQUIREMENT**, **DESIGN DECISION**, or
**HISTORICAL CHECKPOINT** — never implementation status.

| Marker | Meaning |
|---|---|
| `[DECISION]` | A design choice a human made and confirmed. Still binding unless revoked |
| `[REQUIREMENT]` | Something that must be true for release. **Not** implemented unless §"Runtime" says otherwise |
| `[PLAN]` | Sequenced intent. Not a commitment |
| `[HISTORICAL]` | A checkpoint record from the legacy era. Records where work stopped |

> **No item in this document is an implementation claim.** Implementation state
> lives in `GAME-0001` and `GAME-0002`, and is runtime-evidenced.

---

## 2. Fixed asset and style decisions `[DECISION]`

These were confirmed (`[x]`) in the legacy checklist. They are **preserved
verbatim in substance** and remain in force.

| # | Decision | Effect on canonical work |
|---|---|---|
| DD-01 | **Quaternius / «Konteri»** — temporary enemy model is fixed in place | Explains capsule-era enemy representation; a placeholder, not final enemy art |
| DD-02 | **Human Character Dummy** (Unity Asset Store asset **178395**) is the technical character foundation | **This is the origin of the character lineage.** Explains why the canonical basis is a generic M-size dummy rather than a bespoke sculpt |
| DD-03 | **City and buildings are excluded** from the current western environment scope | Scope boundary. The 9 rooms are interiors; do not plan an exterior city |
| DD-04 | Separate western assets are required: **rocks/mountains, cacti, trees/ground, fences, lanterns, wagons, crates, barrels, covers** | The required environment prop set. Matches the Crate/Barrel/Cover assets found in `Assets/Art/Props/` |
| DD-05 | The environment must be assembled from **replaceable prefab pools** | Architectural requirement. Explains why the arena is runtime-generated |
| DD-06 | **Do not mix incompatible low-poly styles** | Art constraint. Binding on all future asset intake |

### 2.1 DD-02 and the current character situation

`DD-02` names the **Human Character Dummy (asset 178395)** as the foundation.
The measured art documents (`ART-0001` §4.1) confirm the lineage in practice:
a generic 1818 mm M-size dummy with a 51-bone `WWG_Template_Armature`, dressed
in western garments.

> **This does not resolve `DEC-02`.** `DD-02` fixes the *basis*; it does not
> choose *which* of the 32 `WWG_Foundation_Source` variants, nor *which*
> `WWG_Doomy_Cowboy` file, is canonical. That remains a human decision.

---

## 3. Progression and economy requirements `[REQUIREMENT]`

All items are **not started**. None has runtime evidence.

| Domain | Requirement | State |
|---|---|---|
| XP | XP system | not implemented — no runtime evidence |
| XP | Level progression | not implemented |
| XP | Gameplay integration for XP | not implemented |
| Economy | Currency and economy | not implemented |
| Economy | Economy progression | not implemented |
| Cards | Card system | not implemented |
| Shop | Shop system | not implemented |
| Encounters | Encounter flow | not implemented — arena generation exists, encounter pacing does not |
| Encounters | Run progression | not implemented |

> **Classification: REQUIREMENT / NOT VERIFIED.** No runtime evidence exists for
> any item above. Do not describe any of them as partially built.

---

## 4. Content design requirements `[REQUIREMENT]`

From the legacy stage scheme, restated against current evidence.

| Domain | Requirement | Evidence status |
|---|---|---|
| Character foundation | Character foundation established | **source exists**, not integrated (`UNI-D02`) |
| Character | Character animation | **not implemented** — 0 Animator, 0 Controller |
| Enemies | Enemy classes (distinct archetypes) | **not verified** — capsules only |
| AI | Group AI coordination | **not verified** |
| AI | Enemy perception/alerting | **partially verified** — vision scanning observed; alerting not tested |
| Environment | Environment foundation | **blocked** by `UNI-D06` |
| Environment | Procedural exterior | out of scope per `DD-03` |
| Environment | World presentation | not verified |
| Environment | Asset rules (replaceable pools) | **REQUIREMENT** `DD-05` |
| Aim | Aim system | not verified |
| Interaction | Interaction system | not verified |
| Weapons | Weapon system | **partially verified** — both prefabs fire; damage forced to 200 (`UNI-D10`) |
| Destructibles | Destructible environment | **not implemented** — 0 instances (`UNI-D12`) |
| Animation | Animation stage | **not implemented** |
| Polish | Visual polish | not started |
| Balance | Final playtest / balance | not started |

---

## 5. Workflow constraints `[DECISION]`

Preserved from the legacy operational documents.

| # | Constraint | Source |
|---|---|---|
| WC-01 | Work resumes from the **last human-confirmed ✅** and the first unfinished item | legacy checklist status rule |
| WC-02 | Detailed project state is stored **separately** from the plan/checklist | legacy checklist preamble |
| WC-03 | The legacy status vocabulary is: ⬜ not started · 🟡 in progress · 🔵 technically done, awaiting user check · ✅ user-confirmed · ⏸ deferred · ❌ needs rework | legacy checklist preamble |
| WC-04 | Stage numbering is **retired**; use `ART-`/`UNI-`/`GAME-`/`AI-`/`DOC-`/`REL-` IDs | `../DOC-0003-IDENTIFIER-SYSTEM.md` |

> **WC-03 mapping:** ✅ *user-confirmed* corresponds to `APPROVED` in the
> canonical state machine. 🔵 *technically done, awaiting user check* is
> **`QA PASS`** — which is exactly where most of the project sits.

### 5.1 Operational rules — binding on how work is executed

**Added in Phase 4.** These govern *process*, not project reality. They belong to
the operational layer and are **not** duplicated into `PROJECT_TRUTH.md`. The
authoritative text remains `AI_CONTEXT/RULES.md`; this is the canonical index.

| # | Rule | Where authoritative |
|---|---|---|
| OR-01 | **Do not break what works.** No rewriting of working code, no deleting existing logic, no whole-file overwrites, without justified need and explicit permission | `AI_CONTEXT/RULES.md` §1 |
| OR-02 | **No new AI states, new scripts, or edits outside the allowed file list** without agreement | `AI_CONTEXT/RULES.md` §3 |
| OR-03 | **Do not duplicate.** Check for an existing system before creating code | `AI_CONTEXT/RULES.md` §4 |
| OR-04 | **Data accuracy.** Never record assumptions as facts. Always separate *implemented / compiled / tested / confirmed*. Cite exact files and lines | `AI_CONTEXT/RULES.md` §5 |
| OR-05 | **Only the user sets `CONFIRMED` / `PASS` / `FINAL`.** An agent may not self-report these | `AI_CONTEXT/VERIFICATION.md` |
| OR-06 | **Evidence requirements** per task type: C# → real compile output; Unity behaviour → Play test with quoted observation; visual/Blender → numeric **and** visual evidence, neither alone suffices; regressions → state what was checked against `CONFIRMED_STATE` | `AI_CONTEXT/VERIFICATION.md` |
| OR-07 | **Blender 3D assets:** Method C (constructive assembly from real parts; Cell/Voronoi fracture, add-ons and physics sim are **prohibited**); every CP ends with a report and **waits for explicit authorisation** before the next CP | `AI_CONTEXT/ART_PIPELINE.md`, `AI_CONTEXT/RULES.md` §7 |
| OR-08 | **Unity integration of environment assets is BATCH ONLY.** Per-asset integration is prohibited. A single batch stage runs after the model set completes and after a separate user decision. `Asset FINAL ≠ reason to start Unity integration` | `AI_CONTEXT/ART_PIPELINE.md` |
| OR-09 | **Modular environment kit performance rule.** Never "1 log = 1 GameObject" or "1 floorboard = 1 GameObject" in ordinary procedural generation. Modules: `WallSegment ≈ 4 m`, `FloorSegment ≈ 4 × 4 m` → 1 mesh → 1 MeshRenderer → 1 shared material → 1 simple collider. Android performance is a constraint from the start. **`ArenaGenerator` is not to be rewritten** before material and kit design are settled | `AI_CONTEXT/ART_PIPELINE.md`, `AI_CONTEXT/RULES.md` §9 |
| OR-10 | **Do not rewrite working code**; Stage 1, Stage 2, A\*, `RefreshPathIfBetter()` (0.85f), individual targets, `visitedRooms`, `EnsureLeaderExists()`, Vision/LOS, Hearing/Noise, Combat/Investigation, accelerated empty search are all protected | `AI_CONTEXT/PROJECT_STATE.md`, `AI_CONTEXT/RULES.md` |
| OR-11 | **Post-change procedure:** compile → report with Implemented/Compiled/Tested/Confirmed → **do not consider the task done before a Unity test** → wait for user confirmation → then update `CONFIRMED_STATE`, `CHANGELOG`, `CURRENT_TASK` | `AI_CONTEXT/RULES.md` §Workflow |
| OR-12 | Independent reviewer (Gemma, filesystem-blind) is **candidate-only** by trigger; its findings are defects only after VERIFY by the main agent. It never sets `CONFIRMED`/`PASS`/`FINAL` | `AI_CONTEXT/VERIFICATION.md` |

### 5.2 Files that must not be changed without explicit permission

Recorded in `AI_CONTEXT/RULES.md`. Listed here so the canonical set exposes the
blast radius of an accidental edit.

`ArenaTacticalMap.cs` · `ArenaGenerator.cs` · `ArenaModule.cs` · `EnemySpawner.cs` ·
`EnemyTacticalVision.cs` · `EnemyHearing.cs` · `CoverSystem.cs` · `CoverPoint.cs` ·
`NoiseSystem.cs` · `PlayerController.cs` · `PlayerHealth.cs` · `WaveManager.cs` ·
`MainMenuController.cs` · `PauseMenuController.cs`

> **⚠️ Contradiction to note.** `EnemyTacticalPlanner.cs`, `ArenaGenerator.cs`
> and `EnemyDirectionIndicator.cs` are on this protected list **and** are
> currently **modified in the working tree** (2026-09-27/28 work, uncommitted).
> Their changes were made under a different authorisation path. Recorded as
> `CONFLICT-16`.

---

## 6. Historical checkpoint record `[HISTORICAL]`

Where the legacy work stopped, as recorded in the legacy checklist:

| Field | Value |
|---|---|
| Last confirmed block | **4.4.1–4.4.4** |
| Current checkpoint | **4.4.5 Background** |
| 4.4.5 status | technically implemented / compiled / tested, **final user status not confirmed** |
| Stage 3 (Procedural Arena) | **not to be considered fully closed** |
| Desert Pack | ⏸ **dormant** |

### 6.1 Reconciliation with runtime evidence

| Legacy statement | Runtime reality |
|---|---|
| 4.4.5 "technically implemented" | consistent with a running background/arena system; **not** independently re-verified by U-12 |
| Stage 3 not fully closed | **consistent** — the arena generates, but 0 modular meshes, 0 destructibles and 0 NavMesh are open |
| Desert Pack dormant | no desert assets found in `Assets/` — consistent |

> The legacy checkpoint is **plausible but not confirmed** by the runtime audit.
> Treat as `HISTORICAL`, not as verified state.

---

## 7. Requirements that Phase 2.5 verified as **not** implemented

To prevent a legacy claim from being read as a plan already in motion:

| Requirement | Verified absence |
|---|---|
| Character animation | 0 Animator / 0 Avatar / 0 AnimatorController / 0 SkinnedMeshRenderer |
| Destructible environment | 0 destructible instances |
| NavMesh navigation | 0 agents, 0 triangulation |
| Android touch controls | all `MobileTouchControls` references `null` |
| Equipment sockets | no socket/attach code in `Assets/Scripts` |
| Progression / XP / economy / cards / shop | no runtime evidence of any kind |
| Enemy classes / group AI | capsules only; no archetype evidence |

---

## 8. Cross-references

- `GAME-0001` — implemented design state
- `GAME-0002` — gameplay systems, with verification status
- `../REL/REL-0003-SECURITY-AND-RELEASE-REQUIREMENTS.md` — the security/release requirement set
- `../ART/ART-0001-ART-STATE.md` — character measurements behind `DD-02`
- `../History/OPEN_DECISIONS.md` — `DEC-02`, `DEC-08`
- `../DOC-0002-SUPERSEDED-DOCUMENTS.md` — S-06…S-09 (legacy source status)

---

**End of `GAME-0004-REQUIREMENTS-AND-DESIGN-DECISIONS.md`**
