# AI

> **Layer:** DOMAIN. **Apply** for AI/agent work. Cross-project truth stays in the root documents.

AI architecture, agent state, tooling, and runtime-verification requirements.

## Current documents

| Document | Role |
|---|---|
| `AI-0001-AI-AGENT-STATE.md` | agent state and operating context |
| `AI-0002-AI-TOOLING-AND-PRODUCTION-METHOD.md` | tooling and production method for AI work |

## Boundary

`../AI_PRODUCTION_METHODOLOGY.md` is the **cross-project production specification** and stays in
the documentation root. It governs every domain, not only AI, and is not duplicated here.

`../../AI_CONTEXT/` is the **operational/reference layer** for agent working rules. It is separate
from canonical documentation and is not a substitute for it.

## Source-of-truth rule

- AI architecture and tooling: this folder
- How production is executed: `../AI_PRODUCTION_METHODOLOGY.md`
- Open AI gaps: `../History/OPEN_ISSUES.md`
- Historical AI audits: `../Archive/Audits/` (context only)