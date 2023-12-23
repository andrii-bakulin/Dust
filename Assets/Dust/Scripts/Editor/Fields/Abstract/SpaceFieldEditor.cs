using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class SpaceFieldEditor : FieldEditor
    {
        protected RemappingEditor m_RemappingEditor;

        protected DuProperty m_GizmoVisibility;
        protected DuProperty m_GizmoFieldColor;

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_RemappingEditor = new RemappingEditor((target as SpaceField).remapping, serializedObject.FindProperty("m_Remapping"));

            m_GizmoVisibility = FindProperty("m_GizmoVisibility", "Visibility");
            m_GizmoFieldColor = FindProperty("m_GizmoFieldColor", "Use Field Color");
        }

        protected void OnInspectorGUI_RemappingBlock()
        {
            m_RemappingEditor.OnInspectorGUI();
        }

        protected void OnInspectorGUI_GizmoBlock()
        {
            if (DustGUI.FoldoutBegin("Gizmo", "DuAnyField.Gizmo"))
            {
                PropertyField(m_GizmoVisibility);
                PropertyField(m_GizmoFieldColor);
                Space();
            }
            DustGUI.FoldoutEnd();
        }
    }
}
