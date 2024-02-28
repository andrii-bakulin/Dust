using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Transform Update Action")]
    public class TransformUpdateAction : InstantAction
    {
        [SerializeField]
        private bool m_PositionEnabled;
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
        private bool m_RotationEnabled;
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
        private bool m_ScaleEnabled;
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
        private DuTransform.Space m_Space = DuTransform.Space.Local;
        public DuTransform.Space space
        {
            get => m_Space;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Space = value;
            }
        }

        [SerializeField]
        private DuTransform.Mode m_TransformMode = DuTransform.Mode.Set;
        public DuTransform.Mode transformMode
        {
            get => m_TransformMode;
            set => m_TransformMode = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionExecute()
        {
            if (Dust.IsNull(activeTargetTransform))
                return;

            if (positionEnabled)
            {
                var value = position;
                DuTransform.UpdatePosition(activeTargetTransform, value, space, transformMode);
            }

            if (rotationEnabled)
            {
                var value = rotation;
                DuTransform.UpdateRotation(activeTargetTransform, value, space, transformMode);
            }

            if (scaleEnabled)
            {
                var value = scale;
                DuTransform.UpdateScale(activeTargetTransform, value, space, transformMode);
            }
        }
    }
}
