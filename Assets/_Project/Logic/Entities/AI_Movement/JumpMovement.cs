using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Logic.Entities.AI_Movement
{
    public class JumpMovement : MonoBehaviour, IMovement
    {
        private const float MinDirectionMagnitudeSqr = 0.01f;
        private const float DefaultBounceDuration = 0.25f;
        private const float BounceArcHeight = 0.5f;
        private const float WarpSampleTolerance = 0.5f;
        private const float ParabolaMultiplier = 4f;

        [SerializeField] private NavMeshAgent agent;

        [Header("Frog Jump Settings")] 
        [SerializeField] private float jumpDistance = 2.5f;
        [SerializeField] private float jumpHeight = 1.0f;
        [SerializeField] private float jumpDuration = 0.35f;
        [SerializeField] private float bounceDistance = 2.0f;

        private bool _isJumping;

        public Vector3 CurrentPosition => transform.position;

        public void MovePosition(Vector3 targetLandingPos)
        {
            if (_isJumping)
            {
                return;
            }

            Vector3 jumpDirection = targetLandingPos - transform.position;
            jumpDirection.y = 0;

            if (jumpDirection.sqrMagnitude < MinDirectionMagnitudeSqr)
            {
                jumpDirection = transform.forward;
            }
            else
            {
                jumpDirection.Normalize();
            }

            if (NavMesh.SamplePosition(targetLandingPos, out NavMeshHit hit, jumpDistance, NavMesh.AllAreas))
            {
                PerformJumpAsync(hit.position, jumpDirection).Forget();
            }
        }

        public void Bounce()
        {
            BounceAsync().Forget();
        }

        private async UniTaskVoid BounceAsync()
        {
            _isJumping = true;
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

            while (elapsed < DefaultBounceDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / DefaultBounceDuration);
                Vector3 currentPos = Vector3.Lerp(startPos, targetBouncePos, t);
                currentPos.y += ParabolaMultiplier * BounceArcHeight * t * (1f - t);

                if (NavMesh.SamplePosition(currentPos, out NavMeshHit sampleHit, WarpSampleTolerance, NavMesh.AllAreas))
                {
                    agent.Warp(sampleHit.position);
                }

                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }

            agent.isStopped = false;
            _isJumping = false;
        }

        private async UniTaskVoid PerformJumpAsync(Vector3 landingPosition, Vector3 direction)
        {
            _isJumping = true;
            agent.isStopped = true;
            agent.ResetPath();

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            Vector3 startPos = transform.position;
            float elapsed = 0f;
            System.Threading.CancellationToken ct = this.GetCancellationTokenOnDestroy();

            while (elapsed < jumpDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / jumpDuration);
                Vector3 currentPos = Vector3.Lerp(startPos, landingPosition, t);
                currentPos.y += ParabolaMultiplier * jumpHeight * t * (1f - t);
                transform.position = currentPos;
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }

            agent.Warp(landingPosition);
            agent.isStopped = false;
            _isJumping = false;
        }
    }
}