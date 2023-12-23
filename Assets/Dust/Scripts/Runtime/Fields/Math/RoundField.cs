using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Round Field")]
    public class RoundField : MathField
    {
        public enum RoundMode
        {
            Round = 0,
            Floor = 1,
            Ceil = 2,
        }

        [SerializeField]
        private RoundMode m_RoundMode = RoundMode.Round;
        public RoundMode roundMode
        {
            get => m_RoundMode;
            set => m_RoundMode = value;
        }

        [SerializeField]
        private float m_Distance = 0.2f;
        public float distance
        {
            get => m_Distance;
            set => m_Distance = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, roundMode);
            DynamicState.Append(ref dynamicState, ++seq, distance);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public float RoundValue(float value)
        {
            if (DuMath.IsZero(distance))
                return 0f;

            switch (roundMode)
            {
                case RoundMode.Round:
                    value = Mathf.Round(value / distance) * distance;
                    break;

                case RoundMode.Floor:
                    value = Mathf.Floor(value / distance) * distance;
                    break;

                case RoundMode.Ceil:
                    value = Mathf.Ceil(value / distance) * distance;
                    break;

                default:
                    break;
            }

            return value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Round";
        }

        public override string FieldDynamicHint()
        {
            string hint = "";

            switch (roundMode)
            {
                case RoundMode.Round: hint = "Round"; break;
                case RoundMode.Floor: hint = "Floor"; break;
                case RoundMode.Ceil:  hint = "Ceil"; break;

                default:
                    break;
            }

            return hint + ", " + distance.ToString("F2");
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            result.fieldPower = RoundValue(fieldPoint.endPower);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (calculateColor)
            {
                result.fieldColor = fieldPoint.endColor;
                result.fieldColor.r = RoundValue(result.fieldColor.r);
                result.fieldColor.g = RoundValue(result.fieldColor.g);
                result.fieldColor.b = RoundValue(result.fieldColor.b);
                result.fieldColor.a = RoundValue(result.fieldColor.a);
            }
            else
            {
                result.fieldColor = Color.clear;
            }
        }
    }
}
