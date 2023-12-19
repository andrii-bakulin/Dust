using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Events/On Collision Event")]
    public class OnCollisionEvent : OnColliderEvent
    {
        [SerializeField]
        private CollisionEvent m_OnEnter = null;
        public CollisionEvent onEnter => m_OnEnter;

        [SerializeField]
        private CollisionEvent m_OnStay = null;
        public CollisionEvent onStay => m_OnStay;

        [SerializeField]
        private CollisionEvent m_OnExit = null;
        public CollisionEvent onExit => m_OnExit;

        //--------------------------------------------------------------------------------------------------------------

        private void OnCollisionEnter(Collision other)
        {
            if (Dust.IsNull(onEnter) || onEnter.GetPersistentEventCount() == 0 || !IsRequireSendEvent(other.gameObject))
                return;

            onEnter.Invoke(other);
        }

        private void OnCollisionStay(Collision other)
        {
            if (Dust.IsNull(onStay) || onStay.GetPersistentEventCount() == 0 || !IsRequireSendEvent(other.gameObject))
                return;

            onStay.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            if (Dust.IsNull(onExit) || onExit.GetPersistentEventCount() == 0 || !IsRequireSendEvent(other.gameObject))
                return;

            onExit.Invoke(other);
        }
    }
}
