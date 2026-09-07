using _Project.Logic.Infrastructure;
using _Project.Logic.Services;
using _Project.Logic.UI_Logic;
using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private AnimalViewCounter viewCounter;

    // Здесь сервисы на протяжении всей игры
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