using System;
using UnityEngine;

namespace DustEngine
{
    [Serializable]
    public struct DuRange : IEquatable<DuRange>
    {
        [SerializeField]
        private float m_Min;
        public float min
        {
            get => m_Min;
            set => m_Min = value;
        }

        [SerializeField]
        private float m_Max;
        public float max
        {
            get => m_Max;
            set => m_Max = value;
        }

        private static readonly DuRange zeroToOneRange = new DuRange(0.0f, 1.0f);
        private static readonly DuRange oneToTwoRange = new DuRange(1.0f, 2.0f);

        public static DuRange zeroToOne => zeroToOneRange;
        public static DuRange oneToTwo => oneToTwoRange;

        public DuRange(float min, float max)
        {
            this.m_Min = min;
            this.m_Max = max;
        }

        public bool Equals(DuRange other)
        {
            return Mathf.Approximately(this.min, other.min) && Mathf.Approximately(this.max, other.max);
        }
    }
}
