namespace _Project.Logic.Entities.Configs.Movement
{
    public class JumpMovementConfig : MovementConfig
    {
        public float StepDistance { get; }
        public float JumpInterval { get; }
        public float MinInitialTimerOffset { get; }
        public float MaxDistanceFromCenterSqr { get; }
        public float MaxTurnAngle { get; }

        public JumpMovementConfig(float stepDistance, float jumpInterval, float minInitialTimerOffset,
            float maxDistanceFromCenterSqr, float maxTurnAngle)
        {
            StepDistance = stepDistance;
            JumpInterval = jumpInterval;
            MinInitialTimerOffset = minInitialTimerOffset;
            MaxDistanceFromCenterSqr = maxDistanceFromCenterSqr;
            MaxTurnAngle = maxTurnAngle;
        }
    }
}