using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ValueFloatAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ValueFloatActionEditor : IntervalActionEditor
    {
        protected DuProperty m_ValueStart;
        protected DuProperty m_ValueEnd;

        protected DuProperty m_OnUpdate;

        //--------------------------------------------------------------------------------------------------------------

        static ValueFloatActionEditor()
        {
            ActionsPopupButtons.AddActionOthers(typeof(ValueFloatAction), "Value Float");
        }

        [MenuItem("Dust/Actions/Values/Value Float")]
        [MenuItem("GameObject/Dust/Actions/Values/Value Float")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Value Float Action", typeof(ValueFloatAction));
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

            OnInspectorGUI_Callbacks("ValueFloatAction");
            OnInspectorGUI_Extended("ValueFloatAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
