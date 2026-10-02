using System;
using _Project.Logic.Entities.Configs.Movement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Project.Logic.Entities.AIMovement
{
    public class JumpMovement : MovementBase
    {
        private JumpMovementConfig _config;
        private Vector3 _currentDirection;
        private float _nextJumpTime;

        public override void Initialize(MovementConfig config)
        {
            base.Initialize(config);

            _config = config as JumpMovementConfig;
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            _currentDirection = new Vector3(randomDir.x, 0f, randomDir.y);
            _nextJumpTime = Time.time + Random.Range(_config!.MinInitialTimerOffset, _config.JumpInterval);
        }

        public override Vector3 GetNextTarget()
        {
            Vector3 currentPos = transform.position;

            if (currentPos.sqrMagnitude > _config.MaxDistanceFromCenterSqr)
            {
                Vector3 toCenter = Vector3.zero - currentPos;
                toCenter.y = 0;
                _currentDirection = toCenter.normalized;
            }
            else
            {
                float angle = Random.Range(-_config.MaxTurnAngle, _config.MaxTurnAngle);
                _currentDirection = Quaternion.Euler(0, angle, 0) * _currentDirection;
                _currentDirection.y = 0;
                _currentDirection.Normalize();
            }

            return currentPos + _currentDirection * _config.StepDistance;
        }

        public override bool SetPosition(Vector3 targetLandingPos)
        {
            if (_isJumping)
            {
                return false;
            }

            if (Time.time < _nextJumpTime)
            {
                return false;
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

            if (NavMesh.SamplePosition(targetLandingPos, out NavMeshHit hit, _config.StepDistance, NavMesh.AllAreas))
            {
                _nextJumpTime = Time.time + _config.JumpInterval;
                PerformJumpAsync(hit.position, jumpDirection).Forget();
                return true;
            }

            return false;
        }

        private async UniTaskVoid PerformJumpAsync(Vector3 landingPosition, Vector3 direction)
        {
            _isJumping = true;
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
                while (elapsed < _config.JumpDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / _config.JumpDuration);
                    Vector3 currentPos = Vector3.Lerp(startPos, landingPosition, t);
                    currentPos.y += ParabolaMultiplier * _config.JumpHeight * t * (1f - t);
                    transform.position = currentPos;
                    await UniTask.Yield(PlayerLoopTiming.Update, destroyCancellationToken);
                }

                _agent.Warp(landingPosition);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (_agent != null)
                {
                    _agent.isStopped = false;
                }

                _isJumping = false;
            }
        }
    }
}