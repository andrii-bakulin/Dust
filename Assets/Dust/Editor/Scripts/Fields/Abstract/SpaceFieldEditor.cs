using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    public abstract class SpaceFieldEditor : BasicFieldEditor
    {
        protected DuProperty m_GizmoVisibility;
        protected DuProperty m_GizmoFieldColor;

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_GizmoVisibility = FindProperty("m_GizmoVisibility", "Visibility");
            m_GizmoFieldColor = FindProperty("m_GizmoFieldColor", "Use Field Color");
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
