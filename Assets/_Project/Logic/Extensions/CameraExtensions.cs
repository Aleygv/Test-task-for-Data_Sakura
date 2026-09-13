using UnityEngine;

namespace _Project.Logic.Extensions
{
    public static class CameraExtensions
    {
        private const float DefaultGroundY = 0f;
        private const float DefaultPadding = 0.1f;

        public static Vector3 GetRandomPositionInCameraView(this Camera viewCamera, float groundY = DefaultGroundY, float padding = DefaultPadding)
        {
            float randomX = Random.Range(padding, 1f - padding);
            float randomY = Random.Range(padding, 1f - padding);

            Ray ray = viewCamera.ViewportPointToRay(new Vector3(randomX, randomY, 0f));

            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, groundY, 0));

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 position = ray.GetPoint(enter);
                return position;
            }

            return Vector3.zero;
        }
        
        public static Vector3 GetRandomPositionAround(this Vector3 origin, float minRadius, float maxRadius)
        {
            Vector2 circle = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
            return new Vector3(origin.x + circle.x, origin.y, origin.z + circle.y);
        }
    }
}