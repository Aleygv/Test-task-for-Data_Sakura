using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Logic.Entities;

namespace _Project.Logic.Services
{
    public class AnimalRegistry : IAnimalRegistry
    {
        public event Action<AnimalRole, int> OnAnimalCountChanged;

        private readonly List<IAnimal> _aliveAnimals = new();
        
        public int GetCount(AnimalRole role) =>
            _aliveAnimals.Count(x => x.Role == role);
        

        public void Register(IAnimal animal)
        {
            _aliveAnimals.Add(animal);

            animal.OnDie += () => Unregister(animal);
            
            NotifyCountChange(animal.Role);
        }

        private void Unregister(IAnimal animal)
        {
            if (_aliveAnimals.Remove(animal))
            {
                NotifyCountChange(animal.Role);
            }
        }

        private void NotifyCountChange(AnimalRole animalRole)
        {
            OnAnimalCountChanged?.Invoke(animalRole, GetCount(animalRole));
        }
    }
}