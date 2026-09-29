# REL-0002 — Build Artifacts

**Document ID:** `REL-0002`
**Status:** CANONICAL
**Date:** 2026-09-29

---

## 1. Build artifacts found

| Artifact | Location | Size | Date | Verdict |
|---|---|---|---|---|
| Android APK | `~/Desktop\WildWest.apk` | **46.2 MB** | 2026-09-09 19:06 | **stale, do not ship** |
| IL2CPP output | `~/Desktop\WildWest_BackUpThisFolder_ButDontShipItWithYourGame\` | 548 files / 656.5 MB | 2026-09-09 19:06 | intermediate output, do not ship |
| Burst debug info | `~/Desktop\WildWestGunslinger_BurstDebugInformation_DoNotShip\` | 1 file / 0.1 MB | 2026-09-09 19:04 | do not ship |

APK SHA256: `022177FFCC8070DEC912EEB89EB58463…` (32 hex chars captured;
full digest should be recomputed before any use).

---

## 2. What the IL2CPP output contains

| Folder | Contents |
|---|---|
| `Managed/` | managed assemblies; **`Assembly-CSharp.dll` dated 2026-09-02 11:48** |
| `il2cppOutput/` | generated C++ IL2CPP translation sources |

---

## 3. Staleness analysis — the critical fact

`Assembly-CSharp.dll` is dated **2026-09-02**, but the build folder is dated
**2026-09-09**. More importantly:

| Reference point | Date |
|---|---|
| IL2CPP managed assembly | 2026-09-02 |
| APK produced | 2026-09-09 |
| Canonical scene last modified | 2026-09-26 |
| Git initialised | 2026-09-25 |
| Character art latest | 2026-09-26 |

> The build reflects code from **2026-09-02** — three weeks before the current
> project state, and before both the scene changes and the entire character art
> programme.

**The APK is therefore a historical artifact of an earlier code state. It is not
evidence that the current project builds, and it is not evidence that Android
works** — touch controls are broken in the current build (`UNI-D07`).

---

## 4. Correction to legacy documentation

`AI_TOOLCHAIN_AUDIT.md` states that no release build exists because the
"checkpoint is frozen" and `Builds/` is local only.

> **This is false.** `WildWest.apk` (46.2 MB) and a 656.5 MB IL2CPP output both
> exist on the Desktop, produced 2026-09-09.

---

## 5. Release readiness

| Criterion | Status |
|---|---|
| Current code produces a build | **unknown** — no build has been made since 2026-09-09 |
| Android input functional | ❌ `UNI-D07` |
| No critical defects | ❌ 7 critical defects open |
| Character rendering | ❌ Capsules |
| NavMesh | ❌ absent |
| Version control coverage | ❌ 82 uncommitted changes; art unversioned |
| Automated build pipeline | ❌ none |
| Release signing / store config | not documented |

**Not releasable.** No build decision should be taken until the critical
defects in `UNI-0003` are closed.

---

## 6. Handling rules

1. These artifacts are **outside the repository** and **unversioned**.
2. They must **not** be copied into the repository.
3. They must **not** be treated as current-state evidence.
4. If a build is needed, produce a new one from a clean, committed state and
   record its commit in this document.

---

## 7. Cross-references

- `UNI-0004` — environment and toolchain
- `UNI-0003` — defects
- `GAME-0003` — Android platform status
- `REL-0001` — recovery and backup

---

**End of `REL-0002-BUILD-ARTIFACTS.md`**
