using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Invert Field")]
    public class InvertField : MathField
    {
        [SerializeField]
        private bool m_ColorInvertAlpha = false;
        public bool colorInvertAlpha
        {
            get => m_ColorInvertAlpha;
            set => m_ColorInvertAlpha = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, colorInvertAlpha);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Invert";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            result.fieldPower = 1f - fieldPoint.endPower;

            if (calculateColor)
            {
                if (colorInvertAlpha)
                    result.fieldColor = DuColor.InvertRGBA(fieldPoint.endColor);
                else
                    result.fieldColor = DuColor.InvertRGB(fieldPoint.endColor);
            }
            else
            {
                result.fieldColor = Color.clear;
            }
        }
    }
}
