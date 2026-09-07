using System;
using System.Threading;
using _Project.Logic.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Infrastructure
{
    public class GameCycleScript : MonoBehaviour
    {
        [SerializeField] private float spawnInterval = 2f;
        private IAnimalFabric _animalFabric;

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
                await UniTask.Delay(TimeSpan.FromSeconds(spawnInterval), DelayType.DeltaTime,
                    PlayerLoopTiming.Update, ct);
                
                await UniTask.WhenAll(
                    _animalFabric.SpawnFrog(),
                    _animalFabric.SpawnSnake());
            }
        }
    }
}