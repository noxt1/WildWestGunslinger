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
