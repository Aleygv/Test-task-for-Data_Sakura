using System;
using UnityEngine;

namespace _Project.Logic.Entities.Animals
{
    public abstract class AnimalBase : IAnimal
    {
        public Guid Id { get; }

        public AnimalRole Role { get; }

        public event Action OnDie;

        public event Action OnBounce;

        public event Action OnAte;

        protected readonly IMovement _movement;

        private readonly float _nextPositionRadius;

        protected AnimalBase(Guid id, AnimalRole role, IMovement movement, float nextPositionRadius)
        {
            Id = id;
            Role = role;
            _movement = movement;
            _nextPositionRadius = nextPositionRadius;
        }

        public virtual void Tick(float deltaTime)
        {
        }

        protected bool IsTargetReached(Vector3 target)
        {
            return Vector3.Distance(_movement.transform.position, target) <= _nextPositionRadius;
        }

        public virtual void Die() => OnDie?.Invoke();

        public virtual void Bounce()
        {
            _movement.Bounce();
            OnBounce?.Invoke();
        }

        public virtual void Eat() => OnAte?.Invoke();
    }
}