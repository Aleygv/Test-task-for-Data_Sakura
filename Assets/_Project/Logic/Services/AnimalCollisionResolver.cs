using System;
using System.Collections.Generic;
using _Project.Logic.Entities;
using _Project.Logic.Entities.Animals.Preyes;
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
                { (AnimalRole.Predator, AnimalRole.Prey), HandlePredatorWithPray },
                { (AnimalRole.Prey, AnimalRole.Predator), (pray, pred)
                    => HandlePredatorWithPray(pred, pray) }
            };
        }

        public void HandlePreyWithPray(IAnimal a, IAnimal b)
        {
            a.Bounce(b.Position);
            b.Bounce(a.Position);
        }

        public void HandlePredatorWithPredator(IAnimal a, IAnimal b)
        {
            if (Random.value > 0.5f)
            {
                a.Die();
            }
            else
            {
                b.Die();
            }
        }

        public void HandlePredatorWithPray(IAnimal predator, IAnimal prey)
        {
            prey.Die();
        }

        public void Resolve(IAnimal first, IAnimal second)
        {
            if (first == null || second == null)
                return;

            if (_interactionMatrix.TryGetValue((first.Role, second.Role), out var action))
            {
                action(first, second);
            }
        }
    }
}