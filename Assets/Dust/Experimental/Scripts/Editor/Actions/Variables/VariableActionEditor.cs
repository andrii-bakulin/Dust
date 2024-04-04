using Dust.Experimental.Variables;
using UnityEngine;
using UnityEditor;

namespace Dust.Experimental.Editor
{
    public abstract class VariableActionEditor : DustEditor.InstantActionEditor
    {
        protected DuProperty m_Scope;
        protected DuProperty m_ReferenceObject;

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Scope = FindProperty("m_Scope", "Scope");
            m_ReferenceObject = FindProperty("m_ReferenceObject", "Reference Object");
        }

        protected override void OnInspectorGUI_BaseControlUI()
        {
            base.OnInspectorGUI_BaseControlUI();

            PropertyField(m_Scope);
            
            if ((VariableAction.Scope)m_Scope.valInt == VariableAction.Scope.Object)
                PropertyField(m_ReferenceObject);
            
            Space();
        }
    }
}
