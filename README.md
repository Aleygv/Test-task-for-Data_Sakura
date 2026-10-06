# Zoo World (Data Sakura — Test Task)

3D-симулятор жизненного цикла и пищевой цепочки животных на Unity с видом сверху (Top-Down).

---

## Требования технического задания (ТЗ)

1. **Геймплей:**
   - Каждые 1–2 секунды на арене случайным образом спавнится животное.
   - Животные перемещаются по арене и остаются в границах видимости экрана (при выходе за пределы — меняют направление движения обратно в экран).
   - Физические коллизии между объектами.
2. **Пищевая цепочка:**
   - **Жертвы (Prey, например Лягушка / Frog):** совершают прыжки раз в `X` секунд на фиксированную дистанцию. При столкновении двух жертв — они отскакивают друг от друга по законам физики. При встрече с хищником — жертва погибает и исчезает с арены.
   - **Хищники (Predator, например Змея / Snake):** непрерывно перемещаются с постоянной скоростью. При столкновении с жертвой — съедают её (над хищником появляется плашка «Tasty!»). При столкновении двух хищников — один случайным образом (50/50) выживает, а второй погибает.
3. **Интерфейс (UI):**
   - В правом верхнем углу расположен счётчик погибших жертв и хищников, выполненный на uGUI.
4. **Главный фокус ТЗ:**
   - Архитектура животных должна быть прозрачной, легко расширяемой и поддерживаемой (с расчётом на возможное добавление сотен и тысяч новых типов животных — птицы, пауки, рыбы, крабы и т.д.).
   - Без использования ECS.

---

### Демонстрация геймплея
https://github.com/user-attachments/assets/53c1de6d-25ba-491a-9a3b-c055c2a3ea36


## Стек технологий

- Unity 6 (6000.3+)
- Zenject
- UniTask
- Addressables
- Unity NavMesh
- TextMeshPro + uGUI

---

## Архитектура

### Расширяемость системы животных

Основное требование ТЗ — возможность легко добавлять новые типы животных. Это достигается следующим образом:

1. **Модель отделена от представления.** Классы животных (`AnimalBase`, `Frog`, `Snake`) — чистый C#, реализуют интерфейс `IAnimal`. Они не зависят от `MonoBehaviour` и содержат только логику поведения. Компонент `AnimalReference` связывает Unity GameObject с C#-моделью.

2. **Движение вынесено за интерфейс `IMovement`.** Конкретные реализации (`JumpMovement`, `LinearMovement`) — это MonoBehaviour-компоненты, которые создаются фабрикой через `AddComponent` и настраиваются через `Initialize(MovementConfig)`. Любое животное может получить любой тип движения без изменения кода — достаточно поменять конфиг.

3. **Конфигурация через данные.** Параметры каждого животного (тип, роль, путь к префабу, числовые настройки поведения, радиус достижения цели) вынесены в конфиг-классы (`AnimalConfig`, `JumpMovementConfig`, `LinearMovementConfig`). Все конфиги централизованы в `GameConfigs`. Фабрика `AnimalFabric` создаёт животных по конфигу.

4. **Матрица взаимодействий.** Правила столкновений между ролями (`Prey` vs `Prey`, `Predator` vs `Prey`, `Predator` vs `Predator`) вынесены в `AnimalCollisionResolver` в виде словаря делегатов, без цепочек `if-else`. При добавлении новой роли достаточно дописать пару записей в матрицу.

5. **Разделение ответственности.** Животное (`AnimalBase`) отвечает за хранение текущей цели и проверку её достижения (`IsTargetReached`). Компонент движения (`IMovement`) отвечает за расчёт следующей цели (`GetNextTarget`) и выполнение перемещения (`SetPosition`). `SetPosition` возвращает `bool` — успешно ли началось движение, что предотвращает потерю цели при отклонённом запросе.

**Чтобы добавить новое животное**, нужно: создать класс, унаследованный от `AnimalBase`; добавить конфиг в `GameConfigs`; добавить case в фабрику; создать префаб. Тип движения настраивается через конфиг — без изменения кода.

---

## Структура проекта

```text
Assets/
├── AddressableResources/      # Префабы животных и UI
└── _Project/
    └── Logic/
        ├── Entities/          # Модели и сущности
        │   ├── AIMovement/    # Компоненты движения (MovementBase, JumpMovement, LinearMovement)
        │   ├── Animals/       # C# классы животных (AnimalBase, Frog, Snake)
        │   ├── Configs/       # Конфиги (AnimalConfig, MovementConfig, JumpMovementConfig, LinearMovementConfig)
        │   ├── AnimalReference.cs
        │   ├── AnimalRole.cs
        │   ├── AnimalTypeId.cs
        │   ├── IAnimal.cs
        │   ├── IMovement.cs
        │   └── SpawnPositionProvider.cs
        ├── Infrastructure/    # Точки входа, циклы игры и инсталлеры Zenject
        │   ├── Bootstrapper.cs
        │   ├── BootstrapInstaller.cs
        │   ├── GameConfigs.cs
        │   ├── GameCycleScript.cs
        │   └── SceneInstaller.cs
        ├── Services/          # Сервисы (фабрика, коллизии, реестр, ассеты)
        │   ├── AnimalCollisionResolver.cs
        │   ├── AnimalFabric.cs
        │   ├── AnimalRegistry.cs
        │   └── IAssets.cs
        ├── UILogic/           # UI (Presenter, View, Factory)
        │   ├── AnimalScorePresenter.cs
        │   ├── AnimalViewCounter.cs
        │   ├── TastyLabelView.cs
        │   └── UIFactory.cs
        └── Extensions/        # Методы расширения (CameraExtensions)
```

---

## Ключевые компоненты

### Животные (`AnimalBase`, `Frog`, `Snake`)

Чистый C#, не наследуют `MonoBehaviour`, не зависят от Unity API (кроме `Vector3` через `IMovement.transform`). Хранят текущую цель (`_currentTarget` + флаг `_hasTarget`), проверяют её достижение (`IsTargetReached` сравнивает дистанцию с `NextPositionRadius` из конфига), запрашивают новую цель (`_movement.GetNextTarget()` + `_movement.SetPosition(nextTarget)`). Цель обновляется только если `SetPosition` вернул `true`.

### Компоненты движения (`IMovement`, `MovementBase`, `JumpMovement`, `LinearMovement`)

Создаются фабрикой (`AnimalFabric.CreateMovement()` делает `AddComponent<JumpMovement/LinearMovement>()` и вызывает `Initialize(config)`). Настраиваются через конфиг — все параметры (дистанция, интервал, высота, скорость) передаются через `Initialize(MovementConfig)`, никаких `[SerializeField]`. Рассчитывают следующую цель (`GetNextTarget()` содержит логику выбора направления и дистанции), выполняют перемещение (`SetPosition(target)` возвращает `bool`). Для `JumpMovement` — прыжок по параболе через UniTask. Для `LinearMovement` — `NavMeshAgent.SetDestination`.

### Фабрика (`AnimalFabric`)

Создаёт животных по конфигу (`SpawnByConfig(AnimalConfig, Vector3)`), создаёт компонент движения (`CreateMovement()` со `switch` по типу `MovementConfig`: `JumpMovementConfig → AddComponent<JumpMovement>()`, `LinearMovementConfig → AddComponent<LinearMovement>()`), инициализирует компонент (`movement.Initialize(config)`), создаёт C#-модель животного (`new Frog(id, role, movement, nextPositionRadius)` / `new Snake(...)`), регистрирует в реестре (`_registry.Register(animal)`), связывает с представлением (`AnimalReference.Initialize(animal)`).

### Конфигурация (`GameConfigs`)

Централизованные конфиги — все параметры животных и движения заданы в одном месте. Легко настраивать — изменить поведение можно без перекомпиляции, просто поменяв значения в `GameConfigs`. Примеры: `JumpMovementConfig` (`StepDistance`, `JumpInterval`, `JumpHeight`, `JumpDuration`, `MaxTurnAngle`), `LinearMovementConfig` (`MinTargetDistance`, `MaxTargetDistance`, `CenterReturnRadius`, `MaxArenaRadiusSqr`), `AnimalConfig` (`AnimalTypeId`, `Role`, `PrefabPath`, `MovementConfig`, `NextPositionRadius`).

### Коллизии (`AnimalCollisionResolver`)

Матрица взаимодействий — словарь `Dictionary<(AnimalRole, AnimalRole), Action<IAnimal, IAnimal>>`. Правила: `Prey vs Prey` — оба отскакивают (`Bounce()`), `Predator vs Prey` — хищник съедает жертву (`Die()` + `Eat()`), `Predator vs Predator` — случайный выживает (50/50), второй погибает. Расширение — для новой роли достаточно добавить записи в матрицу.

### UI (`AnimalScorePresenter`, `AnimalViewCounter`, `TastyLabelView`)

Счётчик погибших (`AnimalViewCounter` отображает количество погибших `Prey` и `Predator`), плашка «Tasty!» (`TastyLabelView` показывает над хищником после съедания жертвы), связь через события (`AnimalRegistry.OnAnimalDied` → `AnimalScorePresenter.OnDied` → `AnimalViewCounter.UpdatePreyCounter/UpdatePredatorCounter`).

### Zenject (`BootstrapInstaller`, `SceneInstaller`)

DI-контейнер — все сервисы регистрируются в `BootstrapInstaller`. Точки входа — `Bootstrapper` инициализирует UI, `GameCycleScript` запускает цикл спавна. `SceneInstaller` регистрирует `SpawnPositionProvider` из сцены.

---

## Как добавить новое животное

1. Создайте класс в `Assets/_Project/Logic/Entities/Animals/`, унаследованный от `AnimalBase`:

```csharp
public class Bird : AnimalBase
{
    private Vector3 _currentTarget;
    private bool _hasTarget;

    public Bird(Guid id, AnimalRole role, IMovement movement, float nextPositionRadius)
        : base(id, role, movement, nextPositionRadius)
    {
        _currentTarget = _movement.transform.position;
        _hasTarget = false;
    }

    public override void Tick(float deltaTime)
    {
        if (!_hasTarget || IsTargetReached(_currentTarget))
        {
            Vector3 nextTarget = _movement.GetNextTarget();
            if (_movement.SetPosition(nextTarget))
            {
                _currentTarget = nextTarget;
                _hasTarget = true;
            }
        }
    }
}
```

2. Добавьте конфиг в `GameConfigs`:

```csharp
public static readonly AnimalConfig Bird = new(
    AnimalTypeId.Bird,
    AnimalRole.Prey,
    "Prefabs/Animals/Bird.prefab",
    JumpMovementConfig,
    0.5f);
```

3. Добавьте case в `AnimalFabric.SpawnByConfig`:

```csharp
case AnimalTypeId.Bird:
    animal = new Bird(id, config.Role, movement, config.NextPositionRadius);
    break;
```

4. Создайте префаб в `AddressableResources/Prefabs/Animals/` с компонентом `AnimalReference` и `TastyLabelView`.

5. Готово — животное появится в игре. Тип движения можно менять через конфиг без изменения кода.

---

## Как добавить новый тип движения

1. Создайте конфиг в `Assets/_Project/Logic/Entities/Configs/Movement/`:

```csharp
public class FlyMovementConfig : MovementConfig
{
    public float FlyHeight { get; }
    public float FlySpeed { get; }
}
```

2. Создайте компонент в `Assets/_Project/Logic/Entities/AIMovement/`:

```csharp
public class FlyMovement : MovementBase
{
    private FlyMovementConfig _config;

    public override void Initialize(MovementConfig config)
    {
        base.Initialize(config);
        _config = config as FlyMovementConfig;
    }

    public override Vector3 GetNextTarget() { /* ... */ }

    public override bool SetPosition(Vector3 targetPosition) { /* ... */ }
}
```

3. Добавьте case в `AnimalFabric.CreateMovement`:

```csharp
FlyMovementConfig => animalObject.AddComponent<FlyMovement>(),
```

4. Используйте в конфиге животного:

```csharp
public static readonly AnimalConfig Bird = new(
    AnimalTypeId.Bird,
    AnimalRole.Prey,
    "Prefabs/Animals/Bird.prefab",
    FlyMovementConfig,
    0.5f);
```

---

## Примечания

- Без ECS — архитектура построена на классических C#-классах и интерфейсах.
- Без `[SerializeField]` — все параметры настраиваются через конфиги в `GameConfigs`.
- UniTask — асинхронные операции (прыжки, отскоки, спавн) выполняются через UniTask с корректной отменой через `CancellationToken`.
- NavMesh — перемещение использует Unity NavMesh для поиска допустимых точек.
