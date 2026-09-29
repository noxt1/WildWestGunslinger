# DOC-0001 — Canonical Documentation Index

**Document ID:** `DOC-0001`
**Status:** CANONICAL — entry point for all WildWestGunslinger documentation
**Date:** 2026-09-29
**Supersedes:** `ART_PIPELINE.md`, `AI_TOOLCHAIN_AUDIT.md`, `AI_PRODUCTION_METHODOLOGY.md`, `AI_CONTEXT/*`, `CHATGPT_*.md` (as canonical sources)

---

## 1. How to use this documentation set

This directory is the **single canonical entry point** for the WildWestGunslinger
project. Read `DOC-0001` (this file) first. It points to everything else.

**Evidence precedence — highest first:**

| Rank | Evidence class | Example |
|---|---|---|
| 1 | Runtime observation in Unity | live play-mode measurements |
| 2 | Verified file/asset facts on disk | GUIDs, sizes, hashes, timestamps |
| 3 | Git history | commits, branches, tags, blobs |
| 4 | Backup manifests | `WWG_RECOVERY_INFO.txt`, `RECOVERY_POINT_REPORT.md` |
| 5 | Measured art documents | `Doomy_measurements.md`, QA renders |
| 6 | Project documentation | this `Documentation/` tree |
| 7 | ChatGPT / agent prose | `CHATGPT_*.md`, skill files — **advisory only** |
| 8 | Assumptions | **must be labelled as such** |

When two sources conflict, the higher rank wins and the conflict is recorded in
`History/OPEN_CONFLICTS.md`.

---

## 2. Canonical document map

### 2.1 Governance

| ID | Document | Purpose |
|---|---|---|
| `DOC-0001` | `DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md` | **This file.** Entry point and precedence rules |
| `DOC-0002` | `DOC-0002-SUPERSEDED-DOCUMENTS.md` | Which legacy docs are superseded, by what |
| `DOC-0003` | `DOC-0003-IDENTIFIER-SYSTEM.md` | `ART-`/`UNI-`/`GAME-`/`AI-`/`DOC-`/`REL-` scheme |
| `DOC-0004` | `DOC-0004-OPEN-QUESTIONS.md` | Unresolved questions requiring a human decision |

### 2.2 Art — `Documentation/ART/`

| ID | Document | Purpose |
|---|---|---|
| `ART-0001` | `ART-0001-ART-STATE.md` | Art programme state, source-of-truth locations |
| `ART-0002` | `ART-0002-ART-ASSET-REGISTER.md` | Per-asset register with state machine |
| `ART-0003` | `ART-0003-ART-PIPELINE-AND-QA-GATES.md` | Blender → FBX → Unity pipeline and QA gates |

### 2.3 Unity — `Documentation/UNI/`

| ID | Document | Purpose |
|---|---|---|
| `UNI-0001` | `UNI-0001-UNITY-PROJECT-STATE.md` | Scene, rendering, packages, open state |
| `UNI-0002` | `UNI-0002-UNITY-DATA-AND-ASSET-REGISTER.md` | Scenes, prefabs, materials, scripts inventory |
| `UNI-0003` | `UNI-0003-UNITY-KNOWN-DEFECTS.md` | Runtime-confirmed defects with severity |
| `UNI-0004` | `UNI-0004-UNITY-ENVIRONMENT-AND-TOOLCHAIN.md` | Editor, platform targets, build status |

### 2.4 Game design — `Documentation/GAME/`

| ID | Document | Purpose |
|---|---|---|
| `GAME-0001` | `GAME-0001-GAME-DESIGN-STATE.md` | Genre, pillars, arena, current design truth |
| `GAME-0002` | `GAME-0002-GAMEPLAY-SYSTEMS-STATE.md` | Player, enemies, combat, cover, FSM |
| `GAME-0003` | `GAME-0003-CONTROLS-AND-PLATFORM-TARGETS.md` | Input schemes and platform intent |

### 2.5 AI / tooling — `Documentation/AI/`

| ID | Document | Purpose |
|---|---|---|
| `AI-0001` | `AI-0001-AI-AGENT-STATE.md` | Which agents/skills are configured and trustworthy |
| `AI-0002` | `AI-0002-AI-TOOLING-AND-PRODUCTION-METHOD.md` | Production methodology, reconciled |

### 2.6 Release / infrastructure — `Documentation/REL/`

| ID | Document | Purpose |
|---|---|---|
| `REL-0001` | `REL-0001-RECOVERY-AND-BACKUP.md` | All recovery points, verified and unverified |
| `REL-0002` | `REL-0002-BUILD-ARTIFACTS.md` | Build outputs, what they are, what they are not |

### 2.7 History and open items — `Documentation/History/`

| ID | Document | Purpose |
|---|---|---|
| `HISTORY-0001` | `HISTORY-0001-TIMELINE.md` | Dated project timeline |
| `HISTORY-0002` | `HISTORY-0002-LEGACY-STAGE-NUMBERING.md` | Legacy Stage N → canonical IDs |
| `HISTORY-0003` | `HISTORY-0003-AUDIT-HISTORY.md` | Audit 1, Audit 2, U-12 and their corrections |
| `OPEN_DECISIONS` | `OPEN_DECISIONS.md` | Decisions that must be made by a human |
| `OPEN_RISKS` | `OPEN_RISKS.md` | Active risks |
| `OPEN_CONFLICTS` | `OPEN_CONFLICTS.md` | Unresolved source conflicts |
| `OPEN_ISSUES` | `OPEN_ISSUES.md` | Open defects and missing artefacts |

---

## 3. Companion root-level evidence documents

These are **evidence, not governance**, and remain at the repository root:

| File | Role |
|---|---|
| `ART_DELTA_AFTER_RECOVERY.md` | Zero art delta proof after the 2026-09-29 recovery point |
| `Documentation/Archive/Historical/RECONCILIATION_SOURCE_INVENTORY.md` | Every documentation/art/backup source discovered |
| `RECOVERY_POINT_REPORT.md` | 2026-09-29 recovery construction and verification |
| `AUDIT_2_RECONCILIATION_REPORT.md` | Audit 2 static reconciliation |
| `AUDIT_2_RUNTIME_REPORT.md` | U-12 runtime audit evidence |
| `Working/reports/*.md` | 10 Blender QA reports |

---

## 4. Asset state machine

Asset states are **strictly ordered** and must never be mixed or skipped:

```
CREATED → QA PASS → APPROVED → PROMOTED → INTEGRATED → RUNTIME VERIFIED
```

| State | Meaning | Evidence required |
|---|---|---|
| `CREATED` | File exists in a source folder | File exists |
| `QA PASS` | QA renders produced and reviewed | QA report + renders |
| `APPROVED` | A human approved it | Explicit approval record |
| `PROMOTED` | Copied into the promotion target | Promotion record |
| `INTEGRATED` | Wired into a Unity scene/prefab | Scene or prefab reference |
| `RUNTIME VERIFIED` | Confirmed working in play mode | Runtime observation |

> **Current project-wide truth:** no asset has reached `RUNTIME VERIFIED`.
> **Two assets are APPROVED** (BARREL_01, FENCE_01 - recorded user confirmation).
> One is at QA PASS awaiting approval (CRATE_01; CP2 **not authorized**).
> **No asset is PROMOTED, INTEGRATED, or RUNTIME VERIFIED.**

---

## 5. Current project truth in one paragraph

WildWestGunslinger is an **unreleased PC/Android third-person western arena
shooter** in active pre-alpha development. The playable arena scene is
`Assets/Scenes/TestArena.unity`. Runtime play-mode auditing confirms the arena
generator runs and builds its layout at runtime (9 rooms, 71 walls, 148 cover objects, 52 spawn zones), enemies
spawn and navigate via a custom A* pathfinder, and both weapon prefabs fire — but
the build is not shippable: 12 broken object references, 4 missing Mono scripts,
zero NavMesh, zero character rig or animation (all actors are capsules), broken
FBX import scale, incorrect materials on two enemy prefabs, a 10× gun-damage
overwrite bug, and no Android touch controls. Art exists in three separate
unversioned locations, none of which is designated canonical. Git history begins
2026-09-25; two full backups exist (2026-09-22 on `Z:`, 2026-09-29 in
`WildWestGunslinger_RECOVERY`). Six human decisions are required before any
promotion or release work can begin.

---

## 6. Reading order for a new contributor

1. `DOC-0001` (this file)
2. `DOC-0004-OPEN-QUESTIONS.md` — what is not decided
3. `UNI-0001-UNITY-PROJECT-STATE.md` — what actually runs
4. `UNI-0003-UNITY-KNOWN-DEFECTS.md` — what is broken
5. `ART-0001-ART-STATE.md` — where the art lives
6. `REL-0001-RECOVERY-AND-BACKUP.md` — how to not lose work
7. `GAME-0001-GAME-DESIGN-STATE.md` — what the game is meant to be

---

## 7. Maintenance rules

1. Every new canonical document gets an ID from `DOC-0003-IDENTIFIER-SYSTEM.md`.
2. Every claim in a canonical document must cite its evidence class (section 1).
3. Every conflict between sources is recorded in `History/OPEN_CONFLICTS.md`.
4. Superseding a document requires updating `DOC-0002` in the same change.
5. **Documentation changes never silently alter assets.** Art, Unity content and
   character work are documented here, not modified by documentation work.

---

**End of `DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md`**
