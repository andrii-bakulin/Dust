using UnityEngine;
using UnityEditor;

namespace Dust
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

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Remapping m_Remapping = new Remapping();
        public Remapping remapping => m_Remapping;

        [SerializeField]
        private Coloring m_Coloring = new Coloring();
        public Coloring coloring => m_Coloring;

        //--------------------------------------------------------------------------------------------------------------

        public override bool IsAllowCalculateFieldColor()
        {
            return coloring.colorMode != Coloring.ColorMode.Ignore;
        }
        
#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return true;
        }
        
        public override Gradient GetFieldColorPreview(out float colorPower)
        {
            return coloring.GetFieldColorPreview(out colorPower);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, power);

            DynamicState.Append(ref dynamicState, ++seq, remapping);
            DynamicState.Append(ref dynamicState, ++seq, coloring);

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
