using UnityEngine;

namespace DustEngine
{
    public abstract class MoveAction : IntervalWithRollbackAction
    {
        protected Vector3 m_DeltaLocalMove;

        protected bool m_AutoRotateBySelfDirection = false;

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionStart()
        {
            base.OnActionStart();

            m_AutoRotateBySelfDirection = false;
        }

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(activeTargetTransform))
                return;
            
            Vector3 moveStep = m_DeltaLocalMove * (playbackState - previousState);

            if (m_AutoRotateBySelfDirection)
                moveStep = DuMath.RotatePoint(moveStep, activeTargetTransform.localEulerAngles);

            if (playingPhase == PlayingPhase.Main)
                activeTargetTransform.localPosition += moveStep;
            else
                activeTargetTransform.localPosition -= moveStep;
        }
    }
}
