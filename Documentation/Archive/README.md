# Documentation Archive

> **Status:** ARCHIVE — historical and source material. **Not an operational source of truth.**

## What this is

This directory preserves documentation that is **no longer current** but whose history matters:
completed audit reports, superseded documents, recovered snapshots, and preserved source material.

It exists so that current documentation can stay small and unambiguous, without destroying the record
of **why** the project is in its present state.

## The rule that matters

> **Archived documentation is preserved for history and context only. It is not an operational
> source of truth and must not be applied to the current project unless a canonical document
> explicitly references it as historical evidence.**

A new agent working on this project should:

1. read `Documentation/README.md` first;
2. use **CURRENT** documents for instructions and state;
3. consult this Archive **only** when it needs to understand a past decision, a previous state, or
   the origin of a current rule;
4. **never** apply a requirement, architecture or status found here to the present project.

## Archive is not a security boundary

This repository is **public**. Moving a document here does **not** make it private, and does not
hide it. Every file remains readable on GitHub at its full history.

Consequently:

- secrets, credentials, API keys, tokens and private keys are forbidden here exactly as they are in
  CURRENT documentation;
- every document in this directory was security-reviewed with the same pattern set before being moved;
- no archived document was edited to remove content — history is preserved as written.

## Subdirectories

| Folder | Contents | When to use it |
|---|---|---|
| `Legacy/` | documentation fully replaced by the current canonical system | understanding what the structure replaced |
| `Audits/` | completed audit, verification and phase reports | checking whether something was actually verified, and how |
| `Historical/` | point-in-time snapshots, recovery records, superseded state | understanding an older state of the project |
| `Superseded/` | documents officially replaced by a canonical document | confirming a replacement and its date |
| `Source/` | preserved source material, including documents that only ever existed on GitHub | reading methodology or external-source material |

## How to tell *why* something was archived

Each archived document keeps its own status header. The migration plan that decided its placement is
`Documentation/DOCUMENTATION_ARCHIVE_MIGRATION_MAP.md`, which records, for every moved file, its
classification, destination and reason.

## Relationship to current documentation

- Current truth: `../PROJECT_TRUTH.md` and the documents listed in `../README.md`
- Current issues: `../History/OPEN_ISSUES.md`
- Current decisions: `../DECISIONS.md`
- The identifier scheme used across current docs: `../DOC-0003-IDENTIFIER-SYSTEM.md`

If an archived document appears to disagree with a current document, **the current document wins**
unless the current document explicitly says otherwise.
