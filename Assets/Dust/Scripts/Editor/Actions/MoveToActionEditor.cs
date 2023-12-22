using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(MoveToAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class MoveToActionEditor : IntervalWithRollbackActionEditor
    {
        private DuProperty m_MoveTo;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static MoveToActionEditor()
        {
            ActionsPopupButtons.AddActionAnimate(typeof(MoveToAction), "MoveTo");
        }

        [MenuItem("Dust/Actions/MoveTo")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("MoveTo Action", typeof(MoveToAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_MoveTo = FindProperty("m_MoveTo", "Move To");
            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_MoveTo);
            OnInspectorGUI_Duration();
            PropertyField(m_Space);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("MoveToAction");
            OnInspectorGUI_Extended("MoveToAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
