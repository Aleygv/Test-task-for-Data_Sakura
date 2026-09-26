using System.Collections.Generic;
using _Project.Logic.Entities;
using _Project.Logic.Entities.Configs.Animal;
using _Project.Logic.Entities.Configs.Movement;

namespace _Project.Logic.Infrastructure
{
    public static class GameConfigs
    {
        public static readonly JumpMovementConfig JumpMovementConfig = new(
            stepDistance: 2.5f,
            jumpInterval: 1.5f,
            minInitialTimerOffset: 0.2f,
            maxDistanceFromCenterSqr: 100.0f,
            maxTurnAngle: 25.0f);

        public static readonly LinearMovementConfig LinearMovementConfig = new(
            maxRepathInterval: 4.0f,
            arrivalThreshold: 1.2f,
            maxArenaRadiusSqr: 80.0f,
            centerReturnRadius: 4.0f,
            minTargetDistance: 4.0f,
            maxTargetDistance: 8.0f);

        public static readonly AnimalConfig Frog = new FrogConfig(
            AnimalTypeId.Frog,
            AnimalRole.Prey,
            "Prefabs/Animals/Frog.prefab",
            JumpMovementConfig);

        public static readonly AnimalConfig Snake = new SnakeConfig(
            AnimalTypeId.Snake,
            AnimalRole.Predator,
            "Prefabs/Animals/Snake.prefab",
            LinearMovementConfig);

        public static readonly List<AnimalConfig> AnimalConfigs = new()
        {
            Frog,
            Snake
        };
    }
}