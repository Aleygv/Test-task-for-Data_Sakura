using _Project.Logic.Entities;
using _Project.Logic.Extensions;
using UnityEngine;

namespace _Project.Logic.Services
{
    public interface IPhysicService
    {
        
    }

    public class PhysicService : IPhysicService
    {
        private readonly Collider[] _hitColliders = new Collider[20];
        private readonly Vector3 _boxHalfExtends = new Vector3(0.5f, 0.5f, 0.5f);
    }
}