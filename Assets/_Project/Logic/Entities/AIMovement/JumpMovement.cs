using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace _Project.Logic.Entities.AIMovement
{
    public class JumpMovement : MovementBase
    {
        [SerializeField] private float _jumpDistance = 2.5f;
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _jumpDuration = 0.35f;

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

            if (NavMesh.SamplePosition(targetLandingPos, out NavMeshHit hit, _jumpDistance, NavMesh.AllAreas))
            {
                PerformJumpAsync(hit.position, jumpDirection).Forget();
            }
        }

        private async UniTaskVoid PerformJumpAsync(Vector3 landingPosition, Vector3 direction)
        {
            IsJumping = true;
            _agent.isStopped = true;
            _agent.ResetPath();

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            Vector3 startPos = transform.position;
            float elapsed = 0f;

            try
            {
                while (elapsed < _jumpDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / _jumpDuration);
                    Vector3 currentPos = Vector3.Lerp(startPos, landingPosition, t);
                    currentPos.y += ParabolaMultiplier * _jumpHeight * t * (1f - t);
                    transform.position = currentPos;
                    await UniTask.Yield(PlayerLoopTiming.Update, destroyCancellationToken);
                }

                _agent.Warp(landingPosition);
                _agent.isStopped = false;
                IsJumping = false;
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        public class Fabric : PlaceholderFactory<JumpMovement>
        {
        }
    }
}