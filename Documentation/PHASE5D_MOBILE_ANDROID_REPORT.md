# PHASE 5D — MOBILE / ANDROID INTEGRITY

**Status:** COMPLETE — PC runtime verified, Android build verified, device runtime not available
**Date:** 2026-09-29
**Issue:** `RT-02` / `UNI-D07`
**Commit:** see §16
**Scope:** mobile input path only. GunController damage, `DEC-11`, XP/HUD, enemy AI, modular kit, character and art pipeline untouched.

---

## 1. Work Already Performed

Phase 5D was interrupted during build polling. The following was already complete and was **not
repeated**:

- canonical documentation reading; `RT-02` / `UNI-D07` status fixed as OPEN
- full `MobileTouchControls.cs` audit (937 lines, 25 methods, lifecycle + every reference usage)
- determinism census: `PlayerController` = 1, `GunController` = 1, `MainCamera`-tagged camera = 1
- PC baseline (before change) and post-change runtime verification
- scene wiring of the three `MobileTouchControls` references to the canonical Player
- Android build job `build-b8dbd5e015` launched and left running

---

## 2. Existing Build Job

No new build was launched and no job was cancelled.

| Field | Value |
|---|---|
| job id | `build-b8dbd5e015` |
| platform | Android |
| output | `C:/Users/cyril/AppData/Local/Temp/opencode/wwg5d_build/WWG5D.apk` |
| started_at | `2026-09-29T16:23:36Z` |
| completed_at | `2026-09-29T16:27:07Z` |
| duration | **210.28 s** |
| **result** | **succeeded** |
| **errors** | **0** |
| warnings | 989 |
| APK size | **54.05 MB** |
| APK SHA256 | `52B03E909CCE8C06337CEA8B684F11141249FC5529B72DF45ADE1844C8ACDEFA` |

**Not a stale artifact — proven by timestamps.** APK `LastWriteTimeUtc` = `2026-09-29T16:27:04Z`,
i.e. 3 seconds before job completion and 3 min 28 s after job start. The output directory contains
exactly **one** APK. The project had no prior Android output in this directory.

APK structure verified: 569 entries, `AndroidManifest.xml`, `classes.dex` 7.13 MB,
`lib/arm64-v8a/libil2cpp.so` 64.81 MB. Debug-signed (no keystore is configured in the project);
no release build or publish was performed.

---

## 3. Mobile Architecture

`MobileTouchControls` sits on the `MobileUI` object (scene GameObject, not a prefab). Three
serialized references:

| Field | Type | Before | After |
|---|---|---|---|
| `playerController` | `PlayerController` | `{fileID: 0}` | `{fileID: 1483193843}` → Player |
| `gunController` | `GunController` | `{fileID: 0}` | `{fileID: 1483193849}` → Player |
| `playerCamera` | `Camera` | `{fileID: 0}` | `{fileID: 1221732974}` → Main Camera |

Platform gate: `Awake()` **and** `Update()` both begin with
`if (!Application.isMobilePlatform) return;`. This is a **runtime** check, not `#if UNITY_ANDROID`,
so the mobile code ships in every platform build and activates only on device.

Paths: `ProcessTouches` → `StartFiring` / `KeepFiring` / `StopFiring` for fire;
`UpdateMovement` → `ConvertInputToCameraMovement` for aim; joystick is built procedurally in
`CreateDynamicJoystick`; cleanup in `OnDestroy` / `DestroyJoystickTextures`.

**Correction to the canonical record:** `UNI-0003` §1.0 / `UNI-D07` states the three fields are
"self-healed at runtime in `MobileTouchControls.cs` (**`Start()`**)". There is **no `Start()` method**
in the class. Resolution happens in `Awake()`.

## 4. Resolver

`ResolveGunController()` (3 call sites: `Awake`, `StartFiring`, `KeepFiring`):

```csharp
if (gunController != null) return gunController;              // now the normal path
if (playerController == null) playerController = FindFirstObjectByType<PlayerController>();
if (playerController != null) gunController = playerController.GetComponent<GunController>();
```

Before this phase the first branch was always taken as *false* (field null), so every resolution
depended on a scene-wide lookup. After the fix it returns the serialized field immediately and the
lookup is a genuine documented fallback.

Other lookups: `Camera.main` in `Awake()` and again inside `ConvertInputToCameraMovement` (so
camera self-heals even outside `Awake`).

`FindFirstObjectByType<GunController>()` is **absent** from the entire codebase — removed in
Phase 5B.1. The only textual match is inside an explanatory comment.

## 5. Initialization

`Awake()` is the single resolution point; there is no retry, no deferred re-resolve and no
`Start()`.

The documented risk — *"if the Player does not exist at `Start()`, there is no re-resolve"* — is
**not applicable to this scene**, and no retry was added:

- `Player` is a **deserialised scene object** (GameObject fileID `1483193842` in `TestArena.unity`),
  present in memory from the moment the scene loads, not spawned at runtime.
- `FindFirstObjectByType` scans all loaded objects regardless of `Awake` ordering, so relative
  `Awake` order between `MobileTouchControls` and `PlayerController` is irrelevant.
- `GetComponent<GunController>()` only needs the component to exist on the GameObject, which is
  true immediately after deserialisation.
- `Camera.main` requires a camera tagged `MainCamera`; exactly 1 such camera exists and it is a
  scene object.

Per the brief, retry logic was **not** added "just in case". The residual risk is recorded in §14.

## 6. PC Runtime

Fresh Play Mode, canonical scene reloaded from disk first.

| # | Check | Result |
|---|---|---|
| 1 | `Player` present | PASS |
| 2 | `MobileTouchControls` present | PASS |
| 3 | `playerController` resolves | PASS → `Player` |
| 4 | `gunController` resolves | PASS → `Player#78538`, damage **100** |
| 5 | `playerCamera` resolves | PASS → `Main Camera` |
| 6 | `FireButton` present, has no `GunController` | PASS (5B.1 preserved) |
| 7 | `ResolveGunController()` | PASS → same instance |
| 8 | `GunController` count in scene | PASS → **1** |
| 9 | mobile == canonical == `UpgradeManager` | PASS → **True** |
| 10 | mobile fire path (`StartFiring`+`KeepFiring`) | PASS → bullet spawned, `bullet.damage = 100` = canonical |

**Note on instance IDs.** `78538` here, `70770` in Phase 5B.1. Unity instance IDs are
**per-session** and are not stable identifiers. Identity was therefore asserted *within* a session
(mobile == canonical == upgrade == the single `GunController`), not against a hardcoded `70770`.
`70770` remains a valid historical reference only.

## 7. Android Compilation

The Android-specific path demonstrably entered the IL2CPP build. Evidence from the generated
C++ of build `build-b8dbd5e015` (462 generated source files scanned):

| Symbol | Hits in generated IL2CPP C++ |
|---|---|
| `MobileTouchControls` | **682** |
| `ResolveGunController` | **7** |
| `isMobilePlatform` | **8** |

Generated thunks include `MobileTouchControls_Awake_…`, `MobileTouchControls_Update_…`,
`MobileTouchControls_ResolveGunController_…`.

Because the platform gate is a runtime `isMobilePlatform` check rather than `#if UNITY_ANDROID`,
the mobile code is compiled into the APK and activates on device. No compile-time exclusion
applies. There are **no stripping-related or `UNITY_ANDROID` compile errors** (build errors = 0).

## 8. Android Build

**ANDROID BUILD VERIFIED** — `result: succeeded`, **0 errors**, 210.28 s, 54.05 MB debug-signed APK,
IL2CPP, `arm64-v8a`. No release build, no publish.

## 9. Android Device

**ANDROID DEVICE NOT VERIFIED.**

`adb devices` → `List of devices attached` with **no entries**. No emulator and no physical device
is available in this environment. Per the brief this is a normal result, not a report failure.

Consequently **none** of the following is claimed: app launch, touch initialisation on device,
on-device Player/GunController/camera resolution, movement, aim, fire, pause/UI, logcat.

Editor-side resolution and fire-path invocation is **not** treated as device verification.

## 10. Changes Made

**Case B** — remove the reliance on scene-wide lookup in favour of explicit canonical references.

Exactly **3 lines** in one scene file; **no C# changes at all**:

```diff
--- a/Assets/Scenes/TestArena.unity
+++ b/Assets/Scenes/TestArena.unity
-  playerController: {fileID: 0}
-  gunController: {fileID: 0}
-  playerCamera: {fileID: 0}
+  playerController: {fileID: 1483193843}
+  gunController: {fileID: 1483193849}
+  playerCamera: {fileID: 1221732974}
```

`MobileTouchControls.cs` is **byte-identical to its Phase 5B.1 committed state** — its 5B.1
`ResolveGunController()` was already correct and needed no edit. The fix is data, not logic.

`Assets/Scenes/TestArena.unity` also carries 370 lines of pre-existing unrelated work
(`WildWestEnvironmentGenerator`, `DIAG_TEMP_*` debris from `ISSUE-20`). Those were **not** staged;
only the 3-line hunk was written to the index.

## 11. Console

Fresh session, mobile fire path exercised:

| Metric | Result |
|---|---|
| Exceptions / `NullReference` / `Missing Script` | **0** |
| mobile initialization warnings | **0** |
| total error+warning entries | 74 |
| `Setting linear/angular velocity of a kinematic body` | 68 |
| Unity AI `NoSubscription` (not project) | 5 |
| editor `-automated` notice (not project) | 1 |

The 68 kinematic-body warnings are a **pre-existing, ambient** issue, source identified as
`Assets/Scripts/Enemies/EnemyController.cs:2297` (`rb.linearVelocity = Vector3.zero` on a
kinematic body in the room-search path). They are **enemy AI** and therefore explicitly out of
Phase 5D scope; they were observed but **not** fixed. In Phase 5C I had attributed similar warnings
to my own test harness — this run shows they occur without any test manipulation, so that earlier
attribution was incomplete and is corrected here.

## 12. Regression

| Check | Result |
|---|---|
| `GunController` count | **1** (no split-brain reintroduced) |
| mobile == canonical == `UpgradeManager` | **True** |
| authored damage | **100 — unchanged** |
| `FireButton` has no `GunController` | preserved (5B.1) |
| `DEC-11` | untouched, **OPEN** |
| XP / HUD | untouched |
| enemy AI / modular kit / character / art | untouched |
| Play Mode exit | clean, scene `dirty=False`, no unintended saves |
| repo pollution from build | none — build output went to temp, 0 build artifacts in repo |

## 13. RT-02 Status

**RT-02 / UNI-D07 = PARTIALLY VERIFIED** — *not* closed.

| Level | Status |
|---|---|
| PC runtime | **RUNTIME VERIFIED** |
| Android build / compilation | **BUILD VERIFIED** (0 errors) |
| Android device runtime | **NOT VERIFIED** (no device) |

Closing requires device runtime, which is impossible on this hardware. Claiming `Android verified`
would be false: **PC RUNTIME VERIFIED ≠ ANDROID BUILD VERIFIED ≠ ANDROID DEVICE VERIFIED.**

## 14. Remaining Risks

| Risk | Detail |
|---|---|
| **No device verification** | touch input, on-device resolution, movement/aim/fire and UI remain unproven |
| **ABI is `arm64-v8a` only** | 32-bit `armeabi-v7a` devices are unsupported by this build. Discovered here; not previously recorded |
| 989 build warnings | typical IL2CPP/Android volume; not triaged individually |
| No retry in resolver | safe for this scene (Player is a scene object), but a future scene that **spawns** the Player at runtime would leave `playerController` null with no re-resolve |
| `UNI-D14` (4097 renderers / 1552 objects) | Android performance still unproven; a build succeeding says nothing about frame rate |
| 68 kinematic-body warnings | pre-existing `EnemyController.cs:2297`; needs a separate task |
| `Application.isMobilePlatform` gate | the mobile path cannot be exercised in Editor without a device |

## 15. Documentation Changes

`PROJECT_TRUTH.md` · `Documentation/PROJECT_STATE.md` · `Documentation/History/OPEN_ISSUES.md` ·
`Documentation/GAME/GAME-0003-CONTROLS-AND-PLATFORM-TARGETS.md` ·
`Documentation/UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md` (`UNI-D07` / `RT-02` mapping) · this report.

The 2026-09-27 HUD evidence and all prior phase reports were left untouched.

## 16. Git Commit

Staged: `Assets/Scenes/TestArena.unity` (**3-line hunk only** via index patch), the documentation
listed above, and this report. No C# files, because no C# change was needed.

`git add .` was not used. The 66 untracked art/material/prefab files, the 370 lines of pre-existing
scene work, and all Phase 5B/5B.1/5C product changes were left untouched. No push, merge, rebase,
reset, clean or stash.
