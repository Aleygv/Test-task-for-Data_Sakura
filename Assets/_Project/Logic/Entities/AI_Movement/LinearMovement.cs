using UnityEngine;
using UnityEngine.AI;

namespace _Project.Logic.Entities.AI_Movement
{
    public class LinearMovement : AnimalMovementBase
    {
        protected override void SetNewDestination()
        {
            Vector3 target = GetRandomPositionInCameraView(MainCamera);

            if (NavMesh.SamplePosition(target, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            else
            {
                IsWaiting = false;
            }
        }
    }
}