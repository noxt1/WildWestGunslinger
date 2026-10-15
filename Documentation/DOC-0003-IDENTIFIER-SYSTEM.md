# DOC-0003 — Identifier System

**Document ID:** `DOC-0003`
**Status:** CANONICAL
**Date:** 2026-09-29

---

## 1. Prefixes

| Prefix | Domain | Folder |
|---|---|---|
| `DOC-` | Governance, index, superseded register, identifiers | `Documentation/` |
| `ART-` | Art programme, assets, pipeline, QA gates | `Documentation/Art/` |
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

### 5.1 `DEC-nn` versus the historical `DEC-0nn` series

Two distinct decision series exist and must never be merged or renumbered:

| Series | Form | Range in use | Meaning | Status |
|---|---|---|---|---|
| **Active** | `DEC-nn` — **two digits** | `DEC-01` … `DEC-12` | project decisions in the canonical register, `DECISIONS.md` §1.1 / `History/OPEN_DECISIONS.md` | **current, authoritative** |
| **Historical** | `DEC-0nn` — **three digits** | `DEC-001` … `DEC-013` | art-pipeline decisions from the Audit 2 enumeration, found only in `Documentation/Archive/Audits/AUDIT_2_RECONCILIATION_REPORT.md` | **archive only — historical, not renumbered** |

Rules:

- `DEC-011` (three digits, *modular wall/floor kit*) and `DEC-11` (two digits, *weapon base damage and
  `GunController` identity*) are **different decisions**. Neither is an alias of the other.
- The three-digit series is **frozen**. Do not renumber, extend or cite it as if it were active.
- When an active decision is assigned an identifier, it takes the next free **two-digit** value and
  is registered in `DECISIONS.md`. Never reuse a two-digit value that already has a meaning.
- Collision check before assigning: search both `DECISIONS.md` and `History/OPEN_DECISIONS.md`
  for the two-digit form **and** for the three-digit form, so a new identifier cannot silently
  overwrite an open decision.

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
