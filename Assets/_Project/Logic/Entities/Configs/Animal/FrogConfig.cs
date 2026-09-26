using _Project.Logic.Entities.Configs.Movement;

namespace _Project.Logic.Entities.Configs.Animal
{
    public class FrogConfig : AnimalConfig
    {
        public JumpMovementConfig JumpMovementConfig { get; }

        public FrogConfig(AnimalTypeId animalTypeId, AnimalRole role, string prefabPath,
            JumpMovementConfig jumpMovementConfig)
            : base(animalTypeId, role, prefabPath)
        {
            JumpMovementConfig = jumpMovementConfig;
        }
    }
}