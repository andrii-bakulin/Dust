using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/2D Fields/Texture Space Field")]
    public class TextureSpaceField : SpaceField
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

        public enum WrapMode
        {
            Repeat = 0,
            Clamp = 1,
            Mirror = 2,
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
        protected WrapMode m_WrapMode = WrapMode.Repeat;
        public WrapMode wrapMode
        {
            get => m_WrapMode;
            set => m_WrapMode = value;
        }

        [SerializeField]
        protected bool m_FlipX = false;
        public bool flipX
        {
            get => m_FlipX;
            set => m_FlipX = value;
        }

        [SerializeField]
        protected bool m_FlipY = false;
        public bool flipY
        {
            get => m_FlipY;
            set => m_FlipY = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_Width = 1.0f;
        public float width
        {
            get => m_Width;
            set => m_Width = NormalizeWidth(value);
        }

        [SerializeField]
        private float m_Height = 1.0f;
        public float height
        {
            get => m_Height;
            set => m_Height = NormalizeHeight(value);
        }

        [SerializeField]
        private Axis3xDirection m_Direction = Axis3xDirection.Y;
        public Axis3xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
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

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, texture);
            DynamicState.Append(ref dynamicState, ++seq, wrapMode);
            DynamicState.Append(ref dynamicState, ++seq, flipX);
            DynamicState.Append(ref dynamicState, ++seq, flipY);

            DynamicState.Append(ref dynamicState, ++seq, direction);
            DynamicState.Append(ref dynamicState, ++seq, width);
            DynamicState.Append(ref dynamicState, ++seq, height);

            DynamicState.Append(ref dynamicState, ++seq, powerSource);
            DynamicState.Append(ref dynamicState, ++seq, applyPowerToAlpha);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Texture Space";
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
                Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

                // Convert to [X+]-axis-space by direction
                localPosition = AxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                float halfWidth = width / 2;
                float halfHeight = height / 2;

                float x = DuMath.Fit(-halfWidth, +halfWidth, 0f, 1f, localPosition.z);
                float y = DuMath.Fit(-halfHeight, +halfHeight, 0f, 1f, localPosition.y);

                if (flipX) x = 1f - x;
                if (flipY) y = 1f - y;

                switch (wrapMode)
                {
                    default:
                    case WrapMode.Repeat:
                        // nothing need to do
                        break;

                    case WrapMode.Clamp:
                        x = Mathf.Clamp01(x);
                        y = Mathf.Clamp01(y);
                        break;

                    case WrapMode.Mirror:
                        x = Mathf.PingPong(x, 1f);
                        y = Mathf.PingPong(y, 1f);
                        break;
                }

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

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = GetGizmoColorRange1();
            DuGizmos.DrawRect(width, height, Vector3.zero, direction);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeWidth(float value)
        {
            return Mathf.Abs(value);
        }

        public static float NormalizeHeight(float value)
        {
            return Mathf.Abs(value);
        }
    }
}
