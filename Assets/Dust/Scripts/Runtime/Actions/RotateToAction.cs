using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/RotateTo Action")]
    public class RotateToAction : IntervalWithRollbackAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_RotateTo = Vector3.zero;
        public Vector3 rotateTo
        {
            get => m_RotateTo;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_RotateTo = value;
            }
        }

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Space = value;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        
        protected Quaternion m_RotationStart;
        protected Quaternion m_RotationFinal;

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionStart()
        {
            base.OnActionStart();

            if (space == Space.World)
            {
                m_RotationStart = activeTargetTransform.rotation;
            }
            else if (space == Space.Local)
            {
                m_RotationStart = activeTargetTransform.localRotation;
            }
             
            m_RotationFinal = Quaternion.Euler(rotateTo);
        }

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(activeTargetTransform))
                return;

            var lerpOffset = 1f;
            var rotateEndPoint = playingPhase == PlayingPhase.Main ? m_RotationFinal : m_RotationStart;

            if (playingPhase == PlayingPhase.Main)
            {
                if (duration > 0f && playbackState < 1f)
                    lerpOffset = deltaTime / ((1f - playbackState) * duration);
            }
            else
            {
                if (rollbackDuration > 0f && playbackState < 1f)
                    lerpOffset = deltaTime / ((1f - playbackState) * rollbackDuration);
            }

            if (space == Space.World)
            {
                activeTargetTransform.rotation = Quaternion.Slerp(activeTargetTransform.rotation, rotateEndPoint, lerpOffset);
            }
            else if (space == Space.Local)
            {
                activeTargetTransform.localRotation = Quaternion.Slerp(activeTargetTransform.localRotation, rotateEndPoint, lerpOffset);
            }
        }
    }
}
