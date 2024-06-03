using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Factory Machines/Transform Machine")]
    public class TransformFactoryMachine : PRSFactoryMachine
    {
        public override string FactoryMachineName()
        {
            return "Transform";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            float intensityByMachine = intensity;

            UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);
        }

        //--------------------------------------------------------------------------------------------------------------

        void Reset()
        {
            ResetToDefaults();
        }
    }
}
