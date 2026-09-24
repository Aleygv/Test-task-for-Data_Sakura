using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Logic.Entities.Animals
{
    public class Snake : AnimalBase
    {
        private readonly float _maxRepathInterval;
        private readonly float _arrivalThreshold;
        private readonly float _maxArenaRadiusSqr;
        private readonly float _centerReturnRadius;
        private readonly float _minTargetDistance;
        private readonly float _maxTargetDistance;

        private Vector3 _currentTarget;
        private float _repathTimer;

        public Snake(Guid id, AnimalRole role, IMovement movement,
            float maxRepathInterval, float arrivalThreshold, float maxArenaRadiusSqr,
            float centerReturnRadius, float minTargetDistance, float maxTargetDistance)
            : base(id, role, movement)
        {
            _maxRepathInterval = maxRepathInterval;
            _arrivalThreshold = arrivalThreshold;
            _maxArenaRadiusSqr = maxArenaRadiusSqr;
            _centerReturnRadius = centerReturnRadius;
            _minTargetDistance = minTargetDistance;
            _maxTargetDistance = maxTargetDistance;

            _currentTarget = Vector3.zero;
            _repathTimer = 0f;
        }

        public override void Tick(float deltaTime)
        {
            _repathTimer -= deltaTime;
            Vector3 currentPos = Movement.CurrentPosition;

            if (_currentTarget == Vector3.zero
                || Vector3.Distance(currentPos, _currentTarget) < _arrivalThreshold
                || _repathTimer <= 0f)
            {
                _repathTimer = _maxRepathInterval;
                _currentTarget = PickNewTarget(currentPos);
                Movement.MovePosition(_currentTarget);
            }
        }

        private Vector3 PickNewTarget(Vector3 currentPos)
        {
            if (currentPos.sqrMagnitude > _maxArenaRadiusSqr)
            {
                Vector2 randomInCenter = Random.insideUnitCircle * _centerReturnRadius;
                return new Vector3(randomInCenter.x, 0f, randomInCenter.y);
            }

            Vector2 randomOffset = Random.insideUnitCircle.normalized *
                                   Random.Range(_minTargetDistance, _maxTargetDistance);
            return new Vector3(currentPos.x + randomOffset.x, currentPos.y, currentPos.z + randomOffset.y);
        }
    }
}