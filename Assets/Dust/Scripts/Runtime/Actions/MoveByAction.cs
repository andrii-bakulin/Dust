using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/MoveBy Action")]
    public class MoveByAction : MoveAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
            SelfFixed = 2,
            SelfDynamic = 3,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_MoveBy = Vector3.zero;
        public Vector3 moveBy
        {
            get => m_MoveBy;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_MoveBy = value;
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

            if (space == Space.World)
            {
                var trParent = activeTargetTransform.parent;
                if (Dust.IsNotNull(trParent))
                    m_DeltaLocalMove = trParent.InverseTransformPoint(moveBy) - trParent.InverseTransformPoint(Vector3.zero);
                else
                    m_DeltaLocalMove = moveBy;
            }
            else if (space == Space.Local)
            {
                m_DeltaLocalMove = moveBy;
            }
            else if (space == Space.SelfFixed)
            {
                m_DeltaLocalMove = DuMath.RotatePoint(moveBy, activeTargetTransform.localEulerAngles);
            }
            else if (space == Space.SelfDynamic)
            {
                m_DeltaLocalMove = moveBy;
                m_AutoRotateBySelfDirection = true;
            }
        }
    }
}
