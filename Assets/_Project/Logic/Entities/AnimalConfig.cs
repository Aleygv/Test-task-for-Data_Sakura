using System;
using _Project.Logic.Extensions;

namespace _Project.Logic.Entities
{
    public class AnimalConfig
    {
        public string PrefabPath { get; }
        public AnimalRole Role { get; }
        public Func<Guid, LightVector3, IAnimal> Factory { get; }

        public AnimalConfig(string prefabPath, AnimalRole role, Func<Guid, LightVector3, IAnimal> factory)
        {
            PrefabPath = prefabPath;
            Role = role;
            Factory = factory;
        }
    }
}