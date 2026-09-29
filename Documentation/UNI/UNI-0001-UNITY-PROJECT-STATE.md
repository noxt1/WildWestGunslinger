# UNI-0001 — Unity Project State

**Document ID:** `UNI-0001`
**Status:** CANONICAL
**Date:** 2026-09-29
**Evidence class:** runtime observation + verified file facts

---

## 1. Project identity

| Field | Value |
|---|---|
| Engine | Unity 6 (6000.x series) |
| Project root | repository root (relative: `.`) |
| Active scene | `Assets/Scenes/TestArena.unity` |
| Stage | Pre-alpha, unreleased |
| Primary platform | Windows PC |
| Secondary platform | Android (blocked — see `UNI-0003`) |

---

## 2. Scene

| Field | Value |
|---|---|
| Canonical path | `Assets/Scenes/TestArena.unity` |
| GUID | `bcc94342232f0d0468763dc03a0ec36d` |
| Size | 223 870 B |
| Modified | 2026-09-26 23:08:42 |
| Build Settings | **enabled** |
| Runtime | **loaded as active scene** |
| Git | tracked |

Duplicate `Assets/TestArena.unity` (GUID `92227d062a096334b8edf7335614c361`,
untracked, 2 differing lines) exists. **REVIEW — do not delete.**

---

## 3. Runtime-confirmed working systems

| System | Measured result |
|---|---|
| Arena generator | 9 rooms, 71 walls, 11 floors |
| Tactical map | `ArenaTacticalMap.IsBuilt = true` |
| Cover | 148 cover objects, 52 spawn zones, 72 `CoverPoint` |
| Enemy spawning | enemies instantiate at runtime |
| Enemy FSM | reaches `Searching`; vision scanning active |
| Navigation | custom A\* returns non-null paths; movement observed |
| Weapons | both `GunController` prefabs fire |

---

## 4. Runtime-confirmed absent systems

| System | Measured result |
|---|---|
| NavMesh | 0 agents, 0 triangulation |
| Character rig | 0 Animator, 0 Avatar, 0 Controller, 0 SkinnedMeshRenderer |
| Player body | Capsule |
| Enemy bodies | Capsule |
| Destructibles | 0 instances |
| Modular environment | 0 meshes integrated |
| `WWG_*` objects in scene | 294 |
| Animator / Avatar / AnimatorController / SkinnedMeshRenderer | 0 / 0 / 0 / 0 |
| **Equipment sockets / bone attachment points** | **none** — no `socket`, `attach`, `BoneAttach` or `EquipPoint` code exists in `Assets/Scripts` (**VERIFIED ABSENCE**, static) |

---

## 5. Script / code state

| Item | Value |
|---|---|
| Assembly | `Assembly-CSharp` (managed DLL of 2026-09-02 exists in the IL2CPP build) |
| Missing Mono Scripts | **4** |
| Broken object references | **12** |
| Custom pathfinding | A\*-based, functional, not NavMesh |
| Enemy FSM states verified | `Searching` only |
| Enemy FSM states unverified | combat, investigation, sound, cover, flanking |

**Never claim the unverified FSM states as working.**

---

## 6. Untracked scene dependencies

The following are required by the scene but are **untracked in Git** — a clean
clone would not reproduce the scene:

| Path | Note |
|---|---|
| `Assets/Prefabs/Environment/WWG_*.prefab` | critical scene dependencies |
| `Assets/Materials/WWG_*.mat` | critical scene dependencies |

---

## 7. What is NOT established

- No Git history before 2026-09-25, so pre-git work has no provenance in-repo.
- `origin/main` is 2 commits ahead and contains 2 Markdown files absent locally.
- No release configuration, no CI, no build pipeline definition is documented.

---

## 8. Cross-references

- `UNI-0002` — data and asset register
- `UNI-0003` — known defects
- `UNI-0004` — environment and toolchain
- `PROJECT_TRUTH.md` — consolidated truth
- `AUDIT_2_RUNTIME_REPORT.md` — raw runtime evidence

---

**End of `UNI-0001-UNITY-PROJECT-STATE.md`**
