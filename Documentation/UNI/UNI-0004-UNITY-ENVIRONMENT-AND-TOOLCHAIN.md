# UNI-0004 — Unity Environment and Toolchain

**Document ID:** `UNI-0004`
**Status:** CANONICAL
**Date:** 2026-09-29

---

## 1. Editor

| Field | Value |
|---|---|
| Engine | Unity 6 (6000.x series) |
| Project root | repository root (relative: `.`) |
| Unity version (exact) | **not pinned in documentation** — verify in `ProjectSettings/ProjectVersion.txt` |
| Render pipeline | URP indicated by the material name `TMP_SDF-HDRP LIT` |
| Input system | not documented |

> The exact Unity version is **not recorded** in any project document. It should
> be read from `ProjectSettings/ProjectVersion.txt` and pinned, since package
> resolution depends on it. Tracked in `History/OPEN_ISSUES.md`.

---

## 2. Packages

Package manifest contents were not captured in the audit reports. The material
name `TMP_SDF-HDRP LIT` indicates Universal Render Pipeline. Full package
inventory is an open item.

---

## 3. Platform targets

| Platform | Intent | State |
|---|---|---|
| Windows PC | primary dev | **RUNTIME VERIFIED** — editor play-mode session completed 2026-09-29; IL2CPP build output from 2026-09-09 also exists |
| Android | secondary | **blocked** (`UNI-D07` touch controls) |
| IL2CPP | backend | evidenced by the 2026-09-09 build output |
| Burst | enabled | `BurstDebugInformation` artifact exists |

---

## 4. Build outputs

| Build | Date | Status |
|---|---|---|
| `WildWest.apk` | 2026-09-09 | **stale** — managed assembly from 2026-09-02 (`REL-0002`) |
| IL2CPP output | 2026-09-09 | stale intermediate |
| Current HEAD build | — | **never attempted** |

---

## 5. Automation

| Facility | State |
|---|---|
| CI/CD | **none** |
| Automated tests | **none documented** |
| Build script | **none documented** |
| Version tagging | only the recovery tag |
| Release signing config | not documented |

---

## 6. Version control

| Field | Value |
|---|---|
| Initialised | 2026-09-25 — **after** all art work (2026-09-21…26) |
| Branch | `main` |
| HEAD | `7ad314c869e5dad1daf8e56fdf0b8f4197592969` |
| `origin/main` | `f932fcc722d22b8150ccf7e9616914324347c6e8` |
| Divergence | **ahead 1 / behind 2** |
| Uncommitted | 82 entries (15 M, 2 D, 65 untracked) |
| Staged | 0 |
| `.gitignore` | **unmodified by the recovery phase** |
| `Working/` tracking | `blender_src/**/*.blend` and `reports/*.md` tracked; `blender_reviews/` and `*.blend1` are ignore candidates |

### Two files on `origin/main` are missing locally

`AI_PRODUCTION_METHODOLOGY.md` and `AUDIT_SESSION_CONTEXT_2026-09-28.md` exist
on `origin/main` (2 commits ahead) but are **absent from the local working
tree**. Retrieved copies live in
`~/WildWestGunslinger_RECOVERY\github_only\`.

> Do not `git pull` blindly — the divergence (ahead 1 / behind 2) plus 82
> uncommitted changes requires a deliberate decision. See
> `History/OPEN_DECISIONS.md` — `DEC-07`.

---

## 7. Cross-references

- `UNI-0001` — project state
- `UNI-0003` — known defects
- `REL-0002` — build artifacts
- `History/OPEN_DECISIONS.md` — Git divergence decision

---

**End of `UNI-0004-UNITY-ENVIRONMENT-AND-TOOLCHAIN.md`**
