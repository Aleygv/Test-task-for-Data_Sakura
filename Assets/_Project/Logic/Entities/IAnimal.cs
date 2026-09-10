using System;
using _Project.Logic.Extensions;

namespace _Project.Logic.Entities
{
    public interface IAnimal
    {
        Guid Id { get; }
        AnimalRole Role { get; }
        LightVector3 Position { get; set; }
        event Action OnDie;
        event Action<LightVector3> OnBounce;
        public event Action OnAte; // Событие: животное только что поело
        void Die();
        void Bounce(LightVector3 position);
        public void Eat();
    }

    public enum AnimalRole
    {
        Prey,
        Predator
    }
}