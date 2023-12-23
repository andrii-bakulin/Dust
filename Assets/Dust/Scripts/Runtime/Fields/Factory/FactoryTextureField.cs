using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Factory Fields/Factory Texture Field")]
    public class FactoryTextureField : Field
    {
        public enum ColorComponent
        {
            Ignore = 0,
            Grayscale = 1,
            Red = 2,
            Green = 3,
            Blue = 4,
            Alpha = 5,
            RGBAverage = 6,
            RGBMin = 7,
            RGBMax = 8,
        }

        public enum SpaceUVW
        {
            UV = 0,
            VW = 1,
            UW = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Texture2D m_Texture;
        public Texture2D texture
        {
            get => m_Texture;
            set => m_Texture = value;
        }

        [SerializeField]
        protected SpaceUVW m_SpaceUVW = SpaceUVW.UV;
        public SpaceUVW spaceUVW
        {
            get => m_SpaceUVW;
            set => m_SpaceUVW = value;
        }

        [SerializeField]
        protected bool m_FlipU = false;
        public bool flipU
        {
            get => m_FlipU;
            set => m_FlipU = value;
        }

        [SerializeField]
        protected bool m_FlipV = false;
        public bool flipV
        {
            get => m_FlipV;
            set => m_FlipV = value;
        }

        [SerializeField]
        protected bool m_FlipW = false;
        public bool flipW
        {
            get => m_FlipW;
            set => m_FlipW = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected ColorComponent m_PowerSource = ColorComponent.Ignore;
        public ColorComponent powerSource
        {
            get => m_PowerSource;
            set => m_PowerSource = value;
        }

        [SerializeField]
        protected bool m_ApplyPowerToAlpha = false;
        public bool applyPowerToAlpha
        {
            get => m_ApplyPowerToAlpha;
            set => m_ApplyPowerToAlpha = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Remapping m_Remapping = new Remapping();
        public Remapping remapping => m_Remapping;

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, texture);
            DynamicState.Append(ref dynamicState, ++seq, spaceUVW);
            DynamicState.Append(ref dynamicState, ++seq, flipU);
            DynamicState.Append(ref dynamicState, ++seq, flipV);
            DynamicState.Append(ref dynamicState, ++seq, flipW);

            DynamicState.Append(ref dynamicState, ++seq, powerSource);
            DynamicState.Append(ref dynamicState, ++seq, applyPowerToAlpha);

            if (powerSource != ColorComponent.Ignore)
                DynamicState.Append(ref dynamicState, ++seq, remapping);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Factory Texture";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            if (Dust.IsNull(texture))
            {
                result.fieldPower = 0f;
                result.fieldColor = Color.black;
            }
            else if (texture.isReadable)
            {
                Vector3 uvw = fieldPoint.inFactoryInstanceState.instance.stateDynamic.uvw;

                float x;
                float y;

                switch (spaceUVW)
                {
                    default:
                    case SpaceUVW.UV:
                        x = flipU ? 1f - uvw.x : uvw.x;
                        y = flipV ? 1f - uvw.y : uvw.y;
                        break;

                    case SpaceUVW.VW:
                        x = flipV ? 1f - uvw.y : uvw.y;
                        y = flipW ? 1f - uvw.z : uvw.z;
                        break;

                    case SpaceUVW.UW:
                        x = flipU ? 1f - uvw.x : uvw.x;
                        y = flipW ? 1f - uvw.z : uvw.z;
                        break;
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                int xOnTexture = Mathf.RoundToInt(x * (texture.width - 1));
                int yOnTexture = Mathf.RoundToInt(y * (texture.height - 1));

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                result.fieldColor = texture.GetPixel(xOnTexture, yOnTexture);

                switch (powerSource)
                {
                    default:
                    case ColorComponent.Ignore:
                        result.fieldPower = 0f;
                        break;

                    case ColorComponent.Grayscale:
                        result.fieldPower = result.fieldColor.grayscale;
                        break;

                    case ColorComponent.Red:
                        result.fieldPower = result.fieldColor.r;
                        break;

                    case ColorComponent.Green:
                        result.fieldPower = result.fieldColor.g;
                        break;

                    case ColorComponent.Blue:
                        result.fieldPower = result.fieldColor.b;
                        break;

                    case ColorComponent.Alpha:
                        result.fieldPower = result.fieldColor.a;
                        break;

                    case ColorComponent.RGBAverage:
                        result.fieldPower = (result.fieldColor.r + result.fieldColor.g + result.fieldColor.b) / 3f;
                        break;

                    case ColorComponent.RGBMin:
                        result.fieldPower = Mathf.Min(Mathf.Min(result.fieldColor.r, result.fieldColor.g), result.fieldColor.b);
                        break;

                    case ColorComponent.RGBMax:
                        result.fieldPower = Mathf.Max(Mathf.Max(result.fieldColor.r, result.fieldColor.g), result.fieldColor.b);
                        break;
                }

                result.fieldPower = remapping.MapValue(result.fieldPower);

                if (applyPowerToAlpha)
                    result.fieldColor.a = result.fieldPower;
            }
            else
            {
                result.fieldPower = 0f;
                result.fieldColor = Color.magenta;

#if UNITY_EDITOR
                Dust.Debug.Warning("Texture [" + texture.name + "] has not read/write enabled");
#endif
            }
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
