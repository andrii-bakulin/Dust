using UnityEngine;

namespace Dust
{
    public abstract class ClampTransform : MonoBehaviour
    {
        [SerializeField]
        protected ClampMode m_ClampModeX;
        public ClampMode clampModeX
        {
            get => m_ClampModeX;
            set => m_ClampModeX = value;
        }

        [SerializeField]
        protected float m_ClampMinX;
        public float clampMinX
        {
            get => m_ClampMinX;
            set => m_ClampMinX = value;
        }

        [SerializeField]
        protected float m_ClampMaxX;
        public float clampMaxX
        {
            get => m_ClampMaxX;
            set => m_ClampMaxX = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected ClampMode m_ClampModeY;
        public ClampMode clampModeY
        {
            get => m_ClampModeY;
            set => m_ClampModeY = value;
        }

        [SerializeField]
        protected float m_ClampMinY;
        public float clampMinY
        {
            get => m_ClampMinY;
            set => m_ClampMinY = value;
        }

        [SerializeField]
        protected float m_ClampMaxY;
        public float clampMaxY
        {
            get => m_ClampMaxY;
            set => m_ClampMaxY = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected ClampMode m_ClampModeZ;
        public ClampMode clampModeZ
        {
            get => m_ClampModeZ;
            set => m_ClampModeZ = value;
        }

        [SerializeField]
        protected float m_ClampMinZ;
        public float clampMinZ
        {
            get => m_ClampMinZ;
            set => m_ClampMinZ = value;
        }

        [SerializeField]
        protected float m_ClampMaxZ;
        public float clampMaxZ
        {
            get => m_ClampMaxZ;
            set => m_ClampMaxZ = value;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        protected bool IsClampsNotEnabled()
        {
            return clampModeX == ClampMode.NoClamp && 
                   clampModeY == ClampMode.NoClamp &&
                   clampModeZ == ClampMode.NoClamp;
        }
            
        protected static float ClampValue(float value, ClampMode mode, float min, float max)
        {
            if (mode == ClampMode.MinOnly)
                value = Mathf.Max(value, min);

            if (mode == ClampMode.MaxOnly)
                value = Mathf.Min(value, max);

            if (mode == ClampMode.MinAndMax)
            {
                value = Mathf.Max(value, Mathf.Min(min, max));
                value = Mathf.Min(value, Mathf.Max(min, max));
            }

            return value;
        }
    }
}
