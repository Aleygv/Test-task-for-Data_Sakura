using UnityEngine;

namespace _Project.Logic.Extensions
{
    public static class FromUnityVector
    {
        public static LightVector3 AsLightVector(this Vector3 vector3) =>
            new LightVector3(vector3.x, vector3.y, vector3.z);

        public static Vector3 AsUnityVector(this LightVector3 vector3) => 
            new Vector3(vector3.x, vector3.y, vector3.z);
    }
}