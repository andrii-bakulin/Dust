using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Torus Gizmo")]
    public class TorusGizmo : AbstractGizmo
    {
        [SerializeField]
        private float m_Radius = 2f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = value;
        }

        [SerializeField]
        private float m_Thickness = 0.5f;
        public float thickness
        {
            get => m_Thickness;
            set => m_Thickness = value;
        }

        [SerializeField]
        private Vector3 m_Center = Vector3.zero;
        public Vector3 center
        {
            get => m_Center;
            set => m_Center = value;
        }

        [SerializeField]
        private Axis3xDirection m_Direction = Axis3xDirection.Y;
        public Axis3xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string GizmoName()
        {
            return "Torus";
        }

#if UNITY_EDITOR
        protected override void DrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = color;

            DuGizmos.DrawWireTorus(radius, thickness, center, direction, 64, 32);
        }
#endif
    }
}
