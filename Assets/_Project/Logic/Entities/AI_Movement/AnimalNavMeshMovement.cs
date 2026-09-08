using Cysharp.Threading.Tasks;
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

        public void BounceFrom(Vector3 fromPosition)
        {
            // Vector3 direction = transform.position - fromPosition;
            // direction.y = 0;
            //
            // if (direction.sqrMagnitude < 0.001f)
            // {
            //     Vector2 randomDir = Random.insideUnitCircle.normalized;
            //     direction = new Vector3(randomDir.x, 0, randomDir.y);
            // }
            // else
            // {
            //     direction.Normalize();
            // }
            //
            // Vector3 targetBouncePos = transform.position + direction * bounceDistance;
            //
            // if (NavMesh.SamplePosition(targetBouncePos, out NavMeshHit hit, bounceDistance, NavMesh.AllAreas ))
            // {
            //     agent.ResetPath();
            //     _isWaiting = false;
            //
            //     agent.SetDestination(hit.position);
            // }

            _ = BounceRoutine(fromPosition);
        }

        private void SetNewDestination()
        {
            // Vector3 target = GetRandomPositionAround(transform.position, minDistance, maxDistance);
            Vector3 target = GetRandomPositionInCameraView(_mainCamera);

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

        private Vector3 GetRandomPositionInCameraView(Camera viewCamera, float groundY = 0f, float padding = 0.1f)
        {
            float randomX = Random.Range(padding, 1f - padding);
            float randomY = Random.Range(padding, 1f - padding);

            Ray ray = viewCamera.ViewportPointToRay(new Vector3(randomX, randomY, 0f));

            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, groundY, 0));

            if (groundPlane.Raycast(ray, out var enter))
            {
                Vector3 worldPoint = ray.GetPoint(enter);
                return worldPoint;
            }

            return Vector3.zero;
        }

        private async UniTaskVoid BounceRoutine(Vector3 fromPosition)
        {
            Vector3 direction = transform.position - fromPosition;
            direction.y = 0;
            direction = direction.sqrMagnitude < 0.01f ? Vector3.forward : direction.normalized;

            agent.isStopped = true;
            agent.ResetPath();

            float duration = 0.2f;
            float elapsed = 0f;
            float startSpeed = 15f;

            var ct = this.GetCancellationTokenOnDestroy();

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;

                float currentSpeed = Mathf.Lerp(startSpeed, 0f, progress);

                Vector3 nextPosition = transform.position + direction * (currentSpeed * Time.deltaTime);

                if (NavMesh.SamplePosition(nextPosition, out NavMeshHit hit, 0.5f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position);
                }

                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }

            agent.isStopped = false;
            _isWaiting = true;
            _waitTimer = 0.4f;
        }
    }
}