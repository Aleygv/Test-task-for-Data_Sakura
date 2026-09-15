using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Logic.Entities.AIMovement
{
    public class JumpMovement : MovementBase
    {
        [Header("Jump Settings")] [SerializeField]
        private float jumpDistance = 2.5f;

        [SerializeField] private float jumpHeight = 1.0f;
        [SerializeField] private float jumpDuration = 0.35f;

        public override void MovePosition(Vector3 targetLandingPos)
        {
            if (IsJumping)
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

        private async UniTaskVoid PerformJumpAsync(Vector3 landingPosition, Vector3 direction)
        {
            IsJumping = true;
            agent.isStopped = true;
            agent.ResetPath();

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            Vector3 startPos = transform.position;
            float elapsed = 0f;

            try
            {
                while (elapsed < jumpDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / jumpDuration);
                    Vector3 currentPos = Vector3.Lerp(startPos, landingPosition, t);
                    currentPos.y += ParabolaMultiplier * jumpHeight * t * (1f - t);
                    transform.position = currentPos;
                    await UniTask.Yield(PlayerLoopTiming.Update, destroyCancellationToken);
                }

                agent.Warp(landingPosition);
                agent.isStopped = false;
                IsJumping = false;
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}