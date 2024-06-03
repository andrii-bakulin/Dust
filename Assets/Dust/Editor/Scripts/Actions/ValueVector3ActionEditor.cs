using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ValueVector3Action))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ValueVector3ActionEditor : IntervalActionEditor
    {
        protected DuProperty m_ValueStart;
        protected DuProperty m_ValueEnd;

        protected DuProperty m_OnUpdate;

        //--------------------------------------------------------------------------------------------------------------

        static ValueVector3ActionEditor()
        {
            ActionsPopupButtons.AddActionOthers(typeof(ValueVector3Action), "Value Vector3");
        }

        [MenuItem("Dust/Actions/Values/Value Vector3")]
        [MenuItem("GameObject/Dust/Actions/Values/Value Vector3")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Value Vector3 Action", typeof(ValueVector3Action));
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

            OnInspectorGUI_Callbacks("ValueVector3Action");
            OnInspectorGUI_Extended("ValueVector3Action");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
