using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Logic.Entities;

namespace _Project.Logic.Services
{
    public class AnimalRegistry : IAnimalRegistry
    {
        public event Action<AnimalRole, int> OnAnimalDied;

        private Dictionary<AnimalRole, int> _deathCounters = new();

        public void Register(IAnimal animal)
        {
            void OnDieHandler()
            {
                animal.OnDie -= OnDieHandler;
                RecordDeath(animal.Role);
            }

            animal.OnDie += OnDieHandler;
        }

        private void RecordDeath(AnimalRole role)
        {
            _deathCounters.TryAdd(role, 0);

            _deathCounters[role]++;
            
            OnAnimalDied?.Invoke(role, _deathCounters[role]);
        }
    }
}