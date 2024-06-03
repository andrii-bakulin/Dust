using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Helpers/Clamp Scale")]
    [ExecuteAlways]
    public class ClampScale : ClampTransform
    {
        private void LateUpdate()
        {
            if (IsClampsNotEnabled())
                return;

            Vector3 scale = transform.localScale;

            scale.x = ClampValue(scale.x, clampModeX, clampMinX, clampMaxX);
            scale.y = ClampValue(scale.y, clampModeY, clampMinY, clampMaxY);
            scale.z = ClampValue(scale.z, clampModeZ, clampMinZ, clampMaxZ);

            transform.localScale = scale;
        }

        private void Reset()
        {
            clampMinX = clampMaxX = +1f;
            clampMinY = clampMaxY = +1f;
            clampMinZ = clampMaxZ = +1f;
        }
    }
}
