using _Project.Logic.UI_Logic;
using Zenject;

namespace _Project.Logic.Infrastructure
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Container.Bind<AnimalViewCounter>().FromComponentInHierarchy().AsSingle();
            // Container.BindInterfacesAndSelfTo<AnimalScorePresenter>().AsSingle().NonLazy();
        } 
    }
}
