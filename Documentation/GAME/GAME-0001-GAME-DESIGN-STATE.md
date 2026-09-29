# GAME-0001 — Game Design State

**Document ID:** `GAME-0001`
**Status:** CANONICAL
**Date:** 2026-09-29
**Evidence class:** runtime observation (arena, weapons, enemies) + verified file facts

---

## 1. Design intent

| Field | Value |
|---|---|
| Genre | Third-person western arena shooter |
| Perspective | Third person |
| Core mode | Arena combat — free-roam rooms with cover, wave/zone-based enemy pressure |
| Setting | Western town interiors (9 rooms) |
| Tone | Gritty western; Kevin Iglesias reference basis for characters |
| Stage | Pre-alpha |

Design documentation in the repository is fragmented and stage-numbered
(`CHATGPT_CHECKLIST.md`, `ART_PIPELINE.md`, `AI_CONTEXT/*`). **No single
authoritative game design document exists.** This file states what the build
actually implements.

---

## 2. Arena

Runtime-measured arena composition:

| Element | Count |
|---|---|
| Rooms | 9 |
| Walls | 71 |
| Floors | 11 |
| Cover objects | 148 |
| Spawn zones | 52 |
| Registered `CoverPoint` | 72 |
| `ArenaTacticalMap.IsBuilt` | `true` |
| `WWG_*` objects in scene | 294 |

The arena is **procedurally generated at runtime** — it is not a hand-built
static level. `ArenaTacticalMap` builds a tactical representation used by AI.

---

## 3. Cover system

- 148 cover objects exist in the scene.
- 52 spawn zones are registered.
- 72 `CoverPoint` entries are registered.
- AI **taking cover is NOT runtime-verified** — see `GAME-0002`.

The count mismatch (148 cover objects vs 72 registered `CoverPoint`) is an open
question, not a proven defect: some cover objects may be decorative.

---

## 4. Characters in game

| Role | Representation | State |
|---|---|---|
| Player | **Capsule** | `UNI-D03` |
| Enemies | **Capsule** | `UNI-D03` |
| Character art | Exists in Blender, **not integrated** | `ART-0001` |

No visible human character exists in the playable build.

---

## 5. Weapons

| Weapon prefab | Fires | Damage (runtime) | Issue |
|---|---|---|---|
| Shooter | yes | **200** | `UNI-D10` — Inspector shows 10 |
| Rusher | yes | **200** | `UNI-D10` — Inspector shows 100 |

Both fire. Both have their damage forced to 200 at runtime, overriding authored
values. Weapon balance is currently **not authorable from data**.

Weapon FBX (Carbine, Revolver, Shotgun, Winchester) exist in the art source but
are not integrated.

---

## 6. Level content not yet present

| Feature | State |
|---|---|
| Modular environment | **0 meshes integrated** (`UNI-D11`) |
| Destructibles | **0 instances** (`UNI-D12`) |
| NavMesh-based navigation | absent (`UNI-D01`) |

---

## 7. Design gaps — must not be documented as designed-and-working

- Wave/encounter structure: no runtime evidence
- Scoring, progression, meta: no evidence
- Difficulty tuning: no evidence
- Narrative/dialogue: no evidence
- Weapon variety beyond 2 prefabs: no evidence

---

## 8. Cross-references

- `GAME-0002` — gameplay systems state
- `GAME-0003` — controls and platforms
- `UNI-0001` — Unity state
- `UNI-0003` — known defects

---

**End of `GAME-0001-GAME-DESIGN-STATE.md`**
