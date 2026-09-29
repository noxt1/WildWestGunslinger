> **Status:** OPERATIONAL / EVIDENCE RULES - reconciled 2026-09-29 (Phase 4)
> **Role:** **authoritative for what counts as evidence and what may not be self-reported.**
> **Canonical index:** ../Documentation/GAME/GAME-0004-REQUIREMENTS-AND-DESIGN-DECISIONS.md section 5.1 (OR-05, OR-06, OR-12)
> **Evidence precedence:** ../Documentation/DOC-0001-CANONICAL-DOCUMENTATION-INDEX.md section 1
> **Canonical project state:** ../Documentation/PROJECT_STATE.md
> **Reconciliation:** ../Documentation/Archive/Audits/PHASE4_AI_CONTEXT_RECONCILIATION.md
> Superseded as canonical source on 2026-09-29. Retained as the **evidence-rules** source.

# VERIFICATION — проверка перед завершением (адаптация verification-before-completion)

## Принцип
Никакая работа не считается готовой на основании слов агента. Только evidence + пользователь.

## Обязательные evidence по типам задач
- Код C#: фактическая компиляция Unity-проекта без ошибок; использовать доступный в проекте механизм сборки/компиляции и приложить его вывод.
- Поведение в Unity: Play-тест по сценарию; наблюдаемое поведение цитируется, не пересказывается.
- Визуал/Blender: numeric evidence + visual evidence (рендер/скриншот + что на нём видно). Один тип evidence без другого = не готово.
- Регрессии: что проверено, что не сломано (связанные системы из CONFIRMED_STATE).

## Запрещено
- Self-reported PASS/FINAL/DONE как основание готовности.
- Маркировки CONFIRMED, PASS, FINAL агентом. Их ставит только пользователь.
- «Проверено» без приложенного evidence.

## Формат отчёта (дополнение к формату RULES.md)
- Evidence: [команда/рендер/тест + результат]
- Статус: Implemented / Compiled / Tested / Confirmed=NO (Confirmed меняет только пользователь)

## Gemma review (независимый слой, candidate-only)
- Reviewer (`reviewer`, Gemma, filesystem-blind) запускается по триггерам: C#/bugfix/architecture — REQUIRED; Blender/Unity-visual/docs — OPTIONAL; config — NOT NEEDED.
- Вход — только `-f` с минимальным набором файлов; секреты/конфиги/MCP reviewer.md в `-f` запрещены.
- Выход — только candidate findings (BLOCKER/WARNING/NOTE с file:line); дефектом считается после VERIFY основным агентом.
- Static/runtime: текст — да; runtime/visual/архитектурные зависимости вне `-f` — UNVERIFIED до фактической проверки.
- Reviewer не заменяет compile/тесты/Play/рендеры и не ставит CONFIRMED/PASS/FINAL.
- В проверенной конфигурации reviewer tool calls отсутствуют; permissions должны оставаться deny для filesystem, edit/write, bash, task/subagent, MCP, web и skill.