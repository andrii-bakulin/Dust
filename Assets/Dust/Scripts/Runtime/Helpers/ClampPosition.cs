using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Helpers/Clamp Position")]
    [ExecuteAlways]
    public class ClampPosition : ClampTransform
    {
        [SerializeField]
        private DuTransform.Space m_Space = DuTransform.Space.Local;
        public DuTransform.Space space
        {
            get => m_Space;
            set => m_Space = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void LateUpdate()
        {
            if (IsClampsNotEnabled())
                return;

            Vector3 position = space == DuTransform.Space.World 
                ? transform.position
                : transform.localPosition;

            position.x = ClampValue(position.x, clampModeX, clampMinX, clampMaxX);
            position.y = ClampValue(position.y, clampModeY, clampMinY, clampMaxY);
            position.z = ClampValue(position.z, clampModeZ, clampMinZ, clampMaxZ);

            if (space == DuTransform.Space.World)
                transform.position = position;
            else
                transform.localPosition = position;
        }
    }
}
