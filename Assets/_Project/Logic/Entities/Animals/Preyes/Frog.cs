using System;
using _Project.Logic.Extensions;
using UnityEngine;

namespace _Project.Logic.Entities.Animals.Preyes
{
    public class Frog : AnimalBase
    {
        public Frog(Guid id, LightVector3 position) : base(id, AnimalRole.Prey, position)
        {
        }

        public override void Bounce(LightVector3 pos)
        {
            base.Bounce(pos);
            Debug.Log($"Лягух отскочил!");
        }
    }
}