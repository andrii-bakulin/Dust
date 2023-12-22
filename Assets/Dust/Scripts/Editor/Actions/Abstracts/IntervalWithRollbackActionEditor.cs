using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class IntervalWithRollbackActionEditor : IntervalActionEditor
    {
        protected DuProperty m_PlayRollback;
        protected DuProperty m_RollbackDuration;

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_PlayRollback = FindProperty("m_PlayRollback", "Play Rollback");
            m_RollbackDuration = FindProperty("m_RollbackDuration", "Rollback Duration");
        }

        protected override void OnInspectorGUI_Duration()
        {
            PropertyDurationField(m_Duration);
            
            PropertyField(m_PlayRollback);
            if (m_PlayRollback.IsTrue)
                PropertyDurationField(m_RollbackDuration);

            if (m_PlayRollback.IsTrue && DuMath.IsZero(m_Duration.valFloat) && DuMath.IsZero(m_RollbackDuration.valFloat))
                DustGUI.HelpBoxWarning("This action has rollback flag and both durations have zero-lengths, so action has no sense.");
        }

        protected override void InspectorCommitUpdates()
        {
            if (m_RollbackDuration.isChanged)
                m_RollbackDuration.valFloat = IntervalWithRollbackAction.NormalizeDuration(m_RollbackDuration.valFloat);

            base.InspectorCommitUpdates();
        }
    }
}
