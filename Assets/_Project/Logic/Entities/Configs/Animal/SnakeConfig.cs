using _Project.Logic.Entities.Configs.Movement;

namespace _Project.Logic.Entities.Configs.Animal
{
    public class SnakeConfig : AnimalConfig
    {
        public LinearMovementConfig LinearMovementConfig { get; }

        public SnakeConfig(AnimalTypeId animalTypeId, AnimalRole role, string prefabPath,
            LinearMovementConfig linearMovementConfig)
            : base(animalTypeId, role, prefabPath)
        {
            LinearMovementConfig = linearMovementConfig;
        }
    }
}