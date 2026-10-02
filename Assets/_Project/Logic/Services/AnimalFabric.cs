using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Logic.Entities;
using _Project.Logic.Entities.AIMovement;
using _Project.Logic.Entities.Animals;
using _Project.Logic.Entities.Configs.Animal;
using _Project.Logic.Entities.Configs.Movement;
using _Project.Logic.Infrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Logic.Services
{
    public class AnimalFabric : IAnimalFabric
    {
        private IAssets _assets;
        private IAnimalCollisionResolver _collisionResolver;
        private IAnimalRegistry _registry;

        [Inject]
        public AnimalFabric(IAssets assets, IAnimalCollisionResolver collisionResolver, IAnimalRegistry registry)
        {
            _assets = assets;
            _collisionResolver = collisionResolver;
            _registry = registry;
        }

        public async UniTask<GameObject> SpawnByConfig(AnimalConfig config, Vector3 atPosition)
        {
            GameObject animalObject = await _assets.Instantiate(config.PrefabPath);
            animalObject.transform.position = atPosition;

            IMovement movement = CreateMovement(animalObject, config.MovementConfig);

            IAnimal animal;
            Guid id = Guid.NewGuid();

            switch (config.AnimalTypeId)
            {
                case AnimalTypeId.Frog:
                    animal = new Frog(id, config.Role, movement, config.NextPositionRadius);
                    break;

                case AnimalTypeId.Snake:
                    animal = new Snake(id, config.Role, movement, config.NextPositionRadius);
                    break;

                default:
                    throw new ArgumentException($"Unknown type of animal");
            }
            
            _registry.Register(animal);

            AnimalReference animalReference = animalObject.GetComponent<AnimalReference>();
            animalReference.Initialize(animal);
            animalReference.OnAnimalCollided += _collisionResolver.Resolve;

            return animalObject;
        }

        private IMovement CreateMovement(GameObject animalObject, MovementConfig movementConfig)
        {
            // foreach (MovementBase existing in animalObject.GetComponents<MovementBase>())
            // {
            //     UnityEngine.Object.DestroyImmediate(existing);
            // }

            MovementBase movement = movementConfig switch
            {
                JumpMovementConfig => animalObject.AddComponent<JumpMovement>(),
                LinearMovementConfig => animalObject.AddComponent<LinearMovement>(),
                _ => throw new ArgumentException($"Unknown movement config type: {movementConfig?.GetType().Name}")
            };

            movement.Initialize(movementConfig);
            return movement;
        }

        public UniTask<GameObject> SpawnRandomAnimal(Vector3 atPosition)
        {
            List<AnimalConfig> animalConfigs = GameConfigs.AnimalConfigs;
            int range = Random.Range(0, animalConfigs.Count);
            return SpawnByConfig(animalConfigs[range], atPosition);
        }

        public UniTask<GameObject> SpawnRandomByRole(AnimalRole role, Vector3 atPosition)
        {
            List<AnimalConfig> animalConfigs = GameConfigs.AnimalConfigs;
            List<AnimalConfig> roleConfigs = animalConfigs.Where(c => c.Role == role).ToList();
            var randomConfig = roleConfigs[Random.Range(0, roleConfigs.Count)];
            return SpawnByConfig(randomConfig, atPosition);
        }
    }
}