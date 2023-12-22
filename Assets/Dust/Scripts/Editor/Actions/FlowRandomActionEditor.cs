using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(FlowRandomAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class FlowRandomActionEditor : FlowActionEditor
    {
        private DuProperty m_Actions;

        private DuProperty m_Seed;

        //--------------------------------------------------------------------------------------------------------------

        static FlowRandomActionEditor()
        {
            ActionsPopupButtons.AddActionFlow(typeof(FlowRandomAction), "Flow Random");
        }

        [MenuItem("Dust/Actions/Flow Random")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Flow Random Action", typeof(FlowRandomAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Actions = FindProperty("m_Actions", "Actions");

            m_Seed = FindProperty("m_Seed");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_Actions);

            Space();

            PropertySeedRandomOrFixed(m_Seed);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Extended("FlowRandomAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
