using System;
using _Project.Logic.Entities;
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

        [Inject]
        public AnimalFabric(IAssets assets, IAnimalCollisionResolver collisionResolver, IAnimalRegistry registry)
        {
            _assets = assets;
            _collisionResolver = collisionResolver;
            _registry = registry;
        }

        public async UniTask<GameObject> SpawnFrog()
        {
            GameObject frogPrefab = await _assets.Instantiate(FrogPath);
            IAnimal frog = new Frog(Guid.NewGuid(), SpawnPositions.GetRandomPosition());
            
            _registry.Register(frog);
            
            frogPrefab.GetComponent<AnimalReference>().Initialize(frog);
            frogPrefab.GetComponent<AnimalReference>().OnAnimalCollided += _collisionResolver.Resolve;
            frogPrefab.transform.position = frog.Position.AsUnityVector();
            return frogPrefab;
        }

        public async UniTask<GameObject> SpawnSnake()
        {
            GameObject snakePrefab = await _assets.Instantiate(SnakePath);
            IAnimal snake = new Snake(Guid.NewGuid(), SpawnPositions.GetRandomPosition());
            
            _registry.Register(snake);
            
            snakePrefab.GetComponent<AnimalReference>().Initialize(snake);
            snakePrefab.GetComponent<AnimalReference>().OnAnimalCollided += _collisionResolver.Resolve;
            snakePrefab.transform.position = snake.Position.AsUnityVector();
            return snakePrefab;
        }
    }
}