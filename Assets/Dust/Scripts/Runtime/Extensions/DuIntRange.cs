using System;
using UnityEngine;

namespace DustEngine
{
    [Serializable]
    public struct DuIntRange : IEquatable<DuIntRange>
    {
        [SerializeField]
        private int m_Min;
        public int min
        {
            get => m_Min;
            set => m_Min = value;
        }

        [SerializeField]
        private int m_Max;
        public int max
        {
            get => m_Max;
            set => m_Max = value;
        }

        private static readonly DuIntRange zeroToOneRange = new DuIntRange(0, 1);
        private static readonly DuIntRange oneToTwoRange = new DuIntRange(1, 2);
        private static readonly DuIntRange oneToFiveRange = new DuIntRange(1, 5);

        public static DuIntRange zeroToOne => zeroToOneRange;
        public static DuIntRange oneToTwo => oneToTwoRange;
        public static DuIntRange oneToFive => oneToFiveRange;

        public DuIntRange(int min, int max)
        {
            this.m_Min = min;
            this.m_Max = max;
        }

        public bool Equals(DuIntRange other)
        {
            return this.min == other.min && this.max == other.max;
        }
    }
}
