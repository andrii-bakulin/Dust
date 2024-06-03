using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Fields/3D Fields/Torus Field")]
    public class TorusField : SpaceObjectField
    {
        [SerializeField]
        private float m_Radius = 2f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = NormalizeRadius(value);
        }

        [SerializeField]
        private float m_Thickness = 0.5f;
        public float thickness
        {
            get => m_Thickness;
            set => m_Thickness = NormalizeThickness(value);
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
            DynamicState.Append(ref dynamicState, ++seq, thickness);
            DynamicState.Append(ref dynamicState, ++seq, direction);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Torus";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            result.power = 0f;

            if (radius > 0f && thickness > 0f && DuVector3.IsAllAxisNonZero(transform.localScale))
            {
                Vector3 localPosition = transform.InverseTransformPoint(fieldPoint.inPosition);

                // Convert to [X+]-axis-space by direction
                localPosition = AxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

                // Convert 3D point to 2D (x; y-&-z) -> (x; y)
                Vector2 localPoint2D = new Vector2(localPosition.x, DuMath.Length(localPosition.y, localPosition.z));
                localPoint2D.duAbs();

                // Move center to torus radius (center of thickness-radius)
                localPoint2D.y -= radius;

                float distanceToPoint = localPoint2D.magnitude;
                float distanceToEdge = thickness;

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
            DuGizmos.DrawWireTorus(radius, thickness * scale, Vector3.zero, direction, 64, 32);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeRadius(float value)
        {
            return Mathf.Abs(value);
        }

        public static float NormalizeThickness(float value)
        {
            return Mathf.Abs(value);
        }
    }
}
