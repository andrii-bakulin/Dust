using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class BasicFieldEditor : FieldEditor
    {
        protected DuProperty m_Power;
        protected DuProperty m_Unlimited;

        protected RemappingEditor m_RemappingEditor;

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Power = FindProperty("m_Power", "Power");
            m_Unlimited = FindProperty("m_Unlimited", "Unlimited");

            m_RemappingEditor = new RemappingEditor((target as BasicField).remapping, serializedObject.FindProperty("m_Remapping"));
        }

        protected void OnInspectorGUI_RemappingBlock()
            => OnInspectorGUI_RemappingBlock(true); 

        protected void OnInspectorGUI_RemappingBlock(bool showColorBlock)
        {
            m_RemappingEditor.OnInspectorGUI(showColorBlock);
        }
    }
}
