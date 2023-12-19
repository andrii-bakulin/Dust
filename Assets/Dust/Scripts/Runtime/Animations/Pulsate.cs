using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Animations/Pulsate")]
    public class Pulsate : DuMonoBehaviour
    {
        internal const float k_MinScaleValue = 0.0001f;

        public enum EaseMode
        {
            Linear = 0,
            EaseInOut = 1,
        };

        public enum TransformMode
        {
            Relative = 0,
            Absolute = 1,
            AppendToAnimation = 2,
        };

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private float m_Power = 1f;
        public float power
        {
            get => m_Power;
            set => m_Power = NormalizePower(value);
        }

        [SerializeField]
        private float m_SleepTime = 0f;
        public float sleepTime
        {
            get => m_SleepTime;
            set => m_SleepTime = NormalizeSleepTime(value);
        }

        [SerializeField]
        private bool m_Freeze = false;
        public bool freeze
        {
            get => m_Freeze;
            set => m_Freeze = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_PositionEnabled = false;
        public bool positionEnabled
        {
            get => m_PositionEnabled;
            set => m_PositionEnabled = value;
        }

        [SerializeField]
        private Vector3 m_PositionAmplitude = Vector3.up;
        public Vector3 positionAmplitude
        {
            get => m_PositionAmplitude;
            set => m_PositionAmplitude = value;
        }

        [SerializeField]
        private float m_PositionSpeed = 1f;
        public float positionSpeed
        {
            get => m_PositionSpeed;
            set => m_PositionSpeed = value;
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
        private Vector3 m_RotationAmplitude = Vector3.up * 90f;
        public Vector3 rotationAmplitude
        {
            get => m_RotationAmplitude;
            set => m_RotationAmplitude = value;
        }

        [SerializeField]
        private float m_RotationSpeed = 1f;
        public float rotationSpeed
        {
            get => m_RotationSpeed;
            set => m_RotationSpeed = value;
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
        private Vector3 m_ScaleAmplitude = DuVector3.New(2f);
        public Vector3 scaleAmplitude
        {
            get => m_ScaleAmplitude;
            set => m_ScaleAmplitude = NormalizeScaleAmplitude(value);
        }

        [SerializeField]
        private float m_ScaleSpeed = 1f;
        public float scaleSpeed
        {
            get => m_ScaleSpeed;
            set => m_ScaleSpeed = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private EaseMode m_EaseMode = EaseMode.EaseInOut;
        public EaseMode easeMode
        {
            get => m_EaseMode;
            set => m_EaseMode = value;
        }

        [SerializeField]
        private TransformMode m_TransformMode = TransformMode.Relative;
        public TransformMode transformMode
        {
            get => m_TransformMode;
            set => m_TransformMode = value;
        }

        [SerializeField]
        private UpdateMode m_UpdateMode = UpdateMode.Update;
        public UpdateMode updateMode
        {
            get => m_UpdateMode;
            set => m_UpdateMode = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private Vector3 m_LastDeltaPosition = Vector3.zero;
        private Vector3 m_LastDeltaRotation = Vector3.zero;
        private Vector3 m_LastMultScale = Vector3.one;

        public Vector3 lastDeltaPosition => m_LastDeltaPosition;
        public Vector3 lastDeltaRotation => m_LastDeltaRotation;
        public Vector3 lastMultScale => m_LastMultScale;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private float m_PositionTimeOffset;
        private float m_RotationTimeOffset;
        private float m_ScaleTimeOffset;

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            if (updateMode != UpdateMode.Update)
                return;

            UpdateState(Time.deltaTime);
        }

        void LateUpdate()
        {
            if (updateMode != UpdateMode.LateUpdate)
                return;

            UpdateState(Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (updateMode != UpdateMode.FixedUpdate)
                return;

            UpdateState(Time.fixedDeltaTime);
        }

        void UpdateState(float deltaTime)
        {
            if (sleepTime > 0f)
            {
                sleepTime = Mathf.Max(sleepTime - deltaTime, 0f);
                return;
            }

            if (positionEnabled)
            {
                Vector3 deltaPosition = Vector3.zero;

                if (!freeze)
                    m_PositionTimeOffset += positionSpeed * deltaTime;

                if (power > 0f)
                {
                    deltaPosition = positionAmplitude * power;
                    deltaPosition *= CalcOffsetByEaseMode(m_PositionTimeOffset);
                }

                switch (transformMode)
                {
                    default:
                    case TransformMode.Relative:
                        transform.localPosition += deltaPosition - m_LastDeltaPosition;
                        break;

                    case TransformMode.Absolute:
                        transform.localPosition = deltaPosition;
                        break;

                    case TransformMode.AppendToAnimation:
                        transform.localPosition += deltaPosition;
                        break;
                }

                m_LastDeltaPosition = deltaPosition;
            }

            if (rotationEnabled)
            {
                Vector3 deltaRotation = Vector3.zero;

                if (!freeze)
                    m_RotationTimeOffset += rotationSpeed * deltaTime;

                if (power > 0f)
                {
                    deltaRotation = rotationAmplitude * power;
                    deltaRotation *= CalcOffsetByEaseMode(m_RotationTimeOffset);
                }

                switch (transformMode)
                {
                    default:
                    case TransformMode.Relative:
                        transform.localEulerAngles += deltaRotation - m_LastDeltaRotation;
                        break;

                    case TransformMode.Absolute:
                        transform.localEulerAngles = deltaRotation;
                        break;

                    case TransformMode.AppendToAnimation:
                        transform.localEulerAngles += deltaRotation;
                        break;
                }

                m_LastDeltaRotation = deltaRotation;
            }

            if (scaleEnabled)
            {
                Vector3 multScale = Vector3.one;

                if (!freeze)
                    m_ScaleTimeOffset += scaleSpeed * deltaTime;

                if (power > 0f)
                {
                    float noiseValue1 = CalcOffsetByEaseMode(m_ScaleTimeOffset);

                    Vector3 noiseValue = DuVector3.New(noiseValue1);

                    multScale.x = CalcScaleValue(scaleAmplitude.x, noiseValue.x);
                    multScale.y = CalcScaleValue(scaleAmplitude.y, noiseValue.y);
                    multScale.z = CalcScaleValue(scaleAmplitude.z, noiseValue.z);

                    multScale = Vector3.Lerp(Vector3.one, multScale, power);
                }

                switch (transformMode)
                {
                    default:
                    case TransformMode.Relative:
                    {
                        Vector3 scale = transform.localScale;
                        scale.duInverseScale(m_LastMultScale);
                        scale.Scale(multScale);
                        transform.localScale = scale;
                        break;
                    }

                    case TransformMode.Absolute:
                        transform.localScale = multScale;
                        break;

                    case TransformMode.AppendToAnimation:
                    {
                        Vector3 scale = transform.localScale;
                        scale.Scale(multScale);
                        transform.localScale = scale;
                        break;
                    }
                }

                m_LastMultScale = multScale;
            }
        }

        private float CalcOffsetByEaseMode(float timeOffset)
        {
            if (easeMode == EaseMode.EaseInOut)
            {
                return Mathf.Sin(Constants.PI2 * timeOffset);
            }

            // (easeMode == EaseMode.Linear)
            return Mathf.PingPong(timeOffset * 4f, 2f) - 1f;
        }

        private float CalcScaleValue(float amplitude, float offset)
        {
            if (amplitude < k_MinScaleValue)
                amplitude = k_MinScaleValue;

            if (offset >= 0f)
                return Mathf.Lerp(1f, amplitude, offset);

            return Mathf.Lerp(1f, 1f / amplitude, -offset);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizePower(float value)
        {
            return Mathf.Clamp01(value);
        }

        public static float NormalizeSleepTime(float value)
        {
            return Mathf.Max(value, 0f);
        }

        public static Vector3 NormalizeScaleAmplitude(Vector3 value)
        {
            value = Vector3.Max(value, DuVector3.New(k_MinScaleValue));
            return value;
        }
    }
}
