using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Logic.Services
{
    public interface IAnimalFabric
    {
        UniTask<GameObject> SpawnFrog();
        UniTask<GameObject> SpawnSnake();
    }
}