# AUDIT_2_RECONCILIATION_REPORT.md

**Project:** WildWestGunslinger
**Audit type:** AUDIT 2 — Deep Reconciliation (read-only, second pass)
**Date:** 2026-09-29, 16:13–16:40 (+03:00)
**Audit window of Audit 1:** 2026-09-29, earlier session
**Prepared by:** OpenCode agent, read-only forensic mode
**Status:** ⏸ **STOP POINT — awaiting user APPROVE before any migration**

> **READ-ONLY COMPLIANCE**
> No C#/Unity/Prefab/asset/MD/Skill was modified. No git write operation (`add`/`commit`/`push`/`merge`/`rebase`/`reset`/`restore`/`clean`/`fetch`/`stash`/`checkout`) was executed. The ONLY file created by this audit is this report.

---

## 1. Executive Summary

### 1.1 Headline

**The project changed materially between Audit 1 and Audit 2 — approximately 5 hours of active art-pipeline work landed on disk, and the canonical documentation (`AI_CONTEXT/`) has not been updated at all.**

The previous audit's conclusion that the modular wall/floor kit is `PLANNED / NOT IMPLEMENTED` is now **partially superseded**: the *Blender/FBX assets* now exist and pass numeric QA, but **zero Unity integration has occurred** — verified by GUID reference scan.

### 1.2 The seven most important findings

| # | Finding | Class |
|---|---|---|
| **N-01** | `Assets/Art/Environment/Modular/` created 2026-09-29 09:42: `WallSegment_01` + `FloorSegment_01` in 3 revisions each (6 FBX). **All 6 have 0 references from any prefab, scene, material or asset.** | `VERIFIED FACT` + `VERIFIED ABSENCE` |
| **N-02** | **`Crate_01 CP2 was executed** on 2026-09-29 09:17 — `Crate_01_Fragments_FINAL.fbx` (57 404 B) + `Crate_01_Working.blend` (130 858 B)`. `AI_CONTEXT` still records CP2 as «**НЕ АВТОРИЗОВАН**». | `CONFLICTED` |
| **N-03** | **`Working/reports/` now contains 7 QA reports** (untracked). 4 QA reports explicitly state that `AI_CONTEXT` is stale and knows nothing of the new assets. | `VERIFIED FACT` |
| **N-04** | **The 7 QA reports contradict each other in 4 documented places** — most sharply: shimmer «NOT REPRODUCED» (14:58) → «REPRODUCED AND CAUSED» (15:20), and prior fracture PASS claims are **invalidated** (fragments cover only 41.7 % of the intact surface). | `CONFLICTED` |
| **N-05** | **Zero Unity-side changes.** 42 `.cs` files (unchanged), 13 environment prefabs (unchanged), 9 materials (unchanged), `ArenaGenerator.cs` last modified **2026-09-26 17:09** — i.e. **3 days BEFORE** the modular kit existed. | `VERIFIED ABSENCE` |
| **N-06** | **Unity MCP port 8080 is now OPEN** (was closed during Audit 1). A live runtime audit is now technically possible for the first time. | `VERIFIED FACT` |
| **N-07** | **`Working/` grew from 66,5 MB / 180 files → 205,7 MB / 646 files** in 5 hours. Still **not covered by `.gitignore`**. `cover_realtime_diag/` alone is 212 files. | `VERIFIED FACT` |

### 1.3 State change since Audit 1

| Metric | Audit 1 | **Audit 2** | Δ |
|---|---|---|---|
| `git status` entries | 79 | **82** | +3 |
| untracked entries | 62 | **65** | +3 |
| `Working/` size | 66,5 MB | **205,7 MB** | **+139,2 MB** |
| `Working/` files | 180 | **646** | +466 |
| `.cs` files | 42 | **42** | 0 |
| Environment prefabs | 13 | **13** | 0 |
| Materials | 9 | **9** | 0 |
| AI_CONTEXT files modified | 8 | **8** | 0 (не обновлялись) |
| `WallSegment`/`FloorSegment` in C# | 0 | **0** | 0 |
| Unity MCP port 8080 | CLOSED | **OPEN** | 🔴 изменилось |
| Blender MCP | UP (empty scene) | **UP** (`Cover_Western_Wooden_Working.blend`) | 🔴 другая сессия |

### 1.4 Bottom line

Documentation is now **more** out of date than at Audit 1, not less. Seven QA reports exist that are not referenced by any canonical document; three confirmed FINAL assets (Wall, Floor, Crate CP2) plus one asset with an open blocking defect (Cover r4) exist that `AI_CONTEXT` has never heard of; and the entire 2026-09-29 art output (466 files, 139 MB) is untracked with no recovery point.

---

## 2. Repository State

### 2.1 Local Git

| Parameter | Value | Source |
|---|---|---|
| Repository | `C:\Users\cyril\WildWestGunslinger\.git` | LOCAL |
| Current branch | **`main`** | LOCAL |
| HEAD | **`040651b1d8963231b814217319b099b7ab5ef16d`** | LOCAL |
| HEAD date / author | 2026-09-25 11:49:35 +0300 · Cyril | LOCAL |
| HEAD message | `feat: add local Gemma reviewer` | LOCAL |
| Total commits | **4** | LOCAL |
| Tracked files | 1091 | LOCAL |
| Local `origin/main` ref | **`040651b`** | LOCAL |
| Branches | `main` (`040651b`), `backup-pre-lfs` (`f9f4899`) | LOCAL |
| Tags | **0** | LOCAL |
| Staged | **0** | LOCAL |
| Recovery branches with upstream | **0** (ни одна ветка не имеет tag/backup-point семантики) | LOCAL |
| Previous audit commits | **0** (аудит 1 не коммитился) | LOCAL |

### 2.2 Working tree — 82 entries

| Category | Count | Detail |
|---|---|---|
| **M** Modified | 15 | 8 × `AI_CONTEXT/*.md`; `TestArena.unity`; `TestArena_Preview.unity`; `EnemyTacticalPlanner.cs`; `ArenaGenerator.cs`; `EnemyDirectionIndicator.cs`; `Mobile_RPAsset.asset`; `PC_RPAsset.asset` |
| **D** Deleted | 2 | `EnemyTacticalEnvironmentScanner_TEST.cs` + `.meta` |
| **??** Untracked | **65** | 5 `WWG_*.prefab` · 5 `WWG_*.mat` · `Assets/Art/Props/` (**20 FBX**) · `Assets/Art/Environment/Modular/` (**6 FBX**) · 15 `Assets/Screenshots/*.png` · `Assets/TestArena.unity` · 2 `_Recovery` · **`Working/` (646 files, 205,7 MB)** |
| Staged | 0 | — |

### 2.3 GitHub

| Parameter | Value |
|---|---|
| Repository | `noxt1/WildWestGunslinger` (ID `1386927322`) |
| **Visibility** | **`private`** |
| Default branch | `main` |
| **main SHA** | **`f932fcc722d22b8150ccf7e9616914324347c6e8`** |
| main date | 2026-09-28T13:40:20Z |
| main message | `Create AI_PRODUCTION_METHODOLOGY.md` |
| Branches | `main` only |
| Tags | **0** |
| Files | 1093 blobs |
| `gh` auth | ✅ active, account `noxt1`, scopes `gist, read:org, repo, workflow` |

### 2.4 Divergence

```
LOCAL  main = 040651b   ← HEAD, also origin/main (STALE)
GITHUB main = f932fcc   ← 2 commits AHEAD of local
```

**Two GitHub-only commits:**
| SHA | Date | Message |
|---|---|---|
| `41da3611` | 2026-09-28T11:21:29Z | `Create AUDIT_SESSION_CONTEXT_2026-09-28.md` |
| `f932fcc7` | 2026-09-28T13:40:20Z | `Create AI_PRODUCTION_METHODOLOGY.md` |

**Local-only:** `backup-pre-lfs` (f9f4899), 0 tags.
**Local-only files:** **0** (274 path differences at Audit 1 were confirmed as git-octal-escaping artifacts of non-ASCII names, not real divergence).

### 2.5 Not-ignored critical paths (confirmed by inventory)

| Path | Size | Tracked? | Scene dependency |
|---|---|---|---|
| `Working/` | **205,7 MB / 646 files** | ❌ **NOT ignored** | source-of-truth for 3 FINAL art assets + all QA |
| `Working/reports/` | 7 MD | ❌ untracked | sole record of QA verdicts |
| `Working/blender_src/` | 6 `.blend` + 6 `.blend1` | ❌ untracked | source of all FINAL FBX |
| `Working/blender_reviews/cover_realtime_diag/` | **212 files** | ❌ untracked | evidence for shimmer root cause |
| `Assets/Art/Environment/Modular/` | 6 FBX | ❌ untracked | **0 refs — not yet used** |
| `Assets/Art/Props/` | 20 FBX | ❌ untracked | **0 refs — not yet used** |
| `Assets/Prefabs/Environment/WWG_*.prefab` | 5 | ❌ untracked | **WIRED into `TestArena.unity`** |
| `Assets/Materials/WWG_*.mat` | 5 | ❌ untracked | **WIRED into `TestArena.unity`** |

---

## 3. Unity State

> **Runtime audit NOT PERFORMED.** Unity MCP port 8080 is now open but no `unityMCP` tools are exposed in this agent session. All Unity facts below are `STATIC VERIFIED` from serialized data on disk.

| Parameter | Value | Class |
|---|---|---|
| Unity version | `6000.3.23f1` | `STATIC VERIFIED` |
| URP | 17.5.0, Deferred renderer | `STATIC VERIFIED` |
| Packages | 46 (incl. `com.coplaydev.unity-mcp` by mutable `#main`) | `STATIC VERIFIED` |
| Scenes | `Game`, `MainMenu`, `TestArena`, `TestArena_Preview` (+4 `_Recovery`) | `STATIC VERIFIED` |
| Build list | MainMenu ✅, TestArena ✅, Game ✅ (empty shell), Desert Demo ❌ | `STATIC VERIFIED` |
| Prefabs | 22 total; 13 environment (8 Western FBX + 5 `WWG_*` cube blockouts) | `STATIC VERIFIED` |
| Materials | 9 project `.mat` + 1 shadergraph (unused) | `STATIC VERIFIED` |
| C# scripts | 42 files, 0 namespace, 0 `.asmdef` | `STATIC VERIFIED` |
| Animator / AnimationClip | **0** | `VERIFIED ABSENCE` |
| NavMesh | **0** usages (`com.unity.ai.navigation` installed but dead) | `VERIFIED ABSENCE` |
| Audio files | **0** | `VERIFIED ABSENCE` |
| Custom tags | 1 (`Enemy`) | `STATIC VERIFIED` |
| Custom layers | **0** | `VERIFIED ABSENCE` |

### 3.1 Broken references — unchanged since Audit 1

| Owner | Field | Count |
|---|---|---|
| `ArenaGenerator` | 7 prop prefab slots | 7 |
| `HUDController` | `xpBar`, `levelText` | 2 |
| `MobileTouchControls` | `playerController`, `gunController`, `playerCamera` | 3 |
| **Total `{fileID: 0}`** | | **12** |

Plus: 4 × `Missing (Mono Script)` (deleted `EnemyTacticalEnvironmentScanner_TEST`, GUID `809b48f6…`, one per enemy prefab); 2 active `DIAG_TEMP_*` objects; 3 unresolvable material GUIDs (15 TMP components + 2 diagnostic cubes).

### 3.2 `TestArena.unity` — canonical development scene (unconfirmed)

`WildWestEnvironmentGenerator` in `TestArena` is wired to **only untracked `WWG_*` prefabs** (cube blockouts) and has **7 of 7 `ArenaGenerator` prop slots = `{fileID: 0}`**.
`TestArena_Preview.unity` has the **inverse** wiring and `previewOnly: 1`, but **all `*Prefabs` arrays empty**.
Neither scene is configured to use the new Modular FBX.

---

## 4. Documentation Inventory

### 4.A PROJECT REALITY DOCUMENTATION

| Path | Bytes | Modified | State | Canonical? |
|---|---:|---|---|---|
| `AI_CONTEXT/README.md` | 4 331 | 2026-09-25 09:20 | tracked, clean | index — **conflicted** |
| `AI_CONTEXT/RULES.md` | 9 649 | **2026-09-28 00:17** | **MODIFIED (uncommitted)** | policy — **3 contradictions** |
| `AI_CONTEXT/PROJECT_STATE.md` | 21 871 | **2026-09-28 00:14** | **MODIFIED** | spec — stale |
| `AI_CONTEXT/ARCHITECTURE.md` | 25 978 | **2026-09-28 00:18** | **MODIFIED** | tech ref — 4 bad figures |
| `AI_CONTEXT/CURRENT_TASK.md` | 24 453 | **2026-09-28 00:13** | **MODIFIED** | operational — **now obsolete** |
| `AI_CONTEXT/CONFIRMED_STATE.md` | 8 048 | **2026-09-28 00:14** | **MODIFIED** | user-gated truth — **now obsolete** |
| `AI_CONTEXT/CHANGELOG.md` | 60 129 | **2026-09-28 00:19** | **MODIFIED** | history — missing Stage 4.4.5 |
| `AI_CONTEXT/ART_PIPELINE.md` | 20 231 | **2026-09-28 00:12** | **MODIFIED** | art domain — **now obsolete** |
| `AI_CONTEXT/VERIFICATION.md` | 2 795 | 2026-09-25 11:54 | **MODIFIED** | process policy — correct |
| `AI_CONTEXT/DEBUGGING.md` | 1 384 | 2026-09-25 09:20 | tracked, clean | procedure — correct |
| `AI_CONTEXT/SPEC_IDEAS.md` | 1 279 | 2026-09-25 09:20 | tracked, clean | notes — conflicts with README |
| `AI_CONTEXT/AI_TOOLCHAIN_AUDIT.md` | 57 093 | 2026-09-25 11:47 | tracked, clean | snapshot — 3 stale claims |

> 🔴 **8 of 12 AI_CONTEXT files are modified but uncommitted, and ALL of them are now out of date** relative to 2026-09-29 work.

### 4.B OPENCODE DOCUMENTATION

| Path | Bytes | State | Class |
|---|---:|---|---|
| `.opencode/skills/wwg-character-art/SKILL.md` | 9 198 | tracked, clean | `SPECIFICATION` (character-art) — **conflicted with reality** |
| `.opencode/skills/EVALS_README.md` | 3 340 | tracked, clean | `LEGACY / FOREIGN` (belongs to `cc-blender-skill`) |
| `.opencode/agents/reviewer.md` | 870 | tracked, clean | `OPERATIONAL` (Gemma reviewer, all tools deny) |
| `.opencode/package.json` | — | **gitignored** | infra |
| `.config/opencode/opencode.json` | 1 937 | 2026-09-26 15:40 | outside repo |
| `.config/opencode/opencode.json.bak` | 1 547 | 2026-09-21 16:43 | outside repo, backup |
| `.config/opencode/opencode.json.bak_triposr_20260926` | 1 633 | 2026-09-22 12:04 | outside repo, backup |

### 4.C CHATGPT DOCUMENTATION

| Path | Bytes | Modified | State | Class |
|---|---:|---|---|---|
| `C:\Users\cyril\Downloads\CHATGPT_CHECKLIST.md` | 15 052 | 2026-09-29 08:08 | outside repo | `ROADMAP` (STAGE 1–20) |
| `C:\Users\cyril\Downloads\CHATGPT_CHECKLIST_before_STAGE19_2026-09-20.md` | 9 337 | 2026-09-29 08:08 | outside repo | `SUPERSEDED` roadmap (STAGE 1–19) |
| `C:\Users\cyril\Downloads\CHATGPT_PROJECT_STATE.md` | 8 864 | 2026-09-29 08:09 | outside repo | `SPECIFICATION` + protected-systems |
| `C:\Users\cyril\Downloads\CHATGPT_WORK_QUEUE.md` | 3 360 | 2026-09-29 08:09 | outside repo | operational queue |
| `Вставленная уценка.md` | — | — | — | ⛔ **NOT PROVIDED** |
| `AI_PRODUCTION_METHODOLOGY.md` (GitHub `f932fcc`) | 30 625 | 2026-09-28 | **local only on GitHub** | `METHODOLOGY` (52 sections) |
| `AUDIT_SESSION_CONTEXT_2026-09-28.md` (GitHub `41da3611`) | 24 322 | 2026-09-28 | **local only on GitHub** | `HISTORICAL` session record |

### 4.D AUDIT DOCUMENTATION (previous audit reports)

| Name | Date | Origin | Scope | Limitations |
|---|---|---|---|---|
| **HARD RECON Report — Part 1** (§1–27) | 2026-09-29, earlier session | agent conversation output, **NOT a file** | Exec summary, sources, inventory, Unity, scenes, prefabs, C#, gameplay, AI, environment, destructibles, character, equipment, Blender, skills, MCP, git, github, docs, GPT, conflicts | **No Unity runtime**; output truncated at §28 |
| **HARD RECON — Continuation 1** (§28–40) | 2026-09-29, same session | agent conversation output, **NOT a file** | Legacy/dup, security, toolchain, doc authority, conflict matrix, GPT audit, P-ahead, D-ahead, user-confirm, high-risk, recon plan, final status | Output truncated inside §39 Фаза 8.3 |
| **HARD RECON — Final continuation** (§39.8.3–40) | 2026-09-29, same session | agent conversation output, **NOT a file** | Stage mapping, checkpoint, phase 9, final status, fact table | Complete |

> 🔴 **The three audit reports exist ONLY as conversation output. They were never written to disk, never committed, and exist nowhere in the repository or on GitHub.**
> Any agent other than this conversation has **zero** record of Audit 1's findings.
> `AUDIT_SESSION_CONTEXT_2026-09-28.md` (on GitHub) is a *different, pre-Audit-1* document.

### 4.E HISTORICAL DOCUMENTATION

No document is explicitly marked `HISTORICAL` or `SUPERSEDED` anywhere. Historical content is embedded inside `CHANGELOG.md` (`> DEPRECATED` blocks) and `AI_TOOLCHAIN_AUDIT.md` (self-corrected claims). `CHATGPT_CHECKLIST_before_STAGE19_2026-09-20.md` is superseded in fact but unmarked.

### 4.F IDEAS / FUTURE

| Item | Where | Class |
|---|---|---|
| `SPEC_IDEAS.md` (Spec Kit / Superpowers / Dagger / n8n / Vibe Kanban verdicts) | `AI_CONTEXT/SPEC_IDEAS.md` | `IDEA` |
| `STAGE 5/13/14/15/17/18/19/20` | `CHATGPT_CHECKLIST.md` | `PLAN` |
| `E0–E5` (Hat → Torso → Legs → Belt → Audit) | `AI_PRODUCTION_METHODOLOGY §43` | `PLAN` |
| `U0–U7` (Unity import & integration) | `AUDIT_SESSION_CONTEXT:923–944` | `PLAN` |
| `Packages A–E` | `AUDIT_SESSION_CONTEXT:464–532` | `PLAN` |

### 4.G NEW — ART QA REPORTS (untracked, unknown to all canonical docs)

| Path | Bytes | Modified | Asset | Verdict |
|---|---:|---|---|---|
| `Working/reports/crate_01_cp2_final_qa.md` | 8 451 | 09-29 09:20 | Crate_01 CP2 | **PASS / FINAL** (10 fragments, 740 tris) |
| `Working/reports/modular_environment_wall_floor_final_qa.md` | 16 515 | 09-29 09:46 | WallSegment_01, FloorSegment_01 | **PASS / FINAL** (1/1/1 each) |
| `Working/reports/cover_western_wooden_final_qa.md` | 12 867 | 09-29 13:04 | Cover_Western_Wooden | **PASS / FINAL** (420 tris, 1/1/1) |
| `Working/reports/wood_language_refinement_r2_final_qa.md` | 11 580 | 09-29 13:52 | Wall, Floor, Cover r2 | PASS gates, **NOT promoted**, awaiting sign-off |
| `Working/reports/wood_language_correction_r3_final_qa.md` | 8 294 | 09-29 14:20 | Wall, Floor, Cover r3 | **r3 candidate, NOT promoted** |
| `Working/reports/cover_shimmer_diagnostic_r3.md` | 8 303 | 09-29 14:58 | Cover shimmer | **NOT REPRODUCED** — later self-corrected |
| `Working/reports/cover_r4_realtime_shimmer_uv_grain_final_qa.md` | 10 782 | 09-29 15:20 | Cover r4 | **SHIMMER CAUSED+fixed; criterion 5 FAIL; r4 NOT promoted** |

---

## 5. ChatGPT Documentation Analysis

### 5.1 Purpose, scope, source

| Doc | Purpose | Scope | Source | Claimed state |
|---|---|---|---|---|
| `CHATGPT_CHECKLIST.md` | Master roadmap + status tracker | STAGE 1–20 | ChatGPT + user | checkpoint = 4.4.5 implemented, awaiting confirmation |
| `CHATGPT_CHECKLIST_before_STAGE19_2026-09-20.md` | Prior roadmap snapshot | STAGE 1–19 | ChatGPT + user | superseded |
| `CHATGPT_PROJECT_STATE.md` | Technical spec + protected systems | whole project | ChatGPT + user | working-systems snapshot dated ~2026-09-18 |
| `CHATGPT_WORK_QUEUE.md` | Operational queue | environment + art | ChatGPT + user | focus = environment art |
| `AI_PRODUCTION_METHODOLOGY.md` | Generic production methodology | process, not project state | ChatGPT + validated OpenCode/Blender workflow | v1.0, 2026-09-28 |
| `AUDIT_SESSION_CONTEXT_2026-09-28.md` | Session record | documentation reconciliation | ChatGPT | reconciliation required, not complete |

### 5.2 Verified / unverified / contradicted

| GPT claim | Class | Reality |
|---|---|---|
| 9-room honeycomb arena, spacing 28 | `VERIFIED FACT` | ✅ `ArenaGenerator.cs:474–502` + scene `seed 12345` |
| Generation pipeline order (8 steps) | `VERIFIED FACT` | ✅ `ArenaGenerator.cs:317–415` exact match |
| `PlayerController` 5 / 1.35 / 3 / 5 | `VERIFIED FACT` | ✅ scene matches |
| GunController `fireRate 0.4`, `attackRange 15` | `VERIFIED FACT` | ✅ scene matches |
| GunController `damage 200` | `PARTIALLY` | Inspector = **100**; code forces `Mathf.Max(damage,200)` → effective 200 |
| `retargetInterval 0.15` | `NOT VERIFIED` | **field absent** from `GunController.cs` |
| Enemy states (7) + Peek (5) | `VERIFIED FACT` | ✅ exact match `EnemyController.cs:15–33` |
| LOS via `RaycastAll` | `VERIFIED FACT` | ✅ 4-point, `EnemyTacticalVision.cs:466–546` |
| `roomSearchConnectionReachDistance ≈ 3` | `CONTRADICTED` | code = **1.4** |
| Bandit detect 16, dmg 10/6 | `CONTRADICTED` | prefab = **18** detect, **10** melee, **5** ranged |
| Script folders `Combat, Core, Enemies/Cover, Level, Player, Procedural, UI` | `CONTRADICTED` | actual 10 dirs incl. `Environment/`, `Performance/`, `Weapons/`; **no `Combat/`** |
| "3 группы по 4 врага" as a *desire* | `SUPERSEDED` | ✅ already implemented: `startingGroups 3` × `size 4` |
| "требуется A* навигация" as a *desire* | `SUPERSEDED` | ✅ `ArenaTacticalMap.cs` 1770 lines, A* + BFS |
| Mobile controls «работают» | `CONFLICTED` | code complete, but **3 refs `{fileID: 0}`** in scene |
| Quaternius / «Konteri» enemy model | `CONTRADICTED` | no such folders; enemies = built-in **Capsule** `10208` |
| Human Character Dummy 178395 = foundation | `PARTIALLY` | folder exists (57 files, 2025-12-04), **not integrated** |
| `Assets/Audio`, `Assets/Resources` as working dirs | `MISLEADING` | **0 files** in each |
| STAGE 4.4.5 «реализован, скомпилирован, протестирован» | `CONTRADICTED` | `AI_CONTEXT`: «FROZEN / PAUSED, технической реализации нет» |
| STAGE 1/2/3/9/10/11/12/16 «не начато» | `CONTRADICTED` | all implemented and wired |
| STAGE 6.8/7/13/14/15/17 «не начато» | `VERIFIED FACT` | ✅ correct, genuinely absent |
| STAGE 8 destructibles «не начато» | `PARTIALLY` | foundation exists (376 lines + 8 prefabs) |
| STAGE 19 = Security, STAGE 20 = Balance | `CONFLICTED` | old checklist: 19 = Balance; `AI_CONTEXT` knows neither |
| Repository visibility = Public | `CONTRADICTED` | GitHub = **private** |
| `Methodology has not been canonicalized into GitHub` | `SUPERSEDED` | it is on GitHub (`f932fcc`) |

### 5.3 Requirements / decisions worth preserving from GPT context

| Item | Class | Preserve? |
|---|---|---|
| Android = primary platform, Windows = secondary | `REQUIREMENT` | ✅ |
| Procedural arena must stay procedural (no hand-placed) | `REQUIREMENT` | ✅ |
| No city/building assets in western scope | `DECISION` | ✅ |
| Environment assembled from replaceable prefab pools | `DECISION` | ✅ |
| Don't mix incompatible low-poly styles | `DECISION` | ⚠️ currently violated (`WWG_*` cubes vs `*_Western_*` FBX) |
| Batch-only Unity integration; `Asset FINAL ≠ повод начинать интеграцию` | `DECISION` | ✅ honoured by all 7 QA reports |
| WallSegment ≈ 4 m / FloorSegment ≈ 4×4 m, 1 mesh → 1 renderer → 1 material → 1 collider | `REQUIREMENT` | ✅ **now delivered as FBX** (1/1/1 verified) |
| Method C (constructive fracture, no Cell/Voronoi/physics) | `DECISION` | ✅ honoured |
| `Col` = FLOAT_COLOR/CORNER, A = metal mask | `DECISION` | ✅ honoured |
| Frozen FBX contract (`FBX_SCALE_NONE`, `bake_anim=False`, `use_visible` forbidden) | `DECISION` | ✅ honoured |
| Implemented → Compiled → Tested → Confirmed (user only) | `PROCESS` | ✅ honoured by QA reports (self-declared PASS, promotion deferred to user) |
| 3 groups × 4 enemies with Bandit/Rusher/Shooter/Tactical roles | `REQUIREMENT` | ✅ satisfied |
| A* or equivalent navigation | `REQUIREMENT` | ✅ satisfied |
| Manual aim (STAGE 6.1) | `REQUIREMENT` | ⛔ **NOT satisfied** — auto-target only |

### 5.4 GPT vs OpenCode — direct contradictions

| Subject | GPT | AI_CONTEXT | Verdict |
|---|---|---|---|
| Stage 3 | Procedural Arena, ⬜ not started | Group AI search distribution, `Tested=YES` | 🔴 same number, different subject |
| Stage 19 | Security/Privacy/Marketplace | does not exist | 🔴 absent from OpenCode |
| Stage 4.4.5 | implemented / compiled / tested | FROZEN / PAUSED, no implementation | 🔴 direct contradiction |
| Crate_01 | not mentioned | CP1 AWAITING APPROVAL, CP2 NOT AUTHORIZED | 🟡 CP2 was executed anyway → AI_CONTEXT now wrong |
| Modular kit | not mentioned | PLANNED, not implemented | 🟡 **now partially delivered** |
| Repo visibility | Public (per AUDIT_SESSION) | not covered | 🔴 reality = private |
| `gh auth` | — | NOT performed | 🔴 reality = authenticated |

---

## 6. OpenCode Documentation Analysis

### 6.1 `AI_CONTEXT` — asset-by-asset

| Doc | Operational rules | Project facts | Workflow | Stale | Conflicting |
|---|---|---|---|---|---|
| `README.md` | read-order, confirmation rule | manifest (incomplete) | **conflicts with `SPEC_IDEAS.md`** | manifest omits `AI_TOOLCHAIN_AUDIT.md` | D2, D3, D4 |
| `RULES.md` | protected files, debugging, reporting | — | — | `dotnet build` unexecutable | D1, D5, D6 — **all 3 still live** |
| `PROJECT_STATE.md` | — | tech spec | — | 41 cs/8 dirs; enemy params; spawn zones; runtime counts | D7, D13, C10 |
| `ARCHITECTURE.md` | — | AI architecture | — | 4 file sizes; "AI-blind" false; mojibake `视线` | D8, DA-29, DA-26 |
| `CURRENT_TASK.md` | 8 open tasks | stopping point | — | **entirely obsolete** (predates 2026-09-29) | — |
| `CONFIRMED_STATE.md` | user-gated only | 6 confirmed items | — | Stage 4.4.4 counts not reproducible | C10, C18 |
| `CHANGELOG.md` | — | history | — | missing Stage 4.4.5 | D10 |
| `ART_PIPELINE.md` | frozen FBX, Method C, Col rules | 3 assets | — | **entirely obsolete** — knows nothing of Wall/Floor/Cover, Crate CP1→FINAL | — |
| `VERIFICATION.md` | evidence policy | — | ✅ correct | — | — |
| `DEBUGGING.md` | debug cycle | — | ✅ correct | — | — |
| `SPEC_IDEAS.md` | — | — | **conflicts with README** | — | D4 |
| `AI_TOOLCHAIN_AUDIT.md` | — | toolchain snapshot (13/15 still correct) | — | 3 stale claims | C1, C2, C3 |

### 6.2 `wwg-character-art/SKILL.md` — audit against reality

| SKILL claim | Reality | Class |
|---|---|---|
| 51-bone shared skeleton, valid skinning/head weights/deformation | **0 armatures**, 0 character FBX in game | `CONTRADICTED` → reclassify as `FUTURE CONTRACT` |
| Protected attachments `WeaponPoint_R/L`, `HolsterPoint`, `BackWeaponPoint` | only one empty `WeaponPoint` Transform exists | `CONTRADICTED` → `FUTURE CONTRACT` |
| "Do NOT rebuild or replace the skeleton" | no skeleton to protect | `NOT APPLICABLE` |
| ~10–11k triangles budget | 0 character triangles | `REQUIREMENT` (target) |
| Kevin Iglesias Human Character Dummy = primary anatomical reference | ✅ `HumanCharacterDummy_M.fbx` / `_F.fbx` exist (2025-12-04), **used as scale reference** in Cover QA (1.8179 m) | `VERIFIED FACT` (as reference only) |
| 5-character family (Player master + 4 enemies) | 0 characters built | `PLAN` |
| Mandatory 4-view visual QA | ✅ followed by all 7 QA reports | `VERIFIED FACT` (operational) |
| "Technical validation ≠ visual acceptance" | ✅ honoured (all promotions deferred) | `VERIFIED FACT` (operational) |

> The **operational half** of this SKILL is working excellently and is being followed. The **descriptive half** describes a system that does not exist.
> **Recommendation:** split the SKILL into `OPERATIONAL QUALITY GATES` (keep, verified working) and `TARGET CHARACTER CONTRACT` (reclassify as `FUTURE CONTRACT`, move to canonical docs).

### 6.3 Skill conflicts (unchanged from Audit 1, 7 total)

| # | Conflict | Severity |
|---|---|---|
| 1 | `unity-game-director` declares itself default for "any Unity work" — contradicts `RULES.md` read-order | HIGH |
| 2 | `unity-gameplay-systems` / `unity-3d-generator` propose creating projects/config — contradicts protected-files | HIGH |
| 3 | `unity-monetization` / `unity-aso-growth` / `unity-analytics-liveops` push SDKs absent from the project | MEDIUM |
| 4 | `blender-rigging-animation` vs `wwg-character-art` — two unlinked rig approaches | MEDIUM |
| 5 | `wireframe-to-3d` duplicated (2 copies) | LOW |
| 6 | `EVALS_README.md` — foreign `cc-blender-skill` artifact in project skills dir | MEDIUM |
| 7 | `wwg-specialized/blender-physics-simulation` targets Blender 5.x; reports now say **5.2.2 LTS** (Audit 1 recorded 5.1.2) | LOW |

---

## 7. Previous Audit Analysis

### 7.1 Which Audit-1 findings still hold

| Audit 1 finding | Audit 2 verdict |
|---|---|
| 79 status entries, HEAD `040651b`, local ≠ GitHub | ✅ **still true** (+3 entries, still no commits) |
| 12 `{fileID: 0}` in `TestArena` | ✅ **still true** |
| 4 × Missing (Mono Script) in enemy prefabs | ✅ **still true** |
| 2 active `DIAG_TEMP_*` | ✅ **still true** |
| 3 unresolvable material GUIDs | ✅ **still true** |
| 0 `Animator` / 0 `Equipment` / 0 sockets | ✅ **still true** |
| Character Foundation not integrated | ✅ **still true** |
| Weapons/equipment/animation absent | ✅ **still true** |
| `Game.unity` empty shell in build list | ✅ **still true** |
| No secrets in repo | ✅ **still true** |
| Repo is `private` | ✅ **still true** |
| 7 untracked `WWG_*` prefabs/materials wired to scene | ✅ **still true** (risk increased) |
| `Working/` not gitignored | ✅ **still true** (now 3.1× larger) |

### 7.2 Which Audit-1 findings are now superseded

| Audit 1 finding | Audit 2 verdict |
|---|---|
| `WallSegment`/`FloorSegment` — **0 matches in project** | 🟡 **SUPERSEDED (partially)**: 6 FBX now exist; still 0 in C#, 0 in Unity |
| Modular kit = `PLANNED / NOT IMPLEMENTED` | 🟡 **SUPERSEDED → `PARTIAL`**: exists as Blender+FBX, 0 Unity integration |
| `Crate_01` CP2 «NOT AUTHORIZED», `Crate_Fragments` empty | 🔴 **SUPERSEDED**: CP2 executed 2026-09-29 09:17, 10 fragments, `PASS / FINAL` |
| `Working/` = 180 files / 66,5 MB | 🔴 **SUPERSEDED**: 646 files / 205,7 MB |
| Blender MCP pointed at empty unsaved scene | 🔴 **SUPERSEDED**: now on `Cover_Western_Wooden_Working.blend`, saved |
| Blender 5.1.2 | 🟡 **UNVERIFIED**: QA reports state **5.2.2 LTS** |
| Unity MCP DOWN | 🔴 **SUPERSEDED**: port 8080 now **OPEN** |

### 7.3 What Audit 1 missed entirely

| Missed | Why it matters |
|---|---|
| 7 QA reports in `Working/reports/` | Sole record of all 2026-09-29 verdicts |
| Modular FBX in `Assets/Art/Environment/Modular/` | 6 production assets, 0 refs |
| 7 `Cover_Western_Wooden` FBX revisions | Most-revised asset, has an open blocking defect |
| `Working/blender_src/{wall_segment,floor_segment,cover}/` | 3 new Blender sources |
| `Working/blender_reviews/approved_reference/` (6 PNG) | Visual reference baseline for new art |
| `Working/blender_reviews/cover_realtime_diag/` (212 files) | Evidence for the shimmer root cause |
| **4 internal contradictions between the QA reports themselves** | New documentation conflict class |
| **The cover fracture defect** (41,7 % coverage) | Blocks FINAL promotion; affects `DestructibleObject` viability |
| **The material-architecture reversal** (report 2 vs report 5) | Approved FINAL assets violate the current material rule |

---

## 8. Reality Reconciliation

### 8.1 Four-state model (EXISTS / INTEGRATED / WIRED / RUNTIME VERIFIED)

| # | System / Asset | EXISTS | INTEGRATED | WIRED | RUNTIME VERIFIED |
|---|---|---|---|---|---|
| 1 | `ArenaGenerator` (9 rooms) | ✅ `.cs` 3864 L | ✅ in `TestArena` | ✅ `player`+materials set, 7 props NULL | ⛔ NOT RUNTIME VERIFIED |
| 2 | `EnemyController` FSM (7+5) | ✅ `.cs` 3738 L | ✅ on 4 prefabs | ✅ tuning set | ⛔ NOT RUNTIME VERIFIED |
| 3 | `EnemyTacticalPlanner` | ✅ `.cs` 1416 L | ✅ on 4 prefabs | ⚠️ `obstacleLayers` empty; 🔴 **path-validation regression** | ⛔ NOT RUNTIME VERIFIED |
| 4 | `ArenaTacticalMap` A* | ✅ `.cs` 1770 L | ✅ in `TestArena` | ✅ cell 1.5 / nodes 2500 | ⛔ NOT RUNTIME VERIFIED |
| 5 | `CoverSystem` + `CoverPoint` | ✅ 514 + 486 L | ✅ GO + 14 prefab CoverPoints | ⚠️ `automaticCover=1` (code default `false`) | ⛔ NOT RUNTIME VERIFIED |
| 6 | `NoiseSystem` + `EnemyHearing` | ✅ 557 + 342 L | ✅ runtime-registered | ✅ 7 emitters | ⛔ NOT RUNTIME VERIFIED |
| 7 | `DestructibleObject` | ✅ 376 L | ✅ 8 prefabs | ⚠️ 7/8 no state groups; `UnityEvent` empty | ⛔ NOT RUNTIME VERIFIED |
| 8 | `WildWestEnvironmentGenerator` | ✅ 1431 L | ✅ in `TestArena` | ⚠️ only **untracked** `WWG_*` cubes | ⛔ NOT RUNTIME VERIFIED |
| 9 | `XPManager` / `UpgradeManager` | ✅ 54 + 265 L | ✅ wired in scene | ✅ | ⛔ NOT RUNTIME VERIFIED |
| 10 | `WaveManager` / `EnemySpawner` | ✅ 162 + 1623 L | ✅ wired | ✅ 3×4 | ⛔ NOT RUNTIME VERIFIED |
| 11 | `EnemyDirectionIndicator` | ✅ 2052 L | ✅ auto-bootstrap | ✅ | ⛔ NOT RUNTIME VERIFIED |
| 12 | `PlayerHealth` / `HUDController` | ✅ | ✅ | 🔴 `xpBar`/`levelText` NULL | ⛔ NOT RUNTIME VERIFIED |
| 13 | `MobileTouchControls` | ✅ 916 L | ✅ in scene | 🔴 **3 refs NULL** | ⛔ NOT RUNTIME VERIFIED |
| 14 | `CameraFollow` | ✅ 18 L | ✅ on Main Camera | ✅ `target` set | ⛔ dead code (0 code refs) |
| 15 | `MobilePerformanceTest` | ✅ 54 L | 🔴 **in shipping scene** | ✅ forces `targetFrameRate=60` | ⛔ |
| 16 | `EnemyHealthBar` | ✅ 106 L | ⛔ **nowhere** | ⛔ | ⛔ fully dead |
| 17 | `DifficultyManager` | ✅ 99 L | ⛔ nowhere | ⛔ 0 consumers | ⛔ inert |
| 18 | `WallSegment_01` FBX | ✅ 3 revs | ⛔ **0 refs** | ⛔ | ⛔ |
| 19 | `FloorSegment_01` FBX | ✅ 3 revs | ⛔ **0 refs** | ⛔ | ⛔ |
| 20 | `Cover_Western_Wooden` FBX | ✅ 5 revs | ⛔ **0 refs** | ⛔ | ⛔ |
| 21 | `Crate_01` CP2 FBX | ✅ 2 files | ⛔ **0 refs** | ⛔ | ⛔ |
| 22 | `Barrel_01` / `Fence_01` FINAL FBX | ✅ | ⛔ 0 refs | ⛔ | ⛔ |
| 23 | `*_Western_*` prefabs (8) | ✅ | ✅ 7 assigned **in Preview only** | 🔴 all NULL in `TestArena` | ⛔ |
| 24 | `WWG_*` prefabs (5) | ✅ untracked | ✅ **wired in `TestArena`** | ✅ | ⛔ |
| 25 | Human Character Dummy | ✅ 57 files | ⛔ nowhere | ⛔ | ⛔ reference only |
| 26 | Animator / AnimationClip | ⛔ **absent** | ⛔ | ⛔ | ⛔ |
| 27 | Equipment / sockets | ⛔ **absent** | ⛔ | ⛔ | ⛔ |
| 28 | Weapon system beyond 1 gun | ⛔ **absent** | ⛔ | ⛔ | ⛔ |
| 29 | Audio content | ⛔ **absent** | ⛔ | ⛔ | ⛔ |
| 30 | NavMesh | ⛔ **absent** | ⛔ | ⛔ | ⛔ |

### 8.2 Reality Reconciliation Matrix

| ID | Domain | Statement | Source | Evidence | Reality | Classification | Conflict | Resolution | Canonical Doc |
|---|---|---|---|---|---|---|---|---|---|
| R2-001 | Unity | Unity 6000.3.23f1, URP 17.5.0 | `PROJECT_STATE.md:7` | `ProjectVersion.txt`, `manifest.json` | matches | `STATIC VERIFIED` | — | none | `PROJECT_STATE.md` |
| R2-002 | Unity | 42 C# files, 9 subdirs | `PROJECT_STATE.md:136` («41 / 8») | filesystem count | **41→42, 8→9** | `CONFLICTED` | D7 | fix doc | `PROJECT_STATE.md` |
| R2-003 | Scene | `TestArena` is the dev scene | GPT | build list + wiring | plausible | `OPEN ISSUE` | dup scene at `Assets/TestArena.unity` | **user decision Q1** | `PROJECT_TRUTH.md` |
| R2-004 | Scene | 12 broken `{fileID: 0}` | *undocumented* | `TestArena.unity` YAML | confirmed | `VERIFIED FACT` | — | record in `OPEN_ISSUES.md` | `OPEN_ISSUES.md` |
| R2-005 | Scene | 4 Missing (Mono Script) | `CURRENT_TASK.md:95–106` | 4 enemy prefabs | confirmed | `VERIFIED FACT` | doc correct | none | `OPEN_ISSUES.md` |
| R2-006 | Scene | 2 active `DIAG_TEMP_*` | *undocumented* | `TestArena.unity` | confirmed | `VERIFIED FACT` | — | `OPEN_ISSUES.md` | `OPEN_ISSUES.md` |
| R2-007 | Scene | `MobileTouchControls` confirmed working | `CONFIRMED_STATE.md:79` | 3 refs NULL | unresolvable statically | `CONFLICTED` | C18 | **runtime test required** | `OPEN_ISSUES.md` |
| R2-008 | Environment | modular kit `WallSegment ≈ 4 m`, 1 mesh/1 renderer/1 material | `ART_PIPELINE.md:155–199`, `RULES.md:56–60`, `METHODOLOGY §26` | `WallSegment_01_FINAL.fbx` QA: 1/1/1, 4.000×0.200×2.000, 280 tris, 0 ngons, 0 non-manifold | **asset EXISTS + QA PASS** | `PARTIALLY VERIFIED` | `1 collider` half not done | record both halves | `Environment/ENVIRONMENT.md` |
| R2-009 | Environment | modular kit is `PLANNED` | Audit 1 | 6 FBX exist | **superseded** | `SUPERSEDED` | — | update | `Environment/ENVIRONMENT.md` |
| R2-010 | Environment | modular kit is `IMPLEMENTED` | *new QA reports* | 0 GUID refs from any prefab/scene | **NOT integrated** | `VERIFIED ABSENCE` | doc overstates | reclassify `PARTIAL` | `Environment/ENVIRONMENT.md` |
| R2-011 | Environment | `ArenaGenerator` uses modular kit | *implied by QA reports* | `ArenaGenerator.cs` mtime 09-26 17:09; kit created 09-29 09:42 | **false** | `VERIFIED ABSENCE` | — | record | `OPEN_ISSUES.md` |
| R2-012 | Art | `Crate_01` CP2 NOT AUTHORIZED | `CURRENT_TASK.md:7–15` | `Crate_01_Fragments_FINAL.fbx` 09-29 09:17; QA `PASS / FINAL` | **executed anyway** | `CONFLICTED` | N-02 | **user decision** | `OPEN_ISSUES.md` |
| R2-013 | Art | Crate_01 has 10 fragments | QA report 09:20 | 10 fragments, 740 tris, reassembly 0.0 m | matches | `STATIC VERIFIED` | — | promote to canonical | `Art/ART_PIPELINE.md` |
| R2-014 | Art | Cover shimmer NOT reproduced | QA 14:58 | QA 15:20 self-corrects: **REPRODUCED AND CAUSED** | superseded | `SUPERSEDED` | C1 | keep both, mark superseded | `Art/ART_PIPELINE.md` |
| R2-015 | Art | Cover fragments pass QA (inflation 0, reassembly 0.0 m) | QA 13:52 + 14:20 | QA 15:20: coverage **41,7 %**, 78 % surface uncovered | **claims invalid** | `CONFLICTED` | C3 | **blocking defect** | `OPEN_ISSUES.md` |
| R2-016 | Art | no image textures, simple PBR (M2) | `ART_PIPELINE.md:94–95` | QA 14:20 uses 14-node **procedural** material graph | **reversed by r3** | `CONFLICTED` | C5 | **user decision** | `Art/ART_PIPELINE.md` |
| R2-017 | Art | `Col` FLOAT_COLOR/CORNER, A = metal mask | `RULES.md:43–44` | all 7 QA reports verify it | matches | `STATIC VERIFIED` | — | none | `Art/ART_PIPELINE.md` |
| R2-018 | Art | frozen FBX contract | `ART_PIPELINE.md:97–115` | reproduced verbatim in 3 QA reports | matches | `STATIC VERIFIED` | — | none | `Art/ART_PIPELINE.md` |
| R2-019 | Art | Method C (no Cell/Voronoi/physics) | `RULES.md:42` | 3 QA reports cite it; none used fracture add-ons | matches | `STATIC VERIFIED` | — | none | `Art/ART_PIPELINE.md` |
| R2-020 | Art | Unity integration batch-only | `RULES.md:50–53` | 5 QA reports state integration not performed | honoured | `STATIC VERIFIED` | — | none | `DECISIONS.md` |
| R2-021 | AI | planner confirmed, no defects | `CONFIRMED_STATE.md` (Stage 3) | `IsPositionValid` returns true unconditionally; `CalculatePositionScore:754–761` dead var | **regression** | `OPEN ISSUE` | P-23 | **user decision Q12** | `AI/AI_SYSTEMS.md` |
| R2-022 | AI | 4-point LOS prevents vision through walls | `ARCHITECTURE.md:167–171` | `EnemyTacticalVision.cs:466–546` | matches statically | `STATIC VERIFIED` | 0.25 s memory latch | record caveat | `AI/AI_SYSTEMS.md` |
| R2-023 | AI | sound pursuit end-to-end | `ARCHITECTURE.md:197–229` | `NoiseSystem`+`EnemyHearing`→`OnNoiseHeard`→`Investigating`→`Searching` | matches statically | `STATIC VERIFIED` | noise overwrites last-known-pos | record | `AI/AI_SYSTEMS.md` |
| R2-024 | AI | planner assigns roles/positions correctly | docs | path validation removed | `OPEN ISSUE` | R2-021 | — | `OPEN_ISSUES.md` |
| R2-025 | Character | 51-bone rig + sockets exist | `wwg-character-art/SKILL.md:283–312` | 0 armatures, 0 sockets | **absent** | `CONFLICTED` | S1 | reclassify as `FUTURE CONTRACT` | `Character/CHARACTER_CONTRACT.md` |
| R2-026 | Character | Human Character Dummy is scale reference | QA report 13:04 | `HumanCharacterDummy_M.fbx` 1.8179 m cited | asset exists | `VERIFIED FACT` (reference only) | — | record | `Character/CHARACTER_CONTRACT.md` |
| R2-027 | Character | no character in game | GPT STAGE 6.8 | player+enemies = primitives | matches | `VERIFIED ABSENCE` | — | none | `PROJECT_TRUTH.md` |
| R2-028 | Combat | weapons/equipment/animation absent | GPT STAGE 7/17 | 0 Animator / Socket / Equipment | matches | `VERIFIED ABSENCE` | — | none | `PROJECT_TRUTH.md` |
| R2-029 | Combat | manual aim required | GPT STAGE 6.1 | auto-target only; no manual aim | **unmet requirement** | `OPEN ISSUE` | — | record as requirement | `REQUIREMENTS.md` |
| R2-030 | Combat | desktop fire input exists | *undocumented gap* | `FireButtonDown` only from mobile code | **absent on PC** | `OPEN ISSUE` | — | record | `OPEN_ISSUES.md` |
| R2-031 | Git | project is a git repo, 0 commits | `AI_TOOLCHAIN_AUDIT.md:390` | 4 commits | superseded | `SUPERSEDED` | C3 | update | `PROJECT_STATE.md` |
| R2-032 | Git | `gh auth login` not performed | `AI_TOOLCHAIN_AUDIT.md:397` | authenticated as `noxt1` | superseded | `SUPERSEDED` | C2 | update | `PROJECT_STATE.md` |
| R2-033 | Git | recovery point exists | `METHODOLOGY §15` | 0 tags, 0 backup branch with point, 82 uncommitted | **absent** | `OPEN ISSUE` | R5 | **blocker** | `OPEN_ISSUES.md` |
| R2-034 | GitHub | repo is public | `AUDIT_SESSION_CONTEXT:27–29` | API `private: true` | contradicted | `CONFLICTED` | G1 | update | `PROJECT_STATE.md` |
| R2-035 | GitHub | local == GitHub | implied | local 2 commits behind | **diverged** | `VERIFIED FACT` | — | record | `OPEN_ISSUES.md` |
| R2-036 | GitHub | `Methodology has not been canonicalized` | `AUDIT_SESSION_CONTEXT:142` | on GitHub since `f932fcc` | superseded | `SUPERSEDED` | DA-22 | update | `RECONCILIATION.md` |
| R2-037 | Docs | `README.md` files contain verified facts | `README.md:5` | 20 internal contradictions | contradicted | `CONFLICTED` | D2 | rewrite | `RECONCILIATION.md` |
| R2-038 | Docs | Stage 3 is not started | GPT | both arena and AI group stages exist | contradicted | `CONFLICTED` | HR-3 | **user decision Q4** | `MASTER_PLAN.md` |
| R2-039 | Docs | Stage 19 = Security | GPT new | old = Balance; AI_CONTEXT = neither | contradicted | `CONFLICTED` | DA-4 | **user decision Q4** | `MASTER_PLAN.md` |
| R2-040 | Docs | `AI_CONTEXT` is current | `README.md:24` | 0 of 12 files mention 2026-09-29 work | contradicted | `CONFLICTED` | N-03 | rewrite | `PROJECT_TRUTH.md` |
| R2-041 | Docs | 7 QA reports exist | *undocumented* | `Working/reports/` | confirmed | `VERIFIED FACT` | — | promote to canonical | `Art/ART_PIPELINE.md` |
| R2-042 | Skills | `wwg-character-art` describes project reality | SKILL | describes a non-existent system | contradicted | `CONFLICTED` | R2-025 | split skill | `Character/CHARACTER_CONTRACT.md` |
| R2-043 | Skills | 88 skills available, no project conflicts | *Audit 1 assumption* | 7 conflicts incl. `unity-game-director` | contradicted | `CONFLICTED` | §6.3 | record | `OPEN_ISSUES.md` |
| R2-044 | Toolchain | Blender 5.1.2 | `AI_TOOLCHAIN_AUDIT.md` | QA reports say **5.2.2 LTS** | unverified conflict | `NOT VERIFIED` | — | verify | `PROJECT_STATE.md` |
| R2-045 | Toolchain | Unity MCP down | `AI_TOOLCHAIN_AUDIT.md:69` | port 8080 **OPEN** | superseded | `SUPERSEDED` | N-06 | update | `PROJECT_STATE.md` |
| R2-046 | Security | no secrets in repo | *undocumented* | full-text scan clean | confirmed | `VERIFIED FACT` | — | record once | `PROJECT_TRUTH.md` |
| R2-047 | Runtime | AI behaviour works as documented | all docs | no Unity runtime access | unverifiable | `NOT VERIFIED` | — | **runtime pass required** | `OPEN_ISSUES.md` |
| R2-048 | Runtime | `Stage 4.4.4` counts (6/16/48/255) | `PROJECT_STATE.md:246` | current scene configs cannot produce them | contradicted | `CONFLICTED` | C10 | runtime re-test | `OPEN_ISSUES.md` |
| R2-049 | Runtime | FPS 56.4–60, MSAA 4, renderScale 1 | `PROJECT_STATE.md:89–90` | settings on disk, uncommitted | not reproducible | `NOT VERIFIED` | — | runtime test | `OPEN_ISSUES.md` |
| R2-050 | Docs | Audit 1 findings preserved | *this session* | Audit 1 exists only in conversation | at risk | `OPEN ISSUE` | §4.D | **write audit 1 to disk** | `History/` |

---

## 9. Character Reconciliation

### 9.1 Chain verification

| Link | Expected | Actual | Class |
|---|---|---|---|
| Character Vision | `SKILL.md:33–53` Kevin Iglesias reference | reference exists & used for scale | `DECISION` |
| Character Requirements | `ART_PIPELINE.md:247` (hat→shirt→pants→boots→belt→holster→revolver) | no work begun | `PLAN` |
| Character Contract | `AUDIT_SESSION_CONTEXT:614–708` (skeleton, sockets, orientation, scale) | **documented on GitHub only**, never applied | `PLAN` |
| Dummy/Doomy base | `Human Character Dummy` (Asset Store 178395) | ✅ 57 files present, **0 in-game use** | `VERIFIED FACT` (asset), `VERIFIED ABSENCE` (use) |
| Topology | — | `HumanCharacterDummy_M.fbx` 1.8179 m, `_F.fbx`, 7 `HumanDummy_*.mat` | `VERIFIED FACT` |
| Clothing | — | ⛔ none | `VERIFIED ABSENCE` |
| Rig | 51 bones | ⛔ **0 armatures** | `CONFLICTED` → reclassify `FUTURE CONTRACT` |
| Skinning | valid weights | ⛔ none | `VERIFIED ABSENCE` |
| Avatar | `Avatar` asset | ⛔ none | `VERIFIED ABSENCE` |
| Animator | required | ⛔ **0 `Animator` components** | `VERIFIED ABSENCE` |
| Animation Clips | Idle/Walk/Run/Combat/Reload/Hit/Death | ⛔ **0 `.anim`/`.controller`** | `VERIFIED ABSENCE` |
| Equipment sockets | `WeaponPoint_R/L`, `HolsterPoint`, `BackWeaponPoint` | ⛔ none | `VERIFIED ABSENCE` |
| Weapons | revolver | ⛔ none (gun is abstract `GunController`) | `VERIFIED ABSENCE` |
| Player integration | character replaces capsule | 🔴 `Player` = built-in **Capsule** `10208` | `VERIFIED ABSENCE` |
| Enemy integration | 4 character classes | 🔴 all 4 = built-in **Capsule** `10208` | `VERIFIED ABSENCE` |

### 9.2 Character assessment

**Character Foundation is `VERIFIED ABSENT` in the game, and there is no partial integration to unwind.**

The only real character asset is the untouched vendor `Human Character Dummy` package. Its FBX has been read (height 1.8179 m, armspan 1.8227, feet Z = 0.0004) purely to calibrate the cover prop height — a **correct and valuable** use, but it is a *scale reference*, not a foundation in progress.

**`wwg-character-art/SKILL.md` must be split** — its operational gates are working and should be preserved verbatim; its descriptive section must become a `FUTURE CONTRACT` document.

### 9.3 Blender sources

| Source | Path | State |
|---|---|---|
| `WallSegment_01_Working.blend` | `Working/blender_src/wall_segment/` | ✅ exists (99 507 B, 09-29 14:15) |
| `FloorSegment_01_Working.blend` | `Working/blender_src/floor_segment/` | ✅ exists (101 021 B, 09-29 14:15) |
| `Cover_Western_Wooden_Working.blend` | `Working/blender_src/cover/` | ✅ exists (130 282 B, **open in Blender right now**) |
| `Crate_01_Working.blend` | `Working/blender_src/crate/` | ✅ modified 09-29 09:17 (CP2) |
| `Barrel_01_Working.blend` | `Working/blender_src/barrel/` | ✅ 09-27 (FINAL) |
| `Fence_01_Working.blend` | `Working/blender_src/fence/` | ✅ 09-27 (FINAL) |
| Character `.blend` | — | ⛔ **none** |

> `WWG_Doomy_Cowboy_REFINED.blend` (Audit 1 §15 request) — **still not found. Still does not exist.**

---

## 10. Enemy / AI Reconciliation

### 10.1 Per-system

| System | EXISTS | INTEGRATED | WIRED | Config notes | Runtime | Doc claim |
|---|---|---|---|---|---|---|
| `EnemyController` (3738 L) | ✅ | ✅ 4 prefabs | ✅ | `showVision=1`, `showRuntimeState=1` (debug ON) | ⛔ NOT RUNTIME VERIFIED | ✅ AI Stage 1 `CONFIRMED` |
| `EnemyTacticalVision` (2268 L) | ✅ | ✅ runtime-added | ✅ 4 LineRenderers/enemy | `Physics.DefaultRaycastLayers` (no custom layers exist) | ⛔ | ✅ 4-point LOS |
| `EnemyTacticalPlanner` (1416 L) | ✅ | ✅ 4 prefabs | ⚠️ `obstacleLayers` empty | 🔴 **regression: path validation removed** | ⛔ | ⚠️ AI Stage 3 `Tested=YES` |
| `EnemyHealth` (440 L) | ✅ | ✅ 4 prefabs | ✅ `xpPrefab`, `enemyBulletPrefab` | creates a Canvas per enemy | ⛔ | ✅ |
| `EnemyBullet` (374 L) | ✅ | ✅ | ✅ | trigger only; `OnCollisionEnter` dead | ⛔ | ✅ |
| `EnemySpawner` (1623 L) | ✅ | ✅ scene | ✅ 4 prefabs + zones | `groundMask` empty; ~200 lines diagnostics | ⛔ | ✅ Stage 9 |
| `CoverPoint` (486 L) | ✅ | ✅ 14 on prefabs | ✅ | green sphere per point in-game | ⛔ | ✅ |
| `CoverSystem` (514 L) | ✅ | ✅ GO | ⚠️ `automaticCover=1` vs code `false` | full A* per candidate cover | ⛔ | ⚠️ says `false` |
| `NoiseSystem` (557 L) | ✅ | ✅ runtime | ✅ | `FindObjectsByType` **every frame** | ⛔ | ✅ |
| `WaveManager` (162 L) | ✅ | ✅ scene | ✅ | no cap, no win condition | ⛔ | ✅ |
| `ArenaTacticalMap` (1770 L) | ✅ | ✅ scene | ✅ | hardcoded `"Room_2_Combat"` debug ~110 L | ⛔ | ✅ AI Stage 2 |
| `EnemyHearing` (342 L) | ✅ | ✅ runtime | ✅ | 5 unused public methods | ⛔ | ✅ |
| `CombatPhysics` (190 L) | ✅ | ⛔ | ⛔ | **fully dead** — hitscan LOS unused | ⛔ | not documented |
| `EnemyHealthBar` (106 L) | ✅ | ⛔ | ⛔ | **fully dead** | ⛔ | not documented |
| `EnemyTacticalEnvironmentScanner` | ⛔ **deleted** | ⛔ | 🔴 4 orphan slots | regression source | ⛔ | `CURRENT_TASK.md:93` records deletion |

### 10.2 Known problems — status

| Problem | Code mitigation | Class of claim |
|---|---|---|
| Vision through walls | ✅ 4-point multi-height `RaycastAll`, other enemies block, everything else blocks | `STATIC VERIFIED` |
| Losing target → direct rush | ✅ 3.5 s memory, `StartInvestigation` → room route | `STATIC VERIFIED` (noise overwrites last-known-pos) |
| Corner navigation | ✅ 6-candidate avoidance + 0.45 s stuck detector | `STATIC VERIFIED` — but `MoveOutToPeek:2778–2786` bypasses all of it |
| Distant rooms / routing | ✅ BFS + per-room A* | `STATIC VERIFIED` — but **3 independent connectivity algorithms** can disagree |
| Sound pursuit | ✅ end-to-end, wall attenuation | `STATIC VERIFIED` |
| Cover | ✅ selection + reachability + destruction invalidation | `STATIC VERIFIED` |
| Tactical planning | ⚠️ **REGRESSION** — path validation deleted | `OPEN ISSUE` |
| Status/debug markers | ⚠️ `OnGUI` state labels + 4 LineRenderers on every enemy, debug ON by default | `OPEN ISSUE` (performance + polish) |
| **All of the above at runtime** | — | ⛔ **`NOT RUNTIME VERIFIED`** |

---

## 11. Environment Reconciliation

### 11.1 WallSegment / FloorSegment target architecture vs reality

| Contract element | Target | WallSegment_01 actual | FloorSegment_01 actual |
|---|---|---|---|
| Length | ≈ 4 m | ✅ **4.000 m** | ✅ **4.000 m** |
| Width/depth | ≈ 4×4 m | 0.200 × 2.000 m | 4.000 × 0.080 m |
| **One mesh** | 1 | ✅ **1** | ✅ **1** |
| Multiple logs/boards inside | yes | ✅ 10 islands | ✅ 16 boards summing to exactly 4.000 m |
| **One MeshRenderer** | 1 | ✅ 1 (1 FBX object) | ✅ 1 |
| **One shared material** | 1 | ✅ 1 slot | ✅ 1 slot |
| **One collider** | 1 | ⛔ **not done** (Unity side) | ⛔ **not done** |
| Reusable variants | required | ✅ repeat 4.000 m, joint gap **0.0 m**, max error 0.0 | ✅ repeat both axes, gaps 0.0 |
| minY = 0 | required | ✅ **0.0** | ✅ **0.0** |
| Triangles | mobile-appropriate | ✅ 280 (FINAL) / 200 (r3) | ✅ 320 (FINAL) / 320 (r3) |

**Verdict: `PARTIALLY IMPLEMENTED` — 6 of 7 contract elements satisfied at asset level; the collider half is Unity work that has not started.**

### 11.2 Integration status

| Item | Status |
|---|---|
| Blender sources | ✅ 2 |
| FBX exported (3 revisions each) | ✅ 6 |
| QA passed (numeric + visual + repetition) | ✅ yes |
| **Unity prefab created** | ⛔ **NO — 0 GUID references** |
| **Wired into `ArenaGenerator`** | ⛔ **NO — 0 mentions in 42 `.cs` files** |
| **`WallSegment`/`FloorSegment` class** | ⛔ **does not exist** |
| **`ArenaGenerator` updated** | ⛔ **mtime 09-26 17:09, 3 days before the kit existed** |
| **Runtime verified** | ⛔ **NO** |

### 11.3 Legacy environment state (unchanged)

| Item | Status |
|---|---|
| `ArenaGenerator` walls | 🔴 `GameObject.CreatePrimitive(Cube)` per segment — **anti-pattern per `RULES.md:56–60`** |
| `ArenaVisualStyle` | 🔴 spawns 1 cube plank per ~0.785 m + 4 beams → **~2 000–3 000 extra objects**, still the dominant cost |
| `Western_Log_Wall_Segment.prefab` | 🔴 **8 MeshRenderers** (contract wants 1) |
| `Western_Plank_Wall_Segment.prefab` | 🔴 **14 MeshRenderers** (contract wants 1) |
| `TestArena` prop wiring | 🔴 **7 of 7 `ArenaGenerator` prop slots NULL** |
| `TestArena_Preview` prop wiring | 🟡 7 assigned, but all `*Prefabs` arrays empty, `previewOnly=1` |
| `WWG_*` prefabs | ⚠️ 5 × **built-in Cube** blockouts, untracked, wired — style mix vs FBX assets |
| 8 × `*_Western_*` FBX prefabs | ⚠️ 20 FBX, all Albedo byte-identical, all Normal byte-identical |

---

## 12. Combat Reconciliation

| Item | EXISTS | INTEGRATED | WIRED | Runtime | Note |
|---|---|---|---|---|---|
| `GunController` | ✅ 621 L | ✅ Player + FireButton (**duplicate**) | ⚠️ FireButton's `weaponPoint` points at Player root, not `Player/WeaponPoint` | ⛔ | `damage` forced ≥200; no ammo/reload/recoil/swap |
| `Bullet` | ✅ 232 L | ✅ | ✅ | ⛔ | sphere, 0.075 world hit radius, `OnCollisionEnter` dead |
| `EnemyBullet` | ✅ 374 L | ✅ 4 prefabs | ✅ | ⛔ | sphere, 0.06 radius, 2 of 3 handlers live |
| `PlayerHealth` | ✅ 102 L | ✅ | ✅ | ⛔ | HP 100, no regen, no i-frames |
| `DestructibleObject` | ✅ 376 L | ✅ 8 prefabs | ⚠️ 7/8 no state groups, `UnityEvent` empty, debris only on Fence | ⛔ | 🔴 **cover fracture is 41,7 % complete → `DestructibleObject` viability at risk** |
| Damage flow | ✅ | ✅ | ✅ | ⛔ | Bullet + EnemyBullet → `PlayerHealth`/`EnemyHealth`/`DestructibleObject` |
| Manual aim | ⛔ | ⛔ | ⛔ | ⛔ | auto-target only — **unmet `REQUIREMENT`** |
| Desktop fire input | ⛔ | ⛔ | ⛔ | ⛔ | `FireButtonDown` only reachable from mobile code |
| Weapon variety | ⛔ | ⛔ | ⛔ | ⛔ | 1 hardcoded gun |
| Equipment | ⛔ | ⛔ | ⛔ | ⛔ | — |
| Audio | ⛔ | ⛔ | ⛔ | ⛔ | 0 files; `SFXVolume` unused; `MusicVolume` drives master |
| Crosshair/HUD | ✅ | ⚠️ | 🔴 `xpBar`/`levelText` NULL, 15 TMP materials unresolved | ⛔ | — |

---

## 13. Git Reconciliation

| ID | Claim / Fact | Source | Reality | Class |
|---|---|---|---|---|
| G-01 | branch = `main` | expectation | ✅ `main` | `VERIFIED FACT` |
| G-02 | HEAD | — | `040651b` (2026-09-25 11:49:35 +0300) | `VERIFIED FACT` |
| G-03 | origin URL | — | `https://github.com/noxt1/WildWestGunslinger.git` | `VERIFIED FACT` |
| G-04 | local `origin/main` | — | `040651b` — **STALE** | `VERIFIED FACT` |
| G-05 | GitHub `main` | — | `f932fcc` (2026-09-28T13:40:20Z) | `VERIFIED FACT` |
| G-06 | divergence | — | **local behind 2 commits** | `VERIFIED FACT` |
| G-07 | uncommitted | — | 82 entries (15 M / 2 D / 65 ??) | `VERIFIED FACT` |
| G-08 | staged | — | 0 | `VERIFIED FACT` |
| G-09 | deleted files | — | 2 (`EnemyTacticalEnvironmentScanner_TEST.cs` + meta) | `VERIFIED FACT` |
| G-10 | untracked critical assets | — | 5 `WWG_*.prefab` (wired!), 5 `.mat` (wired!), 26 FBX, 646 files in `Working/` | `VERIFIED FACT` |
| G-11 | ignored | — | `Library/ Builds/ Logs/ obj/ .vs/ .utmp/ UserSettings/ .opencode/node_modules` | `VERIFIED FACT` |
| G-12 | **NOT ignored** | — | **`Working/` 205,7 MB**; `Assets/Screenshots/`; `Assets/Art/Props/`; `Assets/Art/Environment/Modular/`; `Assets/TestArena.unity` | `VERIFIED FACT` |
| G-13 | recovery branches | — | `backup-pre-lfs` exists but is **a duplicate of the first commit** (`f9f4899` vs `44c2f15`, same message) — **not a recovery point** | `VERIFIED FACT` |
| G-14 | tags | — | **0 local, 0 remote** | `VERIFIED FACT` |
| G-15 | previous audit commits | — | **0** | `VERIFIED FACT` |
| G-16 | LFS | — | 7 rules; 1 non-matching (en-dash vs hyphen) | `VERIFIED FACT` |
| G-17 | `.gitignore` readability | — | comments mojibake | `VERIFIED FACT` |
| G-18 | destructive ops executed this session | — | **NONE** | `VERIFIED FACT` |

> `backup-pre-lfs` (`f9f4899`, `chore: initialize WWG project baseline`) is a **pre-existing orphan** whose message duplicates `44c2f15`. It is not a safety net.

---

## 14. Documentation Conflicts

> Full detail in `DOCUMENTATION_CONFLICTS.md` (to be created post-APPROVE). Summary of **31 tracked conflicts** (D1–D20, S1, C1–C18, G1–G5, R1–R9 carried from Audit 1) **plus 4 new QA-internal conflicts (Q1–Q4)**.

### 14.1 New conflicts introduced by the 2026-09-29 QA reports

| ID | Source A | Source B | Subject | A claims | B claims | Reality | Canonical statement |
|---|---|---|---|---|---|---|---|
| **Q1** | `cover_shimmer_diagnostic_r3.md` 14:58 | `cover_r4_realtime_shimmer_uv_grain_final_qa.md` 15:20 | Cover shimmer | «**NOT REPRODUCED**. No geometry defect found» | «**REPRODUCED AND CAUSED**» — prior report was a methodological error (Cycles/offline vs EEVEE realtime) | B is correct; A self-corrected | `SHIMMER = CAUSED (UV/Col mismatch + viewport visibility) — FIXED in r4, not promoted` |
| **Q2** | `wood_language_refinement_r2_…` + `wood_language_correction_r3_…` | `cover_r4_…` | Cover fracture QA | «triangle inflation 0», «reassembly deviation 0.0 m» = **PASS** | «those checks only compared vertex coincidence… **41,7 % coverage, 78 % surface uncovered**» | B is correct; A invalidated | `Cover fracture is INCOMPLETE — blocks FINAL promotion` |
| **Q3** | `cover_western_wooden_final_qa.md` 13:04 / `wood_language_…r3` 14:20 | `cover_r4_…` 15:20 | Cover geometry state | intact = **420 tris** (r3) | intact = **604 faces / 1028 tris** (r4) | r4 reverted intact to r2 density without documenting it | `UNRECONCILED — needs author clarification` |
| **Q4** | `modular_environment_wall_floor_final_qa.md` 09:46 | `wood_language_correction_r3_…` 14:20 | Material architecture | «simple Principled PBR, **no image textures, no noise**» (M2 rule) | «**my premise was wrong** — the approved assets are texture-free **procedural**… 14-node graph» | r3 supersedes; but the r2-09:46 `*_FINAL.fbx` **violates the current rule** | `M2 rule REVISED at r3 — approved FINAL assets now non-conformant. USER DECISION REQUIRED` |

### 14.2 Conflicts escalated to critical

| ID | Conflict | Why critical |
|---|---|---|
| **R5** | No verified recovery point; 82 uncommitted entries; `Working/` 205,7 MB | Any destructive op loses 466 files / 139 MB of untracked work, incl. 3 FINAL sources |
| **HR-2** | GPT says STAGE 1–3/9–12 «не начато»; all implemented | Agent may re-implement working systems |
| **HR-3** | «Stage 3» = 2 different subjects | Agent may rewrite `ArenaGenerator`, violating a frozen rule |
| **S1 / R2-025** | `wwg-character-art` describes 51-bone rig + 4 sockets; all absent | Agent may build against non-existent attachment points |
| **C16** | Modular kit rule; violated by `ArenaVisualStyle` and legacy prefabs | Agent may not know the dominant perf cost |
| **R2-016 / Q4** | M2 material rule reversed at r3; approved FINALs now non-conformant | Art pipeline rule incoherent |
| **R2-012** | `Crate_01` CP2 «NOT AUTHORIZED» but executed | Process gate bypassed without a recorded approval |

---

## 15. Missing Information

| # | Missing | Blocks |
|---|---|---|
| M-01 | Written record of **Audit 1** (§1–27) | traceability of 30 prior findings |
| M-02 | **Runtime evidence** for any AI/combat claim | `RUNTIME VERIFIED` class unusable |
| M-03 | `Вставленная уценка.md` (GPT source) | GPT analysis completeness |
| M-04 | Local copies of the 2 GitHub-only MD | methodology + audit context unavailable locally |
| M-05 | **User approval record** for `Crate_01` CP2 execution | process gate integrity |
| M-06 | `AI_CONTEXT/ART_PIPELINE.md` "НОВАЯ АРХИТЕКТУРА" section (cited by QA report 2 L13–15) | the modular-kit contract's canonical location |
| M-07 | Blender version actually installed | `AI_TOOLCHAIN_AUDIT` says 5.1.2, QA reports say 5.2.2 LTS |
| M-08 | Whether `Assets/Kevin Iglesias` package is licensed for commercial use | release-readiness |
| M-09 | Android permissions / manifest / signing config | release-readiness |
| M-10 | Author identity / session ownership of the 2026-09-29 uncommitted work | `RULES.md:19–22` compliance |
| M-11 | Any `anim`/`controller`/Avatar anywhere in project history | character/animation state |
| M-12 | Whether the 2 `DIAG_TEMP_*` objects and `MobilePerformanceTest` are intentional | scene hygiene |

---

## 16. Unverified Claims

> These claims appear in documentation and **cannot be confirmed** without a live Unity runtime.

| ID | Claim | Source | Why unverified |
|---|---|---|---|
| U-01 | «Stage 3 group search distribution works» | `CONFIRMED_STATE.md`, `CHANGELOG.md:652` | no runtime; plus known regression |
| U-02 | «Stage 4.4.4 confirmed: 6 cliffs / 16 trees / 48 bushes / 255 grass» | `PROJECT_STATE.md:246` | current scene configs cannot produce these numbers |
| U-03 | «FPS 56.4 → ~60 after renderScale 1» | `PROJECT_STATE.md:89` | screenshot only, uncommitted setting |
| U-04 | «MSAA 4, no regression» | `PROJECT_STATE.md:90` | screenshot only |
| U-05 | «Enemy takes ~10 k vertices, ~5 k tris» | `PROJECT_STATE.md:227` | editor-only profile, runtime unknown |
| U-06 | «Mobile dynamic joystick + hold fire работают» | GPT `WORK_QUEUE` | 3 refs NULL |
| U-07 | «Bullet/target ring работают» | GPT `WORK_QUEUE` | no runtime |
| U-08 | «FPS 60, 0 console errors» | `CHANGELOG.md:480` | historical, settings changed since |
| U-09 | «arrow direction dot = 1.000000, error 0.0000°» | `CHANGELOG.md:549` | computed offline, not in-engine |
| U-10 | «Vision does not see through walls» | `ARCHITECTURE.md:167` | code verified; `DefaultRaycastLayers` behaviour unproven in-engine |
| U-11 | «MobileTouchControls self-resolves refs» | *hypothesis* | no code path confirmed |
| U-12 | All modular-kit / cover / crate art verdicts | 7 QA reports | Blender-only; **zero Unity contact** |
| U-13 | `mesh_smooth_type='OFF'` does not carry flat shading | `ART_PIPELINE.md:114` | observed on Blender reimport; **Unity reimport not tested** |

---

## 17. Recommended Canonical Documentation Structure

> **Recommendation only. NOT created. Awaiting APPROVE.**

```
Documentation/
├── README.md                      ← ENTRY POINT: reading order + authority rules
├── PROJECT_TRUTH.md               ← §18.1 — current reality, no roadmap
├── PROJECT_STATE.md               ← §18.2 — git/unity/scenes/assets with statuses
├── REQUIREMENTS.md                ← §18.3 — confirmed requirements only
├── DECISIONS.md                   ← §18.4 — approved decisions, DEC-xxx IDs
├── MASTER_PLAN.md                 ← §18.5 — new canonical numbering + legacy crosswalk
├── OPEN_ISSUES.md                 ← §18.6 — CRITICAL/HIGH/MED/LOW + evidence
├── RECONCILIATION.md              ← §18.7 — claim → source → finding → decision → canonical home
├── DOCUMENTATION_CONFLICTS.md     ← §18.8 — DC-xxx conflict register
├── Verification/
│   ├── VERIFICATION_POLICY.md     ← from VERIFICATION.md (proven to work)
│   └── RUNTIME_VERIFICATION.md    ← NEW: what is and isn't runtime-proven
├── AI/
│   └── AI_SYSTEMS.md              ← FSM, perception, planner, cover, noise, A*
├── Character/
│   ├── CHARACTER_CONTRACT.md      ← ← split out of wwg-character-art (FUTURE CONTRACT)
│   └── CHARACTER_RECONCILIATION.md
├── Enemy/  Combat/  Environment/  Weapons/  Progression/  UI/
├── Art/
│   ├── ART_PIPELINE.md            ← + 7 QA reports promoted
│   └── QA_REPORT_INDEX.md         ← NEW: index of Working/reports
└── History/
    ├── GPT_CONTEXT/               ← CHATGPT_* + AUDIT_SESSION_CONTEXT
    ├── Audit1/                    ← ← written from conversation (M-01)
    └── LEGACY_INDEX.md
```

### 17.1 Deviations from the ТЗ proposal, and why

| ТЗ proposal | Recommendation | Reason |
|---|---|---|
| `Documentation/` at root | ✅ as proposed | clean separation from `AI_CONTEXT` |
| — | ➕ `Verification/` subfolder | `VERIFICATION.md` is the **only** doc that has demonstrably worked; isolate it |
| — | ➕ `Art/QA_REPORT_INDEX.md` | 7 QA reports have no index; the whole art pipeline is invisible |
| — | ➕ `History/Audit1/` | Audit 1 exists only in conversation (M-01) |
| `Weapons/`, `Progression/`, `UI/` as full dirs | keep, but seed with **absence statements** + open issues | empty dirs are worse than explicit "NOT IMPLEMENTED" |

---

## 18. Migration Plan

> **PLAN ONLY — NOT EXECUTED.** Every step requires separate user approval per ТЗ §28.

### 18.1 `PROJECT_TRUTH.md` — must answer for a cold-start agent

1. What the project is (mobile-first western arena shooter, Android primary, PC test).
2. Unity 6000.3.23f1 + URP 17.5.0 Deferred; **2 quality levels** (Mobile / PC RP assets).
3. Development scene = `Assets/Scenes/TestArena.unity` — **⚠️ pending user confirmation** (duplicate at `Assets/TestArena.unity`).
4. What EXISTS and is WIRED (table from §8.1).
5. What is **NOT INTEGRATED** despite existing (all 13 new FBX; all QA-approved art).
6. What is `VERIFIED ABSENT` (weapons/equipment/animation/audio/NavMesh/manual aim/desktop fire input).
7. What is broken (12 refs, 4 Missing Script, 2 DIAG_TEMP, 3 materials, dead code).
8. **What is not runtime verified** (all of it — list the U-01…U-13 set).
9. Hard constraints (procedural arena stays procedural; batch-only Unity integration; no image-texture rule **currently under revision**; Android perf rule).
10. Pointer map to the rest of `Documentation/`.

### 18.2 `PROJECT_STATE.md` — every item gets a status

Git (branch/HEAD/divergence/82 entries/recovery gap) · Unity (version/packages/build list) · scenes · scripts (42) · assets (untracked inventory) · characters (absent) · AI (statically verified, runtime pending) · environment (partial) · combat (partial) · UI (partial) · Blender (6 sources, active) · animation (absent) · known defects · verification status.

### 18.3 `REQUIREMENTS.md` — confirmed requirements only

| REQ | Requirement | Source | Status |
|---|---|---|---|
| REQ-01 | Android = primary platform | `RULES.md:98` | active |
| REQ-02 | Windows = secondary, editor testing | `RULES.md:99` | active |
| REQ-03 | Western low-poly / stylized visual direction, no excessive yellow/brown filter | `ART_PIPELINE.md:275` | active |
| REQ-04 | Arena remains procedural — no manual mass placement | `WORK_QUEUE.md` | active |
| REQ-05 | Android performance is a design constraint from the start | `RULES.md:58` | active |
| REQ-06 | WallSegment ≈ 4 m / FloorSegment ≈ 4×4 m; 1 mesh → 1 renderer → 1 material → 1 collider | `RULES.md:56`, `METHODOLOGY §26` | **6/7 satisfied at asset level** |
| REQ-07 | Environment from replaceable prefab pools | GPT FIXED DECISIONS | partially satisfied |
| REQ-08 | No city/building assets | GPT FIXED DECISIONS | active |
| REQ-09 | 3 enemy groups × 4 enemies with Bandit/Rusher/Shooter/Tactical roles | GPT | **satisfied** |
| REQ-10 | Character foundation replaces capsules | `SKILL.md` | **unmet** |
| REQ-11 | Manual aim | GPT STAGE 6.1 | **unmet** |
| REQ-12 | No compatible low-poly styles mixed | GPT FIXED DECISIONS | ⚠️ **currently violated** |
| REQ-13 | No confirmation without user test | `VERIFICATION.md:12` | active, **working** |

### 18.4 `DECISIONS.md` — extract and preserve

| ID | Decision | Date | Status |
|---|---|---|---|
| DEC-001 | Procedural arena, 9-room honeycomb | ≤2026-09-18 | active |
| DEC-002 | Custom A* instead of NavMesh | ≤2026-09-18 | active |
| DEC-003 | Method C (constructive fracture; no Cell/Voronoi/physics) | 2026-09-27 | active |
| DEC-004 | `Col` = FLOAT_COLOR/CORNER, A = metal mask | 2026-09-27 | active |
| DEC-005 | Frozen FBX contract | 2026-09-27 | active |
| DEC-006 | Batch-only Unity integration; `FINAL ≠ integration trigger` | 2026-09-27 | active, **honoured** |
| DEC-007 | Presentation state: `Fragments.hide_viewport=TRUE` | 2026-09-27 | **VIOLATED by Cover** (cause of shimmer, fixed in r4) |
| DEC-008 | Implemented/Compiled/Tested/Confirmed separation | 2026-09-25 | active, **working** |
| DEC-009 | User is sole authority for CONFIRMED/promotion | 2026-09-25 | active, **working** |
| DEC-010 | 4-point multi-height LOS | ≤2026-09-18 | active |
| DEC-011 | Modular wall/floor kit replaces per-log GameObjects | 2026-09-28 | active, **delivery in progress** |
| DEC-012 | **M2 material rule — no image textures** | 2026-09-27 | 🔴 **REVISED at r3 (procedural allowed); approved FINAL assets now non-conformant — needs user ruling** |
| DEC-013 | `wwg-character-art` as sole character quality authority | 2026-09-21 | active, but **descriptive half invalid** |

### 18.5 `MASTER_PLAN.md` — new canonical numbering

**Recommendation: introduce a new unambiguous axis and crosswalk, do not reuse legacy numbers.**

| New ID | Domain | Crosswalk to legacy |
|---|---|---|
| `ART-0xx` | Blender asset production (CP1/CP2/CP3, E0–E5) | `CP1–CP3`, `E0–E5` |
| `UNI-0xx` | Unity integration (U0–U7) | `U0–U7` |
| `GAME-0xx` | Gameplay | legacy `STAGE 1–20` + `AI Stage 1–3` |
| `ART` vs `GAME` split removes the current `Stage 3` collision. |

All legacy numbers preserved in `History/LEGACY_INDEX.md`. **Do not renumber silently** — record the crosswalk explicitly.

### 18.6 `OPEN_ISSUES.md` — seeded list

| Sev | ID | Issue |
|---|---|---|
| **CRITICAL** | ISS-01 | No verified recovery point; 82 uncommitted; `Working/` 205,7 MB unignored |
| **CRITICAL** | ISS-02 | Local 2 commits behind GitHub; `origin/main` stale |
| **CRITICAL** | ISS-03 | 5 `WWG_*.prefab` + 5 `.mat` untracked but **wired into the dev scene** |
| **CRITICAL** | ISS-04 | `MobileTouchControls` 3 refs NULL vs `CONFIRMED` |
| **HIGH** | ISS-05 | `Cover_Western_Wooden` fracture 41,7 % coverage — blocks FINAL, risks `DestructibleObject` |
| **HIGH** | ISS-06 | 12 `{fileID: 0}` in `TestArena` |
| **HIGH** | ISS-07 | 4 × Missing (Mono Script) in enemy prefabs |
| **HIGH** | ISS-08 | `EnemyTacticalPlanner` path-validation regression |
| **HIGH** | ISS-09 | 2 active `DIAG_TEMP_*` + `MobilePerformanceTest` forcing `targetFrameRate=60` in dev scene |
| **HIGH** | ISS-10 | 3 unresolvable material GUIDs (15 TMP → magenta) |
| **HIGH** | ISS-11 | `TestArena` 7/7 prop slots NULL; `Preview` all prefab arrays empty |
| **HIGH** | ISS-12 | DEC-012 material-rule reversal unresolved |
| **MEDIUM** | ISS-13 | `Working/` 205,7 MB not gitignored |
| **MEDIUM** | ISS-14 | `Assets/TestArena.unity` duplicate scene |
| **MEDIUM** | ISS-15 | `Game.unity` empty shell in build list |
| **MEDIUM** | ISS-16 | Modular kit has 0 Unity references |
| **MEDIUM** | ISS-17 | Desktop fire input absent |
| **MEDIUM** | ISS-18 | `DifficultyManager` inert; `Audio` empty |
| **MEDIUM** | ISS-19 | 7 skill conflicts incl. `unity-game-director` |
| **MEDIUM** | ISS-20 | 3 stage-numbering schemes without crosswalk |
| **MEDIUM** | ISS-21 | Audit 1 not written to disk |
| **LOW** | ISS-22 | Dead code (`EnemyHealthBar`, `CameraFollow`, `OnCollisionEnter` ×2, `CombatPhysics`) |
| **LOW** | ISS-23 | ~200 lines spawn diagnostics + `OnGUI`/`LineRenderer` debug ON in enemy prefabs |
| **LOW** | ISS-24 | `Wood_Western.shadergraph` unused; duplicate fence materials; identical 8×Albedo/Normal |
| **LOW** | ISS-25 | mojibake in `.gitignore` comments, `opencode.json`, `AndroidT.asset`; LFS rule non-matching |
| **DOC** | ISS-26 | 0 of 12 `AI_CONTEXT` files mention 2026-09-29 work |
| **DOC** | ISS-27 | `README.md:5` false claim; manifest incomplete |
| **RUNTIME** | ISS-28 | **No runtime evidence exists for any claim** |
| **RUNTIME** | ISS-29 | Unity MCP port now OPEN — runtime pass possible for the first time |
| **GIT** | ISS-30 | `backup-pre-lfs` is an orphan duplicate of commit 1, not a recovery point |

### 18.7 `RECONCILIATION.md` — the audit→truth ledger

Per the ТЗ format: *claim → where claimed → what was found → what is real → decision taken → canonical home*. Seeded from the 50 rows of §8.2.

### 18.8 `DOCUMENTATION_CONFLICTS.md` — DC register

All 31 carried conflicts (D/S/C/G/R series) + 4 new QA conflicts (Q1–Q4) = **35 entries**, each with: ID, Source A/B, exact subject, claims A/B, reality evidence, current resolution, canonical statement, affected documents.

---

## 19. Risks

| # | Risk | Sev | Mitigation |
|---|---|---|---|
| RK-01 | `git clean -fd` destroys 646 untracked files (205,7 MB) incl. 3 FINAL sources | **CRITICAL** | never run; add `Working/` to `.gitignore` first (needs approval) |
| RK-02 | `git stash -u` removes the same | **CRITICAL** | avoid |
| RK-03 | `git pull` merges 2 MD that exist only on GitHub | HIGH | resolve Q15 first |
| RK-04 | A fresh clone loses the wired `WWG_*` prefabs/materials → dev scene breaks | HIGH | commit them |
| RK-05 | Creating canonical docs *before* the new numbering is agreed re-multiplies the `Stage 3`/`Stage 19` ambiguity | HIGH | decide `MASTER_PLAN` numbering first |
| RK-06 | Treating 7 QA reports as canonical without resolving Q1–Q4 propagates **invalidated fracture claims** | HIGH | resolve conflicts first |
| RK-07 | `wwg-character-art` continues to assert a non-existent rig → agents build against phantom sockets | HIGH | split the SKILL |
| RK-08 | Art work continues while `Working/` grows untracked — each hour increases loss exposure | **CRITICAL** | recovery point urgently |
| RK-09 | `Assets/TestArena.unity` duplicate keeps causing MCP writes to the wrong scene | MEDIUM | user decision Q1 |
| RK-10 | `unity-game-director` auto-activation redirects work away from `RULES.md` | MEDIUM | record conflict; consider disabling |
| RK-11 | Unity MCP now reachable → an agent may run Play-mode/diagnostics that mutate scenes | MEDIUM | keep runtime audit read-only or user-supervised |
| RK-12 | Blender session is live on `Cover_Western_Wooden_Working.blend` — an in-flight asset at risk | MEDIUM | save/commit before further work |

---

## 20. Recovery / Safety Assessment

### 20.1 Is a recovery point available?

| Criterion (ТЗ §29) | Status |
|---|---|
| Commit that captures current work | ❌ **NO** — HEAD is 4 days old |
| Branch capturing current work | ❌ **NO** — `backup-pre-lfs` duplicates commit 1 |
| Tag | ❌ **NO** — 0 local, 0 remote |
| Content verified | ❌ **NO** |
| Accessibility verified | ❌ **NO** |
| Untracked assets protected from loss | ❌ **NO** — 646 files / 205,7 MB, none ignored, none committed |

**VERDICT: NO VERIFIED RECOVERY POINT EXISTS.**

### 20.2 Assets at risk of loss

| Asset | Size | Risk | Recoverable? |
|---|---|---|---|
| `Working/blender_src/` (6 `.blend` + 6 `.blend1`) | ~1,5 MB | HIGH | ❌ only copy |
| `Working/reports/` (7 QA MD) | ~77 KB | HIGH | ❌ only record of verdicts |
| `Working/blender_reviews/` (19 dirs, ~580 PNG) | ~204 MB | HIGH | ❌ only QA evidence |
| `Assets/Art/Environment/Modular/` (6 FBX) | ~175 KB | HIGH | ❌ **not in any prefab/scene** |
| `Assets/Art/Props/` (20 FBX) | ~800 KB | HIGH | ❌ **not in any prefab/scene** |
| `Assets/Prefabs/Environment/WWG_*.prefab` (5) | ~27 KB | **CRITICAL** | ❌ **wired into dev scene** |
| `Assets/Materials/WWG_*.mat` (5) | ~19 KB | **CRITICAL** | ❌ **wired into dev scene** |
| 82 uncommitted modifications | — | **CRITICAL** | ❌ |

### 20.3 Prohibited operations confirmed NOT executed

`git clean` · `git reset --hard` · `git stash` · `git checkout` (destructive) · `git add` (mass) · `git pull` · `git merge` · `git rebase` · `git restore` · `git commit` · `git push` · `git fetch` · `git tag` · branch deletion · untracked-file deletion

### 20.4 Recommended recovery sequence — **NOT EXECUTED**

> Requires explicit user approval. Listed in order of increasing risk.

| # | Step | Risk | Note |
|---|---|---|---|
| 1 | Create tag `pre-recovery-2026-09-29` at local `040651b` | none | marks the point *before* recovery |
| 2 | Create branch `recovery/checkpoint-2026-09-29` from current working tree | none | captures all 82 entries |
| 3 | Add `Working/` and diagnostic screenshot paths to `.gitignore` (or a project-local ignore) | LOW | requires approval to modify `.gitignore` |
| 4 | Fetch + reconcile the 2 GitHub-only MD | MEDIUM | resolve Q15 first |
| 5 | Create recovery commit + push | MEDIUM | requires explicit approval |
| 6 | Verify: `git status` clean, untracked assets tracked, tag + branch present | none | mandatory before any documentation work |

**Steps 1–6 were NOT performed.**

---

## 21. STOP POINT

Per ТЗ §27, the audit stops here.

**Not done, awaiting APPROVE:**
- ❌ canonical `Documentation/` structure not created
- ❌ no verified facts migrated into any canonical document
- ❌ no requirements/decisions/roadmap migrated
- ❌ no old MD marked `SUPERSEDED`/`HISTORICAL`
- ❌ no OpenCode entry documentation updated
- ❌ no links between documents created
- ❌ no contradiction-resolution pass performed
- ❌ no `DOCUMENTATION_CONFLICTS.md` created
- ❌ no recovery point created
- ❌ no git write operation of any kind
- ❌ no Unity/C#/Prefab/asset/Skill modification

### 21.1 Decisions required from the user before migration

| # | Decision | Blocks |
|---|---|---|
| **U-1** | Approve recovery sequence §20.4 (steps 1–6)? | **everything** |
| **U-2** | Which scene is canonical — `Assets/Scenes/TestArena.unity` or `Assets/TestArena.unity`? | `PROJECT_TRUTH.md` |
| **U-3** | Approve new numbering scheme (§18.5: `ART-`/`UNI-`/`GAME-`) or choose another? | `MASTER_PLAN.md` |
| **U-4** | Ruling on **DEC-012**: is the M2 «no image textures» rule still binding, given r3 uses a 14-node procedural graph and the promoted `*_FINAL.fbx` are non-conformant? | Art pipeline |
| **U-5** | Approve promotion of `Crate_01` CP2 (executed without recorded approval) to canonical FINAL? | `CONFIRMED_STATE` |
| **U-6** | Approve promotion of `WallSegment_01` / `FloorSegment_01` `*_FINAL.fbx` (r1, 09:46) or promote r3? | `Art/ART_PIPELINE.md` |
| **U-7** | `Cover_Western_Wooden`: accept r4 (with 41,7 % fracture defect) or regenerate fracture first? | Art pipeline |
| **U-8** | Approve splitting `wwg-character-art` into operational gates + future contract? | `Character/` |
| **U-9** | Approve writing Audit 1 to disk under `History/Audit1/`? | M-01 |
| **U-10** | Authoritative naming/location for the new `Documentation/` tree? | structure |
| **U-11** | Should the 2 GitHub-only MD be pulled locally before migration? | Q15 |
| **U-12** | Authorise a **read-only runtime verification pass** now that Unity MCP port is open? | ISS-28/29 |

---

**AUDIT 2 COMPLETE — READ-ONLY — NO CHANGES MADE**

*Prepared 2026-09-29. Integrity: `HEAD 040651b`, branch `main`, 82 status entries (15 M / 2 D / 65 untracked), 0 staged. Only file created: this report.*
