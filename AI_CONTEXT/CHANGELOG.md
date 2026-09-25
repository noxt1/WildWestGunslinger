# CHANGELOG — Хронология AI-разработки

## Формат записей

Каждая запись содержит:
- **Описание** — что было сделано
- **Статусы** — единая схема: Implemented / Compiled / Tested / Confirmed (без смешивания; `PENDING` = промежуточное состояние до bundle-фиксации, не текущий статус).

Промежуточные диагнозы сохраняются как история. Если диагноз superseded текущим root cause — он помечен `> DEPRECATED`. Текущий root cause Stage 3: непостроенная tactical map (`Instance != null`, `IsBuilt=false`), не отсутствие компонента. Текущий статус Stage 3 — только строка `Stage 3 working baseline` в Итого; отдельные `Compiled=NO / Tested=NO` — история.

---

## 2026-09-18: Stage 1 — Интеграция EnemyTacticalPlanner с EnemyController

**Описание:** Добавлена связь между `EnemyController` и `EnemyTacticalPlanner`. EnemyController теперь содержит ссылку на Planner, передаёт позицию угрозы, и использует Planner target для стрельбы.

**Реализация:**
- Поле `tacticalPlanner` в `EnemyController` (line 160)
- `GetComponent<EnemyTacticalPlanner>()` в `Awake()` (line 272-273)
- `tacticalPlanner.SetThreatPosition()` в `UpdatePerception()` (line 566-568)
- `GetCombatTargetPosition()` предпочитает Planner target (line 791-807)

**Файлы:** `EnemyController.cs`

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | YES (0 errors) |
| Tested | YES |
| Confirmed | **YES** |

---

## 2026-09-18: Stage 2 — A* навигация

**Описание:** Подключена A* навигация из `ArenaTacticalMap` к движению врагов. Враги используют A* для дальних дистанций (>= 5м) и прямое движение для близких (< 5м).

**Реализация:**
- Поля: `currentPathWaypoints`, `currentPathIndex`, `pathTargetPosition` и др. (lines 162-173)
- Методы: `ClearNavigationPath()` (line 2795), `TryGetPathToTarget()` (line 2804), `BuildPathToTarget()` (line 2875), `MoveDirectlyToTarget()` (line 2902)
- Модифицирован `MoveTowardsKnownTarget()` (line 2733)
- `ClearNavigationPath()` вызывается в 7 переходах состояний

**Файлы:** `EnemyController.cs`

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | YES (0 errors) |
| Tested | YES |
| Confirmed | **YES** |

---

## 2026-09-18: Stage 3 — Групповое распределение целей поиска

**Описание:** Реализовано распределение целей поиска между членами группы с использованием данных `ArenaTacticalMap` о комнатах. Лидер группы распределяет цели, каждый член движется в свою комнату.

**Реализация:**
- Поля: `searchMode`, `searchCenter`, `searchTarget`, `hasSearchTarget`, `nextRedistributeTime`, `redistributeInterval`
- Поля: `hasDistributedTargets`, `lastDistributedCenter`
- Методы: `SetSearchTarget()`, `ClearSearchMode()`, `TryGetSearchTarget()`, `OnMemberDied()`, `RequestNextSearchTargets()`, `DistributeSearchTargets()`, `SelectRoomForMember()`, `FindWalkablePositionInRoom()`, `SetDirectSearchTarget()`, `RemoveDeadMembers()`, `EnsureLeaderExists()`
- Интеграция в `EnemyController`: `SetSearchTarget` в `UpdateInitialBehavior()`, `ClearSearchMode` в Combat, `SetSearchTarget` в `StartInvestigation()`, Planner searchTarget в `UpdateSearchingState()`, `OnMemberDied` в `OnDestroy()`

**Файлы:** `EnemyTacticalPlanner.cs`, `EnemyController.cs`

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | YES (0 errors) |
| Tested | **PENDING** |
| Confirmed | **NO** |

---

## 2026-09-18: Обнаружение проблемы currentRoom=null

**Описание:** При тестировании обнаружено, что `DistributeSearchTargets()` вызывает `FindRoom(searchCenter)` до гарантированной сборки `ArenaTacticalMap`. `FindRoom()` НЕ вызывает `Build()` (в отличие от `TryFindPath()`). Список `rooms` пуст → `FindRoom()` возвращает `null`.

**Цепочка ошибки:**
1. `DistributeSearchTargets()` → `FindRoom(searchCenter)` → `null`
2. → `DistributeFallbackSearchTargets()`
3. → `Update()` вызывает `DistributeSearchTargets()` каждый кадр
4. → Fallback-цель назначается снова и снова

| Статус | Значение |
|--------|----------|
| Diagnosed | YES |
| Root cause identified | YES |

---

## 2026-09-18: Исправление Build() guarantee

**Описание:** В `DistributeSearchTargets()` добавлена проверка `ArenaTacticalMap.Instance.IsBuilt` и вызов `Build()` при необходимости. Использует существующий публичный API `IsBuilt` (line 40) и `Build()` (line 65).

**Код:**
```csharp
if (!ArenaTacticalMap.Instance.IsBuilt)
{
    ArenaTacticalMap.Instance.Build();
}
```

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | YES |
| Tested | **PENDING** |
| Confirmed | **NO** |

---

## 2026-09-18: Добавление защиты от повторного распределения

**Описание:** Добавлены поля `hasDistributedTargets` и `lastDistributedCenter`. В `DistributeSearchTargets()` добавлена guard-проверка: если флаг `true` и `searchCenter` не изменился — распределение пропускается. Флаг сбрасывается в `SetSearchTarget()`, `RequestNextSearchTargets()`, `OnMemberDied()`.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | YES |
| Tested | **PENDING** |
| Confirmed | **NO** |

---

## 2026-09-18: Глубокий аудит двух проблем — timing и повторный SetSearchTarget

> DEPRECATED (частично): вывод «Timing — НЕ является проблемой» superseded позднейшим evidence (Wave 1 = 43 зоны, Wave 2 = 53 зоны) и WaveManager-фиксом. Актуально из раздела только: `IsBuilt` не гарантирует комнат + guard после `Instance == null`. Текущий root cause — непостроенная карта.

**Описание:** Проведён детальный аудит двух обнаруженных проблем. Результаты:

### Timing — НЕ является проблемой
- Unity execution order: все `Start()` завершаются до первого `FixedUpdate()`
- `ArenaGenerator.Start()` создаёт 9 `ArenaModule` объектов синхронно
- К моменту первого `EnemyController.FixedUpdate()` модули существуют
- **Timing исключён как причина**

### `IsBuilt` не гарантирует наличие комнат
- `Build()` устанавливает `built = true` даже если `FindObjectsByType<ArenaModule>()` вернул 0 объектов
- `rooms` остаётся пустым → `FindRoom()` возвращает null
- **Нужен runtime diagnostic log**

### Guard `hasDistributedTargets` стоит после проверки `Instance == null`
- Если `ArenaTacticalMap.Instance == null`, fallback вызывается без проверки guard
- `Planner.Update()` → `DistributeSearchTargets()` → fallback повторяется
- **Guard нужно переместить ВЫШЕ**

### Установленные факты
1. `FindRoom()` возвращает `null` ТОЛЬКО если `rooms` пуст
2. `SetSearchTarget()` вызывается ОДИН раз на врага (guard `initialSearchStarted`)
3. `[SEARCH TARGET]` повторяется из-за fallback, а не из-за повторного SetSearchTarget

### Возможные причины (требуют runtime confirmation)
1. `ArenaTacticalMap.Instance == null`
2. `ArenaTacticalMap.Instance != null`, но `rooms.Count == 0` после `Build()`

| Статус | Значение |
|--------|----------|
| Diagnosed | YES |
| Root cause confirmed | **YES** — runtime evidence: Instance=False, ArenaModules=9 |

---

## 2026-09-18: Runtime подтверждение ArenaTacticalMap.Instance == null

> DEPRECATED как текущий root cause: промежуточный evidence (`Instance=False`, компонент отсутствовал в той сцене). Superseded точным root cause ниже (карта существует, но не построена). Сохранено как история.

**Описание:** Получен runtime evidence из Unity:

```
[TACTICAL DEBUG] Instance=False IsBuilt=False Rooms=-1 ArenaModules=9
```

**Установлено:**
- 9 ArenaModule существуют в сцене
- `ArenaTacticalMap.Instance == null` — компонент не существует в сцене
- `Build()` не вызывается
- `DistributeSearchTargets()` сразу уходит в fallback

**Причина:** `ArenaTacticalMap` — MonoBehaviour, но нет ни одного GameObject с этим компонентом в сцене. Singleton pattern не работает, потому что компонент отсутствует.

| Статус | Значение |
|--------|----------|
| Runtime evidence | **YES** |

---

## 2026-09-18: Runtime подтверждение неполной генерации арены

**Описание:** Получен runtime evidence из Unity:

```
Wave 1: EnemySpawnZones = 43
Wave 2: EnemySpawnZones = 53
```

**Установлено:**
- Wave 1 находит 43 зоны (арена не полностью сгенерирована)
- Wave 2 находит 53 зоны (+10 зон)
- `WaveManager.Start()` вызывает `StartNextWave()` до завершения `ArenaGenerator.GenerateArena()`
- Unity не гарантирует порядок `Start()` между разными объектами

| Статус | Значение |
|--------|----------|
| Runtime evidence | **YES** |

---

## 2026-09-18: Исправление WaveManager timing

**Описание:** В `WaveManager.Start()` заменён немедленный вызов `StartNextWave()` на coroutine с `yield return null`. Это гарантирует, что `ArenaGenerator.Start()` → `GenerateArena()` завершится полностью перед запуском первой волны.

**Код:**
```csharp
private void Start()
{
    // ...
    StartCoroutine(StartFirstWaveNextFrame());
}

private IEnumerator StartFirstWaveNextFrame()
{
    yield return null;
    StartNextWave();
}
```

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | YES |
| Tested | **NO** |
| Confirmed | **NO** |

---

## 2026-09-18: Точный runtime root cause — непостроенная tactical map

**Описание:** Диагностические счётчики `EnemySpawner` показали:

```
[SPAWNER DIAG STRICT] valid=80 (inside=0, ground=0)
[SPAWNER DIAG RELAXED] valid=80
[SPAWNER DIAG FALLBACK1] valid=53
[SPAWNER DIAG FALLBACK2] valid=53
[SPAWNER DIAG ZONE] inside01=True, foundZone=та же зона
```

**Установлено:**
- `ArenaTacticalMap.Instance != null` (существует в сцене)
- Но `IsBuilt == false`, `Rooms.Count == 0` до первого спавна
- `IsValidEnemySpawnPosition()` видит `Instance != null` → `IsPositionWalkable()` → `FindRoom()` → null
- ВСЕ кандидаты отклоняются на `IsValidEnemySpawnPosition` во всех 4 путях
- Координаты зоны/комнаты корректны (`inside01=True`), проблема не в геометрии, а в непостроенной карте

| Статус | Значение |
|--------|----------|
| Runtime evidence | **YES** |
| Root cause confirmed | **YES** |

---

## 2026-09-18: WaveManager гарантирует Build() перед Wave 1

**Описание:** В существующей coroutine `StartFirstWaveNextFrame()` между `yield return null` и `StartNextWave()` добавлен гарантированный `Build()` tactical map. Задержка в один кадр, волны, spawn rules, fallback, AI — не менялись.

**Код:**
```csharp
private IEnumerator StartFirstWaveNextFrame()
{
    yield return null;

    if (ArenaTacticalMap.Instance != null &&
        !ArenaTacticalMap.Instance.IsBuilt)
    {
        ArenaTacticalMap.Instance.Build();
    }

    StartNextWave();
}
```

**Гарантия:** `GenerateArena()` завершён → `Build()` → tactical map содержит реальные комнаты → только потом `EnemySpawner` ищет позицию группы.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | NO (проверка невозможна при открытом Unity; синтаксис проверен чтением) |
| Tested | **NO** |
| Confirmed | **NO** |

---

## 2026-09-18: ArenaTacticalMap — направленный критерий соседства вместо порога 12 м

**Описание:** В `FindRoomNearConnection()` удалён фиксированный гейт `bestDistance > max(12, cellSize*8)`, отбрасывавший реальные коридорные связи при gaps 15–18 м. Новый критерий (без магических чисел): кандидат — ближайшая комната за плоскостью стены в направлении наружу от центра source через точку connection (`dot >= 0` в XZ); комнаты позади стены исключаются. Исключение source, nearest-wins, граф `Neighbours`, `FindRoomRoute()`/A* — без изменений. Никаких новых скриптов, чисел 18/19/20/24 не зашито. Диагностические логи оставлены.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | NO (проверка невозможна при открытом Unity; синтаксис проверен чтением) |
| Tested | **NO** |
| Confirmed | **NO** |

---

## 2026-09-18: EnemyController — однократный re-arm Planner search mode

**Описание:** `ClearSearchMode()` имеет единственную точку вызова (вход в Combat). Возврат в `Searching` обходными путями оставлял planner выключенным → `[SEARCH REACHED]`/`RequestNextSearchTargets()` не срабатывали. Добавлена bool-защёлка `plannerRearmRequested` (не state/enum/скрипт): взвод рядом с `ClearSearchMode()`, сработка один раз на первом кадре `UpdateSearchingState()` через `SetSearchTarget(transform.position)` при `planner != null && !SearchMode && !playerCurrentlyVisible`. `ClearSearchMode()`, `initialSearchStarted`, `searchPoints`, `RoomSearching`, A* — не менялись. Цикл исключён: защёлка сбрасывается до вызова, распределение её не взводит.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | NO (проверка невозможна при открытом Unity; синтаксис проверен чтением) |
| Tested | **NO** |
| Confirmed | **NO** |

---

## 2026-09-18: EnemyController — гейт StartInvestigation против перезаписи активной цели

**Описание:** Оба вызова `tacticalPlanner.SetSearchTarget(lastKnownPlayerPosition)` в `StartInvestigation()` (RoomSearching-ветка и Investigating-ветка) обёрнуты условием `tacticalPlanner == null || !tacticalPlanner.SearchMode`. Позиции/аргументы веток не менялись. Активная SearchTarget больше не перезаписывается при возвратах из Combat/cover; после `ClearSearchMode()` режим выключен → новая цель создаётся как раньше. `UpdateInitialBehavior()`, `RequestNextSearchTargets()`, `ClearSearchMode()`, Planner, `[SEARCH REACHED]`, движение, A* — не менялись.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | NO (проверка невозможна при открытом Unity; синтаксис проверен чтением) |
| Tested | **NO** |
| Confirmed | **NO** |

---

## 2026-09-18: EnemyController — неразрушающее обновление A* маршрута

**Описание:** `TryGetPathToTarget()` разделён на жёсткую перестройку (`hardRebuild`: путь пуст/завершён/сдвиг цели + emergency cooldown — как раньше через `BuildPathToTarget` с `Clear/index=0`) и мягкое обновление по таймеру (`RefreshPathIfBetter`): новый путь строится во временный список и принимается только если короче оставшейся длины текущего; иначе старый путь/индекс сохраняются, продлевается только дедлайн. Временный список создаётся только на истечении интервала 1.5 с. Emergency rebuild и cooldown, прямое движение <5 м, A* на ≥5 м, скорости, радиусы, avoidance, Planner, состояния — не менялись. Без правила «застрял = достиг», без искусственных `RequestNextSearchTargets()`.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | NO (проверка невозможна при открытом Unity; синтаксис проверен чтением) |
| Tested | **NO** |
| Confirmed | **NO** |

---

## 2026-09-18: EnemyController — гистерезис 15% + гейт выходов RoomSearching

**Описание (проблема 1):** В `RefreshPathIfBetter()` условие приёмки изменено на точное `candidateLength < remainingCurrent * 0.85f` — новый маршрут принимается только при улучшении минимум на 15% (40/39 и 40/35 — отклонить; 40/34 — принять). Мелкий дрейф больше не сносит прогресс: индекс/путь сохраняются, `Clear()`/`index=0` только при реальном улучшении, пустом/завершённом пути или сдвиге цели. Hard rebuild, emergency cooldown, интервал 1.5 с, A*, скорости, радиусы — без изменений.

**Описание (проблема 2):** Оба выхода из `RoomSearching` (в `Searching` и в `Investigating`) обёрнуты гейтом `tacticalPlanner == null || !tacticalPlanner.SearchMode` (тот же паттерн, что ранее в `StartInvestigation()`). Активная SearchTarget больше не перезаписывается завершением room-маршрута; новая цель назначается только при выключенном режиме. Переходы, движение, `RequestNextSearchTargets()`, `OnMemberDied()`, распределение — не менялись. Цепочка REACHED → RequestNext → SetSearchTarget — без изменений.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | NO (проверка невозможна при открытом Unity; синтаксис проверен чтением) |
| Tested | **NO** |
| Confirmed | **NO** |

---

## 2026-09-18: Waypoint <= + leaderless fallback (разрыв цикла REACHED/REQUEST)

**Описание (исправление 1, `EnemyController.cs`):** условие потребления вейпоинта `distance < waypointReachDistance` заменено на `distance <= waypointReachDistance` — ровно-1.20 теперь закрывает последний вейпоинт (`index` уходит в `finished`), устраняя рассинхрон со строгим/нестрогим прибытием. `searchPointReachDistance`, A*, скорости — без изменений.

**Описание (исправление 2, `EnemyTacticalPlanner.cs`):** в `RequestNextSearchTargets()` добавлен флаг `leaderFound`; если живой лидер в `groupMembers` не найден, вместо молчаливого return вызывается новый приватный `DistributeLocalSearchTarget()` — только существующие механики (`FindRoom`, список searchRoom+Neighbours, `SelectRoomForMember(this,...)`, `FindWalkablePositionInRoom`, `SetDirectSearchTarget` с логом, флаги `hasDistributedTargets/lastDistributedCenter`; при отсутствии карты/комнаты — существующий `DistributeFallbackSearchTargets`). Исключение только что достигнутой цели: текущий `searchTarget` кладётся в `existingTargets`, поэтому новая точка обязана отличаться минимум на `minimumMemberDistance`. Ветка «лидер найден», `DistributeSearchTargets()`, `StartInvestigation()`, `RoomSearching`, `OnMemberDied()` — не менялись. Новых состояний/скриптов нет.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | NO (проверка невозможна при открытом Unity; синтаксис проверен чтением) |
| Tested | **NO** |
| Confirmed | **NO** |

---

## 2026-09-18: Planner — перевыбор лидера на каждом Update

**Описание:** В `EnemyTacticalPlanner.Update()` после лидерной ветки добавлен вызов существующего `EnsureLeaderExists()` — выполняется каждым активным planner (вне лидерной ветки). После смерти лидера каждый выживший на своём refresh чистит мёртвые ссылки (`RemoveDeadMembers()` уже был рядом) и сходится на одном новом живом лидере через существующую логику (при живом лидере — no-op). `RequestNextSearchTargets()`, `DistributeLocalSearchTarget()`, `OnMemberDied()`, `OnDestroy()`, структура `groupMembers` — не менялись. Новых систем/состояний нет.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | NO (Unity запущен, batchmode заблокирован; синтаксис проверен чтением) |
| Tested | **NO** |
| Confirmed | **NO** |

---

## 2026-09-18: Быстрый routine-осмотр пустых комнат (Stage 3)

**Описание (только `EnemyController.cs`, без новых состояний/скриптов/таймерных систем):**
1. `UpdateSearchingState()`: вычисление `usingPlannerTarget` поднято выше скан-блока (чистые чтения, поведение то же); добавлен `routineQuickScan = usingPlannerTarget && !hadPlayerContact && !investigatingNoise` → длительность прибытийного скана 0.5 с (дуга ~150° вместо почти полного круга 1.15 с) с немедленным `RequestNextSearchTargets()` дальше. Локальные точки, пост-контакт, шум, ambush — прежние 1.15 с.
2. `UpdateRoomSearchingState()`: пауза у дверного вейпоинта `roomPause = roomSearchIsTargeted ? roomSearchPause (0.8) : 0.3f` — рутинный транзит делает короткую проверку лицом к выходу, охота за lastKnown-паузу не меняет. Ветки завершения, Investigating, Combat, Vision/LOS/hearing, A* (пути, 0.85f, радиусы, движение), распределение Planner — не менялись. Диагностические логи оставлены.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | NO (UnityLockfile занят, batchmode заблокирован; синтаксис проверен чтением) |
| Tested | **NO** |
| Confirmed | **NO** |

---

## 2026-09-18: Planner — индивидуальные цели, visitedRooms, без перераздачи

**Описание (только `EnemyTacticalPlanner.cs`):** `RequestNextSearchTargets()` больше не запускает полную `DistributeSearchTargets()`: помечает пройденную комнату в личном `visitedRooms` и назначает новую цель только запросившему через `AssignIndividualSearchTarget()` (приоритет: непосещённый сосед → непосещённая с карты → повторный цикл без только-что-покинутой; чужие активные targetRoom исключаются; детерминированно, без random). Остальные члены/пути не трогаются. `DistributeLocalSearchTarget()` заменён новой механикой (удалён как дублирующий). `OnMemberDied()` больше не перераздаёт (только cleanup + `EnsureLeaderExists()`). Новая цель — через `SetDirectSearchTarget` с `[SEARCH TARGET]`; достигнутая точка исключается через `existingTargets`. A*, навигация, восприятие, сканы 0.5/0.3, состояния — не менялись. Диагностические логи оставлены.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | NO (UnityLockfile занят, batchmode заблокирован; синтаксис проверен чтением) |
| Tested | **NO** |
| Confirmed | **NO** |

---

## 2026-09-18: Stage 3 зафиксирован как рабочий baseline

**Описание:** Текущая реализация принята за основу дальнейшей разработки (код не менялся при фиксации). Состав baseline: индивидуальное назначение целей (`RequestNext` только запросившему), `visitedRooms` с приоритетом непосещённых, исключение чужих активных targetRoom, восстановление лидера (`EnsureLeaderExists` в `Update`), направленный критерий соседства (без порога 12 м), неразрушающий A* refresh + гистерезис 0.85 + `<=` для вейпоинта, Build-guarantee перед Wave 1, ускоренный пустой поиск (скан 0.5 с, дверная пауза 0.3 с), гейты против перезаписи активной цели. Практика Unity: распределение стартовой тройки, самостоятельные переходы, A* между комнатами, следующие индивидуальные цели, продолжение поиска после смерти, корректный детект. Известные ограничения (не повод для отката): повторные REACHED-строки в радиусе, временное деление зоны парой, нужна полировка скорости/естественности.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | YES (Assembly-CSharp.dll новее всех исходников) |
| Tested | YES (практический тест в Unity) |
| Confirmed | **NO** |

---

## 2026-09-18: Stage 4.4.1–4.4.4 — WildWestEnvironment (Ground/Rocks/Cliffs/Vegetation)

**Описание:** Новый `WildWestEnvironmentGenerator` (`Assets/Scripts/Environment/`): seed + `System.Random`, root sibling `GeneratedArena`, топология из `ArenaTacticalMap` после `IsBuilt`, deterministic категории. 4.4.2: Ground (Plane 152×156 м, y=−0.05, `M_Ground_Desert`). 4.4.3: Cliffs=6 (`UNS_Rock_Cliff_*`), Rocks=32 (Forest+Tiny, фикс `rockMaxDistance` 45→75 по keep-out геометрии). 4.4.4: Trees=16, Bushes=48, Grass=255 (Collider off на инстансах); сэмплинг от границы bounds; независимый spacing; keep-out bounds + Exit-радиусы для всех. Preview: `TestArena_Preview.unity` (волны заглушены 0/0/99999, врагов нет, ошибок нет). Desert Pack dormant. TestArena, AI, карта не изменялись.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | YES |
| Tested | YES |
| Confirmed | **YES (только 4.4.4; 4.4.1–4.4.3 входят в состав, отдельно не подтверждались)** |

---

## 2026-09-21: Fence prefab + DestructibleObject + Fences pool (предшествующий Unity-проход, задокументировано сейчас)

**Описание:** `Assets/Prefabs/Environment/Fence_Western_Wooden.prefab` из Blender FBX (18 мешей, ~504 вершины): инстанс распакован (`UnpackPrefabInstance`, сам FBX не тронут), меши разложены в `State_Intact` (12) / `State_Damaged` (4 broken) / `State_Destroyed` (2 debris); по умолчанию активен только INTACT. Новый `Assets/Scripts/Environment/DestructibleObject.cs` — единственное ядро разрушаемости (HP, INTACT→DAMAGED→DESTROYED, массивы hide/show, UnityEvents, без Update; fence HP=50, debris-физика только 2 тела). `Bullet.cs` / `EnemyBullet.cs` — минимальные вставки `TakeDamage` через `GetComponentInParent<DestructibleObject>`. `CoverSystem.cs` — 2 строки guard `isActiveAndEnabled` в `GetBestCover` / `GetSearchPositions`. Fence: корневой BoxCollider, 2 CoverPoint (Front/Back), инстанс в `TestArena_Preview` привязан к prefab. `WildWestEnvironmentGenerator.cs` — категория Fences (дефолт-путь на fence-prefab, max 12, спавн вне bounds) + хук авторегистрации CoverPoint в `PlaceScatter`. Временные primitive-prefab'ы Barrel/Crate/Log/Plank Wall созданы как placeholders (заменены следующим проходом, см. ниже).

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | YES (0 errors) |
| Tested | PARTIAL (Edit-mode: стадии INTACT→DAMAGED→DESTROYED, debris, cover off, reset) |
| Confirmed | **NO** |

---

## 2026-09-21: Blender environment props — Unity-интеграция 7 реальных FBX

**Описание:** Отдельный Blender-проход создал 7 ассетов (`C:\Users\cyril\Documents\WildWestGunslinger art\art\Environment\<Name>\`: FBX + `_Source.blend` + `textures/Albedo + Normal`). В Unity скопированы только FBX+текстуры (`Assets/Art/Environment/<Name>/`, `.blend` не импортировались). Все 7 FBX: scale 1, Import normals, структурные части сохранены, Camera/Light/Collider внутри — нет. **Критическая находка: все 7 экспортированы Z-up** (фонарь «лежал» 0.82×0.37×2.67) — исправлено на уровне prefab (`Model` с rotation −90° X, геометрия FBX не тронута); проверка: все стоят на minY=0.00. Все 8 Albedo байт-идентичны (SHA256 `5F3921E1…`), все 8 Normal байт-идентичны (`4455C48E…`) — единый атлас сета. Все 7 `*_Normal.png` переключены на NormalMap. Созданы 4 shared-материала `Assets/Art/Environment/Shared/Materials/` (Wood_Western_Planks / Wood_Western_Dark / Metal_Dark_Western / Glass_Lantern_Warm, URP Lit; Glass — transparent + warm emission, realtime Light — 0). Prefab'ы (`Assets/Prefabs/Environment/`): Lantern (15 частей, HP 40, без CoverPoint), Barrel (6 частей, HP 30), Crate (10 частей, HP 40), Cover (8 частей, HP 50), Wagon (14 частей, HP 80, 2 BoxCollider), Log Wall (8 частей, HP 60), Plank Wall (14 частей, HP 60); у всех — DestructibleObject с массивами hide/show на структурных частях, 1 BoxCollider (wagon 2), CoverPoints (везде по 2, кроме lantern). **Замена primitive:** Barrel/Crate/Log/Plank перезаписаны по тем же путям с сохранением GUID (`1bbb3e2e`, `50242a99`, `3d75465e`, `df2cd228`); отсутствие ссылок из сцен проверено grep по `*.unity`. Генератор: пулы Props (Barrel/Crate/Cover, max 10), Lanterns (max 6), Wagons (max 4) — все вне комнат с keep-out/exit-защитой. Стены — PREPARED (prefab'ы готовы, стыковка plank-wall gap=0.000), НЕ scattered, в ручную сцену НЕ интегрированы. Розовых материалов нет. Сцена сохранена, временных объектов не осталось.

| Статус | Значение |
|--------|----------|
| Implemented | YES |
| Compiled | YES (0 errors, только посторонний шум `NoSubscription` в консоли) |
| Tested | PARTIAL (Edit-mode: bounds/pivot/материалы/коллайдеры/CoverPoints/стадии barrel/smoke-тест стыковки; Play-тест НЕ проводился) |
| Confirmed | **NO** |

**Ручные проверки (ожидают пользователя):** Play `TestArena_Preview`; визуал всех 7 моделей; стрельба по destructible-пропам; использование wagon/cover AI как укрытий; стыковка 2–3 сегментов стен. **Не подтверждено и не исправлено:** intermittent FPS ~30 (в этой сессии не наблюдался); гипотеза про CoverPoint sphere-маркеры — неподтверждена.

---

## Итого

| Этап | Implemented | Compiled | Tested | Confirmed |
|------|-------------|----------|--------|-----------|
| Stage 1 | YES | YES | YES | **YES** |
| Stage 2 | YES | YES | YES | **YES** |
| Stage 3 (промежуточный, до bundle — superseded строкой ниже) | YES | YES | **PENDING** | **NO** |
| WaveManager timing fix + Build() guarantee (история, входит в bundle) | YES | **NO** (pending Unity на момент записи) | **NO** | **NO** |
| ArenaTacticalMap neighbour criterion fix (история, входит в bundle) | YES | **NO** (pending Unity на момент записи) | **NO** | **NO** |
| EnemyController planner re-arm fix (история, входит в bundle) | YES | **NO** (pending Unity на момент записи) | **NO** | **NO** |
| EnemyController StartInvestigation guard fix (история, входит в bundle) | YES | **NO** (pending Unity на момент записи) | **NO** | **NO** |
| EnemyController non-destructive path refresh fix (история, входит в bundle) | YES | **NO** (pending Unity на момент записи) | **NO** | **NO** |
| EnemyController hysteresis 15% + RoomSearching guard fix (история, входит в bundle) | YES | **NO** (pending Unity на момент записи) | **NO** | **NO** |
| Waypoint <= + leaderless fallback fix (история, входит в bundle) | YES | **NO** (pending Unity на момент записи) | **NO** | **NO** |
| Planner per-update leader re-election fix (история, входит в bundle) | YES | **NO** (pending Unity на момент записи) | **NO** | **NO** |
| Planner individual targets + visitedRooms fix (история, входит в bundle) | YES | **NO** (pending Unity на момент записи) | **NO** | **NO** |
| Fast empty-room sweep (routine scan/pause) fix (история, входит в bundle) | YES | **NO** (pending Unity на момент записи) | **NO** | **NO** |
| Stage 3 working baseline (bundle above — ТЕКУЩИЙ статус Stage 3) | YES | YES | YES | **NO** |
| Stage 4.4.1–4.4.3 Environment (skeleton/Ground/Rocks/Cliffs) | YES | YES | YES | **NO** (входят в 4.4.4) |
| Stage 4.4.4 Trees/Bushes/Grass | YES | YES | YES | **YES** |
| Fence prefab + DestructibleObject + Fences pool | YES | YES | PARTIAL (Edit-mode) | **NO** |
| Blender props: 7 FBX → Unity prefabs (rep. 4 primitives, GUID kept) | YES | YES | PARTIAL (Edit-mode) | **NO** |
| Generator pools Props/Lanterns/Wagons + shared materials | YES | YES | PARTIAL (Edit-mode) | **NO** |
| Wall segments modular (PREPARED, not scattered) | YES | YES | PARTIAL (joint gap=0.000) | **NO** |
