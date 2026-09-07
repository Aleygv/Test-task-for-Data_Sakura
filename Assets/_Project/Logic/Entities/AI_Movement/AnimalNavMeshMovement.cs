using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Project.Logic.Entities.AI_Movement
{
    public class AnimalNavMeshMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;

        [Header("Movement Settings")] 
        [SerializeField] private float minDistance = 3f;
        [SerializeField] private float maxDistance = 7f;
        [SerializeField] private float minWaitTime = 0.5f;
        [SerializeField] private float maxWaitTime = 2.0f;

        [Header("Bounce Settings")] 
        [SerializeField] private float bounceDistance = 3f;

        private float _waitTimer;
        private bool _isWaiting;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (_isWaiting)
            {
                _waitTimer -= Time.deltaTime;
                if (_waitTimer <= 0)
                {
                    _isWaiting = false;
                    SetNewDestination();
                }
                return;
            }
            
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                _isWaiting = true;
                _waitTimer = Random.Range(minWaitTime, maxWaitTime);
            }
        }

        private void SetNewDestination()
        {
            Vector3 target = GetRandomPositionAround(transform.position, minDistance, maxDistance);

            if (NavMesh.SamplePosition(target, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            else
            {
                _isWaiting = false;
            }
        }

        private Vector3 GetRandomPositionAround(Vector3 origin, float minRadius, float maxRadius)
        {
            Vector2 circle = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
            return new Vector3(origin.x + circle.x, origin.y, origin.z + circle.y);
        }

        public void BounceFrom(Vector3 fromPosition)
        {
            Vector3 direction = transform.position - fromPosition;
            direction.y = 0;

            if (direction.sqrMagnitude < 0.001f)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                direction = new Vector3(randomDir.x, 0, randomDir.y);
            }
            else
            {
                direction.Normalize();
            }

            Vector3 targetBouncePos = transform.position + direction * bounceDistance;

            if (NavMesh.SamplePosition(targetBouncePos, out NavMeshHit hit, bounceDistance, NavMesh.AllAreas ))
            {
                agent.ResetPath();
                _isWaiting = false;

                agent.SetDestination(hit.position);
            }
        }
    }
}