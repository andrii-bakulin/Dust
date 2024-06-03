using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Fields/Basic Fields/Coordinates Field")]
    public class CoordinatesField : SpaceField
    {
        public enum ShapeMode
        {
            Linear = 0,
            Repeat = 1,
            PingPong = 2,
        }

        public enum AggregateMode
        {
            Avg = 0,
            Min = 1,
            Max = 2,
            Sum = 3,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_PowerEnabled = true;
        public bool powerEnabled
        {
            get => m_PowerEnabled;
            set => m_PowerEnabled = value;
        }

        [SerializeField]
        private bool m_PowerUseAxisX = true;
        public bool powerUseAxisX
        {
            get => m_PowerUseAxisX;
            set => m_PowerUseAxisX = value;
        }

        [SerializeField]
        private bool m_PowerUseAxisY = true;
        public bool powerUseAxisY
        {
            get => m_PowerUseAxisY;
            set => m_PowerUseAxisY = value;
        }

        [SerializeField]
        private bool m_PowerUseAxisZ = true;
        public bool powerUseAxisZ
        {
            get => m_PowerUseAxisZ;
            set => m_PowerUseAxisZ = value;
        }

        [SerializeField]
        private float m_PowerScale = 1f;
        public float powerScale
        {
            get => m_PowerScale;
            set => m_PowerScale = value;
        }

        [SerializeField]
        private AggregateMode m_PowerAggregate = AggregateMode.Max;
        public AggregateMode powerAggregate
        {
            get => m_PowerAggregate;
            set => m_PowerAggregate = value;
        }

        [SerializeField]
        private float m_PowerMin;
        public float powerMin
        {
            get => m_PowerMin;
            set => m_PowerMin = value;
        }

        [SerializeField]
        private float m_PowerMax = 1f;
        public float powerMax
        {
            get => m_PowerMax;
            set => m_PowerMax = value;
        }

        [SerializeField]
        private ShapeMode m_PowerShape = ShapeMode.PingPong;
        public ShapeMode powerShape
        {
            get => m_PowerShape;
            set => m_PowerShape = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ColorEnabled = true;
        public bool colorEnabled
        {
            get => m_ColorEnabled;
            set => m_ColorEnabled = value;
        }

        [SerializeField]
        private float m_ColorScale = 1f;
        public float colorScale
        {
            get => m_ColorScale;
            set => m_ColorScale = value;
        }

        [SerializeField]
        private ShapeMode m_ColorShape = ShapeMode.Linear;
        public ShapeMode colorShape
        {
            get => m_ColorShape;
            set => m_ColorShape = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, powerEnabled);
            DynamicState.Append(ref dynamicState, ++seq, colorEnabled);

            if (powerEnabled)
            {
                DynamicState.Append(ref dynamicState, ++seq, powerUseAxisX);
                DynamicState.Append(ref dynamicState, ++seq, powerUseAxisY);
                DynamicState.Append(ref dynamicState, ++seq, powerUseAxisZ);
                DynamicState.Append(ref dynamicState, ++seq, powerScale);
                DynamicState.Append(ref dynamicState, ++seq, powerAggregate);
                DynamicState.Append(ref dynamicState, ++seq, powerMin);
                DynamicState.Append(ref dynamicState, ++seq, powerMax);
                DynamicState.Append(ref dynamicState, ++seq, powerShape);
            }

            if (colorEnabled)
            {
                DynamicState.Append(ref dynamicState, ++seq, colorScale);
                DynamicState.Append(ref dynamicState, ++seq, colorShape);
            }

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Coordinates";
        }

        public override string FieldDynamicHint()
        {
            if (!powerEnabled)
                return "";

            var hint = "";
            var items = new List<string>();

            if (powerUseAxisX) items.Add("X");
            if (powerUseAxisY) items.Add("Y");
            if (powerUseAxisZ) items.Add("Z");

            if (items.Count > 1)
            {
                hint = powerAggregate switch
                {
                    AggregateMode.Avg => "AVG( " + string.Join(", ", items) + " )",
                    AggregateMode.Min => "MIN( " + string.Join(", ", items) + " )",
                    AggregateMode.Max => "MAX( " + string.Join(", ", items) + " )",
                    AggregateMode.Sum => "SUM( " + string.Join(", ", items) + " )",
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            else if (items.Count == 1)
            {
                hint = items[0];
            }

            if (hint != "")
                hint += " -> Power";

            return hint;
        }

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            Vector3 localPoint = transform.InverseTransformPoint(fieldPoint.inPosition);

            if (powerEnabled)
            {
                localPoint = DuVector3.Abs(localPoint);

                float value = 0f;

                if (powerUseAxisX && powerUseAxisY && powerUseAxisZ)
                {
                    value = powerAggregate switch
                    {
                        AggregateMode.Avg => (localPoint.x + localPoint.y + localPoint.z) / 3f,
                        AggregateMode.Min => Mathf.Min(Mathf.Min(localPoint.x, localPoint.y), localPoint.z),
                        AggregateMode.Max => Mathf.Max(Mathf.Max(localPoint.x, localPoint.y), localPoint.z),
                        AggregateMode.Sum => localPoint.x + localPoint.y + localPoint.z,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
                else if (powerUseAxisX && powerUseAxisY)
                {
                    value = powerAggregate switch
                    {
                        AggregateMode.Avg => (localPoint.x + localPoint.y) / 2f,
                        AggregateMode.Min => Mathf.Min(localPoint.x, localPoint.y),
                        AggregateMode.Max => Mathf.Max(localPoint.x, localPoint.y),
                        AggregateMode.Sum => localPoint.x + localPoint.y,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
                else if (powerUseAxisY && powerUseAxisZ)
                {
                    value = powerAggregate switch
                    {
                        AggregateMode.Avg => (localPoint.y + localPoint.z) / 2f,
                        AggregateMode.Min => Mathf.Min(localPoint.y, localPoint.z),
                        AggregateMode.Max => Mathf.Max(localPoint.y, localPoint.z),
                        AggregateMode.Sum => localPoint.y + localPoint.z,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
                else if (powerUseAxisX && powerUseAxisZ)
                {
                    value = powerAggregate switch
                    {
                        AggregateMode.Avg => (localPoint.x + localPoint.z) / 2f,
                        AggregateMode.Min => Mathf.Min(localPoint.x, localPoint.z),
                        AggregateMode.Max => Mathf.Max(localPoint.x, localPoint.z),
                        AggregateMode.Sum => localPoint.x + localPoint.z,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
                else if (powerUseAxisX)
                {
                    value = localPoint.x;
                }
                else if (powerUseAxisY)
                {
                    value = localPoint.y;
                }
                else if (powerUseAxisZ)
                {
                    value = localPoint.z;
                }

                result.power = RepackValueByShape(powerShape, value * powerScale, powerMin, powerMax);
            }
            else
            {
                result.power = 0f;
            }

            result.power *= power;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (calculateColor && colorEnabled && DuMath.IsNotZero(colorScale))
            {
                result.color = localPoint.duToColor();

                result.color.r = RepackValueByShape(colorShape, result.color.r / colorScale, 0f, 1f);
                result.color.g = RepackValueByShape(colorShape, result.color.g / colorScale, 0f, 1f);
                result.color.b = RepackValueByShape(colorShape, result.color.b / colorScale, 0f, 1f);
                result.color.a = 1f;
            }
            else
            {
                result.color = Color.clear;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public override bool IsAllowCalculateFieldColor()
        {
            return colorEnabled;
        }

#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return false;
        }

        public override Gradient GetFieldColorPreview(out float colorPower)
        {
            colorPower = 0f;
            return null;
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public float RepackValueByShape(ShapeMode shape, float value, float min, float max)
        {
            switch (shape)
            {
                default:
                case ShapeMode.Linear:
                    return value;

                case ShapeMode.Repeat:
                    if (min.Equals(max))
                        return 0f;

                    return min + Mathf.Repeat(value, max - min);

                case ShapeMode.PingPong:
                    if (min.Equals(max))
                        return 0f;

                    return min + Mathf.PingPong(value, max - min);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            // Nothing for now...
        }
#endif
    }
}
