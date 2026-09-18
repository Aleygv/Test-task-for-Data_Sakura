using System;
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
        
        [SerializeField] protected NavMeshAgent agent;
        [SerializeField] protected float bounceDistance = 2.0f;
        [SerializeField] protected float bounceDuration = 0.25f;

        protected bool IsJumping;
        
        private const float BounceArcHeight = 0.5f;

        public Vector3 CurrentPosition => transform.position;

        public abstract void MovePosition(Vector3 targetPosition);

        public virtual void Bounce()
        {
            if (IsJumping)
            {
                return;
            }

            BounceAsync().Forget();
        }

        protected virtual async UniTaskVoid BounceAsync()
        {
            IsJumping = true;
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

            try
            {
                while (elapsed < bounceDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / bounceDuration);
                    Vector3 currentPos = Vector3.Lerp(startPos, targetBouncePos, t);
                    currentPos.y += ParabolaMultiplier * BounceArcHeight * t * (1f - t);

                    if (NavMesh.SamplePosition(currentPos, out NavMeshHit sampleHit, WarpSampleTolerance, NavMesh.AllAreas))
                    {
                        agent.Warp(sampleHit.position);
                    }

                    await UniTask.Yield(PlayerLoopTiming.Update, destroyCancellationToken);
                }

                agent.isStopped = false;
                IsJumping = false;
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}