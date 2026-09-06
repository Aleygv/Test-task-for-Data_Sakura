using System;
using _Project.Logic.Extensions;
using UnityEngine;

namespace _Project.Logic.Entities.Animals.Predators
{
    public class Snake : IAnimal
    {
        public Guid Id { get; }
        public AnimalRole Role { get; }
        public LightVector3 Position { get; set; }
        public event Action OnDie;

        public Snake(Guid id, LightVector3 position)
        {
            Id = id;
            Role = AnimalRole.Predator;
            Position = position;
        }

        public void Die()
        {
            OnDie?.Invoke();
            Debug.Log($"Змейка с Id {Id} умерла");
        }

        public void Bounce(LightVector3 fromPosition)
        {
            Debug.Log($"Змейка отскочила в на {Position.x} метров");
        }
    }
}