using _Project.Logic.Entities;

namespace _Project.Logic.Services
{
    public interface IAnimalCollisionResolver
    {
        void HandlePreyWithPray(IAnimal a, IAnimal b);
        void HandlePredatorWithPredator(IAnimal a, IAnimal b);
        void Resolve(IAnimal first, IAnimal second);
    }
}