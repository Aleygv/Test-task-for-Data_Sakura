using System.Collections.Generic;
using _Project.Logic.Entities;

namespace _Project.Logic.Infrastructure
{
    public static class GameConfigs
    {
        public static readonly AnimalConfig Frog = new AnimalConfig(
            AnimalType.Frog,
            AnimalRole.Prey,
            "Prefabs/Animals/Frog.prefab");
        
        public static readonly AnimalConfig Snake = new AnimalConfig(
            AnimalType.Snake,
            AnimalRole.Predator,
            "Prefabs/Animals/Snake.prefab");

        public static readonly List<AnimalConfig> AnimalConfigs = new List<AnimalConfig>()
        {
            Frog,
            Snake
        };
    }
}