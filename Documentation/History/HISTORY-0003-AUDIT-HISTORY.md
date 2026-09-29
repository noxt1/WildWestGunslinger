# HISTORY-0003 — Audit History

**Document ID:** `HISTORY-0003`
**Status:** CANONICAL
**Date:** 2026-09-29

Three audits were performed before Phase 2. All three searched **only inside the
project directory**, which is why each has a characteristic blind spot.

---

## 1. Audit 1 — HARD RECON

| Field | Value |
|---|---|
| Date | 2026-09-29 |
| Type | Static reconnaissance |
| Artefact | **Conversation history only — no file was ever written** |
| Scope | Project directory only |

### Principal finding, now corrected

Audit 1 reported `WWG_Doomy_Cowboy_REFINED.blend` as **NOT FOUND**.

> **Correction:** the file **exists** at
> `~/Documents/WildWestGunslinger art\art\Characters\Kevin Iglesias\WildWest\WWG_Doomy_Cowboy\WWG_Doomy_Cowboy_REFINED.blend`
> (2 360 988 B, 2026-09-22 16:16).
>
> **Root cause:** the search was scoped to the project root. The entire
> character programme lives outside it.

**Lesson:** asset searches must include every known art root, not only the
repository.

---

## 2. Audit 2 — static reconciliation

| Field | Value |
|---|---|
| Date | 2026-09-29 |
| Type | Documentation vs. repository reconciliation |
| Artefact | `AUDIT_2_RECONCILIATION_REPORT.md` (committed `7ad314c`) |

### Incorrect figures carried forward

| Audit 2 claim | Canonical value | Source |
|---|---|---|
| `Chest = 141 cm` | **`100`** (size-50 table) | `Doomy_measurements.md` |
| `Skeleton = 62 bones` | **`51 bones`** (`WWG_Template_Armature`) | `Doomy_measurements.md` |

Because Audit 2's "foundation identified" conclusion rested on those two
numbers, **that conclusion must be re-derived**. The corrected data is in
`ART-0001` §4.1.

The report itself is **retained as audit evidence**; only these figures are
corrected (`DOC-0002` S-11).

### Blind spot

Audit 2 described the backup picture as the 2026-09-29 snapshot only. It did not
know about the 2026-09-22 `Z:` backup (11.87 GB).

---

## 3. U-12 — runtime audit

| Field | Value |
|---|---|
| Date | 2026-09-29 |
| Type | Play-mode verification |
| Artefact | `AUDIT_2_RUNTIME_REPORT.md` (committed `7ad314c`) |
| Authority | **Highest** — this is runtime evidence |

### Confirmed working
Arena generation (9 rooms / 71 walls / 11 floors), `ArenaTacticalMap.IsBuilt`,
148 cover objects, 52 spawn zones, 72 `CoverPoint`, enemy spawning, FSM
`Searching`, vision scanning, custom A\* navigation, weapon firing on both
prefabs.

### Confirmed broken
12 broken references, 4 missing Mono Scripts, 0 NavMesh, 0 Animator/Avatar/
Controller/SkinnedMeshRenderer, Capsules for player and enemies, modular FBX at
0.01× with uncompensated Z-up, `TMP_SDF-HDRP LIT` on `Shooter.prefab`,
`FrameDebuggerRenderTargetDisplay` on `Rusher.prefab`, damage forced to 200,
`MobileTouchControls` fully null, 0 destructibles, 0 modular meshes.

### Not tested
AI combat, investigation, sound, cover-taking, flanking.

**U-12 is the authoritative source for all runtime statements in this
documentation set.**

---

## 4. Phase 2 — consolidation

| Field | Value |
|---|---|
| Date | 2026-09-29 |
| Artefacts | `ART_DELTA_AFTER_RECOVERY.md`, `Documentation/Archive/Historical/RECONCILIATION_SOURCE_INVENTORY.md`, `Documentation/**`, `CONSOLIDATION_VALIDATION_REPORT.md` |

### What Phase 2 corrected

1. **Art delta = ZERO** after the recovery point (`ART_DELTA_AFTER_RECOVERY.md`).
2. **Canonical scene resolved** to `Assets/Scenes/TestArena.unity` on build +
   runtime + mtime + GUID evidence.
3. **Five new source families found** outside the project:
   `Documents\WildWestGunslinger art` (307 files), `Desktop\Коллаж тест VVG`
   (24), `Z:\Мой диск\WWG_RECOVERY` (90 626 files), the 2026-09-09 APK/IL2CPP
   build, and `Desktop\jvyb` VPN docs (out of scope).
4. **Six prior claims corrected** — see `Documentation/Archive/Historical/RECONCILIATION_SOURCE_INVENTORY.md` §13.
5. **One security risk flagged:** `Desktop\omniroute ключи.txt` — not read,
   not printed, must never enter the repository.

---

## 5. Audit-independent conclusions

| # | Conclusion | Evidence class |
|---|---|---|
| 1 | No art asset is above `QA PASS` | runtime + file |
| 2 | The build is not releasable — 7 critical defects open | runtime |
| 3 | The newest verified backup does **not** cover character art | backup manifest |
| 4 | A clean clone cannot reproduce the playable scene | git + file |
| 5 | Documentation drift is systematic, not incidental | all three audits |
| 6 | Measurement-first art practice is the project's real strength | `Doomy_*` docs |

---

## 6. Cross-references

- `Documentation/Archive/Historical/RECONCILIATION_SOURCE_INVENTORY.md` — §13 correction table
- `HISTORY-0001` — timeline
- `UNI-0003` — defects
- `AI-0001` §6 — AI error corrections

---

**End of `HISTORY-0003-AUDIT-HISTORY.md`**
