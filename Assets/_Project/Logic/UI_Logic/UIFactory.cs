using _Project.Logic.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Logic.UI_Logic
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI/UI_Root.prefab";

        private GameObject _uiRoot;
        private IAssets _assets;
        private AnimalViewCounter.Fabric _viewCounterFabric;

        private readonly DiContainer _container;

        [Inject]
        public UIFactory(IAssets assets, DiContainer container, AnimalViewCounter.Fabric viewCounterFabric)
        {
            _assets = assets;
            _container = container;
            _viewCounterFabric = viewCounterFabric;
        }

        public async UniTask<AnimalViewCounter> CreateAnimalCounterWindow()
        {
            await CreateUIRoot();
            AnimalViewCounter viewCounter = _viewCounterFabric.Create();
            _container.InjectGameObject(viewCounter.gameObject);
            viewCounter.gameObject.transform.SetParent(_uiRoot.transform, false);
            return viewCounter;
        }

        private async UniTask CreateUIRoot()
        {
            _uiRoot = await _assets.Instantiate(UIRootPath);
            _container.InjectGameObject(_uiRoot);
        }
    }
}