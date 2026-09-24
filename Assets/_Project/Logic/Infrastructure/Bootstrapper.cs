using System;
using _Project.Logic.UILogic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Infrastructure
{
    public class Bootstrapper : IInitializable
    {
        private readonly IAnimalScorePresenter _scorePresenter;
        private readonly IUIFactory _uiFactory;

        [Inject]
        public Bootstrapper(IAnimalScorePresenter scorePresenter, IUIFactory uiFactory)
        {
            _scorePresenter = scorePresenter;
            _uiFactory = uiFactory;
        }

        public void Initialize()
        {
            InitializeAsync().Forget();
        }

        private async UniTaskVoid InitializeAsync()
        {
            try
            {
                AnimalViewCounter viewCounter = await _uiFactory.CreateAnimalCounterWindow();
                _scorePresenter.Initialize(viewCounter);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Async error: {exception.Message}");
            }
        }
    }
}