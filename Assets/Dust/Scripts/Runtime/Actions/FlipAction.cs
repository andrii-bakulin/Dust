using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Flip Action")]
    public class FlipAction : InstantAction
    {
        [SerializeField]
        private bool m_FlipX;
        public bool flipX
        {
            get => m_FlipX;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_FlipX = value;
            }
        }

        [SerializeField]
        private bool m_FlipY;
        public bool flipY
        {
            get => m_FlipY;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_FlipY = value;
            }
        }

        [SerializeField]
        private bool m_FlipZ;
        public bool flipZ
        {
            get => m_FlipZ;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_FlipZ = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionExecute()
        {
            if (Dust.IsNull(activeTargetTransform))
                return;

            Vector3 scale = activeTargetTransform.localScale;

            if (flipX) scale.x *= -1f;
            if (flipY) scale.y *= -1f;
            if (flipZ) scale.z *= -1f;

            activeTargetTransform.localScale = scale;
        }
    }
}
