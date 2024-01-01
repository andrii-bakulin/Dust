using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/3D Fields/Cube Field")]
    public class CubeField : Space3DField
    {
        internal class Calc
        {
            internal Ray ray;
            internal Plane planeX;
            internal Plane planeY;
            internal Plane planeZ;
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_Size = Vector3.one;
        public Vector3 size
        {
            get => m_Size;
            set
            {
                m_Size = NormalizeSize(value);
                ResetCalcData();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private Calc m_Calc = null;

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, size);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Cube";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            if (Dust.IsNull(m_Calc))
            {
                Vector3 halfSize = size / 2f;

                m_Calc = new Calc();

                m_Calc.ray.origin = Vector3.zero;
                m_Calc.planeX.Set3Points(new Vector3(halfSize.x, 0, 0), new Vector3(halfSize.x, halfSize.y, 0), new Vector3(halfSize.x, halfSize.y, halfSize.z));
                m_Calc.planeY.Set3Points(new Vector3(0, halfSize.y, 0), new Vector3(0, halfSize.y, halfSize.z), new Vector3(halfSize.x, halfSize.y, halfSize.z));
                m_Calc.planeZ.Set3Points(new Vector3(0, 0, halfSize.z), new Vector3(halfSize.x, 0, halfSize.z), new Vector3(halfSize.x, halfSize.y, halfSize.z));
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            float offset = 0f;

            if (DuVector3.IsAllAxisNonZero(size) && DuVector3.IsAllAxisNonZero(transform.localScale))
            {
                Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

                float distanceToPoint = localPosition.magnitude;
                float distanceToEdge = Mathf.Infinity;

                m_Calc.ray.direction = DuVector3.Abs(localPosition);

                float distanceToPlane;

                if (m_Calc.planeX.Raycast(m_Calc.ray, out distanceToPlane) && distanceToPlane > 0)
                    distanceToEdge = Mathf.Min(distanceToEdge, distanceToPlane);

                if (m_Calc.planeY.Raycast(m_Calc.ray, out distanceToPlane) && distanceToPlane > 0)
                    distanceToEdge = Mathf.Min(distanceToEdge, distanceToPlane);

                if (m_Calc.planeZ.Raycast(m_Calc.ray, out distanceToPlane) && distanceToPlane > 0)
                    distanceToEdge = Mathf.Min(distanceToEdge, distanceToPlane);

                offset = 1f - (distanceToEdge > 0f ? distanceToPoint / distanceToEdge : 0f);
            }

            result.power = remapping.MapValue(offset);
            result.color = GetFieldColorFromRemapping(remapping, result.power, calculateColor);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void ResetCalcData()
        {
            m_Calc = null;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmo(float scale)
        {
            Gizmos.DrawWireCube(Vector3.zero, size * scale);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static Vector3 NormalizeSize(Vector3 value)
        {
            return DuVector3.Abs(value);
        }
    }
}
