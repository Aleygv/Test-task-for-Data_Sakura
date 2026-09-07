using System;
using _Project.Logic.Extensions;

namespace _Project.Logic.Entities.Animals.Predators
{
    public class Snake : AnimalBase
    {
        public Snake(Guid id, LightVector3 position) : base(id, AnimalRole.Predator, position)
        { }
    }
}