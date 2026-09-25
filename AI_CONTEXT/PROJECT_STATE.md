# PROJECT_STATE — Текущее состояние проекта

## Общая информация

| Параметр | Значение |
|----------|----------|
| Название проекта | WildWestGunslinger |
| Движок | Unity 6000.3.23f1 (Unity 6) |
| Путь проекта | `C:\Users\cyril\WildWestGunslinger` |
| Целевые платформы | Android + Windows |
| Язык AI-ассистента | Русский |

## CURRENT PROJECT CHECKPOINT

| Параметр | Значение |
|----------|----------|
| CHECKPOINT | Stage 4.4.5, FROZEN / PAUSED |
| Последняя подтверждённая стадия | Stage 4.4.4 (Trees + Bushes + Grass) |
| Статус 4.4.5 | checkpoint/focus frozen; дальнейшее продвижение по Stage временно приостановлено из-за необходимости подготовить 3D dependencies (ранее в MD: NOT STARTED — расхождение зафиксировано, техническая реализация 4.4.5 не утверждается) |
| Текущая ветка | Отдельная подготовительная production-ветка Blender / 3D assets (НЕ новый Stage, номер checkpoint не меняется) |
| Назначение ветки | Подготовка зависимостей для будущих этапов: Environment assets, procedural placement, reusable prefab-ready assets, destructible environment assets, character foundation / character art |
| Возврат к Stage-плану | После готовности необходимых assets/pipeline и отдельного решения по checkpoint. Будущие Stage 5/8/9 НЕ считаются выполненными из-за подготовки assets |
| Art-мост | См. `AI_CONTEXT/ART_PIPELINE.md` + `.opencode/skills/wwg-character-art/SKILL.md` |

## Структура проекта

```
Assets/
├── Scripts/
│   ├── Core/           — GameSettings, DifficultyManager, MainMenuController, PauseMenuController, SettingsController
│   ├── Enemies/        — EnemyController, EnemyHealth, EnemyHealthBar, EnemyTacticalPlanner,
│   │                     EnemyTacticalVision, EnemyHearing, NoiseSystem, EnemyBullet, CombatPhysics
│   │   └── Cover/      — CoverSystem, CoverPoint
│   ├── Level/          — WaveManager, EnemySpawner, EnemySpawnZone, ArenaVisualStyle
│   ├── Performance/    — MobilePerformanceTest
│   ├── Player/         — PlayerController, PlayerHealth, CameraFollow, MobileJoystick, MobileTouchControls,
│   │                     XPManager, XPOrb, XPBarController, UpgradeManager, UpgradeUI
│   ├── Procedural/     — ArenaGenerator, ArenaModule, ArenaTacticalMap
│   ├── UI/             — HUDController, DeathUIController, EnemyDirectionIndicator, SettingsUIManager
│   └── Weapons/        — GunController, Bullet
├── Builds/
├── Library/
├── Logs/
├── Packages/
├── ProjectSettings/
└── UserSettings/
```

Всего: **41 .cs файл** в 8 поддиректориях.

## Процедурная арена

| Параметр | Значение |
|----------|----------|
| Тип раскладки | Honeycomb (3x3 сетка) |
| Количество комнат | 9 (фиксировано) |
| Размер комнат | 16–22 × 16–22 единиц |
| Ширина проходов | 3.5–5.0 |
| Длина коридоров | 4.0–6.0 |
| Высота стен | 2.4–3.2 |
| Толщина стен | 0.4–0.8 |
| Генерация при запуске | Да (`generateOnStart = true`) |
| Случайный seed | Да (`useRandomSeed = true`) |

### Типы комнат (по прогрессу)

| Прогресс | Combat | Tactical | Elite |
|----------|--------|----------|-------|
| Начало (index 0) | Start | — | — |
| < 0.3 | 70% | 30% | 0% |
| 0.3–0.7 | 45% | 35% | 20% |
| > 0.7 | 25% | 40% | 35% |
| Конец (последний) | Final | — | — |

### Покрытие (cover)

- 3–5 покрытий на комнату
- Tactical: +2, Elite: +1, Final: +3
- Максимум: `maximumCoverCount + 3` = 8

### Зоны спауна

- 3–5 зон на арену
- Радиус: 1.5–3.0
- Минимальное расстояние между зонами: 4.0

## Волновая система

| Параметр | Значение |
|----------|----------|
| Стартовые враги | 3 |
| Прирост за волну | +2 |
| Задержка между волнами | 3 сек |
| Максимум волн | не ограничен |

Формула: `враги = 3 + (волна - 1) * 2`

## Типы врагов

| Тип | Волна появления | Шанс спауна | Скорость | Дистанция обнаружения | Урон (ближний/дальний) |
|-----|----------------|-------------|----------|----------------------|----------------------|
| Bandit | всегда | остаток | 2.5 | 16 | 10 / 6 |
| Shooter | 3 | 0.25 | 2.2 | 20 | 10 / 7 |
| Rusher | 4 | 0.15 | 3.375 | 20 | 15 / 5 |
| Tactical | 5 | 0.10 | 2.7 | 22 | 10 / 6 |

### Элитные враги

- Волна появления: 5
- Шанс: 0.15
- Множитель HP: x2
- Множитель урона: x1.5
- Множитель скорости: x1.15
- Множитель XP: x3

## Масштабирование по волнам

| Параметр | Формула |
|----------|---------|
| HP врагов | `1 + (волна-1) * 0.08`, макс. x3 |
| Урон врагов | `1 + (волна-1) * 0.05`, макс. x2 |

## Тактическая карта (ArenaTacticalMap)

| Параметр | Значение |
|----------|----------|
| Размер ячейки | 1.5 |
| Радиус агента | 0.65 |
| Макс. узлов пути | 2500 |
| Алгоритм | A* (4-направленные соседи, без диагоналей) |
| Порог включения A* | 5.0 единиц от цели |

## Игрок

| Параметр | Значение |
|----------|----------|
| Скорость ходьбы | 3.0 |
| Скорость бега | 5.0 |
| Тихая скорость | 1.35 |
| HP | 100 |

## Статус разработки AI

| Этап | Статус |
|------|--------|
| Stage 1 — Интеграция EnemyTacticalPlanner с EnemyController | CONFIRMED |
| Stage 2 — A* навигация | CONFIRMED |
| Stage 3 — Групповое распределение целей поиска | BASELINE (Implemented=YES, Compiled=YES, Tested=YES, Confirmed=NO) |
| Stage 4.4.1 — Скелет WildWestEnvironmentGenerator | DONE (not confirmed) |
| Stage 4.4.2 — Ground | DONE (not confirmed) |
| Stage 4.4.3 — Rocks + Cliffs | DONE (not confirmed) |
| Stage 4.4.4 — Trees + Bushes + Grass | **CONFIRMED** |
| Stage 4.4.5 — Background | **FROZEN / PAUSED** (ранее: NOT STARTED; технической реализации нет, см. CURRENT PROJECT CHECKPOINT) |

## WildWestEnvironment (Stage 4.4)

Новый компонент `Assets/Scripts/Environment/WildWestEnvironmentGenerator.cs`: собственный seed (`System.Random`), root `WildWestEnvironment` (sibling `GeneratedArena`), чтение топологии из `ArenaTacticalMap` (Rooms/bounds/Exits), генерация после `IsBuilt`, deterministic, категории вкл/выкл. Preview-сцена: `Assets/Scenes/TestArena_Preview.unity` (рабочая TestArena не тронута).

Runtime 4.4.4: Ground=1 (`M_Ground_Desert`, 152×156 м, y=−0.05), Cliffs=6 (`UNS_Rock_Cliff_*`), Rocks=32 (`UNS_Standard_Rock_*` + `UNS_Tiny_Rock_*`, rockMaxDistance исправлен 45→75), Trees=16 (`UNS_Spruce_01/02`), Bushes=48 (`UNS_Bush`), Grass=255 (`UNS_Grass`, Collider отключён на инстансах). Сэмплинг растительности — от границы bounds; spacing независимый по категориям. Desert Pack dormant до URP-конверсии `Mat_01`. Background не реализован.

## Blender environment props (Unity-интеграция 2026-09-21, DONE — not confirmed)

Источник моделей: `Documents\WildWestGunslinger art\art\Environment\<Name>\` (FBX + `_Source.blend` + `textures/`); в Unity импортированы только FBX+текстуры. Fence FBX не переделывался.

| Prefab (`Assets/Prefabs/Environment/`) | Части | HP | CoverPoints |
|---|---|---|---|
| `Fence_Western_Wooden.prefab` (States INTACT 12 / DAMAGED 4 / DESTROYED 2 debris) | 18 (~504 v) | 50 | 2 |
| `Lantern_Post_Western.prefab` (Glass/Bulb/frame, света нет) | 15 (338 v / 160 t) | 40 | 0 (декор) |
| `Barrel_Western_Wooden.prefab` (Staves/Hoops/Lids) | 6 (456 v / 268 t) | 30 | 2 |
| `Crate_Western_Wooden.prefab` (Corners/Panels) | 10 (230 v / 110 t) | 40 | 2 |
| `Cover_Western_Wooden.prefab` (Posts/Feet/Planks/TopLog) | 8 (221 v / 113 t) | 50 | 2 |
| `Wagon_Western_Wooden.prefab` (Wheels/Axles/Bed/Walls/Bench/Shafts, 2 BoxCollider) | 14 (568 v / 320 t, самый тяжёлый) | 80 | 2 |
| `Western_Log_Wall_Segment.prefab` — PREPARED, не scattered | 8 (416 v / 236 t) | 60 | 2 |
| `Western_Plank_Wall_Segment.prefab` — PREPARED, не scattered | 14 (322 v / 154 t) | 60 | 2 |

Ключевые факты: все 7 FBX были Z-up → prefab-level коррекция `Model` rotation −90° X (minY=0.00 у всех); все Albedo байт-идентичны, все Normal байт-идентичны, Normal-импортёры = NormalMap; shared-материалы `Art/Environment/Shared/Materials/` (4 шт., URP Lit); в FBX нет Camera/Light; коллайдеры — 1 Box на проп (wagon 2), Rigidbody только 2 fence-debris (kinematic, parked); Barrel/Crate/Log/Plank перезаписаны с сохранением GUID (ссылки из сцен проверены — отсутствуют); генератор: пулы Fences (max 12), Props (max 10), Lanterns (max 6), Wagons (max 4), всё вне комнат; realtime lights — 0; per-frame destruction-логики нет; FPS ~30 intermittent — НЕ подтверждён, гипотеза CoverPoint-маркеров — неподтверждена. Ручной Play-тест (визуал, стрельба, cover-AI, стыковка стен) — ожидает пользователя.

Принцип подготовки (без изменения checkpoint): один качественно подготовленный asset должен быть пригоден для многократного процедурного использования, разных позиций/ориентаций/вариантов размещения и последующей интеграции в Unity. Коллайдеры/Rigidbody — минимальные и контролируемые (Android). Стены — PREPARED, не DONE. Это подготовка будущего Stage 5, НЕ завершение Environment Stage. Детали — `AI_CONTEXT/ART_PIPELINE.md`.

## Рабочий baseline Stage 3 (зафиксирован как основа дальнейшей разработки)

Наблюдения практического теста в Unity:

1. Трое врагов на старте получают разные SearchTarget/targetRoom и расходятся по арене.
2. Враги самостоятельно переходят между комнатами без шума и визуального контакта.
3. A* ведёт между комнатами и через проходы корректно.
4. После достижения цели враг получает следующую индивидуальную цель.
5. Обычный SEARCH REACHED не перезаписывает цели других членов (только запросивший).
6. Индивидуальная память `visitedRooms` на планировщик.
7. При выборе учитываются чужие активные targetRoom (меньше конкуренции).
8. Лидер восстанавливается через `EnsureLeaderExists()` после смерти.
9. Ускоренный пустой поиск: первичный скан 0.5 с, дверная пауза 0.3 с.
10. Шум/Investigation/lastKnown/контакт/Combat — прежнее тщательное поведение.
11. Детект корректен: проход мимо при взгляде в сторону + обнаружение при досканировании (норма).
12. Боевой тест: самостоятельный выход в центр, пропуск игрока вне взгляда, детект и атака.
13. После смерти одного оставшиеся продолжают поиск и получают новые цели.

## Известные ограничения baseline (не повод для отката)

- Повторные `[SEARCH REACHED]`-строки внутри радиуса достижения.
- Пары врагов иногда делят одну зону некоторое время.
- Скорость/естественность обхода требуют дальнейшей полировки.

## Не ломать без необходимости

Stage 1, Stage 2, A*, `RefreshPathIfBetter()` (0.85f), индивидуальные цели, `visitedRooms`, `EnsureLeaderExists()`, Vision/LOS, Hearing/Noise, Combat/Investigation, ускоренный пустой поиск.
