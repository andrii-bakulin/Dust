using UnityEngine;

namespace DustEngine
{
    [System.Serializable]
    public class Remapping : IDynamicState
    {
        public enum PostReshapeMode
        {
            None = 0,
            Step = 1,
            Curve = 2,
        }

        public enum ColorMode
        {
            Ignore = 0,
            Color = 1,
            Gradient = 2,
            Rainbow = 3,
            RandomColor = 4,
            RandomColorInRange = 5,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_RemapPowerEnabled = true;
        public bool remapPowerEnabled
        {
            get => m_RemapPowerEnabled;
            set => m_RemapPowerEnabled = value;
        }

        [SerializeField]
        private float m_Strength = 1.0f;
        public float strength
        {
            get => m_Strength;
            set => m_Strength = value;
        }

        [SerializeField]
        private float m_Offset = 0.0f;
        public float offset
        {
            get => m_Offset;
            set => m_Offset = NormalizeOffset(value);
        }

        [SerializeField]
        private bool m_LimitByStrength = false;
        public bool limitByStrength
        {
            get => m_LimitByStrength;
            set => m_LimitByStrength = value;
        }

        [SerializeField]
        private bool m_Invert = false;
        public bool invert
        {
            get => m_Invert;
            set => m_Invert = value;
        }

        [SerializeField]
        private float m_Min = 0.0f;
        public float min
        {
            get => m_Min;
            set => m_Min = value;
        }

        [SerializeField]
        private float m_Max = 1.0f;
        public float max
        {
            get => m_Max;
            set => m_Max = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private ClampMode m_ClampMode = ClampMode.MinAndMax;
        public ClampMode clampMode
        {
            get => m_ClampMode;
            set => m_ClampMode = value;
        }

        [SerializeField]
        private float m_ClampMin = 0.0f;
        public float clampMin
        {
            get => m_ClampMin;
            set => m_ClampMin = value;
        }

        [SerializeField]
        private float m_ClampMax = 1.0f;
        public float clampMax
        {
            get => m_ClampMax;
            set => m_ClampMax = value;
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

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private ColorMode m_ColorMode = ColorMode.Color;
        public ColorMode colorMode
        {
            get => m_ColorMode;
            set => m_ColorMode = value;
        }

        [SerializeField]
        protected Color m_Color = new Color(0.0f, 0.5f, 1.0f);
        public Color color
        {
            get => m_Color;
            set => m_Color = value;
        }

        [SerializeField]
        protected Gradient m_Gradient = DuGradient.CreateBlackToColor(new Color(0.0f, 0.5f, 1.0f));
        public Gradient gradient
        {
            get => m_Gradient;
            set => m_Gradient = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_RainbowMinOffset = 0.0f;
        public float rainbowMinOffset
        {
            get => m_RainbowMinOffset;
            set => m_RainbowMinOffset = value;
        }

        [SerializeField]
        private float m_RainbowMaxOffset = 1.0f;
        public float rainbowMaxOffset
        {
            get => m_RainbowMaxOffset;
            set => m_RainbowMaxOffset = value;
        }

        [SerializeField]
        private bool m_RainbowRepeat = false;
        public bool rainbowRepeat
        {
            get => m_RainbowRepeat;
            set => m_RainbowRepeat = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected Color m_RandomMinColor = Color.black;
        public Color randomMinColor
        {
            get => m_RandomMinColor;
            set => m_RandomMinColor = value;
        }

        [SerializeField]
        protected Color m_RandomMaxColor = Color.red;
        public Color randomMaxColor
        {
            get => m_RandomMaxColor;
            set => m_RandomMaxColor = value;
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
                DynamicState.Append(ref dynamicState, ++seq, offset);
                DynamicState.Append(ref dynamicState, ++seq, limitByStrength);
                DynamicState.Append(ref dynamicState, ++seq, invert);
                DynamicState.Append(ref dynamicState, ++seq, min);
                DynamicState.Append(ref dynamicState, ++seq, max);
                DynamicState.Append(ref dynamicState, ++seq, clampMode);

                if (clampMode == ClampMode.MinOnly || clampMode == ClampMode.MinAndMax)
                    DynamicState.Append(ref dynamicState, ++seq, clampMin);

                if (clampMode == ClampMode.MaxOnly || clampMode == ClampMode.MinAndMax)
                    DynamicState.Append(ref dynamicState, ++seq, clampMax);

                DynamicState.Append(ref dynamicState, ++seq, postPower);
                DynamicState.Append(ref dynamicState, ++seq, postReshapeMode);
                DynamicState.Append(ref dynamicState, ++seq, postStepsCount);
                DynamicState.Append(ref dynamicState, ++seq, postCurve);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            DynamicState.Append(ref dynamicState, ++seq, colorMode);

            switch (colorMode)
            {
                case ColorMode.Ignore:
                case ColorMode.RandomColor:
                    // none
                    break;

                case ColorMode.Color:
                    DynamicState.Append(ref dynamicState, ++seq, color);
                    break;

                case ColorMode.Gradient:
                    DynamicState.Append(ref dynamicState, ++seq, gradient);
                    break;

                case ColorMode.Rainbow:
                    DynamicState.Append(ref dynamicState, ++seq, rainbowMinOffset);
                    DynamicState.Append(ref dynamicState, ++seq, rainbowMaxOffset);
                    DynamicState.Append(ref dynamicState, ++seq, rainbowRepeat);
                    break;

                case ColorMode.RandomColorInRange:
                    DynamicState.Append(ref dynamicState, ++seq, randomMinColor);
                    DynamicState.Append(ref dynamicState, ++seq, randomMaxColor);
                    break;

                default:
                    break;
            }

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public float MapValue(float inValue)
        {
            if (!remapPowerEnabled)
                return inValue;

            //----------------------------------------------------------------------------------------------------------

            float inMin = 0f;
            float inMax = 1f - offset;

            if (Mathf.Approximately(inMin, inMax))
                inMax = 0.0001f;

            float outMin;
            float outMax;

            if (!invert)
            {
                outMin = min;
                outMax = Mathf.LerpUnclamped(min, max, strength);
            }
            else
            {
                outMin = 1f - min;
                outMax = Mathf.LerpUnclamped(1f - min, 1f - max, strength);
            }

            if (limitByStrength && inValue > inMax)
                inValue = inMax;

            float outValue = DuMath.Fit(inMin, inMax, outMin, outMax, inValue);

            //----------------------------------------------------------------------------------------------------------
            // Clamp values if need

            if (clampMode == ClampMode.MinOnly || clampMode == ClampMode.MinAndMax)
                outValue = Mathf.Max(outValue, clampMin);

            if (clampMode == ClampMode.MaxOnly || clampMode == ClampMode.MinAndMax)
                outValue = Mathf.Min(outValue, clampMax);

            //----------------------------------------------------------------------------------------------------------
            // Post Reshape

            switch (postReshapeMode)
            {
                case PostReshapeMode.None:
                    // Nothing need to do
                    break;

                case PostReshapeMode.Step:
                    outValue = DuMath.Step(outValue, postStepsCount, outMin, outMax);
                    break;

                case PostReshapeMode.Curve:
                {
                    float valueNormalized = DuMath.Fit(outMin, outMax, 0f, 1f, outValue);

                    valueNormalized = postCurve.Evaluate(valueNormalized);
                    outValue = DuMath.Fit01To(outMin, outMax, valueNormalized);
                    break;
                }

                default:
                    break;
            }

            outValue *= postPower;

            //----------------------------------------------------------------------------------------------------------

            return outValue;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeOffset(float value)
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
