using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Clamp Field")]
    public class ClampField : MathField
    {
        [SerializeField]
        private ClampMode m_PowerClampMode = ClampMode.MinAndMax;
        public ClampMode powerClampMode
        {
            get => m_PowerClampMode;
            set => m_PowerClampMode = value;
        }

        [SerializeField]
        private float m_PowerClampMin = 0f;
        public float powerClampMin
        {
            get => m_PowerClampMin;
            set => m_PowerClampMin = value;
        }

        [SerializeField]
        private float m_PowerClampMax = 1f;
        public float powerClampMax
        {
            get => m_PowerClampMax;
            set => m_PowerClampMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private ClampMode m_ColorClampMode = ClampMode.MinAndMax;
        public ClampMode colorClampMode
        {
            get => m_ColorClampMode;
            set => m_ColorClampMode = value;
        }

        [SerializeField]
        private Color m_ColorClampMin = new Color(0f, 0f, 0f, 0f);
        public Color colorClampMin
        {
            get => m_ColorClampMin;
            set => m_ColorClampMin = value;
        }

        [SerializeField]
        private Color m_ColorClampMax = new Color(1f, 1f, 1f, 1f);
        public Color colorClampMax
        {
            get => m_ColorClampMax;
            set => m_ColorClampMax = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, powerClampMode);

            if (powerClampMode != ClampMode.NoClamp)
            {
                DynamicState.Append(ref dynamicState, ++seq, powerClampMin);
                DynamicState.Append(ref dynamicState, ++seq, powerClampMax);
            }


            DynamicState.Append(ref dynamicState, ++seq, colorClampMode);

            if (colorClampMode != ClampMode.NoClamp)
            {
                DynamicState.Append(ref dynamicState, ++seq, colorClampMin);
                DynamicState.Append(ref dynamicState, ++seq, colorClampMax);
            }

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Clamp";
        }

        public override string FieldDynamicHint()
        {
            var parts = new List<string>();

            switch (powerClampMode)
            {
                case ClampMode.MinAndMax:
                    parts.Add("[" + powerClampMin.ToString("F2") + " .. " + powerClampMax.ToString("F2") + "]");
                    break;

                case ClampMode.MinOnly:
                    parts.Add("[" + powerClampMin.ToString("F2") + " .. ∞)");
                    break;

                case ClampMode.MaxOnly:
                    parts.Add("(∞ .. " + powerClampMax.ToString("F2") + "]");
                    break;

                case ClampMode.NoClamp:
                    parts.Add("(∞ .. ∞)");
                    break;

                default:
                    break;
            }

            if (colorClampMode != ClampMode.NoClamp)
                parts.Add("Color");

            return string.Join(" + ", parts);
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            result.fieldPower = fieldPoint.endPower;

            if (powerClampMode == ClampMode.MinAndMax || powerClampMode == ClampMode.MinOnly)
                result.fieldPower = Mathf.Max(result.fieldPower, powerClampMin);

            if (powerClampMode == ClampMode.MinAndMax || powerClampMode == ClampMode.MaxOnly)
                result.fieldPower = Mathf.Min(result.fieldPower, powerClampMax);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (calculateColor)
            {
                result.fieldColor = fieldPoint.endColor;

                if (colorClampMode == ClampMode.MinAndMax || colorClampMode == ClampMode.MinOnly)
                    result.fieldColor = DuColor.Max(result.fieldColor, colorClampMin);

                if (colorClampMode == ClampMode.MinAndMax || colorClampMode == ClampMode.MaxOnly)
                    result.fieldColor = DuColor.Min(result.fieldColor, colorClampMax);
            }
            else
            {
                result.fieldColor = Color.clear;
            }
        }
    }
}
