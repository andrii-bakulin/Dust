using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Fit Field")]
    public class FitField : MathField
    {
        [SerializeField]
        private float m_MinInput = 0f;
        public float minInput
        {
            get => m_MinInput;
            set => m_MinInput = value;
        }

        [SerializeField]
        private float m_MaxInput = 1f;
        public float maxInput
        {
            get => m_MaxInput;
            set => m_MaxInput = value;
        }

        [SerializeField]
        private float m_MinOutput = 0f;
        public float minOutput
        {
            get => m_MinOutput;
            set => m_MinOutput = value;
        }

        [SerializeField]
        private float m_MaxOutput = 1f;
        public float maxOutput
        {
            get => m_MaxOutput;
            set => m_MaxOutput = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, minInput);
            DynamicState.Append(ref dynamicState, ++seq, maxInput);
            DynamicState.Append(ref dynamicState, ++seq, minOutput);
            DynamicState.Append(ref dynamicState, ++seq, maxOutput);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Fit";
        }

        public override string FieldDynamicHint()
        {
            return "[" + minInput.ToString("F2") + " .. " + maxInput.ToString("F2") + "]"
                + " > "
                + "[" + minOutput.ToString("F2") + " .. " + maxOutput.ToString("F2") + "]";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            result.fieldPower = DuMath.Fit(minInput, maxInput, minOutput, maxOutput, fieldPoint.endPower);

            result.fieldColor = Color.clear;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override bool IsAllowCalculateFieldColor()
        {
            return false;
        }
    }
}
