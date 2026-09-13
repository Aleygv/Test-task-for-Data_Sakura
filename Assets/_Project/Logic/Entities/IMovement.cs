using UnityEngine;

namespace _Project.Logic.Entities
{
    public interface IMovement
    {
        Vector3 CurrentPosition { get; }
        void MovePosition(Vector3 targetPosition);
        void Bounce();
    }
}