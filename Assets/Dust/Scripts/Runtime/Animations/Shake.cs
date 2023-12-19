using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Animations/Shake")]
    public class Shake : DuMonoBehaviour
    {
        internal const float k_MinScaleValue = 0.0001f;

        public enum TransformMode
        {
            Relative = 0,
            Absolute = 1,
            AppendToAnimation = 2,
        };

        //--------------------------------------------------------------------------------------------------------------

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
                ResetStates();
            }
        }

        [SerializeField]
        private float m_Power = 1f;
        public float power
        {
            get => m_Power;
            set => m_Power = NormalizePower(value);
        }

        [SerializeField]
        private float m_WarmUpTime = 0f;
        public float warmUpTime
        {
            get => m_WarmUpTime;
            set => m_WarmUpTime = NormalizeWarmUpTime(value);
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
        private Vector3 m_PositionAmplitude = Vector3.one;
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

        [SerializeField]
        private bool m_ScaleUniform = false;
        public bool scaleUniform
        {
            get => m_ScaleUniform;
            set => m_ScaleUniform = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

        private DuNoise m_DuNoise;
        private DuNoise duNoise => m_DuNoise ??= new DuNoise(seed);

        private float m_TimeOffset = 0f;

        private float m_PositionTimeOffset;
        private float m_RotationTimeOffset;
        private float m_ScaleTimeOffset;

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            if (updateMode != UpdateMode.Update)
                return;

            UpdateState(Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (updateMode != UpdateMode.LateUpdate)
                return;

            UpdateState(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (updateMode != UpdateMode.FixedUpdate)
                return;

            UpdateState(Time.fixedDeltaTime);
        }

        private void UpdateState(float deltaTime)
        {
            m_TimeOffset += deltaTime;

            float warmUpOffset = warmUpTime > 0f ? m_TimeOffset / warmUpTime : 1f;

            if (positionEnabled)
            {
                Vector3 deltaPosition = Vector3.zero;

                if (!freeze)
                    m_PositionTimeOffset += positionSpeed * deltaTime;

                if (power > 0f)
                {
                    deltaPosition = positionAmplitude * power;
                    deltaPosition.Scale(duNoise.Perlin1D_asWideVector3(0f, m_PositionTimeOffset));
                }

                if (warmUpOffset < 1f)
                    deltaPosition = Vector3.Lerp(Vector3.zero, deltaPosition, warmUpOffset);

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
                    deltaRotation.Scale(duNoise.Perlin1D_asWideVector3(0f, m_RotationTimeOffset));
                }

                if (warmUpOffset < 1f)
                    deltaRotation = Vector3.Lerp(Vector3.zero, deltaRotation, warmUpOffset);

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
                    Vector3 noiseValue;

                    if (scaleUniform)
                    {
                        noiseValue.x = noiseValue.y = noiseValue.z = duNoise.Perlin1D_asWide(0f, m_ScaleTimeOffset);
                    }
                    else
                    {
                        noiseValue = duNoise.Perlin1D_asWideVector3(0f, m_ScaleTimeOffset);
                    }

                    multScale.x = CalcScaleValue(scaleAmplitude.x, noiseValue.x);
                    multScale.y = CalcScaleValue(scaleAmplitude.y, noiseValue.y);
                    multScale.z = CalcScaleValue(scaleAmplitude.z, noiseValue.z);

                    multScale = Vector3.Lerp(Vector3.one, multScale, power);
                }

                if (warmUpOffset < 1f)
                    multScale = Vector3.Lerp(Vector3.one, multScale, warmUpOffset);

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

        private float CalcScaleValue(float amplitude, float offset)
        {
            if (amplitude < k_MinScaleValue)
                amplitude = k_MinScaleValue;

            if (offset >= 0f)
                return Mathf.Lerp(1f, amplitude, offset);

            return Mathf.Lerp(1f, 1f / amplitude, -offset);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Reset()
        {
            ResetStates();
        }

        public void ResetStates()
        {
            m_DuNoise = null;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizePower(float value)
        {
            return Mathf.Clamp01(value);
        }

        public static float NormalizeWarmUpTime(float value)
        {
            return Mathf.Max(value, 0f);
        }

        public static Vector3 NormalizeScaleAmplitude(Vector3 value)
        {
            return Vector3.Max(value, DuVector3.New(k_MinScaleValue));
        }
    }
}
