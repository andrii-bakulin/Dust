using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Fields/Math Fields/Invert Field")]
    public class InvertField : MathField
    {
        [SerializeField]
        private bool m_ColorInvertAlpha;
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
            result.power = 1f - fieldPoint.result.power;

            if (calculateColor)
            {
                if (colorInvertAlpha)
                    result.color = DuColor.InvertRGBA(fieldPoint.result.color);
                else
                    result.color = DuColor.InvertRGB(fieldPoint.result.color);
            }
            else
            {
                result.color = Color.clear;
            }
        }
    }
}
