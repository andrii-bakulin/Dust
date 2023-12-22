using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class ActionWithCallbacksEditor : ActionEditor
    {
        protected DuProperty m_OnCompleteCallback;
        protected DuProperty m_OnCompleteActions;

        //--------------------------------------------------------------------------------------------------------------
        
        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_OnCompleteCallback = FindProperty("m_OnCompleteCallback", "Callback");
            m_OnCompleteActions = FindProperty("m_OnCompleteActions", "On Complete Start Actions");
        }

        //--------------------------------------------------------------------------------------------------------------
        
        protected virtual void OnInspectorGUI_Callbacks(string actionId)
            => OnInspectorGUI_Callbacks(actionId, false);

        protected virtual void OnInspectorGUI_Callbacks(string actionId, bool callbackExpanded)
        {
            if (DustGUI.FoldoutBegin("On Complete Callback", actionId + ".Callback", this, callbackExpanded))
            {
                PropertyField(m_OnCompleteCallback);
            }
            DustGUI.FoldoutEnd();

            PropertyField(m_OnCompleteActions, $"{m_OnCompleteActions.title} ({m_OnCompleteActions.property.arraySize})");
        }
    }
}
