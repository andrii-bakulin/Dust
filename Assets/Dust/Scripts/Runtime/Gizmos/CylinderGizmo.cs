using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Cylinder Gizmo")]
    public class CylinderGizmo : AbstractGizmo
    {
        [SerializeField]
        private float m_Radius = 1f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = value;
        }

        [SerializeField]
        private float m_Height = 2f;
        public float height
        {
            get => m_Height;
            set => m_Height = value;
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
            return "Cylinder";
        }

#if UNITY_EDITOR
        protected override void DrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = color;

            DuGizmos.DrawWireCylinder(radius, height, center, direction, 64, 4);
        }
#endif
    }
}
