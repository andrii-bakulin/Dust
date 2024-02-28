using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Transform Random Action")]
    public class TransformRandomAction : InstantAction
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
        private Vector3 m_PositionRangeMin = DuVector3.New(-1f);
        public Vector3 positionRangeMin
        {
            get => m_PositionRangeMin;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_PositionRangeMin = value;
            }
        }

        [SerializeField]
        private Vector3 m_PositionRangeMax = DuVector3.New(+1f);
        public Vector3 positionRangeMax
        {
            get => m_PositionRangeMax;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_PositionRangeMax = value;
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
        private Vector3 m_RotationRangeMin = Vector3.up * -180f;
        public Vector3 rotationRangeMin
        {
            get => m_RotationRangeMin;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_RotationRangeMin = value;
            }
        }

        [SerializeField]
        private Vector3 m_RotationRangeMax = Vector3.up * +180f;
        public Vector3 rotationRangeMax
        {
            get => m_RotationRangeMax;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_RotationRangeMax = value;
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
        private Vector3 m_ScaleRangeMin = DuVector3.New(-0.5f);
        public Vector3 scaleRangeMin
        {
            get => m_ScaleRangeMin;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ScaleRangeMin = value;
            }
        }

        [SerializeField]
        private Vector3 m_ScaleRangeMax = DuVector3.New(+1.0f);
        public Vector3 scaleRangeMax
        {
            get => m_ScaleRangeMax;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ScaleRangeMax = value;
            }
        }

        [SerializeField] 
        private bool m_ScaleUniform = true;
        public bool scaleUniform
        {
            get => m_ScaleUniform;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ScaleUniform = value;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private DuTransform.Mode m_TransformMode = DuTransform.Mode.Set;
        public DuTransform.Mode transformMode
        {
            get => m_TransformMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TransformMode = value;
            }
        }

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

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Seed;
        public int seed
        {
            get => m_Seed;
            set
            {
                if (!IsAllowUpdateProperty()) return;

                if (m_Seed == value)
                    return;

                m_Seed = value;
                m_DuRandom = null;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuRandom m_DuRandom;
        private DuRandom duRandom => m_DuRandom ??= new DuRandom(seed);

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionExecute()
        {
            if (Dust.IsNull(activeTargetTransform))
                return;

            if (positionEnabled)
            {
                var value = duRandom.Range(positionRangeMin, positionRangeMax);
                DuTransform.UpdatePosition(activeTargetTransform, value, space, transformMode);
            }

            if (rotationEnabled)
            {
                var value = duRandom.Range(rotationRangeMin, rotationRangeMax);
                DuTransform.UpdateRotation(activeTargetTransform, value, space, transformMode);
            }

            if (scaleEnabled)
            {
                var value = scaleUniform
                    ? Vector3.Lerp(scaleRangeMin, scaleRangeMax, duRandom.Next())
                    : duRandom.Range(scaleRangeMin, scaleRangeMax);
                DuTransform.UpdateScale(activeTargetTransform, value, space, transformMode);
            }
        }
    }
}
