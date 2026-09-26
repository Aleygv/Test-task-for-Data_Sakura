using System;
using _Project.Logic.Entities.Configs.Movement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Logic.Entities.Animals
{
    public class Snake : AnimalBase
    {
        private LinearMovementConfig _config;
        private Vector3 _currentTarget;
        private float _repathTimer;

        public Snake(Guid id, AnimalRole role, IMovement movement, LinearMovementConfig config)
            : base(id, role, movement)
        {
            _config = config;
            _currentTarget = Vector3.zero;
            _repathTimer = 0f;
        }

        public override void Tick(float deltaTime)
        {
            _repathTimer -= deltaTime;
            Vector3 currentPos = _movement.CurrentPosition;

            if (_currentTarget == Vector3.zero
                || Vector3.Distance(currentPos, _currentTarget) < _config.ArrivalThreshold
                || _repathTimer <= 0f)
            {
                _repathTimer = _config.MaxRepathInterval;
                _currentTarget = PickNewTarget(currentPos);
                _movement.MovePosition(_currentTarget);
            }
        }

        private Vector3 PickNewTarget(Vector3 currentPos)
        {
            if (currentPos.sqrMagnitude > _config.MaxArenaRadiusSqr)
            {
                Vector2 randomInCenter = Random.insideUnitCircle * _config.CenterReturnRadius;
                return new Vector3(randomInCenter.x, 0f, randomInCenter.y);
            }

            Vector2 randomOffset = Random.insideUnitCircle.normalized *
                                   Random.Range(_config.MinTargetDistance, _config.MaxTargetDistance);
            return new Vector3(currentPos.x + randomOffset.x, currentPos.y, currentPos.z + randomOffset.y);
        }
    }
}