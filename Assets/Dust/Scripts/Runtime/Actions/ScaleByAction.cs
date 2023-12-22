using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/ScaleBy Action")]
    public class ScaleByAction : ScaleAction
    {
        [SerializeField]
        private Vector3 m_ScaleBy = Vector3.one;
        public Vector3 scaleBy
        {
            get => m_ScaleBy;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ScaleBy = value;
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

            m_ScaleFinal = Vector3.Scale(m_ScaleStart, scaleBy);
            
            // for 1st Update I should decrease Next value for current scale-value
            m_ScaleLast = m_ScaleStart;
        }
    }
}
