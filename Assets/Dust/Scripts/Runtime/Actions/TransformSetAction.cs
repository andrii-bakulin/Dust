using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Transform Set Action")]
    public class TransformSetAction : InstantAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_PositionEnabled = false;
        public bool positionEnabled
        {
            get => m_PositionEnabled;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_PositionEnabled = value;
            }
        }

        [SerializeField]
        private Vector3 m_Position = Vector3.zero;
        public Vector3 position
        {
            get => m_Position;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Position = value;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_RotationEnabled = false;
        public bool rotationEnabled
        {
            get => m_RotationEnabled;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_RotationEnabled = value;
            }
        }

        [SerializeField]
        private Vector3 m_Rotation = Vector3.zero;
        public Vector3 rotation
        {
            get => m_Rotation;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Rotation = value;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ScaleEnabled = false;
        public bool scaleEnabled
        {
            get => m_ScaleEnabled;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ScaleEnabled = value;
            }
        }

        [SerializeField]
        private Vector3 m_Scale = Vector3.one;
        public Vector3 scale
        {
            get => m_Scale;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Scale = value;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Space = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(activeTargetTransform))
                return;

            if (space == Space.World)
            {
                if (positionEnabled)
                    activeTargetTransform.position = position;

                if (rotationEnabled)
                    activeTargetTransform.rotation = Quaternion.Euler(rotation);

                if (scaleEnabled)
                    DuTransform.SetGlobalScale(activeTargetTransform, scale);
            }
            else if (space == Space.Local)
            {
                if (positionEnabled)
                    activeTargetTransform.localPosition = position;

                if (rotationEnabled)
                    activeTargetTransform.localRotation = Quaternion.Euler(rotation);

                if (scaleEnabled)
                    activeTargetTransform.localScale = scale;
            }
        }
    }
}
