# PROJECT TRUTH

**Document ID:** `DOC-TRUTH-2026-09-29`
**Status:** CANONICAL — consolidated ground truth
**Date:** 2026-09-29
**Evidence basis:** Audit 1 (HARD RECON), Audit 2 (static), U-12 (runtime, 2026-09-29), Phase 2 source discovery

---

## 1. Identity

| Field | Value |
|---|---|
| Project | WildWestGunslinger |
| Genre | Third-person western arena shooter |
| Engine | Unity 6 (6000.x) |
| Platforms | Windows PC (primary dev), Android (target) |
| Stage | Pre-alpha, unreleased |
| Canonical scene | `Assets/Scenes/TestArena.unity` |
| Unity project root | repository root (relative: `.`) |

---

## 2. Canonical scene — resolved

Two files with the same name exist. Evidence decides:

| Field | `Assets/Scenes/TestArena.unity` | `Assets/TestArena.unity` |
|---|---|---|
| Size | 223 870 B | 223 870 B |
| Modified | **2026-09-26 23:08:42** | 2026-09-26 22:46:04 |
| SHA256 (head) | `C718164DDCCA5DE3…` | `0C589C5BF660DCAF…` |
| GUID | `bcc94342232f0d0468763dc03a0ec36d` | `92227d062a096334b8edf7335614c361` |
| In Build Settings | **Yes (enabled)** | No |
| Loaded at runtime | **Yes — active scene** | No |
| Git | tracked | **untracked** |
| Diff | 2 differing lines | — |

> **DECISION:** `Assets/Scenes/TestArena.unity` is canonical.
> `Assets/TestArena.unity` is a **DUPLICATE — REVIEW, DO NOT DELETE.**

---

## 3. What actually works (RUNTIME VERIFIED, 2026-09-29)

Confirmed by play-mode observation on 2026-09-29:

| System | Evidence |
|---|---|
| Arena generation | 9 rooms, 71 walls, 11 floors; `ArenaTacticalMap.IsBuilt = true` |
| Cover system | 148 cover objects, 52 spawn zones, 72 `CoverPoint` registered |
| Enemy spawning | Enemies instantiate at runtime |
| Enemy FSM | Reaches `Searching` state; vision scanning active |
| Enemy navigation | Custom A\* pathfinder produces non-null paths; movement observed |
| Weapons | Both `GunController` prefabs fire |

---

## 4. What is broken (runtime-confirmed)

| # | Defect | Severity | Evidence |
|---|---|---|---|
| 1 | **12 broken object references** | Critical | Runtime |
| 2 | **4 missing Mono Script** references | Critical | Runtime |
| 3 | **Zero NavMesh** — 0 agents, 0 triangulation | Critical | Runtime |
| 4 | **No character rig** — 0 Animator, 0 Avatar, 0 Controller, 0 SkinnedMeshRenderer | Critical | Runtime |
| 5 | **Player and enemies are Capsules** | Critical | Runtime |
| 6 | **Modular FBX import at 0.01× scale**, Z-up not compensated | Critical | Runtime |
| 7 | **`Shooter.prefab` uses `TMP_SDF-HDRP LIT`** (font material as surface) | High | Runtime |
| 8 | **`Rusher.prefab` uses `FrameDebuggerRenderTargetDisplay`** | High | Runtime |
| 9 | **Gun damage overwritten to 200** (Inspector shows 10 / 100) | High | Runtime |
| 10 | **`MobileTouchControls` references are null** — latent on PC, **critical for Android** | Critical (Android) | Runtime |
| 11 | **0 destructible instances** despite controlled-fracture assets existing | Medium | Runtime |
| 12 | **0 modular meshes integrated** in scene | High | Runtime |

**Untested at runtime** (must not be claimed as working): AI combat,
investigation, sound propagation, cover-taking behaviour, flanking.

---

## 5. Scene content census

- 294 `WWG_*` objects present in scene
- 12 broken references among them
- Most scene dependencies (`WWG_*.prefab` in `Assets/Prefabs/Environment/`,
  `WWG_*.mat` in `Assets/Materials/`) are **untracked in Git**

---

## 6. Git state

| Field | Value |
|---|---|
| Branch | `main` |
| HEAD | `7ad314c869e5dad1daf8e56fdf0b8f4197592969` |
| `origin/main` | `f932fcc722d22b8150ccf7e9616914324347c6e8` |
| Divergence | **ahead 1 / behind 2** |
| Working tree | **82 entries: 15 modified, 2 deleted, 65 untracked** |
| Staged | 0 |
| History start | 2026-09-25 (git initialised **after** most art work) |

`origin/main` is ahead by 2 commits and contains two Markdown files
(`AI_PRODUCTION_METHODOLOGY.md`, `AUDIT_SESSION_CONTEXT_2026-09-28.md`) that are
**absent from the local working tree**.

### Recovery branch and tag

| Ref | Commit |
|---|---|
| `recovery/checkpoint-2026-09-29-audit2` | `040651b1d8963231b814217319b099b7ab5ef16d` |
| tag `recovery-2026-09-29-audit2` | `040651b1d8963231b814217319b099b7ab5ef16d` |

---

## 7. Art truth

**Art delta after the 2026-09-29 recovery point: ZERO.** See
`ART_DELTA_AFTER_RECOVERY.md`.

### Art lives in three unversioned, non-canonical locations

| Location | Files | Size | Newest |
|---|---|---|---|
| `~/Documents/WildWestGunslinger art` | 307 | 180.2 MB | 2026-09-26 |
| `~/Desktop/Коллаж тест VVG` | 24 | ~13.5 MB | 2026-09-26 |
| `Working/` (Blender sources + QA reports) | 747 | 226.1 MB | 2026-09-29 16:34 |

### Character foundation — corrected

| Item | Canonical value | Source |
|---|---|---|
| Character basis | Kevin Iglesias human dummy, size M | `Doomy_measurements.md` |
| Height to crown | **1818 mm** | `Doomy_measurements.md` |
| Torso Z range | 926–1579 mm | `Doomy_measurements.md` |
| Head Z range | 1537–1818 mm | `Doomy_measurements.md` |
| Head height / width / depth | 281.1 / 198.2 / 255.4 mm | `Doomy_measurements.md` |
| Chest | **100** (size-50 table: chest width 21) | `Doomy_measurements.md` + vest reference |
| Armature | `WWG_Template_Armature`, **51 bones** | `Doomy_measurements.md` |

> **CORRECTION:** `AUDIT_2_RECONCILIATION_REPORT.md` states `Chest=141cm` and
> `Skeleton=62 bones`. **Both are false.** Chest is 100 (size 50), skeleton is
> 51 bones. The audit's "foundation identified" conclusion must be re-derived.

---

## 8. Backups

| Backup | Location | Size | Verified |
|---|---|---|---|
| 2026-09-22 full | `Z:\Мой диск\WWG_RECOVERY\2026-09-22_FULL\` | 90 626 files / 11.87 GB | Manifest says PASS; **cloud sync UNKNOWN** |
| 2026-09-29 recovery | `~/WildWestGunslinger_RECOVERY\` | 873 files / 227.64 MB | **12/12 SHA256 PASS** |

The 2026-09-29 recovery **excludes** the character art source in `Documents` and
the `Desktop` collage. The 2026-09-22 `Z:` backup **does** include the art copy
(161 files / 96.9 MB) but declares `SECRETS: EXCLUDED` and does **not** prove
cloud upload completion.

---

## 9. Build artifacts

| Item | Detail | Status |
|---|---|---|
| `~/Desktop/WildWest.apk` | 46.2 MB, 2026-09-09, SHA256 `022177FFCC8070DEC…` | **HISTORICAL ARTIFACT — STALE, NOT A CURRENT RELEASE** |
| `~/Desktop/WildWest_BackUpThisFolder_ButDontShipItWithYourGame\` | IL2CPP output, 548 files / 656.5 MB | Build artifact, do not ship |
| `~/Desktop/WildWestGunslinger_BurstDebugInformation_DoNotShip\` | 1 file / 0.1 MB | Do not ship |

`Managed/Assembly-CSharp.dll` is dated **2026-09-02** — the IL2CPP build reflects
code far older than the current project state.

> **There is NO current release build.** The 2026-09-09 APK predates the current
> scene (2026-09-26), the entire character programme (2026-09-21…26) and Git
> initialisation (2026-09-25). It is **not** evidence that the project builds
> today, and **not** evidence that Android works — touch controls are broken
> (`UNI-D07`). No build from current code has ever been attempted.

> **CORRECTION:** `AI_TOOLCHAIN_AUDIT.md` claims no release build exists. **False**
> — an APK exists. Equally, claiming a *current* release exists would also be
> **false**. Both statements are wrong in opposite directions; the artifact is
> historical.

---

## 10. Asset promotion state

**Two assets are `APPROVED`** (`BARREL_01`, `FENCE_01` - recorded user confirmation). `CRATE_01` is at `QA PASS` with CP2 not authorized. **No asset has reached `RUNTIME VERIFIED`.** Highest state of the remaining assets is
`QA PASS`. Modular environment FBX, character FBX, and all `WWG_*` prefabs are
below the promotion line, and the character rig is not integrated at all.

---

## 11. What must be decided before any release or promotion work

1. Repository material policy
2. Character foundation identity (which of 32 `WWG_Foundation_Source` variants is canonical)
3. `Crate` CP2 promotion
4. `Wall` / `Floor` FINAL selection
5. `Cover` r4 / r5 status
6. Legacy Stage numbering authority

See `Documentation/History/OPEN_DECISIONS.md`.

---

**End of `PROJECT_TRUTH.md`**
