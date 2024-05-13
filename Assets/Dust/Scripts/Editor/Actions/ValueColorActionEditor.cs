using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ValueColorAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ValueColorActionEditor : IntervalActionEditor
    {
        protected DuProperty m_ValueStart;
        protected DuProperty m_ValueEnd;

        protected DuProperty m_OnUpdate;

        //--------------------------------------------------------------------------------------------------------------

        static ValueColorActionEditor()
        {
            ActionsPopupButtons.AddActionOthers(typeof(ValueColorAction), "Value Color");
        }

        [MenuItem("Dust/Actions/Values/Value Color")]
        [MenuItem("GameObject/Dust/Actions/Values/Value Color")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Value Color Action", typeof(ValueColorAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ValueStart = FindProperty("m_ValueStart", "Start");
            m_ValueEnd = FindProperty("m_ValueEnd", "End");
            
            m_OnUpdate = FindProperty("m_OnUpdate", "OnUpdate");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_ValueStart);
            PropertyField(m_ValueEnd);
            OnInspectorGUI_Duration();

            Space();
            
            PropertyField(m_OnUpdate);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("ValueColorAction");
            OnInspectorGUI_Extended("ValueColorAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
