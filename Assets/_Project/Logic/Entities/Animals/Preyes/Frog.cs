using System;
using _Project.Logic.Extensions;
using UnityEngine;

namespace _Project.Logic.Entities.Animals.Preyes
{
    public class Frog : IAnimal
    {
        public Guid Id { get; }
        public AnimalRole Role { get; }
        public LightVector3 Position { get; set; }
        public event Action OnDie;

        public Frog(Guid id, LightVector3 position)
        {
            Id = id;
            Role = AnimalRole.Prey;
            Position = position;
        }

        public void Die()
        {
            OnDie?.Invoke();
            Debug.Log($"Лягух с Id {Id} съеден");
        }

        public void Bounce(LightVector3 fromPosition)
        {
            Debug.Log($"Лягух оттолкнулся на {Position.x} метров");
        }
    }
}