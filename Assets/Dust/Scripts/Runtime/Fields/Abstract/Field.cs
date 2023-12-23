using System;
using UnityEngine;

namespace DustEngine
{
    public abstract partial class Field : DuMonoBehaviour, IDynamicState
    {
        public static readonly Color k_GizmosColorRangeZero = new Color(0.0f, 0.3f, 0.6f);
        public static readonly Color k_GizmosColorRangeOne = new Color(0.0f, 0.5f, 1.0f);

        //--------------------------------------------------------------------------------------------------------------

        public class Point
        {
            // In
            internal Vector3 inPosition; // point in world position
            internal float inOffset; // offset for point in sequence of points [0..1]

#if DUST_NEW_FEATURE_FACTORY
            internal FactoryMachine.FactoryInstanceState inFactoryInstanceState;
#endif

            // Out/End/Resulted values
            internal float endPower; // power calculated by fieldsMap
            internal Color endColor; // color calculated by fieldsMap
        }

        public struct Result : IEquatable<Result>
        {
            internal float fieldPower; // power calculated by field
            internal Color fieldColor; // color calculated by field, Color.alpha used as power of color

            public bool Equals(Result other)
            {
                throw new NotImplementedException();
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected string m_CustomHint = "";
        public string customHint
        {
            get => m_CustomHint;
            set => m_CustomHint = value;
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

        private void Start()
        {
            // Require to show enabled-checkbox in editor for all fields
        }

        //--------------------------------------------------------------------------------------------------------------

        public abstract string FieldName();

        public abstract string FieldDynamicHint();

        public abstract void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public abstract bool IsAllowCalculateFieldColor();

#if UNITY_EDITOR
        public abstract bool IsHasFieldColorPreview();
        public abstract Gradient GetFieldColorPreview(out float colorPower);
#endif

        //--------------------------------------------------------------------------------------------------------------

        public virtual FieldsMap.FieldRecord.BlendPowerMode GetBlendPowerMode()
        {
            return FieldsMap.FieldRecord.BlendPowerMode.Ignore;
        }

        public virtual FieldsMap.FieldRecord.BlendColorMode GetBlendColorMode()
        {
            return FieldsMap.FieldRecord.BlendColorMode.Ignore;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public virtual int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            // Paste here local vars
            DynamicState.Append(ref dynamicState, ++seq, true);

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
        protected Color GetFieldColorByPower(Color color, float powerByField)
        {
            color.a *= powerByField;

            if (color.a > 1f)
            {
                color.r *= color.a;
                color.g *= color.a;
                color.b *= color.a;
                color.a = 1f;
            }

            color.duClamp01();
            return color;
        }

        protected Color GetFieldColorFromRemapping(Remapping remapping, float powerByField, bool calculateColor)
        {
            if (calculateColor == false)
                return Color.clear;

            switch (remapping.colorMode)
            {
                case Remapping.ColorMode.Ignore:
                    return Color.clear;

                case Remapping.ColorMode.Color:
                    return GetFieldColorByPower(remapping.color, powerByField);

                case Remapping.ColorMode.Gradient:
                    return remapping.gradient.Evaluate(powerByField);

                case Remapping.ColorMode.Rainbow:
                {
                    var rainbowOffset = remapping.rainbowRepeat ? Mathf.Repeat(powerByField, 1f) : Mathf.Clamp01(powerByField);
                    rainbowOffset = DuMath.Fit01To(remapping.rainbowMinOffset, remapping.rainbowMaxOffset, rainbowOffset);
                    rainbowOffset = Mathf.Repeat(rainbowOffset, 1f);
                    return Color.HSVToRGB(rainbowOffset, 1f, 1f);
                }

                case Remapping.ColorMode.RandomColor:
                {
                    var colorRandomOffset = colorRandomNoise.Perlin1D(powerByField * 2589.7515f, 0f, 2f);
                    return Color.HSVToRGB(colorRandomOffset, 1f, 1f);
                }

                case Remapping.ColorMode.RandomColorInRange:
                {
                    var colorRandomOffset = colorRandomNoise.Perlin1D_asVector3(powerByField * 2589.7515f, 0f, 2f);
                    var colorAlpha = remapping.randomMinColor.a;

                    if (Mathf.Approximately(remapping.randomMinColor.a, remapping.randomMaxColor.a) == false)
                    {
                        var colorRandomAlpha = colorRandomNoise.Perlin1D(powerByField * 2589.7515f, 0f, 2f);
                        colorAlpha = DuMath.Fit01To(remapping.randomMinColor.a, remapping.randomMaxColor.a, colorRandomAlpha);
                    }

                    return new Color(
                        DuMath.Fit01To(remapping.randomMinColor.r, remapping.randomMaxColor.r, colorRandomOffset.x),
                        DuMath.Fit01To(remapping.randomMinColor.g, remapping.randomMaxColor.g, colorRandomOffset.y),
                        DuMath.Fit01To(remapping.randomMinColor.b, remapping.randomMaxColor.b, colorRandomOffset.z),
                        colorAlpha);
                }

                default:
                    return Color.magenta;
            }
        }

        protected Gradient GetFieldColorPreview(Remapping remapping, out float colorPower)
        {
            colorPower = 1f;

            switch (remapping.colorMode)
            {
                case Remapping.ColorMode.Ignore:
                    return Color.clear.duToGradient();

                case Remapping.ColorMode.Color:
                    colorPower = remapping.color.a;
                    return remapping.color.duToGradient();

                case Remapping.ColorMode.Gradient:
                    return remapping.gradient;

                case Remapping.ColorMode.Rainbow:
                    return DuGradient.CreateRainbow(remapping.rainbowMinOffset, remapping.rainbowMaxOffset);

                case Remapping.ColorMode.RandomColor:
                    return DuGradient.CreateRandomSet();

                case Remapping.ColorMode.RandomColorInRange:
                    return DuGradient.CreateBetweenColors(remapping.randomMinColor, remapping.randomMaxColor);

                default:
                    return Color.magenta.duToGradient();
            }
        }
    }
}
