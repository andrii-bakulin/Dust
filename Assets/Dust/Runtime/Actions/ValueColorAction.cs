using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Value Color Action")]
    public class ValueColorAction : IntervalAction
    {
        [Serializable]
        public class UpdateEvent : UnityEvent<Color>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Color m_ValueStart = Color.black;
        public Color valueStart
        {
            get => m_ValueStart;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ValueStart = value;
            }
        }

        [SerializeField]
        private Color m_ValueEnd = Color.white;
        public Color valueEnd
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
            onUpdate?.Invoke( Color.Lerp(valueStart, valueEnd, m_PlaybackState) );
        }
    }
}
