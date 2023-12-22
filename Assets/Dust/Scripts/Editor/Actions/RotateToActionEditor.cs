using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(RotateToAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class RotateToActionEditor : IntervalWithRollbackActionEditor
    {
        private DuProperty m_RotateTo;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static RotateToActionEditor()
        {
            ActionsPopupButtons.AddActionAnimate(typeof(RotateToAction), "RotateTo");
        }

        [MenuItem("Dust/Actions/RotateTo")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("RotateTo Action", typeof(RotateToAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_RotateTo = FindProperty("m_RotateTo", "Rotate To");
            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_RotateTo);
            OnInspectorGUI_Duration();
            PropertyField(m_Space);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("RotateToAction");
            OnInspectorGUI_Extended("RotateToAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
