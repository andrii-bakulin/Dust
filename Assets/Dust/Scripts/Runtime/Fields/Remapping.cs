using System;
using UnityEngine;

namespace Dust
{
    [System.Serializable]
    public class Remapping : IDynamicState
    {
        public enum PostReshapeMode
        {
            None = 0,
            Steps = 1,
            Curve = 2,
        }
        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_RemapPowerEnabled;
        public bool remapPowerEnabled
        {
            get => m_RemapPowerEnabled;
            set => m_RemapPowerEnabled = value;
        }

        [SerializeField]
        private float m_InMin;
        public float inMin
        {
            get => m_InMin;
            set => m_InMin = NormalizeInMinMax(value);
        }

        [SerializeField]
        private float m_InMax = 1.0f;
        public float inMax
        {
            get => m_InMax;
            set => m_InMax = NormalizeInMinMax(value);
        }

        [SerializeField]
        private float m_Strength = 1.0f;
        public float strength
        {
            get => m_Strength;
            set => m_Strength = value;
        }

        [SerializeField]
        private bool m_Invert;
        public bool invert
        {
            get => m_Invert;
            set => m_Invert = value;
        }

        [SerializeField]
        private float m_OutMin;
        public float outMin
        {
            get => m_OutMin;
            set => m_OutMin = value;
        }

        [SerializeField]
        private float m_OutMax = 1.0f;
        public float outMax
        {
            get => m_OutMax;
            set => m_OutMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private ClampMode m_ClampInMode = ClampMode.NoClamp;
        public ClampMode clampInMode
        {
            get => m_ClampInMode;
            set => m_ClampInMode = value;
        }

        [SerializeField]
        private ClampMode m_ClampOutMode = ClampMode.MinAndMax;
        public ClampMode clampOutMode
        {
            get => m_ClampOutMode;
            set => m_ClampOutMode = value;
        }

        [SerializeField]
        private float m_ClampOutMin;
        public float clampOutMin
        {
            get => m_ClampOutMin;
            set => m_ClampOutMin = value;
        }

        [SerializeField]
        private float m_ClampOutMax = 1.0f;
        public float clampOutMax
        {
            get => m_ClampOutMax;
            set => m_ClampOutMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_PostPower = 1.0f;
        public float postPower
        {
            get => m_PostPower;
            set => m_PostPower = value;
        }

        [SerializeField]
        private PostReshapeMode m_PostReshapeMode = PostReshapeMode.None;
        public PostReshapeMode postReshapeMode
        {
            get => m_PostReshapeMode;
            set => m_PostReshapeMode = value;
        }

        [SerializeField]
        private int m_PostStepsCount = 1;
        public int postStepsCount
        {
            get => m_PostStepsCount;
            set => m_PostStepsCount = NormalizePostStepsCount(value);
        }

        [SerializeField]
        private AnimationCurve m_PostCurve = DuAnimationCurve.StraightLine01();
        public AnimationCurve postCurve
        {
            get => m_PostCurve;
            set => m_PostCurve = NormalizePostCurve(value);
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DynamicState.Append(ref dynamicState, ++seq, remapPowerEnabled);

            if (remapPowerEnabled)
            {
                DynamicState.Append(ref dynamicState, ++seq, strength);
                DynamicState.Append(ref dynamicState, ++seq, inMin);
                DynamicState.Append(ref dynamicState, ++seq, inMax);
                DynamicState.Append(ref dynamicState, ++seq, outMin);
                DynamicState.Append(ref dynamicState, ++seq, outMax);

                DynamicState.Append(ref dynamicState, ++seq, clampInMode);
                DynamicState.Append(ref dynamicState, ++seq, clampOutMode);
                {
                    if (clampOutMode == ClampMode.MinOnly || clampOutMode == ClampMode.MinAndMax)
                        DynamicState.Append(ref dynamicState, ++seq, clampOutMin);

                    if (clampOutMode == ClampMode.MaxOnly || clampOutMode == ClampMode.MinAndMax)
                        DynamicState.Append(ref dynamicState, ++seq, clampOutMax);
                }

                DynamicState.Append(ref dynamicState, ++seq, invert);

                DynamicState.Append(ref dynamicState, ++seq, postPower);
                DynamicState.Append(ref dynamicState, ++seq, postReshapeMode);
                DynamicState.Append(ref dynamicState, ++seq, postStepsCount);
                DynamicState.Append(ref dynamicState, ++seq, postCurve);
            }

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public float MapValue(float inValue)
        {
            if (!remapPowerEnabled)
                return inValue;

            //----------------------------------------------------------------------------------------------------------
            // Define <in>

            float _inMin = inMin;
            float _inMax = inMax;

            if (Mathf.Approximately(_inMin, _inMax))
                _inMax = _inMin + 0.0001f;

            if (clampInMode == ClampMode.MinOnly || clampInMode == ClampMode.MinAndMax)
                inValue = Mathf.Max(inValue, _inMin);

            if (clampInMode == ClampMode.MaxOnly || clampInMode == ClampMode.MinAndMax)
                inValue = Mathf.Min(inValue, _inMax);

            //----------------------------------------------------------------------------------------------------------
            // Define <out>

            float _outMin;
            float _outMax;

            if (!invert)
            {
                _outMin = outMin;
                _outMax = Mathf.LerpUnclamped(outMin, outMax, strength);
            }
            else
            {
                _outMin = 1f - outMin;
                _outMax = Mathf.LerpUnclamped(1f - outMin, 1f - outMax, strength);
            }

            //----------------------------------------------------------------------------------------------------------
            // Fit It

            float outValue = DuMath.Fit(_inMin, _inMax, _outMin, _outMax, inValue);

            //----------------------------------------------------------------------------------------------------------
            // Clamp values if need

            if (clampOutMode == ClampMode.MinOnly || clampOutMode == ClampMode.MinAndMax)
                outValue = Mathf.Max(outValue, clampOutMin);

            if (clampOutMode == ClampMode.MaxOnly || clampOutMode == ClampMode.MinAndMax)
                outValue = Mathf.Min(outValue, clampOutMax);

            //----------------------------------------------------------------------------------------------------------
            // Post Reshape

            switch (postReshapeMode)
            {
                case PostReshapeMode.None:
                    // Nothing need to do
                    break;

                case PostReshapeMode.Steps:
                    outValue = DuMath.Step(outValue, postStepsCount, _outMin, _outMax);
                    break;

                case PostReshapeMode.Curve:
                    float valueNormalized = DuMath.Fit(_outMin, _outMax, 0f, 1f, outValue);

                    valueNormalized = postCurve.Evaluate(valueNormalized);
                    outValue = DuMath.Fit01To(_outMin, _outMax, valueNormalized);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            outValue *= postPower;

            //----------------------------------------------------------------------------------------------------------

            return outValue;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeInMinMax(float value)
        {
            return Mathf.Clamp01(value);
        }

        public static int NormalizePostStepsCount(int value)
        {
            return Mathf.Max(1, value);
        }

        public static AnimationCurve NormalizePostCurve(AnimationCurve curve)
        {
            curve.duClamp01TimeAndValues(true);
            return curve;
        }
    }
}
