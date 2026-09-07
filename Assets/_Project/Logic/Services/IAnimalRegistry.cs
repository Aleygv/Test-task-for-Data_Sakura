using System;
using _Project.Logic.Entities;

namespace _Project.Logic.Services
{
    public interface IAnimalRegistry
    {
        event Action<AnimalRole, int> OnAnimalCountChanged;

        int GetCount(AnimalRole role);
        void Register(IAnimal animal);
    }
}