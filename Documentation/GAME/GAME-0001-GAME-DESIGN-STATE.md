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

## 7. Verified gameplay parameters

**Added in Phase 4.** Phase 2 recorded only runtime counts. These are the
authored values recovered from `AI_CONTEXT` and spot-checked against source.

### 7.1 Arena generation (`ArenaGenerator.cs`, `ArenaTacticalMap.cs`)

| Parameter | Value | Verified |
|---|---|---|
| Layout | Honeycomb 3×3 | `BuildHoneycombLayout()` ✅ |
| Room count | 9 (fixed) | ✅ runtime = 9 |
| Room size | 16–22 × 16–22 units | source |
| Corridor width | 3.5–5.0 | source |
| Corridor length | 4.0–6.0 | source |
| Wall height | 2.4–3.2 | source |
| Wall thickness | 0.4–0.8 | source |
| Generate on start | `generateOnStart = true` | ✅ source L86 |
| Random seed | `useRandomSeed = true` | ✅ source L87 |
| Tactical cell size | `1.5f` | ✅ source L9 |
| Agent radius | `0.65f` | ✅ source L10 |
| Max path nodes | `2500` | ✅ source L15 |
| A\* neighbours | 4-directional, no diagonals | source |
| A\* activation threshold | 5.0 units from target | source |
| Cover per room | 3–5; max = `maximumCoverCount + 3` = 8 | source |
| Spawn zones | 3–5 per arena, radius 1.5–3.0, min separation 4.0 | source |

> ⚠️ `CURRENT_TASK.md` expects `SpawnZones=53` after generation; the U-12 runtime
> measured **52**. One-off discrepancy — see `CONFLICT-14`.

### 7.2 Wave system (`WaveManager.cs`)

| Parameter | Value |
|---|---|
| Starting enemies | 3 |
| Growth per wave | +2 |
| Delay between waves | 3 s |
| Maximum waves | unlimited |
| Formula | `enemies = 3 + (wave - 1) * 2` |

### 7.3 Enemy archetypes

| Type | Spawn wave | Spawn chance | Speed | Detection range | Damage (near / far) |
|---|---|---|---|---|---|
| Bandit | always | remainder | 2.5 | 16 | 10 / 6 |
| Shooter | 3 | 0.25 | 2.2 | 20 | 10 / 7 |
| Rusher | 4 | 0.15 | 3.375 | 20 | 15 / 5 |
| Tactical | 5 | 0.10 | 2.7 | 22 | 10 / 6 |

**Elites:** wave 5, chance 0.15, HP ×2, damage ×1.5, speed ×1.15, XP ×3.

**Wave scaling:** enemy HP `1 + (wave-1) * 0.08` capped ×3; enemy damage
`1 + (wave-1) * 0.05` capped ×2.

> ⚠️ **Damage conflict:** the table above lists authored per-archetype damage
> (10/6, 10/7, 15/5, 10/6), but `UNI-D10` establishes that
> `GunController.Awake()` forces `damage = Mathf.Max(damage, 200f)` for **player**
> weapons. These are different damage paths (enemy output vs player weapon), so
> both stand — but player weapon damage is **not authorable**.

### 7.4 Player (`PlayerController.cs`)

| Parameter | Value |
|---|---|
| Walk speed | 3.0 |
| Run speed | 5.0 |
| Quiet speed | 1.35 |
| HP | 100 |

### 7.5 Group spawning (`EnemySpawner.cs`)

| Parameter | Value |
|---|---|
| Starting group size | 4 |
| Extra group | every 3 waves |
| Min distance from player | 6 |
| Min distance between groups | 4 |

### 7.6 AI perception and noise

| Parameter | Value |
|---|---|
| Vision scan distance | 18 |
| FOV | 30° normal / 80° investigating noise |
| Player body raycast points | 4 (0.35, 0.9, 1.35, 1.7) |
| `NoiseSystem` — Explosion | radius 45, intensity 1.5 |
| `NoiseSystem` — Gunshot | 35 / 1.0 |
| `NoiseSystem` — Sprint | 16 / 0.55 |
| `NoiseSystem` — Impact | 12 / 0.4 |
| `NoiseSystem` — Footstep | 9 / 0.3 |
| `NoiseSystem` — Reload | 8 / 0.2 |
| `NoiseSystem` — Interaction | 6 / 0.15 |
| Propagation | `strength = intensity × (1 − distance/radius)`; walls: >4 walls = fully blocked, otherwise ×0.55 per wall |
| Hearing priority | Explosion 12 · Gunshot 10 · Sprint 6 · Impact 4 · Footstep 3 · Reload 2 · Interaction 1 |

### 7.7 EnemyDirectionIndicator (verified in source)

| Parameter | Value |
|---|---|
| `worldRingRadius` | `0.835f` ✅ source L21 |
| `worldRingThickness` | `0.1f` ✅ source L22 |
| `worldRingHeightOffset` | `0.02f` ✅ source L24 |
| `midRingRadius` | derived: `worldRingRadius − …` (0.785) |
| Arrow length / half-width | 0.30 / 0.094 |
| Arrow colour | `RGBA(1, 0.18, 0.05, 0.95)` URP Unlit transparent |
| Ring mesh | procedural flat annulus, 98 verts / 96 tris |
| Creation | `[RuntimeInitializeOnLoadMethod]`, `DontDestroyOnLoad`, **not serialized in the scene** |
| Ground probe | `RaycastNonAlloc`, start +3, distance 40, preallocated `RaycastHit[8]` |
| Tilt handling | flat → horizontal; slope > 0.1° → `Quaternion.FromToRotation` |
| Rollback | `useWorldRing` / `useWorldArrows` bool switches |

> **Open visual question:** arrows sit at the outer radius (0.835) and read as
> being on the *inner* contour. A visual A/B test is pending to choose inner /
> mid (0.785) / outer. Direction logic was verified correct
> (`dot = 1.000000`, angular error `0.0000°`) — **this is not a direction bug**.

---

## 8. Design gaps — must not be documented as designed-and-working

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
