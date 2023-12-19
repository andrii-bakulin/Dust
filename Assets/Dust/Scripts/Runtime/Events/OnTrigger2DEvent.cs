using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Events/On Trigger Event 2D")]
    public class OnTrigger2DEvent : OnColliderEvent
    {
        [SerializeField]
        private Trigger2DEvent m_OnEnter = null;
        public Trigger2DEvent onEnter => m_OnEnter;

        [SerializeField]
        private Trigger2DEvent m_OnStay = null;
        public Trigger2DEvent onStay => m_OnStay;

        [SerializeField]
        private Trigger2DEvent m_OnExit = null;
        public Trigger2DEvent onExit => m_OnExit;

        //--------------------------------------------------------------------------------------------------------------

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Dust.IsNull(onEnter) || onEnter.GetPersistentEventCount() == 0 || !IsRequireSendEvent(other.gameObject))
                return;

            onEnter.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (Dust.IsNull(onStay) || onStay.GetPersistentEventCount() == 0 || !IsRequireSendEvent(other.gameObject))
                return;

            onStay.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (Dust.IsNull(onExit) || onExit.GetPersistentEventCount() == 0 || !IsRequireSendEvent(other.gameObject))
                return;

            onExit.Invoke(other);
        }
    }
}
