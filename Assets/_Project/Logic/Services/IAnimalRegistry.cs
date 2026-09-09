using System;
using _Project.Logic.Entities;

namespace _Project.Logic.Services
{
    public interface IAnimalRegistry
    {
        event Action<AnimalRole, int> OnAnimalDied;
        void Register(IAnimal animal);
    }
}