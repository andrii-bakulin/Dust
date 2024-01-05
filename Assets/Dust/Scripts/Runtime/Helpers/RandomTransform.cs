using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Helpers/Random Transform")]
    public class RandomTransform : DuMonoBehaviour
    {
        public enum ActivateMode
        {
            Awake = 0,
            Start = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_PositionEnabled;
        public bool positionEnabled
        {
            get => m_PositionEnabled;
            set => m_PositionEnabled = value;
        }

        [SerializeField]
        private Vector3 m_PositionRangeMin = DuVector3.New(-1f);
        public Vector3 positionRangeMin
        {
            get => m_PositionRangeMin;
            set => m_PositionRangeMin = value;
        }

        [SerializeField]
        private Vector3 m_PositionRangeMax = DuVector3.New(+1f);
        public Vector3 positionRangeMax
        {
            get => m_PositionRangeMax;
            set => m_PositionRangeMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_RotationEnabled;
        public bool rotationEnabled
        {
            get => m_RotationEnabled;
            set => m_RotationEnabled = value;
        }

        [SerializeField]
        private Vector3 m_RotationRangeMin = Vector3.up * -180f;
        public Vector3 rotationRangeMin
        {
            get => m_RotationRangeMin;
            set => m_RotationRangeMin = value;
        }

        [SerializeField]
        private Vector3 m_RotationRangeMax = Vector3.up * +180f;
        public Vector3 rotationRangeMax
        {
            get => m_RotationRangeMax;
            set => m_RotationRangeMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ScaleEnabled;
        public bool scaleEnabled
        {
            get => m_ScaleEnabled;
            set => m_ScaleEnabled = value;
        }

        [SerializeField]
        private Vector3 m_ScaleRangeMin = DuVector3.New(-0.5f);
        public Vector3 scaleRangeMin
        {
            get => m_ScaleRangeMin;
            set => m_ScaleRangeMin = value;
        }

        [SerializeField]
        private Vector3 m_ScaleRangeMax = DuVector3.New(+1.0f);
        public Vector3 scaleRangeMax
        {
            get => m_ScaleRangeMax;
            set => m_ScaleRangeMax = value;
        }

        [SerializeField] 
        private bool m_ScaleUniform = true;
        public bool scaleUniform
        {
            get => m_ScaleUniform;
            set => m_ScaleUniform = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private ActivateMode m_ActivateMode = ActivateMode.Awake;
        public ActivateMode activateMode
        {
            get => m_ActivateMode;
            set => m_ActivateMode = value;
        }

        [SerializeField]
        private DuTransform.Mode m_TransformMode = DuTransform.Mode.Set;
        public DuTransform.Mode transformMode
        {
            get => m_TransformMode;
            set => m_TransformMode = value;
        }

        [SerializeField]
        private DuTransform.Space m_Space = DuTransform.Space.Local;
        public DuTransform.Space space
        {
            get => m_Space;
            set => m_Space = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Seed;
        public int seed
        {
            get => m_Seed;
            set
            {
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

        private void Awake()
        {
            if (activateMode == ActivateMode.Awake)
                ApplyRandomState();
        }

        private void Start()
        {
            if (activateMode == ActivateMode.Start)
                ApplyRandomState();
        }

        private void ApplyRandomState()
        {
            if (positionEnabled)
            {
                var value = duRandom.Range(positionRangeMin, positionRangeMax);
                DuTransform.UpdatePosition(transform, value, space, transformMode);
            }

            if (rotationEnabled)
            {
                var value = duRandom.Range(rotationRangeMin, rotationRangeMax);
                DuTransform.UpdateRotation(transform, value, space, transformMode);
            }

            if (scaleEnabled)
            {
                var value = scaleUniform
                    ? Vector3.Lerp(scaleRangeMin, scaleRangeMax, duRandom.Next())
                    : duRandom.Range(scaleRangeMin, scaleRangeMax);
                DuTransform.UpdateScale(transform, value, space, transformMode);
            }
        }
    }
}
