# REL-0003 — Security and Release Requirements

**Document ID:** `REL-0003`
**Status:** CANONICAL
**Date:** 2026-09-29
**Preserves content from:** `~/Downloads/CHATGPT_CHECKLIST.md`, legacy "STAGE 19 — Security, Privacy and Marketplace Release"
**Phase 2.5 purpose:** §8 reconciliation — this requirement set existed **only** in a legacy superseded document. It is preserved here, reclassified, and re-verified where evidence exists.

---

## 1. Classification

**EVERY item in this document is `[REQUIREMENT]` and `NOT IMPLEMENTED`.**

This is a release-readiness checklist, not a description of current state. The
legacy source marked all of it `⬜` (not started), and nothing in it has been
implemented since.

| Item | Meaning |
|---|---|
| `VERIFIED PASS` | Phase 2.5 actually checked it and it passed |
| `NOT IMPLEMENTED` | no work done; the requirement stands |
| `BLOCKED` | cannot be started until a decision is made |

---

## 2. 19.1 — Secrets and credentials

| # | Requirement | Status |
|---|---|---|
| 19.1.1 | No API keys, private tokens, passwords, signing secrets or developer credentials inside `Assets`, `Resources`, `StreamingAssets` or the shipped build | **NOT IMPLEMENTED** — see §3 for the partial Phase 2.5 check |
| 19.1.2 | OpenCode, Blender MCP, development MCP config and AI-provider credentials remain development-only | **NOT IMPLEMENTED** as a build-time exclusion — no build pipeline exists (`ISSUE-09`) |
| 19.1.3 | Search the Unity project and Git history for accidentally committed secrets | **PARTIALLY VERIFIED PASS** — see §3 |
| 19.1.4 | Check the final APK/AAB for accidentally included development credentials | **NOT IMPLEMENTED** — no current APK exists (`REL-0002`) |

## 3. 19.1.3 — Phase 2.5 verification result

| Scope | Files scanned | High-confidence secret patterns |
|---|---|---|
| Project Markdown | 57 | **0** |
| Git-tracked text files (`.cs`, `.json`, `.meta`, `.asset`, `.unity`, `.prefab`, `.mat`, config, `.md`, …) | 674 | **0** |
| Staged files | 0 | **0** |

15 pattern classes scanned. **No secret value is reproduced in any
documentation.** One secret-suspect file exists **outside** the project; its
contents were never read, printed, hashed, copied, or indexed — see
`REL-0001-RECOVERY-AND-BACKUP.md` §5 and `../History/OPEN_RISKS.md` `RISK-02`.

> **Scope limit — honest statement.** This check covered the **working tree**.
> It did **not** walk full Git history, and it did **not** unpack the 2026-09-09
> APK. Requirement 19.1.3 and 19.1.4 therefore remain **NOT fully closed**.

## 4. 19.2 — Network and external services

| # | Requirement | Status |
|---|---|---|
| 19.2.1 | Inventory every network request and third-party SDK/service | **NOT IMPLEMENTED** |
| 19.2.2 | Remove unnecessary development endpoints and test services from the release | **NOT IMPLEMENTED** |
| 19.2.3 | Store only the minimum data required by external services | **NOT IMPLEMENTED** |
| 19.2.4 | Validate external input and handle service failures safely | **NOT IMPLEMENTED** |
| 19.2.5 | Confirm the game remains functional when optional online services are unavailable | **NOT IMPLEMENTED** |

> No network or SDK inventory exists. Note: the legacy project scope is
> single-player, so 19.2.5 is likely a formality — but it is unverified.

## 5. 19.3 — Local data and saves

| # | Requirement | Status |
|---|---|---|
| 19.3.1 | Review `PlayerPrefs`, save files and other local storage | **NOT IMPLEMENTED** |
| 19.3.2 | Do not store credentials or unnecessary personal data locally | **NOT IMPLEMENTED** |
| 19.3.3 | Validate save data to prevent crashes or unintended gameplay values from modified/corrupted saves | **NOT IMPLEMENTED** |
| 19.3.4 | Review which gameplay values can safely remain client-side | **NOT IMPLEMENTED** |
| 19.3.5 | If online/competitive features are introduced later, move progression/reward validation to an authoritative server | **NOT IMPLEMENTED** — conditional on a scope change that has not been decided |

## 6. 19.4 — Anti-cheat and tamper protection

| # | Requirement | Status |
|---|---|---|
| 19.4.1 | Review client-side values affecting progression, currency, XP, rewards and unlocks | **NOT IMPLEMENTED** — and note `UNI-D10`: gun damage is currently a client-side runtime value, which is the same class of problem |
| 19.4.2 | Prevent simple save/config manipulation from unintentionally granting rewards | **NOT IMPLEMENTED** |
| 19.4.3 | Add server-side validation if online/competitive functionality is introduced | **NOT IMPLEMENTED** — conditional |
| 19.4.4 | Avoid intrusive anti-cheat unnecessary for the architecture | **REQUIREMENT** — binding design constraint for an offline game |

## 7. 19.5 — Permissions and privacy

| # | Requirement | Status |
|---|---|---|
| 19.5.1 | Use only Android permissions required by the game and its SDKs | **NOT IMPLEMENTED** — manifest not reviewed |
| 19.5.2 | Review the Android manifest and permissions introduced by third-party SDKs | **NOT IMPLEMENTED** |
| 19.5.3 | Prepare accurate privacy/data disclosures required by the target marketplace | **NOT IMPLEMENTED** |
| 19.5.4 | Verify SDK data collection against declared privacy information | **NOT IMPLEMENTED** |
| 19.5.5 | Remove unnecessary permissions and SDKs | **NOT IMPLEMENTED** |

## 8. 19.6 — Third-party assets, plugins and generated content

| # | Requirement | Status | Evidence |
|---|---|---|---|
| 19.6.1 | Verify commercial/marketplace licences for all shipped assets, plugins, models, textures, audio, fonts | **NOT IMPLEMENTED** | character references are CC0/public-domain (`_REFERENCES/SOURCES.md`); the **rest** of the asset set is unaudited |
| 19.6.2 | Confirm redistribution rights for modified/generated assets from third-party sources | **NOT IMPLEMENTED** | |
| 19.6.3 | Keep OpenCode, Blender MCP, AI tooling and development-only tools out of the shipped game | **NOT IMPLEMENTED** — no build pipeline to enforce it | |
| 19.6.4 | Maintain a simple inventory of third-party assets and their licences | **PARTIAL** — exists for character references only | `_REFERENCES/SOURCES.md` |

## 9. 19.7 — Advertising and monetization

| # | Requirement | Status |
|---|---|---|
| 19.7.1 | Integrate advertising only after core gameplay is stable | **REQUIREMENT** — core gameplay is **not** stable (7 critical defects open) |
| 19.7.2 | Use an established mobile advertising SDK | **NOT IMPLEMENTED** |
| 19.7.3 | Prefer optional rewarded ads over intrusive forced advertising | **REQUIREMENT** |
| 19.7.4 | Never interrupt active combat with an advertisement | **REQUIREMENT** |
| 19.7.5 | Do not show disruptive ads after every action or encounter | **REQUIREMENT** |
| 19.7.6 | Player explicitly chooses to watch an ad for a clearly stated bonus | **REQUIREMENT** |
| 19.7.7 | Define supported rewarded bonuses before implementation | **NOT IMPLEMENTED** |
| 19.7.8 | Possible bonuses: currency, extra reward, revive, temporary boost, post-match bonus | **IDEA** — explicitly non-binding |
| 19.7.9 | One completed advertisement cannot grant the same reward twice | **NOT IMPLEMENTED** |
| 19.7.10 | Handle cancelled, failed, unavailable and offline ad states without breaking gameplay | **NOT IMPLEMENTED** |
| 19.7.11 | Keep advertising SDK init and callbacks isolated from core combat/gameplay | **REQUIREMENT** |
| 19.7.12 | Review consent and privacy requirements for the chosen SDK | **NOT IMPLEMENTED** |
| 19.7.13 | Test advertising on Android test builds for correct reward delivery | **NOT IMPLEMENTED** |
| 19.7.14 | Verify advertising does not harm mobile controls, FPS or memory | **NOT IMPLEMENTED** |
| 19.7.15 | Finalize frequency/cooldown limits so monetization stays optional and non-intrusive | **NOT IMPLEMENTED** |

## 10. 19.8 — Release build hardening

| # | Requirement | Status |
|---|---|---|
| 19.8.1 | Configure Android release build correctly | **NOT IMPLEMENTED** for current code — the only APK is from 2026-09-09 and reflects 2026-09-02 code (`REL-0002`) |
| 19.8.2 | Use proper release signing | **NOT IMPLEMENTED** — not documented |
| 19.8.3 | Enable stripping/minification appropriately and verify it does not break the game | **NOT IMPLEMENTED** |
| 19.8.4 | Remove development logging, debug UI and development-only diagnostics | **NOT IMPLEMENTED** — no build pipeline to enforce it |
| 19.8.5 | Remove test scenes, debug menus and developer commands from the release | **NOT IMPLEMENTED** |
| 19.8.6 | Review packaged files and Android permissions | **NOT IMPLEMENTED** |
| 19.8.7 | Build a clean release APK/AAB | **NOT IMPLEMENTED** |
| 19.8.8 | Test the release build on a real Android device | **NOT IMPLEMENTED** — and touch controls are broken (`UNI-D07`), so this cannot pass today |

> Note: 19.8.2 interacts with `RISK-02`. Signing secrets must be sourced from a
> secret store, never from a Desktop text file.

## 11. 19.9 — Final security audit

| # | Requirement | Status |
|---|---|---|
| 19.9.1 | Search the entire project for secrets, test endpoints, debug code and developer credentials | **PARTIAL** — working tree only; Git history and APK not swept |
| 19.9.2 | Review permissions, SDKs, network access, saves, advertising and privacy disclosures together | **NOT IMPLEMENTED** |
| 19.9.3 | Perform clean-install testing | **NOT IMPLEMENTED** |
| 19.9.4 | Perform basic save-corruption and modified-save testing | **NOT IMPLEMENTED** |
| 19.9.5 | Verify development tools and credentials are absent from the release package | **NOT IMPLEMENTED** |
| 19.9.6 | Confirm marketplace-required privacy, advertising and security declarations | **NOT IMPLEMENTED** |
| 19.9.7 | Complete final security sign-off before the first marketplace submission | **NOT IMPLEMENTED** — **mandatory gate** |

---

## 12. Summary

| Sub-section | Requirements | Verified pass | Partial | Not implemented |
|---|---|---|---|---|
| 19.1 Secrets | 4 | 0 | 1 (19.1.3, working tree only) | 3 |
| 19.2 Network | 5 | 0 | 0 | 5 |
| 19.3 Saves | 5 | 0 | 0 | 5 |
| 19.4 Anti-cheat | 4 | 0 | 0 | 4 |
| 19.5 Permissions | 5 | 0 | 0 | 5 |
| 19.6 Third-party | 4 | 0 | 1 (19.6.4) | 3 |
| 19.7 Advertising | 15 | 0 | 0 | 15 |
| 19.8 Build hardening | 8 | 0 | 0 | 8 |
| 19.9 Final audit | 7 | 0 | 1 (19.9.1, partial) | 6 |
| **Total** | **57** | **0** | **3** | **54** |

> **19.9.7 (final security sign-off) is a hard gate. No marketplace submission
> may occur before it passes.**

---

## 13. Cross-references

- `REL-0002-BUILD-ARTIFACTS.md` — the stale APK
- `REL-0001-RECOVERY-AND-BACKUP.md` — §5 secret handling
- `../GAME/GAME-0004-REQUIREMENTS-AND-DESIGN-DECISIONS.md` — non-security requirements
- `../UNI/UNI-0003-UNITY-KNOWN-DEFECTS.md` — `UNI-D07`, `UNI-D10`
- `../History/OPEN_RISKS.md` — `RISK-02`
- `../History/OPEN_ISSUES.md` — `ISSUE-09` (no build pipeline)

---

**End of `REL-0003-SECURITY-AND-RELEASE-REQUIREMENTS.md`**
