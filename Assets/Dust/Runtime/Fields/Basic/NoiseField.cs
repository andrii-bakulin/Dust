using System;
using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Fields/Basic Fields/Noise Field")]
    public class NoiseField : SpaceField
    {
        public enum NoiseMode
        {
            Random = 0,
            Perlin = 1,
        }

        public enum NoiseSpace
        {
            Global = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private NoiseMode m_NoiseMode = NoiseMode.Random;
        public NoiseMode noiseMode
        {
            get => m_NoiseMode;
            set => m_NoiseMode = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_AnimationSpeed;
        public float animationSpeed
        {
            get => m_AnimationSpeed;
            set => m_AnimationSpeed = value;
        }

        [SerializeField]
        private float m_AnimationOffset;
        public float animationOffset
        {
            get => m_AnimationOffset;
            set => m_AnimationOffset = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private NoiseSpace m_NoiseSpace;
        public NoiseSpace noiseSpace
        {
            get => m_NoiseSpace;
            set => m_NoiseSpace = value;
        }

        [SerializeField]
        private float m_NoiseScale = 1f;
        public float noiseScale
        {
            get => m_NoiseScale;
            set => m_NoiseScale = NormalizeNoiseScale(value);
        }

        [SerializeField]
        private float m_NoisePower = 1f;
        public float noisePower
        {
            get => m_NoisePower;
            set => m_NoisePower = NormalizeNoisePower(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_IgnoreAxisX;
        public bool ignoreAxisX
        {
            get => m_IgnoreAxisX;
            set => m_IgnoreAxisX = value;
        }

        [SerializeField]
        private bool m_IgnoreAxisY;
        public bool ignoreAxisY
        {
            get => m_IgnoreAxisY;
            set => m_IgnoreAxisY = value;
        }

        [SerializeField]
        private bool m_IgnoreAxisZ;
        public bool ignoreAxisZ
        {
            get => m_IgnoreAxisZ;
            set => m_IgnoreAxisZ = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Seed = Constants.RANDOM_SEED_DEFAULT;
        public int seed
        {
            get => m_Seed;
            set
            {
                value = NormalizeSeed(value);

                if (m_Seed == value)
                    return;

                m_Seed = value;
                ResetStates();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuNoise m_DuNoise;

        private DuNoise duNoise => m_DuNoise ??= new DuNoise(seed);

        private float m_OffsetDynamic;

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, noiseMode);
            DynamicState.Append(ref dynamicState, ++seq, seed);

            switch (m_NoiseMode)
            {
                case NoiseMode.Random:
                    // Nothing for now
                    break;

                case NoiseMode.Perlin:
                    DynamicState.Append(ref dynamicState, ++seq, noiseSpace);
                    DynamicState.Append(ref dynamicState, ++seq, noiseScale);
                    DynamicState.Append(ref dynamicState, ++seq, noisePower);

                    DynamicState.Append(ref dynamicState, ++seq, animationSpeed);
                    DynamicState.Append(ref dynamicState, ++seq, animationOffset);

                    DynamicState.Append(ref dynamicState, ++seq, ignoreAxisX);
                    DynamicState.Append(ref dynamicState, ++seq, ignoreAxisY);
                    DynamicState.Append(ref dynamicState, ++seq, ignoreAxisZ);

                    DynamicState.Append(ref dynamicState, ++seq, m_OffsetDynamic);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            m_OffsetDynamic += Time.deltaTime * animationSpeed;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Noise";
        }

        public override string FieldDynamicHint()
        {
            switch (noiseMode)
            {
                case NoiseMode.Random:
                    return "Random";

                case NoiseMode.Perlin:
                    return "Perlin";

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            result.power = 0f;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            switch (noiseMode)
            {
                case NoiseMode.Random:
                    result.power = duNoise.Perlin1D(fieldPoint.inOffset * 1328.8767f, fieldPoint.inOffset * 2984.7353f, 2f);
                    break;

                case NoiseMode.Perlin:
                    Vector3 inSpaceOffset = fieldPoint.inPosition; // point in world space

                    if (noiseSpace == NoiseSpace.Local)
                        inSpaceOffset = transform.InverseTransformPoint(inSpaceOffset);

                    if (ignoreAxisX) inSpaceOffset.x = 0f;
                    if (ignoreAxisY) inSpaceOffset.y = 0f;
                    if (ignoreAxisZ) inSpaceOffset.z = 0f;

                    float animTotalOffset = m_OffsetDynamic + animationOffset;

                    if (DuMath.IsNotZero(noiseScale))
                        inSpaceOffset /= noiseScale;

                    result.power = duNoise.Perlin3D(inSpaceOffset, animTotalOffset, noisePower);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            result.power *= power;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            result.power = remapping.MapValue(result.power);
            result.color = calculateColor ? coloring.GetColor(result.power) : Color.clear;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            // Nothing for now...
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public void ResetStates()
        {
            m_DuNoise = null;
        }

        protected override void ResetDefaults()
        {
            m_DuNoise = null;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeNoiseScale(float value)
        {
            return Mathf.Clamp(value, 0.0001f, float.MaxValue);
        }

        public static float NormalizeNoisePower(float value)
        {
            return Mathf.Clamp(value, 0.0001f, float.MaxValue);
        }

        public static int NormalizeSeed(int value)
        {
            return DuRandom.NormalizeSeedToNonRandom(value);
        }
    }
}
