using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Fields/3D Fields/Cylinder Field")]
    public class CylinderField : SpaceObjectField
    {
        [SerializeField]
        private float m_Radius = 1.0f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = NormalizeRadius(value);
        }

        [SerializeField]
        private float m_Height = 2.0f;
        public float height
        {
            get => m_Height;
            set => m_Height = NormalizeHeight(value);
        }

        [SerializeField]
        private Axis3xDirection m_Direction = Axis3xDirection.Y;
        public Axis3xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, radius);
            DynamicState.Append(ref dynamicState, ++seq, height);
            DynamicState.Append(ref dynamicState, ++seq, direction);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Cylinder";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            result.power = 0f;

            if (radius > 0f && height > 0f && DuVector3.IsAllAxisNonZero(transform.localScale))
            {
                Vector3 localPosition = transform.InverseTransformPoint(fieldPoint.inPosition);

                // Convert to [X+]-axis-space by direction
                localPosition = AxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

                float distanceToPoint = localPosition.magnitude;
                float distanceToEdge = DuMath.Cylinder.DistanceToEdge(radius, height, localPosition);

                result.power = 1f - (distanceToEdge > 0f ? distanceToPoint / distanceToEdge : 0f);
                
                if (!unlimited)
                    result.power = Mathf.Clamp01(result.power);

                result.power *= power;
            }

            result.power = remapping.MapValue(result.power);
            result.color = calculateColor ? coloring.GetColor(result.power) : Color.clear;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmo(float scale)
        {
            DuGizmos.DrawWireCylinder(radius * scale, height * scale, Vector3.zero, direction, 32, 4);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeHeight(float value)
        {
            return Mathf.Abs(value);
        }

        public static float NormalizeRadius(float value)
        {
            return Mathf.Abs(value);
        }
    }
}
