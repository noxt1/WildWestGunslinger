# RECONCILIATION SOURCE INVENTORY

**Document ID:** `DOC-INV-2026-09-29`
**Phase:** 2 — Documentation Consolidation
**Status:** FINAL
**Date:** 2026-09-29

---

## 1. Purpose

Every documentation, art, build, and backup source that carries information about
WildWestGunslinger, wherever it physically lives — inside the project, in
`Downloads`, on `Desktop`, in `Documents`, on network drive `Z:`, or on GitHub.

This inventory exists because the three prior audits (Audit 1, Audit 2, U-12
runtime) **searched only inside the project directory** and therefore missed
entire source families. Several audit conclusions are corrected here.

---

## 2. In-Project Documentation

| File | Size | Status | Canonical role |
|---|---|---|---|
| `ART_PIPELINE.md` | ~40 KB | Legacy, conflicting | SUPERSEDED → `Documentation/ART-*` |
| `AI_TOOLCHAIN_AUDIT.md` | ~39 KB | Legacy, partly wrong | SUPERSEDED → `Documentation/AI-*` |
| `AI_PRODUCTION_METHODOLOGY.md` | 30 625 B | **GitHub-only** | Reconciled → `Documentation/AI-*` |
| `AUDIT_SESSION_CONTEXT_2026-09-28.md` | 24 322 B | **GitHub-only** | Reconciled → `Documentation/History/*` |
| `AUDIT_2_RECONCILIATION_REPORT.md` | — | Committed `7ad314c` | Evidence, retained |
| `AUDIT_2_RUNTIME_REPORT.md` | — | Committed `7ad314c` | Evidence, retained |
| `RECOVERY_POINT_REPORT.md` | — | Committed `7ad314c` | Evidence, retained |
| `Working/reports/*.md` (10 files) | — | Tracked | QA evidence, retained |
| `AI_CONTEXT/` (12 files) | — | Many modified/conflicted | SUPERSEDED → `Documentation/*` |

### Git status at consolidation time

- branch `main`, HEAD `7ad314c869e5dad1daf8e56fdf0b8f4197592969`
- `origin/main` = `f932fcc722d22b8150ccf7e9616914324347c6e8`
- **ahead 1 / behind 2**
- working tree: **82 entries = 15 modified + 2 deleted + 65 untracked**, 0 staged

---

## 3. ChatGPT-Generated Documents (`~/Downloads`)

| File | Notes |
|---|---|
| `CHATGPT_CHECKLIST.md` | Roadmap/checklist, stale stage numbering |
| `CHATGPT_CHECKLIST_before_STAGE19_2026-09-20.md` | Historical pre-Stage-19 |
| `CHATGPT_PROJECT_STATE.md` | GPT state claims, not runtime-verified |
| `CHATGPT_WORK_QUEUE.md` | GPT queue, not authoritative |
| `Вставленная уценка.md` | **Referenced but ABSENT** — open issue |

**Rule applied:** GPT claims are advisory only. They are never promoted to
"current implementation" without file or runtime evidence.

---

## 4. OpenCode / Project Skills

| Path | Status |
|---|---|
| `.opencode/skills/wwg-character-art/SKILL.md` | Partly valid QA gates; contains **nonexistent current rig claims** |
| `~/.config/opencode\skill-libraries\` (many skills) | Generic external library, not project-specific |

---

## 5. Character Art Source — `~/Documents/WildWestGunslinger art`

**CORRECTION TO AUDIT 1.** The project-level character source is real and lives
outside the project directory.

| Metric | Value |
|---|---|
| Path | `~/Documents/WildWestGunslinger art\art\Characters\Kevin Iglesias\WildWest` |
| Total files | 307 |
| Total size | 180.2 MB |
| Newest write | 2026-09-26 |

### Composition

| Category | Files |
|---|---|
| `.blend` (character + variants) | 43 |
| `.blend1` backups | 24 |
| `.fbx` (Unity export) | 28 |
| `.png` / `.jpg` | 153 |
| `.prefab` | 14 |
| `.mat` | 7 |
| `.md` | 5 |
| `.unity` | 1 |
| `.zip` | 1 |

### `WWG_Doomy_Cowboy` — CONFIRMED PRESENT

Audit 1 reported `WWG_Doomy_Cowboy_REFINED.blend` as **NOT FOUND**. This was a
**search-scope error**: the file exists at
`...\Kevin Iglesias\WildWest\WWG_Doomy_Cowboy\WWG_Doomy_Cowboy_REFINED.blend`
(2 360 988 B, 2026-09-22 16:16).

| File | Size | Date | Note |
|---|---|---|---|
| `WWG_Doomy_Cowboy_REFINED.blend` | 2 360 988 | 2026-09-22 16:16 | **Audit 1 target — EXISTS** |
| `WWG_Doomy_Cowboy_APPROVAL_CANDIDATE.blend` | 2 309 505 | 2026-09-22 15:48 | Approval candidate |
| `WWG_Doomy_Cowboy_VEST_APPROVAL_CANDIDATE.blend` | 2 364 062 | 2026-09-22 17:12 | Vest candidate |
| `WWG_Doomy_Cowboy_CLOTHING_REBUILD_SB_WORK.blend` | 2 179 296 | 2026-09-26 12:02 | Latest work file |
| `WWG_Doomy_Cowboy_FINAL_REPAIR_SB_WORK.blend` | 2 568 092 | 2026-09-26 00:33 | Repair |
| `WWG_Doomy_Cowboy_FINAL_REPAIR_SB_WORK_PRESAVE_20260926_053821.blend` | 16 196 184 | 2026-09-26 05:38 | Presave |
| `WWG_Doomy_Cowboy_VEST_FULL_REPAIR_SB_WORK.blend` | 2 567 279 | 2026-09-26 00:10 | Vest repair |
| `WWG_Doomy_Cowboy_VEST_MARK1_REPAIR_SB_WORK.blend` | 2 552 003 | 2026-09-25 23:33 | Mark repair |
| `WWG_Doomy_Cowboy_VEST_WORK.blend` | 2 551 057 | 2026-09-25 12:05 | Vest work |
| `WWG_Doomy_Cowboy_WORK.blend` | 1 704 329 | 2026-09-21 17:00 | Base work |

Plus `QA/` (**~120 PNG renders**, checkpoints `CK1`–`CK18`, `R_*`, `SHIRT_*`) and
`_REFERENCES/` (**38 reference images** + `SOURCES.md`).

### Character measurement documents (canonical body data)

| File | Content |
|---|---|
| `Doomy_measurements.md` | Canonical Kevin Iglesias dummy, M size. Height to crown **1818 mm**; torso Z 926–1579; legs Z 119–1023; head Z 1537–1818. Axes: X width, Y depth (front − / back +), Z up. Built from `WWG_Zone_*` templates + `WWG_Template_Armature` (**51 bones**) in the `WWG_Foundation_Source.blend` lineage. |
| `Doomy_clearance_profile.md` | Controlled garment clearances (mm) with measured facts: hat interior radial 15–20 / top 30–35 (actual 16/17/32); shirt torso 8–12 (actual min Torso↔Shirt **1.2**); sleeves 6–12 (actual Hands↔Shirt 4.3); pants 8–14 (actual Legs↔Pants 3.2); boots 10–18 (actual Feet↔Boots 5.9); belt 12–20 (actual Shirt↔Strap min 25.5); holster 5–10 (actual 8.5). |
| `Doomy_vest_measurements.md` | Vest measured over SHIRT surface (body untouched / dead-zone). Chest table by z-slice, e.g. z 1.42 → width 435.1, depth 262.2, circumference ≈1112. |
| `Doomy_vest_clearance_profile.md` | Vest clearance profile. |
| `_REFERENCES/SOURCES.md` | 39 public-domain/CC0/CC-BY reference images (MET Costume Institute, Nairn, Vincent, Grabill, XIT, Auckland, Tony Lama, Boss of the Plains…). **Reference only — never copied into geometry or texture, never transferred to Unity.** |

### `WWG_Foundation_Source` lineage (32 `.blend` files)

Named variants span 2026-09-21 → 2026-09-22:
`_baseline`, `_baseline_DesignAssets_20260922`, `_baseline_FinalCorrective_20260922`,
`_baseline_FiveChar_20260921`, `_baseline_PatternTest_20260922`, `_baseline_PlayerPass_20260921`,
`_baseline_Polish_20260921`, `_baseline_VestAssembly_20260922`, `_baseline_VestNEW2_20260922`,
`_DesignAssets`, `_FiveChar`, `_PlayerPass1`, `_PlayerPass2`, `_PlayerPass2_protected`,
`_PlayerVestRepair`, `_Polish`, `_VestAssembly`, `_VestClean`, `_VestOpusRepair`,
`_VestOpusRepair_V2`, `_VestOpusRepair_V3`, `_VestPatternPatch`, and base
`WWG_Foundation_Source.blend`.

**No single one of these is designated canonical.** This is an **OPEN DECISION**
— see `Documentation/History/OPEN_DECISIONS.md`.

### Unity export sets

- `FBX/`: `WWG_Player`, `WWG_Bandit`, `WWG_Rusher`, `WWG_Shooter`, `WWG_Tactical`
  + weapons `WWG_Carbine`, `WWG_Revolver`, `WWG_Shotgun`, `WWG_Winchester`
- `Unity_Export/Characters/` and `Unity_Export/Weapons/`: byte-identical duplicates
  of `FBX/` for the same assets.

---

## 6. Vest/Glove QA Collage — `~/Desktop/Коллаж тест VVG`

**Not present in any project documentation.** 25 files, ~13.5 MB, 2026-09-23 → 2026-09-26.

| Item | Detail |
|---|---|
| `00_Vest_Reference/REFERENCE_INDEX.txt` | Catalogue of 14 dialogue-context reference images used for the vest rebuild. Notes that original source PNGs **could not be extracted** (no tool access to chat attachments) — only the index exists. |
| `01_Vest/` | 15+ QA renders: `Vest_VisualQA_2x2`, `Vest_TechnicalQA_REVISION_2x2`, `Vest_ShirtFit_*`, `Vest_FinalCorrection_*`, `Vest_UpperShoulder_TopQA_2x2` |
| `01_Vest/Gloves_Repair/` | `Gloves_MarkFix_2x2`, `Gloves_Repair_2x2` |
| `01_Vest/Vest_Repair/` | `Vest_MarkFix_2x2`, `Vest_Repair_2x2`, `Vest_Repair_Closeup`, `Vest_Repair_TopQA` |
| `01_Vest/FinalRepair_SB/` | `Vest_FinalRepair_SB.png` |
| `01_Vest/Gloves_FinalRepair_SB/` | `Gloves_FinalRepair_SB.png` |
| `WesternShootingGloves_FinalCorrection_2x2.png` | Glove correction QA render ("Final" is part of the filename, **not** an approval state) |
| `2026-09-26_13-44-51.png` | Latest collage render |
| `wildwestgunslinger-directory.json` | 11.94 MB directory export (2026-09-26 13:08) |

Key reference facts extracted from `REFERENCE_INDEX.txt`: brown suede western
vest, moderate V-neck, snap buttons down centre front, left chest welt pocket,
two lower welt pockets, pointed front hems overlapping belt, dark back with cinch
strap + buckle; size-50 measurement table (chest 100, neck width 8, armhole
height 25, back length 46, back width 20.5, armhole width 13, chest width 21 —
"our torso ~100").

---

## 7. Pre-Existing Full Backup — `Z:\Мой диск\WWG_RECOVERY`

**UNKNOWN TO ALL THREE AUDITS.** A dated full recovery snapshot predating
git initialisation (2026-09-25) exists on network drive `Z:`.

| Field | Value |
|---|---|
| Path | `Z:\Мой диск\WWG_RECOVERY\2026-09-22_FULL\` |
| Manifest | `2026-09-22_FULL\WWG_RECOVERY_INFO.txt` (1 349 B) |
| Files | **90 626** |
| Size | **12 157.8 MB** (11.87 GB) |
| Project copy | 90 464 files / 12 060.8 MB |
| Art copy | 161 files / 96.9 MB → `WildWestGunslinger_art` |
| Copy date | 2026-09-22 17:31 |
| Desktop shortcut | `WildWestGunslinger Recovery.lnk` (2026-09-22 17:31) → `Z:\Мой диск\WWG_RECOVERY` |
| Project source | `(repository root)` |
| Art source | `~/Documents/WildWestGunslinger art` |

### Manifest-declared status

- Project copy: **SUCCESS** (robocopy `/E`, no deletions)
- Art copy: **SUCCESS** (robocopy `/E`, exit 1, no failures)
- Verify: **PASS** — 11 413 `.unity`/`.prefab`/`.cs` assets present
- Google Drive sync: **UNKNOWN** — DriveFS active, cloud upload completion NOT verified. **Do not treat as cloud-confirmed.**
- File-count delta 4: locked Unity `Temp\FSTimeGet-*` (3) + `Temp\UnityLockfile` (1) — error 32, regenerable lock files, **not project data**
- `SECRETS: EXCLUDED`
- Destination dir was created by an earlier aborted run (partial 9 967 files); resumed additively, nothing deleted or overwritten
- Explicit instruction: **DATED SNAPSHOT — DO NOT OVERWRITE AUTOMATICALLY**

`Z:` was verified **accessible** during this phase.

---

## 8. Build Artifacts — `~/Desktop`

**CORRECTION TO `AI_TOOLCHAIN_AUDIT.md`**, which states no release builds exist
(checkpoint frozen, local `Builds/`). **An Android build exists.**

| Item | Detail |
|---|---|
| `WildWest.apk` | **46.2 MB**, 2026-09-09 19:06, SHA256 `022177FFCC8070DEC912EEB89EB58463…` |
| `WildWest_BackUpThisFolder_ButDontShipItWithYourGame\` | IL2CPP output: 548 files / 656.5 MB (`il2cppOutput/`, `Managed/`) |
| `WildWestGunslinger_BurstDebugInformation_DoNotShip\` | 1 file, 0.1 MB |

`Managed/Assembly-CSharp.dll` is dated **2026-09-02 11:48**, i.e. the IL2CPP
build reflects code from 2026-09-02, well before the 2026-09-09 build date and
far before the current audit window.

None of these are inside the project and none are versioned. They are
**build artifacts, not deliverables** — recorded, not promoted.

---

## 9. Out-of-Scope Sources (non-project)

Machine-specific absolute paths are shown here only because these are **external
resources outside the repository**. Per `Documentation/DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md`
§8 (Path policy), unrelated personal Desktop items are **not enumerated**.

| External resource | Detail | Disposition |
|---|---|---|
| `~/Desktop/jvyb/VPN_DIAGNOSTIC_REPORT.md` | 32 669 B, 2026-09-26 21:18. Happ VPN / WFP network diagnostics | **OUT OF SCOPE** — unrelated to WWG |
| `~/Desktop/jvyb/VPN_WFP_READONLY_CHECK.md` | 15 072 B, 2026-09-26 21:32. Read-only WFP check | **OUT OF SCOPE** — unrelated to WWG |

> Other items exist on the user Desktop (personal documents, browser
> bookmarks, unrelated media folders, a loose text file of unverified content).
> They are **deliberately not itemised** here: they are unrelated to WildWestGunslinger
> and enumerating them would place personal data in project documentation. Their
> existence was observed during source discovery and is recorded only as a count.

### Security flag — metadata only, contents never accessed

| Field | Value |
|---|---|
| Existence | **PRESENT** (metadata confirmed) |
| Size | 233 B |
| Last modified | 2026-09-18 |
| Location | user Desktop (exact path recorded in `Documentation/REL/REL-0001-RECOVERY-AND-BACKUP.md` §5, which is the security register) |
| Contents | **NEVER READ. NEVER PRINTED. NEVER HASHED. NEVER COPIED.** |

> **HANDLING:** this file is outside the project and outside both backup scopes
> (the 2026-09-22 `Z:` manifest declares `SECRETS: EXCLUDED`). It must never be
> committed, copied into the repository, indexed, quoted, or included in any
> `Documentation/` artifact. Tracked as `RISK-02` in
> `Documentation/History/OPEN_RISKS.md`.

### Full secret-exposure scan result (Phase 2.5, 2026-09-29)

| Scope | Files | High-confidence pattern hits |
|---|---|---|
| Project Markdown | 57 | **0** |
| Git-tracked text files | 674 | **0** |
| Staged files | 0 | **0** |

15 pattern classes scanned (cloud access keys, VCS tokens, LLM API keys, OAuth
tokens, private-key blocks, JWTs, bearer tokens, URL-embedded credentials,
connection-string credentials, generic key/token/secret assignments, password
assignments, key-file extensions). **No value from any suspected secret is
reproduced in this report.** See
`Documentation/CONSOLIDATION_PRECOMMIT_VALIDATION.md` §B.

---

## 10. GitHub-Only Documents (retrieved for reconciliation)

Retrieved to an **external** folder on 2026-09-29 via `gh api`. Not copied into
the project; not canonical until reconciled.

| File | Local copy | GitHub size | Note |
|---|---|---|---|
| `AI_PRODUCTION_METHODOLOGY.md` | `~/WildWestGunslinger_RECOVERY\github_only\AI_PRODUCTION_METHODOLOGY.md` | 30 625 B | 29 300 B on disk |
| `AUDIT_SESSION_CONTEXT_2026-09-28.md` | `…\github_only\AUDIT_SESSION_CONTEXT_2026-09-28.md` | 24 322 B | 23 269 B on disk |

**Byte-difference cause:** the retrieved copies use LF line endings while the
GitHub blobs use CRLF (difference equals line count: 1 325 and 1 053
respectively). Content is semantically equivalent. For byte-exact verification,
compare against `git cat-file blob <sha>` from `origin/main` rather than the
LF-normalised copies.

Both files are **absent from the local working tree** while `origin/main` is
2 commits ahead — see `Documentation/DOC-0002-SUPERSEDED-DOCUMENTS.md`.

---

## 11. Sources That Do Not Exist

| Referenced source | Status |
|---|---|
| `Вставленная уценка.md` | Not found in Downloads or anywhere searched |
| Audit 1 standalone report file | Exists **only** in conversation history, never written to disk |
| Character "Blend in project" | **Contradicted** — real sources are in `Documents\WildWestGunslinger art` |
| `WWG_Doomy_Cowboy_REFINED.blend` "missing" | **Contradicted** — exists (Audit 1 scope error) |
| Release build "does not exist" | **Contradicted** — `WildWest.apk` exists |

---

## 12. Inventory Summary

| # | Source family | Location | Status |
|---|---|---|---|
| 1 | Project docs | repo root + `AI_CONTEXT/` | inventoried |
| 2 | GitHub-only docs | `origin/main` | retrieved externally |
| 3 | ChatGPT docs | `Downloads` | 4 found, 1 missing |
| 4 | OpenCode / skills | `.opencode/`, `~/.config/opencode` | inventoried |
| 5 | Audits 1/2/U-12 | repo + conversation | inventoried |
| 6 | Character art | `Documents\WildWestGunslinger art` | **newly found** |
| 7 | Vest/glove QA | `Desktop\Коллаж тест VVG` | **newly found** |
| 8 | Full backup | `Z:\Мой диск\WWG_RECOVERY` | **newly found** |
| 9 | Build artifacts | `Desktop` (apk/IL2CPP) | **newly found** |
| 10 | Recovery 2026-09-29 | `WildWestGunslinger_RECOVERY` | inventoried |
| 11 | Out-of-scope | `Desktop\jvyb`, misc | excluded |
| 12 | Secret-suspect | `Desktop\omniroute ключи.txt` | **flagged, not read** |

---

## 13. Audit Corrections Issued by This Inventory

| # | Prior claim | Source | Correction |
|---|---|---|---|
| 1 | `WWG_Doomy_Cowboy_REFINED.blend` NOT FOUND | Audit 1 | **FALSE** — exists in `Documents\WildWestGunslinger art` |
| 2 | Character blend sources "in project / in progress" | `AI_TOOLCHAIN_AUDIT.md` | **Misleading** — real sources outside project, dated 2026-09-21…26 |
| 3 | No release builds exist | `AI_TOOLCHAIN_AUDIT.md` | **FALSE** — `WildWest.apk` 46.2 MB (2026-09-09) + 656.5 MB IL2CPP output |
| 4 | `Chest=141cm` | `AUDIT_2_RECONCILIATION_REPORT.md` | **FALSE** — canonical `Doomy_measurements.md` chest **100** (size-50 table), torso height 926–1579 mm |
| 5 | `Skeleton=62 bones` | `AUDIT_2_RECONCILIATION_REPORT.md` | **FALSE** — canonical `WWG_Template_Armature` has **51 bones** |
| 6 | Backup picture = 2026-09-29 snapshot only | All audits | **Incomplete** — a 12.16 GB dated full backup exists at `Z:\` from 2026-09-22 |

Corrections 4 and 5 mean the audit's "foundation identified" conclusion was built
on incorrect numbers and must be re-derived. This is tracked as an open decision.

---

**End of `RECONCILIATION_SOURCE_INVENTORY.md`**
