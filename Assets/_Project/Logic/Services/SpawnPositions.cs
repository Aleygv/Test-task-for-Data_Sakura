using System.Collections.Generic;
using _Project.Logic.Extensions;
using UnityEngine;

namespace _Project.Logic.Services
{
    public static class SpawnPositions
    {
        private static LightVector3 UpPosition = new LightVector3(-7.13f, 0.5f, 1.59f);
        private static LightVector3 DownPosition = new LightVector3(8.27f, 0.5f, 1.43f);
        private static LightVector3 LeftPosition = new LightVector3(0.5f, 0.5f, -7f);
        private static LightVector3 RightPosition = new LightVector3(0.7f, 0.5f, 10f);

        public static List<LightVector3> Positions = new() { UpPosition, DownPosition, LeftPosition, RightPosition };
        
        public static LightVector3 GetRandomPosition()
        {
            return SpawnPositions.Positions[Random.Range(0, Positions.Count)];
        }
    }
}