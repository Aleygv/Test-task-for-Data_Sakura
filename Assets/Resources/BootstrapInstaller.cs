using _Project.Logic.Services;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    // Здесь сервисы на протяжении всей игры
    public override void InstallBindings()
    {
        Container.Bind<IAnimalCollisionResolver>().To<AnimalCollisionResolver>().AsSingle();
        Container.Bind<IAssets>().To<IAssets.AssetsProvider>().AsSingle();
        Container.Bind<IAnimalFabric>().To<AnimalFabric>().AsSingle();
    } 
}
