# OPEN ISSUES

**Status:** CANONICAL register of open defects, missing artefacts, and gaps
**Date:** 2026-09-29

Unity gameplay/engine defects live in `UNI-0003` with severities. This file
tracks **missing artefacts, unidentified defects, and documentation gaps**.

---

## Runtime issues (audit refs preserved)

The U-12 runtime audit numbered its findings `RT-01`…`RT-09`. Those identifiers
are **preserved** here and mapped to the canonical `UNI-D*` register. None is
closed. `RT-*` items must never be silently dropped when editing
`UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md`.

| Audit ref | Issue | Canonical | Status |
|---|---|---|---|
| **`RT-01`** | Modular FBX import at 0.01×, Z-up uncompensated (4 m wall → 4 cm tile) | `UNI-D06` | **OPEN** — blocks all environment promotion |
| **`RT-02`** | `MobileTouchControls` all 3 refs null — inert on PC, **activates on Android** | `UNI-D07` | **OPEN** — Android blocker |
| `RT-03` | `FireButton`/`GunController` duplicate; `weaponPoint` → Player root; damage 10→200 | `UNI-D10` (partial) | **OPEN** — the duplicate/`weaponPoint` element has no dedicated entry |
| **`RT-04`** | `GunController.Awake` forces `damage ≥ 200`, discarding Inspector values | `UNI-D10` | **OPEN** — code confirmed at `GunController.cs:37` |
| **`RT-05`** | `Rusher.prefab` = `FrameDebuggerRenderTargetDisplay`; `Shooter.prefab` = `TMP_SDF-HDRP LIT` | `UNI-D08`, `UNI-D09` | **OPEN** |
| `RT-06` | 9 missing-script warnings per spawn cycle; scales with enemy count | `UNI-D05` | **OPEN** |
| `RT-07` | `HUDController.xpBar` / `levelText` null — level never displayed | **`UNI-D13`** | **OPEN** — added in Phase 3 |
| `RT-08` | 12 broken references, all runtime-confirmed | `UNI-D04` | **OPEN** — unidentified |
| `RT-09` | 4097 renderers / 1552 objects at runtime — Android perf unproven | **`UNI-D14`** | **OPEN** — added in Phase 3 |

> **Phase 3 gap closed.** `RT-07` and `RT-09` were absent from the Phase 2
> register. They are now `UNI-D13` and `UNI-D14`. All nine audit refs are mapped.

### AI runtime not verified

| Behaviour | Status |
|---|---|
| AI combat | **NOT VERIFIED** |
| AI investigation | **NOT VERIFIED** |
| AI sound propagation / reaction | **NOT VERIFIED** |
| AI cover-taking | **NOT VERIFIED** |
| AI flanking | **NOT VERIFIED** |

`UNI-D04`/`UNI-D05` may already explain this — see `ISSUE-01`/`ISSUE-02`.

### Absent systems that must stay listed as open

| Item | Status |
|---|---|
| Character Foundation in Unity | **ABSENT** — `UNI-D02` |
| Missing Animator / Avatar / Controller | **ABSENT** — `UNI-D02` |
| Modular environment integration | **ABSENT** — `UNI-D11`, 0 meshes |
| NavMesh | **ABSENT** — `UNI-D01`, 0 agents / 0 triangulation |

### Release security items

| Item | Status |
|---|---|
| `REL-0003` §19.1.3 — search Unity project **and Git history** for secrets | **PARTIAL** — working tree only (0 findings / 731 files); **Git history not swept** |
| `REL-0003` §19.1.4 — check the final APK/AAB for development credentials | **NOT STARTED** — no current APK exists |

---

## Unidentified defects — must be enumerated first

### ISSUE-01 — 12 broken object references, unidentified
- **Severity:** Critical
- **Known:** 12 references resolve to `None` at runtime
- **Unknown:** which GameObjects, which fields, which systems they disable
- **Why first:** a nulled reference can silently disable a whole system and may explain several "untested" AI behaviours
- **Ref:** `UNI-D04`

### ISSUE-02 — 4 missing Mono Script references, unidentified
- **Severity:** Critical
- **Known:** 4 components show `Missing (Mono Script)`
- **Unknown:** the owning GameObjects and the lost logic
- **Approach:** identify by script GUID against the repository before any repair
- **Ref:** `UNI-D05`

---

## Missing artefacts

### ISSUE-03 — `Вставленная уценка.md` not found
- **Severity:** Low
- Referenced as a project document; absent from `Downloads` and everywhere searched
- Either it was never created, or it lives somewhere not yet searched

### ISSUE-04 — Vest reference source images missing
- **Severity:** Medium
- `Desktop\Коллаж тест VVG\00_Vest_Reference\REFERENCE_INDEX.txt` catalogues **14 reference images** and states the originals **could not be extracted** (no tool access to chat attachments)
- Only the index survives. Provenance for those 14 items is descriptive only
- **Note:** this does not affect the vest itself, whose measurements are recorded in `Doomy_vest_measurements.md`

### ISSUE-05 — Unity version not pinned
- **Severity:** Medium
- The exact Unity 6 version is not recorded in any project document
- Should be read from `ProjectSettings/ProjectVersion.txt` and pinned, because package resolution depends on it

### ISSUE-06 — Character FBX not present in `Assets/`
- **Severity:** High
- Five character FBX and four weapon FBX exist in the art source (`Documents\…`)
- No integrated character FBX is present in `Assets/`, and the scene has 0 `SkinnedMeshRenderer`
- This is expected given `UNI-D02`, but means the promotion target is undefined

### ISSUE-07 — Package manifest not captured
- **Severity:** Low
- The audit reports contain no package inventory. URP is inferred from the material name `TMP_SDF-HDRP LIT`
- Capture `Packages/manifest.json` into `UNI-0004`

### ISSUE-08 — No authoritative game design document
- **Severity:** Medium
- Design intent is scattered across `CHATGPT_*`, `ART_PIPELINE.md`, `AI_CONTEXT/*`
- `GAME-0001` is the first consolidated statement, built from runtime evidence rather than design intent
- True design intent still needs to be authored by a human

### ISSUE-09 — No CI, no tests, no build pipeline
- **Severity:** High
- Nothing automated exists: no CI, no test suite, no build script, no release config
- Every defect in `UNI-0003` must therefore be found and verified manually

### ISSUE-10 — Audit 1 has no artefact on disk
- **Severity:** Low
- Audit 1 exists only in conversation history. Its findings survive only through this documentation set and `HISTORY-0003`
- No report file was ever written

---

## Evidence gaps

### ISSUE-11 — Five AI behaviours untested
Combat, investigation, sound propagation, cover-taking, flanking.
**Status:** must not be reported as working. See `GAME-0002` §3

### ISSUE-12 — 146 of 307 art files unaccounted for in the 2026-09-22 backup
- The `Z:` art copy holds 161 files; the live source holds 307
- The manifest does not record the exclusion criteria
- **Ref:** `CONFLICT-10` / `RISK-01`

---

## Cross-references

- `UNI-0003` — Unity defects with severity
- `OPEN_DECISIONS.md` — decisions
- `OPEN_RISKS.md` — risks
- `OPEN_CONFLICTS.md` — source conflicts

---

**End of `OPEN_ISSUES.md`**
