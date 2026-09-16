using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Logic.Entities.Animals.Preyes
{
    public class Frog : AnimalBase
    {
        private const float DefaultStepDistance = 2.5f;
        private const float DefaultJumpInterval = 1.5f;
        private const float MinInitialTimerOffset = 0.2f;
        private const float MaxDistanceFromCenterSqr = 100.0f;
        private const float MaxTurnAngle = 25.0f;

        private readonly float _stepDistance;
        private readonly float _jumpInterval;
        private float _timer;
        private Vector3 _currentDirection;

        public Frog(Guid id, AnimalRole role, IMovement movement, float stepDistance = DefaultStepDistance, float jumpInterval = DefaultJumpInterval)
            : base(id, role, movement)
        {
            _stepDistance = stepDistance;
            _jumpInterval = jumpInterval;
            _timer = Random.Range(MinInitialTimerOffset, _jumpInterval);

            Vector2 randomDir = Random.insideUnitCircle.normalized;
            _currentDirection = new Vector3(randomDir.x, 0, randomDir.y);
        }

        public override void Tick(float deltaTime)
        {
            _timer -= deltaTime;
            if (_timer <= 0f)
            {
                _timer = _jumpInterval;
                Vector3 target = CalculateNextJumpTarget();
                Movement.MovePosition(target);
            }
        }

        private Vector3 CalculateNextJumpTarget()
        {
            Vector3 currentPos = Movement.CurrentPosition;

            if (currentPos.sqrMagnitude > MaxDistanceFromCenterSqr)
            {
                Vector3 toCenter = Vector3.zero - currentPos;
                toCenter.y = 0;
                _currentDirection = toCenter.normalized;
            }
            else
            {
                float angle = Random.Range(-MaxTurnAngle, MaxTurnAngle);
                _currentDirection = Quaternion.Euler(0, angle, 0) * _currentDirection;
                _currentDirection.y = 0;
                _currentDirection.Normalize();
            }

            return currentPos + _currentDirection * _stepDistance;
        }
    }
}