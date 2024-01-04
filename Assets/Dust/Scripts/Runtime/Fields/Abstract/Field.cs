using System;
using UnityEngine;

namespace Dust
{
    public abstract partial class Field : DuMonoBehaviour, IDynamicState
    {
        public class Point
        {
            // In
            internal Vector3 inPosition; // point in world position
            internal float inOffset; // offset for point in sequence of points [0..1]
            internal FactoryMachine.FactoryInstanceState inFactoryInstanceState;

            // Out/End/Result values
            internal Result result; // also, here I save data for each iteration of calculations (for fields-list)
        }

        public struct Result : IEquatable<Result>
        {
            internal float power; // power calculated by field
            internal Color color; // color calculated by field, Color.alpha used as power of color

            public bool Equals(Result other)
            {
                throw new NotImplementedException();
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected string m_Hint = "";
        public string hint
        {
            get => m_Hint;
            set => m_Hint = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            // Require to show enabled-checkbox in editor for all fields
        }

        //--------------------------------------------------------------------------------------------------------------

        public abstract string FieldName();

        public abstract string FieldDynamicHint();

        public abstract void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public abstract bool IsAllowCalculateFieldColor();

#if UNITY_EDITOR
        public abstract bool IsHasFieldColorPreview();
        public abstract Gradient GetFieldColorPreview(out float colorPower);
#endif

        //--------------------------------------------------------------------------------------------------------------

        public virtual FieldsMap.FieldRecord.BlendPowerMode GetBlendPowerMode()
        {
            return FieldsMap.FieldRecord.BlendPowerMode.Ignore;
        }

        public virtual FieldsMap.FieldRecord.BlendColorMode GetBlendColorMode()
        {
            return FieldsMap.FieldRecord.BlendColorMode.Ignore;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public virtual int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            // Paste here local vars
            DynamicState.Append(ref dynamicState, ++seq, true);

            return DynamicState.Normalize(dynamicState);
        }
    }
}
