using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ValueGradientAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ValueGradientActionEditor : IntervalActionEditor
    {
        protected DuProperty m_Gradient;

        protected DuProperty m_OnUpdate;

        //--------------------------------------------------------------------------------------------------------------

        static ValueGradientActionEditor()
        {
            ActionsPopupButtons.AddActionOthers(typeof(ValueGradientAction), "Value Gradient");
        }

        [MenuItem("Dust/Actions/Values/Value Gradient")]
        [MenuItem("GameObject/Dust/Actions/Values/Value Gradient")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Value Gradient Action", typeof(ValueGradientAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Gradient = FindProperty("m_Gradient", "Gradient");
            
            m_OnUpdate = FindProperty("m_OnUpdate", "OnUpdate");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_Gradient);
            OnInspectorGUI_Duration();

            Space();
            
            PropertyField(m_OnUpdate);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("ValueGradientAction");
            OnInspectorGUI_Extended("ValueGradientAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
