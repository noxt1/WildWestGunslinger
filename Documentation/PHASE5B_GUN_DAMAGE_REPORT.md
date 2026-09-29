# PHASE 5B — GUNCONTROLLER DAMAGE INTEGRITY

**Document ID:** `DOC-P5B-2026-09-29`
**Status:** COMPLETE — **RUNTIME VERIFIED**
**Date:** 2026-09-29
**Base commit:** `4e1c76a` (Phase 5R)

---

## 1. Initial state

| Item | Value |
|---|---|
| HEAD | `4e1c76a80a16b13583d958e909dc619c46a3713b` |
| Runtime channel | available via the documented JSON-RPC fallback (`UNI-0001` §9) |
| `RT-04` / `UNI-D10` | OPEN |
| GunController instances in canonical scene | **2** |
| Authored damage in scene | `Player` = **100**, `FireButton` = **10** |
| Runtime damage (before fix) | **both 200** |
| Enemy base HP | 100 (`EnemyHealth.maxHealth`) |

---

## 2. Root cause

`Assets/Scripts/Weapons/GunController.cs` line 37, inside `Awake()`:

```csharp
damage = Mathf.Max(damage, 200f);
```

A hard-coded **floor** with:

- no design documentation anywhere in the repository
- no `[Min]`, header, tooltip or comment justifying it
- **zero dependents** — a search of all 42 scripts found the literal `200` nowhere else

The clamp could only ever *raise* damage, so any authored value below 200 was silently discarded. It is the sole reason the Inspector value and the runtime value disagreed.

**Correction to Phase 3's note:** the clamp was described as setting damage *to* 200. It is a `Max` floor, so a value above 200 would have survived. In practice both instances were below it, so the observed result was the same.

---

## 3. Intended damage model — derived from code, not assumed

Dependency map, verified line by line:

```
[SerializeField] damage                     ← base, authored in Inspector
   │  GunController.cs:14
   ├─► Awake()                               ← clamp REMOVED (this fix)
   │
   ├─► IncreaseDamage(multiplier)            ← upgrade layer, MULTIPLICATIVE, cumulative
   │     GunController.cs:400-408   (guard: multiplier <= 0 → ignored)
   │     called by UpgradeManager.ApplyDamageUpgrade()  UpgradeManager.cs:177-192
   │     with damageMultiplier = 1.20f       UpgradeManager.cs:22
   │
   ├─► GetDamage()                           ← runtime accessor
   │     GunController.cs:430
   │
   └─► bullet.Initialize(direction, damage)  ← transfer to projectile
         GunController.cs:336-339
             │
             ▼
         Bullet.damage                       Bullet.cs:11, :30-34
             │
             ├─► enemy.TakeDamage(Mathf.RoundToInt(damage))        Bullet.cs:76-77, :124-125
             └─► destructible.TakeDamage(Mathf.RoundToInt(damage)) Bullet.cs:89-90, :137-138
                      │
                      ▼
              EnemyHealth.currentHealth -= amount     (base maxHealth = 100)
```

| Layer | Owner | Value semantics |
|---|---|---|
| **Base damage** | `GunController.damage` (serialized) | authored per weapon |
| **Upgrade damage** | `IncreaseDamage()` via `UpgradeManager` | multiplicative ×1.20 per level, cumulative |
| **Runtime damage** | `GunController.damage` after upgrades | what `GetDamage()` returns |
| **Projectile** | `Bullet.damage` | copied at `Initialize`, rounded to int on hit |
| **Enemy intake** | `EnemyHealth.TakeDamage(int)` | `currentHealth -= amount` |

**Conclusion:** the Inspector value *is* the base-damage layer. The upgrade layer
is multiplicative and already correct. The only anomaly was the floor.

---

## 4. Files changed

| File | Change |
|---|---|
| `Assets/Scripts/Weapons/GunController.cs` | **6 lines deleted** — the `Mathf.Max(damage, 200f)` clamp in `Awake()` |

Diff is a **pure deletion**. No other line touched. `Mathf.Max` occurrences in the file: 1 → **0**.

Pre-fix SHA256 `7738A76…51AD37`; file was clean in Git before the edit, so it is restorable from `HEAD`.

**Deliberately not changed:** the field default `[SerializeField] private float damage = 200f;` (line 14). That is authored configuration for *newly added* components, not a defect, and changing it would substitute one arbitrary number for another. Recorded in §5 as an observation for a balance decision.

---

## 5. Duplicate controller findings — recorded, not fixed

**§3 requirement: evidence only, no duplicate-integration fix in this phase.**

Empirically measured at runtime:

| Consumer | Resolution | Bound to | Base damage |
|---|---|---|---|
| `FindFirstObjectByType<GunController>()` | first match | **`FireButton`** | **10** |
| `UpgradeManager.gunController` | `GetComponent<GunController>()` on Player | **`Player`** | **100** |
| `MobileTouchControls.gunController` | *(not yet assigned — null)* | — | — |

```
SPLIT = true
```

**Confirmed defect (`RT-03` / new `ISSUE-22`):** the weapon that fires and the object that receives damage upgrades are **two different `GunController` instances**. Damage upgrades are therefore delivered to the Player's controller, while the firing path uses the FireButton's controller.

This was masked before the fix, because **both were clamped to 200** and the difference was invisible. **The fix exposes it.** This is a genuine, important consequence and is called out in §8.

---

## 6. Static verification

| Check | Result |
|---|---|
| Diff shape | pure deletion, 6 lines |
| Other damage logic untouched | `IncreaseDamage`, `GetDamage`, `bullet.Initialize` intact |
| `Mathf.Max` in file | **0** |
| Compile | **0 errors** after `refresh_unity` |
| File restorable | yes, clean in Git pre-edit |

---

## 7. Runtime tests

All run through the restored Unity MCP channel. Nothing saved.

| # | Test | Expected | Observed | Result |
|---|---|---|---|---|
| T1 | Inspector base damage, `Player` | 100 | **100** | **PASS** |
| T2 | Inspector base damage, `FireButton` | 10 | **10** (was 200) | **PASS** |
| T3 | Runtime base, 0 upgrades | = base | **100** | **PASS** |
| T4 | Damage upgrade, 0 multiplier | ignored | **172.8 unchanged** | **PASS** |
| T5 | Damage upgrade, +1 (×1.20) | 120 | **120** | **PASS** |
| T6 | Damage upgrade, +2 | 144 | **144** | **PASS** |
| T7 | Damage upgrade, +3 | 172.8 | **172,8** | **PASS** — cumulative |
| T8 | Player fire | projectile created | bullets spawned continuously, 2–6 in flight, `damage=10` each | **PASS** |
| T9 | Enemy receives damage | HP drops / death | **enemies 3 → 2** (one killed) | **PASS** |
| T10 | Console | 0 project errors | **0 of 500 entries** | **PASS** |

**Before/after comparison (same session, same scene):**

| | Before fix | After fix |
|---|---|---|
| `Player` damage | 200 | **100** |
| `FireButton` damage | 200 | **10** |

### 7.1 Time-to-kill arithmetic

Bullet damage **10**, enemy `maxHealth` **100** → 10 hits. `fireRate = 0.4 s`
→ 4.0 s of sustained fire. Firing for 4 s produced exactly one enemy death
(3 → 2). The chain Inspector → `GunController` → `Bullet` → `EnemyHealth` →
death is therefore confirmed end to end with authored values.

---

## 8. Damage values — behaviour change to be aware of

| Weapon | Damage before | Damage after | Hits to kill a 100 HP enemy |
|---|---|---|---|
| `FireButton` (the one that actually fires) | 200 | **10** | 1 → **10** |
| `Player` (the one upgrades target) | 200 | **100** | 1 → **1** |

**This is a deliberate and correct consequence of restoring authored intent**, but
it is a real balance change: the effective firing weapon becomes **10× slower to
kill** than the current build, while upgrades buff a controller that does not fire.

Two decisions are therefore now open and are **not** mine to make:

1. Should the `FireButton` base damage really be 10, or was 100/200 intended?
2. Should the duplicate `GunController` be resolved so firing and upgrades share one instance?

---

## 9. Upgrade tests

| Check | Result |
|---|---|
| `IncreaseDamage` is multiplicative, not additive | **confirmed** — 100→120→144→172.8 |
| Cumulative across multiple calls | **confirmed** |
| `multiplier <= 0` guard prevents a reset to 0 | **confirmed** — unchanged at 172.8 |
| `UpgradeManager` binds the Player controller | **confirmed** at runtime |
| `UpgradeManager.damageMultiplier` | `1.20f` (`UpgradeManager.cs:22`) |
| `UpgradeManager` calls `gunController.IncreaseDamage(damageMultiplier)` | confirmed (`UpgradeManager.cs:190-191`) |
| **Full level-up UI flow exercised** | **NO** — the matrix drove `GunController.IncreaseDamage` directly, the same method `UpgradeManager` calls. The UI → level-up → apply path was not driven |

---

## 10. Console

500 entries read after the fix and a full fire test.

| Category | Count |
|---|---|
| **Project-origin errors** (`error CS`, `Compilation failed`, `Missing (Mono`, `referenced script`, `NullReference`, `Exception`) | **0** |
| Unity AI service `NoSubscription` (unrelated) | present |
| Upgrade log lines | 0 (no upgrade was applied through the UI) |

---

## 11. Regression check

| System | Result |
|---|---|
| Enemies spawn | 3 spawned |
| Enemies path / FSM | no change observed |
| Player movement | unaffected (not touched) |
| Weapon firing | **improved** — bullets carry the authored damage |
| Destructible damage path | `Bullet.cs:89,137` still forwards the same value; unchanged |
| Enemy AI, UI, mobile, art | **not touched by this change** (1 file, 6 deleted lines) |
| `RT-06` missing scripts | not reintroduced — 0 in console |

---

## 12. Issue status

| Issue | Before | After |
|---|---|---|
| `RT-04` / `UNI-D10` — damage overwritten to 200 | OPEN | **✅ FIXED → RUNTIME VERIFIED** |
| `RT-03` — duplicate `GunController` / split binding | noted as risk | **CONFIRMED empirically → `ISSUE-22`, still OPEN** |
| `RT-01`, `RT-02`, `RT-05` | OPEN | untouched |

---

## 13. Documentation updated

| Document | Update |
|---|---|
| `Documentation/UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md` | `UNI-D10` **RUNTIME VERIFIED**; `RT-04` row closed; `RT-03` row upgraded with measured evidence |
| `Documentation/GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` | combat damage row corrected to authored values |
| `Documentation/PROJECT_TRUTH.md` | defect table + combat summary |
| `Documentation/PROJECT_STATE.md` | gun damage row corrected |
| `Documentation/History/OPEN_ISSUES.md` | `ISSUE-13`-style entry: `ISSUE-22` duplicate controller added |
| `Documentation/History/OPEN_DECISIONS.md` | `DEC-11` weapon base damage + controller identity |
| `Documentation/PHASE5B_GUN_DAMAGE_REPORT.md` | this report |

`RT-04` is **retained in history**, marked closed, not deleted.

---

## 14. Git commit

| Field | Value |
|---|---|
| Message | `fix: restore configurable gun damage` |
| Product files | 1 (`GunController.cs`) |
| Documentation | 7 |
| Prohibited operations | none — no `add .`, `amend`, `merge`, `rebase`, `reset`, `clean`, `stash`, `push` |
| Other 74 product changes | excluded |

---

**End of `PHASE5B_GUN_DAMAGE_REPORT.md`**
