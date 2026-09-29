# PHASE 5B.1 — GUNCONTROLLER IDENTITY / FIREBUTTON WIRING

**Status:** COMPLETE — RUNTIME VERIFIED
**Date:** 2026-09-29
**Canonical finding closed:** `ISSUE-22` / `RT-03`
**Commit:** `fix: unify gun controller firing and upgrade target`
**Scope:** identity/wiring only. **No balance decision taken** — `DEC-11` remains OPEN.

---

## 1. Initial State

Phase 5B removed the `Mathf.Max(damage, 200f)` clamp in `GunController.Awake()` and thereby
*exposed* a wiring defect that the clamp had hidden: while both instances were forced to 200 they
were indistinguishable, so nobody noticed that firing and upgrades targeted different objects.

Carried forward from Phase 5B: `ISSUE-22` OPEN, `DEC-11` OPEN, canonical scene `TestArena`.

## 2. Controller Inventory (BEFORE — runtime)

Captured via `FindObjectsByType<GunController>` on the reloaded canonical scene:

| Instance ID | Object | Path | damage | weaponPoint | playerController |
|---|---|---|---|---|---|
| 67864 | `Player` | `Player` | 100 | `WeaponPoint` | **OK** |
| 67492 | `FireButton` | `MobileUI/HUD/FireButton` | 10 | **`Player` (root transform)** | **NULL** |

```
TOTAL_GUNCONTROLLERS      = 2
UPGRADE_TARGET instanceID = 67864   (Player)
FIND_FIRST_ID             = 67492   (FireButton)
SPLIT_UPGRADE_VS_FIRE     = True
FIREBUTTON_GC             = PRESENT
```

Decisive detail: the duplicate's `playerController` resolved to **NULL** — the UI button has no
`PlayerController` in its ancestry — while its `weaponPoint` pointed at the **Player's root
transform**. A HUD control was acting as a weapon, firing from the player's body.

## 3. Fire Path (BEFORE)

`MobileTouchControls` (on `MobileUI`, serialized field `gunController: {fileID: 0}`) →
`FindFirstObjectByType<GunController>()` → **FireButton instance 67492** → `SetFiring` /
`FireButtonDown` → `Update` gate (`firing == true && weaponPoint != null`) → `Shoot()`.

Three call sites performed this unordered lookup: `Awake`, touch-fire start, and `KeepFiring`.
Off-device it is dormant because `Awake()` early-returns when `!Application.isMobilePlatform` —
which is exactly why the split survived Editor testing.

## 4. Upgrade Path (BEFORE)

`UpgradeManager` (on `Player`) → `GetComponent<GunController>()` → **Player instance 67864** →
`IncreaseDamage(damageMultiplier)`. Correct target, but a *different* object from the firing one.

Net effect: **every Damage upgrade multiplied a controller that never fired.**

## 5. Root Cause

Not "a duplicate is bad" — the second controller existed for a specific, identifiable reason and
filled a real role, which is why removing it was safe:

1. `MobileTouchControls.gunController` was **never assigned in the scene** (`fileID: 0`).
2. The class compensated with **unordered scene-wide discovery**, `FindFirstObjectByType<GunController>()`.
3. `GunController` was therefore attached to the `FireButton` UI control as a **convenient handle**
   that happened to be reachable from the mobile input code — the nearest thing to a "fire" object
   in the mobile hierarchy.
4. That handle was mis-configured: `weaponPoint` → Player root, no `PlayerController` ancestry.
5. The real weapon controller on `Player` already existed and was fully configured, but nothing in
   the fire path referenced it.

So the duplicate was an **artifact of runtime discovery substituting for a missing explicit
reference**, not a second weapon. That is why deletion is correct rather than a merge of two real
things.

## 6. Files Changed

| File | Change |
|---|---|
| `Assets/Scripts/Player/MobileTouchControls.cs` | added `ResolveGunController()`; replaced 3 unordered `FindFirstObjectByType<GunController>()` sites |
| `Assets/Scripts/Player/UpgradeManager.cs` | fallback re-anchored on `PlayerController` instead of scene-wide `FindFirstObjectByType<GunController>()` |
| `Assets/Scenes/TestArena.unity` | removed obsolete `GunController` from `FireButton` (22 lines) |

Base damage, bullet damage model, enemy health, upgrade multiplier, AI, HUD: **untouched**.

## 7. Exact Wiring Change

Canonical instance: **`Player/GunController` (instance 70770)**, chosen against §4 criteria —
owned by the real player weapon, `weaponPoint` → `WeaponPoint`, valid `PlayerController` ancestry,
already the upgrade target, and no duplicate required.

```
BEFORE  MobileTouchControls --FindFirstObjectByType--> FireButton#67492 --Fire--> bullets
        UpgradeManager       --GetComponent---------> Player#67864    --IncreaseDamage-->

AFTER   MobileTouchControls --ResolveGunController()-> Player#70770 --Fire--> bullets
        UpgradeManager       --GetComponent---------> Player#70770 --IncreaseDamage-->
```

New resolver (deterministic; scene iteration order is irrelevant):

```csharp
private GunController ResolveGunController()
{
    if (gunController != null) return gunController;
    if (playerController == null)
        playerController = FindFirstObjectByType<PlayerController>();
    if (playerController != null)
        gunController = playerController.GetComponent<GunController>();
    return gunController;
}
```

`FindFirstObjectByType<GunController>()` no longer appears in any executable code path in the
codebase (the only remaining textual match is inside a comment explaining the removal).

## 8. Runtime Before

See §2. `SPLIT_UPGRADE_VS_FIRE = True`; 2 controllers; `FireButton` carried one.

## 9. Runtime After

```
TEST1_CONTROLLER_COUNT = 1          (was 2)
TEST2_FIRE_ID          = 70770  Player
TEST3_UPGRADE_ID       = 70770  Player
TEST4_IDENTITY_EQUAL   = True
FIREBUTTON_GC          = REMOVED
```

## 10. Identity Equality Result

**`FIRING instanceID == UPGRADE_TARGET instanceID` → `70770 == 70770` — PASS.**
This is the primary acceptance criterion of Phase 5B.1.

## 11. Damage Preservation

| Check | Value |
|---|---|
| Authored base damage, fresh session | **100 — unchanged** |
| After one ×1.20 upgrade | **120** |
| Upgrade landed on canonical firing controller | **True** |
| Other controllers mutated | **none (count = 1)** |

No damage value was edited. `100` remains `100`; the `10` disappeared only because the obsolete
component carrying it was deleted. **Consequence to note:** effective firing damage is now 100
rather than the 10 the mis-wired path was firing at. That is the unavoidable result of unifying
onto the canonical instance, and choosing the final number is **`DEC-11`, deliberately left OPEN**.

## 12. Mobile Sanity

`Android = NOT VERIFIED` — no Android runtime/device was available, and no claim is made.
What *was* verified is the wiring logic itself: the private `ResolveGunController()` was invoked
directly on the live `MobileTouchControls` instance and returned `Player#70770` (Test 2), so the
mobile path is pointed at the canonical weapon. `MobileTouchControls` remains present and
unbroken; the fix touched only reference resolution, not the touch architecture.

## 13. Console

Clean Play Mode session (console cleared, no test manipulation):

| Metric | Result |
|---|---|
| error+warning entries | 6 (all pre-existing non-project) |
| **project-origin errors/warnings** | **0** |
| Missing Script warnings | **0** |
| NullReferenceException | **0** |
| compile errors after edit | **0** |

Non-project entries: Unity AI `NoSubscription` and the `-automated` editor notice.
Note: an earlier session showed ~140 `Setting linear velocity of a kinematic body` warnings.
Those were **caused by the test harness itself** (`isKinematic = true` applied to hold an enemy in
place), not by the product change, and did not recur once Play Mode was properly restarted.

## 14. Regression

| Check | Result |
|---|---|
| `Player` exists | PASS |
| `GunController` exists (exactly 1) | PASS |
| `FireButton` exists, no `GunController` | PASS |
| `UpgradeManager` exists | PASS |
| `MobileTouchControls` exists | PASS |
| firing works | PASS (bullets spawned from canonical weapon) |
| upgrade works | PASS (100 → 120 → 172.8) |
| bullet works | PASS (`bullet.damage = 120`, sampled mid-flight) |
| enemy damage works | PASS (100 HP enemy destroyed; enemy count 3 → 2) |
| no new project errors | PASS |

## 15. Documentation Updated

`PROJECT_TRUTH.md` · `Documentation/PROJECT_STATE.md` · `Documentation/History/OPEN_ISSUES.md` ·
`Documentation/GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` ·
`Documentation/UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md` (`RT-03`) · this report.

## 16. ISSUE-22 Status

`OPEN → FIXED → RUNTIME VERIFIED → CLOSED`

- **BEFORE:** fire controller `67492` ≠ upgrade controller `67864`
- **ROOT CAUSE:** unassigned `MobileTouchControls.gunController` + unordered `FindFirstObjectByType`
  discovery produced a `GunController` handle attached to the `FireButton` UI control
- **AFTER:** one canonical instance `Player#70770` for both paths
- **VERIFICATION:** runtime instance IDs 70770 == 70770, single controller, 0 project errors

## 17. DEC-11 Status

**OPEN — deliberately not decided in this phase.** Player base damage stays `100`, enemy HP stays
`100`, upgrade multiplier stays `1.20`. The effective TTK change created by unification is recorded
as input to `DEC-11`, not resolved here.

## 18. Git Commit

`fix: unify gun controller firing and upgrade target`

Staged: 2 product `.cs` files, `TestArena.unity` (**single hunk only** — the scene carried 370 lines
of unrelated pre-existing work: `WildWestEnvironmentGenerator` + `DIAG_TEMP_*` debris from
`ISSUE-20`, deliberately left unstaged), and the documentation listed above.
No `git add .`, no push, no merge/rebase/reset/clean/stash.
