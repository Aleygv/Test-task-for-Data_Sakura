using UnityEngine;
using UnityEngine.AI;

namespace _Project.Logic.Entities.AIMovement
{
    public class LinearMovement : MovementBase
    {
        [SerializeField] private float _sampleDistance = 2f;

        public override void MovePosition(Vector3 targetPosition)
        {
            if (IsJumping)
            {
                return;
            }

            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, _sampleDistance, NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);
            }
        }
    }
}