# DECISIONS

**Document ID:** `DOC-DEC-2026-09-29`
**Status:** CANONICAL
**Step 6 of the navigation chain** — see `README.md` §2.

Two halves: **what has been decided** (binding) and **what has not**
(requires human authority).

---

## 1. Decided — binding

### 1.1 Evidence and governance


> **Current approved decisions are authoritative here.** Historical decisions are preserved in ``Documentation/Archive/`` and do **not** remain active unless explicitly re-adopted here.

| # | Decision | Basis |
|---|---|---|
| GD-01 | Evidence precedence: runtime > file facts > git > backups > measured art docs > project docs > AI prose > assumptions | adopted Phase 2 |
| GD-02 | Asset state machine `CREATED → QA PASS → APPROVED → PROMOTED → INTEGRATED → RUNTIME VERIFIED`, strictly ordered | adopted Phase 2 |
| GD-03 | `ART-`/`UNI-`/`GAME-`/`AI-`/`DOC-`/`REL-`/`HISTORY-` identifier scheme replaces legacy Stage numbers | `DOC-0003` |
| GD-04 | Legacy Stage numbering is **history only** | `HISTORY-0002` |
| GD-05 | A `FINAL` or `APPROVAL_CANDIDATE` **filename is not an approval state** | `ART-0002` §0 |
| GD-06 | Documentation work never mutates product content | Phase 2 constraint |
| GD-07 | Agents never promote an asset past `APPROVED` | `AI-0001` §7 |
| GD-08 | Suspected secrets: metadata only — never read, printed, hashed, copied or committed | `RISK-02` |
| GD-09 | Machine-specific paths only for recovery, external sources, backups, diagnostics | `README.md` §8 |
| GD-10 | Canonical documentation outranks skill files and legacy docs for project state | `AI-0001` §2.2 |

### 1.1 Decisions confirmed by the project owner

| # | Decision | Evidence / source | Date | Status |
|---|---|---|---|---|
| `DEC-07` | **Land the canonical documentation commit on `main` as-is — documentation only, no product files. Defer the `origin/main` divergence (ahead/behind) to a separate planned operation. Do not merge the two GitHub-only Markdown files.** | Explicit project-owner APPROVE, 2026-09-29 | 2026-09-29 | **CONFIRMED** |

**Effect:** unblocks the first consolidation commit. `git pull`, `merge`,
`rebase`, `reset`, `clean`, `stash`, `push` and force operations were **not**
performed. All 74 pre-existing product entries remain uncommitted and unstaged.
After this commit `main` is **ahead 2 / behind 2** — accepted, and the divergence
remains a separate open item.

Full record: `History/OPEN_DECISIONS.md` → `DEC-07`.

### 1.2 Project facts resolved by evidence

| # | Decision | Evidence |
|---|---|---|
| PD-01 | Canonical scene is **`Assets/Scenes/TestArena.unity`** | Build Settings enabled, loaded at runtime, later mtime, tracked, distinct GUID |
| PD-02 | `Assets/TestArena.unity` is a **DUPLICATE — REVIEW, DO NOT DELETE** | 2 differing lines, untracked, never loaded |
| PD-03 | Character basis: **Kevin Iglesias human dummy, size M**, height to crown **1818 mm**, chest **100** | `Doomy_measurements.md` + size-50 table |
| PD-04 | Armature is **51 bones** (`WWG_Template_Armature`) | `Doomy_measurements.md` |
| PD-05 | `Chest=141cm` and `Skeleton=62 bones` are **false** | `CONFLICT-02`, `CONFLICT-03` |
| PD-06 | `WWG_Doomy_Cowboy_REFINED.blend` **exists** | `CONFLICT-01` — Audit 1 search scope error |
| PD-07 | An Android APK **exists** (2026-09-09), but is **stale** — it reflects 2026-09-02 code | `REL-0002` |
| PD-08 | Navigation is **custom A\*-only**; NavMesh is absent | `UNI-D01` |
| PD-09 | Two dated backups exist: 2026-09-22 (`Z:`, 11.87 GB) and 2026-09-29 (verified) | `REL-0001` |
| PD-10 | The vest/glove QA collage contains **25** files (Phase 2 recorded 24) | counted during Phase 2.5 |
| PD-11 | No equipment socket/attach code exists in `Assets/Scripts` | static scan |
| PD-12 | No standalone armature/animation file exists — the 51-bone rig lives **inside** the `.blend` lineage | Phase 2.5 scan |

### 1.3 Design decisions preserved from legacy sources

Full text and rationale: `GAME/GAME-0004-REQUIREMENTS-AND-DESIGN-DECISIONS.md`.

| # | Decision |
|---|---|
| `DD-01` | Quaternius / «Konteri» — temporary enemy model fixed in place |
| `DD-02` | **Human Character Dummy** (Unity Asset Store 178395) is the technical character foundation |
| `DD-03` | Cities and buildings excluded from the western environment scope |
| `DD-04` | Required western prop set: rocks/mountains, cacti, trees/ground, fences, lanterns, wagons, crates, barrels, covers |
| `DD-05` | Environment assembled from **replaceable prefab pools** |
| `DD-06` | Do not mix incompatible low-poly styles |

### 1.4 Security and release decisions

| # | Decision |
|---|---|
| `SR-D01` | Advertising only after core gameplay is stable (currently **not** stable) |
| `SR-D02` | Optional rewarded ads preferred; never interrupt combat; no disruptive frequency |
| `SR-D03` | Advertising SDK init/callbacks isolated from core combat |
| `SR-D04` | Avoid intrusive anti-cheat unnecessary for an offline architecture |
| `SR-D05` | Marketplace submission requires final security sign-off (`REL-0003` 19.9.7) |

---

## 2. Not decided — requires human authority

Full register: `History/OPEN_DECISIONS.md`. **No agent may resolve any of these.**

> **`DEC-07` was confirmed on 2026-09-29** and has moved to §1.1 below. The
> `origin/main` divergence itself remains **deferred**, not resolved.

| # | Open decision | Blocks |
|---|---|---|
| `DEC-01` | Repository material policy | all material repair, scene reproducibility |
| `DEC-02` | **Canonical character foundation file** (10 `Doomy` candidates, 32 `Foundation` variants) | every character asset |
| `DEC-03` | Character source location policy (repo vs. external + schedule) | `RISK-01`/`RISK-03` long-term |
| `DEC-04` | Which `WallSegment`/`FloorSegment` revision is FINAL | environment promotion |
| `DEC-05` | **`CRATE_01` CP1 approval.** State established: CP1 = COMPLETE, **CP2 = NOT AUTHORIZED / AWAITING APPROVAL**. Open: does the user approve CP1? | crate CP2 |
| `DEC-06` | `Cover` r4 / r5 status | cover promotion |
| `DEC-07` | ~~Git divergence resolution~~ **CONFIRMED 2026-09-29** — land documentation commit as-is; **divergence deferred**, GitHub-only MD not merged | closed |
| `DEC-08` | Whether legacy Stage numbering retains any authority | legacy docs |
| `DEC-09` | Disposition of the duplicate scene `Assets/TestArena.unity` | scene hygiene |
| `DEC-10` | Whether untracked `WWG_*` prefabs/materials are committed | clean-clone reproducibility |

---

## 3. Unresolved conflicts

Full register: `History/OPEN_CONFLICTS.md`.

| # | Conflict | Status |
|---|---|---|
| `CONFLICT-01`…`07` | character file, chest, bones, build, backups, scene, APK | **RESOLVED** by evidence |
| `CONFLICT-08` | 148 cover objects vs 72 `CoverPoint` | **OPEN** |
| `CONFLICT-09` | `AI_CONTEXT/` internal consistency | **RESOLVED** (superseded) |
| `CONFLICT-10` | 2 files on `origin/main` absent locally | **OPEN** — needs `DEC-07` |

---

## 4. Risk status

Full register: `History/OPEN_RISKS.md`.

| Risk | Status |
|---|---|
| `RISK-01` character art unprotected | **CLOSED** — 332/332 verified, restore-tested |
| `RISK-02` secret-suspect file | **MITIGATED** — never accessed; ongoing hygiene required |
| `RISK-03` clean clone cannot reproduce the scene | **OPEN** |
| `RISK-04` `Z:` cloud sync unconfirmed | **OPEN** |
| `RISK-05` 82 uncommitted changes | **OPEN** — needs `DEC-07` |
| `RISK-06` unidentified broken refs / missing scripts | **OPEN** — first repair step |
| `RISK-07` unversioned `.blend1` + duplicate FBX sets | **OPEN** |
| `RISK-08` stale APK may be mistaken for working | **MITIGATED** — labelled in `REL-0002` |
| `RISK-09` systematic documentation drift | **MITIGATED** — this documentation set + supersession |

---

**End of `DECISIONS.md`**
