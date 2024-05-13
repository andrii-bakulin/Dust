using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Value Float Action")]
    public class ValueFloatAction : IntervalAction
    {
        [Serializable]
        public class UpdateEvent : UnityEvent<float>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private float m_ValueStart = 0f;
        public float valueStart
        {
            get => m_ValueStart;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ValueStart = value;
            }
        }

        [SerializeField]
        private float m_ValueEnd = 1f;
        public float valueEnd
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
            onUpdate?.Invoke( Mathf.Lerp(valueStart, valueEnd, m_PlaybackState) );
        }
    }
}
