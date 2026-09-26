namespace _Project.Logic.Entities.Configs.Animal
{
    public class AnimalConfig
    {
        public AnimalTypeId AnimalTypeId { get; }
        public AnimalRole Role { get; }
        public string PrefabPath { get; }

        public AnimalConfig(AnimalTypeId animalTypeId, AnimalRole role, string prefabPath)
        {
            AnimalTypeId = animalTypeId;
            PrefabPath = prefabPath;
            Role = role;
        }
    }
}