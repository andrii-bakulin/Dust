using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Rigidbody AddTorque")]
    public class RigidbodyAddTorqueAction : InstantAction
    {
        public enum Space
        {
            World = 0,
            Relative = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_TorqueVector = Vector3.up;
        public Vector3 torqueVector
        {
            get => m_TorqueVector;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TorqueVector = value;
            }
        }

        [SerializeField]
        private ForceMode m_TorqueMode = ForceMode.Impulse;
        public ForceMode torqueMode
        {
            get => m_TorqueMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TorqueMode = value;
            }
        }

        [SerializeField]
        private Space m_TorqueSpace = Space.Relative;
        public Space torqueSpace
        {
            get => m_TorqueSpace;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TorqueSpace = value;
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

            switch (torqueSpace)
            {
                case Space.World:
                    rb.AddTorque(torqueVector, torqueMode);
                    break;

                case Space.Relative:
                    rb.AddRelativeTorque(torqueVector, torqueMode);
                    break;
            }
        }
    }
}
