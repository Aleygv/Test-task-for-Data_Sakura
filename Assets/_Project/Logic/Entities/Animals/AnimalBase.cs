using System;

namespace _Project.Logic.Entities.Animals
{
    public abstract class AnimalBase : IAnimal
    {
        public Guid Id { get; }
        public AnimalRole Role { get; }
        public event Action OnDie;
        public event Action OnBounce;
        public event Action OnAte;

        protected readonly IMovement Movement;

        protected AnimalBase(Guid id, AnimalRole role, IMovement movement)
        {
            Id = id;
            Role = role;
            Movement = movement;
        }

        public abstract void Tick(float deltaTime);

        public virtual void Die() => OnDie?.Invoke();
        public virtual void Bounce()
        {
            Movement.Bounce();
            OnBounce?.Invoke();
        }

        public virtual void Eat() => OnAte?.Invoke();
    }
}