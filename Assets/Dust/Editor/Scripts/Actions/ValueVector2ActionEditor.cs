using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ValueVector2Action))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ValueVector2ActionEditor : IntervalActionEditor
    {
        protected DuProperty m_ValueStart;
        protected DuProperty m_ValueEnd;

        protected DuProperty m_OnUpdate;

        //--------------------------------------------------------------------------------------------------------------

        static ValueVector2ActionEditor()
        {
            ActionsPopupButtons.AddActionOthers(typeof(ValueVector2Action), "Value Vector2");
        }

        [MenuItem("Dust/Actions/Values/Value Vector2")]
        [MenuItem("GameObject/Dust/Actions/Values/Value Vector2")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Value Vector2 Action", typeof(ValueVector2Action));
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

            OnInspectorGUI_Callbacks("ValueVector2Action");
            OnInspectorGUI_Extended("ValueVector2Action");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
