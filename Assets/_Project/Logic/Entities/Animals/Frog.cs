using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Logic.Entities.Animals
{
    public class Frog : AnimalBase
    {
        private readonly float _stepDistance;
        private readonly float _jumpInterval;
        private readonly float _maxDistanceFromCenterSqr;
        private readonly float _maxTurnAngle;
        private float _timer;
        private Vector3 _currentDirection;

        public Frog(Guid id, AnimalRole role, IMovement movement, float stepDistance, float jumpInterval,
            float minInitialTimerOffset, float maxDistanceFromCenterSqr, float maxTurnAngle) : base(id, role, movement)
        {
            _stepDistance = stepDistance;
            _jumpInterval = jumpInterval;
            _maxDistanceFromCenterSqr = maxDistanceFromCenterSqr;
            _maxTurnAngle = maxTurnAngle;
            _timer = Random.Range(minInitialTimerOffset, _jumpInterval);

            Vector2 randomDir = Random.insideUnitCircle.normalized;
            _currentDirection = new Vector3(randomDir.x, 0f, randomDir.y);
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

            if (currentPos.sqrMagnitude > _maxDistanceFromCenterSqr)
            {
                Vector3 toCenter = Vector3.zero - currentPos;
                toCenter.y = 0;
                _currentDirection = toCenter.normalized;
            }
            else
            {
                float angle = Random.Range(-_maxTurnAngle, _maxTurnAngle);
                _currentDirection = Quaternion.Euler(0, angle, 0) * _currentDirection;
                _currentDirection.y = 0;
                _currentDirection.Normalize();
            }

            return currentPos + _currentDirection * _stepDistance;
        }
    }
}