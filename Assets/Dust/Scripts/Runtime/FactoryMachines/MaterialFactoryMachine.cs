using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Factory Machines/Material Machine")]
    public class MaterialFactoryMachine : FactoryMachine
    {
        public override string FactoryMachineName()
        {
            return "Material";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override bool PrepareForUpdateInstancesStates(FactoryInstanceState factoryInstanceState)
        {
            // Should execute logic even if intensityByFactory is ZERO
            return true;
        }

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            float intensityByMachine = factoryInstanceState.intensityByFactory
                                       * intensity;

            factoryInstanceState.instance.ApplyMaterialUpdatesToObject(intensityByMachine);
        }

        public override void FinalizeUpdateInstancesStates(FactoryInstanceState factoryInstanceState)
        {
            // Hide base implementation
        }

        //--------------------------------------------------------------------------------------------------------------

        void Reset()
        {
            // Define default states
        }
    }
}
