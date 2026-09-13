using _Project.Logic.Entities;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Infrastructure
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SpawnPositionProvider>().FromComponentInHierarchy().AsSingle();
        } 
    }
}
