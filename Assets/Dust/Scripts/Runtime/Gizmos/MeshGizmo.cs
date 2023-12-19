using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Mesh Gizmo")]
    public class MeshGizmo : AbstractGizmo
    {
        [SerializeField]
        private Mesh m_Mesh;
        public Mesh mesh
        {
            get => m_Mesh;
            set => m_Mesh = value;
        }

        [SerializeField]
        private Vector3 m_Position = Vector3.zero;
        public Vector3 position
        {
            get => m_Position;
            set => m_Position = value;
        }

        [SerializeField]
        private Vector3 m_Rotation = Vector3.zero;
        public Vector3 rotation
        {
            get => m_Rotation;
            set => m_Rotation = value;
        }

        [SerializeField]
        private Vector3 m_Scale = Vector3.one;
        public Vector3 scale
        {
            get => m_Scale;
            set => m_Scale = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string GizmoName()
        {
            return "Mesh";
        }

#if UNITY_EDITOR
        protected override void DrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = color;
            Gizmos.DrawWireMesh(mesh, position, Quaternion.Euler(rotation), scale);
        }
#endif
    }
}
