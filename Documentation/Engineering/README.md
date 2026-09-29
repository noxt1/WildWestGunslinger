# Engineering

> **Layer:** DOMAIN. **Apply** for Unity project, toolchain, build and release work.

Unity project state, data and asset registers, known defects, environment and toolchain, plus
recovery, build artefacts and security/release requirements.

## Current documents

| Document | Role |
|---|---|
| `UNI-0001-UNITY-PROJECT-STATE.md` | current Unity project state |
| `UNI-0002-UNITY-DATA-AND-ASSET-REGISTER.md` | scene, prefab and asset register |
| `UNI-0003-UNITY-KNOWN-DEFECTS.md` | defect register (`UNI-D*`) and runtime-audit mapping (`RT-*`) |
| `UNI-0004-UNITY-ENVIRONMENT-AND-TOOLCHAIN.md` | Unity version, environment, toolchain |
| `REL-0001-RECOVERY-AND-BACKUP.md` | recovery points and backup verification |
| `REL-0002-BUILD-ARTIFACTS.md` | build outputs and their validity |
| `REL-0003-SECURITY-AND-RELEASE-REQUIREMENTS.md` | security, privacy and release requirements |

## Why this folder exists

The Phase 6.3 target tree named only the gameplay, art and AI domains. `UNI-*` and `REL-*` are
real current engineering documents with no home in that list, and filing them under a gameplay or
art folder would misfile them. This folder is therefore an explicit, evidence-based addition.

## Source-of-truth rule

- Unity state, defects, toolchain: this folder
- Gameplay-facing system state: `../GAME/`
- Release readiness: `../REQUIREMENTS.md`, `../PROJECT_STATE.md`
- Cross-project reality: `../../PROJECT_TRUTH.md`