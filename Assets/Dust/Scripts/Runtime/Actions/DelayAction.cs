using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Delay Action")]
    public class DelayAction : IntervalAction
    {
        public enum DelayMode
        {
            Fixed = 0,
            Range = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private DelayMode m_DelayMode = DelayMode.Fixed;
        public DelayMode delayMode
        {
            get => m_DelayMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_DelayMode = value;
            }
        }

        [SerializeField]
        private DuRange m_DurationRange = DuRange.oneToTwo;
        public DuRange durationRange
        {
            get => m_DurationRange;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_DurationRange.min = NormalizeDuration(value.min);
                m_DurationRange.max = NormalizeDuration(value.max);
            }
        }

        [SerializeField]
        private int m_Seed = 0;
        public int seed
        {
            get => m_Seed;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Seed = value;
                m_Random = null;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuRandom m_Random;
        protected DuRandom random => m_Random ??= new DuRandom(seed);

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionStart()
        {
            base.OnActionStart();

            if (delayMode == DelayMode.Range)
            {
                m_Duration = random.Range(durationRange.min, durationRange.max);
            }
        }

        protected override void OnActionUpdate(float deltaTime)
        {
            // Nothing need to do :)
        }
    }
}
