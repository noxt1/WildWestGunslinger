# PHASE 5A — CORE INTEGRITY AUDIT + FIX

**Document ID:** `DOC-P5A-2026-09-29`
**Status:** COMPLETE (static) · runtime confirmation pending
**Date:** 2026-09-29
**Base commit:** `78c07f2` · **Predecessor:** `4b04f5f` (Phase 3) · **Phase 4:** `78c07f2`

---

## 1. Initial state

| Field | Value |
|---|---|
| Branch | `main` |
| HEAD | `78c07f29efefe1d6398f30d2f6c1b774b20ef156` |
| `origin/main` | `f932fcc` · **behind 2 / ahead 3** (deferred per `DEC-07`; not touched) |
| Recovery branch / tag | `recovery/checkpoint-2026-09-29-audit2` / `recovery-2026-09-29-audit2` → `040651b` — intact |
| Working tree before | 74 entries, **0 staged** |
| Canonical docs | current, read from Git before any change |
| Unity | 6000.3.23f1 installed and running · **no Unity control channel available** |

---

## 2. Issues investigated

| Issue | Canonical source | Investigated |
|---|---|---|
| A-01 broken scene references | `UNI-D04` | ✅ |
| A-02 deleted enemy script references | `UNI-D05`, `ISSUE-13`, `RT-06` | ✅ |
| A-03 `MobileTouchControls` | `UNI-D07`, `RT-02` | ✅ |

---

## 3. Root causes

### A-01 — the "12 broken references" were a misclassification

A full static enumeration of `Assets/Scenes/TestArena.unity` (57 distinct GUID references, all resolved) plus a MonoBehaviour-field scan produced:

| Group | Count | Actual behaviour | Broken? |
|---|---:|---|---|
| `ArenaGenerator` × 7 prefab slots | 7 | `HaveClusterPrefabs()` returns false → **graceful fallback** to `BuildLegacyCovers()` | **No** |
| `MobileTouchControls` × 3 | 3 | **self-healed** at runtime via `FindFirstObjectByType` / `Camera.main` | **No** |
| `HUDController.xpBar` / `levelText` | 2 | **dead fields** — `HUDController.cs` (53 lines) never reads them; no XP UI objects exist | **No** |

**Genuinely broken references: 2** — unresolvable *material* GUIDs on `DIAG_TEMP_White` (`5e0759d7…`) and `DIAG_TEMP_Gray` (`e0b90311…`). Hand-authored fileIDs (`910000001`), Unity built-in Quad mesh, `m_Enabled: 1`, casting shadows — debris from a washed-out-scene diagnosis session. **Not removed** (destructive scene edit → needs a decision).

### A-02 — deleted scanner script

- **Cause:** `EnemyTacticalEnvironmentScanner_TEST.cs` (435 lines) was deleted as temporary diagnostic cleanup; its `.meta` went with it, but 4 enemy prefabs kept a serialized reference to GUID `809b48f6d9f34f34d90ca85975a9c332`.
- **What it was:** a diagnostic obstacle scanner with `showDebug = true` and `OnDrawGizmos()`, exposing `ScanNow()`, `TryGetBestRoute()`, `IsDirectionClear()`.
- **Is it replaced? YES** — absorbed into `EnemyController.cs`: `obstacleProbeRadius = 0.35` (**identical** to the deleted `bodyRadius`), `obstacleRouteProbeDistance = 2.2` (vs `2.5`), `obstacleSideProbeDistance = 1.6` (vs `sideProbeAngle = 70`), plus a newer developed system (`obstacleStuckTime`, `obstacleRouteCommitTime`, `obstacleRouteActive`).
- **Callers remaining:** `TryGetBestRoute`, `IsDirectionClear`, `RouteCandidate`, `ObstacleInfo`, `visibleObstacles` → **0 references** project-wide.
- **State lost:** **none** — all 16 serialized fields were still readable in the prefabs.
- **Decision: REMOVE obsolete reference.**

### A-03 — self-healing, not a code defect

3 null fields, all resolved at runtime. Real residual risks: `FindFirstObjectByType<GunController>()` can bind the **wrong** instance (compounded by the `RT-03` duplicate), and there is **no retry** if the Player does not exist at `Start()`.

---

## 4. Files changed

| File | Change |
|---|---|
| `Assets/Prefabs/Enemies/Bandit.prefab` | −29 lines |
| `Assets/Prefabs/Enemies/Rusher.prefab` | −29 lines |
| `Assets/Prefabs/Enemies/Shooter.prefab` | −29 lines |
| `Assets/Prefabs/Enemies/Tactical.prefab` | −29 lines |
| `Documentation/UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md` | `UNI-D04` reclassified · `UNI-D05` fixed · `UNI-D07` reclassified |
| `Documentation/History/OPEN_ISSUES.md` | `ISSUE-13` closed |
| `Documentation/PROJECT_TRUTH.md` | defect counts corrected |
| `Documentation/PHASE5A_CORE_INTEGRITY_REPORT.md` | this report |

**No C#, no scenes, no materials, no FBX, no ProjectSettings, no Packages, no art touched.**

---

## 5. Static verification

| Check | Result |
|---|---|
| Dangling GUID occurrences across 4 prefabs | **4 → 0** |
| `EnemyTacticalEnvironmentScanner` name occurrences | **→ 0** |
| Orphaned `m_Component` references | **0** |
| Component count per prefab | 10 → **9** |
| `EnemyHealth` / `EnemyTacticalPlanner` / `EnemyController` intact | **yes** |
| Diff shape | **116 deletions, 0 insertions**, 4 files |
| Orphan-block count vs `HEAD` | **2 → 2, identical** (the GameObject headers) → no regression |
| Prefabs restorable | yes — clean in Git before the edit |

---

## 6. Runtime tests

| Test | Result |
|---|---|
| Launch Unity / enter Play mode | **NOT PERFORMED** — no Unity control channel in this environment |
| Console check | **NOT PERFORMED** |
| Reproduce the `missing script` warnings | **NOT PERFORMED** |
| Confirm 0 warnings after fix | **NOT PERFORMED** |

> Per §12 a runtime fix is verified only with Unity running, Console checked, scenario reproduced and behaviour observed. **None of that was possible here.** `RT-06` is therefore `FIXED (static) / NOT VERIFIED (runtime)`.

---

## 7. Console result

**NOT VERIFIED** — the Console was not observed. The pre-existing baseline (7 warnings on one `Bandit`, `RT-06`) is the reference for comparison.

---

## 8. Regressions

| Risk | Assessment |
|---|---|
| Enemy behaviour change | **None expected** — the removed component had **0 callers**; its capability lives in `EnemyController` |
| Serialized state loss | **None** — no live data was in the removed block; it was an inert orphan |
| Prefab structural integrity | Verified: 0 orphan refs, component list and blocks consistent |
| Other prefabs | Not touched |

---

## 9. Issues closed

| Issue | Before | Root cause | Fix | Runtime | Status |
|---|---|---|---|---|---|
| `A-02` / `UNI-D05` / `RT-06` / `ISSUE-13` | 4 prefabs referenced a deleted script GUID; 7 warnings per `Bandit` | Diagnostic scanner deleted, prefab slots orphaned, capability absorbed into `EnemyController` | Removed orphaned component block + `m_Component` entry from 4 prefabs | **NOT VERIFIED** | **FIXED (static)** |

---

## 10. Issues still open

| Issue | Status | Why |
|---|---|---|
| `A-01` / `UNI-D04` | **RECLASSIFIED** | 12 → **2** real broken refs (`DIAG_TEMP_*` debris). Removal needs a user decision |
| `A-03` / `UNI-D07` / `RT-02` | **RECLASSIFIED, Android OPEN** | Self-healing on PC; no Android device available |
| `RT-06` | **FIXED (static), runtime pending** | Needs a Play-mode run |
| `RT-01`, `RT-04`, `RT-05` | **OPEN** | Out of Phase 5A scope |
| `UNI-D01`, `D02`, `D03`, `D11`, `D12`, `D13` | **OPEN** | Out of scope |
| 2 `DIAG_TEMP_*` objects | **REVIEW** | Destructive removal pending decision |

---

## 11. Documentation updated

| Document | Update |
|---|---|
| `UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md` | `UNI-D04` reclassified (12 → 2); `UNI-D05` fixed with full evidence; `UNI-D07` reclassified |
| `History/OPEN_ISSUES.md` | `ISSUE-13` closed; `ISSUE-20` added for the `DIAG_TEMP_*` debris |
| `PROJECT_TRUTH.md` | defect table corrected |
| `PHASE5A_CORE_INTEGRITY_REPORT.md` | new |

---

## 12. Git commit

| Field | Value |
|---|---|
| Message | `fix(prefabs): remove orphaned EnemyTacticalEnvironmentScanner references` |
| Scope | 4 prefabs + documentation only |
| Prohibited operations | none used — no `add .`, no `amend`, `merge`, `rebase`, `reset`, `clean`, `stash`, or `push` |

---

## 13. Remaining risks

| # | Risk |
|---|---|
| 1 | **`RT-06` runtime unconfirmed.** If Unity shows other missing-script sources, the fix is incomplete |
| 2 | **No Unity control channel** blocks all runtime verification in this environment — every future runtime-dependent fix will hit the same wall |
| 3 | `DIAG_TEMP_*` objects still active and casting shadows with missing materials |
| 4 | `A-03` instance ambiguity: `FindFirstObjectByType<GunController>()` may bind the wrong weapon on device |
| 5 | `origin/main` still behind 2 / ahead 3 — deferred, untouched |
| 6 | 70 other product changes still uncommitted |

---

## 14. Required next verification (human or Unity-capable agent)

1. Open `Assets/Scenes/TestArena.unity`; confirm **no** `Missing (Mono Script)` on any enemy prefab.
2. Enter Play mode; trigger a spawn; confirm **0** `The referenced script (Unknown)...` warnings.
3. Confirm enemies still path, search and shoot as before (regression check for A-02).
4. Decide the fate of `DIAG_TEMP_White` / `DIAG_TEMP_Gray`.
5. On an Android device, validate `MobileTouchControls` (A-03) — PC PASS ≠ Android PASS.

---

**End of `PHASE5A_CORE_INTEGRITY_REPORT.md`**
