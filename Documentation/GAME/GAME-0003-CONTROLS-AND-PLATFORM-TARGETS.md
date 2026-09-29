# GAME-0003 — Controls and Platform Targets

**Document ID:** `GAME-0003`
**Status:** CANONICAL
**Date:** 2026-09-29

---

## 1. Platform targets

| Platform | Intent | Status |
|---|---|---|
| **Windows PC** | primary development target | playable in editor and via IL2CPP build output |
| **Android** | secondary target | **BLOCKED** — touch controls non-functional |
| Other platforms | not planned | — |

---

## 2. PC controls

| Input | Status | Evidence |
|---|---|---|
| Keyboard/mouse | ✅ assumed working | PC play-mode session completed |
| Movement | ✅ | movement observed |
| Fire | ✅ | both weapons fired |
| Aim | ❓ | not explicitly tested |
| Reload | ❓ | not tested |
| Sprint / crouch / cover | ❓ | not tested |

> Only movement and firing were exercised. **Do not document a full control
> scheme as verified.**

---

## 3. Android controls

| Item | Status | Evidence |
|---|---|---|
| `MobileTouchControls` object | ❌ **present but all references `null`** | runtime |
| Touch input | ❌ | consequence of the above |
| On-screen controls | ❌ | not functional |
| APK build | ✅ artifact exists | `Desktop\WildWest.apk`, 46.2 MB, 2026-09-09 |

> **Contradiction to resolve:** an APK build exists from 2026-09-09, but the
> build machine's `Assembly-CSharp.dll` is dated **2026-09-02**, and touch
> controls are broken. Either the APK predates the touch-control code, or the
> APK was produced from a different (earlier) code state. The APK is a stale
> artifact and must not be treated as evidence that Android works.
> Tracked in `History/OPEN_CONFLICTS.md`.

---

## 4. Input architecture

| Layer | Status |
|---|---|
| Input actions / input map | not documented in the repository |
| `MobileTouchControls` script | exists in the scene with null references |
| Rebinding / accessibility | not implemented |

---

## 5. Required before Android is a viable target

1. Repair `MobileTouchControls` references (`UNI-D07`).
2. Enumerate and fix `UNI-D04` / `UNI-D05` — a null-ref path may be why touch
   references are null in the first place.
3. Rebuild the APK from a known commit; discard the 2026-09-09 artifact or
   archive it explicitly as historical.
4. Verify on a physical device, not only in the editor.

---

## 6. Cross-references

- `UNI-0003` — defects
- `REL-0002` — build artifacts
- `GAME-0002` — gameplay systems state

---

**End of `GAME-0003-CONTROLS-AND-PLATFORM-TARGETS.md`**
