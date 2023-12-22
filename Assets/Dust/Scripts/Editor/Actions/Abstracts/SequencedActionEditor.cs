using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class SequencedActionEditor : ActionEditor
    {
        protected DuProperty m_OnCompleteActions;

        //--------------------------------------------------------------------------------------------------------------
        
        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_OnCompleteActions = FindProperty("m_OnCompleteActions", "On Complete Start Next Actions");
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnInspectorGUI_Callbacks(string actionId)
        {
            PropertyField(m_OnCompleteActions);
        }
    }
}
