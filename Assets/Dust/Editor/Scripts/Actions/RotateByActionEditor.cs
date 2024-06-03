using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(RotateByAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class RotateByActionEditor : IntervalWithRollbackActionEditor
    {
        protected DuProperty m_RotateBy;
        protected DuProperty m_Space;

        protected DuProperty m_ImproveAccuracy;
        protected DuProperty m_ImproveAccuracyThreshold;
        protected DuProperty m_ImproveAccuracyMaxIterations;

        //--------------------------------------------------------------------------------------------------------------

        static RotateByActionEditor()
        {
            ActionsPopupButtons.AddActionAnimate(typeof(RotateByAction), "RotateBy");
        }

        [MenuItem("Dust/Actions/RotateBy")]
        [MenuItem("GameObject/Dust/Actions/RotateBy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("RotateBy Action", typeof(RotateByAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_RotateBy = FindProperty("m_RotateBy", "Rotate By");
            m_Space = FindProperty("m_Space", "Space");
            
            m_ImproveAccuracy = FindProperty("m_ImproveAccuracy", "Enabled");
            m_ImproveAccuracyThreshold = FindProperty("m_ImproveAccuracyThreshold", "Threshold");
            m_ImproveAccuracyMaxIterations = FindProperty("m_ImproveAccuracyMaxIterations", "Max Iterations");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_RotateBy);
            OnInspectorGUI_Duration();
            PropertyField(m_Space);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("RotateByAction");
            OnInspectorGUI_Extended("RotateByAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Improve Accuracy", "RotateByAction.Accuracy", false))
            {
                PropertyField(m_ImproveAccuracy);
                
                if (!m_ImproveAccuracy.IsTrue)
                    DustGUI.Lock();

                PropertyExtendedSlider(m_ImproveAccuracyThreshold, 0.01f, 5.0f, +0.01f, 0.01f);
                PropertyExtendedIntSlider(m_ImproveAccuracyMaxIterations, 1, 25, 1, 1, 1000);
                
                DustGUI.Unlock();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_ImproveAccuracyThreshold.isChanged)
                m_ImproveAccuracyThreshold.valFloat =
                    RotateByAction.NormalizeAccuracyThreshold(m_ImproveAccuracyThreshold.valFloat);

            if (m_ImproveAccuracyMaxIterations.isChanged)
                m_ImproveAccuracyMaxIterations.valInt =
                    RotateByAction.NormalizeAccuracyMaxIterations(m_ImproveAccuracyMaxIterations.valInt);

            InspectorCommitUpdates();
        }
    }
}
