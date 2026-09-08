using _Project.Logic.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Logic.Services
{
    public interface IAnimalFabric
    {
        UniTask<GameObject> SpawnByConfig(AnimalConfig config);
        UniTask<GameObject> SpawnRandomAnimal();
        UniTask<GameObject> SpawnRandomByRole(AnimalRole role);
    }
}