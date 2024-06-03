using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Value Gradient Action")]
    public class ValueGradientAction : IntervalAction
    {
        [Serializable]
        public class UpdateEvent : UnityEvent<Color>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Gradient m_Gradient;
        public Gradient gradient
        {
            get => m_Gradient;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Gradient = value;
            }
        }

        [SerializeField]
        protected UpdateEvent m_OnUpdate;
        public UpdateEvent onUpdate => m_OnUpdate;

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            onUpdate?.Invoke( gradient.Evaluate(m_PlaybackState) );
        }
    }
}
