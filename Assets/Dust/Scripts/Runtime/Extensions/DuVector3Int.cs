using UnityEngine;

namespace DustEngine
{
    public static class DuVector3Int
    {
        public static Vector3Int Clamp(Vector3Int value, Vector3Int min, Vector3Int max)
        {
            value.x = Mathf.Clamp(value.x, min.x, max.x);
            value.y = Mathf.Clamp(value.y, min.y, max.y);
            value.z = Mathf.Clamp(value.z, min.z, max.z);
            return value;
        }
    }
}
