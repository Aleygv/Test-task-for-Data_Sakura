using System;
using _Project.Logic.Entities.Configs.Movement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Logic.Entities.Animals
{
    public class Frog : AnimalBase
    {
        private readonly JumpMovementConfig _config;
        private float _timer;
        private Vector3 _currentDirection;

        public Frog(Guid id, AnimalRole role, IMovement movement, JumpMovementConfig config) : base(id, role, movement)
        {
            _config = config;
            
            _timer = Random.Range(_config.MinInitialTimerOffset, _config.JumpInterval);

            Vector2 randomDir = Random.insideUnitCircle.normalized;
            _currentDirection = new Vector3(randomDir.x, 0f, randomDir.y);
        }

        public override void Tick(float deltaTime)
        {
            _timer -= deltaTime;
            if (_timer <= 0f)
            {
                _timer = _config.JumpInterval;
                Vector3 target = CalculateNextJumpTarget();
                _movement.MovePosition(target);
            }
        }

        private Vector3 CalculateNextJumpTarget()
        {
            Vector3 currentPos = _movement.CurrentPosition;

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
    }
}