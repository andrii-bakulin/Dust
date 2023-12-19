using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Helpers/Random Transform")]
    public class RandomTransform : DuMonoBehaviour
    {
        public enum ActivateMode
        {
            Awake = 0,
            Start = 1,
        }

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
        private bool m_RotationEnabled = false;
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
        private bool m_ScaleEnabled = false;
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
        private TransformMode m_TransformMode = TransformMode.Set;
        public TransformMode transformMode
        {
            get => m_TransformMode;
            set => m_TransformMode = value;
        }

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set => m_Space = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Seed = 0;
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
        private DuRandom duRandom => m_DuRandom ?? (m_DuRandom = new DuRandom(seed));

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
                Vector3 value = duRandom.Range(positionRangeMin, positionRangeMax);
                Vector3 position = Vector3.zero;

                if (space == Space.World)
                    position = transform.position;
                else if (space == Space.Local)
                    position = transform.localPosition;

                if (transformMode == TransformMode.Add)
                    position += value;
                else if (transformMode == TransformMode.Set)
                    position = value;

                if (space == Space.World)
                    transform.position = position;
                else if (space == Space.Local)
                    transform.localPosition = position;
            }

            if (rotationEnabled)
            {
                Vector3 value = duRandom.Range(rotationRangeMin, rotationRangeMax);
                Vector3 rotation = Vector3.zero;

                if (space == Space.World)
                    rotation = transform.eulerAngles;
                else if (space == Space.Local)
                    rotation = transform.localEulerAngles;

                if (transformMode == TransformMode.Add)
                    rotation += value;
                else if (transformMode == TransformMode.Set)
                    rotation = value;

                if (space == Space.World)
                    transform.eulerAngles = rotation;
                else if (space == Space.Local)
                    transform.localEulerAngles = rotation;
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
                    scale = transform.lossyScale;
                else if (space == Space.Local)
                    scale = transform.localScale;

                if (transformMode == TransformMode.Add)
                    scale += value;
                else if (transformMode == TransformMode.Set)
                    scale = value;

                if (space == Space.World)
                    DuTransform.SetGlobalScale(transform, scale);
                else if (space == Space.Local)
                    transform.localScale = scale;
            }
        }
    }
}
