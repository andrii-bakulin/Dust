using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    public abstract class BasicFieldEditor : FieldEditor
    {
        protected DuProperty m_Power;

        protected RemappingEditor m_RemappingEditor;
        protected ColoringEditor m_ColoringEditor;

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Power = FindProperty("m_Power", "Power");

            m_RemappingEditor = new RemappingEditor((target as BasicField).remapping, serializedObject.FindProperty("m_Remapping"));
            m_ColoringEditor = new ColoringEditor(serializedObject.FindProperty("m_Coloring"));
        }

        protected void OnInspectorGUI_RemappingBlock()
        {
            m_RemappingEditor.OnInspectorGUI();
        }

        protected void OnInspectorGUI_ColoringBlock()
        {
            m_ColoringEditor.OnInspectorGUI();
        }
    }
}
