using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Events/On Collision Event")]
    public class OnCollisionEvent : OnCollideEvent
    {
        [SerializeField]
        private CollisionEvent m_OnEnter;
        public CollisionEvent onEnter => m_OnEnter;

        [SerializeField]
        private CollisionEvent m_OnStay;
        public CollisionEvent onStay => m_OnStay;

        [SerializeField]
        private CollisionEvent m_OnExit;
        public CollisionEvent onExit => m_OnExit;

        //--------------------------------------------------------------------------------------------------------------

        private void OnCollisionEnter(Collision other)
        {
            if (!this.enabled ||
                Dust.IsNull(onEnter) || onEnter.GetPersistentEventCount() == 0 ||
                !IsRequireSendEvent(other.gameObject))
                return;

            onEnter.Invoke(other);
        }

        private void OnCollisionStay(Collision other)
        {
            if (!this.enabled ||
                Dust.IsNull(onStay) || onStay.GetPersistentEventCount() == 0 ||
                !IsRequireSendEvent(other.gameObject))
                return;

            onStay.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            if (!this.enabled ||
                Dust.IsNull(onExit) || onExit.GetPersistentEventCount() == 0 ||
                !IsRequireSendEvent(other.gameObject))
                return;

            onExit.Invoke(other);
        }
    }
}
