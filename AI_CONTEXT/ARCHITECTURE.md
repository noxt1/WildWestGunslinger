> **Status:** OPERATIONAL / REFERENCE LAYER - reconciled 2026-09-29 (Phase 4)
> **Role:** component-level engineering reference for the AI and environment systems.
> **Canonical architecture:** ../Documentation/ARCHITECTURE.md (authoritative for as-built system shape)
> **Code inventory with current line counts:** ../Documentation/Engineering/UNI-0001-UNITY-PROJECT-STATE.md section 5.1
> **Not canonical for:** project state, requirements, decisions, or plan.
> **Line counts in this file are STALE** - measured values live in UNI-0001 section 5.1.
> **Reconciliation:** ../Documentation/Archive/Audits/PHASE4_AI_CONTEXT_RECONCILIATION.md
> Superseded as canonical source on 2026-09-29; retained as an operational reference.

# ARCHITECTURE — Архитектура AI-системы

## Обзор компонентов

```
                    ┌─────────────────────┐
                    │   EnemySpawner      │
                    │ (группы, роли,      │
                    │  типы врагов)       │
                    └─────────┬───────────┘
                              │ создаёт врагов
                              ▼
┌──────────────────────────────────────────────────────────┐
│                    EnemyController                        │
│  (состояния: Combat, Searching, Investigating,           │
│   RoomSearching, MovingToCover, InCover, Flanking)       │
│  (A* навигация, движение, стрельба, укрытия)            │
├──────────┬──────────┬──────────┬──────────┬──────────────┤
│          │          │          │          │              │
│  ▼       │  ▼       │  ▼       │  ▼       │  ▼           │
│ EnemyTacticalPlanner │ EnemyTacticalVision│ EnemyHearing │
│ (роли, позиции,      │ (обнаружение,      │ (шумы,       │
│  группы, поиск)      │  сканирование,     │  расслед.)   │
│                      │  уклонение)        │              │
└──────────┴──────────┴──────────┴──────────┴──────────────┘
                              │
                    ┌─────────┴───────────┐
                    │   ArenaTacticalMap   │
                    │ (A*, комнаты, путь)  │
                    └─────────────────────┘
                              │
                    ┌─────────┴───────────┐
                    │   NoiseSystem       │
                    │ ( singleton, шумы)   │
                    └─────────────────────┘
```

## Компоненты

---

### EnemyDirectionIndicator (2026-09-27)

**Файл:** `Assets/Scripts/UI/EnemyDirectionIndicator.cs`
**Ответственность:** Индикатор направления к врагам для игрока. Кольцо и стрелки расположены на полу под ногами игрока как 3D-объекты, участвуют в глубине сцены и перекрываются геометрией мира.

**Важно:** объект создаётся в runtime через `[RuntimeInitializeOnLoadMethod]` (`Bootstrap`), в сцене не сериализуется. Создаётся один экземпляр, `DontDestroyOnLoad`.

**Инициализация:** `Canvas` (`ScreenSpaceOverlay`, `sortingOrder = 5000`) для UI-стрелок (сохранены для отката, выключены при `useWorldArrows`) + 3D-объекты `EnemyDirectionWorldRing` и `EnemyDirectionWorldArrow_0..5` как дочерние элементы самого индикатора (вне Canvas).

**3D-кольцо (`EnemyDirectionWorldRing`):**
- `MeshFilter` + `MeshRenderer`, процедурный плоский annulus в XZ (98 вершин / 96 треугольников)
- Параметры: `worldRingRadius = 0.835`, `worldRingThickness = 0.1`, `worldRingHeightOffset = 0.02`
- Материал: `Universal Render Pipeline/Unlit`, Transparent, `_ZWrite = 0`, `_Cull = Off`, `renderQueue = 3000`, `_BaseColor = RGBA(1, 1, 1, 0.45)`
- Collider отсутствует; `shadowCastingMode = Off`, `receiveShadows = false`, light/reflection probe usage выключены

**3D-стрелки (`EnemyDirectionWorldArrow_0..5`):**
- По одному треугольнику на стрелку; остриё вдоль локальной +X, основание сзади
- Параметры: `arrowWorldLength = 0.30`, `arrowWorldHalfWidth = 0.094`
- Позиция: `player.position + worldDirection * worldRingRadius`, `Y = currentGroundY + worldRingHeightOffset` (та же точка и высота, что у кольца)
- Поворот: `rotY = Atan2(-worldDirection.z, worldDirection.x)` — остриё совмещается с направлением на врага
- Материал общий на все стрелки: URP Unlit Transparent, `RGBA(1, 0.18, 0.05, 0.95)`; `shadowCastingMode = Off`; Collider отсутствует

**Определение поверхности (`TryGetGroundUnderPlayer`):**
- `Physics.RaycastNonAlloc` вниз из `player.position + up * groundProbeStartHeight (3)`, дистанция `groundProbeDistance (40)`, `Physics.DefaultRaycastLayers`, `QueryTriggerInteraction.Ignore`
- Предвыделенный статический буфер `RaycastHit[8]` — без аллокаций в кадре
- Исключаются собственные трансформы индикатора и коллайдеры игрока; берётся ближайший валидный hit
- Результат кэшируется в `currentGroundY` и используется и кольцом, и стрелками
- Ориентация: ровный пол → горизонтально; наклон > 0.1° → `Quaternion.FromToRotation(up, normal)`

**Выбор врагов (не изменялось):** `visibleEnemies` собирается из `EnemyController`, сортируется по дистанции к игроку; стрелка с индексом `i` соответствует `visibleEnemies[i]`. Активных стрелок — `min(maxArrows, visibleEnemies.Count)`.

**Rollback:** `useWorldRing` и `useWorldArrows` (bool, SerializeField) — при `false` восстанавливается прежнее UI-представление (объекты остаются созданными, отключаются через `enabled`).

---

### EnemyController (3738 строк)

**Файл:** `Assets/Scripts/Enemies/EnemyController.cs`

**Ответственность:** Главный AI state machine. Управляет всем поведением врага: обнаружение игрока, переходы между состояниями, движение, стрельба, укрытия, A* навигация.

**AI-состояния:**

| Состояние | Описание |
|-----------|----------|
| `Combat` | Бой с игроком: движение к цели, стрельба, укрытия |
| `Searching` | Поиск игрока по известным точкам |
| `Investigating` | Расследование шума или последней известной позиции |
| `RoomSearching` | Поиск по комнатам арены |
| `MovingToCover` | Движение к укрытию |
| `InCover` | Пребывание в укрытии, peek-стрельба |
| `Flanking` | Фланговое движение (только Tactical) |

**Связи:**
- Содержит `EnemyTacticalPlanner tacticalPlanner` (Stage 1)
- Использует `EnemyTacticalVision` для обнаружения
- Использует `EnemyHearing` для шумов
- Вызывает `ArenaTacticalMap.TryFindPath()` для A* (Stage 2)
- Обращается к `CoverSystem` для укрытий

**Ключевые методы:**
- `UpdatePerception()` — обновление восприятия, вызов `tacticalPlanner.SetThreatPosition()`
- `UpdateCombatState()` — логика боя
- `UpdateSearchingState()` — логика поиска (с поддержкой Planner searchTarget)
- `UpdateInvestigatingState()` — логика расследования
- `MoveTowardsKnownTarget()` — A* для дальних дистанций, прямое движение для близких
- `OnDestroy()` — вызов `tacticalPlanner.OnMemberDied()`
- Один re-arm planner-режима: защёлка `plannerRearmRequested` (взвод у `ClearSearchMode`, сработка на первом кадре `Searching`)
- Гейты `!SearchMode` в `StartInvestigation()` и выходах `RoomSearching` (активная цель не затирается)
- Неразрушающий A* refresh: `hardRebuild` vs `RefreshPathIfBetter()` (принятие только при улучшении ≥15%, порог 0.85f); потребление вейпоинта `<=`
- Быстрый routine-осмотр: скан прибытия 0.5 с (только planner-цель без контакта/шума), дверная пауза 0.3 с (нетаргетный транзит); контакт/шум/охота — прежние 1.15 с / 0.8 с

---

### EnemyTacticalPlanner (1469 строк)

**Файл:** `Assets/Scripts/Enemies/EnemyTacticalPlanner.cs`

**Ответственность:** Тактическое управление группой врагов: распределение ролей (LeftFlank, RightFlank, Support, Rear, Pressure), групповые позиции, тактический поиск по комнатам.

**Роли:**

| Роль | Описание |
|------|----------|
| `LeftFlank` | Левый фланг |
| `RightFlank` | Правый фланг |
| `Support` | Поддержка |
| `Rear` | Задняя позиция |
| `Pressure` | Давление (лидер группы) |

**Ключевые методы:**
- `ConfigureGroup()` — настройка группы
- `SetThreatPosition()` — обновление позиции угрозы
- `BuildPlan()` — расчёт позиции на основе роли
- `SetSearchTarget()` — начало поиска по комнатам (лидер раздаёт всей группе — стартовое распределение)
- `DistributeSearchTargets()` — полное распределение (старт, Build-guarantee, guard `hasDistributedTargets`)
- `RequestNextSearchTargets()` — ТОЛЬКО индивидуальная цель запросившему (без перераздачи группе)
- `visitedRooms` — личная память проверенных комнат (приоритет: непосещённый сосед → непосещённая с карты → повторный цикл без только-что-покинутой; чужие активные targetRoom исключаются; детерминированно, без random)
- `OnMemberDied()` — только cleanup + `EnsureLeaderExists()` (без перераздачи)
- `EnsureLeaderExists()` — вызывается каждым активным планировщиком в `Update()` (восстановление лидера после смерти)
- `TryGetSearchTarget()` — получение индивидуальной цели поиска

**Связи:**
- Содержит ссылку на `EnemyController`
- Содержит список `groupMembers` (другие планировщики)
- Использует `ArenaTacticalMap.Instance.FindRoom()` для поиска комнат
- Вызывает `ArenaTacticalMap.Instance.Build()` при необходимости

---

### EnemyTacticalVision (2268 строк)

**Файл:** `Assets/Scripts/Enemies/EnemyTacticalVision.cs`

**Ответственность:** Визуальное восприятие врага. 360° сканирование, обнаружение игрока, обход препятствий, выбор маршрутов.

**Ответственность НЕ включает:** навигацию/A* — это делает `ArenaTacticalMap`.

**Ключевые методы:**
- `ScanNow()` — основной тик сканирования
- `CanDetectPlayer()` — проверка видимости игрока (дистанция, FOV, рейкасты)
- `HasDirectVisionToPlayer()` — полная проверка видимости
- `LookAtNoise()` — поворот к источнику шума
- `GetTacticalDirection()` — смешивание желаемого направления с уклонением

**Обнаружение:**
- Дистанция сканирования: 18
- FOV: 30° (нормальный), 80° (расследование шума)
- 4 рейкаста по точкам тела игрока (0.35, 0.9, 1.35, 1.7)
- Другие `EnemyController` блокируют视线

**Состояния:**

| Состояние | Описание |
|-----------|----------|
| `Idle` | Пассивный режим |
| `Scanning` | Сканирование окружения |
| `PlayerDetected` | Игрок обнаружен |
| `Searching` | Поиск потерянного игрока |
| `Investigating` | Расследование шума |

---

### EnemyHearing (342 строки)

**Файл:** `Assets/Scripts/Enemies/EnemyHearing.cs`

**Ответственность:** Обработка шумов. Регистрация в `NoiseSystem`, приоритизация шумов, запуск расследования.

**Ключевые методы:**
- `RegisterWithNoiseSystem()` — регистрация слушателя
- `ReceiveNoise()` — приём шума, расчёт приоритета
- `IsInvestigating()` — проверка состояния расследования
- `StopInvestigation()` — остановка расследования

**Приоритеты шумов:**

| Тип | Множитель |
|-----|-----------|
| Explosion | 12 |
| Gunshot | 10 |
| Sprint | 6 |
| Impact | 4 |
| Footstep | 3 |
| Reload | 2 |
| Interaction | 1 |

---

### NoiseSystem (557 строк)

**Файл:** `Assets/Scripts/Enemies/NoiseSystem.cs`

**Ответственность:** Singleton-менеджер шумов. Рассылает события шумов всем зарегистрированным `EnemyHearing`.

**Типы шумов:**

| Тип | Радиус | Интенсивность |
|-----|--------|---------------|
| Explosion | 45 | 1.5 |
| Gunshot | 35 | 1.0 |
| Sprint | 16 | 0.55 |
| Impact | 12 | 0.4 |
| Footstep | 9 | 0.3 |
| Reload | 8 | 0.2 |
| Interaction | 6 | 0.15 |

**Распространение:** `сила = интенсивность * (1 - дистанция/радиус)`. Стены блокируют: при >4 стен — полная блокировка, иначе x0.55 за стену.

---

### ArenaTacticalMap (1770 строк)

**Файл:** `Assets/Scripts/Procedural/ArenaTacticalMap.cs`

**Ответственность:** A* навигация, поиск комнат, тактические позиции. Singleton.

**Ключевые методы:**
- `Build()` — построение карты из `ArenaModule` (вызывается из `WaveManager` до Wave 1)
- `FindRoom(Vector3)` — поиск комнаты по позиции (с fallback на ближайшую)
- `TryFindPath(start, target, result)` — A* поиск пути (вызывает `Build()` если нужно)
- `FindRoomNearConnection()` — направленный критерий соседства (комната за плоскостью стены, без фиксированного порога 12 м)
- `FindRoomRoute()` — межкомнатный маршрут BFS по `Neighbours`
- `GetTacticalPosition()` — тактическая позиция для роли

**Структуры данных:**
- `RoomMap` — карта комнаты (связи, ячейки, модуль)
- `GridCell` — ячейка сетки (позиция, проходимость)

**Принцип:** `FindRoom()` НЕ вызывает `Build()`. `TryFindPath()` вызывает `Build()` если `!built`. `IsBuilt` — публичное свойство.

---

### CoverSystem (510 строк)

**Файл:** `Assets/Scripts/Enemies/Cover/CoverSystem.cs`

**Ответственность:** Singleton-менеджер укрытий. Поиск, оценка, регистрация укрытий.

**Ключевые методы:**
- `GetBestCover()` — поиск лучшего укрытия
- `GetSearchPositions()` — позиции для расследования
- `BuildAutomaticCoverPointsNow()` — автогенерация укрытий

---

### CoverPoint (486 строк)

**Файл:** `Assets/Scripts/Enemies/Cover/CoverPoint.cs`

**Ответственность:** Одна позиция укрытия. Занятость, peek-позиции, валидность.

---

### WildWestEnvironmentGenerator (Stage 4.4)

**Файл:** `Assets/Scripts/Environment/WildWestEnvironmentGenerator.cs`

**Ответственность:** Внешнее процедурное окружение поверх готовой арены. Не знает про AI, не влияет на навигацию (объекты вне bounds комнат, создаются после `Build()`).

**Источники данных (только чтение):** `ArenaTacticalMap.Rooms`, `RoomMap.Module.GetWorldBounds()`, `RoomMap.Exits`. `ArenaGenerator` не используется (нет публичного API).

**Категории (root `WildWestEnvironment`):** Ground (Plane + `M_Ground_Desert`) → Cliffs/Rocks (общий spacing-лист) → Trees/Bushes/Grass (сэмплинг от границы bounds, независимые spacing-списки + проверка крупных объектов, Collider травы off на инстансах). Deterministic `System.Random(seed)`; keep-out bounds + Exit-радиусы абсолютны для всех.

**Пулы префабов (вне комнат, после `IsBuilt`):** Fences (max 12, `Fence_Western_Wooden` + `DestructibleObject` HP=50, 2 CoverPoint) → Props (max 10: Barrel/Crate/Cover) → Lanterns (max 6, декор, CoverPoint нет) → Wagons (max 4, 2 BoxCollider). Стены (`Western_Log_Wall_Segment`, `Western_Plank_Wall_Segment`) — PREPARED, не scattered, в ручную сцену не интегрированы.

---

### DestructibleObject (foundation, не Stage 8)

**Файл:** `Assets/Scripts/Environment/DestructibleObject.cs`

**Ответственность:** Единственное ядро разрушаемости: HP, INTACT→DAMAGED→DESTROYED, массивы hide/show, UnityEvents, без Update. `Bullet`/`EnemyBullet` — минимальные вставки `TakeDamage` через `GetComponentInParent<DestructibleObject>`. `CoverSystem` — guard `isActiveAndEnabled`. Принцип: контролируемое частичное разрушение, отдельные части могут отделяться, физика только точечно (2 fence-debris kinematic), Android perf. Подготовка будущего Stage 8, НЕ его выполнение.

---

### Blender production → Unity prefab pipeline (вне Stage-номера)

Источник: `Documents\WildWestGunslinger art\art\Environment\<Name>\` (FBX + `_Source.blend` + `textures/`); в Unity — только FBX+текстуры. FBX Z-up → prefab-level `Model` rotation −90° X, minY=0.00. Shared URP Lit материалы `Art/Environment/Shared/Materials/` (4 шт.), realtime lights 0, коллайдеры минимальны (1 Box, wagon 2). Детали art-фокуса — `AI_CONTEXT/ART_PIPELINE.md`; character-стандарт — `.opencode/skills/wwg-character-art/SKILL.md`.

### Blender-authored destructible assets → batched Unity integration (2026-09-28)

Второй, независимый от legacy-пайплайна поток. Ассеты создаются **с нуля** в `Working\blender_src\<name>\<Name>_01_Working.blend` по Method C; legacy FBX используются только как read-only baseline и основой не являются.

**Двухэтапная схема (Unity-интеграция отложена):**

```
Этап Blender (текущий)                        Этап Unity (отложен)
─────────────────────────────                 ────────────────────────
model → QA (NUMERIC + VISION)                 FBX import
→ controlled fracture → final FBX             normals/smoothing verification
                                             materials, vertex colors
                                             colliders, prefabs
                                             destructible setup
                                             runtime destruction
                                             gameplay integration
                                             performance validation
```

**Жёсткое правило:** поштучная Unity-интеграция запрещена. Один отдельный **пакетный** этап выполняется только после завершения набора environment-моделей и отдельного решения пользователя. Статус отдельного asset `FINAL` означает «Blender-часть завершена и подтверждена» и **не** является поводом начинать Unity-интеграцию.

**Runtime-контракт destructible asset:**

| Collection | `hide_viewport` | `hide_render` | Роль |
|---|---|---|---|
| `<Name>_Intact` | FALSE | FALSE | default presentation |
| `<Name>_Fragments` | **TRUE** | FALSE | доступны для exploded QA / inspection / FBX export |

Причина правила: одновременное отображение intact и reassembled fragments на совпадающих поверхностях вызывает «дёрганье» wood surface во вьюпорте (подтверждённый root cause на Barrel_01). Fragments остаются в blend и в рендере.

**Material contract (M2):** один material slot на объект; `Col` = `FLOAT_COLOR` / `CORNER`, RGB = тон элемента, A = metal mask; `Metallic` из A; общий dark old iron `(0.0785, 0.0794, 0.0830)`; UV world-proportional, U вдоль длинной оси элемента; image textures не используются.

**Планируемая модульная архитектура окружения (ещё не реализована):** `WallSegment ≈ 4 m` и `FloorSegment ≈ 4 × 4 m` — каждый = 1 mesh + 1 MeshRenderer + 1 shared material + 1 simple collider. Запрещено «1 log = 1 GameObject» / «1 floorboard = 1 GameObject»: генератор размещает переиспользуемые модули. Android perf — ограничение с самого начала; `ArenaGenerator` не переписывается до settled material и kit design.

---

### EnemySpawner (1498 строк)

**Файл:** `Assets/Scripts/Level/EnemySpawner.cs`

**Ответственность:** Спаун врагов группами. Назначение ролей, типов, элитных статусов.

**Ключевые методы:**
- `SpawnEnemies(count)` — спаун указанного количества врагов
- `ConfigureTacticalGroup()` — настройка групп и ролей
- `GetLiveEnemyCount()` — количество живых врагов

**Группировка:**
- Стартовый размер группы: 4
- Доп. группа каждые 3 волны
- Мин. расстояние от игрока: 6
- Мин. расстояние между группами: 4

---

### WaveManager (149 строк)

**Файл:** `Assets/Scripts/Level/WaveManager.cs`

**Ответственность:** Управление волнами. Подсчёт врагов, переход к следующей волне.

**Ключевые методы:**
- `StartWave()` — начало волны
- `CheckWaveComplete()` — проверка завершения волны
- `NextWaveDelay()` — задержка между волнами

---

## Принципы архитектуры

1. **Восприятие vs Навигация:** `EnemyTacticalVision` отвечает за обнаружение и визуальное восприятие. `ArenaTacticalMap` отвечает за навигацию/A* и пространственный анализ.

2. **Групповое управление:** `EnemyTacticalPlanner` управляет группой через роли и координацию. Лидер группы (role `Pressure`) распределяет задачи.

3. **Одно состояние = один файл:** AI-состояния находятся в `EnemyController`. Новые состояния НЕ создаются без согласования.

4. **Singleton-системы:** `ArenaTacticalMap`, `CoverSystem`, `NoiseSystem` — singleton. Доступ через `Instance`.

5. **Связь через ссылки:** `EnemyController` содержит ссылку на `EnemyTacticalPlanner`. `EnemyHearing` содержит ссылку на `EnemyTacticalVision`. Группы связаны через `groupMembers`.