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
        void Die();
        void Bounce(LightVector3 position);
    }

    public enum AnimalRole
    {
        Prey,
        Predator
    }
}