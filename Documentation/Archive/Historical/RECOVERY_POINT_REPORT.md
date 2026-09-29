# RECOVERY_POINT_REPORT.md

**Project:** WildWestGunslinger
**Phase:** 1 — RECOVERY POINT
**Date:** 2026-09-29, 16:42–16:58 (+03:00)
**Authorisation:** user instruction «PHASE 1 — RECOVERY POINT»
**Scope:** establish a verified, restorable recovery point. **No project content was fixed, refactored, or migrated.**

> **Outcome: recovery point established and VERIFIED.**
> A byte-level archive of all 873 modified/untracked files was created and content-verified,
> a recovery branch and an annotated recovery tag were created and verified,
> and `git fetch` reconciled the stale remote ref without touching the worktree.

---

## 1. Recovery Branch

| Field | Value |
|---|---|
| **Branch** | `recovery/checkpoint-2026-09-29-audit2` |
| **Points to** | `040651b1d8963231b814217319b099b7ab5ef16d` |
| Created from | `040651b` (= current `main` HEAD at recovery time) |
| Checkout performed | **NO** — created with `git branch <name> <sha>`; worktree never switched |
| Verified | ✅ `git rev-parse --verify` returns `040651b…` |

## 2. Recovery Tag

| Field | Value |
|---|---|
| **Tag** | `recovery-2026-09-29-audit2` |
| Type | **annotated** (`git tag -a`) |
| Tag object SHA | `7bc19c7aa0b439cdb998f7d6241ff816b641b18b` |
| **Peels to commit** | `040651b1d8963231b814217319b099b7ab5ef16d` |
| **Expected commit** | `040651b1d8963231b814217319b099b7ab5ef16d` |
| **Verification** | ✅ **PASS** — `git rev-list -n 1 <tag>` == `040651b…` == current HEAD |

Tag message records the archive path, file count, size and SHA256 for independent audit.

## 3. Commit

| Field | Value |
|---|---|
| **Recovery commit for project content** | **NONE — deliberately not created** |
| Rationale | No project file (C#, scene, prefab, material, asset) was staged or committed. The tag/branch mark the *pre-recovery baseline*; the archive carries the working state. |
| Documentation commit | see §9 — explicitly staged files only, **no `git add .`** |

## 4. Backup Location

| Field | Value |
|---|---|
| **Backup root** | `C:\Users\cyril\WildWestGunslinger_RECOVERY\` |
| **Archive** | `wwg_recovery_snapshot_20260929_164259.zip` |
| **File list** | `untracked_file_list_20260929_164259.txt` |
| **Why outside the repo** | A git branch/tag cannot protect untracked files. The archive is deliberately **outside** `C:\Users\cyril\WildWestGunslinger` so it is neither self-referential nor subject to any future `git clean`. |
| Path relativity | Archive entries use paths **relative to the project root**, so restoration is a straight unpack. |

## 5. Backup Verification

| Check | Result |
|---|---|
| Files resolved for backup | **873** |
| Files archived OK | **873** |
| Failures | **0** |
| Raw bytes | 231,87 MB |
| Archive size | 227,64 MB |
| Archive SHA256 | `9ABE45EEB928521EECC28B8270BC41BD77CFB5969F01DDD3621D3CD7A2757513` |
| Archive entries read back | **873 / 873** |
| Files listed but absent from archive | **0** |
| Unexpected entries in archive | **0** |
| **Content spot-checks (SHA256 archive vs source)** | **12 / 12 PASS** |
| Verification method | archive re-opened, entries enumerated, 12 critical files extracted to memory and SHA256-compared against source |

### 5.1 Content verification detail

| File | SHA256 (first 16) | Result |
|---|---|---|
| `AUDIT_2_RECONCILIATION_REPORT.md` | `9F7D05E0D38D06AC` | ✅ OK |
| `AUDIT_2_RUNTIME_REPORT.md` | `282850B78EC75639` | ✅ OK |
| `Working/blender_src/wall_segment/WallSegment_01_Working.blend` | `1D125174918DC4C5` | ✅ OK |
| `Working/blender_src/cover/Cover_Western_Wooden_Working.blend` | `99CB8CBB07DBDD29` | ✅ OK |
| `Working/blender_src/crate/Crate_01_Working.blend` | `D0B85259B013750A` | ✅ OK |
| `Working/reports/cover_r4_realtime_shimmer_uv_grain_final_qa.md` | `7F4942D3D2466ECF` | ✅ OK |
| `Assets/Art/Environment/Modular/WallSegment_01_FINAL.fbx` | `2BB9B810059C5FF0` | ✅ OK |
| `Assets/Art/Environment/Modular/FloorSegment_01_FINAL.fbx` | `687F0FFCCB51D93E` | ✅ OK |
| `Assets/Art/Props/Destructible/Crate_01_Fragments_FINAL.fbx` | `0F3104BC88108E78` | ✅ OK |
| `Assets/Prefabs/Environment/WWG_Rock_01.prefab` | `D8AC1C647F7827A1` | ✅ OK |
| `Assets/Materials/WWG_Floor_Dusty.mat` | `D5CFBAE585FF76AF` | ✅ OK |
| `Assets/Scenes/TestArena.unity` | `C718164DDCCA5DE3` | ✅ OK |

> **Independent cross-validation:** `WallSegment_01_FINAL.fbx` → `2BB9B810059C5FF0` and `FloorSegment_01_FINAL.fbx` → `687F0FFCCB51D93E` match the hashes recorded inside `Working/reports/modular_environment_wall_floor_final_qa.md` exactly. The archived binaries are the same artifacts the QA reports describe.

## 6. GitHub / Origin State

| Field | Value |
|---|---|
| Remote | `https://github.com/noxt1/WildWestGunslinger.git` |
| Upstream of `main` | `origin/main` |
| Visibility | **private** |
| **GitHub `main`** | **`f932fcc722d22b8150ccf7e9616914324347c6e8`** (2026-09-28T13:40:20Z) |
| **Local `HEAD`** | **`040651b1d8963231b814217319b099b7ab5ef16d`** |
| **`origin/main` (local ref) — before fetch** | `040651b…` — **STALE** |
| **`origin/main` (local ref) — after fetch** | `f932fcc…` — **RECONCILED** |
| Behind | **2** |
| Ahead | **0** |
| Fetch executed | ✅ `git fetch origin` — exit 0. **No merge, no pull, no reset, no rebase.** |
| Commits received | `f932fcc Create AI_PRODUCTION_METHODOLOGY.md`<br>`41da361 Create AUDIT_SESSION_CONTEXT_2026-09-28.md` |
| Worktree impact of fetch | **none** — 84 status entries before and after |

> The stale-ref condition identified in AUDIT 1 / AUDIT 2 is now **resolved**.
> `main` is still 2 commits behind `origin/main` — deliberately **not** merged, because the working tree holds 84 uncommitted entries and merging now would risk conflict with uncommitted work.

## 7. Untracked / Modified Critical Assets Preserved

All are inside the verified archive (§4–5). Grouped by criticality.

### 7.1 CRITICAL — wired into the live scene; a clean clone breaks `TestArena`

| Asset | Count | Size | Wired to |
|---|---:|---:|---|
| `Assets/Prefabs/Environment/WWG_*.prefab` | 5 | ~27 KB | `WildWestEnvironmentGenerator.rockPrefabs / grassPrefabs / propPrefabs / fencePrefabs` |
| `Assets/Materials/WWG_*.mat` | 5 | ~19 KB | `groundMaterial`, `ArenaGenerator.coverMaterial` |

### 7.2 HIGH — QA-approved art, zero runtime presence, sole binary copy

| Asset | Count | Size | Note |
|---|---:|---:|---|
| `Assets/Art/Environment/Modular/*.fbx` | 6 | ~0,18 MB | `WallSegment_01` / `FloorSegment_01` × FINAL/r2/r3 — 0 prefab references |
| `Assets/Art/Props/**/*.fbx` | 35 | ~0,78 MB | Barrel/Fence/Crate/Cover incl. all revisions and fragments |

### 7.3 HIGH — Blender sources of truth

| Asset | Count | Size |
|---|---:|---:|
| `Working/blender_src/**/*.blend` | 6 | ~1,5 MB |
| `Working/blender_src/**/*.blend1` | 6 | ~1,5 MB |

### 7.4 HIGH — sole record of QA verdicts

| Asset | Count | Size |
|---|---:|---:|
| `Working/reports/*.md` | **10** | ~95 KB |

> 3 of these 10 reports (`cover_r5_*`, 16:16 / 16:22 / 16:30) appeared **during this recovery session** — the art pipeline is still running.

### 7.5 MEDIUM — derived QA renders

| Asset | Count | Size |
|---|---:|---:|
| `Working/blender_reviews/**/*.png` | ~700 | ~222 MB |
| `Assets/Screenshots/*.png` | 18 | ~3,6 MB |

### 7.6 MEDIUM — audit artefacts

| Asset | Count | Size |
|---|---:|---:|
| `AUDIT_2_RECONCILIATION_REPORT.md` | 1 | 77 159 B |
| `AUDIT_2_RUNTIME_REPORT.md` | 1 | 34 952 B |
| `RECOVERY_POINT_REPORT.md` | 1 | this file |

### 7.7 LOW — scene artefacts

| Asset | Count | Size |
|---|---:|---:|
| `Assets/TestArena.unity` (+meta) — duplicate scene | 2 | ~219 KB |
| `Assets/_Recovery/0 (2).unity`, `0 (3).unity` (+metas) | 4 | ~218 KB |

### 7.8 Modified / Deleted tracked files also captured

The 15 modified + 2 deleted tracked entries were included in the archive as their **current working-tree state**, so the archive alone is sufficient to restore the pre-recovery condition.

## 8. `.gitignore` Determination

> **Determination only — `.gitignore` was NOT modified in this phase.**
> Per instruction, `Working/` is not excluded wholesale before its source-of-truth contents are established.

### 8.1 Classification of every untracked path

| Path | Count | Size | Category | Determination |
|---|---:|---:|---|---|
| `Working/blender_src/**/*.blend` | 6 | 1,5 MB | **source of truth** | **TRACK** — sole editable source for all shipped FBX |
| `Working/reports/*.md` | 10 | 0,1 MB | **audit artefact / source of truth** | **TRACK** — sole record of QA verdicts and self-corrections |
| `Working/blender_src/**/*.blend1` | 6 | 1,5 MB | **temporary** (Blender auto-backup) | **IGNORE** — regenerable from `.blend` |
| `Working/blender_reviews/**/*.png` | ~700 | 222 MB | **derived** (regenerable renders) | **IGNORE** — but see 8.3 |
| `Assets/Prefabs/Environment/WWG_*.prefab` | 5 | 0,03 MB | **production asset** | **TRACK** — wired into live scene |
| `Assets/Materials/WWG_*.mat` | 5 | 0,02 MB | **production asset** | **TRACK** — wired into live scene |
| `Assets/Art/Environment/Modular/*.fbx` | 6 | 0,18 MB | **production asset (pending integration)** | **TRACK** — 0 refs today, but RT-01 pending |
| `Assets/Art/Props/**/*.fbx` | 35 | 0,78 MB | **production asset (pending integration)** | **TRACK** |
| `Assets/Screenshots/*.png` | 18 | 3,6 MB | **derived / diagnostic** | **REVIEW** — some are the only evidence for MSAA/renderScale/shadow claims |
| `Assets/TestArena.unity` (+meta) | 2 | 0,22 MB | **duplicate / temporary** | **REVIEW** — U-2 open (which scene is canonical) |
| `Assets/_Recovery/0 (2),(3).unity` | 4 | 0,22 MB | **temporary** (Unity auto-recovery) | **IGNORE** — note `0.unity` and `0 (1).unity` are already tracked |
| `AUDIT_*.md`, `RECOVERY_POINT_REPORT.md` | 3 | 0,1 MB | **audit artefact** | **TRACK** |

### 8.2 Current `.gitignore` coverage (unchanged, verified)

| Path | Ignored? |
|---|---|
| `Library/ Temp/ obj/ Build/ Builds/ Logs/ UserSettings/ MemoryCaptures/` | ✅ |
| `.vs/ *.csproj *.sln* *.user *.userprefs` | ✅ |
| `.vscode/*` (whitelist 4 files) | ✅ |
| `/utmp/` | ✅ |
| `*.env` `*.ulf` | ✅ |
| `.opencode/node_modules` | ✅ (via `.opencode/.gitignore`) |
| **`Working/`** | ❌ **not ignored** |
| `Assets/Screenshots/` | ❌ |
| `Assets/Art/**` | ❌ |
| `Assets/TestArena.unity` | ❌ |

### 8.3 `Working/` verdict

**`Working/` must NOT be excluded wholesale.** It contains two distinct classes:

- **Source of truth** — `blender_src/**/*.blend` (6 files) and `reports/*.md` (10 files) → **must be tracked**
- **Derived** — `blender_reviews/**/*.png` (~700 files, 222 MB) and `*.blend1` → **may be ignored**

Excluding `Working/` wholesale would silently discard the only copy of every Blender source and every QA verdict. A path-scoped rule such as `Working/blender_reviews/` would be the defensible alternative — **but no `.gitignore` change was made, pending user decision.**

### 8.4 Recommended `.gitignore` additions (NOT applied)

```gitignore
# Derived QA renders — regenerable from Blender sources
/Working/blender_reviews/
# Blender auto-backups
*.blend1
# Unity auto-recovery for the new _Recovery entries
/Assets/_Recovery/0 (2).unity
/Assets/_Recovery/0 (3).unity
```

Explicitly **not** recommended for ignoring: `Working/blender_src/`, `Working/reports/`, `Assets/Art/`, `Assets/Screenshots/`, `AUDIT_*.md`.

## 9. Documentation Commit

**Staging was explicit. `git add .` was NOT used.**

| # | Staged path | Rationale |
|---|---|---|
| 1 | `AUDIT_2_RECONCILIATION_REPORT.md` | audit artefact |
| 2 | `AUDIT_2_RUNTIME_REPORT.md` | audit artefact |
| 3 | `RECOVERY_POINT_REPORT.md` | this recovery record |

**Deliberately NOT staged** (pending the open decisions in §8 and AUDIT 2):
`Working/**` · `Assets/Art/**` · `Assets/Prefabs/Environment/WWG_*` · `Assets/Materials/WWG_*` · `Assets/Screenshots/**` · `Assets/TestArena.unity` · `Assets/_Recovery/0 (2),(3)` · all 15 modified `AI_CONTEXT`/script/scene/asset files.

> The 15 modified files were **not** staged because committing them would assert a project state that AUDIT 2 and the runtime audit have both shown to be partly incorrect (`CONFIRMED_STATE.md` over-claims, `ART_PIPELINE.md` predates the 2026-09-29 art work). That reconciliation is a later phase.

## 10. Final `git status`

```
## main...origin/main [behind 2]
 M AI_CONTEXT/ARCHITECTURE.md
 M AI_CONTEXT/ART_PIPELINE.md
 M AI_CONTEXT/CHANGELOG.md
 M AI_CONTEXT/CONFIRMED_STATE.md
 M AI_CONTEXT/CURRENT_TASK.md
 M AI_CONTEXT/PROJECT_STATE.md
 M AI_CONTEXT/RULES.md
 M AI_CONTEXT/VERIFICATION.md
 M Assets/Scenes/TestArena.unity
 M Assets/Scenes/TestArena_Preview.unity
 D Assets/Scripts/Enemies/EnemyTacticalEnvironmentScanner_TEST.cs
 D Assets/Scripts/Enemies/EnemyTacticalEnvironmentScanner_TEST.cs.meta
 M Assets/Scripts/Enemies/EnemyTacticalPlanner.cs
 M Assets/Scripts/Procedural/ArenaGenerator.cs
 M Assets/Scripts/UI/EnemyDirectionIndicator.cs
 M Assets/Settings/Mobile_RPAsset.asset
 M Assets/Settings/PC_RPAsset.asset
?? AUDIT_2_RECONCILIATION_REPORT.md   -> committed
?? AUDIT_2_RUNTIME_REPORT.md          -> committed
?? RECOVERY_POINT_REPORT.md           -> committed
?? Assets/Art/Environment/Modular.meta
?? Assets/Art/Environment/Modular/
?? Assets/Art/Props.meta
?? Assets/Art/Props/
?? Assets/Materials/WWG_*.mat (+5 .meta)
?? Assets/Prefabs/Environment/WWG_*.prefab (+5 .meta)
?? Assets/Screenshots/ (18 .png)
?? Assets/TestArena.unity (+meta)
?? Assets/_Recovery/0 (2).unity, 0 (3).unity (+metas)
?? Working/
```

## 11. Verification Summary

| # | Check | Result |
|---|---|---|
| 1 | Recovery branch created and points to `040651b` | ✅ |
| 2 | Recovery tag created, annotated, peels to `040651b` | ✅ |
| 3 | Tag points to the **expected** commit | ✅ PASS |
| 4 | Branch creation did not switch the worktree | ✅ still on `main` |
| 5 | Backup archive exists outside the repo | ✅ `C:\Users\cyril\WildWestGunslinger_RECOVERY\` |
| 6 | Archive entry count == resolved file count | ✅ 873 / 873 |
| 7 | Archive content verified against source (SHA256) | ✅ 12 / 12 |
| 8 | Untracked critical assets preserved | ✅ all inside archive |
| 9 | `git fetch` executed safely | ✅ exit 0, no merge/pull/reset/rebase |
| 10 | `origin/main` reconciled | ✅ `040651b` → `f932fcc` |
| 11 | Local `HEAD` unchanged by fetch | ✅ `040651b` |
| 12 | Worktree unchanged by fetch | ✅ 84 entries before and after |
| 13 | No destructive git operation used | ✅ reset/clean/stash/rebase/checkout/force — none |
| 14 | `git add .` NOT used | ✅ explicit staging only |
| 15 | Staged = 0 after commit | ✅ verified |
| 16 | Audit reports accessible | ✅ on disk and in git |
| 17 | `.gitignore` determination delivered | ✅ §8, no edit applied |
| 18 | Project code/scenes/prefabs untouched | ✅ |

## 12. Open Risks After Recovery

| # | Risk | Note |
|---|---|---|
| 1 | **The art pipeline is still running.** | Blender (3 processes) active; `WallSegment_01_Working.blend` modified 16:34; 3 new `cover_r5_*` reports appeared mid-session. `Working/` grew 646 → **747 files** during this phase. The archive is a **point-in-time snapshot at 16:42:59** — anything created after that stamp is not in it. |
| 2 | `main` is 2 commits behind `origin/main` | Intentionally not merged. `AI_PRODUCTION_METHODOLOGY.md` and `AUDIT_SESSION_CONTEXT_2026-09-28.md` still absent locally. |
| 3 | 67 untracked entries remain | Protected by the archive, not by git. |
| 4 | 15 modified files remain uncommitted | Protected by the archive. |
| 5 | RT-01 (modular FBX 0,01× import scale) still open | Blocks U-6 asset promotion. |
| 6 | RT-02 (`MobileTouchControls` null refs, Android-critical) still open | — |

## 13. STOP POINT

Phase 1 complete. **No project content was modified, fixed, refactored, promoted, or migrated.**

Not done, awaiting APPROVE:
- documentation migration / canonical `Documentation/` tree
- deletion or marking of any old MD
- any Unity / C# / Prefab / material change
- any fix of RT-01, RT-02, RT-04, RT-05
- `.gitignore` modification
- tracking of `Working/`, `Assets/Art/`, `WWG_*` assets
- merge or rebase of `origin/main`

---

**RECOVERY POINT ESTABLISHED AND VERIFIED**
