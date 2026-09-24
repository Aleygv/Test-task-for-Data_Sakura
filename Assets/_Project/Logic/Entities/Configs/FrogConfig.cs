namespace _Project.Logic.Entities.Configs
{
    public class FrogConfig : AnimalConfig
    {
        public float StepDistance { get; }
        public float JumpInterval { get; }
        public float MinInitialTimerOffset { get; }
        public float MaxDistanceFromCenterSqr { get; }
        public float MaxTurnAngle { get; }
        
        public FrogConfig(AnimalTypeId animalTypeId, AnimalRole role, string prefabPath, float stepDistance,
            float jumpInterval, float minInitialTimerOffset, float maxDistanceFromCenterSqr, float maxTurnAngle)
            : base(animalTypeId, role, prefabPath)
        {
            StepDistance = stepDistance;
            JumpInterval = jumpInterval;
            MinInitialTimerOffset = minInitialTimerOffset;
            MaxDistanceFromCenterSqr = maxDistanceFromCenterSqr;
            MaxTurnAngle = maxTurnAngle;
        }
    }
}