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

        [Inject]
        public AnimalFabric(IAssets assets, IAnimalCollisionResolver collisionResolver)
        {
            _assets = assets;
            _collisionResolver = collisionResolver;
        }

        public async UniTask<GameObject> SpawnFrog()
        {
            GameObject frogPrefab = await _assets.Instantiate(FrogPath);
            IAnimal frog = new Frog(Guid.NewGuid(), SpawnPositions.GetRandomPosition());
            frogPrefab.GetComponent<AnimalReference>().Initialize(frog);
            frogPrefab.GetComponent<AnimalReference>().OnAnimalCollided += _collisionResolver.Resolve;
            frogPrefab.transform.position = frog.Position.AsUnityVector();
            return frogPrefab;
        }

        public async UniTask<GameObject> SpawnSnake()
        {
            GameObject snakePrefab = await _assets.Instantiate(SnakePath);
            IAnimal snake = new Snake(Guid.NewGuid(), SpawnPositions.GetRandomPosition());
            snakePrefab.GetComponent<AnimalReference>().Initialize(snake);
            snakePrefab.GetComponent<AnimalReference>().OnAnimalCollided += _collisionResolver.Resolve;
            snakePrefab.transform.position = snake.Position.AsUnityVector();
            return snakePrefab;
        }
    }
}