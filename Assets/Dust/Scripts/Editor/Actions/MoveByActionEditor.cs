using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(MoveByAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class MoveByActionEditor : IntervalWithRollbackActionEditor
    {
        private DuProperty m_MoveBy;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static MoveByActionEditor()
        {
            ActionsPopupButtons.AddActionAnimate(typeof(MoveByAction), "MoveBy");
        }

        [MenuItem("Dust/Actions/MoveBy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("MoveBy Action", typeof(MoveByAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_MoveBy = FindProperty("m_MoveBy", "Move By");
            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_MoveBy);
            OnInspectorGUI_Duration();
            PropertyField(m_Space);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("MoveByAction");
            OnInspectorGUI_Extended("MoveByAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
