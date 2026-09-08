using System;
using System.Threading;
using _Project.Logic.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Logic.Infrastructure
{
    public class GameCycleScript : MonoBehaviour
    {
        [SerializeField] private float spawnInterval = 2f;
        private IAnimalFabric _animalFabric;
        
        private float _timeCounter;

        [Inject]
        public void Construct(IAnimalFabric animalFabric)
        {
            _animalFabric = animalFabric;
        }

        private void Start()
        {
            StartSpawnLoopAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid StartSpawnLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                float delay = Random.Range(1f, spawnInterval);

                await UniTask.Delay(TimeSpan.FromSeconds(delay), DelayType.DeltaTime, PlayerLoopTiming.Update, ct);

                await _animalFabric.SpawnRandomAnimal();
            }
        }

    }
}