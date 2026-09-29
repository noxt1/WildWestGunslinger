# PHASE 5C — HUD / XP INTEGRITY

**Status:** COMPLETE — one defect FIXED and RUNTIME VERIFIED; one UI gap confirmed absent and deliberately left OPEN
**Date:** 2026-09-29
**Issues:** `ISSUE-23` (found + closed here) · `UNI-D13` / `RT-07` (confirmed still open)
**Commit:** `fix: restore XP and HUD integrity`
**Scope:** HUD/XP only. Weapon balance, GunController damage, mobile, enemy AI, modular environment, character and art pipeline untouched. `DEC-11` not touched.

---

## 1. Initial State

Carried from Phase 5B/5B.1: canonical scene `TestArena`, single canonical `GunController`
`Player#70770`, `DEC-11` open, `TestArena.unity` carrying 370 lines of unrelated pre-existing work.

Canonical records asserted three things, all of which this phase re-tested rather than assumed:
`HUDController.xpBar` / `levelText` are dead fields; **no XP UI exists** in the scene; historical
HUD evidence from 2026-09-27 is real but not a current PASS.

## 2. Existing XP Architecture

Evidence-based map (all confirmed at runtime):

| Concern | Owner | Detail |
|---|---|---|
| XP source | `EnemyHealth.SpawnXP()` | on death instantiates `XPOrb.prefab`, `SetXP(xpReward)`; `xpReward = 1`, elite ×3 |
| Prefab wiring | all 4 enemy prefabs | `Bandit` / `Rusher` / `Shooter` / `Tactical` → `xpPrefab` = `XPOrb.prefab` (guid `13eceda…`) |
| Pickup | `XPOrb.OnTriggerEnter` | tag `Player` → `XPManager.AddXP(xpAmount)` → destroys itself |
| XP + level storage | `XPManager` (on `Player`) | `currentXP`, `requiredXP`, `currentLevel` |
| Level-up | `XPManager.AddXP` `while` loop | `currentXP -= requiredXP`; `LevelUp()`; `requiredXP = round(requiredXP * 1.35f)` |
| Level-up broadcast | `XPManager.OnLevelUp` event | `UpgradeManager.OnEnable` subscribes, `OnDisable` unsubscribes |
| Upgrade choice | `UpgradeManager.HandleLevelUp` | `Time.timeScale = 0` + `UpgradeUI.Show(3 random upgrades)` |

**Answers to §3 questions:** current XP and current level both live in `XPManager`; XP is granted by
enemy death → `XPOrb`; level-up happens in `XPManager.AddXP`; `UpgradeManager` is told via the
`OnLevelUp` event; the HUD is told by **polling** (`XPBarController.Update`), not by any event.

## 3. Existing HUD Architecture

Two *different* fields share the name `xpBar`, which is the source of the long-standing confusion:

| Component | Object | `xpBar` type | Role |
|---|---|---|---|
| `XPBarController` | `XPBar` (+ child `XPFill`) | `Image xpFill` | **real, working** XP bar — polls `CurrentXP / RequiredXP` into `fillAmount` |
| `UpgradeUI` | `UpgradePanel` | `GameObject xpBar` | hides the bar while the upgrade panel is open, restores on `Hide()` |
| `HUDController` | `HUD` | `Slider xpBar`, `TMP_Text levelText` | **dead** — `UpdateHealth()` only ever touches `healthBar` / `healthText` |

Scene assignment: `HUDController.xpBar = {fileID: 0}`, `HUDController.levelText = {fileID: 0}`.

## 4. Historical Evidence (2026-09-27) — kept separate

`UNI-0003` §1.0 FIX-01 recorded a white-haze overlay caused by a fullscreen `Image` on
`TestArena → MobileUI → HUD`; the fix set `m_Enabled: 0` on component fileID `2096943043`.

- **Current state confirmed:** that component is still `m_Enabled: 0` in the live scene. The fix persists.
- **Historical claim, NOT re-verified as a current PASS:** FPS 60 / 0 console errors / two runs, and
  `Confirmed = NO` because the user never visually signed it off. This phase did not re-run the A/B
  visual checks and does not upgrade that status.

`fileID 2096943043` is a **component** of HUD GameObject `2096943040` (not the object itself), with
`m_Color RGBA(1, 1, 1, 0.392)`.

## 5. Current Runtime Evidence

Baseline: `XP=0 Level=1 Req=10 damage=100 fireRate=0.4 range=15 gunId=70770`, `HUDController.xpBar`/
`levelText` both NULL, `XPBar` active, `XPFill.fillAmount = 0`.

| Observation | Result |
|---|---|
| XP bar exists and is a real UI object | **YES** — `XPBar` + `XPFill`, active in hierarchy |
| `XPFill` tracks XP | **YES** — 0 → **0.5** at 5/10 XP; **0.8723** at 246/282 (exact match) |
| `HUDController.xpBar` / `levelText` assigned | **NO** — both `{fileID: 0}`, never read in code |
| Level readout object anywhere in scene | **NONE** — 17 `TMP_Text` in scene; only `"LEVEL UP!"` on the upgrade panel, which is not a level display |
| `UpgradeUI` instances | 1 |

**This corrects the canonical record.** `PHASE5A_CORE_INTEGRITY_REPORT.md`, `UNI-0003` L85 and
`GAME-0002` L66 all state that *no XP UI objects exist* and the XP readout is broken. That is
**factually wrong**: the XP bar exists and works. Only the *level* readout is missing. `HUDController.xpBar`
is dead precisely *because* the bar role is served by `XPBarController`.

## 6. Root Cause (defect found this phase)

**Every upgrade choice was applied twice.** A single click on `DAMAGE +20%` produced ×1.44 instead
of ×1.20.

Measured proof, not inference:

| Step | Measurement |
|---|---|
| `UpgradeManager.ApplyUpgrade(Damage)` called once directly | ratio **1.2** — `UpgradeManager` is correct |
| One click on the button | ratio **1.44** — two applications |
| `RemoveAllListeners()`, then one click | **no change** — listeners were the cause |
| Add exactly one listener, then one click | ratio **1.2** — correct |
| runtime listener census, scene freshly loaded | `damageButton`/`fireRateButton`/`maxHealthButton` = **0** |
| runtime listener census, after one level-up | all three = **2** |

Mechanism: `UpgradePanel` is **inactive at scene start**, so `UpgradeUI.Awake()` does not run during
scene load (census = 0). It runs *later* — from inside `Show()`, at `upgradePanel.SetActive(true)` —
which is **after** `Show()` has already called `SetButton()` for all three buttons, and `SetButton()`
does `RemoveAllListeners()` then `AddListener(ChooseDamage)`. `Awake()` then appended a **second**
identical listener on top. One click ⇒ `ChooseDamage` twice ⇒ upgrade applied twice.

`UpgradeUI.cs` was confirmed **unmodified** by this phase — the defect is pre-existing, not introduced here.

## 7. Changes

Single product file: `Assets/Scripts/Player/UpgradeUI.cs`.

The three `AddListener` calls were removed from `Awake()`. `SetButton()` — which already calls
`RemoveAllListeners()` immediately before adding — is now the single registration point. The buttons
are only ever used through `Show()`, so behaviour is otherwise unchanged. No new abstraction, no new
UI object, no damage/multiplier/HP change.

Deliberately **not** changed: `HUDController` (dead fields kept, see §16), `XPManager`, `XPBarController`,
`XPOrb`, `EnemyHealth`, weapon values, `DEC-11`.

## 8. XP Tests

| Test | Result |
|---|---|
| XP source identified | `EnemyHealth.SpawnXP()` → `XPOrb.prefab` → player trigger → `AddXP` |
| below threshold (`AddXP(5)`, req 10) | XP 0→5, level stays 1, no level-up |
| bar reacts to XP | `fillAmount 0 → 0.5` |
| threshold reached (`AddXP(5)`) | XP 5→0, level 1→**2**, req 10→**14** (10×1.35=13.5→14) |
| multi-level grant `AddXP(1000)` from L3 | level 3→**12** (9 level-ups), req 19→282, XP 246 |
| arithmetic check | hand-traced all 9 steps with `round(req×1.35)` — **matches exactly** |
| no duplicate / no infinite loop | **PASS** — 9 sequential level-ups, no repeats |

## 9. Level-Up Tests

| # | Test | Result |
|---|---|---|
| 1 | XP below threshold | PASS |
| 2 | XP gain | PASS |
| 3 | threshold reached | PASS |
| 4 | level increments exactly once | PASS (1→2 per threshold crossing) |
| 5 | `UpgradeManager` receives the level-up | PASS — `UpgradePanel.activeInHierarchy = True`, `Time.timeScale = 0` |
| 6 | damage upgrade functional | PASS — **100 → 120**, ratio exactly 1.2, on `Player#70770` |
| 7 | no duplicate level-up | PASS |
| 8 | 0 project-origin console errors | PASS |

**Regression check on Phase 5B.1:** `GunControllers in scene = 1`; `UpgradeManager.gunController`
instance = **70770**; the level-up damage upgrade landed on that same instance. No split-brain regression.

**Both fix effects verified on three different upgrade types:**

| Upgrade | Result | Doubled would have been |
|---|---|---|
| `ATTACK RANGE +5` | 15 → **20** (once) | 25 |
| `FIRE RATE +15%` | 0.4 → **0.3478** = /1.15 (once) | 0.3024 |
| `DAMAGE +20%` | 100 → **120** (once) | 144 |

Listener census after the fix: **1 per button** (was 2).

## 10. HUD Tests

| Check | Result |
|---|---|
| XP bar exists | **YES** — `XPBar` / `XPFill` |
| XP bar updates | **PASS** — 0 → 0.5 → 0.8723, exact |
| XP bar hidden while upgrade panel open | **PASS** — `UpgradeUI.Show` hides, `Hide()` restores |
| bar restored with correct fill after `Hide()` | **PASS** — 0.8723 vs expected 0.872 |
| level text exists | **NO — VERIFIED ABSENCE** (not created, per §6) |
| Canvas activation | upgrade panel activates/deactivates correctly |

`levelText` and `HUDController.xpBar` remain **dead fields** — unassigned and never referenced.

## 11. Upgrade Regression

| Check | Result |
|---|---|
| canonical `GunController` | `Player#70770`, **1 instance** in scene |
| level-up reaches `UpgradeManager` | PASS |
| upgrade lands on canonical controller | PASS — `UpgradeManager.gunController` id 70770, damage 100→120 |
| no weapon-balance change | PASS — multiplier `1.20`, `DEC-11` untouched |
| firing unaffected | PASS — `GunController` untouched this phase |

## 12. Console

Fresh Play Mode session, console cleared before the run:

| Metric | Result |
|---|---|
| error+warning entries | 6 (all pre-existing non-project) |
| **project-origin** | **0** |
| Missing Script | **0** |
| NullReferenceException | **0** |

Non-project: Unity AI `NoSubscription` ×N, editor `-automated` notice.

## 13. Final Status

| Area | Status |
|---|---|
| XP source / gain / level-up | **VERIFIED WORKING** |
| Level-up → `UpgradeManager` | **VERIFIED WORKING** |
| XP bar (fill, hide, restore) | **VERIFIED WORKING** |
| Upgrade applied exactly once | **FIXED (Phase 5C)** |
| Level readout | **VERIFIED ABSENT — not implemented, `UNI-D13` stays OPEN** |
| Canonical `GunController` identity | **VERIFIED — `Player#70770`, 1 instance** |

Split verdict, per §15: **gameplay progression is verified; the level-readout UI is absent and not
implemented.** The UI issue is deliberately *not* closed.

## 14. Documentation Updated

`PROJECT_TRUTH.md` · `Documentation/PROJECT_STATE.md` · `Documentation/History/OPEN_ISSUES.md` ·
`Documentation/GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` ·
`Documentation/UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md` (`UNI-D13` narrowed) · this report.

The 2026-09-27 HUD evidence is preserved unchanged in `UNI-0003` §1.0 and remains marked
`Confirmed = NO`.

## 15. Git Commit

`fix: restore XP and HUD integrity`

Staged: `Assets/Scripts/Player/UpgradeUI.cs`, the documentation listed above, and this report.
**No scene change was needed** — `TestArena.unity` is byte-identical to the start of this phase
(370/2 pre-existing, untouched), so no hunk-patching was required. No `git add .`; the 66 untracked
art/material/prefab files were left alone. No push/merge/rebase/reset/clean/stash.

## 16. Remaining Issues

| Item | Status | Note |
|---|---|---|
| `UNI-D13` / `RT-07` — level never displayed | **OPEN** | narrowed: the *bar* works, only the *level readout* is missing. Needs a UI implementation task |
| `HUDController.xpBar` / `levelText` dead fields | **OPEN, retained** | `xpBar` is redundant with the working `XPBarController` path; kept as the declared contract so the eventual `UNI-D13` fix has a home. Removing them was rejected as it would destroy the contract without fixing the user-visible gap |
| `ISSUE-23` — upgrade applied twice | **CLOSED (RUNTIME VERIFIED)** | fixed this phase |
| `DEC-11` — weapon damage balance | **OPEN** | untouched |
