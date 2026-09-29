> **Status:** HISTORICAL CONFIRMATIONS - reconciled 2026-09-29 (Phase 4)
> **Role:** records systems the **project owner explicitly confirmed** in earlier sessions. These are *dated historical confirmations*, not 2026-09-29 runtime evidence.
> **Canonical current state:** ../Documentation/PROJECT_STATE.md (runtime-verified) · ../Documentation/GAME/GAME-0002-GAMEPLAY-SYSTEMS-STATE.md section 3.2
> **CONFLICTS:** this file's mobile-UI confirmation conflicts with UNI-D07; see ../Documentation/History/OPEN_CONFLICTS.md CONFLICT-13. Script-inventory claims here are stale - see CONFLICT-11.
> **Only the user may set CONFIRMED** (VERIFICATION.md).
> **Reconciliation:** ../Documentation/PHASE4_AI_CONTEXT_RECONCILIATION.md
> Superseded as canonical state on 2026-09-29. Retained as the **confirmation record**.

# CONFIRMED_STATE — Подтверждённые системы

## Важно

Этот документ содержит **только** системы и этапы, которые пользователь **явно подтвердил** как работающие. Если систему невозможно достоверно подтвердить — она не включается.

## AI-разработка

### Stage 1 — Интеграция EnemyTacticalPlanner с EnemyController

**Статус: CONFIRMED**

Пользователь подтвердил работоспособность.

Реализация:
- `tacticalPlanner` поле в `EnemyController`
- `GetComponent<EnemyTacticalPlanner>()` в `Awake()`
- `tacticalPlanner.SetThreatPosition()` в `UpdatePerception()`
- `GetCombatTargetPosition()` предпочитает Planner target

### Stage 2 — A* навигация

**Статус: CONFIRMED**

Пользователь подтвердил/принял реализацию A* навигации.

Реализация:
- `currentPathWaypoints`, `currentPathIndex`, `pathTargetPosition` поля
- `TryGetPathToTarget()`, `BuildPathToTarget()`, `MoveDirectlyToTarget()` методы
- A* для дистанций >= 5м, прямое движение для < 5м
- `ClearNavigationPath()` в переходах состояний

### Stage 3 — Групповое распределение целей поиска

**Статус: NOT CONFIRMED (рабочий baseline)**

Финальное подтверждение пользователем НЕ объявлено. Зафиксировано как baseline:
Implemented=YES, Compiled=YES, Tested=YES (рабочее поведение наблюдалось в Unity), Confirmed=NO.

### Stage 4.4.4 — Trees + Bushes + Grass (WildWestEnvironment)

**Статус: CONFIRMED**

Пользователь подтвердил по runtime Preview (`TestArena_Preview.unity`):
Ground=1, Cliffs=6, Rocks=32, Trees=16, Bushes=48, Grass=255 — только UNS-префабы
(`UNS_Spruce_01/02`, `UNS_Bush`, `UNS_Grass`), без Desert Pack.
Подтверждено как часть: индивидуальный `visitedRooms`-обход MAINTAINED (Stage 3 baseline),
сэмплинг растительности от границы bounds, независимый spacing по категориям,
Collider травы отключён только на инстансах, префабы/материалы не изменялись.
Примечание: подэтапы 4.4.1–4.4.3 отдельно не подтверждались, но входят в проверенный состав 4.4.4.

Подтверждённые практикой элементы (не путать с формальным Confirmed): стартовое распределение по разным комнатам, самостоятельные переходы, A* между комнатами, индивидуальные следующие цели, `visitedRooms`, учёт чужих targetRoom, восстановление лидера, ускоренный пустой поиск (0.5 с / 0.3 с), корректный детект (промах вне взгляда + детект при досканировании).

## Blender 3D assets (подтверждено пользователем)

Подтверждение относится **только к Blender-части** (model + QA + final FBX). Unity-интеграция этих assets **не выполнялась** и подтверждением не покрывается.

| Asset | Статус | Состав | Подтверждённые результаты |
|---|---|---|---|
| **Barrel_01** | **CONFIRMED (Blender asset FINAL)** | stylized low-poly western, dark oak, dark old iron, rivets; Method C | 12 независимых fragments; intact 580 tris, fragments 900 tris; strict FBX separation, reimport validation, reassembly — PASS |
| **Fence_01** | **CONFIRMED (Blender asset FINAL)** | stylized low-poly western, dark oak, dark old iron, nails; Method C | 13 независимых fragments; intact 668 tris, fragments 1500 tris; strict FBX separation, reimport validation, reassembly — PASS |

Файлы и полные числа — `AI_CONTEXT/ART_PIPELINE.md` → «Blender asset registry».

**Подтверждённое правило presentation state** (выведено из практики и подтверждено пользователем): fragments скрыты во вьюпорте (`hide_viewport = TRUE`), `hide_render = FALSE`, intact видим. Одновременное отображение intact и reassembled fragments на совпадающих поверхностях вызывает «дёрганье» wood surface — это подтверждённый root cause, а не гипотеза.

## Игровые системы

Ниже перечислены системы, которые работают на основе подтверждений пользователя в предыдущих сессиях.

### Основные системы

| Система | Статус |
|---------|--------|
| Главное меню | CONFIRMED |
| Пауза | CONFIRMED |
| Рестарт | CONFIRMED |
| Мобильное UI (joystick, touch controls) | CONFIRMED |
| Управление игроком (движение) | CONFIRMED |
| Стрельба игрока | CONFIRMED |
| Bullet (пули) | CONFIRMED |
| XP (опыт) | CONFIRMED |
| Level Up (повышение уровня) | CONFIRMED |
| UpgradePanel (панель улучшений) | CONFIRMED |
| WaveManager (волновая система) | CONFIRMED |
| EnemySpawner (спаун врагов) | CONFIRMED |

### AI-системы

| Система | Статус |
|---------|--------|
| EnemyController (state machine) | CONFIRMED (Stage 1+2) |
| EnemyTacticalPlanner (роли, группы) | CONFIRMED (Stage 1) |
| A* навигация | CONFIRMED (Stage 2) |
| EnemyTacticalVision (восприятие) | EXISTS (не изменялась) |
| EnemyHearing (слух) | EXISTS (не изменялся) |
| NoiseSystem (шумы) | EXISTS (не изменялся) |
| CoverSystem (укрытия) | EXISTS (не изменялся) |
| ArenaTacticalMap (A*) | EXISTS (не изменялась) |

### Процедурная генерация

| Система | Статус |
|---------|--------|
| Honeycomb арена (9 комнат) | EXISTS |
| Генерация покрытий | EXISTS |
| Генерация зон спауна | EXISTS |

## НЕ подтверждено (не переносить в CONFIRMED без пользователя)

- Stage 3 baseline, Fence/Destructible/пулы, 7 Blender FBX→prefabs, стены PREPARED, character/blender plans, Stage 4.4.5 и будущие Stage 5/8/9. Техническая готовность ≠ подтверждение.
- **CRATE_01** — CP1 COMPLETE, но **пользователем не подтверждён** (AWAITING APPROVAL). Не вносить в подтверждённые.
- **CRATE_01 CP2 (fracture)** — не авторизован.
- **Unity-интеграция Barrel_01 / Fence_01** — не выполнялась; ждёт отдельного пакетного этапа.
- **Модульный wall/floor kit** (`WallSegment ≈ 4 m`, `FloorSegment ≈ 4 × 4 m`) и WESTERN WALL / FLOOR материалы — план, не реализовано.

## Статусы

- **CONFIRMED** — пользователь явно подтвердил работоспособность
- **EXISTS** — система существует, не изменялась в рамках AI-разработки
- **NOT CONFIRMED** — ожидает тестирования/подтверждения
- **UNKNOWN** — невозможно достоверно подтвердить