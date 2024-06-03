using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Fields/3D Fields/Sphere Field")]
    public class SphereField : SpaceObjectField
    {
        [SerializeField]
        private float m_Radius = 1.0f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = NormalizeRadius(value);
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, radius);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Sphere";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            result.power = 0f;

            if (radius > 0f && DuVector3.IsAllAxisNonZero(transform.localScale))
            {
                Vector3 localPosition = transform.InverseTransformPoint(fieldPoint.inPosition);

                float distanceToPoint = localPosition.magnitude;
                float distanceToEdge = radius;

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
            Gizmos.DrawWireSphere(Vector3.zero, radius * scale);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeRadius(float value)
        {
            return Mathf.Abs(value);
        }
    }
}
