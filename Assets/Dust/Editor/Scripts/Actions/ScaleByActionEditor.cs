using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ScaleByAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ScaleByActionEditor : IntervalWithRollbackActionEditor
    {
        protected DuProperty m_ScaleBy;
        protected DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static ScaleByActionEditor()
        {
            ActionsPopupButtons.AddActionAnimate(typeof(ScaleByAction), "ScaleBy");
        }

        [MenuItem("Dust/Actions/ScaleBy")]
        [MenuItem("GameObject/Dust/Actions/ScaleBy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("ScaleBy Action", typeof(ScaleByAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ScaleBy = FindProperty("m_ScaleBy", "Scale By");
            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_ScaleBy);
            OnInspectorGUI_Duration();
            PropertyField(m_Space);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("ScaleByAction");
            OnInspectorGUI_Extended("ScaleByAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
