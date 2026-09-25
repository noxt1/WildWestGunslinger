# CURRENT_TASK — Текущая задача

## CHECKPOINT: Stage 4.4.5, FROZEN / PAUSED

Последняя подтверждённая стадия: Stage 4.4.4. Продвижение по Stage временно приостановлено — готовятся 3D dependencies (см. `PROJECT_STATE.md` → CURRENT PROJECT CHECKPOINT и `ART_PIPELINE.md`). Номер checkpoint не меняется, будущие Stage 5/8/9 не считаются выполненными.

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
