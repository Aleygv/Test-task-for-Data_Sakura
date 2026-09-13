using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Logic.Entities.AI_Movement
{
    public class LinearMovement : MonoBehaviour, IMovement
    {
        private const float MinDirectionMagnitudeSqr = 0.01f;
        private const float WarpSampleTolerance = 0.5f;

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float sampleDistance = 2f;
        [SerializeField] private float bounceDistance = 2f;
        [SerializeField] private float bounceDuration = 0.25f;

        private bool _isBouncing;

        public Vector3 CurrentPosition => transform.position;

        public void MovePosition(Vector3 targetPosition)
        {
            if (_isBouncing)
            {
                return;
            }

            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, sampleDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }

        public void Bounce()
        {
            BounceAsync().Forget();
        }

        private async UniTaskVoid BounceAsync()
        {
            if (_isBouncing)
            {
                return;
            }
            _isBouncing = true;

            agent.isStopped = true;
            agent.ResetPath();

            Vector3 bounceDirection = -transform.forward;
            bounceDirection.y = 0;
            if (bounceDirection.sqrMagnitude < MinDirectionMagnitudeSqr)
            {
                bounceDirection = -Vector3.forward;
            }
            else
            {
                bounceDirection.Normalize();
            }

            Vector3 startPos = transform.position;
            Vector3 targetBouncePos = startPos + bounceDirection * bounceDistance;

            if (NavMesh.SamplePosition(targetBouncePos, out NavMeshHit hit, bounceDistance, NavMesh.AllAreas))
            {
                targetBouncePos = hit.position;
            }

            float elapsed = 0f;
            System.Threading.CancellationToken ct = this.GetCancellationTokenOnDestroy();

            while (elapsed < bounceDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / bounceDuration);
                Vector3 nextPos = Vector3.Lerp(startPos, targetBouncePos, t);

                if (NavMesh.SamplePosition(nextPos, out NavMeshHit sampleHit, WarpSampleTolerance, NavMesh.AllAreas))
                {
                    agent.Warp(sampleHit.position);
                }

                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }

            agent.isStopped = false;
            _isBouncing = false;
        }
    }
}