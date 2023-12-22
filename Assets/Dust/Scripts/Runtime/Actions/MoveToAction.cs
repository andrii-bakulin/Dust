using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/MoveTo Action")]
    public class MoveToAction : MoveAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_MoveTo = Vector3.zero;
        public Vector3 moveTo
        {
            get => m_MoveTo;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_MoveTo = value;
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

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnActionStart()
        {
            base.OnActionStart();

            if (Dust.IsNull(activeTargetTransform))
                return;

            Vector3 localMoveTo;

            if (space == Space.World)
            {
                var trParent = activeTargetTransform.parent;
                localMoveTo = Dust.IsNotNull(trParent) ? trParent.InverseTransformPoint(m_MoveTo) : m_MoveTo;
            }
            else if (space == Space.Local)
            {
                localMoveTo = m_MoveTo;
            }
            else return;

            m_DeltaLocalMove = localMoveTo - activeTargetTransform.localPosition; 
        }
    }
}
