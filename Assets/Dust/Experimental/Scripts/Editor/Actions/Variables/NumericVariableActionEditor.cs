using Dust.DustEditor;
using UnityEngine;
using UnityEditor;

namespace Dust.Experimental.Editor
{
    [CustomEditor(typeof(NumericVariableAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class NumericVariableActionEditor : VariableActionEditor
    {
        protected DuProperty m_VariableName;
        protected DuProperty m_Action;
        protected DuProperty m_Value;

        //--------------------------------------------------------------------------------------------------------------

        static NumericVariableActionEditor()
        {
            // ActionsPopupButtons.AddActionOthers(typeof(NumericVariableAction), "Numeric Variable");
        }

        [MenuItem("Dust/* Experimental/Actions/Numeric Variable")]
        [MenuItem("GameObject/Dust/* Experimental/Actions/Numeric Variable")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Numeric Variable Action", typeof(NumericVariableAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_VariableName = FindProperty("m_VariableName", "Variable Name");
            m_Action = FindProperty("m_Action", "Action");
            m_Value = FindProperty("m_Value", "Value");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_VariableName);
            PropertyField(m_Action);
            PropertyField(m_Value);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("NumericVariableAction");
            OnInspectorGUI_Extended("NumericVariableAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
