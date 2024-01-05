using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Events/On Trigger Event")]
    public class OnTriggerEvent : OnColliderEvent
    {
        [SerializeField]
        private TriggerEvent m_OnEnter;
        public TriggerEvent onEnter => m_OnEnter;

        [SerializeField]
        private TriggerEvent m_OnStay;
        public TriggerEvent onStay => m_OnStay;

        [SerializeField]
        private TriggerEvent m_OnExit;
        public TriggerEvent onExit => m_OnExit;

        //--------------------------------------------------------------------------------------------------------------

        private void OnTriggerEnter(Collider other)
        {
            if (Dust.IsNull(onEnter) || onEnter.GetPersistentEventCount() == 0 || !IsRequireSendEvent(other.gameObject))
                return;

            onEnter.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (Dust.IsNull(onStay) || onStay.GetPersistentEventCount() == 0 || !IsRequireSendEvent(other.gameObject))
                return;

            onStay.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (Dust.IsNull(onExit) || onExit.GetPersistentEventCount() == 0 || !IsRequireSendEvent(other.gameObject))
                return;

            onExit.Invoke(other);
        }
    }
}
