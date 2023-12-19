using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Cube Gizmo")]
    public class CubeGizmo : AbstractGizmo
    {
        [SerializeField]
        private Vector3 m_Size = Vector3.one;
        public Vector3 size
        {
            get => m_Size;
            set => m_Size = value;
        }

        [SerializeField]
        private Vector3 m_Center = Vector3.zero;
        public Vector3 center
        {
            get => m_Center;
            set => m_Center = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string GizmoName()
        {
            return "Cube";
        }

#if UNITY_EDITOR
        protected override void DrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = color;
            Gizmos.DrawWireCube(center, size);
        }
#endif
    }
}
