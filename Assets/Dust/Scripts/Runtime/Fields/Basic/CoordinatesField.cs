using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Basic Fields/Coordinates Field")]
    public class CoordinatesField : Field
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
        private float m_PowerMin = 0;
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

            DynamicState.Append(ref dynamicState, ++seq, transform);

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
                switch (powerAggregate)
                {
                    case AggregateMode.Avg: hint = "AVG( " + string.Join(", ", items) + " )"; break;
                    case AggregateMode.Min: hint = "MIN( " + string.Join(", ", items) + " )"; break;
                    case AggregateMode.Max: hint = "MAX( " + string.Join(", ", items) + " )"; break;
                    case AggregateMode.Sum: hint = "SUM( " + string.Join(", ", items) + " )"; break;
                    default:
                        break;
                }
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
            Vector3 localPoint = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

            if (powerEnabled)
            {
                localPoint = DuVector3.Abs(localPoint);

                float value = 0f;

                if (powerUseAxisX && powerUseAxisY && powerUseAxisZ)
                {
                    switch (powerAggregate)
                    {
                        default:
                        case AggregateMode.Avg: value = (localPoint.x + localPoint.y + localPoint.z) / 3f; break;
                        case AggregateMode.Min: value = Mathf.Min(Mathf.Min(localPoint.x, localPoint.y), localPoint.z); break;
                        case AggregateMode.Max: value = Mathf.Max(Mathf.Max(localPoint.x, localPoint.y), localPoint.z); break;
                        case AggregateMode.Sum: value = localPoint.x + localPoint.y + localPoint.z; break;
                    }
                }
                else if (powerUseAxisX && powerUseAxisY)
                {
                    switch (powerAggregate)
                    {
                        default:
                        case AggregateMode.Avg: value = (localPoint.x + localPoint.y) / 2f; break;
                        case AggregateMode.Min: value = Mathf.Min(localPoint.x, localPoint.y); break;
                        case AggregateMode.Max: value = Mathf.Max(localPoint.x, localPoint.y); break;
                        case AggregateMode.Sum: value = localPoint.x + localPoint.y; break;
                    }
                }
                else if (powerUseAxisY && powerUseAxisZ)
                {
                    switch (powerAggregate)
                    {
                        default:
                        case AggregateMode.Avg: value = (localPoint.y + localPoint.z) / 2f; break;
                        case AggregateMode.Min: value = Mathf.Min(localPoint.y, localPoint.z); break;
                        case AggregateMode.Max: value = Mathf.Max(localPoint.y, localPoint.z); break;
                        case AggregateMode.Sum: value = localPoint.y + localPoint.z; break;
                    }
                }
                else if (powerUseAxisX && powerUseAxisZ)
                {
                    switch (powerAggregate)
                    {
                        default:
                        case AggregateMode.Avg: value = (localPoint.x + localPoint.z) / 2f; break;
                        case AggregateMode.Min: value = Mathf.Min(localPoint.x, localPoint.z); break;
                        case AggregateMode.Max: value = Mathf.Max(localPoint.x, localPoint.z); break;
                        case AggregateMode.Sum: value = localPoint.x + localPoint.z; break;
                    }
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

                result.fieldPower = RepackValueByShape(powerShape, value * powerScale, powerMin, powerMax);
            }
            else
            {
                result.fieldPower = 0f;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (calculateColor && colorEnabled && DuMath.IsNotZero(colorScale))
            {
                result.fieldColor = localPoint.duToColor();

                result.fieldColor.r = RepackValueByShape(colorShape, result.fieldColor.r / colorScale, 0f, 1f);
                result.fieldColor.g = RepackValueByShape(colorShape, result.fieldColor.g / colorScale, 0f, 1f);
                result.fieldColor.b = RepackValueByShape(colorShape, result.fieldColor.b / colorScale, 0f, 1f);
                result.fieldColor.a = 1f;
            }
            else
            {
                result.fieldColor = Color.clear;
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
    }
}
