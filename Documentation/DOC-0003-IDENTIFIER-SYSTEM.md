# DOC-0003 — Identifier System

**Document ID:** `DOC-0003`
**Status:** CANONICAL
**Date:** 2026-09-29

---

## 1. Prefixes

| Prefix | Domain | Folder |
|---|---|---|
| `DOC-` | Governance, index, superseded register, identifiers | `Documentation/` |
| `ART-` | Art programme, assets, pipeline, QA gates | `Documentation/ART/` |
| `UNI-` | Unity project, data, defects, toolchain | `Documentation/UNI/` |
| `GAME-` | Game design, gameplay systems, controls | `Documentation/GAME/` |
| `AI-` | AI agents, skills, production method | `Documentation/AI/` |
| `REL-` | Recovery, backups, builds, release | `Documentation/REL/` |
| `HISTORY-` | Timeline, legacy numbering, audit history | `Documentation/History/` |
| `OPEN_` | Unresolved decisions, risks, conflicts, issues | `Documentation/History/` |

---

## 2. Numbering

- Four digits, sequential within a domain: `0001`, `0002`, …
- Numbers are **permanent**. A retired document keeps its number; the number is
  never reused.
- New documents take the next free number in their domain.

---

## 3. Naming convention

```
<ID>-<UPPERCASE-TITLE-WITH-HYPHENS>.md
```

Examples:

- `UNI-0003-UNITY-KNOWN-DEFECTS.md`
- `ART-0001-ART-STATE.md`
- `HISTORY-0001-TIMELINE.md`

`OPEN_*` documents use a descriptive name without a number, because their count
is expected to shrink to zero:

- `OPEN_DECISIONS.md`
- `OPEN_RISKS.md`
- `OPEN_CONFLICTS.md`
- `OPEN_ISSUES.md`

---

## 4. Legacy Stage numbering

The historical `Stage 1` … `Stage 20+` sequence from `CHATGPT_CHECKLIST.md` and
`ART_PIPELINE.md` is **retired as an identifier system**. It is preserved only
as history in `History/HISTORY-0002-LEGACY-STAGE-NUMBERING.md` for traceability.

**New work must not introduce new Stage numbers.**

---

## 5. Defect identifiers

Unity defects use `UNI-Dnn`:

| ID | Title |
|---|---|
| `UNI-D01` | Zero NavMesh |
| `UNI-D02` | No character rig integrated |
| `UNI-D03` | Player and enemies are Capsules |
| `UNI-D04` | 12 broken object references |
| `UNI-D05` | 4 missing Mono Script references |
| `UNI-D06` | Modular FBX imported at 0.01×, Z-up uncompensated |
| `UNI-D07` | Android touch controls broken |
| `UNI-D08` | `Shooter.prefab` font material on mesh renderer |
| `UNI-D09` | `Rusher.prefab` debug material |
| `UNI-D10` | Gun damage overwritten to 200 |
| `UNI-D11` | Zero modular meshes integrated |
| `UNI-D12` | Zero destructible instances |

Art pipeline defects use `ART-Pnn` (see `ART-0003` §6).
Risks use `RISK-nn`. Decisions use `DEC-nn`. Conflicts use `CONFLICT-nn`.
Issues use `ISSUE-nn`.

---

## 6. Evidence tags

Every factual claim in a canonical document should be traceable to an evidence
class (`DOC-0001` §1). Use these inline tags where helpful:

| Tag | Meaning |
|---|---|
| `[runtime]` | observed in play mode |
| `[file]` | verified file/hash/GUID fact |
| `[git]` | from Git history |
| `[backup]` | from a backup manifest |
| `[measured]` | from a measurement document |
| `[doc]` | from project documentation |
| `[ai]` | AI/agent prose — advisory |
| `[assume]` | assumption, must be confirmed |

---

## 7. Cross-references

- `DOC-0001` — canonical index and evidence precedence
- `DOC-0002` — superseded register
- `History/HISTORY-0002` — legacy Stage numbering

---

**End of `DOC-0003-IDENTIFIER-SYSTEM.md`**
