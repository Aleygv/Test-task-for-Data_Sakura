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

        void Die();
        void Bounce(LightVector3 fromPosition);
    }

    public enum AnimalRole
    {
        Prey,
        Predator
    }
}