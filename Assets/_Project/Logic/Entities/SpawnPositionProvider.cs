using _Project.Logic.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Logic.Entities
{
    public class SpawnPositionProvider : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float navMeshSampleDistance = 2f;

        public Vector3 GetSpawnPosition()
        {
            Vector3 targetPoint = mainCamera.GetRandomPositionInCameraView();

            if (NavMesh.SamplePosition(targetPoint, out NavMeshHit hit, navMeshSampleDistance, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return targetPoint;
        }
    }
}