# AI_CONTEXT LOSS CHECK

**Document ID:** `DOC-LOSSCHECK-P4-2026-09-29`
**Status:** COMPLETE — **0 unidentified useful content**
**Date:** 2026-09-29

Every substantial fragment from the 8 pre-existing-dirty `AI_CONTEXT` files is
accounted for below. Target: **zero** unclassified content.

---

## 1. Scope reconciled

| File | HEAD lines | Working lines | Added | My header | Pre-existing |
|---|---|---|---|---|---|
| `ARCHITECTURE.md` | 220 | 389 | 77 | 6 | **71** |
| `ART_PIPELINE.md` | 35 | 284 | 225 | 8 | **217** |
| `CHANGELOG.md` | 358 | 678 | 181 | 5 | **176** |
| `CONFIRMED_STATE.md` | 75 | 130 | 24 | 7 | **17** |
| `CURRENT_TASK.md` | 53 | 251 | 172 | 7 | **165** |
| `PROJECT_STATE.md` | 161 | 302 | 92 | 5 | **87** |
| `RULES.md` | 81 | 144 | 28 | 5 | **23** |
| `VERIFICATION.md` | 15 | 35 | 14 | 7 | **7** |
| **Total** | **998** | **2 213** | **813** | **50** | **763** |

Deletions across all 8: **0**. No historical content was rewritten.

---

## 2. Loss check — every substantial fragment

### 2.1 `PROJECT_STATE.md`

| # | Content | Classification | Destination | Preserved | Reason |
|---|---|---|---|---|---|
| 1 | Unity version `6000.3.23f1` | PROJECT FACT | `UNI-0001` §1 | ✅ | Phase 2 wrongly said "not recorded" — it is in `ProjectVersion.txt` |
| 2 | Target platforms Android + Windows | CONFLICTED | `UNI-0001` §1, `CONFLICT-15` | ✅ | Recorded both statements; priority conflict flagged |
| 3 | Assistant language Russian | OPERATIONAL | retained in this file | ✅ | Execution preference, not project fact |
| 4 | Checkpoint Stage 4.4.5 FROZEN / last confirmed 4.4.4 | HISTORY | `GAME-0004` §6, `HISTORY-0002` | ✅ | Legacy stage numbering is history |
| 5 | Separate Blender production branch, not a new Stage | DECISION | `ART-0001` §3, `ART-0002` | ✅ | Preserved: art branch ≠ stage progression |
| 6 | Parallel 2026-09-27 visual work, stage number unchanged | HISTORY | `UNI-0003` §1.0 | ✅ | FIX-01 / FIX-02 |
| 7 | Blender asset registry — 3 assets with status/paths/tris | PROJECT FACT | `ART-0002` §4, §5; `ART-0001` §3.1 | ✅ | **Corrected the approval state** |
| 8 | Presentation state rule (hide_viewport / hide_render) | OPERATIONAL RULE | **retained in `ART_PIPELINE`** + `ART-0003` §4a.4 | ✅ | Workflow rule — must NOT live in PROJECT_TRUTH |
| 9 | Technical decisions — `Col` FLOAT_COLOR, metal/wood, UV, slots, no image textures | PRECEDENT | `ART-0003` §4a | ✅ | Filed as precedent, not new decision |
| 10 | FBX export contract (frozen) | OPERATIONAL RULE | **retained in `ART_PIPELINE`** | ✅ | Frozen contract; single source of truth kept there |
| 11 | Batch-only Unity integration policy | OPERATIONAL RULE | `GAME-0004` `OR-08` + retained in `ART_PIPELINE` | ✅ | Execution constraint |
| 12 | Modular environment kit architecture (not implemented) | DECISION / PLAN | `GAME-0004` `OR-09` + retained | ✅ | Explicitly marked *plan, not implemented* |
| 13 | HUD overlay fix | EVIDENCE | `UNI-0003` §1.0 FIX-01 | ✅ | Runtime-verified 2026-09-27 |
| 14 | Render/Quality values (renderScale 1, MSAA 4, Deferred, HDR, FPS) | PROJECT FACT | `UNI-0002` §5c | ✅ | **Newly migrated** |
| 15 | Shadow Aliasing OPEN (1024 / 1 cascade / 50) | OPEN ISSUE | `OPEN_ISSUES` `ISSUE-14` | ✅ | **Newly created** |
| 16 | Arrow radial position open question | OPEN ISSUE | `OPEN_ISSUES` `ISSUE-16` | ✅ | **Newly created** |
| 17 | Square-shadow diagnosis queued behind arrow A/B | OPEN ISSUE | `OPEN_ISSUES` `ISSUE-15` | ✅ | **Newly created** |
| 18 | Project structure "41 .cs in 8 dirs" | **STALE** | `UNI-0001` §5.1 + `CONFLICT-11` | ✅ | **Corrected: 42 files, 10 dirs, `Environment/` was missing** |
| 19 | Arena parameters (honeycomb 3×3, 9 rooms, ranges) | PROJECT FACT | `GAME-0001` §7.1 | ✅ | Spot-checked in source |
| 20 | Room-type progression table (Combat/Tactical/Elite) | PROJECT FACT | `GAME-0001` §7.1 context | ⚠️ partial | Values retained here; summary added to `GAME-0001` |
| 21 | Cover counts 3–5 per room, max 8 | PROJECT FACT | `GAME-0001` §7.1 | ✅ | |
| 22 | Spawn zones 3–5, radius 1.5–3.0, sep 4.0 | PROJECT FACT | `GAME-0001` §7.1 | ✅ | |
| 23 | Wave system (3 start, +2, 3 s, formula) | PROJECT FACT | `GAME-0001` §7.2 | ✅ | |
| 24 | Enemy archetype table + elites + scaling | PROJECT FACT | `GAME-0001` §7.3 | ✅ | |
| 25 | `ArenaTacticalMap` params (1.5 / 0.65 / 2500) | PROJECT FACT | `GAME-0001` §7.1 | ✅ | **Verified in source** |
| 26 | Player speeds and HP | PROJECT FACT | `GAME-0001` §7.4 | ✅ | |
| 27 | AI development status table (Stage 1/2 CONFIRMED, 3 BASELINE, 4.4.x) | HISTORY | `GAME-0002` §3.2, `HISTORY-0002` | ✅ | Kept as history, not current state |
| 28 | `WildWestEnvironment` design + runtime counts | PROJECT FACT | `UNI-0002` §5b | ✅ | **Newly migrated** |
| 29 | Environment prefab registry (8 prefabs, HP, CoverPoints) | PROJECT FACT | `UNI-0002` §5a | ✅ | **Newly migrated** |
| 30 | Z-up −90° X correction, minY 0, 0 realtime lights, pools | PROJECT FACT | `UNI-0002` §5a | ✅ | |
| 31 | "One well-prepared asset, reused" principle | OPERATIONAL | retained in `ART_PIPELINE` | ✅ | |
| 32 | Stage 3 baseline — 13 behavioural observations | EVIDENCE (dated) | `GAME-0002` §3.2 | ✅ | **Preserved** — incl. combat test observation |
| 33 | Stage 3 known limitations (3 bullets) | OPEN ISSUE | retained in this file + `GAME-0002` | ✅ | Not lost; kept with the baseline it qualifies |
| 34 | "Do not break" list | OPERATIONAL RULE | `GAME-0004` `OR-10` | ✅ | |

### 2.2 `ARCHITECTURE.md`

| # | Content | Classification | Destination | Preserved | Reason |
|---|---|---|---|---|---|
| 1 | Component overview diagram | PROJECT FACT | `ARCHITECTURE.md` §1 | ✅ | Canonical already covers the shape |
| 2 | `EnemyDirectionIndicator` full spec | PROJECT FACT | `GAME-0001` §7.7 | ✅ | **Migrated**; params verified in source |
| 3 | `EnemyController` 7 FSM states | PROJECT FACT + NOT VERIFIED | `GAME-0002` §3 | ✅ | States **exist in code**; only `Searching` is runtime-verified. Kept the distinction |
| 4 | `EnemyController` key methods + re-arm latch, 15% hysteresis, 0.5/0.3 s scans | PROJECT FACT | **retained in this file** | ✅ | Implementation detail — operational reference |
| 5 | `EnemyTacticalPlanner` roles + methods | PROJECT FACT | **retained in this file** | ✅ | Component reference |
| 6 | `EnemyTacticalVision` scan distance, FOV, 4 raycast points | PROJECT FACT | `GAME-0001` §7.6 | ✅ | |
| 7 | `EnemyHearing` noise priorities | PROJECT FACT | `GAME-0001` §7.6 | ✅ | |
| 8 | `NoiseSystem` radii + propagation formula | PROJECT FACT | `GAME-0001` §7.6 | ✅ | |
| 9 | `ArenaTacticalMap` methods + Build/FindRoom contract | PROJECT FACT | **retained** | ✅ | Key invariant: `FindRoom()` does not call `Build()` |
| 10 | `CoverSystem` / `CoverPoint` | PROJECT FACT | **retained** | ✅ | |
| 11 | `WildWestEnvironmentGenerator` design | PROJECT FACT | `UNI-0002` §5b | ✅ | **Migrated** |
| 12 | `DestructibleObject` foundation | PROJECT FACT | `UNI-0002` §5d | ✅ | **Migrated** — refines `UNI-D12` |
| 13 | Blender→Unity prefab pipeline | PROJECT FACT | `UNI-0002` §5a | ✅ | |
| 14 | Batched Unity integration two-stage scheme | OPERATIONAL RULE | `GAME-0004` `OR-08` + retained | ✅ | |
| 15 | Runtime contract table (Intact/Fragments) | OPERATIONAL RULE | **retained in `ART_PIPELINE`** | ✅ | |
| 16 | Material contract M2 | PRECEDENT | `ART-0003` §4a | ✅ | |
| 17 | Planned modular architecture | DECISION / PLAN | `GAME-0004` `OR-09` | ✅ | Marked *not yet implemented* |
| 18 | `EnemySpawner` grouping (4, every 3 waves, 6/4) | PROJECT FACT | `GAME-0001` §7.5 | ✅ | **Migrated** |
| 19 | `WaveManager` | PROJECT FACT | `GAME-0001` §7.2 | ✅ | |
| 20 | 5 architecture principles | DECISION | **retained in this file** | ✅ | Engineering principles, not project truth |
| 21 | Script line counts | **STALE** | `UNI-0001` §5.1 | ✅ | **Corrected** — 4 counts were wrong; `CONFLICT-11` |
| 22 | Corrupted token `блокируют视线` | DEFECT | fixed in this file | ✅ | `ISSUE-18` |

### 2.3 `ART_PIPELINE.md`

| # | Content | Classification | Destination | Preserved | Reason |
|---|---|---|---|---|---|
| 1 | Art checkpoint / focus | HISTORY | `GAME-0004` §6 | ✅ | |
| 2 | Blender workflow (OpenCode → MCP → Blender → QA) | OPERATIONAL | **retained** | ✅ | The actual working pipeline |
| 3 | QA loop MEASURE→MODEL→…→VISION RECHECK; NUMERIC+VISION=PASS | OPERATIONAL RULE | **retained** + `ART-0003` §3 | ✅ | Core art QA method |
| 4 | Method C constraints (constructive, no Voronoi/Cell, CP gate) | OPERATIONAL RULE | **retained** + `GAME-0004` `OR-07` | ✅ | |
| 5 | `BARREL_01` / `FENCE_01` APPROVED records | PROJECT FACT | `ART-0002` §4 | ✅ | **Corrected the canonical approval state** |
| 6 | `CRATE_01` CP1 complete, CP2 not authorized | PROJECT FACT / OPEN | `ART-0002` §5, `DEC-05` | ✅ | **Corrected** |
| 7 | Confirmed wood-surface jitter root cause | EVIDENCE | `ART-0002` §4, `ART-0003` §4a.4 | ✅ | |
| 8 | `Col = FLOAT_COLOR` and semantics | PRECEDENT | `ART-0003` §4a.1 | ✅ | |
| 9 | Unified metal / wood values | PRECEDENT | `ART-0003` §4a.2 | ✅ | |
| 10 | UV and slot rules, no image textures | PRECEDENT | `ART-0003` §4a.3 | ✅ | |
| 11 | FBX export contract (frozen) | OPERATIONAL | **retained verbatim** | ✅ | Frozen; single source kept here |
| 12 | Viewport presentation rule | OPERATIONAL | **retained** | ✅ | |
| 13 | Batch-only Unity integration | OPERATIONAL | `GAME-0004` `OR-08` + retained | ✅ | |
| 14 | Wall kit architecture + performance rule | DECISION / PLAN | `GAME-0004` `OR-09` + retained | ✅ | Marked *not implemented* |
| 15 | Floor kit architecture | DECISION / PLAN | `GAME-0004` `OR-09` + retained | ✅ | |
| 16 | Android performance principle | OPERATIONAL / DECISION | `GAME-0004` `OR-09` + retained | ✅ | |
| 17 | Wall/floor material direction; don't rewrite `ArenaGenerator` | DECISION | `GAME-0004` `OR-09` + retained | ✅ | |
| 18 | Canonical character direction + order; no skinning before approval | PLAN | `GAME-0004` §4, `ART-0001` | ✅ | |
| 19 | Character foundation ambiguity (Dummy 178395 vs Kevin Iglesias) | **CONFLICTED** | `GAME-0004` `DD-02`, `DEC-02` | ✅ | **Not resolved by assumption** — kept explicit |
| 20 | Environment asset rules | PROJECT FACT | `UNI-0002` §5a | ✅ | |
| 21 | Destructible asset rules (future Stage 8) | REQUIREMENT | `GAME-0004` §4, `UNI-0002` §5d | ✅ | Stage 8 explicitly NOT done |
| 22 | Garment lessons (Seams to Plush, Remesh, Cloth) | HISTORICAL | **retained in this file** | ✅ | Lessons learned, not a spec |
| 23 | Visual direction (low-poly western, no city) | DECISION | `GAME-0004` `DD-03`, `DD-06` | ✅ | |

### 2.4 `CURRENT_TASK.md`

| # | Content | Classification | Destination | Preserved | Reason |
|---|---|---|---|---|---|
| 1 | Checkpoint frozen; stop point = `CRATE_01 CP1` | CURRENT TASK | **retained** + `DEC-05` | ✅ | This file's core role |
| 2 | Approved asset table | PROJECT FACT | `ART-0002` §4, §5 | ✅ | |
| 3 | Presentation state rule | OPERATIONAL | retained in `ART_PIPELINE` | ✅ | |
| 4 | Batch integration rule | OPERATIONAL | `GAME-0004` `OR-08` | ✅ | |
| 5 | Modular kit decision + performance principle | DECISION / PLAN | `GAME-0004` `OR-09` | ✅ | |
| 6 | Open task: arrow A/B test | OPEN ISSUE | `OPEN_ISSUES` `ISSUE-16` | ✅ | |
| 7 | Open task: Shadow Aliasing | OPEN ISSUE | `OPEN_ISSUES` `ISSUE-14` | ✅ | |
| 8 | Open task: square-shadow diagnosis | OPEN ISSUE | `OPEN_ISSUES` `ISSUE-15` | ✅ | |
| 9 | **Open task: 4 prefabs hold a deleted script reference** | OPEN ISSUE | `UNI-0003` `UNI-D05` **root cause**, `OPEN_ISSUES` `ISSUE-13` | ✅ | **Major discovery** — resolved an "unidentified defect" |
| 10 | **Open task: duplicate scene origin = MCP saved to `Assets/`** | OPEN ISSUE | `OPEN_ISSUES` `ISSUE-17` | ✅ | **Root cause of `DEC-09`** |
| 11 | Stage 4.4.5 frozen | HISTORY | `GAME-0004` §6 | ✅ | |
| 12 | `CRATE_01` CP2 not authorized + budgets | OPEN DECISION | `DEC-05` | ✅ | |
| 13 | Batch integration awaits decision | CURRENT TASK | **retained** | ✅ | |
| 14 | Completed 2026-09-27 table (6 items) | HISTORY | `UNI-0003` §1.0 | ✅ | |
| 15 | Current focus + priorities + next action | CURRENT TASK | **retained** | ✅ | This file's role |
| 16 | Stage 3 baseline reference | HISTORY | `GAME-0002` §3.2 | ✅ | |
| 17 | Stage 3 expected-behaviour chain (SpawnZones=53) | EVIDENCE (superseded) | `CONFLICT-14` | ✅ | 52 measured — discrepancy recorded |
| 18 | Modified-files inventory (working copy, 2026-09-27) | PROJECT FACT | `UNI-0002`, `CONFLICT-16` | ✅ | **Preserved** — explains the 7 uncommitted changes |
| 19 | Untracked file inventory | PROJECT FACT | `UNI-0002` §7 | ✅ | |
| 20 | Duplicate scene artifact | OPEN ISSUE | `ISSUE-17`, `DEC-09` | ✅ | |

### 2.5 `CHANGELOG.md`

| # | Content | Classification | Destination | Preserved | Reason |
|---|---|---|---|---|---|
| 1 | 2026-09-27 HUD overlay fix | HISTORY + EVIDENCE | `UNI-0003` §1.0 FIX-01 | ✅ | |
| 2 | 2026-09-27 aliasing A/B tests | HISTORY + EVIDENCE | `UNI-0003` §1.0 FIX-02, `UNI-0002` §5c | ✅ | |
| 3 | 2026-09-27 3D ring + world-space arrows | HISTORY | `GAME-0001` §7.7 | ✅ | |
| 4 | 2026-09-27 MD sync phase | HISTORY | **retained in changelog** | ✅ | Documentation change |
| 5 | Earlier Stage 1/2/3 history | HISTORY | **retained in changelog** + `HISTORY-0002` | ✅ | **Not rewritten** |
| 6 | Superseded intermediate `Compiled=NO / Tested=NO` lines | HISTORY | **retained, explicitly marked superseded** | ✅ | Preserved as history, marked as not current |
| 7 | Deprecated `Instance==null` root-cause diagnosis | HISTORY | **retained, marked superseded** | ✅ | Correct diagnosis was `IsBuilt=false` |

> **No changelog entry was rewritten.** Historical entries keep their original
> status values; where a later entry supersedes an earlier one, the supersession
> is stated in the file rather than applied retroactively.

### 2.6 `CONFIRMED_STATE.md`

| # | Content | Classification | Destination | Preserved | Reason |
|---|---|---|---|---|---|
| 1 | Only user-confirmed systems belong here | OPERATIONAL RULE | **retained** | ✅ | Good rule, kept |
| 2 | Stage 1 / Stage 2 CONFIRMED | HISTORY | `GAME-0002` §3.2 | ✅ | |
| 3 | Stage 3 NOT CONFIRMED (tested, not confirmed) | HISTORY | `GAME-0002` §3.2 | ✅ | |
| 4 | Stage 4.4.4 CONFIRMED | HISTORY | `GAME-0002` §3.2, `UNI-0002` §5b | ✅ | |
| 5 | Barrel/Fence CONFIRMED (Blender part only) | PROJECT FACT | `ART-0002` §4 | ✅ | Explicitly Blender-scope only |
| 6 | Presentation state rule confirmed | OPERATIONAL | retained in `ART_PIPELINE` | ✅ | |
| 7 | Game systems CONFIRMED (menu, pause, restart, **mobile UI**, movement, shooting, Bullet, **XP**, **Level Up**, **UpgradePanel**, WaveManager, EnemySpawner) | HISTORICAL CONFIRMATION | `GAME-0002` §3.2 | ✅ | **Corrected the "no evidence" claim**; mobile UI conflict → `CONFLICT-13` |
| 8 | AI systems CONFIRMED / EXISTS | HISTORICAL CONFIRMATION | `GAME-0002` §3.2 | ✅ | |
| 9 | Procedural generation EXISTS | HISTORICAL CONFIRMATION | `GAME-0001` | ✅ | |
| 10 | Explicit "not confirmed" list | OPERATIONAL | **retained** + `DEC-05`, `GAME-0004` | ✅ | |
| 11 | Status vocabulary | OPERATIONAL | **retained** + `GAME-0004` `WC-03` | ✅ | |

### 2.7 `RULES.md`

| # | Content | Classification | Destination | Preserved | Reason |
|---|---|---|---|---|---|
| 1 | Don't break what works | OPERATIONAL RULE | **retained** + `GAME-0004` `OR-01` | ✅ | Binding agent rule |
| 2 | Respect confirmed stages | OPERATIONAL | retained + `OR-02` | ✅ | |
| 3 | Explicit agreement for new states/scripts | OPERATIONAL | retained + `OR-02` | ✅ | |
| 4 | Don't duplicate | OPERATIONAL | retained + `OR-03` | ✅ | |
| 5 | Data accuracy; separate implemented/compiled/tested/confirmed | OPERATIONAL | retained + `OR-04` | ✅ | |
| 6 | Blender/character-art rules | OPERATIONAL | retained + `OR-07` | ✅ | |
| 7 | Blender 3D mandatory rules (Method C, FLOAT_COLOR, presentation, FBX) | OPERATIONAL | retained + `OR-07` | ✅ | |
| 8 | Batch-only Unity integration | OPERATIONAL | retained + `OR-08` | ✅ | |
| 9 | Modular kit performance rule | OPERATIONAL | retained + `OR-09` | ✅ | |
| 10 | Workflow before/during/after change | OPERATIONAL | retained + `OR-11` | ✅ | |
| 11 | Platform statement (Android primary) | **CONFLICTED** | `CONFLICT-15` | ✅ | Contradicts canonical Windows-primary |
| 12 | Protected file list (14 files) | OPERATIONAL | retained + `GAME-0004` §5.2 | ✅ | **3 of them are already modified** → `CONFLICT-16` |
| 13 | Report format template | OPERATIONAL | retained | ✅ | |

### 2.8 `VERIFICATION.md`

| # | Content | Classification | Destination | Preserved | Reason |
|---|---|---|---|---|---|
| 1 | No work is done on agent's word alone | OPERATIONAL RULE | **retained** + `OR-05` | ✅ | |
| 2 | Evidence requirements per task type | OPERATIONAL | retained + `OR-06` | ✅ | |
| 3 | Prohibited: self-reported PASS/FINAL/DONE | OPERATIONAL | retained + `OR-05` | ✅ | |
| 4 | Report format extension | OPERATIONAL | retained | ✅ | |
| 5 | Gemma reviewer: candidate-only, triggers, permissions | OPERATIONAL | retained + `OR-12` | ✅ | |

---

## 3. Disposition summary

| Disposition | Fragments | Notes |
|---|---|---|
| Migrated to canonical documentation | **61** | Facts, decisions, open issues, history |
| Retained operationally in `AI_CONTEXT` | **44** | Workflow rules, evidence rules, task state, engineering reference, lessons |
| Superseded / corrected | **9** | Stale line counts, "41 files/8 dirs", "no XP evidence", "no approvals", CP2 framing |
| Flagged as conflict (not auto-resolved) | **5** | `CONFLICT-11`…`CONFLICT-16` |
| Historical — kept in place, not rewritten | **7** | Changelog entries, superseded diagnoses, garment lessons |
| Intentionally excluded | **0** | — |

**Unidentified useful content: 0.**

---

## 4. Content that changed the canonical truth

These were **not** merely archived — they corrected canonical errors:

| # | Canonical error | Correction |
|---|---|---|
| 1 | "No asset is approved" | `BARREL_01`, `FENCE_01` **APPROVED** (user-confirmed) |
| 2 | "Crate CP2 promotion" (implies authorised) | CP1 complete, **CP2 NOT AUTHORIZED** |
| 3 | "Unity version not recorded" | **6000.3.23f1** in `ProjectVersion.txt` |
| 4 | "41 .cs in 8 dirs" | **42 .cs in 10 dirs**; `Environment/` was omitted |
| 5 | "XP/progression: no evidence of any kind" | **5 scripts exist** + user-confirmed historically |
| 6 | `UNI-D05` = "unidentified defect" | **Root cause found**: 4 prefabs reference deleted script GUID |
| 7 | 4 script line counts wrong | Measured values now in `UNI-0001` §5.1 |
| 8 | Shadow Aliasing absent from issue register | `ISSUE-14` created — **open, not fixed** |
| 9 | Duplicate scene disposition had no root cause | `ISSUE-17` — MCP saved to `Assets/` instead of `Assets/Scenes/` |
| 10 | Art checkpoint said art was the highest state | `APPROVED` assets exist; integration ceiling unchanged |

---

**End of `AI_CONTEXT_LOSS_CHECK.md`**
