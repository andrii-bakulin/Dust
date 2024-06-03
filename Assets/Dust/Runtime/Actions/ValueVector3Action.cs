using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Value Vector3 Action")]
    public class ValueVector3Action : IntervalAction
    {
        [Serializable]
        public class UpdateEvent : UnityEvent<Vector3>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_ValueStart = Vector3.zero;
        public Vector3 valueStart
        {
            get => m_ValueStart;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ValueStart = value;
            }
        }

        [SerializeField]
        private Vector3 m_ValueEnd = Vector3.one;
        public Vector3 valueEnd
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
            onUpdate?.Invoke( Vector3.Lerp(valueStart, valueEnd, m_PlaybackState) );
        }
    }
}
