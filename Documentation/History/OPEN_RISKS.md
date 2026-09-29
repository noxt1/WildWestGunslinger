# OPEN RISKS

**Status:** CANONICAL risk register
**Date:** 2026-09-29

Severity: **Critical** = active risk of irreversible loss or a security incident.
**High** = likely loss or a blocked release. **Medium** = managed risk.

---

## RISK-01 — Character art is not covered by the newest verified backup

| Field | Value |
|---|---|
| Severity | **Critical** |
| Detail | The 2026-09-29 verified recovery (873 files, 12/12 SHA256 PASS) contains **no** character art. The character source is 307 files / 180.2 MB in `~/Documents/WildWestGunslinger art`, **unversioned** and **unbacked-up by that snapshot** |
| Older coverage | The 2026-09-22 `Z:` backup includes a **161-file / 96.9 MB** art subset — roughly half — and its cloud sync is **unverified** |
| Consequence | Loss of the `Z:` drive without cloud completion, or loss of the local `Documents` folder, would destroy up to 307 files of art that exist nowhere else |
| Mitigation | Create a full, checksummed art backup of `Documents\WildWestGunslinger art` and `Desktop\Коллаж тест VVG`; confirm `Z:` cloud completion |
| Ref | `REL-0001` §3, `DEC-03` |
| Owner | human |

---

## RISK-02 — Secret-suspect file outside the project

| Field | Value |
|---|---|
| Severity | **Critical** |
| Item | `~/Desktop\omniroute ключи.txt` — 233 B, 2026-09-18 |
| Handling so far | **Contents NOT read and NOT printed.** The file is outside the project and outside both backup scopes. The 2026-09-22 manifest declares `SECRETS: EXCLUDED` |
| Risk | Accidental commit to Git, inclusion in a backup, or leakage into a `Documentation/` artifact |
| Rules | **Never** read, commit, copy into the repository, or quote in any document. If credentials are ever needed for the project, source them from a proper secret store |
| Ref | `REL-0001` §5 |
| Owner | human |

---

## RISK-03 — A clean clone cannot reproduce the playable scene

| Field | Value |
|---|---|
| Severity | **High** |
| Detail | The canonical scene depends on **untracked** `Assets/Prefabs/Environment/WWG_*.prefab` and `Assets/Materials/WWG_*.mat`, plus an untracked duplicate scene. Git was initialised on 2026-09-25, after the art work |
| Consequence | Anyone cloning the repository gets a scene with missing references |
| Mitigation | Resolve `DEC-10`, then commit deliberately |
| Owner | human |

---

## RISK-04 — The `Z:` backup may not exist in the cloud

| Field | Value |
|---|---|
| Severity | **High** |
| Detail | The 2026-09-22 manifest states `GOOGLE DRIVE SYNC STATUS: UNKNOWN` — "DriveFS active, cloud upload completion not verified — **do not treat as cloud-confirmed**" |
| Consequence | A local `Z:` cache loss would destroy the only backup that covers character art |
| Mitigation | Verify DriveFS sync status explicitly; treat the backup as local-only until confirmed |
| Owner | human |

---

## RISK-05 — 82 uncommitted changes with no safety net

| Field | Value |
|---|---|
| Severity | **High** |
| Detail | 15 modified, 2 deleted, 65 untracked. `main` is ahead 1 / behind 2. The recovery commit `7ad314c` contains **only** the three reports — none of the 82 changes |
| Consequence | A failed merge, reset, or clean would lose substantial uncommitted work |
| Mitigation | Resolve `DEC-07` deliberately; consider an interim branch commit before any history operation |
| Owner | human |

---

## RISK-06 — Missing scripts and broken references may silently disable systems

| Field | Value |
|---|---|
| Severity | **High** |
| Detail | 4 missing Mono Scripts and 12 broken references are **unidentified**. A nulled reference can disable a whole gameplay system without any error |
| Consequence | Some of the five "untested" AI behaviours may already be non-functional for this reason |
| Mitigation | Enumerate `UNI-D04` and `UNI-D05` first, before any other repair (`UNI-0003` §repair ordering) |
| Owner | human |

---

## RISK-07 — Unversioned `.blend1` and duplicate FBX sets

| Field | Value |
|---|---|
| Severity | **Medium** |
| Detail | 24 `.blend1` files interleaved with sources; `FBX/` and `Unity_Export/` are byte-identical duplicates; 32 `WWG_Foundation_Source` variants with no canonical selection |
| Consequence | It is impossible to tell which file is current; wrong-file export is likely |
| Mitigation | Resolve `DEC-02`; separate sources from backups |
| Ref | `ART-0003` §6 P2, P3, P4 |
| Owner | human |

---

## RISK-08 — Stale build artifact may be mistaken for a working build

| Field | Value |
|---|---|
| Severity | **Medium** |
| Detail | `WildWest.apk` (46.2 MB, 2026-09-09) reflects code from 2026-09-02 — before the entire character programme and the current scene |
| Consequence | It may be shipped or treated as proof that the project builds or that Android works (it does not — `UNI-D07`) |
| Mitigation | Label as historical; rebuild from a committed state; record the new commit in `REL-0002` |
| Owner | human |

---

## RISK-09 — Documentation drift is systematic

| Field | Value |
|---|---|
| Severity | **Medium** |
| Detail | All three audits produced false claims; six corrections were needed in Phase 2 alone. Legacy Stage docs assert completed work that does not exist in the build |
| Consequence | Planning based on legacy documents will produce wrong work |
| Mitigation | The `Documentation/` tree, evidence classes (`DOC-0001` §1), and the `SUPERSEDED` headers |
| Owner | ongoing |

---

## Cross-references

- `REL-0001` — recovery and backup
- `OPEN_DECISIONS.md` — decisions
- `UNI-0003` — defects

---

**End of `OPEN_RISKS.md`**
