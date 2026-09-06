using System;
using UnityEngine;

namespace _Project.Logic.Entities
{
    public class AnimalReference : MonoBehaviour
    {
        public event Action<IAnimal, IAnimal> OnAnimalCollided;

        public IAnimal Animal { get; private set; }

        public void Initialize(IAnimal animal)
        {
            Animal = animal;
            Animal.OnDie += OnAnimalDied;
        }

        private void OnAnimalDied()
        {
            if (Animal != null)
            {
                Animal.OnDie -= OnAnimalDied;
                Animal = null;
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<AnimalReference>(out var animalReference))
            {
                OnAnimalCollided?.Invoke(Animal, animalReference.Animal);
            }
        }
    }
}