using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dust
{
    /// <summary>
    ///
    /// Main idea for color usage:
    /// - R+G+B define color
    /// - A     define intensity of color in this point
    ///
    /// If need change intensity of some color only require multiple A for required value
    ///
    /// </summary>
    [Serializable]
    public class FieldsMap : IDynamicState
    {
        [Serializable]
        public class FieldRecord
        {
            public enum BlendPowerMode
            {
                Ignore = 0,
                Set = 1,
                Add = 2,
                Subtract = 3,
                Multiply = 4,
                Divide = 5,
                Avg = 6,
                Min = 7,
                Max = 8,
            }

            public enum BlendColorMode
            {
                Ignore = 0,
                Blend = 1,
                Set = 2,
                Add = 3,
                Subtract = 4,
                Multiply = 5,
                Min = 6,
                Max = 7,
            }

            //----------------------------------------------------------------------------------------------------------

            [SerializeField]
            private bool m_Enabled = true;
            public bool enabled
            {
                get => m_Enabled;
                set => m_Enabled = value;
            }

            [SerializeField]
            private Field m_Field;
            public Field field
            {
                get => m_Field;
                set => m_Field = value;
            }

            [SerializeField]
            private BlendPowerMode m_BlendPowerMode = BlendPowerMode.Set;
            public BlendPowerMode blendPowerMode
            {
                get => m_BlendPowerMode;
                set => m_BlendPowerMode = value;
            }

            [SerializeField]
            private BlendColorMode m_BlendColorMode = BlendColorMode.Set;
            public BlendColorMode blendColorMode
            {
                get => m_BlendColorMode;
                set => m_BlendColorMode = value;
            }

            [SerializeField]
            private float m_Intensity = 1f;
            public float intensity
            {
                get => m_Intensity;
                set => m_Intensity = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public enum FieldsMapMode
        {
            Custom = 0,
            FieldsSpace = 1,
            FactoryMachine = 2,
#if DUST_NEW_FEATURE_DEFORMER
            Deformer = 3,
#endif
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private FieldsMapMode m_FieldsMapMode = FieldsMapMode.Custom;
        public FieldsMapMode fieldsMapMode => m_FieldsMapMode;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_CalculatePower;
        public bool calculatePower
        {
            get => m_CalculatePower;
            set => m_CalculatePower = value;
        }

        [SerializeField]
        private float m_DefaultPower;
        public float defaultPower
        {
            get => m_DefaultPower;
            set => m_DefaultPower = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_CalculateColor;
        public bool calculateColor
        {
            get => m_CalculateColor;
            set => m_CalculateColor = value;
        }

        [SerializeField]
        private Color m_DefaultColor = Color.clear;
        public Color defaultColor
        {
            get => m_DefaultColor;
            set => m_DefaultColor = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected List<FieldRecord> m_Fields = new List<FieldRecord>();
        public List<FieldRecord> fields => m_Fields;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private Field.Point m_CalcFieldPoint = new Field.Point();

        //--------------------------------------------------------------------------------------------------------------

#if DUST_NEW_FEATURE_DEFORMER
        public static FieldsMap Deformer()
        {
            return new FieldsMap
            {
                m_FieldsMapMode = FieldsMapMode.Deformer,

                calculatePower = true,
                calculateColor = false,

                defaultPower = 1f,
            };
        }
#endif

        public static FieldsMap FactoryMachine()
        {
            return new FieldsMap
            {
                m_FieldsMapMode = FieldsMapMode.FactoryMachine,

                calculatePower = true,
                calculateColor = true,

                defaultPower = 1f,
                defaultColor = Color.clear,
            };
        }

        public static FieldsMap FieldsSpace()
        {
            return new FieldsMap
            {
                m_FieldsMapMode = FieldsMapMode.FieldsSpace,

                calculatePower = true,
                calculateColor = true,

                defaultPower = 0f,
                defaultColor = Color.black,
            };
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DynamicState.Append(ref dynamicState, ++seq, calculatePower);
            DynamicState.Append(ref dynamicState, ++seq, defaultPower);
            DynamicState.Append(ref dynamicState, ++seq, calculateColor);
            DynamicState.Append(ref dynamicState, ++seq, defaultColor);

            DynamicState.Append(ref dynamicState, ++seq, fields.Count);

            foreach (FieldRecord fieldRecord in fields)
            {
                // @WARNING!!! require sync code in: GetDynamicStateHashCode() + Calculate()

                if (Dust.IsNull(fieldRecord) || !fieldRecord.enabled || Dust.IsNull(fieldRecord.field))
                    continue;

                if (!fieldRecord.field.enabled || !fieldRecord.field.gameObject.activeInHierarchy)
                    continue;

                // @END

                DynamicState.Append(ref dynamicState, ++seq, fieldRecord.enabled);
                DynamicState.Append(ref dynamicState, ++seq, fieldRecord.field);
                DynamicState.Append(ref dynamicState, ++seq, fieldRecord.blendPowerMode);
                DynamicState.Append(ref dynamicState, ++seq, fieldRecord.blendColorMode);
                DynamicState.Append(ref dynamicState, ++seq, fieldRecord.intensity);
            }

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public bool Calculate(Vector3 worldPosition, float sequenceOffset, out Field.Result result)
        {
            if (!HasFields())
            {
                result.power = defaultPower;
                result.color = defaultColor;
                return false;
            }

            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = sequenceOffset;
            m_CalcFieldPoint.inFactoryInstanceState = null;

            bool didCalculated = Calculate(m_CalcFieldPoint);
            result = m_CalcFieldPoint.result;
            return didCalculated;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public bool Calculate(FactoryMachine.FactoryInstanceState factoryInstanceState, out Field.Result result)
        {
            if (!HasFields())
            {
                result.power = defaultPower;
                result.color = defaultColor;
                return false;
            }

            m_CalcFieldPoint.inPosition = factoryInstanceState.factory.GetPositionInWorldSpace(factoryInstanceState.instance);
            m_CalcFieldPoint.inOffset = factoryInstanceState.instance.offset;
            m_CalcFieldPoint.inFactoryInstanceState = factoryInstanceState;

            bool didCalculated = Calculate(m_CalcFieldPoint);
            result = m_CalcFieldPoint.result;
            return didCalculated;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public bool Calculate(Field.Point fieldPoint)
        {
            fieldPoint.result.power = defaultPower;
            fieldPoint.result.color = defaultColor;

            if (fields.Count == 0)
                return false;

            Field.Result fieldResultData;

            foreach (FieldRecord fieldRecord in fields)
            {
                // @WARNING!!! require sync code in: GetDynamicStateHashCode() + Calculate()

                if (Dust.IsNull(fieldRecord) || !fieldRecord.enabled || Dust.IsNull(fieldRecord.field))
                    continue;

                if (!fieldRecord.field.enabled || !fieldRecord.field.gameObject.activeInHierarchy)
                    continue;

                // @END

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                bool calculatePowerNow = calculatePower
                                         && fieldRecord.blendPowerMode != FieldRecord.BlendPowerMode.Ignore;

                bool calculateColorNow = calculateColor
                                         && fieldRecord.blendColorMode != FieldRecord.BlendColorMode.Ignore
                                         && fieldRecord.field.IsAllowCalculateFieldColor();

                fieldRecord.field.Calculate(fieldPoint, out fieldResultData, calculateColorNow);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Calculate Power

                if (calculatePowerNow)
                {
                    float afterBlendPower;

                    switch (fieldRecord.blendPowerMode)
                    {
                        default:
                        case FieldRecord.BlendPowerMode.Set:
                            afterBlendPower = fieldResultData.power;
                            break;

                        case FieldRecord.BlendPowerMode.Add:
                            afterBlendPower = fieldPoint.result.power + fieldResultData.power;
                            break;

                        case FieldRecord.BlendPowerMode.Subtract:
                            afterBlendPower = fieldPoint.result.power - fieldResultData.power;
                            break;

                        case FieldRecord.BlendPowerMode.Multiply:
                            afterBlendPower = fieldPoint.result.power * fieldResultData.power;
                            break;

                        case FieldRecord.BlendPowerMode.Divide:
                            afterBlendPower = DuMath.IsNotZero(fieldResultData.power) ? fieldPoint.result.power / fieldResultData.power : 0f;
                            break;

                        case FieldRecord.BlendPowerMode.Avg:
                            afterBlendPower = (fieldPoint.result.power + fieldResultData.power) / 2f;
                            break;

                        case FieldRecord.BlendPowerMode.Min:
                            afterBlendPower = Mathf.Min(fieldPoint.result.power, fieldResultData.power);
                            break;

                        case FieldRecord.BlendPowerMode.Max:
                            afterBlendPower = Mathf.Max(fieldPoint.result.power, fieldResultData.power);
                            break;
                    }

                    fieldPoint.result.power = Mathf.LerpUnclamped(fieldPoint.result.power, afterBlendPower, fieldRecord.intensity);
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Calculate new color

                if (calculateColorNow)
                {
                    Color blendedColor;

                    switch (fieldRecord.blendColorMode)
                    {
                        default:
                        case FieldRecord.BlendColorMode.Set:
                            blendedColor = fieldResultData.color;
                            break;

                        case FieldRecord.BlendColorMode.Blend:
                            blendedColor = ColorBlend.AlphaBlend(fieldPoint.result.color, fieldResultData.color);
                            break;

                        case FieldRecord.BlendColorMode.Add:
                            blendedColor = ColorBlend.Add(fieldPoint.result.color, fieldResultData.color);
                            break;

                        case FieldRecord.BlendColorMode.Subtract:
                            blendedColor = ColorBlend.Subtract(fieldPoint.result.color, fieldResultData.color);
                            break;

                        case FieldRecord.BlendColorMode.Multiply:
                            blendedColor = ColorBlend.Multiply(fieldPoint.result.color, fieldResultData.color);
                            break;

                        case FieldRecord.BlendColorMode.Min:
                            blendedColor = ColorBlend.MinAfterBlend(fieldPoint.result.color, fieldResultData.color);
                            break;

                        case FieldRecord.BlendColorMode.Max:
                            blendedColor = ColorBlend.MaxAfterBlend(fieldPoint.result.color,fieldResultData.color);
                            break;
                    }

                    fieldPoint.result.color = Color.Lerp(fieldPoint.result.color, blendedColor, fieldRecord.intensity);
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            }

            return true;
        }

        //--------------------------------------------------------------------------------------------------------------

        public bool HasFields()
        {
            return fields.Count > 0;
        }

        public bool HasField(Field field)
        {
            return Dust.IsNotNull(field) && fields.Any(rec => rec.field == field);
        }

        public FieldRecord.BlendPowerMode GetDefaultBlendPower(Field field)
        {
            var blendMode = field.GetBlendPowerMode();

            if (blendMode != FieldRecord.BlendPowerMode.Ignore)
                return blendMode;

            return fields.Count == 0 ? FieldRecord.BlendPowerMode.Set : FieldRecord.BlendPowerMode.Max;
        }

        public FieldRecord.BlendColorMode GetDefaultBlendColor(Field field)
        {
            var blendMode = field.GetBlendColorMode();

            if (blendMode != FieldRecord.BlendColorMode.Ignore)
                return blendMode;

            return FieldRecord.BlendColorMode.Blend;
        }

        //--------------------------------------------------------------------------------------------------------------

        public FieldRecord AddField(Field field)
        {
            var fieldRecord = new FieldRecord
            {
                field = field,
                enabled = true,
                intensity = 1f,
                blendPowerMode = GetDefaultBlendPower(field),
                blendColorMode = GetDefaultBlendColor(field),
            };

            fields.Add(fieldRecord);

            return fieldRecord;
        }
    }
}
