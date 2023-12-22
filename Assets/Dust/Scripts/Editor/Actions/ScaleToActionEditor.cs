using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(ScaleToAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ScaleToActionEditor : IntervalWithRollbackActionEditor
    {
        private DuProperty m_ScaleTo;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static ScaleToActionEditor()
        {
            ActionsPopupButtons.AddActionAnimate(typeof(ScaleToAction), "ScaleTo");
        }

        [MenuItem("Dust/Actions/ScaleTo")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("ScaleTo Action", typeof(ScaleToAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ScaleTo = FindProperty("m_ScaleTo", "Scale To");
            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_ScaleTo);
            OnInspectorGUI_Duration();
            PropertyField(m_Space);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("ScaleToAction");
            OnInspectorGUI_Extended("ScaleToAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
