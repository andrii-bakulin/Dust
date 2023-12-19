using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(MeshGizmo))]
    [CanEditMultipleObjects]
    public class MeshGizmoEditor : AbstractGizmoEditor
    {
        private DuProperty m_Mesh;
        private DuProperty m_Position;
        private DuProperty m_Rotation;
        private DuProperty m_Scale;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Mesh")]
        public static void AddComponentToSelectedObjects()
        {
            AddGizmoToSelectedOrNewObject(typeof(MeshGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Mesh = FindProperty("m_Mesh", "Mesh");
            m_Position = FindProperty("m_Position", "Position");
            m_Rotation = FindProperty("m_Rotation", "Rotation");
            m_Scale = FindProperty("m_Scale", "Scale");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_Mesh);
            PropertyField(m_Position);
            PropertyField(m_Rotation);
            PropertyField(m_Scale);
            Space();
            PropertyField(m_Color);
            PropertyField(m_GizmoVisibility);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
