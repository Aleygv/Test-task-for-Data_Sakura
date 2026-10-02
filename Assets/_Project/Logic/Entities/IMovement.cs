using _Project.Logic.Entities.Configs.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Logic.Entities
{
    public interface IMovement
    {
        Transform transform { get; }
        void Initialize(MovementConfig config);
        Vector3 GetNextTarget();
        bool SetPosition(Vector3 targetPosition);
        void Bounce();
    }
}