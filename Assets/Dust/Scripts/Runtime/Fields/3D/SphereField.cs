using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/3D Fields/Sphere Field")]
    public class SphereField : Space3DField
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
            float offset = 0f;

            if (radius > 0f && DuVector3.IsAllAxisNonZero(transform.localScale))
            {
                Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

                float distanceToPoint = localPosition.magnitude;
                float distanceToEdge = radius;

                offset = 1f - (distanceToEdge > 0f ? distanceToPoint / distanceToEdge : 0f);
            }

            result.power = remapping.MapValue(offset);
            result.color = GetFieldColorFromRemapping(remapping, result.power, calculateColor);
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
