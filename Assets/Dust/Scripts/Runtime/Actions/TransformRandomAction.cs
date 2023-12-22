using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Transform Random Action")]
    public class TransformRandomAction : InstantAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        public enum TransformMode
        {
            Add = 0,
            Set = 1,
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
        private TransformMode m_TransformMode = TransformMode.Set;
        public TransformMode transformMode
        {
            get => m_TransformMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TransformMode = value;
            }
        }

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

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Seed = 0;
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

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(activeTargetTransform))
                return;

            if (positionEnabled)
            {
                Vector3 value = duRandom.Range(positionRangeMin, positionRangeMax);
                Vector3 position = Vector3.zero;

                if (space == Space.World)
                    position = activeTargetTransform.position;
                else if (space == Space.Local)
                    position = activeTargetTransform.localPosition;

                if (transformMode == TransformMode.Add)
                    position += value;
                else if (transformMode == TransformMode.Set)
                    position = value;

                if (space == Space.World)
                    activeTargetTransform.position = position;
                else if (space == Space.Local)
                    activeTargetTransform.localPosition = position;
            }

            if (rotationEnabled)
            {
                Vector3 value = duRandom.Range(rotationRangeMin, rotationRangeMax);
                Vector3 rotation = Vector3.zero;

                if (space == Space.World)
                    rotation = activeTargetTransform.eulerAngles;
                else if (space == Space.Local)
                    rotation = activeTargetTransform.localEulerAngles;

                if (transformMode == TransformMode.Add)
                    rotation += value;
                else if (transformMode == TransformMode.Set)
                    rotation = value;

                if (space == Space.World)
                    activeTargetTransform.eulerAngles = rotation;
                else if (space == Space.Local)
                    activeTargetTransform.localEulerAngles = rotation;
            }

            if (scaleEnabled)
            {
                Vector3 value;
                
                if (scaleUniform)
                    value = Vector3.Lerp(scaleRangeMin, scaleRangeMax, duRandom.Next());
                else
                    value = duRandom.Range(scaleRangeMin, scaleRangeMax);

                Vector3 scale = Vector3.one;

                if (space == Space.World)
                    scale = activeTargetTransform.lossyScale;
                else if (space == Space.Local)
                    scale = activeTargetTransform.localScale;

                if (transformMode == TransformMode.Add)
                    scale += value;
                else if (transformMode == TransformMode.Set)
                    scale = value;

                if (space == Space.World)
                    DuTransform.SetGlobalScale(activeTargetTransform, scale);
                else if (space == Space.Local)
                    activeTargetTransform.localScale = scale;
            }
        }
    }
}
