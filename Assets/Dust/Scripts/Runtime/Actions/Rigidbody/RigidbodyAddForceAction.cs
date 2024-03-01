using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Rigidbody AddForce")]
    public class RigidbodyAddForceAction : InstantAction
    {
        public enum Space
        {
            World = 0,
            Relative = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_ForceVector = Vector3.up;
        public Vector3 forceVector
        {
            get => m_ForceVector;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ForceVector = value;
            }
        }

        [SerializeField]
        private ForceMode m_ForceMode = ForceMode.Impulse;
        public ForceMode forceMode
        {
            get => m_ForceMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ForceMode = value;
            }
        }

        [SerializeField]
        private Space m_ForceSpace = Space.Relative;
        public Space forceSpace
        {
            get => m_ForceSpace;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ForceSpace = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionExecute()
        {
            if (Dust.IsNull(activeTargetTransform))
                return;

            Rigidbody rb = activeTargetTransform.GetComponent<Rigidbody>();

            if (Dust.IsNull(rb))
                return;

            switch (forceSpace)
            {
                case Space.World:
                    rb.AddForce(forceVector, forceMode);
                    break;

                case Space.Relative:
                    rb.AddRelativeForce(forceVector, forceMode);
                    break;
            }
        }
    }
}
