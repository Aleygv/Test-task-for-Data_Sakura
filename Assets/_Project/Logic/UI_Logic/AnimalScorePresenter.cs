using System;
using _Project.Logic.Entities;
using _Project.Logic.Services;
using Zenject;

namespace _Project.Logic.UI_Logic
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
            
            _registry.OnAnimalCountChanged += OnCountChanged;
            
            _viewCounter.UpdatePreyCounter(_registry.GetCount(AnimalRole.Prey));
            _viewCounter.UpdatePredatorCounter(_registry.GetCount(AnimalRole.Predator));
        }

        private void OnCountChanged(AnimalRole role, int amount)
        {
            if (role == AnimalRole.Prey) _viewCounter.UpdatePreyCounter(amount);
            if (role == AnimalRole.Predator) _viewCounter.UpdatePredatorCounter(amount);
        }

        public void Dispose()
        {
            _registry.OnAnimalCountChanged -= OnCountChanged;
        }
    }
}