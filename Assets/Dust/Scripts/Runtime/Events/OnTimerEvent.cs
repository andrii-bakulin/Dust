using System;
using UnityEngine;
using UnityEngine.Events;

namespace DustEngine
{
    [AddComponentMenu("Dust/Events/On Timer Event")]
    public class OnTimerEvent : OnAbstractEvent
    {
        [Serializable]
        public class TimerEvent : UnityEvent<GameObject>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private float m_Delay = 1f;
        public float delay
        {
            get => m_Delay;
            set => m_Delay = NormalizeDelay(value);
        }

        [SerializeField]
        private int m_Repeat = 0;
        public int repeat
        {
            get => m_Repeat;
            set => m_Repeat = NormalizeRepeat(value);
        }

        [SerializeField]
        private bool m_FireOnStart = false;
        public bool fireOnStart
        {
            get => m_FireOnStart;
            set => m_FireOnStart = value;
        }

        [SerializeField]
        private TimerEvent m_OnFire = null;
        public TimerEvent onFire => m_OnFire;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private int m_FireCounts;
        public int fireCounts
        {
            get => m_FireCounts;
            set => m_FireCounts = value;
        }

        private float m_Timer;
        public float timer
        {
            get => m_Timer;
            set => m_Timer = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        void Start()
        {
            m_FireCounts = 0;
            m_Timer = 0f;

            if (fireOnStart)
                Fire();
        }

        void Update()
        {
            if (m_Repeat > 0 && m_FireCounts >= m_Repeat)
                return;

            m_Timer += Time.deltaTime;

            if (m_Timer >= m_Delay)
                Fire();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Fire()
        {
            onFire?.Invoke(gameObject);

            m_FireCounts++;
            m_Timer = 0f;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeDelay(float value)
        {
            return Mathf.Clamp(value, 0.0f, float.MaxValue);
        }

        public static int NormalizeRepeat(int value)
        {
            return Mathf.Clamp(value, 0, int.MaxValue);
        }
    }
}
