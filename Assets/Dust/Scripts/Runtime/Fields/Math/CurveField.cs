using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Curve Field")]
    public class CurveField : MathField
    {
        public enum CurveMode
        {
            Clamp = 0,
            Loop = 1,
            PingPong = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private AnimationCurve m_Shape = DuAnimationCurve.StraightLine01();
        public AnimationCurve shape
        {
            get => m_Shape;
            set => m_Shape = NormalizeShape(value);
        }

        [SerializeField]
        private float m_Offset = 0f;
        public float offset
        {
            get => m_Offset;
            set => m_Offset = value;
        }

        [SerializeField]
        private float m_AnimationSpeed = 0f;
        public float animationSpeed
        {
            get => m_AnimationSpeed;
            set => m_AnimationSpeed = value;
        }

        [SerializeField]
        private CurveMode m_BeforeCurve = CurveMode.Clamp;
        public CurveMode beforeCurve
        {
            get => m_BeforeCurve;
            set => m_BeforeCurve = value;
        }

        [SerializeField]
        private CurveMode m_AfterCurve = CurveMode.Clamp;
        public CurveMode afterCurve
        {
            get => m_AfterCurve;
            set => m_AfterCurve = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private float m_OffsetDynamic;

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, shape);
            DynamicState.Append(ref dynamicState, ++seq, offset);
            DynamicState.Append(ref dynamicState, ++seq, animationSpeed);
            DynamicState.Append(ref dynamicState, ++seq, beforeCurve);
            DynamicState.Append(ref dynamicState, ++seq, afterCurve);

            DynamicState.Append(ref dynamicState, ++seq, m_OffsetDynamic);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Curve";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            result.fieldPower = shape.Evaluate(RecalculateValue(fieldPoint.endPower));

            if (calculateColor)
            {
                result.fieldColor = fieldPoint.endColor;
                result.fieldColor.r = shape.Evaluate(RecalculateValue(result.fieldColor.r));
                result.fieldColor.g = shape.Evaluate(RecalculateValue(result.fieldColor.g));
                result.fieldColor.b = shape.Evaluate(RecalculateValue(result.fieldColor.b));
                result.fieldColor.duClamp01();
            }
            else
            {
                result.fieldColor = Color.clear;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected float RecalculateValue(float value)
        {
            value = value + m_OffsetDynamic + offset * animationSpeed;

            if (value < 0.0f)
            {
                switch (beforeCurve)
                {
                    default:
                    case CurveMode.Clamp:
                        value = 0f;
                        break;

                    case CurveMode.Loop:
                        value = Mathf.Repeat(value, 1f);
                        break;

                    case CurveMode.PingPong:
                        value = Mathf.PingPong(value, 1f);
                        break;
                }
            }
            else if (value > 1.0f)
            {
                switch (afterCurve)
                {
                    default:
                    case CurveMode.Clamp:
                        value = 1f;
                        break;

                    case CurveMode.Loop:
                        value = Mathf.Repeat(value, 1f);
                        break;

                    case CurveMode.PingPong:
                        value = Mathf.PingPong(value, 1f);
                        break;
                }
            }

            return value;
        }

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            m_OffsetDynamic += Time.deltaTime * animationSpeed;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static AnimationCurve NormalizeShape(AnimationCurve curve)
        {
            curve.duClamp01TimeAndValues(true);
            return curve;
        }
    }
}
