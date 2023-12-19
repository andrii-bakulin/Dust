using System;
using UnityEngine;
using UnityEngine.Events;

namespace DustEngine
{
    [AddComponentMenu("Dust/Events/On Key Event")]
    public class OnKeyEvent : OnAbstractEvent
    {
        [SerializeField]
        private KeyCode m_KeyCode = KeyCode.None;
        public KeyCode keyCode
        {
            get => m_KeyCode;
            set => m_KeyCode = value;
        }

        [SerializeField]
        private KeyEvent m_OnKeyDown = null;
        public KeyEvent onKeyDown => m_OnKeyDown;

        [SerializeField]
        private KeyEvent m_OnKeyHold = null;
        public KeyEvent onKeyHold => m_OnKeyHold;

        [SerializeField]
        private KeyEvent m_OnKeyUp = null;
        public KeyEvent onKeyUp => m_OnKeyUp;

        //--------------------------------------------------------------------------------------------------------------

        [Serializable]
        public class KeyEvent : UnityEvent<KeyCode>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            if (m_KeyCode == KeyCode.None)
                return;

            if (Input.GetKeyDown(m_KeyCode) && Dust.IsNotNull(onKeyDown) && onKeyDown.GetPersistentEventCount() > 0)
            {
                onKeyDown.Invoke(m_KeyCode);
            }

            if (Input.GetKey(m_KeyCode) && Dust.IsNotNull(onKeyHold) && onKeyHold.GetPersistentEventCount() > 0)
            {
                onKeyHold.Invoke(m_KeyCode);
            }

            if (Input.GetKeyUp(m_KeyCode) && Dust.IsNotNull(onKeyUp) && onKeyUp.GetPersistentEventCount() > 0)
            {
                onKeyUp.Invoke(m_KeyCode);
            }
        }
    }
}
