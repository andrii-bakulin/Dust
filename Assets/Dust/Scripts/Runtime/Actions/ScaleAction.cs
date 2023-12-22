using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    public abstract class ScaleAction : IntervalWithRollbackAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

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

        protected Vector3 m_ScaleStart;
        protected Vector3 m_ScaleFinal;
        
        protected Vector3 m_ScaleLast;

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle
        
        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(activeTargetTransform))
                return;

            var scaleNext = playingPhase == PlayingPhase.Main
                ? Vector3.Lerp(m_ScaleStart, m_ScaleFinal, playbackState)
                : Vector3.Lerp(m_ScaleFinal, m_ScaleStart, playbackState);

            var scaleDiff = scaleNext - m_ScaleLast;

            if (space == Space.World)
            {
                DuTransform.SetGlobalScale(activeTargetTransform, activeTargetTransform.lossyScale + scaleDiff);
            }
            else if (space == Space.Local)
            {
                activeTargetTransform.localScale += scaleDiff;
            }

            m_ScaleLast = scaleNext;
        }
    }
}
