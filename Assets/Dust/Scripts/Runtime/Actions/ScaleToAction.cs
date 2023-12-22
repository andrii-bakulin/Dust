using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/ScaleTo Action")]
    public class ScaleToAction : ScaleAction
    {
        [SerializeField]
        private Vector3 m_ScaleTo = Vector3.one;
        public Vector3 scaleTo
        {
            get => m_ScaleTo;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ScaleTo = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionStart()
        {
            base.OnActionStart();

            if (Dust.IsNull(activeTargetTransform))
                return;

            if (space == Space.World)
                m_ScaleStart = activeTargetTransform.lossyScale;
            else if (space == Space.Local)
                m_ScaleStart = activeTargetTransform.localScale;

            m_ScaleFinal = scaleTo;
            
            // for 1st Update I should decrease Next value for current scale-value
            m_ScaleLast = m_ScaleStart;
        }
    }
}
