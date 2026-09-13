using System;
using System.Collections.Generic;
using _Project.Logic.Entities;
using Random = UnityEngine.Random;

namespace _Project.Logic.Services
{
    public class AnimalCollisionResolver : IAnimalCollisionResolver
    {
        private readonly Dictionary<(AnimalRole, AnimalRole), Action<IAnimal, IAnimal>> _interactionMatrix;

        public AnimalCollisionResolver()
        {
            _interactionMatrix = new Dictionary<(AnimalRole, AnimalRole), Action<IAnimal, IAnimal>>()
            {
                { (AnimalRole.Prey, AnimalRole.Prey), HandlePreyWithPray },
                { (AnimalRole.Predator, AnimalRole.Predator), HandlePredatorWithPredator },
                { (AnimalRole.Predator, AnimalRole.Prey), HandlePredatorWithPrey },
            };
        }

        public void HandlePreyWithPray(IAnimal a, IAnimal b)
        {
            a.Bounce();
            b.Bounce();
        }

        public void HandlePredatorWithPredator(IAnimal a, IAnimal b)
        {
            if (Random.value > 0.5f)
            {
                a.Die();
                b.Eat();
            }
            else
            {
                b.Die();
                a.Eat();
            }
        }

        public void HandlePredatorWithPrey(IAnimal a, IAnimal b)
        {
            IAnimal predator = a.Role == AnimalRole.Predator ? a : b;
            IAnimal prey = a.Role == AnimalRole.Prey ? a : b;
            
            prey.Die();
            predator.Eat();
        }

        public void Resolve(IAnimal first, IAnimal second)
        {
            if (first == null || second == null)
                return;

            if (_interactionMatrix.TryGetValue((first.Role, second.Role), out Action<IAnimal, IAnimal> action) ||
                _interactionMatrix.TryGetValue((second.Role, first.Role), out action))
            {
                action(first, second);
            }
        }
    }
}