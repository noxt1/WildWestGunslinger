> **Status:** OPERATIONAL / BINDING RULES - reconciled 2026-09-29 (Phase 4)
> **Role:** **authoritative for how an agent may work on this project.** These are operational rules and are deliberately NOT duplicated into PROJECT_TRUTH.
> **Canonical index of these rules:** ../Documentation/GAME/GAME-0004-REQUIREMENTS-AND-DESIGN-DECISIONS.md section 5.1 (OR-01 .. OR-12)
> **Protected-file list:** section 5.2 of the same document. Note CONFLICT-16: three listed files are already modified in the working tree.
> **Canonical project state:** ../Documentation/PROJECT_STATE.md
> **Reconciliation:** ../Documentation/PHASE4_AI_CONTEXT_RECONCILIATION.md
> Superseded as canonical project state on 2026-09-29. Retained as the **operational rules** source.

# RULES — Постоянные правила работы с проектом

## Основные принципы

### 1. Не ломать работающее

- **Не переделывать** работающий код без обоснованной необходимости.
- **Не удалять** существующую логику без явного разрешения пользователя.
- **Не перезаписывать** файлы целиком, если можно внести точечные изменения.

### 2. Уважать подтверждённые Stage

- **Stage 1** — CONFIRMED. Не изменять логику интеграции `EnemyTacticalPlanner` с `EnemyController` без необходимости.
- **Stage 2** — CONFIRMED. Не изменять A* навигацию без необходимости.
- **Stage 3** — NOT CONFIRMED. Ожидает тестирования в Unity.

### 3. Явное согласование

- **Не создавать** новые AI-состояния (enum AIState) без согласования.
- **Не создавать** новые скрипты без согласования.
- **Не изменять** файлы за пределами разрешённого списка без согласования.
- **Не переделывать** A* навигацию без согласования.

### 4. Не дублировать

- **Не создавать** дубликаты существующих классов.
- **Не создавать** новые скрипты, если функционал можно добавить в существующие.
- **Проверять** наличие похожих систем перед созданием нового кода.

### 5. Точность данных

- **Не записывать** предположения как факты.
- **Отделять** "implemented", "compiled", "tested", "confirmed".
- **Указывать** точные ссылки на файлы и строки при описании изменений.

### 6. Blender / character-art

- Для Blender / character-art агент обязан учитывать `.opencode/skills/wwg-character-art/SKILL.md` и `AI_CONTEXT/ART_PIPELINE.md` и не делать assumptions, противоречащих им.
- Не считать подготовленный asset подтверждённым без пользователя. Не менять Stage-checkpoint из-за art-ветки.

### 7. Blender 3D assets — обязательные правила
- **Метод C (Component / Controlled Fracture).** Конструктивная сборка из реальных элементов. Cell Fracture, Voronoi-fracture, add-ons и physics simulation **запрещены**.
- **Vertex attribute только `FLOAT_COLOR`.** `BYTE_COLOR` хранит sRGB и возвращает записанное линейное значение примерно в 12 раз темнее — использовать нельзя.
- **Семантика `Col`:** RGB = тон элемента, A = metal mask (0 = wood, 1 = metal).
- **Один material slot** на каждый объект. Image textures на текущем этапе запрещены (M2 = vertex attributes + simple PBR).
- **Presentation state destructible-assets:** `<Name>_Fragments.hide_viewport = TRUE`, `hide_render = FALSE`, intact видим. Одновременное отображение intact и reassembled fragments на совпадающих поверхностях вызывает «дёрганье» wood surface (подтверждённый root cause, не гипотеза). Fragments не удалять и не исключать — нужны для exploded QA и FBX export.
- **FBX export:** frozen-контракт из `ART_PIPELINE.md`. Только explicit selection; `use_visible` не использовать; существующие FBX не перезапитывать и не удалять — новое имя на каждый экспорт.
- **Каждый CP завершается отчётом и ждёт явной авторизации пользователя на следующий CP.** Начинать следующий CP молча нельзя.

### 8. Unity integration environment assets
- **Поштучная Unity-интеграция запрещена.** Workflow: `Blender model → Blender QA → fracture preparation → final FBX → [ПАКЕТНЫЙ ЭТАП] Unity integration`.
- Один отдельный пакетный этап для всех готовых assets (FBX import, normals/smoothing, materials, vertex colors, colliders, prefabs, destructible setup, runtime destruction, gameplay integration, performance validation) — после завершения набора моделей и отдельного решения пользователя.
- **Asset FINAL ≠ повод начинать Unity-интеграцию.**

### 9. Модульный environment kit — производительность
- **НЕ создавать «1 log = 1 GameObject» и «1 floorboard = 1 GameObject»** при обычной процедурной генерации. Визуальная конструкция может содержать много элементов, но runtime использует небольшое число переиспользуемых mesh-модулей (`WallSegment ≈ 4 m`, `FloorSegment ≈ 4 × 4 m`): 1 mesh → 1 MeshRenderer → 1 shared material → 1 simple collider.
- Генератор **размещает** модули; коридоры переиспользуют то же семейство.
- Android performance — ограничение с самого начала: переиспользуемые меши, shared материалы, низкое число Renderer/GameObject, простые коллайдеры, контролируемая плотность треугольников, instancing/batching.
- Визуальное богатство — из модульности, силуэта, материала и контролируемой вариации, **не** из роста числа объектов.
- `ArenaGenerator` **не переписывать** до settled material и modular-kit design.

## Workflow

### Перед изменением

1. Прочитать `AI_CONTEXT/README.md`.
2. Прочитать `AI_CONTEXT/CURRENT_TASK.md`.
3. Прочитать `AI_CONTEXT/CONFIRMED_STATE.md`.
4. Прочитать `AI_CONTEXT/ARCHITECTURE.md`.
5. Изучить затрагиваемые файлы в коде.
6. Найти точные методы/поля/строки.

### При планировании

1. Описать что будет изменено.
2. Указать затрагиваемые файлы.
3. Показать план пользователю.
4. **Дождаться явного одобрения.**

### После изменения

1. Проверить компиляцию (`dotnet build`).
2. Если ошибки — исправить.
3. Сообщить пользователю о результатах.
3a. При триггере — Gemma review изменённых файлов (только `-f`, findings → triage → verify → fix → re-check).
3b. Findings reviewer без VERIFY дефектами не считать; NOTE закрыть явно.
4. **Не считать задачу завершённой до теста в Unity.**
5. **Дождаться подтверждения пользователя.**

### После подтверждения

1. Обновить `CONFIRMED_STATE.md`.
2. Обновить `CHANGELOG.md`.
3. Обновить `CURRENT_TASK.md`.

## Платформы

- **Android** — основная платформа. Учитывать производительность, touch-управление, ограничения мобильных устройств.
- **Windows** — вторичная платформа. Тестирование в редакторе.

## Файлы, которые НЕЛЬЗЯ изменять без явного разрешения

- `ArenaTacticalMap.cs` — A* навигация (Stage 2, CONFIRMED)
- `ArenaGenerator.cs` — процедурная генерация арены
- `ArenaModule.cs` — модули арены
- `EnemySpawner.cs` — спаун врагов
- `EnemyTacticalVision.cs` — визуальное восприятие
- `EnemyHearing.cs` — слух
- `CoverSystem.cs` — система укрытий
- `CoverPoint.cs` — точки укрытий
- `NoiseSystem.cs` — система шумов
- `PlayerController.cs` — управление игроком
- `PlayerHealth.cs` — здоровье игрока
- `WaveManager.cs` — волновая система
- `MainMenuController.cs` — главное меню
- `PauseMenuController.cs` — пауза

## Формат отчёта об изменении

```
## Изменение: [краткое описание]

### Файлы
- `EnemyTacticalPlanner.cs` — [что изменено]

### Методы
- `DistributeSearchTargets()` — добавлена проверка `IsBuilt`

### Причина
- `FindRoom()` возвращал null, т.к. `Build()` не был вызван

### Статус
- Implemented: YES
- Compiled: YES (0 errors)
- Tested: pending Unity test
- Confirmed: pending user confirmation