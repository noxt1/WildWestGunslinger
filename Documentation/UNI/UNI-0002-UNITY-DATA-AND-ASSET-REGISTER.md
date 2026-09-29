# UNI-0002 — Unity Data and Asset Register

**Document ID:** `UNI-0002`
**Status:** CANONICAL
**Date:** 2026-09-29

---

## 1. Scenes

| Path | GUID | Bytes | Modified | Build Settings | Runtime | Git |
|---|---|---|---|---|---|---|
| `Assets/Scenes/TestArena.unity` | `bcc94342232f0d0468763dc03a0ec36d` | 223 870 | 2026-09-26 23:08:42 | **enabled** | **active** | tracked |
| `Assets/TestArena.unity` | `92227d062a096334b8edf7335614c361` | 223 870 | 2026-09-26 22:46:04 | no | not loaded | **untracked** |

The two differ by 2 lines. `Assets/Scenes/TestArena.unity` is canonical.
The root copy is a **DUPLICATE — REVIEW, DO NOT DELETE**.

---

## 2. Prefabs

| Prefab | Issue | Git |
|---|---|---|
| `Shooter.prefab` | material `TMP_SDF-HDRP LIT` (`UNI-D08`) | tracked |
| `Rusher.prefab` | material `FrameDebuggerRenderTargetDisplay` (`UNI-D09`) | tracked |
| `Assets/Prefabs/Environment/WWG_*.prefab` | required by scene | **untracked** |

Both confirmed-firing weapon prefabs carry a **broken surface material**:
a TextMeshPro font material on Shooter, a frame-debugger display material on
Rusher. Neither has a valid character texture applied.

---

## 3. Materials

| Path / material | Issue | Git |
|---|---|---|
| `TMP_SDF-HDRP LIT` | font material misapplied to a mesh renderer | — |
| `FrameDebuggerRenderTargetDisplay` | debug material never replaced | — |
| `Assets/Materials/WWG_*.mat` | required by scene | **untracked** |

---

## 4. Environment art — modular kit

| Path | State | Blocker |
|---|---|---|
| `Assets/Art/Environment/Modular/` — Wall FBX revisions | present | import at 0.01× (`UNI-D06`) |
| `Assets/Art/Environment/Modular/` — Floor FBX revisions | present | import at 0.01× (`UNI-D06`) |
| FINAL Wall / Floor selection | **OPEN DECISION** | — |
| Scene integration | **0 modular meshes** | `UNI-D06` |

---

## 5. Props art

| Path | State |
|---|---|
| `Assets/Art/Props/` — FBX revisions | present |
| `Assets/Art/Props/` — controlled-fracture assets | present, **0 instances in scene** (`UNI-D12`) |
| `CRATE_01` | `QA PASS` — CP1 complete; **CP2 NOT AUTHORIZED** (`DEC-05`). `BARREL_01`/`FENCE_01` are `APPROVED` but **0 destructible instances** exist in the scene |
| `Cover` r4 / r5 | `QA PASS`; **status is an OPEN DECISION** |

---

## 6. Characters

| Path | State |
|---|---|
| Character FBX in `Assets/` | **not integrated** |
| SkinnedMeshRenderer in scene | **0** |
| Animator / Avatar / Controller in scene | **0 / 0 / 0** |

---

## 7. Git tracking gaps

The following are required by the canonical scene but are **untracked**:

- `Assets/Prefabs/Environment/WWG_*.prefab`
- `Assets/Materials/WWG_*.mat`
- `Assets/TestArena.unity` (the duplicate scene)

**A clean clone cannot reproduce the playable scene.** This is `RISK-03`.

---

## 8. Cross-references

- `UNI-0001` — project state
- `UNI-0003` — known defects
- `ART-0002` — art asset register

---

**End of `UNI-0002-UNITY-DATA-AND-ASSET-REGISTER.md`**
