using UnityEngine;

namespace DustEngine
{
    public abstract class FactoryMachine : DuMonoBehaviour, IDynamicState
    {
        public class FactoryInstanceState
        {
            // In
            internal Factory factory;
            internal FactoryInstance instance;
            internal float intensityByFactory;
            internal float intensityByMachine;

            // Calculated values
            internal float fieldPower;
            internal Color fieldColor;

            // Calculated-n-Supported params:               // Use by FactoryMachine: Noise
            internal bool extraPowerEnabled;
            internal Vector3 extraPowerPosition;
            internal Vector3 extraPowerRotation;
            internal Vector3 extraPowerScale;
        }

        [System.Serializable]
        public class Record
        {
            [SerializeField]
            private FactoryMachine m_FactoryMachine = null;
            public FactoryMachine factoryMachine
            {
                get => m_FactoryMachine;
                set => m_FactoryMachine = value;
            }

            [SerializeField]
            private float m_Intensity = 1f;
            public float intensity
            {
                get => m_Intensity;
                set => m_Intensity = value;
            }

            [SerializeField]
            private bool m_Enabled = true;
            public bool enabled
            {
                get => m_Enabled;
                set => m_Enabled = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected string m_CustomHint = "";
        public string customHint
        {
            get => m_CustomHint;
            set => m_CustomHint = value;
        }

        [SerializeField]
        protected float m_Intensity = 1.0f;
        public float intensity
        {
            get => m_Intensity;
            set => m_Intensity = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            // Require to show enabled-checkbox in editor for all factory-machine
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public virtual int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DynamicState.Append(ref dynamicState, ++seq, intensity);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public abstract string FactoryMachineName();

        public abstract string FactoryMachineDynamicHint();

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public abstract bool PrepareForUpdateInstancesStates(FactoryInstanceState factoryInstanceState);

        public abstract void UpdateInstanceState(FactoryInstanceState factoryInstanceState);

        public abstract void FinalizeUpdateInstancesStates(FactoryInstanceState factoryInstanceState);
    }
}
