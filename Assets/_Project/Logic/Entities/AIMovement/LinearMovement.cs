using _Project.Logic.Entities.Configs.Movement;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Project.Logic.Entities.AIMovement
{
    public class LinearMovement : MovementBase
    {
        private LinearMovementConfig _config;

        public override void Initialize(MovementConfig config)
        {
            base.Initialize(config);

            _config = config as LinearMovementConfig;
        }

        public override Vector3 GetNextTarget()
        {
            Vector3 currentPos = transform.position;

            if (currentPos.sqrMagnitude > _config.MaxArenaRadiusSqr)
            {
                Vector2 randomInCenter = Random.insideUnitCircle * _config.CenterReturnRadius;
                return new Vector3(randomInCenter.x, 0f, randomInCenter.y);
            }

            Vector2 randomOffset = Random.insideUnitCircle.normalized *
                                   Random.Range(_config.MinTargetDistance, _config.MaxTargetDistance);
            return new Vector3(currentPos.x + randomOffset.x, currentPos.y, currentPos.z + randomOffset.y);
        }

        public override bool SetPosition(Vector3 targetPosition)
        {
            if (_isJumping)
            {
                return false;
            }

            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, _config.CenterReturnRadius,
                    NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);
                return true;
            }

            return false;
        }
    }
}