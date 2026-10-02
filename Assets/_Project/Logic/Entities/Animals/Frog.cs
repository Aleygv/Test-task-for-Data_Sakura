using System;
using UnityEngine;

namespace _Project.Logic.Entities.Animals
{
    public class Frog : AnimalBase
    {
        private Vector3 _currentTarget;
        private bool _hasTarget;

        public Frog(Guid id, AnimalRole role, IMovement movement, float nextPositionRadius)
            : base(id, role, movement, nextPositionRadius)
        {
            _currentTarget = _movement.transform.position;
            _hasTarget = false;
        }

        public override void Tick(float deltaTime)
        {
            if (!_hasTarget || IsTargetReached(_currentTarget))
            {
                Vector3 nextTarget = _movement.GetNextTarget();
                if (_movement.SetPosition(nextTarget))
                {
                    _currentTarget = nextTarget;
                    _hasTarget = true;
                }
            }
        }
    }
}