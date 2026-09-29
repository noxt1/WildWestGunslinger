> **Status:** OPERATIONAL / TASK STATE - reconciled 2026-09-29 (Phase 4)
> **Role:** the **current active task, its stop point, and its approval gates**. It must never restate project reality.
> **Canonical project state:** ../Documentation/PROJECT_STATE.md · open issues ../Documentation/History/OPEN_ISSUES.md · decisions ../Documentation/DECISIONS.md
> **History of completed work:** this directory's CHANGELOG.md and ../Documentation/History/HISTORY-0001-TIMELINE.md
> **Reconciliation:** ../Documentation/PHASE4_AI_CONTEXT_RECONCILIATION.md
> Superseded as canonical source on 2026-09-29; retained as the operational task-state source.

# CURRENT_TASK — Текущая задача

## CHECKPOINT: Stage 4.4.5, FROZEN / PAUSED

Последняя подтверждённая стадия: Stage 4.4.4. Продвижение по Stage временно приостановлено — готовятся 3D dependencies (см. `PROJECT_STATE.md` → CURRENT PROJECT CHECKPOINT и `ART_PIPELINE.md`). Номер checkpoint не меняется, будущие Stage 5/8/9 не считаются выполненными.

## ТОЧКА ОСТАНОВКИ: BLENDER 3D ASSET PRODUCTION

**Точная точка остановки на 2026-09-28:**

```
CRATE_01 CP1 COMPLETE — AWAITING USER APPROVAL
```

**Не начинать CRATE_01 CP2 молча.** Fracture для Crate_01 **не авторизован**. Следующее действие — дождаться явного решения пользователя.

### Подтверждённые Blender assets

| Asset | Статус | Working .blend | Final FBX | Intact / Fragments tris |
|---|---|---|---|---|
| **Barrel_01** | **FINAL / APPROVED** | `Working\blender_src\barrel\Barrel_01_Working.blend` | `Assets\Art\Props\Destructible\Barrel_01_Intact_FINAL.fbx`, `Barrel_01_Fragments_FINAL.fbx` | 580 / 900 (12 fragments) |
| **Fence_01** | **FINAL / APPROVED** | `Working\blender_src\fence\Fence_01_Working.blend` | `Assets\Art\Props\Destructible\Fence_01_Intact_FINAL.fbx`, `Fence_01_Fragments_FINAL.fbx` | 668 / 1500 (13 fragments) |
| **Crate_01** | **CP1 COMPLETE — AWAITING APPROVAL** | `Working\blender_src\crate\Crate_01_Working.blend` | нет (CP1 без экспорта) | 740 / — (fracture не выполнялся) |

Полные числа, QA-результаты и технические решения — `AI_CONTEXT/ART_PIPELINE.md` → «Blender asset registry» и «Принятые технические решения по 3D-ассетам».

### Правило presentation state (обязательное)

```
Intact:    hide_viewport = FALSE, hide_render = FALSE
Fragments: hide_viewport = TRUE,  hide_render = FALSE
```

Подтверждённый root cause «дёрганья» wood surface: одновременное отображение intact и reassembled fragments на совпадающих поверхностях во вьюпорте. Fragments не удалять и не исключать — они нужны для exploded QA и FBX export.

## Unity integration — ТОЛЬКО ПАКЕТНЫЙ ЭТАП

Unity-интеграция этих environment-моделей **не выполняется поштучно**. Workflow:

```
Blender model → Blender QA (NUMERIC + VISION) → fracture preparation → final FBX → [ПАКЕТНЫЙ ЭТАП] Unity integration
```

Когда набор environment-моделей будет завершён, выполняется **один отдельный пакетный этап** для всех готовых assets: FBX import, normals/smoothing verification, materials, vertex colors, colliders, prefabs, destructible setup, runtime destruction, gameplay integration, performance validation.

**Нельзя** начинать Unity-интеграцию только потому, что отдельный Blender asset достиг FINAL. Asset FINAL = Blender-часть завершена и подтверждена, не более.

## НОВОЕ АРХИТЕКТУРНОЕ РЕШЕНИЕ — модульный environment kit

Цель: уйти от визуально плоских cube-стен, которые сейчас создаёт `ArenaGenerator`.

| Kit | Модуль | Содержимое одного меша | Unity-представление |
|---|---|---|---|
| **Western log/timber wall** | `WallSegment ≈ 4 m` | несколько горизонтальных брёвен, видимые швы, low-poly bevel, контролируемая вариация | 1 mesh → 1 MeshRenderer → 1 shared material → 1 simple collider |
| **Western plank floor** | `FloorSegment ≈ 4 × 4 m` | несколько досок, видимые швы, лёгкая вариация ширины и тона | 1 mesh → 1 MeshRenderer → 1 shared material → 1 simple collider |

**Критическое правило:** НЕ «1 log = 1 GameObject» и НЕ «1 floorboard = 1 GameObject». Визуальная конструкция может содержать много элементов, но runtime использует небольшое число переиспользуемых mesh-модулей. Генератор **размещает** модули. Коридоры переиспользуют то же семейство модулей.

**Performance principle:** Android — ограничение с самого начала. Предпочитать переиспользуемые меши, shared материалы, низкое число Renderer/GameObject, простые коллайдеры, контролируемую плотность треугольников, instancing/batching. Избегать сотен Renderer-ов, materials per element, уникальных мешей на повторяющийся элемент, микро-геометрии. Визуальное богатство — из модульности, силуэта, материала и контролируемой вариации, **не** из роста числа объектов.

**Material direction:** авторские WESTERN WALL и WESTERN FLOOR материалы (weathered timber / planks, тот же visual family, что Barrel_01 / Fence_01), а не перекраска placeholder `GeneratedArena_URP_Material`. Избегать чрезмерного жёлтого/оранжевого, generic плоского коричневого, шумной procedural texturing. **`ArenaGenerator` пока не переписывать** — сначала settled material и modular-kit design.

Детали — `AI_CONTEXT/ART_PIPELINE.md`.

## ОТКРЫТЫЕ ЗАДАЧИ (Next Steps)

### 1. Положение стрелок на кольце — визуальный A/B-тест (TODO)

Стрелки `EnemyDirectionIndicator` сейчас позиционируются на радиусе `worldRingRadius = 0.835` и визуально воспринимаются как расположенные на внутреннем контуре кольца. Требуется отдельный визуальный A/B-тест для выбора оптимального положения:

- внутренний контур толщины кольца
- середина толщины кольца (`midRingRadius = 0.785`, именно это значение использовалось в UI-варианте)
- внешний контур (`worldRingRadius = 0.835`, текущее значение)

Ограничения: логику направления, размер кольца, высоту (`worldRingHeightOffset = 0.02`) и остальные параметры без необходимости не менять. **Это не баг направления и не ошибка привязки к Enemy** — привязка математически проверена (`dot = 1.000000`, угловая ошибка `0.0000°`).

### 2. Shadow Aliasing — НЕ РЕШЕНО (открытая проблема)

| Параметр | Значение |
|----------|----------|
| Main Light Shadow Resolution | 1024 |
| Shadow Cascade Count | 1 |
| Shadow Distance | 50 |

Наблюдается блочность / aliasing теней. В этапе 2026-09-27 не исправлялось. Следующий тест должен менять **одну** shadow-настройку за раз. Первый кандидат — `1024 → 2048`. Не считать исправленным.

### 3. Диагностика квадратных/блочных теней

Зафиксировано пользователем как отдельное наблюдение. Выполняется отдельным шагом после определения оптимального положения стрелок. В этапе 2026-09-27 не исправлялось.

### 4. Висячие ссылки на удалённый диагностический скрипт (открытая проблема)

Файл `Assets/Scripts/Enemies/EnemyTacticalEnvironmentScanner_TEST.cs` (+ `.meta`) **удалён** из проекта. Удаление выполнялось как чистка временного диагностического кода; в `EnemyTacticalPlanner.cs` удалены все ссылки на этот тип, поэтому проект компилируется без ошибок.

**Однако 4 префаба врагов всё ещё содержат сериализованную ссылку на удалённый скрипт** (GUID `809b48f6d9f34f34d90ca85975a9c332`, строка 301 каждого файла):

| Префаб | Статус |
|--------|--------|
| `Assets/Prefabs/Enemies/Bandit.prefab` | висячая ссылка |
| `Assets/Prefabs/Enemies/Rusher.prefab` | висячая ссылка |
| `Assets/Prefabs/Enemies/Shooter.prefab` | висячая ссылка |
| `Assets/Prefabs/Enemies/Tactical.prefab` | висячая ссылка |

**Наблюдаемое последствие:** в Play Mode на заспавненных врагах (напр. `Bandit(Clone)`, слот компонента между `EnemyHealth` и `EnemyTacticalPlanner`) выводятся предупреждения `The referenced script (Unknown) on this Behaviour is missing!` — 7 штук на `Bandit` в одном замере. Ошибок компиляции при этом нет.

**Статус:** НЕ исправлено. Требуется отдельное решение: удалить пустой слот компонента из 4 префабов в Unity Editor. Функциональность врагов при этом не нарушена, но предупреждения загрязняют консоль.

### 5. Дубликат файла сцены `Assets/TestArena.unity`

Присутствует файл `Assets/TestArena.unity` рядом с настоящим `Assets/Scenes/TestArena.unity` (untracked). Образован при сохранении сцены через MCP по пути `Assets/` вместо `Assets/Scenes/`. Из-за этого часть сохранений не попадала в настоящую сцену.

**Статус:** НЕ исправлено (cleanup запрещён). Требуется отдельное решение пользователя.

### 6. Stage 4.4.5 — FROZEN / PAUSED

Технической реализации нет. Разблокировка — отдельное решение пользователя после готовности зависимостей.

### 7. CRATE_01 — CP2 (fracture) НЕ АВТОРИЗОВАН

CP1 завершён и ожидает одобрения пользователя. **Стартовать CP2 без явной авторизации нельзя.** Целевые бюджеты CP2 (из ТЗ пользователя, к исполнению): 8–14 fragments, 1000–2000 tris суммарно, сохранение реальных конструктивных частей ящика. Заготовка дизайна fragment-групп — `AI_CONTEXT/ART_PIPELINE.md` → «CRATE_01».

### 8. Unity integration environment assets — ожидает пакетного этапа

Поштучная интеграция запрещена (см. «Unity integration — ТОЛЬКО ПАКЕТНЫЙ ЭТАП»). Начинать после завершения набора environment-моделей и отдельного решения пользователя.

## ВЫПОЛНЕНО 2026-09-27 (визуальные исправления TestArena)

| Работа | Статус |
|--------|--------|
| Белое HUD-перекрытие (`MobileUI → HUD → Image`, `m_Enabled: 0`) | Implemented=YES, Compiled=YES, Tested=YES (2 прогона), Confirmed=NO |
| A/B `renderScale` 0.8 → 1 (`Mobile_RPAsset`) | Implemented=YES, Tested=YES, Confirmed=NO |
| A/B `MSAA` 1 → 4 (`Mobile_RPAsset`, Deferred) | Implemented=YES, Tested=YES, Confirmed=NO |
| 3D-кольцо `EnemyDirectionWorldRing` | Implemented=YES, Compiled=YES, Tested=YES, Confirmed=NO |
| World-space стрелки `EnemyDirectionWorldArrow_0..5` | Implemented=YES, Compiled=YES, Tested=YES, Confirmed=NO |
| Диагностика направления стрелок (файлы не изменялись) | Результат: логика корректна, `dot = 1.000000`, поправки угла не требуются |

Подробности — `CHANGELOG.md` (раздел «2026-09-27»), технические значения — `PROJECT_STATE.md`, архитектура — `ARCHITECTURE.md`.

## CURRENT FOCUS: подготовка и refinement 3D assets / Blender pipeline

Рабочий pipeline (уже используется, не план): OpenCode Desktop → Blender MCP → Blender → 3D asset → visual/numeric QA → при необходимости correction → повторная проверка → позднее Unity integration.

Приоритеты:

1. Доведение Blender asset pipeline.
2. Quality control моделей (NUMERIC PASS + VISION PASS = PASS).
3. Reusable / procedural-ready assets (многократное использование, позиции/ориентации/варианты).
4. Destructible-ready asset preparation (контролируемое частичное разрушение, Android perf; Stage 8 НЕ выполнен).
5. Canonical character/outfit prototype (Dummy → hat → shirt → pants → boots → belt → holster → revolver; без массового Bandit/Rusher/Shooter/Tactical и без skinning/export до visual approval).
6. После готовности зависимостей — возвращение к основному Stage checkpoint (решение отдельно).

Следующий рабочий пункт: продолжить Blender asset pipeline + QA по `ART_PIPELINE.md` (Environment/props/destructible-ready + canonical character), без изменения Stage-номера.

**Ближайшее конкретное действие (2026-09-28):** дождаться решения пользователя по `CRATE_01 CP1`. Дальнейшие Blender-ассеты среды и модульный wall/floor kit (`WallSegment ≈ 4 m`, `FloorSegment ≈ 4 × 4 m`) — только после отдельного согласования; `ArenaGenerator` не переписывать до settled material и kit design.

## STAGE 3 — baseline (опорная справка, не активный фокус)

**STAGE 3 — Групповая тактическая разведка и распределение врагов по комнатам/направлениям (BASELINE: Implemented=YES, Compiled=YES, Tested=YES практический тест Unity, Confirmed=NO)**

## Описание Stage 3

Распределение целей поиска между членами группы врагов с использованием данных `ArenaTacticalMap` о комнатах арены. Лидер группы распределяет цели, каждый член группы движется в свою комнату.

## Цепочка работы поиска

```
Spawn → SetSearchTarget → DistributeSearchTargets → движение в комнату
→ достижение searchTarget → RequestNextSearchTargets → новый searchCenter
→ новый этап распределения
```

## Stage 3 — опорная диагностика (кратко; полная история — CHANGELOG.md)

Точный root cause (актуален): `ArenaTacticalMap` существует (`Instance != null`), но до первого спавна не построен (`IsBuilt=false`, `Rooms.Count=0`) → `FindRoom()` → null → все кандидаты спавна отклоняются. Устаревший промежуточный диагноз `Instance==null` — см. CHANGELOG как deprecated/superseded, текущим не считать.

Фиксы в коде (состав baseline, детали — CHANGELOG): WaveManager coroutine + гарантированный `Build()` перед Wave 1; Planner Build-guarantee + guard `hasDistributedTargets` + индивидуальные цели + `visitedRooms` + `EnsureLeaderExists()` в `Update()`; Controller re-arm защёлка + `!SearchMode` гейты + неразрушающий A* refresh + гистерезис 15% + `<=` + быстрый routine-скан 0.5с / пауза 0.3с; `FindRoomNearConnection` — направленный критерий без порога 12м.

Итог baseline: Implemented=YES, Compiled=YES (dll новее исходников на момент фиксации), Tested=YES (практический тест Unity), Confirmed=NO. Отдельные промежуточные строки `Compiled=NO / Tested=NO` в истории — состояние до bundle-фиксации, текущим статусом не считать.

## Статус Stage 3 bundle

| Этап | Implemented | Compiled | Tested | Confirmed |
|------|-------------|----------|--------|-----------|
| Stage 3 working baseline (bundle всех фиксов выше) | YES | YES | YES (практический тест Unity) | NO |

Промежуточные `Compiled=NO / Tested=NO` по отдельным патчам — история до bundle-фиксации (см. CHANGELOG), текущим статусом не являются.

## Stage 4.4 — WildWestEnvironment (заморожен на 4.4.5)

| Подэтап | Статус |
|---------|--------|
| 4.4.1 Скелет генератора | Implemented=YES, Compiled=YES, Tested=YES, Confirmed=NO |
| 4.4.2 Ground | Implemented=YES, Compiled=YES, Tested=YES, Confirmed=NO |
| 4.4.3 Rocks + Cliffs | Implemented=YES, Compiled=YES, Tested=YES, Confirmed=NO |
| 4.4.4 Trees + Bushes + Grass | Implemented=YES, Compiled=YES, Tested=YES, **Confirmed=YES** |
| 4.4.5 Background | **FROZEN / PAUSED** (ранее NOT STARTED; технической реализации нет) |

Runtime 4.4.4: Ground=1, Cliffs=6, Rocks=32, Trees=16, Bushes=48, Grass=255 (rejected 7/13/44), ошибок нет. Desert Pack dormant (Mat_01 не URP). TestArena, AI, ArenaTacticalMap не изменялись.

## Изменённые файлы

### Фактическое состояние рабочей копии (проверено `git status`, 2026-09-27)

**Отслеживаемые файлы с изменениями:**

| Файл | Изменения |
|------|-----------|
| `Assets/Scenes/TestArena.unity` | `MobileUI → HUD → Image` → `m_Enabled: 0`; `WildWestEnvironmentGenerator`; `floorMaterial`/`coverMaterial`; Directional Light color → (1,1,1) |
| `Assets/Settings/Mobile_RPAsset.asset` | `m_RenderScale` 0.8 → 1; `m_MSAA` 1 → 4 |
| `Assets/Settings/PC_RPAsset.asset` | `m_ColorGradingMode` 0 → 1 (HighDynamicRange) |
| `Assets/Scripts/UI/EnemyDirectionIndicator.cs` | 3D-кольцо `EnemyDirectionWorldRing` + world-space стрелки `EnemyDirectionWorldArrow_0..5`; ground raycast; переключатели `useWorldRing` / `useWorldArrows` |
| `Assets/Scripts/Procedural/ArenaGenerator.cs` | Footprint-aware cover placement: `GetCoverFootprintRadius` (0.5·√(w²+d²)), `IsTooCloseFootprint`, `IsInsideRoomBoundsFootprint`, `IsNearAnyOpenConnectionFootprint`, `minimumCoverClearance = 0.75`; `coverMaterial`/`floorMaterial` |
| `Assets/Scripts/Enemies/EnemyTacticalPlanner.cs` | Удалены ссылки на удалённый диагностический `EnemyTacticalEnvironmentScanner` |
| `Assets/Scenes/TestArena_Preview.unity` | `coverMaterial` (null), `useCoverClusters` |
| `AI_CONTEXT/RULES.md`, `AI_CONTEXT/VERIFICATION.md` | Раздел Gemma review |
| `Assets/TextMesh Pro/.../LiberationSans SDF - Fallback.asset` | Изменён Unity (не вручную) |

**Новые (untracked) файлы:**

| Файл | Назначение |
|------|-----------|
| `Assets/Prefabs/Environment/WWG_GroundPatch_01.prefab` | Environment prefab (Phase C) |
| `Assets/Prefabs/Environment/WWW_Rock_01.prefab`, `WWG_Rock_02.prefab` | Environment prefabs |
| `Assets/Prefabs/Environment/WWG_DeadGrass_01.prefab` | Environment prefab |
| `Assets/Prefabs/Environment/WWG_Cover_Wood_01.prefab` | Environment prefab |
| `Assets/Materials/WWG_Ground_Patch.mat`, `WWG_Floor_Dusty.mat`, `WWG_Rock_Sandstone.mat`, `WWG_Wood_Cover.mat`, `WWG_DeadGrass.mat` | Материалы для Phase C |
| `Assets/TestArena.unity` | **ДУБЛЬ сцены — нежелательный артефакт** (см. ниже) |

**Известный артефакт (не исправлялся в этапе документации):** присутствует дублирующий файл `Assets/TestArena.unity` рядом с настоящим `Assets/Scenes/TestArena.unity`. Причина — операция сохранения сцены через MCP выполнилась по пути `Assets/` вместо `Assets/Scenes/`. Из-за этого удаления объектов, сохранённые в дубликате, не отражались в настоящей сцене. Требуется отдельное решение пользователя (удалить дубликат или перенести).

### Изменённые файлы (историческая секция Stage 3)

| Файл | Изменения |
|------|-----------|
| `WaveManager.cs` | Coroutine `StartFirstWaveNextFrame()`: `yield return null` → гарантированный `ArenaTacticalMap.Build()` (если `Instance != null && !IsBuilt`) → `StartNextWave()` |
| `EnemyTacticalPlanner.cs` | Поля `hasDistributedTargets`, `lastDistributedCenter`; Build() guarantee; guard-проверка; diagnostic logs; сброс флага; `leaderFound` + локальный фолбэк (заменён); `EnsureLeaderExists()` в `Update()`; **индивидуальные цели: `visitedRooms` на планировщик, `RequestNext` назначает только запросившему (P1 непосещённый сосед → P2 непосещённая с карты → P3 повторный цикл без только-что-покинутой), исключение чужих targetRoom, без перераздачи при смерти** |
| `EnemySpawner.cs` | ТОЛЬКО диагностика: агрегированные счётчики отказов `[SPAWNER DIAG STRICT/RELAXED/FALLBACK1/FALLBACK2/ZONE]`; логика спавна не менялась |
| `EnemyController.cs` | Защёлка `plannerRearmRequested` + гейт `!SearchMode` в `StartInvestigation()` и выходах `RoomSearching`; неразрушающий A* refresh + гистерезис 15% + `<=` для вейпоинта; быстрый routine-скан пустых комнат (0.5 с при planner-цели без контакта/шума) и короткая дверная пауза (0.3 с) при нетаргетном транзите |
## Ожидаемое поведение после WaveManager исправления

1. `WaveManager.Start()` → coroutine → `yield return null` (один кадр)
2. За этот кадр `ArenaGenerator.Start()` → `GenerateArena()` завершается (Rooms=9, SpawnZones=53)
3. `ArenaTacticalMap.Build()` → tactical map содержит реальные комнаты (`IsBuilt=true`, `Rooms.Count>0`)
4. `StartNextWave()` → `SpawnEnemies()` → `IsPositionWalkable()` находит комнаты → Wave 1 создаёт врагов