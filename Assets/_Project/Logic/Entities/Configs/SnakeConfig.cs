namespace _Project.Logic.Entities.Configs
{
    public class SnakeConfig : AnimalConfig
    {
        public float MaxRepathInterval { get; }
        public float ArrivalThreshold { get; }
        public float MaxArenaRadiusSqr { get; }
        public float CenterReturnRadius { get; }
        public float MinTargetDistance { get; }
        public float MaxTargetDistance { get; }
        
        public SnakeConfig(AnimalTypeId animalTypeId, AnimalRole role, string prefabPath,
            float maxRepathInterval, float arrivalThreshold, float maxArenaRadiusSqr, 
            float centerReturnRadius, float minTargetDistance, float maxTargetDistance)
            : base(animalTypeId, role, prefabPath)
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