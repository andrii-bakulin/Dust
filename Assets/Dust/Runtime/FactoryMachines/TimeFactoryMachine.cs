using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Factory Machines/Time Machine")]
    public class TimeFactoryMachine : PRSFactoryMachine
    {
        private float m_TimeSinceStart;
        public float timeSinceStart => m_TimeSinceStart;

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "Time";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            m_TimeSinceStart += Time.deltaTime;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            float intensityByMachine = timeSinceStart * intensity;

            UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, m_TimeSinceStart);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        void Reset()
        {
            ResetToDefaults();
        }
    }
}
