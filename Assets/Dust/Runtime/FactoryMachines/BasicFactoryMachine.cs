using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Factory Machines/Basic Machine")]
    public class BasicFactoryMachine : FactoryMachine
    {
        public enum ValueBlendMode
        {
            Set = 0,
            Add = 1,
            Subtract = 2,
            Multiply = 3,
            Divide = 4,
            Avg = 5,
            Min = 6,
            Max = 7,
            BlendClamped = 8,
            BlendUnclamped = 9,
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public enum ColorBlendMode
        {
            Blend = 0,
            Set = 1,
            Add = 2,
            Subtract = 3,
            Multiply = 4,
            Min = 5,
            Max = 6,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected bool m_ValueImpactEnabled;
        public bool valueImpactEnabled
        {
            get => m_ValueImpactEnabled;
            set => m_ValueImpactEnabled = value;
        }

        [SerializeField]
        protected float m_ValueImpactIntensity = 1f;
        public float valueImpactIntensity
        {
            get => m_ValueImpactIntensity;
            set => m_ValueImpactIntensity = NormalizeImpactIntensity(value);
        }

        [SerializeField]
        protected ValueBlendMode m_ValueBlendMode = ValueBlendMode.Add;
        public ValueBlendMode valueBlendMode
        {
            get => m_ValueBlendMode;
            set => m_ValueBlendMode = value;
        }

        [SerializeField]
        protected bool m_ValueClampEnabled;
        public bool valueClampEnabled
        {
            get => m_ValueClampEnabled;
            set => m_ValueClampEnabled = value;
        }

        [SerializeField]
        protected float m_ValueClampMin;
        public float valueClampMin
        {
            get => m_ValueClampMin;
            set => m_ValueClampMin = value;
        }

        [SerializeField]
        protected float m_ValueClampMax = 1f;
        public float valueClampMax
        {
            get => m_ValueClampMax;
            set => m_ValueClampMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected bool m_ColorImpactEnabled;
        public bool colorImpactEnabled
        {
            get => m_ColorImpactEnabled;
            set => m_ColorImpactEnabled = value;
        }

        [SerializeField]
        protected float m_ColorImpactIntensity = 1f;
        public float colorImpactIntensity
        {
            get => m_ColorImpactIntensity;
            set => m_ColorImpactIntensity = NormalizeImpactIntensity(value);
        }

        [SerializeField]
        protected ColorBlendMode m_ColorBlendMode = ColorBlendMode.Blend;
        public ColorBlendMode colorBlendMode
        {
            get => m_ColorBlendMode;
            set => m_ColorBlendMode = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected FieldsMap m_FieldsMap = FieldsMap.FactoryMachine();
        public FieldsMap fieldsMap => m_FieldsMap;

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "Basic";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override bool PrepareForUpdateInstancesStates(FactoryInstanceState factoryInstanceState)
        {
            return DuMath.IsNotZero(factoryInstanceState.intensityByFactory);
        }

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            float intensityByMachine = intensity;

            UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);
        }

        public override void FinalizeUpdateInstancesStates(FactoryInstanceState factoryInstanceState)
        {
            // Hide base implementation
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void UpdateInstanceDynamicState(FactoryInstanceState factoryInstanceState, float intensityByMachine)
        {
            factoryInstanceState.intensityByMachine = intensityByMachine;

            fieldsMap.Calculate(factoryInstanceState, out Field.Result result);
            {
                factoryInstanceState.fieldPower = result.power;
                factoryInstanceState.fieldColor = result.color;
            }

            UpdateInstanceDynamicState_Value(factoryInstanceState);
            UpdateInstanceDynamicState_Color(factoryInstanceState);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected void UpdateInstanceDynamicState_Value(FactoryInstanceState factoryInstanceState)
        {
            if (!valueImpactEnabled)
                return;

            if (DuMath.IsZero(valueImpactIntensity) || DuMath.IsZero(factoryInstanceState.intensityByMachine))
                return;

            var instanceState = factoryInstanceState.instance.stateDynamic;

            float newValue = factoryInstanceState.fieldPower;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Blending

            switch (valueBlendMode)
            {
                default:
                case ValueBlendMode.Set:
                    // Nothing need to do...
                    break;

                case ValueBlendMode.Add:
                    newValue = instanceState.value + newValue;
                    break;

                case ValueBlendMode.Subtract:
                    newValue = instanceState.value - newValue;
                    break;

                case ValueBlendMode.Multiply:
                    newValue = instanceState.value * newValue;
                    break;

                case ValueBlendMode.Divide:
                    newValue = DuMath.IsNotZero(newValue) ? instanceState.value / newValue : 0f;
                    break;

                case ValueBlendMode.Avg:
                    newValue = (instanceState.value + newValue) / 2f;
                    break;

                case ValueBlendMode.Min:
                    newValue = Mathf.Min(instanceState.value, newValue);
                    break;

                case ValueBlendMode.Max:
                    newValue = Mathf.Max(instanceState.value, newValue);
                    break;

                case ValueBlendMode.BlendClamped:
                    newValue = Mathf.Lerp(instanceState.value, newValue, newValue);
                    break;

                case ValueBlendMode.BlendUnclamped:
                    newValue = Mathf.LerpUnclamped(instanceState.value, newValue, newValue);
                    break;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Merge
            
            // Here final calculation may looks like a bug.
            // But it's NO!
            // Final value calculates next way:
            // 1) Took Value before calculations by upper rules [v1]
            // 2) Calculate Value by all upper rules [v2]
            // 3) Get UnclampedLerp between v1 and v2 by finalIntensity (included all intensities).
            //
            // For finalIntensity [0..1] -> it's looks good
            // For finalIntensity < 0 or finalIntensity > 1 -> something strange may happened
            //
            // Situation:
            //      Base Factory has init power 10.0f
            //      Inner TransformFactoryMachine has final power 2.0f
            //      ValueBlendMode = SET <<!!
            // For SET mode:
            //      For finalIntensity = 0.f => LerpUnclamped(10f, 2f, 1f) =>   10f (OK) 
            //      For finalIntensity = 1.f => LerpUnclamped(10f, 2f, 1f) =>   2f  (OK)
            //      For finalIntensity = 2.f => LerpUnclamped(10f, 2f, 2f) =>  -6f  (Hm...) 
            //      For finalIntensity = 3.f => LerpUnclamped(10f, 2f, 2f) => -14f  (Hm...) 
            //      For finalIntensity =-1.f => LerpUnclamped(10f, 2f, 1f) =>  18f  (Hm...)
            //
            // But I keep this logic to save linearity of results!
            
            // @notice: here fieldPower also involve to transferPower (not like in PRS-Factory)
            float finalIntensity = factoryInstanceState.intensityByFactory 
                                   * factoryInstanceState.intensityByMachine 
                                   * valueImpactIntensity;

            instanceState.value = Mathf.LerpUnclamped(instanceState.value, newValue, finalIntensity);

            if (valueClampEnabled)
                instanceState.value = Mathf.Clamp(instanceState.value, valueClampMin, valueClampMax);
        }

        protected void UpdateInstanceDynamicState_Color(FactoryInstanceState factoryInstanceState)
        {
            if (!colorImpactEnabled)
                return;

            if (DuMath.IsZero(colorImpactIntensity) || DuMath.IsZero(factoryInstanceState.intensityByMachine))
                return;

            var instanceState = factoryInstanceState.instance.stateDynamic;

            Color newColor = factoryInstanceState.fieldColor;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Blending

            switch (colorBlendMode)
            {
                case ColorBlendMode.Blend:
                default:
                    newColor = ColorBlend.AlphaBlend(instanceState.color, newColor);
                    break;

                case ColorBlendMode.Set:
                    // nothing need to do
                    // newColor = newColor;
                    break;

                case ColorBlendMode.Add:
                    newColor = ColorBlend.Add(instanceState.color, newColor);
                    break;

                case ColorBlendMode.Subtract:
                    newColor = ColorBlend.Subtract(instanceState.color, newColor);
                    break;

                case ColorBlendMode.Multiply:
                    newColor = ColorBlend.Multiply(instanceState.color, newColor);
                    break;

                case ColorBlendMode.Min:
                    newColor = ColorBlend.MinAfterBlend(instanceState.color, newColor);
                    break;

                case ColorBlendMode.Max:
                    newColor = ColorBlend.MaxAfterBlend(instanceState.color,newColor);
                    break;
            }

            // @notice: here fieldPower also involve to transferPower (not like in PRS-Factory)
            float transferPower = Mathf.Abs(factoryInstanceState.intensityByFactory)
                                  * factoryInstanceState.intensityByMachine
                                  * colorImpactIntensity;

            instanceState.color = Color.LerpUnclamped(instanceState.color, newColor, transferPower).duClamped01();
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, valueImpactEnabled);
            if (valueImpactEnabled)
            {
                DynamicState.Append(ref dynamicState, ++seq, valueImpactIntensity);
                DynamicState.Append(ref dynamicState, ++seq, valueBlendMode);
                DynamicState.Append(ref dynamicState, ++seq, valueClampEnabled);
                DynamicState.Append(ref dynamicState, ++seq, valueClampMin);
                DynamicState.Append(ref dynamicState, ++seq, valueClampMax);
            }

            DynamicState.Append(ref dynamicState, ++seq, colorImpactEnabled);
            if (colorImpactEnabled)
            {
                DynamicState.Append(ref dynamicState, ++seq, colorImpactIntensity);
                DynamicState.Append(ref dynamicState, ++seq, colorBlendMode);
            }

            DynamicState.Append(ref dynamicState, ++seq, fieldsMap);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void ResetToDefaults()
        {
            fieldsMap.defaultColor = new Color(0.66f, 0.12f, 0.83f);
        }

        void Reset()
        {
            ResetToDefaults();

            m_ValueImpactEnabled = true;
            m_ColorImpactEnabled = true;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeImpactIntensity(float value)
        {
            return Mathf.Clamp01(value);
        }
    }
}
