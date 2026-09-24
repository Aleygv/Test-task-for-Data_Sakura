using _Project.Logic.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Logic.Entities
{
    public class SpawnPositionProvider : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _navMeshSampleDistance = 2f;

        public Vector3 GetSpawnPosition()
        {
            Vector3 targetPoint = _mainCamera.GetRandomPositionInCameraView();

            if (NavMesh.SamplePosition(targetPoint, out NavMeshHit hit, _navMeshSampleDistance, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return targetPoint;
        }
    }
}