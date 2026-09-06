using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Logic.Services
{
    public interface IAssets
    {
        UniTask<GameObject> Instantiate(string path);
        UniTask<GameObject> Instantiate(string path, Vector3 at);

        public class AssetsProvider : IAssets
        {
            public async UniTask<GameObject> Instantiate(string path)
            {
                return await Addressables.InstantiateAsync(path).ToUniTask();
            }

            public async UniTask<GameObject> Instantiate(string path, Vector3 at)
            {
                return await Addressables.InstantiateAsync(path, at, Quaternion.identity).ToUniTask();
            }
        }
    }
}