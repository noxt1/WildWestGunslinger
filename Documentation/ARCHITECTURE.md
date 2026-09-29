# ARCHITECTURE

**Document ID:** `DOC-ARCH-2026-09-29`
**Status:** CANONICAL
**Step 4 of the navigation chain** — see `README.md` §2.

**Scope note.** This describes the architecture **as it actually exists in the
running build**, plus the asset pipeline that feeds it. It does not describe an
intended architecture. Where a component is absent, it is listed as absent.

---

## 1. Runtime shape

```
  ┌──────────────────────────────────────────────────────────┐
  │  Assets/Scenes/TestArena.unity      (canonical scene)    │
  │  294 WWG_* objects · 12 broken refs · 4 missing scripts  │
  └───────────────┬──────────────────────────────────────────┘
                  │  Play mode
                  ▼
  ┌──────────────────────────────────────────────────────────┐
  │  ARENA GENERATION                     RUNTIME VERIFIED   │
  │  9 rooms · 71 walls · 11 floors                            │
  │  builds layout at runtime (not a hand-built static level) │
  └───────────────┬──────────────────────────────────────────┘
                  │
        ┌─────────┴──────────┐
        ▼                    ▼
  ┌──────────────┐   ┌────────────────────────────────────┐
  │ ArenaTactical│   │  COVER / SPAWN REGISTRY             │
  │ Map          │   │  148 cover objects                  │
  │ IsBuilt=true │   │  52 spawn zones                     │
  └──────┬───────┘   │  72 CoverPoint                      │
         │           └────────────────────────────────────┘
         ▼
  ┌──────────────────────────────────────────────────────────┐
  │  ENEMY LAYER                        RUNTIME VERIFIED    │
  │  spawn → FSM (Searching) → vision scan → A* path → move  │
  │  NOT VERIFIED: combat · investigation · sound ·          │
  │                cover-taking · flanking                   │
  └──────────────────────────────────────────────────────────┘

  ┌──────────────────────────────────────────────────────────┐
  │  WEAPON LAYER                       RUNTIME VERIFIED    │
  │  GunController (Shooter) ── fires ✅                      │
  │  GunController (Rusher)  ── fires ✅                      │
  │  damage  ── forced to 200 at runtime  ❌ UNI-D10          │
  └──────────────────────────────────────────────────────────┘
```

---

## 2. Navigation — two systems, only one exists

| System | Status | Role |
|---|---|---|
| **Custom A\*** pathfinder | `RUNTIME VERIFIED` — produces non-null paths, movement observed | **The only working navigation** |
| **NavMesh** | `VERIFIED ABSENCE` — 0 agents, 0 triangulation | Not configured, not baked |

This is the single most important architectural fact about the AI layer: the
build is **A\*-only**. Any design that assumes NavMesh-based cover projection,
agent crowd avoidance, or off-mesh links is **not** currently satisfiable.

---

## 3. Actor representation

| Actor | Representation | Consequence |
|---|---|---|
| Player | `Capsule` | no visible character, no animation |
| Enemies | `Capsule` | no visible character, no animation |
| Rig in scene | 0 `Animator`, 0 `Avatar`, 0 `AnimatorController`, 0 `SkinnedMeshRenderer` | no animation is reachable in game |
| Equipment sockets | none in `Assets/Scripts` | weapon attachment is not rig-based |

Character art exists in Blender and is backed up, but **the scene has no
skinned representation to attach it to**. This is the boundary between
`EXTERNAL SOURCE EXISTS` and `UNITY INTEGRATED` — only the former is true.

---

## 4. Asset pipeline boundary

```
  ~/Documents/WildWestGunslinger art        ~/Desktop/Коллаж тест VVG
  (307 files, external, NOT in repo)        (25 files, external, NOT in repo)
  Kevin Iglesias / WildWest lineage         vest + glove QA renders
  51-bone WWG_Template_Armature             │
  43 .blend · 28 .fbx · 140 raster          │
            │                                │
            └──────────┬─────────────────────┘
                       ▼
        ┌──────────────────────────────┐
        │  VERIFIED BACKUP             │  CLOSED (RISK-01)
        │  332/332 files, SHA256       │  Documentation/Character/
        │  full-manifest + restore test│  CHARACTER_SOURCE_BACKUP.md
        └──────────────┬───────────────┘
                       │
              ╔════════╧════════╗
              ║  THE GAP        ║   ← no promotion has ever occurred
              ╚════════╤════════╝
                       ▼
        ┌──────────────────────────────┐
        │  Assets/  (in repo)          │
        │  Environment/Modular: 6 FBX  │  static exists
        │    WallSegment_01_{FINAL,r2,r3}
        │    FloorSegment_01_{FINAL,r2,r3}
        │  Props/ FBX + fracture assets│  0 instances in scene
        │  Prefabs/Environment/WWG_*   │  UNTRACKED in git
        │  Materials/WWG_*             │  UNTRACKED in git
        └──────────────┬───────────────┘
                       │  import 0.01×, Z-up uncompensated  ❌ UNI-D06
                       │  0 modular meshes in scene       ❌ UNI-D11
                       ▼
        ┌──────────────────────────────┐
        │  Assets/Scenes/TestArena.unity│
        │  but actors are capsules      │
        └──────────────────────────────┘
```

**The pipeline is severed at two points:** the promotion step (never executed)
and the import step (`UNI-D06`).

---

## 5. Asset state machine

```
CREATED → QA PASS → APPROVED → PROMOTED → INTEGRATED → RUNTIME VERIFIED
   ✅          ✅         ❌         ❌          ❌              ❌
```

Everything sits at `QA PASS`. `APPROVED` requires a human approval record
(`ART-0003` G5). A `FINAL` or `APPROVAL_CANDIDATE` **filename is not a state**.

---

## 6. Data integrity faults in the running build

| Fault | Count | Known owner? |
|---|---|---|
| Broken object references (`None` at runtime) | 12 | **No — `ISSUE-01`** |
| Missing Mono Script components | 4 | **No — `ISSUE-02`** |

Both are **unidentified**. A nulled reference or a stripped script can silently
disable an entire gameplay system, and may already explain why five AI
behaviours remain `NOT VERIFIED`. **Enumerating these is the first repair step.**

---

## 7. Build and platform architecture

| Aspect | State |
|---|---|
| Primary target | Windows PC — `RUNTIME VERIFIED` in editor |
| Secondary target | Android — **BLOCKED** (`UNI-D07`) |
| Scripting backend | IL2CPP (evidenced by 2026-09-09 output) |
| Burst | enabled (debug-info artifact exists) |
| Render pipeline | URP inferred from material names — **exact version not pinned** (`ISSUE-05`) |
| CI / tests / build script | **none exist** (`ISSUE-09`) |
| Release signing | not documented |

---

## 8. Version control architecture

| Aspect | State |
|---|---|
| Git initialised | 2026-09-25 — **after** all art work (2026-09-21…26) |
| Branches | `main`, `recovery/checkpoint-2026-09-29-audit2` |
| Tags | `recovery-2026-09-29-audit2` |
| Divergence | `main` **ahead 1 / behind 2** |
| Working tree | 82 pre-existing changes, **0 staged** |
| Clean-clone reproducibility | **NO** — scene depends on untracked prefabs and materials (`RISK-03`) |
| External art in VCS | **NO** — 332 files outside the repo (`RISK-01`, now backed up) |

---

## 9. Documentation architecture

```
  Documentation/README.md          ← entry point
       ├── PROJECT_TRUTH.md        consolidated truth (root level)
       ├── PROJECT_STATE.md        per-system status
       ├── ARCHITECTURE.md         ← this file
       ├── REQUIREMENTS.md         what must be true
       ├── DECISIONS.md            decided / open
       ├── MASTER_PLAN.md          dependency-ordered sequencing
       ├── History/OPEN_ISSUES.md  unresolved items
       └── domains: ART- UNI- GAME- AI- REL- Character- DOC-
```

Precedence is fixed (`DOC-0001` §1): **runtime > file facts > git > backups >
measured art docs > project docs > AI prose > assumptions.**

---

**End of `ARCHITECTURE.md`**
