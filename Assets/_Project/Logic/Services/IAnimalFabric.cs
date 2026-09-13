using _Project.Logic.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Logic.Services
{
    public interface IAnimalFabric
    {
        UniTask<GameObject> SpawnByConfig(AnimalConfig config, Vector3 atPosition);
        UniTask<GameObject> SpawnRandomAnimal(Vector3 atPosition);
        UniTask<GameObject> SpawnRandomByRole(AnimalRole role, Vector3 atPosition);
    }
}