using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(CubeGizmo))]
    [CanEditMultipleObjects]
    public class CubeGizmoEditor : AbstractGizmoEditor
    {
        private DuProperty m_Size;
        private DuProperty m_Center;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Cube")]
        public static void AddComponentToSelectedObjects()
        {
            AddGizmoToSelectedOrNewObject(typeof(CubeGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Size = FindProperty("m_Size", "Size");
            m_Center = FindProperty("m_Center", "Center");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_Size);
            PropertyField(m_Center);
            Space();
            PropertyField(m_Color);
            PropertyField(m_GizmoVisibility);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
