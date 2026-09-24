using _Project.Logic.Infrastructure;
using _Project.Logic.Services;
using _Project.Logic.UILogic;
using UnityEngine;
using Zenject;

namespace _Project.Resources
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private AnimalViewCounter viewCounter;
        
        public override void InstallBindings()
        {
            BindPrefabs();
            BindServices();
        }

        private void BindPrefabs()
        {
            Container.BindInterfacesAndSelfTo<Bootstrapper>().AsSingle();
            Container.BindFactory<AnimalViewCounter, AnimalViewCounter.Fabric>().FromComponentInNewPrefab(viewCounter);
        }

        private void BindServices()
        {
            Container.Bind<IAnimalCollisionResolver>().To<AnimalCollisionResolver>().AsSingle();
            Container.Bind<IAssets>().To<IAssets.AssetsProvider>().AsSingle();
            Container.Bind<IAnimalFabric>().To<AnimalFabric>().AsSingle();
            Container.Bind<IAnimalRegistry>().To<AnimalRegistry>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
            Container.Bind<IAnimalScorePresenter>().To<AnimalScorePresenter>().AsSingle();
        }
    }
}