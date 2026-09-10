using System;
using _Project.Logic.Entities.AI_Movement;
using _Project.Logic.Extensions;
using _Project.Logic.UI_Logic;
using UnityEngine;

namespace _Project.Logic.Entities
{
    public class AnimalReference : MonoBehaviour
    {
        public event Action<IAnimal, IAnimal> OnAnimalCollided;

        public IAnimal Animal { get; private set; }

        private AnimalMovementBase _meshMovementBase;
        private TastyLabelView _labelView;

        public void Initialize(IAnimal animal)
        {
            Animal = animal;
            Animal.OnDie += OnAnimalDied;
            Animal.OnBounce += HandleBounce;
            Animal.OnAte += HandleAte;
            
            _meshMovementBase = GetComponent<AnimalMovementBase>();
            _labelView = GetComponent<TastyLabelView>();
        }

        private void HandleAte()
        {
            _labelView?.ShowTasty();
        }

        private void HandleBounce(LightVector3 position)
        {
            _meshMovementBase.BounceFrom(position.AsUnityVector());
        }

        private void OnAnimalDied()
        {
            if (Animal != null)
            {
                Animal.OnDie -= OnAnimalDied;
                Animal.OnBounce -= HandleBounce;
                Animal.OnAte -= HandleAte;
                
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