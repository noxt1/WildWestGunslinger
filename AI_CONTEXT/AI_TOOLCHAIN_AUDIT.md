# AI Toolchain Audit — WildWestGunslinger

Дата: 2026-09-25

> Статус документа: только чтение и отчёт. Никаких установок, обновлений и изменений конфигурации в ходе аудита не выполнялось.
> Единственное изменение в системе: создание этого файла.
> Источники фактов: локальные файлы проекта (`AI_CONTEXT/`, `.opencode/`, `Packages/manifest.json`),
> глобальный `opencode.json`, живые проверки (`opencode --version`, TCP-пробы портов, процессы ОС),
> официальные README репозиториев на GitHub (без клонирования, только чтение страниц).

---

## Executive Summary

- Текущий baseline **подтверждён фактически**: OpenCode **1.18.31 (V1)**, Node v24.21.0, Python 3.14.7, Git 2.55.0, Unity 6000.3.23f1, Blender MCP **жив**, Unity MCP **настроен, но недоступен** (Unity не запущен).
- Проект `C:\Users\cyril\WildWestGunslinger` — **не git-репозиторий**. Это блокирует всё, что зависит от git worktrees (Vibe Kanban, Superpowers `using-git-worktrees` / `finishing-a-development-branch`, GitHub-интеграции).
- Docker **отсутствует**. Это блокирует Dagger и затрудняет self-hosted n8n.
- Ни один из кандидатов (Spec Kit, Superpowers, Gemini CLI, n8n, Dagger, Vibe Kanban) **не установлен**. Конфигурация OpenCode их не содержит — конфликтов установки сейчас нет, есть только потенциальные.
- Главная рекомендация: **ничего не ставить целиком**. Взять процедурные механизмы (verification-before-completion, systematic-debugging, speckit converge/bug-assess) как текст в `AI_CONTEXT`, а единственным кандидатом на установку после approval оставить **Gemini CLI как независимого reviewer** + **GitHub CLI** как предпосылку для всего git-будущего.
- **Vibe Kanban — SKIP** (официально sunsetting + требует git). **Dagger — LATER**, **n8n — LATER**, **Spec Kit — ADAPT** (без `specify init`), **Superpowers — ADAPT** (без plugin).

---

## Current Environment

| Компонент | STATUS | VERSION | LOCATION | SOURCE |
|---|---|---|---|---|
| OpenCode | INSTALLED | 1.18.31 (V1, подтверждено `opencode --version` + V1-форма конфига) | npm global `opencode-ai@1.18.31` | https://github.com/sst/opencode |
| Node / npm | INSTALLED | v24.21.0 / 11.19.0 | системный | https://nodejs.org |
| Python | INSTALLED | 3.14.7 (`python`; алиас `python3` отсутствует) / pip 26.2.1 | `C:\Python314` | https://www.python.org |
| uv / uvx | INSTALLED | версия не проверялась (бинарники есть) | `C:\Users\cyril\.local\bin\` | https://github.com/astral-sh/uv |
| Git | INSTALLED (но проект — не репозиторий) | 2.55.0.windows.3 | системный | https://git-scm.com |
| GitHub CLI (`gh`) | NOT_INSTALLED | — | — | https://github.com/cli/cli |
| Gemini CLI | NOT_INSTALLED | — | — | https://github.com/google-gemini/gemini-cli |
| n8n | NOT_INSTALLED | — | — | https://github.com/n8n-io/n8n |
| Dagger | NOT_INSTALLED | — | — | https://github.com/dagger/dagger |
| Spec Kit / `specify` | NOT_INSTALLED | — | — | https://github.com/github/spec-kit |
| Superpowers | NOT_INSTALLED (нет `plugin`/`plugins` ключа в конфиге) | — | — | https://github.com/obra/superpowers |
| Docker | NOT_INSTALLED | — | — | https://www.docker.com |
| blender-mcp (исполнитель) | INSTALLED | версия флагом не отдаёт (`--version` не поддерживается; `--help` OK) | `C:\Users\cyril\.local\bin\blender-mcp.exe` | совместим с https://github.com/ahujasid/blender-mcp |
| Unity Editor | INSTALLED (по логу и PROJECT_STATE) | 6000.3.23f1 | `C:\Program Files\Unity\Hub\Editor\6000.3.23f1\` | Unity Hub |
| Blender | INSTALLED (процесс жив) | версия не проверялась (не трогали сцену) | процесс `blender` PID 23104 | https://www.blender.org |
| `claude.exe` | OBSERVED (бинарник есть, не проверялся) | UNKNOWN | `C:\Users\cyril\.local\bin\claude.exe` | UNKNOWN, требует отдельной проверки |
| OmniRoute provider | CONFIGURED (не проверялся живой опрос) | — | `http://localhost:20128/v1` (см. opencode.json) | локальный шлюз, владелец неизвестен — уточнить |

Доступные команды OpenCode CLI (из `opencode --help`): `completion`, `acp`, `mcp`, `attach`, `run`, `debug`, `providers` (`auth`), `agent`, `upgrade`, `uninstall`, `serve`, `web`, `models`, `stats`, `export`, `import`, `github`, `pr`, `session`, `plugin` (`plug`), `db`.
Проектных кастомных commands/агентов не обнаружено (в `.opencode/` только `skills/`, `node_modules/`, `package.json`).

---

## Existing OpenCode Setup

- **Версия и поколение: V1, 1.18.31.** Ожидаемый baseline 1.18.31 **подтвердился точно**. Признаки V1: плоская секция `"mcp": { "<name>": {...} }`, отсутствие `mcp.servers`, plugin-механизм V1 (`"plugin"` singular, см. INSTALL.md Superpowers). V2-миграция не выполнялась и не требуется.
- **Поддержка версии плагинами:** Superpowers поддерживает 1.x через `"plugin": ["superpowers@git+..."]`; `@opencode-ai/plugin` зафиксирован как `1.18.31` и в глобальном, и в проектном `package.json` — консистентно. Известный риск только на стороне Windows: у некоторых сборок OpenCode бывают проблемы с git-backed plugin specs (Bun не находит `git.exe`) — задокументировано в INSTALL.md Superpowers, обход через локальный `npm install` существует.
- **Конфиги:** глобальный `C:\Users\cyril\.config\opencode\opencode.json` (skills-пути ×4, провайдеры omniroute + lmstudio, MCP unityMCP + blenderMCP). Бэкап `opencode.json.bak` отличается только отсутствием пути `wwg-specialized` — исторически добавлен позже, конфликта нет. Проектный `.opencode/package.json` — только зависимость `@opencode-ai/plugin 1.18.31`. Кастомных конфигураций агентов/пермишенов не найдено.
- **Skills discovery:**
  - Project-level (1): `wwg-character-art` (`C:\Users\cyril\WildWestGunslinger\.opencode\skills\`).
  - Global через `skills[]` (4 корня): `cc-blender-skill` (~31 skill: modeling, materials, lighting, cameras, rendering, animation, export, uv-texturing, contour-to-mesh, reference-to-3d, wireframe-to-3d, quality-refinement-autoloop и др.), `opencode-skills` (27: tdd-workflow, structured-project-execution, multi-agent-orchestration, blender-* и др.), `unity-game-skills` (27: unity-game-director, unity-mcp-bridge, unity-debug-profiler, unity-qa-release и др.), `wwg-specialized` (1: blender-physics-simulation).
- **Конфликты имён skills:** прямых коллизий ID **нет** (проверены листинги всех 4 корней + проектный). Концептуальные пересечения (не коллизии): `tdd-workflow` ↔ будущий `test-driven-development`, `structured-project-execution` ↔ `writing-plans`/`executing-plans`, `multi-agent-orchestration` ↔ `dispatching-parallel-agents`. Разрешение — приоритетом/переименованием только после approval, сейчас ничего не трогать.
- **MCP в конфиге:** `unityMCP` (remote `http://127.0.0.1:8080/mcp`, enabled) и `blenderMCP` (local `blender-mcp`, env `BLENDER_MCP_HOST/PORT=localhost:9876`, enabled). Других MCP нет.
- **AI_CONTEXT как source of truth:** `README, RULES, PROJECT_STATE, CURRENT_TASK, CONFIRMED_STATE, ARCHITECTURE, ART_PIPELINE, CHANGELOG` прочитаны выборочно (релевантные toolchain факты). Workflow проекта: АНАЛИЗ → ПЛАН → СОГЛАСОВАНИЕ → ИЗМЕНЕНИЕ → COMPILE → UNITY TEST → ПОДТВЕРЖДЕНИЕ; подтверждение даёт только пользователь. Checkpoint: Stage 4.4.5 FROZEN/PAUSED, активна Blender/art-ветка без смены номера Stage.

---

## Existing MCP

| MCP | STATUS | VERSION | ENDPOINT | CONNECTION | CONFIG LOCATION | OWNER/PROCESS | POTENTIAL CONFLICTS |
|---|---|---|---|---|---|---|---|
| Unity MCP (`unityMCP`) | CONFIGURED / DOWN | пакет `com.coplaydev.unity-mcp` `#main` (см. `Packages/manifest.json`); версия сервера UNKNOWN | `http://127.0.0.1:8080/mcp` (remote) | TCP 127.0.0.1:8080 **closed** 2026-09-25 (Unity Editor не запущен; batchmode-лог от 18.09 подтверждает было использование) | `opencode.json` (клиент) + UPM-пакет в проекте (сервер) | CoplayDev/unity-mcp (MIT). Процесс Unity сейчас не найден | Будущий второй Unity-MCP (напр. IvanMurzak) на том же порту 8080 — конфликт. Не ставить параллельно |
| Blender MCP (`blenderMCP`) | INSTALLED / UP | UNKNOWN (флаг `--version` не поддерживается; `--help` OK; API-поверхность отвечает) | stdio + `localhost:9876` | TCP 127.0.0.1:9876 **open**; процессы `blender-mcp` ×2; Blender отвечает (Layout, unsaved scene, selection пустой) | `opencode.json` (env `BLENDER_MCP_HOST/PORT`) + бинарник `.local\bin` | ahujasid/blender-mcp-совместимый (по CLI-сигнатуре). Процессы 24012/32380 + Blender 23104 | Альтернативные Blender-MCP (PatrykIti, Blender Foundation) дерутся за тот же Blender/addon-порт — только один за раз |

Целевая архитектура `OpenCode → MCP → Unity → Blender → GitHub → Files` **принципиально возможна**: два плеча уже описаны в конфиге, GitHub-плечо частично есть (`opencode github/pr` команды, но без `gh` и без git-репозитория — неработоспособно до инициализации git). Перезапуск/смена MCP не выполнялись и не требуются.

---

## Spec Kit

Источник: https://github.com/github/spec-kit (~139k stars, MIT, Python 3.11+, установка `uv tool install specify-cli`).

- **Актуальная версия:** точный номер релиза не фиксируем (релизы частые; проверяется командой `specify --version` после установки). Важно: `uv` уже есть на машине — установка тривиальна, но **не выполнена сознательно**.
- **Установка:** `specify init <dir> --integration opencode` (+ `--non-interactive --force` для непустой директории). Native integration для OpenCode **есть** (ключ `opencode`).
- **Что добавляет в проект:** каталог `.specify/` (specs, plans, tasks, constitution, опционально `bugs/`, `assessments/`); по умолчанию legacy-команды `.opencode/commands/speckit.*.md` (современный OpenCode их как skills **не подхватывает**); opt-in режим `--integration-options="--skills"` кладёт ~10 skills `.opencode/skills/speckit-*/SKILL.md` (PR #2494/#3539; в skills-режиме `commands/` не создаётся — режимы взаимоисключающие).
- **Что может изменить:** при `init` — создание `.specify/` + файлов интеграции; риск перезаписи существующих файлов **низкий** (имена `speckit-*` с нашими не пересекаются), но `init` в непустой проект требует `--force` — делать только после approval и бэкапа.
- **Совместимость с OpenCode 1.18.31 (V1):** skills-режим совместим концептуально (V1 читает `.opencode/skills/` — именно там сейчас живут наши 60+ skills). Legacy commands-режим для нас бесполезен.
- **Частичное использование:** да — `constitution`, `specify/plan/tasks`, `implement/converge`, `bug-assess/fix/test` можно вызывать как процедуры вручную, без `init`.
- **Workflow без замены AI_CONTEXT:** да, но с оговоркой — `.specify/specs` дублирует роль `AI_CONTEXT/PROJECT_STATE + CURRENT_TASK`. Правило: AI_CONTEXT остаётся source of truth; `.specify/` (если появится) — подчинённый рабочий каталог одной фичи.
- **Конфликты с текущими skills:** прямых коллизий имён нет (`speckit-*` уникальны); шум discovery (+10 skills) и методологический конфликт (два spec-процесса) — средний.

**SPEC_KIT: ADAPT.** Причина: полный `init` даёт больше дублирования (второй spec-центр, +10 skills), чем пользы на текущем checkpoint (frozen Stage + art-ветка). Взять механизмы: `constitution` → сверка с `RULES.md`, `converge` (implement → converge до «Converged») → усиление нашего COMPILE→TEST→CONFIRMED, `bug-assess → fix → test` → формализация багфиксов. Без установки и без `init` до отдельного решения.

---

## Superpowers

Источник: https://github.com/obra/superpowers (~291k stars, MIT). OpenCode-интеграция: V1 `"plugin": [...]`, V2 (≥2.0.4) `"plugins": [...]`, установка через `Fetch and follow .../.opencode/INSTALL.md`. Наш конфиг plugin-ключа **не содержит** — не установлен, конфликтов сейчас нет.

Все 11 запрошенных skills существуют (плюс `finishing-a-development-branch`, `diagnosing-superpowers`, `writing-skills`, `using-superpowers`):

| Skill | Полезность для WWG | Возможный конфликт | Рекомендация |
|---|---|---|---|
| brainstorming | Средняя (дизайн уже зафиксирован AI_CONTEXT; полезен для новых Stage) | Пересечение с фазой АНАЛИЗ→ПЛАН | ADAPT процедуру, не ставить plugin |
| writing-plans | Средняя (наш ПЛАН→СОГЛАСОВАНИЕ уже есть; bite-size tasks полезны) | Дублирование формата плана | ADAPT формат задач |
| executing-plans | Средняя | Пересечение с ИЗМЕНЕНИЕ-фазой | ADAPT по необходимости |
| dispatching-parallel-agents | Низкая сейчас (один исполнитель, нет git-изоляции) | Нет | LATER (после git + нескольких агентов) |
| subagent-driven-development | Средняя (идея review после каждой задачи ценна) | Наш Task-инструмент уже умеет сабагентов; дублирование процесса | ADAPT идею post-task review |
| test-driven-development | Низкая-средняя (Unity-проект: тесты через Unity Test Framework, не RED-GREEN по умолчанию; пересечение с `tdd-workflow`) | Концептуальный дубль `tdd-workflow` | SKIP как skill; оставить наш `tdd-workflow` |
| systematic-debugging | **Высокая** (4-phase root cause ≈ наш evidence-based подход) | Нет прямого | **ADAPT в первую очередь** (процедура в AI_CONTEXT) |
| verification-before-completion | **Высокая** (прямое усиление «Implemented≠Confirmed») | Нет, полное согласие с RULES.md | **ADAPT в первую очередь** |
| requesting-code-review | Средняя (покрывается Gemini-reviewer, см. §Gemini CLI) | Нет | ADAPT чек-лист |
| receiving-code-review | Низкая-средняя | Нет | ADAPT при появлении внешнего reviewer |
| using-git-worktrees | Заблокирована: **проект не git-репозиторий** | Нет (неприменимо) | SKIP до инициализации git |

Особое внимание: `verification-before-completion` и `systematic-debugging` — полностью совместимы с evidence-based verification WWG (статусы implemented/compiled/tested/confirmed + запрет считать готовым без пользователя). Портировать как текст, не как plugin: plugin тянет ещё 13 skills, хук `using-superpowers`,.bootstrap каждой сессии и риск Windows-проблем с git-backed install — избыточно для извлечения двух процедур.

---

## Gemini CLI

Источник: https://github.com/google-gemini/gemini-cli (~107k stars, Apache 2.0).

- **Установлен:** нет. **Версия:** UNKNOWN (нечего проверять). **Запуск:** `npx @google/gemini-cli` или `npm install -g @google/gemini-cli` (Node 24 подходит).
- **Аутентификация (без выполнения login):** 3 пути — Google OAuth (личный аккаунт), `GEMINI_API_KEY` (AI Studio), Vertex AI (enterprise). Для reviewer-роли достаточно OAuth или API-ключ после approval.
- **Бесплатные лимиты (официальные, на 2026):** OAuth — 60 запросов/мин и 1000 запросов/день; API-key tier — 1000 запросов/день (mix flash/pro). Достаточно для точечных review, недостаточно для основного исполнителя — что и требуется.
- **MCP support:** есть (конфиг `~/.gemini/settings.json`), т.е. reviewer при желании может читать те же MCP-источники.
- **Независимый reviewer:** да — `gemini -p "<prompt>" --output-format json` (headless), скриптуемо, дёшево, отдельная модель/провайдер от основного исполнителя.
- **Только audit/verification:** да, через `.geminiignore` + узкие промпты (передавать файл/дифф, а не весь проект) + запуск вручную или по правилу после COMPARISON.
- **Передача данных наружу:** **да, требует** — промпты уходят в Google Cloud. Исходники целиком слать нельзя; передавать только ревьюируемый фрагмент. Категория риска HIGH при неосторожном использовании, LOW при узких скоупах (см. Security Matrix).

**Безопасная архитектура:** OpenCode (OmniRoute/LM Studio, локальные MCP, файлы) = единственный писатель кода. Gemini CLI = читающий reviewer без прав на запись (запуск из копии/диффа, без shell-прав на проект, секреты не передавать, `.geminiignore` исключает `Library/`, `Builds/`, ключи). Login/ключи — только после human approval.

---

## n8n

Источник: https://github.com/n8n-io/n8n (self-hosted Community Edition доступен; лицензия fair-code/Sustainable Use — не OSI; Business/Enterprise-фичи по ключу с ежедневным пингом лицензионного сервера; встроенный MCP-сервер instance-level + ноды MCP Server Trigger / MCP Client, зрело с v1.88+/2.x; GitHub-интеграция, webhook, filesystem, HTTP, scheduling, notifications — всё штатно из коробки).

- **Зачем:** оркестрация рутины вокруг кода (нотификации о сборках, scheduled проверки, webhook-хуки, склейка GitHub↔мессенджер), позже — вызов WWG-воркфлоу как MCP-инструментов из OpenCode.
- **Что автоматизировал бы для WWG:** отчёты о Play-тестах, напоминания о неподтверждённых Stage, сборка CHANGELOG-дайджестов, мост GitHub Issues ↔ AI_CONTEXT. Ничего из этого не горит на frozen-checkpoint.
- **Self-host:** да, но на машине **нет Docker** — установка = Docker Desktop + БД + обслуживание;Telemetry по умолчанию (отключаемая).
- **Ограничения:** community-версия достаточна для старта; лимиты касаются в основном коллаборации/SSO/энвайронментов. История критических CVE 2025–2026 (RCE через expressions/webhook) требует least-privilege и закрытого доступа — весомый аргумент не поднимать раньше времени.

**Рекомендация: LATER.** Причина: нет Docker, нет git/GitHub-плеча (нечего оркестрировать), нет горящих периодических задач. Пересмотреть после: git init + GitHub + первая повторяющаяся рутина.

---

## Dagger

Источник: https://github.com/dagger/dagger (Apache 2.0, programmatic CI/CD-конвейеры на Go/Python/Node; концепция «конвейер как код»).

- **Unity build/test:** сам Dagger Unity не собирает; связка — Dagger + GameCI (`game-ci/unity-builder`) или проект Dirk (`bardic/Dirk`: DaggerCI+GameCI, Unity 6000 упоминается). Работоспособно, но требует написания модуля и Unity-лицензии (ULF/serial в secrets).
- **Windows:** поддержка есть через раннеры/контейнеры, но на практике Windows+Unity-сборки в контейнерах — самый болезненный кейс (GPU, лицензия, вес образов).
- **Docker:** **обязателен** (движок контейнеров) — отсутствует на машине. Без Docker Dagger не запускается вообще.
- **Бесплатность/self-host:** да, open source, свой раннер.
- **Нужность сейчас:** низкая — релизных сборок нет (checkpoint frozen, `Builds/` локальные), тестовый контур = `dotnet build` + ручной Play-тест по RULES.md.

**Рекомендация: LATER (после n8n, по факту — последним).** Причина: нет Docker, нет CI-триггеров (нет git), ручной контур покрывает текущие нужды. Когда появится git + GitHub — сначала оценить **чистый GameCI** (unity-builder/unity-test-runner) без Dagger-прослойки: меньше движущихся частей на Windows.

---

## Vibe Kanban

Источник: https://github.com/BloopAI/vibe-kanban.

- **OpenCode support:** заявлен (рядом с Claude Code, Codex, Gemini CLI и др.).
- **Gemini support:** заявлен.
- **Worktrees / parallel agents:** ядро продукта — kanban + автоматические `git worktree` + параллельные агенты + diff-review + PR.
- **Активность:** **проект официально sunsetting** (баннер в README репозитория). Активной разработки, патчей безопасности и гарантии совместимости с новыми версиями агентов нет.
- **Windows:** десктоп-приложение (Rust/React); работало на Windows при жизни проекта, но теперь без поддержки.
- **Нужность для WWG:** нулевая при текущих фактах: требует git-репозиторий (отсутствует), даёт оркестрацию поверх агентов (у нас один исполнитель), дублирует согласование, которое уже живёт в AI_CONTEXT.

**Рекомендация: SKIP.** Причина: sunset-статус (неприемлемо для production-зависимости) + жёсткое требование git + отсутствие выгоды на текущем масштабе. Архитектурные идеи (worktree-изоляция, diff-review loop) зафиксировать текстом на будущее, когда появятся git и второй агент.

---

## Additional Candidates

Короткий поиск, без установок. Звёзды — порядок величины на момент аудита (где точное число не снималось — UNKNOWN, проверяется после approval).

| # | NAME | GITHUB | PURPOSE | STARS ~ | LAST ACTIVITY | LICENSE | LOCAL/PAID | OpenCode compat | WWG relevance | RECOMMENDATION |
|---|---|---|---|---|---|---|---|---|---|---|
| 1 | GitHub CLI (`gh`) | github/cli | Git-операции, PR, Issues из терминала | ~40k (UNKNOWN точно) | активен | MIT | local/free | indirect (вызов из bash) | Высокая — разблокирует git-будущее, `opencode pr` без него неполноценен | INSTALL AFTER APPROVAL (с git init) |
| 2 | MCO (Multi-CLI Orchestrator) | mco-org/mco | Параллельный fan-out задач в Claude/Codex/Gemini/OpenCode/Qwen, dedup, SARIF/PR-репорты | UNKNOWN | активен (2026) | UNKNOWN (проверить) | local/free (+API-ключи агентов) | native (OpenCode — провайдер) | Средняя — готовый «второй reviewer» конвейер после установки Gemini | LATER |
| 3 | IvanMurzak/Unity-MCP | IvanMurzak/Unity-MCP | Альтернативный Unity MCP (70+ tools, skills, CLI) | UNKNOWN | активен | UNKNOWN (проверить) | local/free | MCP-совместим | Средняя — только как запасной вариант, НЕ параллельно с CoplayDev | SKIP (держать в уме; конфликт порта 8080) |
| 4 | blender-ai-mcp | PatrykIti/blender-ai-mcp | Production-MCP для Blender (goal-routing, asserts, vision) | низкая-средняя | активен | UNKNOWN (проверить) | local/free | MCP-совместим | Низкая-средняя — интересно для character-QA, но смена аддона рискованна | SKIP (переоценить при упоре в character-art) |
| 5 | Blender Official MCP Server | blender.org/lab/mcp-server | Официальный лёгкий MCP-сервер Blender | н/д | активен (Blender 5.1+) | UNKNOWN (проверить) | local/free | MCP-совместим | Низкая — дождаться зрелости, текущий стек работает | SKIP |
| 6 | kodo | ikamensh/kodo | Overnight multi-agent оркестрация + независимая верификация (SWE-bench 57%) | UNKNOWN | активен | MIT | local (подписки агентов) | indirect (вызывает CLI агентов) | Низкая сейчас — требует бэкенды (Claude Code/Cursor/Codex/Gemini), которых нет | SKIP/LATER |
| 7 | GameCI unity-builder + test-runner | game-ci/unity-builder, game-ci/unity-test-runner | Unity-сборки/тесты в CI без Dagger-прослойки | ~популярны | активен | MIT | local/free (свои раннеры) | indirect | Средняя на будущее (Android/Windows сборки) | LATER (первым кандидатом на CI, раньше Dagger) |
| 8 | czlonkowski/n8n-mcp | czlonkowski/n8n-mcp | Community MCP-сервер: учит AI-агента строить корректные n8n-воркфлоу (1800+ нод) | UNKNOWN | активен | MIT | local/free | MCP-совместим | Низкая — имеет смысл только вместе с n8n | LATER (связкой с n8n) |
| 9 | lazygit | jesseduffield/lazygit | TUI для git (дешёвая замена kanban-доскам на старте) | ~60k+ (UNKNOWN точно) | активен | MIT | local/free | indirect | Средняя — после git init | LATER (с git init) |
| 10 | uv | astral-sh/uv | Python package/инструмент-менеджер (уже стоит; им же ставится `specify`) | ~60k+ (UNKNOWN точно) | активен | MIT/Apache | local/free | indirect | Средняя — гигиена Python-окружения (Blender-скрипты, MCP-утилиты) | ADAPT (использовать для будущих Python-установок; уже есть) |
| 11 | Orcy | waterworkshq/orcy | MCP-native task-оркестрация (atomic claiming, quality gates) | UNKNOWN | активен | UNKNOWN (проверить) | local | MCP-совместим | Низкая — overhead для одного исполнителя | SKIP |
| 12 | OrchestrAI | musaceylan/OrchestrAI | Role-based routing (Planner/Coder/Tester/Reviewer/Judge) по моделям + privacy tiers | UNKNOWN | 2026 | UNKNOWN (проверить) | local+cloud | MCP-совместим | Низкая — интересно идеей `secret`-tier, но продукт незрелый | SKIP |

Исключены сознательно (нет интеграционной ценности для WWG сейчас): generic MCP-хабы (autoteam, agent-workflow-mcp, central-mcp, AIOrc) — решают несуществующую проблему мульти-агентности; VibeSync (Unity↔Blender live-sync, experimental v0.4) — незрелый и ломает ручной QA-контур; песок визуального QA покрыт Blender-скриншотами.

---

## Compatibility Matrix

| Инструмент | OpenCode 1.18.31 (V1) | Node 24 | Python 3.14 | Windows | Unity 6000.3 | Blender MCP (текущий) | AI_CONTEXT workflow | Итог |
|---|---|---|---|---|---|---|---|---|
| Superpowers (plugin) | OK (ключ `plugin`) | OK | н/д | Риск (git-backed install, задокументирован обход) | н/д | н/д | Частичное дублирование | ADAPT без plugin |
| Spec Kit (`--skills`) | OK (`.opencode/skills/`) | н/д (нужен uv — есть) | Требуется 3.11+ (3.14 OK) | OK | н/д | н/д | Дублирование spec-центра | ADAPT без init |
| Gemini CLI | indirect (вызов из bash, не plugin) | OK (npm) | н/д | OK | н/д | н/д (свой MCP-конфиг) | Усиливает (reviewer) | INSTALL после approval |
| GitHub CLI | indirect | OK | н/д | OK | н/д | н/д | Разблокирует git-будущее | INSTALL с git init |
| n8n | indirect (MCP-клиент/триггер) | н/д (свой раннер) | н/д | Требует Docker (нет) | н/д | н/д | Нечего оркестрировать | LATER |
| Dagger | indirect | OK (SDK) | OK | Больно (контейнеры+Unity) + нет Docker | Через GameCI | н/д | Нечего собирать в CI | LATER (после GameCI-оценки) |
| Vibe Kanban | Заявлен | н/д | н/д | Без поддержки (sunset) | н/д | н/д | Требует git (нет) | SKIP |
| MCO / kodo / Orcy | indirect | OK | mixed | OK | н/д | н/д | Overhead сейчас | LATER/SKIP (см. таблицу) |
| IvanMurzak/Unity-MCP | Конфликт транспорта (порт 8080) | н/д | н/д | OK | OK | н/д | Замена, не дополнение | SKIP (запасной) |
| GameCI (без Dagger) | indirect | н/д | н/д | Раннеры существуют | OK (6000.x в матрицах) | н/д | Будущий CI | LATER |

---

## Security Matrix

Шкала: LOW / MEDIUM / HIGH / UNKNOWN. Отдельно фиксируется, может ли инструмент отправить исходники наружу.

| Инструмент | Local exec | Cloud dependency | API keys | Telemetry | Code upload | FS access | Shell access | MCP access | Git access | Destructive actions | Итог |
|---|---|---|---|---|---|---|---|---|---|---|---|
| Superpowers (plugin) | YES (промпты+хуки) | Нет (свой код) | Нет своих | Opt-out флаг `SUPERPOWERS_DISABLE_TELEMETRY` (компаньон тянет лого с сайта) | Нет (но агент с ним может) | Через агента | Через агента | Через агента | `using-git-worktrees` (н/п без git) | Средний (агент правит код по методологии) | MEDIUM |
| Spec Kit | YES (`specify` CLI) | Нет | Нет | Нет данных (UNKNOWN) | Нет | Шаблоны в проект | Нет своего | Нет | Нет | Низкий (генерация md) | LOW |
| Gemini CLI | YES | **YES (Google Cloud, обязательно)** | OAuth/API key | Да (Google, управляется политикой) | **Да — весь промпт уходит в облако** | Да (file tools) | Да | Да (свой конфиг) | Да | Высокий при широких правах → запускать scoped, read-only | HIGH (управляемый до LOW узкими скоупами) |
| n8n | YES (self-host) | Лицензионный пинг (Business+) | Хранит секреты воркфлоу | По умолчанию вкл (откл.) | При облачных нодах — да | Да (FS-нода) | Да (Code-нода) | Да (MCP-ноды/сервер) | Да | Высокий (RCE-история CVE-2025/2026) → least-privilege, закрыть от интернета | HIGH (до установки — теоретический) |
| Dagger | YES | Нет (образы тянутся, как везде) | Secrets-механизм (скраб логов) | UNKNOWN | Нет (локальные контейнеры) | Через контейнеры | Через контейнеры | Нет своего | Да (checkout) | Средний (сборки, публикация образов) | MEDIUM |
| Vibe Kanban | YES | Relay-туннель (опц.) | Ключи агентов в env | UNKNOWN (sunset — патчей нет) | Через агентов | Через агентов | Через агентов | Через агентов | Да (worktrees) | Высокий (параллельные агенты пишут код) + неподдерживаемость | HIGH → SKIP |
| MCO/kodo | YES | Через провайдеров (Anthropic/OpenAI/Google) | Ключи каждого CLI | UNKNOWN | Да (промпты провайдерам) | Через агентов | Да | Нет своего | Да | Высокий (kodo: `bypassPermissions`) | HIGH |
| GameCI | YES (раннер) | Нет | Unity-лицензия (ULF/serial) | Нет | Нет | Да | Да | Нет | Да | Средний (сборки, артефакты) | MEDIUM |
| GitHub CLI | YES | GitHub API (только по команде) | `gh auth` токен | Нет | Только указанные репозитории/файлы | Read по команде | Нет своего | Нет | Да | Средний (push/PR по явной команде) | LOW-MEDIUM |
| Текущий стек (OpenCode+OmniRoute/LM Studio+MCP) | YES | Зависит от провайдера (OmniRoute-гейтвей — владелец/политика UNKNOWN, уточнить) | `{env:OMNIROUTE_API_KEY}` | UNKNOWN (провайдер) | Зависит от провайдера | Да | Да | Да | Нет (не репозиторий) | Контролируется RULES.md + approval | MEDIUM (снизить, прояснив OmniRoute) |

Вывод: единственный кандидат, который **по построению** отправляет данные наружу — Gemini CLI (и любые мульти-провайдерные оркестраторы). Допустим только в reviewer-роли с узким скоупом. Superpowers/Spec Kit сами по себе ничего не отправляют. n8n/Dagger держать за периметром до появления Docker и явной нужды.

---

## Conflicts

Карта конфликтов:

```
OpenCode 1.18.31 (V1)
├── existing skills (60+, 5 корней: проект + 4 global paths)
│   └── прямых коллизий ID нет; концептуальные дубли: tdd-workflow,
│       structured-project-execution, multi-agent-orchestration
├── Superpowers (НЕ установлен)
│   ├── +15 skills через plugin-хук (не через файлы) — ID не пересекаются,
│   │   но discovery-шум и два «центра методологии»
│   ├── требует ключ "plugin" в opencode.json (сейчас отсутствует — плюс:
│   │   установка не заткнёт существующее; минус: Windows git-install риск)
│   └── using-git-worktrees/finishing-a-development-branch — МЁРТВЫ без git
├── Spec Kit (НЕ установлен)
│   ├── legacy-режим пишет .opencode/commands/ (мусор для V1-discovery)
│   ├── skills-режим пишет .opencode/skills/speckit-*/ (+10 к discovery)
│   ├── .specify/ — второй spec-центр против AI_CONTEXT
│   └── specify init --force в непустой проект — только с бэкапом
├── MCP
│   ├── unityMCP :8080 DOWN — любой второй Unity-MCP = конфликт порта
│   ├── blenderMCP :9876 UP — любой второй Blender-MCP = конфликт аддона/порта
│   └── n8n MCP-сервер (будущий) — отдельный порт, ок; но токены без per-client scope
├── Gemini (НЕ установлен)
│   └── свой ~/.gemini/settings.json + ключи — изоляции от opencode.json нет
│       по умолчанию; разводить руками (разные файлы — уже разведены)
└── custom WWG workflow (AI_CONTEXT)
    └── инвариант: подтверждение только от пользователя; любой инструмент,
        заявляющий «done» сам (converge-автоматы, overnight-оркестраторы),
        нарушает RULES.md, пока его вердикт не подчинён human approval
```

Таблица конфликтов:

| Пара | Тип конфликта | Серьёзность | Разрешение |
|---|---|---|---|
| Superpowers ↔ existing skills | Дубли методологий (TDD, plans, review) | Средняя | Не ставить plugin; портировать 2–4 процедуры текстом |
| Spec Kit ↔ AI_CONTEXT | Два spec-центра (`.specify/` vs `AI_CONTEXT/`) | Средняя | AI_CONTEXT = source of truth; `.specify/` только как подчинённый (после approval) |
| Spec Kit legacy ↔ OpenCode discovery | Мёртвые `commands/`-файлы | Низкая | Использовать только `--skills`-режим, если вообще init |
| Superpowers ↔ Windows/Bun | git-backed install может не встать | Средняя | Обход через локальный npm-путь (задокументирован); или не ставить |
| Второй Unity-MCP ↔ CoplayDev :8080 | Порт/транспорт | Высокая | Один Unity-MCP за раз; держать CoplayDev |
| Второй Blender-MCP ↔ текущий :9876 | Аддон/порт | Высокая | Один Blender-MCP за раз; держать текущий |
| kodo/MCO ↔ RULES.md | Авто-коммиты, bypassPermissions | Высокая | Не ставить; reviewer-модель только через Gemini-scope |
| Vibe Kanban ↔ отсутствие git | Неприменимость | Блокирующая | SKIP |
| Dagger/n8n ↔ отсутствие Docker | Неустанавливаемость | Блокирующая | LATER (сначала Docker, по отдельной заявке) |
| Любой авто-verifier ↔ «Confirmed только пользователем» | Методологический | Высокая | Вердикт инструмента = «candidate», Confirmed ставит человек |

---

## Recommended Architecture

```
┌─────────────────────────────────────────────────────────┐
│ OpenCode 1.18.31 (V1) — ЕДИНСТВЕННЫЙ ИСПОЛНИТЕЛЬ        │
│ config: opencode.json (без изменений)                   │
│ skills: существующие 60+ (project wwg-character-art     │
│         имеет приоритет; global — поддержка)            │
│ procedures: +systematic-debugging, +verification-before-│
│ completion, +speckit-converge/bug-assess (текстом в     │
│ AI_CONTEXT, после approval)                             │
└──────┬──────────────────────────────┬───────────────────┘
       │ MCP (существующие, без смен) │
       ├─ blenderMCP :9876 (UP) → Blender → assets → QA   │
       │   (NUMERIC+VIMSION PASS, ART_PIPELINE.md)        │
       └─ unityMCP :8080 (по требованию) → Unity 6000.3   │
           (CoplayDev; запускать Editor по сессии)        │
┌─────────────────────────────────────────────────────────┐
│ Gemini CLI — НЕЗАВИСИМЫЙ REVIEWER (read-only, scoped)   │
│ запуск: вручную, диффом/файлом; без shell-прав на проект│
│ вердикт: candidate → Confirmed ставит человек           │
└─────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────┐
│ GitHub-плечо (после git init): gh + lazygit (опц.)      │
│ CI-плечо (позже): чистый GameCI → (если мало) Dagger    │
│ Автоматизация (позже): n8n self-hosted + Docker         │
└─────────────────────────────────────────────────────────┘
Инварианты: AI_CONTEXT = source of truth; один MCP одного типа за раз;
V1 остаётся (миграция на V2 не нужна); ничего не ставится без approval.
```

---

## Installation Order

Строго после человеческого подтверждения каждого пункта, по одному за раз, с проверкой:

1. `git init` в `C:\Users\cyril\WildWestGunslinger` + `.gitignore` (Unity-стандарт: `Library/`, `Builds/`, `Logs/`, ключи) + первый коммит. Предпосылка для всего остального git-будущего. (Без этого пункты 2–3 бессмысленны.)
2. GitHub CLI (`gh`) + `gh auth login` (интерактивно, человеком) — разблокирует `opencode pr`, Issues, будущий CI.
3. Gemini CLI (`npm install -g @google/gemini-cli`) + `.geminiignore` + reviewer-промпт-шаблон в `AI_CONTEXT/` + пробный review одного файла. Login — человеком.
4. (Позже, отдельной заявкой) Docker Desktop → n8n self-hosted (первый воркфлоу: нотификации/дайджесты) → GameCI-оценка → Dagger только если GameCI мало.
5. Процедурные порты (без установок, правкой MD после approval): systematic-debugging + verification-before-completion + speckit-converge/bug-assess в `RULES.md`/`README.md`.

Откат каждого пункта: удаление бинарника/расширения + откат MD-правки; конфиг OpenCode при пунктах 1–3 не затрагивается вообще.

---

## What Must NOT Be Installed

- **Vibe Kanban** — sunset + требует git; архитектурно не нужен.
- **Второй Unity-MCP** (IvanMurzak и др.) параллельно с CoplayDev — конфликт порта/транспорта; держать текущий.
- **Второй Blender-MCP** (PatrykIti, официальный Blender Lab) параллельно с текущим — конфликт аддона/порта; держать текущий до зрелости альтернатив.
- **Superpowers как plugin** — тянет 15 skills, хуки, Windows-риски; нужное забирается текстом.
- **Spec Kit через `specify init`** — второй spec-центр и мусорные `commands/`; нужное забирается текстом.
- **kodo / MCO / Orcy / OrchestrAI / MCP-хабы** — мульти-агентный overhead и `bypassPermissions`-риски при одном исполнителе.
- **Dagger / n8n** — до появления Docker и первой реальной периодической задачи (сейчас нечего оркестрировать/собирать).
- **Миграция OpenCode V1 → V2** — не требуется ни одним из рекомендованных пунктов; V1 поддерживается всеми нужными механизмами.

---

## Human Approval Required

1. Разрешение на `git init` + стандарт Unity `.gitignore` (необратимо в смысле истории — делать один раз, правильно).
2. Разрешение на установку GitHub CLI + интерактивный `gh auth login` (человеком, не агентом).
3. Разрешение на установку Gemini CLI + способ аутентификации (OAuth vs API key) + согласие с передачей ревьюируемых фрагментов в Google Cloud + scope-правила (что слать нельзя).
4. Разрешение на текстовые правки `AI_CONTEXT` (портированные процедуры Superpowers/Spec Kit) — построчно показать дифф.
5. Разрешение на любые будущие пункты раздела Installation Order п.4 (Docker, n8n, GameCI, Dagger) — каждый отдельной заявкой.
6. Уточнить владельца/политику OmniRoute (`localhost:20128`, `{env:OMNIROUTE_API_KEY}`) — от этого зависит cloud-риск основного исполнителя.

---

## ФИНАЛЬНАЯ РЕКОМЕНДАЦИЯ

### INSTALL AFTER APPROVAL

- **Gemini CLI** — независимый reviewer (headless `-p`, JSON-вывод, scoped промпты, `.geminiignore`). Единственный инструмент, дающий новую способность (второе мнение другой модели), а не дубль существующего.
- **GitHub CLI (`gh`)** — предпосылка git-будущего; строго связкой с `git init` (по отдельности бесполезны).

### ADAPT / INTEGRATE

- Из **Superpowers**: `verification-before-completion`, `systematic-debugging` (первая очередь); `requesting-code-review` (чек-лист для Gemini-reviewer), `brainstorming`/`writing-plans`/`executing-plans`/`subagent-driven-development` (идеи, не целиком) — текстом в `AI_CONTEXT`, без plugin.
- Из **Spec Kit**: `constitution` (сверка с RULES.md), цикл `implement → converge` (усиление COMPILE→TEST→CONFIRMED), `bug-assess → fix → test` (формализация багфиксов) — как процедуры, без `specify init`.
- Из **Vibe Kanban**: только идеи worktree-изоляции и diff-review loop — на будущее с git.
- **uv** — уже есть; использовать как стандартный установщик Python-инструментов (включая гипотетический `specify`).

### SKIP

- **Vibe Kanban** (sunset, требует git, нет выгоды).
- **Superpowers plugin / Spec Kit init** как установки (дубли, шум, риски — брать только процедуры).
- **Dagger, n8n** — сейчас (нет Docker, нет задач); пересмотр по триггерам из Installation Order.
- **kodo, MCO, Orcy, OrchestrAI, MCP-хабы, VibeSync** — overhead/риски при одном исполнителе.
- **Альтернативные Unity/Blender MCP** — держать текущие; переключение только при доказанной необходимости.
- **Миграция OpenCode на V2** — не нужна.

---

## PHASE 2 RESULTS

Дата исполнения: 2026-09-25. Разрешены были только пункты Phase 2 (Git, gh, Gemini install-only, драфты документов).
Запрещённое (Superpowers/Spec Kit целиком, OpenCode V2, вторые MCP, Vibe Kanban, n8n, Dagger, Docker,
оркестраторы, доп. MCP) — не ставилось. Конфиг OpenCode, MCP, Unity, Blender, skills — не менялись.

| Компонент | STATUS | Факт проверки |
|---|---|---|
| Git | INSTALLED (был) + repo инициализирован | `git --version` = 2.55.0.windows.3; `git init -b main` выполнен 2026-09-25 (approval получен); `.git/` создан; коммит НЕ делался |
| `.gitignore` | ADAPTED (создан) | Корневой `.gitignore` (Unity-стандарт + `*.env`, `*.ulf`, `/.utmp/`); `git status` = 8 untracked, утечек Library/Builds/Logs/Temp/UserSettings/.vs/obj нет |
| GitHub CLI | INSTALLED (без auth) | `winget install --id GitHub.cli` (approval получен); `gh --version` = 2.101.0 (2026-09-15), путь `C:\Program Files\GitHub CLI\gh.exe`; `gh auth login` НЕ выполнялся (остаётся за человеком); в новых shell подхватится через PATH |
| Gemini CLI | INSTALLED (без login, без отправок) | `npm install -g @google/gemini-cli` (approval получен, install-only); `gemini --version` = 0.61.0; login/smoke-тест НЕ выполнялись; замечание: npm предупредил про keytar install-script (allowScripts) — влияет только на хранение credentials при будущем login, решается человеком |
| WWG verification workflow | WAITING FOR APPROVAL | Драфты `VERIFICATION.md`, `DEBUGGING.md` показаны пользователю, создание — после отдельного ок |
| Spec Kit adaptations | WAITING FOR APPROVAL | Драфт `SPEC_IDEAS.md` показан пользователю, создание — после отдельного ок |

Проверки, подтвердившие статусы: `git --version`, `git status --short` (+leak-check по паттернам),
`gh --version` (полным путём), `gemini --version`. Исходники проекта наружу не отправлялись.
Destructive actions не выполнялись (единственные записи на диск: `.git/`, `.gitignore`, этот раздел).
