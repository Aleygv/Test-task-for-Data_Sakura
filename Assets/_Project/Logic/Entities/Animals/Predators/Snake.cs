using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Logic.Entities.Animals.Predators
{
    public class Snake : AnimalBase
    {
        private const float MaxRepathInterval = 4.0f;
        private const float ArrivalThreshold = 1.2f;
        private const float BounceTargetOffset = 6.0f;
        private const float MaxArenaRadiusSqr = 80.0f;
        private const float CenterReturnRadius = 4.0f;
        private const float MinTargetDistance = 4.0f;
        private const float MaxTargetDistance = 8.0f;

        private Vector3 _currentTarget;
        private float _repathTimer;

        public Snake(Guid id, AnimalRole role, IMovement movement) : base(id, role, movement)
        {
            _currentTarget = Vector3.zero;
            _repathTimer = 0f;
        }

        public override void Tick(float deltaTime)
        {
            _repathTimer -= deltaTime;
            Vector3 currentPos = Movement.CurrentPosition;

            if (_currentTarget == Vector3.zero
                || Vector3.Distance(currentPos, _currentTarget) < ArrivalThreshold
                || _repathTimer <= 0f)
            {
                _repathTimer = MaxRepathInterval;
                _currentTarget = PickNewTarget(currentPos);
                Movement.MovePosition(_currentTarget);
            }
        }

        public override void Bounce()
        {
            base.Bounce();
            Vector3 currentPos = Movement.CurrentPosition;
            Vector3 bounceDirection = (Vector3.zero - currentPos).normalized;
            _currentTarget = currentPos + bounceDirection * BounceTargetOffset;
            _repathTimer = MaxRepathInterval;
            Movement.MovePosition(_currentTarget);
        }

        private Vector3 PickNewTarget(Vector3 currentPos)
        {
            if (currentPos.sqrMagnitude > MaxArenaRadiusSqr)
            {
                Vector2 randomInCenter = Random.insideUnitCircle * CenterReturnRadius;
                return new Vector3(randomInCenter.x, 0, randomInCenter.y);
            }

            Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(MinTargetDistance, MaxTargetDistance);
            return new Vector3(currentPos.x + randomOffset.x, currentPos.y, currentPos.z + randomOffset.y);
        }
    }
}