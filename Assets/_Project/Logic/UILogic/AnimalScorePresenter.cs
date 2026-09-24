using System;
using System.Diagnostics;
using _Project.Logic.Entities;
using _Project.Logic.Services;
using Zenject;

namespace _Project.Logic.UILogic
{
    public class AnimalScorePresenter : IAnimalScorePresenter, IDisposable
    {
        private readonly IAnimalRegistry _registry;
        private AnimalViewCounter _viewCounter;

        [Inject]
        public AnimalScorePresenter(IAnimalRegistry registry)
        {
            _registry = registry;
        }

        public void Initialize(AnimalViewCounter viewCounter)
        {
            _viewCounter = viewCounter;
            
            _registry.OnAnimalDied += OnDied;
        }

        public void Dispose()
        {
            _registry.OnAnimalDied -= OnDied;
        }

        private void OnDied(AnimalRole role, int amount)
        {
            if (role == AnimalRole.Prey)
            {
                _viewCounter.UpdatePreyCounter(amount);
            }

            if (role == AnimalRole.Predator)
            {
                _viewCounter.UpdatePredatorCounter(amount);
            }
        }
    }
}