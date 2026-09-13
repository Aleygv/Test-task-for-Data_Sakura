using System;
using System.Threading;
using _Project.Logic.Entities;
using _Project.Logic.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Logic.Infrastructure
{
    public class GameCycleScript : MonoBehaviour
    {
        [SerializeField] private float minSpawnInterval = 1f;
        [SerializeField] private float maxSpawnInterval = 2f;
        
        private IAnimalFabric _animalFabric;
        private SpawnPositionProvider _spawnPositionProvider;

        [Inject]
        public void Construct(IAnimalFabric animalFabric, SpawnPositionProvider spawnPositionProvider)
        {
            _animalFabric = animalFabric;
            _spawnPositionProvider = spawnPositionProvider;
        }

        private void Start()
        {
            StartSpawnLoopAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid StartSpawnLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                float delay = Random.Range(minSpawnInterval, maxSpawnInterval);

                await UniTask.Delay(TimeSpan.FromSeconds(delay), DelayType.DeltaTime, PlayerLoopTiming.Update, ct);

                await _animalFabric.SpawnRandomAnimal(_spawnPositionProvider.GetSpawnPosition());
            }
        }

    }
}