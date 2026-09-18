using System;

namespace _Project.Logic.Entities
{
    public interface IAnimal
    {
        Guid Id { get; }
        AnimalRole Role { get; }
        event Action OnDie;
        event Action OnBounce;
        public event Action OnAte; // Событие: животное только что поело
        void Die();
        void Bounce();
        public void Eat();
        void Tick(float deltaTime);
    }
}