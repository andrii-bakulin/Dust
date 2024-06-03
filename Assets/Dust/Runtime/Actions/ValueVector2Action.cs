using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Value Vector2 Action")]
    public class ValueVector2Action : IntervalAction
    {
        [Serializable]
        public class UpdateEvent : UnityEvent<Vector2>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector2 m_ValueStart = Vector2.zero;
        public Vector2 valueStart
        {
            get => m_ValueStart;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ValueStart = value;
            }
        }

        [SerializeField]
        private Vector2 m_ValueEnd = Vector2.one;
        public Vector2 valueEnd
        {
            get => m_ValueEnd;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ValueEnd = value;
            }
        }

        [SerializeField]
        protected UpdateEvent m_OnUpdate;
        public UpdateEvent onUpdate => m_OnUpdate;

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            onUpdate?.Invoke( Vector2.Lerp(valueStart, valueEnd, m_PlaybackState) );
        }
    }
}
