namespace _Project.Logic.Entities
{
    public class AnimalConfig
    {
        public AnimalType AnimalType { get; }
        public AnimalRole Role { get; }
        public string PrefabPath { get; }

        public AnimalConfig(AnimalType animalType, AnimalRole role, string prefabPath)
        {
            AnimalType = animalType;
            PrefabPath = prefabPath;
            Role = role;
        }
    }
}