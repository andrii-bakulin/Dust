using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(CubeGizmo))]
    [CanEditMultipleObjects]
    public class CubeGizmoEditor : AbstractGizmoEditor
    {
        protected DuProperty m_Size;
        protected DuProperty m_Center;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Cube")]
        [MenuItem("GameObject/Dust/Gizmos/Cube")]
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
