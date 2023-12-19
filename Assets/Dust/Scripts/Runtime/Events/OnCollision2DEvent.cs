using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Events/On Collision Event 2D")]
    public class OnCollision2DEvent : OnColliderEvent
    {
        [SerializeField]
        private Collision2DEvent m_OnEnter = null;
        public Collision2DEvent onEnter => m_OnEnter;

        [SerializeField]
        private Collision2DEvent m_OnStay = null;
        public Collision2DEvent onStay => m_OnStay;

        [SerializeField]
        private Collision2DEvent m_OnExit = null;
        public Collision2DEvent onExit => m_OnExit;

        //--------------------------------------------------------------------------------------------------------------

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (Dust.IsNull(onEnter) || onEnter.GetPersistentEventCount() == 0 || !IsRequireSendEvent(other.gameObject))
                return;

            onEnter.Invoke(other);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (Dust.IsNull(onStay) || onStay.GetPersistentEventCount() == 0 || !IsRequireSendEvent(other.gameObject))
                return;

            onStay.Invoke(other);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (Dust.IsNull(onExit) || onExit.GetPersistentEventCount() == 0 || !IsRequireSendEvent(other.gameObject))
                return;

            onExit.Invoke(other);
        }
    }
}
