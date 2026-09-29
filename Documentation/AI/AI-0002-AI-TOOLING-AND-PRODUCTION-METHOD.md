# AI-0002 — AI Tooling and Production Method

**Document ID:** `AI-0002`
**Status:** CANONICAL
**Date:** 2026-09-29
**Reconciles:** `AI_PRODUCTION_METHODOLOGY.md` (GitHub-only), `AI_TOOLCHAIN_AUDIT.md`, `AI_CONTEXT/*`

---

## 1. What the legacy methodology got right

`AI_PRODUCTION_METHODOLOGY.md` (30 625 B, `origin/main` only) and
`AI_TOOLCHAIN_AUDIT.md` (repo root) both advocate a **stage-gated pipeline**
with explicit gates before promotion. That structure is sound and is preserved
here as the state machine in `DOC-0001` §4.

The **measurement-first** art method documented in `ART-0003` is the strongest
part of the project's methodology and predates the AI documents — it is
canonical on its own evidence, not because an AI described it.

---

## 2. What the legacy methodology got wrong

| Legacy claim | Correction | Canonical source |
|---|---|---|
| `Chest=141cm` | Chest is **100** (size 50 table) | `Doomy_measurements.md` |
| `Skeleton=62 bones` | **51 bones** (`WWG_Template_Armature`) | `Doomy_measurements.md` |
| Character blend "in project, in progress" | Sources are complete, dated 2026-09-21…26, and live in `Documents\` | `ART-0001` §3 |
| `WWG_Doomy_Cowboy_REFINED.blend` missing | **Exists** | filesystem |
| No release build | `WildWest.apk` 46.2 MB (2026-09-09) | filesystem |
| Checkpoint frozen, `Builds/` local only | A 12.16 GB dated backup exists at `Z:` | `WWG_RECOVERY_INFO.txt` |
| Legacy "Stage 19/20/…" numbering is current | Superseded by `ART-`/`UNI-`/`GAME-`/`AI-`/`DOC-`/`REL-` IDs | `DOC-0003` |

> The legacy stage numbers are retained **only** as history, in
> `History/HISTORY-0002-LEGACY-STAGE-NUMBERING.md`. They are not identifiers.

---

## 3. Reconciled production method (canonical)

### Gate model

```
CREATED → QA PASS → APPROVED → PROMOTED → INTEGRATED → RUNTIME VERIFIED
```

Gates are **sequential and blocking**. Skipping a gate invalidates all later
claims. The project is currently at the `QA PASS → APPROVED` boundary.

### Evidence model

Every statement carries an evidence class (`DOC-0001` §1). Untested behaviour is
marked ❓, not omitted and not assumed.

### Change model

Documentation work never mutates product content. Product changes are made
deliberately, with QA, and then documented.

---

## 4. Current gate position

| Gate | Art | Environment | Characters | Weapons |
|---|---|---|---|---|
| G1 reference provenance | PASS | n/a | PASS | n/a |
| G2 measurement | PASS | — | PASS | — |
| G3 clearance | PASS | — | PASS | — |
| G4 multi-view QA | PASS | PASS | PASS | PASS |
| **G5 human approval** | **NOT PASSED** | **NOT PASSED** | **NOT PASSED** | **NOT PASSED** |
| G6 promotion | not started | not started | not started | not started |
| G7 integration | not started | **blocked (`UNI-D06`)** | not started | not started |
| G8 runtime verification | not started | not started | not started | not started |

---

## 5. Toolchain state

| Tool | State |
|---|---|
| Unity 6 (6000.x) | installed and used |
| Blender 5.1 | used for all art |
| Git | initialised 2026-09-25, history does not cover the art work |
| `gh` CLI | available (used to retrieve GitHub-only docs) |
| Blender MCP | used during prior sessions |
| CI | none documented |
| Build pipeline | none documented; only manual IL2CPP/APK output exists |

---

## 6. Cross-references

- `AI-0001` — agent state and trust model
- `ART-0003` — pipeline and QA gates
- `DOC-0002` — superseded documents
- `History/HISTORY-0003` — audit history and corrections

---

**End of `AI-0002-AI-TOOLING-AND-PRODUCTION-METHOD.md`**
