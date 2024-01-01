using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class BasicField : Field
    {
        [SerializeField]
        private float m_Power = 1f;
        public float power
        {
            get => m_Power;
            set => m_Power = value;
        }

        [SerializeField]
        private bool m_Unlimited = false;
        public bool unlimited
        {
            get => m_Unlimited;
            set => m_Unlimited = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Remapping m_Remapping = new Remapping();
        public Remapping remapping => m_Remapping;

        //--------------------------------------------------------------------------------------------------------------

        public override bool IsAllowCalculateFieldColor()
        {
            return remapping.colorMode != Remapping.ColorMode.Ignore;
        }
        
#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return true;
        }
        
        public override Gradient GetFieldColorPreview(out float colorPower)
        {
            return GetFieldColorPreview(remapping, out colorPower);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, power);
            DynamicState.Append(ref dynamicState, ++seq, unlimited);

            DynamicState.Append(ref dynamicState, ++seq, remapping);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Reset()
        {
            ResetDefaults();
        }
        
        protected virtual void ResetDefaults()
        {
            // Use this method to reset values for default to remapping object
        }
    }
}
