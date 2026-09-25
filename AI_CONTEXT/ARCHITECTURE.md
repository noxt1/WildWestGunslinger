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
