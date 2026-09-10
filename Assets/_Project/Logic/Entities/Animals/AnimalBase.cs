using System;
using _Project.Logic.Extensions;

namespace _Project.Logic.Entities.Animals
{
    public abstract class AnimalBase : IAnimal
    {
        public Guid Id { get; }
        public AnimalRole Role { get; }
        public LightVector3 Position { get; set; }
        public event Action OnDie;
        public event Action<LightVector3> OnBounce;
        public event Action OnAte;

        protected AnimalBase(Guid id, AnimalRole role, LightVector3 position)
        {
            Id = id;
            Role = role;
            Position = position;
        }

        public virtual void Die() => OnDie?.Invoke();
        public virtual void Bounce(LightVector3 pos) => OnBounce?.Invoke(pos);
        public virtual void Eat() => OnAte?.Invoke();
    }
}