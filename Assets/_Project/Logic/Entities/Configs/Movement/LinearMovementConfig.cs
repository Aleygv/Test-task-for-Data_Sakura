namespace _Project.Logic.Entities.Configs.Movement
{
    public class LinearMovementConfig : MovementConfig
    {
        public float MaxRepathInterval { get; }
        public float ArrivalThreshold { get; }
        public float MaxArenaRadiusSqr { get; }
        public float CenterReturnRadius { get; }
        public float MinTargetDistance { get; }
        public float MaxTargetDistance { get; }

        public LinearMovementConfig(float maxRepathInterval, float arrivalThreshold,
            float maxArenaRadiusSqr, float centerReturnRadius, float minTargetDistance, float maxTargetDistance)
        {
            MaxRepathInterval = maxRepathInterval;
            ArrivalThreshold = arrivalThreshold;
            MaxArenaRadiusSqr = maxArenaRadiusSqr;
            CenterReturnRadius = centerReturnRadius;
            MinTargetDistance = minTargetDistance;
            MaxTargetDistance = maxTargetDistance;
        }
    }
}