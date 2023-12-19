using UnityEngine;

namespace DustEngine
{
    public static class DuTransform
    {
        public static void SetGlobalScale(Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;

            Vector3 curScale = transform.lossyScale;

            transform.localScale = new Vector3(
                DuMath.IsNotZero(curScale.x) ? globalScale.x / curScale.x : 0f, 
                DuMath.IsNotZero(curScale.y) ? globalScale.y / curScale.y : 0f, 
                DuMath.IsNotZero(curScale.z) ? globalScale.z / curScale.z : 0f);
        }

        public static void Reset(Transform tr)
        {
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
            tr.localScale = Vector3.one;
        }
    }
}
