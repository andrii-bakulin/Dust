using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class MathField : Field
    {
        public override FieldsMap.FieldRecord.BlendPowerMode GetBlendPowerMode()
        {
            return FieldsMap.FieldRecord.BlendPowerMode.Set;
        }

        public override FieldsMap.FieldRecord.BlendColorMode GetBlendColorMode()
        {
            return FieldsMap.FieldRecord.BlendColorMode.Set;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override bool IsAllowCalculateFieldColor()
        {
            return true;
        }

#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return false;
        }

        public override Gradient GetFieldColorPreview(out float colorPower)
        {
            colorPower = 0f;
            return null;
        }
#endif
    }
}
