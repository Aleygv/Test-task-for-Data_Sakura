using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Logic.Entities;
using _Project.Logic.Entities.Animals;
using _Project.Logic.Entities.Animals.Predators;
using _Project.Logic.Entities.Animals.Preyes;
using _Project.Logic.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Logic.Services
{
    public class AnimalFabric : IAnimalFabric
    {
        private const string FrogPath = "Prefabs/Animals/Frog.prefab";
        private const string SnakePath = "Prefabs/Animals/Snake.prefab";

        private IAssets _assets;
        private IAnimalCollisionResolver _collisionResolver;
        private IAnimalRegistry _registry;

        private readonly List<AnimalConfig> _animalConfigs;

        [Inject]
        public AnimalFabric(IAssets assets, IAnimalCollisionResolver collisionResolver, IAnimalRegistry registry)
        {
            _assets = assets;
            _collisionResolver = collisionResolver;
            _registry = registry;

            _animalConfigs = new List<AnimalConfig>
            {
                new(FrogPath, AnimalRole.Prey, (id, pos) => new Frog(id, pos)),
                new(SnakePath, AnimalRole.Predator, (id, pos) => new Snake(id, pos))
            };
        }

        public async UniTask<GameObject> SpawnByConfig(AnimalConfig config)
        {
            GameObject prefab = await _assets.Instantiate(config.PrefabPath);
            IAnimal animal = config.Factory.Invoke(Guid.NewGuid(), SpawnPositions.GetRandomPosition());

            _registry.Register(animal);

            AnimalReference animalReference = prefab.GetComponent<AnimalReference>();
            animalReference.Initialize(animal);
            animalReference.OnAnimalCollided += _collisionResolver.Resolve;
            prefab.transform.position = animal.Position.AsUnityVector();
            return prefab;            
        }

        public UniTask<GameObject> SpawnRandomAnimal()
        {
            int range = Random.Range(0, _animalConfigs.Count);
            return SpawnByConfig(_animalConfigs[range]);
        }

        public UniTask<GameObject> SpawnRandomByRole(AnimalRole role)
        {
            List<AnimalConfig> roleConfigs = _animalConfigs.Where(c => c.Role == role).ToList();
            var randomConfig = roleConfigs[Random.Range(0, roleConfigs.Count)];
            return SpawnByConfig(randomConfig);
        }
    }
}