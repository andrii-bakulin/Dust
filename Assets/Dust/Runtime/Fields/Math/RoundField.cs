using System;
using UnityEngine;

namespace Dust
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

            value = roundMode switch
            {
                RoundMode.Round => Mathf.Round(value / distance) * distance,
                RoundMode.Floor => Mathf.Floor(value / distance) * distance,
                RoundMode.Ceil => Mathf.Ceil(value / distance) * distance,
                _ => throw new ArgumentOutOfRangeException()
            };

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
            string roundType = roundMode switch
            {
                RoundMode.Round => "Round",
                RoundMode.Floor => "Floor",
                RoundMode.Ceil => "Ceil",
                _ => throw new ArgumentOutOfRangeException()
            };

            return roundType + ", " + distance.ToString("F2");
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            result.power = RoundValue(fieldPoint.result.power);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (calculateColor)
            {
                result.color = fieldPoint.result.color;
                result.color.r = RoundValue(result.color.r);
                result.color.g = RoundValue(result.color.g);
                result.color.b = RoundValue(result.color.b);
                result.color.a = RoundValue(result.color.a);
            }
            else
            {
                result.color = Color.clear;
            }
        }
    }
}
