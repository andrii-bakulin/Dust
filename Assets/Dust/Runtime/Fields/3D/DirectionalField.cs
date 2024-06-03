using System;
using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Fields/3D Fields/Directional Field")]
    public class DirectionalField : SpaceObjectField
    {
        [SerializeField]
        private float m_Length = 1.0f;
        public float length
        {
            get => m_Length;
            set => m_Length = NormalizeLength(value);
        }

        [SerializeField]
        private Axis6xDirection m_Direction = Axis6xDirection.XPlus;
        public Axis6xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_GizmoWidth = 4.0f;
        public float gizmoWidth
        {
            get => m_GizmoWidth;
            set => m_GizmoWidth = NormalizeGizmoWidth(value);
        }

        [SerializeField]
        private float m_GizmoHeight = 2.0f;
        public float gizmoHeight
        {
            get => m_GizmoHeight;
            set => m_GizmoHeight = NormalizeGizmoHeight(value);
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, length);
            DynamicState.Append(ref dynamicState, ++seq, direction);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Directional";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            result.power = 0f;

            Vector3 localPosition = transform.InverseTransformPoint(fieldPoint.inPosition);

            float distanceToPoint = direction switch
            {
                Axis6xDirection.XPlus  => -localPosition.x,
                Axis6xDirection.XMinus => +localPosition.x,
                Axis6xDirection.YPlus  => -localPosition.y,
                Axis6xDirection.YMinus => +localPosition.y,
                Axis6xDirection.ZPlus  => -localPosition.z,
                Axis6xDirection.ZMinus => +localPosition.z,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (DuMath.IsNotZero(length))
            {
                float halfLength = length / 2f;
                result.power = DuMath.Fit(-halfLength, +halfLength, 0f, 1f, distanceToPoint);
            }
            else
            {
                result.power = distanceToPoint >= 0f ? +1f : -1f;
            }

            result.power = 1f - result.power;

            if (!unlimited)
                result.power = Mathf.Clamp01(result.power);

            result.power *= power;

            result.power = remapping.MapValue(result.power);
            result.color = calculateColor ? coloring.GetColor(result.power) : Color.clear;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            float innerMinScale = 1f - remapping.inMin;
            float innerMaxScale = 1f - remapping.inMax;

            float halfLength = length / 2f;

            Vector3 plainSize = AxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(0.001f, gizmoHeight, gizmoWidth));
            Vector3 offsetPlane0 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-halfLength, 0f, 0f));
            Vector3 offsetPlane1 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(+halfLength, 0f, 0f));

            Gizmos.matrix = transform.localToWorldMatrix;

            Color colorRange0 = GetGizmoColorRange0();
            Color colorRange1 = GetGizmoColorRange1();

            if (remapping.remapPowerEnabled)
            {
                if (innerMinScale > 0f && innerMaxScale > 0f)
                {
                    // End plane
                    Gizmos.color = GetGizmoColorDefaultShape();
                    Gizmos.DrawWireCube(offsetPlane1, plainSize);
                }

                if (innerMinScale < 1f && innerMaxScale < 1f)
                {
                    // Begin plane
                    Gizmos.color = GetGizmoColorDefaultShape();
                    Gizmos.DrawWireCube(offsetPlane0, plainSize);
                }

                if (!Mathf.Approximately(innerMinScale, innerMaxScale))
                {
                    // Middle plane - MAX
                    Gizmos.color = !remapping.invert ? colorRange1 : colorRange0;
                    Gizmos.DrawWireCube(DuVector3.Fit01To(offsetPlane0, offsetPlane1, 1f - innerMaxScale), plainSize);
                }

                // Middle plane - MIN
                Gizmos.color = !remapping.invert ? colorRange0 : colorRange1;
                Gizmos.DrawWireCube(DuVector3.Fit01To(offsetPlane0, offsetPlane1, 1f - innerMinScale), plainSize);
            }
            else
            {
                // End plane
                Gizmos.color = colorRange0;
                Gizmos.DrawWireCube(offsetPlane0, plainSize);

                // Begin plane
                Gizmos.color = colorRange1;
                Gizmos.DrawWireCube(offsetPlane1, plainSize);
            }

            // 3: Draw arrow
            float arrowSign = remapping.remapPowerEnabled && remapping.invert ? -1f : +1f;
            Gizmos.color = colorRange1;
            Gizmos.DrawRay(offsetPlane0 * arrowSign, (offsetPlane1 - offsetPlane0) * arrowSign);
            Gizmos.DrawRay(offsetPlane1 * arrowSign, AxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-0.2f, 0f, +0.06f) * halfLength) * arrowSign);
            Gizmos.DrawRay(offsetPlane1 * arrowSign, AxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-0.2f, 0f, -0.06f) * halfLength) * arrowSign);
            Gizmos.DrawRay(offsetPlane1 * arrowSign, AxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-0.2f, +0.06f, 0f) * halfLength) * arrowSign);
            Gizmos.DrawRay(offsetPlane1 * arrowSign, AxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-0.2f, -0.06f, 0f) * halfLength) * arrowSign);
        }

        protected override void DrawFieldGizmo(float scale)
        {
        }

        private void Reset()
        {
            ResetDefaults();
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeLength(float value)
        {
            return Mathf.Max(0f, value);
        }

        public static float NormalizeGizmoWidth(float value)
        {
            return Mathf.Abs(value);
        }

        public static float NormalizeGizmoHeight(float value)
        {
            return Mathf.Abs(value);
        }
    }
}
