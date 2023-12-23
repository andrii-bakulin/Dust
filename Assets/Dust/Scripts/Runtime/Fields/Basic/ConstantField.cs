using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Basic Fields/Constant Field")]
    public class ConstantField : Field
    {
        [SerializeField]
        private float m_Power = 1f;
        public float power
        {
            get => m_Power;
            set => m_Power = value;
        }

        [SerializeField]
        private Color m_Color = Color.white;
        public Color color
        {
            get => m_Color;
            set => m_Color = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, power);
            DynamicState.Append(ref dynamicState, ++seq, color);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Constant";
        }

        public override string FieldDynamicHint()
        {
            return "Power " + power.ToString("F2");
        }

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            result.fieldPower = power;

            // Notice: ignore power in alpha, but used what user defined in editor
            result.fieldColor = calculateColor ? color : Color.clear;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override bool IsAllowCalculateFieldColor()
        {
            return true;
        }

#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return true;
        }

        public override Gradient GetFieldColorPreview(out float colorPower)
        {
            colorPower = color.a;
            return color.duToGradient();
        }
#endif
    }
}
