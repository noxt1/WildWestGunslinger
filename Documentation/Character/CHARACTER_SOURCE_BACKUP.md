# CHARACTER SOURCE BACKUP

**Document ID:** `CHAR-0001`
**Status:** **VERIFIED**
**Date:** 2026-09-29
**Closes:** `RISK-01` (see `../History/OPEN_RISKS.md`)

---

## 1. Source

**External resource** (outside the Git repository):

```
~/Documents/WildWestGunslinger art
```

Absolute form: `C:\Users\cyril\Documents\WildWestGunslinger art`

| Metric | Value |
|---|---|
| File count | **307** |
| Total size | **180.2 MB** |
| Newest file | 2026-09-26 12:02 |
| Substantive sub-tree | `art/Characters/Kevin Iglesias/WildWest/` |

### 1.1 File-type census (source)

| Type | Files | Role |
|---|---|---|
| `.png` | 117 | QA renders (character `QA/` sets) |
| `.blend` | 43 | **character + rig source of record** |
| `.jpg` | 36 | clothing reference images |
| `.meta` | 31 | Unity metadata leftovers |
| `.fbx` | 28 | Unity export (characters + weapons) |
| `.blend1` | 24 | Blender auto-backups |
| `.prefab` | 14 | Unity prefab leftovers |
| `.mat` | 7 | Unity material leftovers |
| `.md` | 5 | **measurement + reference-provenance docs** |
| `.zip` | 1 | — |
| `.unity` | 1 | Unity scene leftover |
| **Total** | **307** | |

### 1.2 Critical content confirmed present

| Item | Count | Note |
|---|---|---|
| `.blend` character sources | 43 | incl. the `WWG_Foundation_Source` lineage |
| QA renders (`.png`) | 117 | `QA/` checkpoints `CK1`–`CK18`, `R_*`, `SHIRT_*` |
| Reference images (`.jpg`) | 36 | `WWG_Doomy_Cowboy/_REFERENCES/` |
| FBX exports | 28 | 5 characters + 4 weapons, in `FBX/` and `Unity_Export/` |
| Measurement documents | 4 | `Doomy_measurements.md`, `Doomy_clearance_profile.md`, `Doomy_vest_measurements.md`, `Doomy_vest_clearance_profile.md` |
| Reference-provenance document | 1 | `_REFERENCES/SOURCES.md` (39 referenced images, licences) |

### 1.3 Rig and animation sources — important finding

| Question | Answer |
|---|---|
| Is there a standalone armature file? | **No.** `WWG_Template_Armature` exists as an **object inside** the `WWG_Foundation_Source.blend` lineage, per `Doomy_measurements.md` |
| Is there a separate skeleton/rig file? | **No** — no file matches armature/skeleton/rig/anim by name |
| Is there a standalone animation data file? | **No** |
| What does this mean? | The **51-bone armature exists only inside the `.blend` files.** Any backup that excluded `.blend` would have lost the rig. This backup is `.blend`-complete. |

> Per the path policy in `../DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md` §8, the
> external source location is recorded here as an **External Resource**, not as
> project-relative content.

---

## 2. Backup

| Field | Value |
|---|---|
| Date/time | **2026-09-29 17:06:06** |
| Archive | `~/WildWestGunslinger_RECOVERY/character_source_20260929_170606/wwg_character_source_20260929_170606.zip` |
| Manifest | `…/character_source_20260929_170606/character_source_manifest_20260929_170606.csv` |
| Archive size | **178 198 663 bytes** (169.94 MB) |
| Archive SHA256 | `FE53758A0B92D27D399C5CED2243F8D0CD4F674B544EE52E98E16C61615B63EF` |
| Entries | **332** |
| Uncompressed total | **213 482 332 bytes** (203.6 MB) |
| Manifest | **full** — path, size, SHA256, source path, mtime for every file |

### 2.1 Scope of the backup

The archive contains **two** external character-art sets:

| Entry prefix | Source | Files |
|---|---|---|
| `Characters/` | `~/Documents/WildWestGunslinger art` | 307 |
| `Collage/` | `~/Desktop/Коллаж тест VVG` | 25 |
| **Total** | | **332** |

The vest/glove QA collage was included because it is character art evidence that
**no other backup covers**.

### 2.2 Independence

| Requirement | Status |
|---|---|
| Independent of the Git repository | **Yes** — stored outside the repository root, not tracked, not staged |
| Independent of Recovery Point 2026-09-29 | **Yes** — separate directory, separate archive, separate manifest |
| Independent of the 2026-09-22 `Z:` backup | **Yes** — different media, different path, freshly computed hashes |
| Verifiable | **Yes** — full SHA256 manifest, §3 |
| Fit for full restore | **Yes** — restore-tested, §4 |

### 2.3 Non-destructive guarantee

The originals were **read only**. They were not moved, renamed, modified,
or deleted. Backup creation used read + archive operations exclusively.

---

## 3. Verification method and result

### 3.1 Method

1. Enumerated every file in both source sets.
2. Computed **SHA256 for every file** (332 of 332 — full manifest, not a sample).
3. Wrote the manifest to CSV alongside the archive.
4. Created the archive with `Optimal` deflate compression.
5. **Re-opened the archive read-only** and streamed every entry back out,
   recomputing SHA256 **from the archived bytes** — not from the originals.
6. Compared per-file: presence, byte size, and SHA256.

### 3.2 Result — full verification

| Check | Expected | Actual | Result |
|---|---|---|---|
| Source file count | 332 | 332 | **PASS** |
| Backup entry count | 332 | 332 | **PASS** |
| Files verified (SHA256 + size) | 332 | **332** | **PASS** |
| Missing from backup | 0 | **0** | **PASS** |
| Extra in backup | 0 | **0** | **PASS** |
| Hash mismatches | 0 | **0** | **PASS** |
| Size mismatches | 0 | **0** | **PASS** |
| Source total bytes | 213 482 332 | 213 482 332 | **PASS** |
| Backup uncompressed bytes | 213 482 332 | 213 482 332 | **PASS** |

> ### BACKUP VERIFIED
> 332/332 files verified by SHA256 **and** byte size, read back from the archive.
> 0 missing, 0 extra, 0 mismatches. This is a **full** verification, not a sample.

---

## 4. Restore test

The archive was **fully extracted** to a temporary directory and the extracted
files re-hashed against the manifest.

| Check | Result |
|---|---|
| Files extracted | 332 / 332 **PASS** |
| Critical file restores | 5 / 5 **PASS** |
| Temporary extraction directory | removed after the test |

> The archive is proven restorable, not merely present.

---

## 5. Critical file hashes

All values are **SHA256 of the archived copy**, which was confirmed identical to
the source.

| File | Bytes | SHA256 |
|---|---|---|
| `WWG_Doomy_Cowboy/WWG_Doomy_Cowboy_REFINED.blend` | 2 360 988 | `47332B9B06953CBC9786C85262E06BC649E247371A6F923440C3AC88CA5D9269` |
| `WWG_Foundation_Source.blend` | 1 704 329 | `6B0EC5D3AE837586C2AAE3CB4412A6B15316EAD46226B841DCF67B399BBEC7FF` |
| `WWG_Doomy_Cowboy/Doomy_measurements.md` | 2 705 | `481B46FB67D948DF36800A90C2F5771B51587774CCDB39800464C47616386F34` |
| `WWG_Doomy_Cowboy/Doomy_clearance_profile.md` | 1 520 | `F463FB35869E4F6D35A5B08E…` *(manifest holds the full value)* |
| `WWG_Doomy_Cowboy/Doomy_vest_measurements.md` | 2 900 | `2C579114EBB2D17E941B883E…` *(manifest holds the full value)* |
| `WWG_Doomy_Cowboy/Doomy_vest_clearance_profile.md` | 1 170 | `E7F726515438DBB917AE3F82…` *(manifest holds the full value)* |
| `WWG_Doomy_Cowboy/_REFERENCES/SOURCES.md` | 11 205 | `D975465DC447D9F55D536C7B6A0144CD101A409ED57B8374C6605D85418D71` |
| `Unity_Export/Characters/WWG_Player.fbx` | 1 230 860 | `0772946CA4A4DA17337C3F9BFC4F6A4005DDD70E2EFE466FAC68AE91F5399A06` |
| `Unity_Export/Characters/WWG_Shooter.fbx` | 1 287 532 | `A916943368289B7FF01547C2…` *(manifest holds the full value)* |
| `Unity_Export/Characters/WWG_Rusher.fbx` | 1 178 796 | `B918CAA36DA11E3C463B8263…` *(manifest holds the full value)* |
| `Unity_Export/Characters/WWG_Bandit.fbx` | 1 175 212 | `B992BE42561BF6627C0AF591…` *(manifest holds the full value)* |
| `Unity_Export/Characters/WWG_Tactical.fbx` | 1 414 236 | `D7730473DE72E407BC25EFE4…` *(manifest holds the full value)* |

> Truncated values above are abbreviated for table width. **The CSV manifest
> contains the complete 64-character SHA256 for all 332 files** and is the
> authoritative record.

### 5.1 Specifically requested checks

| Requested | Result |
|---|---|
| `WWG_Doomy_Cowboy_REFINED.blend` | **PRESENT, VERIFIED** — `47332B9B…D9269`; also explicitly restore-tested |
| `WWG_Template_Armature` source | **No standalone file exists** — it is an object inside the `WWG_Foundation_Source.blend` lineage, which is backed up and hash-verified (`6B0EC5D3…EC7FF`) |

---

## 6. Limitations and residual risk

| # | Limitation | Impact |
|---|---|---|
| L1 | The archive is on the **same physical machine** as the source (`C:` → `C:`) | Protects against accidental deletion/edit, **not** against machine or disk failure |
| L2 | The archive is **not** on `Z:` and **not** cloud-confirmed | The 2026-09-22 `Z:` cloud-sync status remains `UNKNOWN` (`RISK-04`) |
| L3 | The **canonical** character variant is still undecided (`DEC-02`) | The backup is complete, so this is a *selection* problem, not a *preservation* problem |
| L4 | `.blend1` backups and the `FBX/` vs `Unity_Export/` duplication are preserved as-is | Deliberate — pruning is a human decision (`DEC-02`) |
| L5 | Git still does not track character art | The backup is a safety net, **not** a substitute for `DEC-03` |

---

## 7. Risk status

| Field | Value |
|---|---|
| Risk | `RISK-01` — character art not covered by any verified backup |
| Status | **CLOSED** |
| Basis | Full 332/332 SHA256 + size verification, read back from the archive; 5/5 critical restore test |
| Residual | `MITIGATED` for machine/disk failure only — see L1/L2, tracked as `RISK-04` |

---

## 8. Cross-references

- `../REL/REL-0001-RECOVERY-AND-BACKUP.md` — all backup topology
- `../ART/ART-0001-ART-STATE.md` — art state and measured data
- `../ART/ART-0002-ART-ASSET-REGISTER.md` — per-asset state
- `../History/OPEN_RISKS.md` — `RISK-01`, `RISK-04`
- `../History/OPEN_DECISIONS.md` — `DEC-02`, `DEC-03`
- `../../Documentation/Archive/Historical/RECONCILIATION_SOURCE_INVENTORY.md` — source discovery record

---

**End of `CHARACTER_SOURCE_BACKUP.md`**
