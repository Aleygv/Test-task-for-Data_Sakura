using System;
using _Project.Logic.Entities.Configs.Movement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Logic.Entities.AIMovement
{
    public abstract class MovementBase : MonoBehaviour, IMovement
    {
        protected const float MinDirectionMagnitudeSqr = 0.01f;
        protected const float WarpSampleTolerance = 0.5f;
        protected const float ParabolaMultiplier = 4f;
        protected const float BounceDistance = 2.0f;
        protected const float BounceDuration = 0.25f;

        private const float BounceArcHeight = 0.5f;

        protected NavMeshAgent _agent;
        protected bool _isJumping;

        public abstract bool SetPosition(Vector3 targetPosition);

        public abstract Vector3 GetNextTarget();

        public virtual void Bounce()
        {
            if (_isJumping)
            {
                return;
            }

            BounceAsync().Forget();
        }

        public virtual void Initialize(MovementConfig config)
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        protected virtual async UniTaskVoid BounceAsync()
        {
            _isJumping = true;
            _agent.isStopped = true;
            _agent.ResetPath();

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
            Vector3 targetBouncePos = startPos + bounceDirection * BounceDistance;

            if (NavMesh.SamplePosition(targetBouncePos, out NavMeshHit hit, BounceDistance, NavMesh.AllAreas))
            {
                targetBouncePos = hit.position;
            }

            float elapsed = 0f;

            try
            {
                while (elapsed < BounceDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / BounceDuration);
                    Vector3 currentPos = Vector3.Lerp(startPos, targetBouncePos, t);
                    currentPos.y += ParabolaMultiplier * BounceArcHeight * t * (1f - t);

                    if (NavMesh.SamplePosition(currentPos, out NavMeshHit sampleHit, WarpSampleTolerance,
                            NavMesh.AllAreas))
                    {
                        _agent.Warp(sampleHit.position);
                    }

                    await UniTask.Yield(PlayerLoopTiming.Update, destroyCancellationToken);
                }
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