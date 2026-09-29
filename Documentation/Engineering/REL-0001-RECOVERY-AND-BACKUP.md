# REL-0001 — Recovery and Backup

**Document ID:** `REL-0001`
**Status:** CANONICAL
**Date:** 2026-09-29

---

## 1. Backup inventory

Two full backups exist. Neither is in the repository.

| # | Date | Location | Size | Verification |
|---|---|---|---|---|
| 1 | **2026-09-22** | `Z:\Мой диск\WWG_RECOVERY\2026-09-22_FULL\` | 90 626 files / 12 157.8 MB (11.87 GB) | manifest says PASS; **cloud sync UNKNOWN** |
| 2 | **2026-09-29** | `~/WildWestGunslinger_RECOVERY\` | 873 files / 227.64 MB | **12/12 SHA256 PASS** |

Plus a dated Git recovery point:

| Ref | Commit | Created |
|---|---|---|
| branch `recovery/checkpoint-2026-09-29-audit2` | `040651b1d8963231b814217319b099b7ab5ef16d` | 2026-09-29 |
| tag `recovery-2026-09-29-audit2` | `040651b1d8963231b814217319b099b7ab5ef16d` | 2026-09-29 |

---

## 2. The 2026-09-22 backup (`Z:`) — previously unknown to all audits

Discovered in Phase 2 via a Desktop shortcut (2026-09-22 17:31) pointing at
`Z:\Мой диск\WWG_RECOVERY`. `Z:` was verified **accessible**.

### Manifest: `2026-09-22_FULL\WWG_RECOVERY_INFO.txt`

| Field | Value |
|---|---|
| Project source | `(repository root)` |
| **Art source** | `~/Documents/WildWestGunslinger art` |
| Destination | `Z:\Мой диск\WWG_RECOVERY\2026-09-22_FULL\` |
| Project file count | source 90 483 / destination 90 479 (**delta 4**) |
| Project total size | 11.78 GB / 11.78 GB |
| Art file count | 161 / 161 |
| Art total size | 96.9 MB / 96.9 MB |
| Project copy status | **SUCCESS** (robocopy `/E`, no deletions) |
| Art copy status | **SUCCESS** (robocopy `/E`, exit 1, no failures) |
| Verify status | **PASS** — 11 413 `.unity`/`.prefab`/`.cs` present |
| **Google Drive sync** | **UNKNOWN** — DriveFS active, cloud upload completion **not verified** |
| Secrets | **EXCLUDED** |
| Instructions | "DATED RECOVERY SNAPSHOT — DO NOT OVERWRITE AUTOMATICALLY" |

### Delta-4 explanation (not project data)

robocopy exit 9; 4 files failed with **error 32 (file in use)**:
`Temp\FSTimeGet-*` (3) and `Temp\UnityLockfile` (1), locked by the running
Unity Editor. All are **regenerable lock files**.

### Structural note

The destination directory was created by an **earlier aborted run** of the same
backup (partial, 9 967 files) and was **resumed additively** with `/E`.
**Nothing was deleted or overwritten from a prior snapshot.** The current
`Z:` copy is a union of the aborted run and the resumed run.

### Art coverage

The 161-file art copy is a **subset** of the 307-file
`Documents\WildWestGunslinger art` source. Which 146 files are excluded is
**not recorded in the manifest**.

---

## 3. The 2026-09-29 recovery

| Field | Value |
|---|---|
| Archive | `wwg_recovery_snapshot_20260929_164259.zip` |
| Cutoff timestamp | 2026-09-29 16:42:59 |
| Files | 873 |
| Size | 227.64 MB |
| SHA256 | `9ABE45EEB928521EECC28B8270BC41BD77CFB5969F01DDD3621D3CD7A2757513` |
| Content checks | 12/12 PASS |
| Art delta after cutoff | **ZERO** — see `ART_DELTA_AFTER_RECOVERY.md` |

### Coverage — and its critical gap

| Source | Covered by 2026-09-29 recovery? |
|---|---|
| Project working tree (873 tracked paths) | ✅ yes |
| `Working/` Blender sources (747 files) | ✅ yes |
| `Assets/` art, prefabs, materials | ✅ yes |
| `~/Documents/WildWestGunslinger art` (307 files) | ❌ **NO** — **but see `Documentation/Character/CHARACTER_SOURCE_BACKUP.md`: now covered by a separate verified backup (Phase 2.5)** |
| `~/Desktop/Коллаж тест VVG` (25 files) | ❌ **NO** — **but see `Documentation/Character/CHARACTER_SOURCE_BACKUP.md`: now covered by a separate verified backup** |
| `Desktop\WildWest.apk` | ❌ NO |
| Desktop IL2CPP output (656.5 MB) | ❌ NO |

> **The newest verified recovery does not cover the character art or the QA
> collage.** The only backup that covers character art is the 2026-09-22 `Z:`
> copy, which is **not cloud-verified** and captures only 161 of 307 files.
>
> See `History/OPEN_RISKS.md` — `RISK-01`.

---

## 4. Git recovery refs

```
main                                          7ad314c  (ahead 1, behind 2)
recovery/checkpoint-2026-09-29-audit2         040651b
tag recovery-2026-09-29-audit2                040651b
origin/main                                   f932fcc
```

`7ad314c` adds only the three audit/recovery reports; it does **not** contain
the 82 working-tree changes, which remain uncommitted.

---

## 5. Secrets handling

- The 2026-09-22 `Z:` manifest declares **`SECRETS: EXCLUDED`**.
- A secret-suspect file exists at `~/Desktop\omniroute ключи.txt`
  (233 B, 2026-09-18). **It was not read and not printed during this phase.**
- It is outside the project and outside both backup scopes.
- **It must never be committed, copied into the repository, or referenced in any
  `Documentation/` artifact.** See `History/OPEN_RISKS.md` — `RISK-02`.

---

## 6. Cross-references

- `ART_DELTA_AFTER_RECOVERY.md` — zero-delta proof
- `RECOVERY_POINT_REPORT.md` — 2026-09-29 construction detail
- `REL-0002` — build artifacts
- `History/OPEN_RISKS.md` — backup coverage risks

---

**End of `REL-0001-RECOVERY-AND-BACKUP.md`**
