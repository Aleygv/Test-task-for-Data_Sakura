using System;
using _Project.Logic.Entities.AI_Movement;
using _Project.Logic.Extensions;
using UnityEngine;

namespace _Project.Logic.Entities
{
    public class AnimalReference : MonoBehaviour
    {
        public event Action<IAnimal, IAnimal> OnAnimalCollided;

        public IAnimal Animal { get; private set; }

        private AnimalNavMeshMovement _meshMovement;

        public void Initialize(IAnimal animal)
        {
            Animal = animal;
            Animal.OnDie += OnAnimalDied;
            Animal.OnBounce += HandleBounce;
            
            _meshMovement = GetComponent<AnimalNavMeshMovement>();
        }

        private void HandleBounce(LightVector3 position)
        {
            _meshMovement.BounceFrom(position.AsUnityVector());
        }

        private void OnAnimalDied()
        {
            if (Animal != null)
            {
                Animal.OnDie -= OnAnimalDied;
                Animal.OnBounce -= HandleBounce;
                
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