> **Status:** OPERATIONAL / ART WORKFLOW LAYER - reconciled 2026-09-29 (Phase 4)
> **Role:** **authoritative for art workflow rules** (Method C, material contract, FBX export contract, presentation state, batch-integration policy). These are *operational* and are NOT duplicated into PROJECT_TRUTH.
> **Canonical art state:** ../Documentation/ART/ART-0001-ART-STATE.md · asset states ../Documentation/ART/ART-0002-ART-ASSET-REGISTER.md · gates ../Documentation/ART/ART-0003-ART-PIPELINE-AND-QA-GATES.md
> **Character quality rules:** .opencode/skills/wwg-character-art/SKILL.md (state sections there are unverified - see ../Documentation/AI/AI-0001-AI-AGENT-STATE.md section 2.1)
> **Reconciliation:** ../Documentation/PHASE4_AI_CONTEXT_RECONCILIATION.md
> Superseded as canonical project state on 2026-09-29. Retained as the **operational art workflow** source.

# ART_PIPELINE — Blender / 3D asset production мост

Мост: `AI_CONTEXT` ↔ OpenCode ↔ Blender MCP ↔ character/environment asset work.
Character quality rules — только `.opencode/skills/wwg-character-art/SKILL.md` (не копируются сюда).

## Current art checkpoint

- Основной checkpoint: Stage 4.4.5, FROZEN / PAUSED. Последняя подтверждённая: 4.4.4.
- Текущая Blender-ветка — подготовка зависимостей будущих этапов, НЕ новый Stage. Номер checkpoint не меняется. Stage 5/8/9 не выполнены.
- Возврат к Stage-плану — после готовности assets/pipeline и отдельного решения.

## Current art focus

Подготовка assets для: Environment assets, procedural environment placement, reusable prefab-ready assets, destructible environment assets, character foundation / character art.

Принцип: один качественно подготовленный asset — многократное процедурное использование, разные позиции/ориентации/варианты размещения, последующая интеграция в Unity.

## Blender workflow (уже рабочий, не план)

OpenCode Desktop → Blender MCP → Blender → 3D asset → visual/numeric QA → при необходимости correction → повторная проверка → позднее Unity integration.

## QA rules

Петля: MEASURE → MODEL → PLACE/FIT → BLENDER SCREENSHOT/RENDER → VISION CHECK → FIND DEFECTS → CORRECT → NEW RENDER → VISION RECHECK.

Правило: NUMERIC PASS + VISION PASS = PASS. Только numeric = не готово. Только Vision = не готово.

## Modeling method — Method C (Component / Controlled Fracture)

Применяется ко всем environment assets. Обязательные ограничения:

- Модель constructively собирается из **реальных элементов** (доски, стойки, брёвна, рамы, крепёж). Произвольная геометрия (Voronoi и т.п.) не используется.
- Fracture = разделение **по реальной конструкции**, а не нарезка на куски ради количества.
- Cell Fracture, Voronoi-fracture, add-ons и physics simulation **запрещены**.
- На intact-стадии random destruction **не делается** (лёгкая вариативность размеров/тона/зерна — да).
- Каждый CP завершается отдельным отчётом и **ждёт явной авторизации пользователя на следующий CP**.

## Blender asset registry (production state)

Источник истины по 3D-ассетам — этот раздел + `PROJECT_STATE.md`. Статусы: `FINAL / APPROVED` = пользователь подтвердил; `CP1 COMPLETE — AWAITING APPROVAL` = работа пользователем не подтверждена.

### BARREL_01 — FINAL / APPROVED (пользователь подтвердил)

| Параметр | Значение |
|---|---|
| Working .blend | `Working\blender_src\barrel\Barrel_01_Working.blend` |
| Final FBX | `Assets\Art\Props\Destructible\Barrel_01_Intact_FINAL.fbx`, `Barrel_01_Fragments_FINAL.fbx` |
| Стиль | stylized low-poly western, dark oak, dark old iron, rivets |
| Метод | Method C, 12 независимых fragments |
| Intact / Fragments tris | 580 / 900 |
| FBX separation / reimport / reassembly | strict separation PASS, reimport PASS, reassembly PASS |

**Подтверждённый root cause «дёрганья» wood surface:** одновременное отображение intact и reassembled fragments на совпадающих поверхностях во вьюпорте Blender. Не material, не геометрия, не TAA.

### FENCE_01 — FINAL / APPROVED (пользователь подтвердил)

| Параметр | Значение |
|---|---|
| Working .blend | `Working\blender_src\fence\Fence_01_Working.blend` |
| Final FBX | `Assets\Art\Props\Destructible\Fence_01_Intact_FINAL.fbx`, `Fence_01_Fragments_FINAL.fbx` |
| Стиль | stylized low-poly western, dark oak, dark old iron, nails |
| Метод | Method C, 13 независимых fragments |
| Intact / Fragments tris | 668 / 1500 |
| FBX separation / reimport / reassembly | strict separation PASS, reimport PASS, reassembly PASS |

### CRATE_01 — CP1 COMPLETE — AWAITING APPROVAL (НЕ подтверждён)

| Параметр | Значение |
|---|---|
| Working .blend | `Working\blender_src\crate\Crate_01_Working.blend` |
| Текущий объект | `Crate_01_Intact` (1 intact mesh) |
| Collections | `Crate_Intact` (1 объект), `Crate_Fragments` (**пустая**) |
| Dimensions | 1.000 × 0.750 × 0.757 m (высота +0.007 = выступы nail heads) |
| Intact tris | 740 (бюджет 400–800) |
| Geometry QA | 0 ngons / 0 loose / 0 duplicate verts / 0 non-manifold / **0 wood-to-wood intersections** |
| Material | `MAT_crate_01_wood_metal`, 1 слот, `Col` FLOAT_COLOR/CORNER, UVMap |
| Стиль | dark oak + dark iron fasteners, constructive western crate |

**CP2 НЕ авторизован.** Точная точка остановки: `CRATE_01 CP1 COMPLETE — AWAITING USER APPROVAL`. Не начинать CP2 молча — ждать явной авторизации пользователя.

Заготовка дизайна CP2 (не исполнено): ~13–14 конструктивно верных fragments (front ×2, rear ×2, left ×2, right ×2, lid ×2, base ×1, corner posts ×4 или парами).

## Принятые технические решения по 3D-ассетам

Эти решения выведены из практики Barrel_01 / Fence_01 / Crate_01 и являются прецедентами для следующих assets.

| Решение | Правило |
|---|---|
| Vertex attribute | **`Col` = `FLOAT_COLOR` / `CORNER`.** `BYTE_COLOR` использовать **НЕЛЬЗЯ**: он хранит sRGB, и записанное линейное значение читается обратно в ~12 раз темнее (0.074 → 0.0063). Это реально ломало читаемость wood. |
| Семантика `Col` | RGB = базовый тон элемента (деревянный/металлический), **A = metal mask** (0 = wood, 1 = metal). `Metallic` берётся напрямую из A. |
| Единый металл | dark old iron = линейный `(0.0785, 0.0794, 0.0830)` — **одинаков для всех assets**. |
| Единое дерево | dark oak, hue ratio ≈ `1 : 0.77 : 0.567`, линейный диапазон ≈ 0.064–0.082. Wood value в рендере sRGB p50 ≈ 0.41–0.49 (Barrel 0.414, Fence 0.482, Crate 0.493). |
| UV | world-proportional, per-face planar, **U вдоль длинной оси элемента** (доски/брёвна — вдоль длины, стойки — вертикально). Зерно должно быть непрерывным между fragment boundaries. |
| Material slots | ровно **1** слот на каждый объект (intact и каждый fragment). |
| Image textures | **запрещены** на текущем этапе. M2 = vertex attributes + simple PBR. |

## FBX export contract (frozen)

Применять ко всем экспортам без отклонений:

```
use_selection = TRUE          object_types = {'MESH'}
use_mesh_modifiers = TRUE     global_scale = 1.0
apply_scale_options = 'FBX_SCALE_NONE'    bake_space_transform = FALSE
axis_forward = '-Z'          axis_up = 'Y'
colors_type = 'SRGB'         mesh_smooth_type = 'OFF'
use_triangles = FALSE        embed_textures = FALSE
bake_anim = FALSE
use_visible = НЕ ИСПОЛЬЗОВАТЬ
```

- Только **explicit selection**. Никогда не полагаться на viewport visibility.
- Существующие FBX **не перезаписывать и не удалять** — каждый новый экспорт получает новое имя (`_FINAL`, `_r2`).
- Известный нюанс: `mesh_smooth_type = 'OFF'` не переносит flat shading — reimport возвращает всё smooth. Это зафиксированное поведение frozen-контракта, **не** дефект экспорта; менять настройку без доказанного Unity-side дефекта нельзя.
- Пустые material nodes после reimport — **ожидаемо** и ошибкой не считается: FBX переносит `Col`, `UVMap`, слот и имя материала, но не procedural node graph.

## Viewport presentation rule (обязательная)

Default presentation state для каждого destructible asset:

| Collection | `hide_viewport` | `hide_render` |
|---|---|---|
| `<Name>_Intact` | FALSE | FALSE |
| `<Name>_Fragments` | **TRUE** | FALSE |

- Fragments **не удаляются** и **не исключаются** из blend — они доступны для exploded QA, fracture inspection и FBX export.
- Render visibility **не меняется**: fragments остаются видимыми для рендера.
- **Причина правила (подтверждено на Barrel_01):** одновременное отображение intact и reassembled fragments на совпадающих поверхностях вызывает визуальное «дёрганье/рябь» wood surface при вращении камеры. Материал, геометрия и TAA ни при чём.

## Unity integration policy — ТОЛЬКО ПАКЕТНО

Unity-интеграция environment assets **не выполняется поштучно**.

Текущий workflow:

```
Blender model → Blender QA (NUMERIC + VISION) → fracture preparation → final FBX → [ПАКЕТНЫЙ ЭТАП] Unity integration
```

Когда запланированный набор environment-моделей будет завершён, выполняется **один отдельный пакетный этап Unity-интеграции для всех готовых assets**. Он включает:

- FBX import;
- normals / smoothing verification;
- materials;
- vertex colors;
- colliders;
- prefabs;
- destructible setup;
- runtime destruction;
- gameplay integration;
- performance validation.

**Правило:** нельзя начинать Unity-интеграцию только потому, что отдельный Blender asset достиг статуса FINAL. Отдельный asset FINAL означает «Blender-часть завершена и подтверждена», не более.

## НОВАЯ АРХИТЕКТУРА: модульный western log/timber wall kit

**Направление:** процедурная арена должна уйти от визуально плоских cube-стен. `ArenaGenerator` сейчас создаёт геометрию стен процедурно из `Cube` и назначает один временный runtime-материал.

**Целевой визуал:** WESTERN LOG / TIMBER WALL.

**Базовый модуль:** `WallSegment ≈ 4 m` длиной. Внутри **одного** меша:

- несколько горизонтальных брёвен (logs);
- western timber / sawmill / cabin construction;
- видимые швы между брёвнами;
- stylized low-poly bevel / detail;
- контролируемая вариация дерева.

**Unity-цель по модулю:**

```
1 WallSegment → 1 MeshFilter / mesh → 1 MeshRenderer → 1 shared material → 1 simple collider
```

**КРИТИЧЕСКОЕ ПРАВИЛО ПРОИЗВОДИТЕЛЬНОСТИ:** не создавать «1 бревно = 1 GameObject» при обычной процедурной генерации. Визуальная конструкция может содержать много брёвен, но runtime-представление должно использовать небольшое число переиспользуемых mesh-модулей.

- Создаются несколько переиспользуемых длин/вариантов стен.
- Коридоры переиспользуют то же семейство wall-модулей.
- Генератор **размещает** wall-модули, а не создаёт сотни отдельных log-объектов.

Существующие `Western_Log_Wall_Segment.prefab` / `Western_Plank_Wall_Segment.prefab` (8 и 14 частей соответственно) — PREPARED, не scattered. Новый kit должен быть построен так, чтобы runtime-стоимость соответствовала правилу выше.

## НОВАЯ АРХИТЕКТУРА: модульный western plank floor kit

**Базовый модуль:** `FloorSegment ≈ 4 × 4 m`. Внутри **одного** меша:

- несколько деревянных досок (floorboards);
- видимые швы между досками;
- лёгкая контролируемая вариация ширины;
- сдержанная вариация тона;
- western wooden floor appearance.

**Unity-цель по модулю:**

```
1 FloorSegment → 1 mesh → 1 MeshRenderer → 1 shared material → 1 simple collider
```

**КРИТИЧЕСКОЕ ПРАВИЛО:** не использовать «1 floorboard = 1 GameObject» при обычной процедурной генерации. Полы комнат и полы коридоров переиспользуют одно семейство floor-модулей.

## Performance principle (Android — ограничение с самого начала)

Android performance — **проектное ограничение с самого начала**, а не финальная оптимизация.

**Предпочитать:**

- переиспользуемые меши;
- shared материалы;
- низкое число Renderer-ов;
- низкое число GameObject-ов;
- простые коллайдеры;
- контролируемая плотность треугольников;
- instancing / batching где уместно.

**Избегать:**

- сотен отдельных Renderer-ов для повторяющихся брёвен/досок;
- один материал на элемент;
- один уникальный меш на повторяющийся элемент;
- ненужную микро-геометрию.

**Визуальное богатство должно приходить преимущественно из:**

- модульной конструкции;
- силуэта;
- качества материала;
- контролируемой вариации.

**НЕ из** большого роста количества объектов.

## Wall / floor material direction

Сейчас сгенерированная арена использует один временный `GeneratedArena_URP_Material` с runtime URP Lit properties.

Будущее направление — **намеренно авторские** материалы для модульного кита, а не произвольные перекрасы текущего placeholder-материала.

**Western wall material:** weathered western timber, видимый wood character, тот же визуальный family, что Barrel_01 / Fence_01.

**Western floor material:** weathered western planks / floorboards, читаемое но сдержанное зерно, тот же world family.

**Избегать:** чрезмерный жёлтый; чрезмерный оранжевый; generic плоский коричневый; шумная procedural texturing.

**`ArenaGenerator` пока НЕ переписывать.** Сначала должны быть settled material и modular-kit design.

## Canonical character direction

Сейчас: ОДИН canonical western character/outfit prototype. Порядок: Human Character Dummy → hat → shirt with strong collar + real sleeves → pants → boots → belt → holster → revolver.

Без массового Bandit/Rusher/Shooter/Tactical до утверждения prototype. Никакого skinning/export/Unity integration до визуального approval.

Основа (конфликт зафиксирован, не решён догадкой):

- Human Character Dummy (Unity Asset Store 178395) = техническая Character Foundation проекта.
- `wwg-character-art/SKILL.md` = текущий character-art standard, содержит Kevin Iglesias reference.
- Связь/идентичность этих двух source assets требует отдельной проверки. Новую character model из-за неоднозначности не создавать автоматически.

## Environment asset rules

Факт подготовки (DONE — not confirmed, НЕ завершение Stage; подготовка будущего Stage 5): Fence prefab подготовлен; DestructibleObject foundation подготовлен; Fences pool подготовлен; несколько Blender environment FBX конвертированы в Unity prefabs; Props / Lanterns / Wagons pools созданы/подготовлены; shared URP Lit materials (4 шт.); стены PREPARED, не DONE; realtime lights = 0.

Нормы: импортированные FBX Z-up → коррекция −90° X на Model; minY = 0.00; коллайдеры и Rigidbody минимальные и контролируемые (Android: 1 Box на проп, wagon 2; Rigidbody только точечно).

## Destructible asset rules (связь с будущим Stage 8, Stage 8 НЕ выполнен)

Концепция: разрушение контролируемое и частичное; не делать множество статичных `whole/broken1/broken2` без необходимости; желателен один reusable breakable object foundation; отдельные части могут отделяться; после повреждения возможно включение физики/отдельных colliders; поле боя должно реально изменяться; учитывать производительность Android.

## Known garment lessons (кратко)

- Seams to Plush полезен для готовых выкроек/швов/cloth fitting, но не автогенератор хорошей одежды с нуля.
- Remesh на фрагментированных открытых оболочках показал ограничения.
- Cloth на плохо подготовленной одежде даёт деформацию/плавание.
- Старые vest experiments — experiment/lessons learned, не canonical final outfit.
- Numeric fitting уже использовался успешно. Real Blender renders + Vision analysis уже успешно применялись для QA.

## Visual direction

Low-poly / stylized western; единый визуальный язык (см. SKILL); без чрезмерного жёлтого/коричневого фильтра; внешняя земля — не плоская коричневая площадка; environment остаётся процедурным; городские/строительные assets вне текущего scope, если отдельно не подтверждены.