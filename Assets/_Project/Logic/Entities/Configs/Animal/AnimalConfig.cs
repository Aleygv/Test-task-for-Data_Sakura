using _Project.Logic.Entities.Configs.Movement;

namespace _Project.Logic.Entities.Configs.Animal
{
    public class AnimalConfig
    {
        public AnimalTypeId AnimalTypeId { get; }
        public AnimalRole Role { get; }
        public string PrefabPath { get; }

        public MovementConfig MovementConfig { get; }

        public float NextPositionRadius { get; }

        public AnimalConfig(AnimalTypeId animalTypeId, AnimalRole role, string prefabPath,
            MovementConfig movementConfig,
            float nextPositionRadius)
        {
            AnimalTypeId = animalTypeId;
            PrefabPath = prefabPath;
            MovementConfig = movementConfig;
            Role = role;
            NextPositionRadius = nextPositionRadius;
        }
    }
}