using System;
using UnityEngine;

namespace Dust
{
    [System.Serializable]
    public class Coloring : IDynamicState
    {
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
        private float m_RainbowMinOffset;
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
        private bool m_RainbowRepeat;
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

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected DuNoise m_ColorRandomNoise;
        protected DuNoise colorRandomNoise
        {
            get
            {
                if (Dust.IsNull(m_ColorRandomNoise))
                    m_ColorRandomNoise = new DuNoise(876805);

                return m_ColorRandomNoise;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

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
                    throw new ArgumentOutOfRangeException();
            }

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // How it works
        // 1) scale alpha by powerByField
        // 2) if alpha greater then 1f, then set alpha to 1f, but scale RGB for same value
        // 3) Clamp 0..1
        // Examples:       RGBA(0.1f, 0.2f, 0.4f, 0.50f);
        // Power 0.5 =>    RGBA(0.1f, 0.2f, 0.4f, 0.25f);   => downgrade alpha 0.5f to 0.25f
        // Power 1.0 =>    RGBA(0.1f, 0.2f, 0.4f, 0.50f);   => Nothing change
        // Power 2.0 =>    RGBA(0.1f, 0.2f, 0.4f, 1.00f);   => multiply alpha 2x
        // Power 4.0 =>    RGBA(0.1f, 0.2f, 0.4f, 2.00f);   => RGBA(0.2f, 0.4f, 0.8f, 1.00f);
        // Power 8.0 =>    RGBA(0.1f, 0.2f, 0.4f, 4.00f);   => RGBA(0.4f, 0.8f, 1.0f, 1.00f);

        protected Color GetFieldColorByPower(Color fieldColor, float powerByField)
        {
            fieldColor.a *= powerByField;

            if (fieldColor.a > 1f)
            {
                fieldColor.r *= fieldColor.a;
                fieldColor.g *= fieldColor.a;
                fieldColor.b *= fieldColor.a;
                fieldColor.a = 1f;
            }

            fieldColor.duClamp01();
            return fieldColor;
        }

        //--------------------------------------------------------------------------------------------------------------

        public Color GetColor(float powerByField)
        {
            switch (colorMode)
            {
                case ColorMode.Ignore:
                    return Color.clear;

                case ColorMode.Color:
                    return GetFieldColorByPower(color, powerByField);

                case ColorMode.Gradient:
                    return gradient.Evaluate(powerByField);

                case ColorMode.Rainbow:
                {
                    var rainbowOffset = rainbowRepeat ? Mathf.Repeat(powerByField, 1f) : Mathf.Clamp01(powerByField);
                    rainbowOffset = DuMath.Fit01To(rainbowMinOffset, rainbowMaxOffset, rainbowOffset);
                    rainbowOffset = Mathf.Repeat(rainbowOffset, 1f);
                    return Color.HSVToRGB(rainbowOffset, 1f, 1f);
                }

                case ColorMode.RandomColor:
                {
                    var colorRandomOffset = colorRandomNoise.Perlin1D(powerByField * 2589.7515f, 0f, 2f);
                    return Color.HSVToRGB(colorRandomOffset, 1f, 1f);
                }

                case ColorMode.RandomColorInRange:
                {
                    var colorRandomOffset = colorRandomNoise.Perlin1D_asVector3(powerByField * 2589.7515f, 0f, 2f);
                    var colorAlpha = randomMinColor.a;

                    if (Mathf.Approximately(randomMinColor.a, randomMaxColor.a) == false)
                    {
                        var colorRandomAlpha = colorRandomNoise.Perlin1D(powerByField * 2589.7515f, 0f, 2f);
                        colorAlpha = DuMath.Fit01To(randomMinColor.a, randomMaxColor.a, colorRandomAlpha);
                    }

                    return new Color(
                        DuMath.Fit01To(randomMinColor.r, randomMaxColor.r, colorRandomOffset.x),
                        DuMath.Fit01To(randomMinColor.g, randomMaxColor.g, colorRandomOffset.y),
                        DuMath.Fit01To(randomMinColor.b, randomMaxColor.b, colorRandomOffset.z),
                        colorAlpha);
                }

                default:
                    return Color.magenta;
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        public Gradient GetFieldColorPreview(out float colorPower)
        {
            colorPower = 1f;

            switch (colorMode)
            {
                case ColorMode.Ignore:
                    return Color.clear.duToGradient();

                case ColorMode.Color:
                    colorPower = color.a;
                    return color.duToGradient();

                case ColorMode.Gradient:
                    return gradient;

                case ColorMode.Rainbow:
                    return DuGradient.CreateRainbow(rainbowMinOffset, rainbowMaxOffset);

                case ColorMode.RandomColor:
                    return DuGradient.CreateRandomSet();

                case ColorMode.RandomColorInRange:
                    return DuGradient.CreateBetweenColors(randomMinColor, randomMaxColor);

                default:
                    return Color.magenta.duToGradient();
            }
        }
#endif
    }
}
