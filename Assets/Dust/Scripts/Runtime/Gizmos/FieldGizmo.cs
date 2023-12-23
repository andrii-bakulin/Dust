using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Field Gizmo")]
    [ExecuteInEditMode]
    public class FieldGizmo : AbstractGizmo
    {
        public enum SourceType
        {
            Field = 0,
            FieldsSpace = 1,
        }

        [SerializeField]
        private SourceType m_SourceType = SourceType.Field;
        public SourceType sourceType
        {
            get => m_SourceType;
            set => m_SourceType = value;
        }

        [SerializeField]
        private Field m_Field;
        public Field field
        {
            get => m_Field;
            set => m_Field = value;
        }

        [SerializeField]
        private FieldsSpace m_FieldsSpace;
        public FieldsSpace fieldsSpace
        {
            get => m_FieldsSpace;
            set => m_FieldsSpace = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Vector3 m_GridSize = Vector3.one * 10f;
        public Vector3 gridSize
        {
            get => m_GridSize;
            set => m_GridSize = value;
        }

        [SerializeField]
        private Vector3 m_GridOffset = Vector3.zero;
        public Vector3 gridOffset
        {
            get => m_GridOffset;
            set => m_GridOffset = value;
        }

        [SerializeField]
        private Vector3Int m_GridPointsCount = new Vector3Int(19, 1, 19);
        public Vector3Int gridPointsCount
        {
            get => m_GridPointsCount;
            set => m_GridPointsCount = NormalizeGridPointsCount(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_PowerVisible = false;
        public bool powerVisible
        {
            get => m_PowerVisible;
            set => m_PowerVisible = value;
        }

        [SerializeField]
        private float m_PowerSize = 1f;
        public float powerSize
        {
            get => m_PowerSize;
            set => m_PowerSize = NormalizeSize(value);
        }

        [SerializeField]
        private bool m_PowerDotsVisible = false;
        public bool powerDotsVisible
        {
            get => m_PowerDotsVisible;
            set => m_PowerDotsVisible = value;
        }

        [SerializeField]
        private float m_PowerDotsSize = 0.4f;
        public float powerDotsSize
        {
            get => m_PowerDotsSize;
            set => m_PowerDotsSize = NormalizeSize(value);
        }

        [SerializeField]
        private bool m_PowerImpactOnDotsSize = false;
        public bool powerImpactOnDotsSize
        {
            get => m_PowerImpactOnDotsSize;
            set => m_PowerImpactOnDotsSize = value;
        }

        [SerializeField]
        private Gradient m_PowerDotsColor = DuGradient.CreateBlackToRed();
        public Gradient powerDotsColor
        {
            get => m_PowerDotsColor;
            set => m_PowerDotsColor = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ColorVisible = true;
        public bool colorVisible
        {
            get => m_ColorVisible;
            set => m_ColorVisible = value;
        }

        [SerializeField]
        private float m_ColorSize = 1f;
        public float colorSize
        {
            get => m_ColorSize;
            set => m_ColorSize = NormalizeSize(value);
        }

        [SerializeField]
        private bool m_PowerImpactOnColorSize = false;
        public bool powerImpactOnColorSize
        {
            get => m_PowerImpactOnColorSize;
            set => m_PowerImpactOnColorSize = value;
        }

        [SerializeField]
        private bool m_ColorAllowTransparent = true;
        public bool colorAllowTransparent
        {
            get => m_ColorAllowTransparent;
            set => m_ColorAllowTransparent = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string GizmoName()
        {
            return "Field";
        }

#if UNITY_EDITOR
        protected override void DrawGizmos()
        {
            bool showPower = false, showColor = false;

            switch (sourceType)
            {
                case SourceType.Field:
                    if (Dust.IsNull(field))
                        return;

                    showPower = (powerVisible || powerDotsVisible);
                    showColor = colorVisible;
                    break;

                case SourceType.FieldsSpace:
                    if (Dust.IsNull(fieldsSpace))
                        return;

                    showPower = (powerVisible || powerDotsVisible) && fieldsSpace.fieldsMap.calculatePower;
                    showColor = colorVisible && fieldsSpace.fieldsMap.calculateColor;
                    break;
            }
            
            if (!showPower && !showColor)
                return;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            Vector3 gridStep = new Vector3
            {
                x = gridPointsCount.x > 1 ? gridSize.x / (gridPointsCount.x - 1) : 0f,
                y = gridPointsCount.y > 1 ? gridSize.y / (gridPointsCount.y - 1) : 0f,
                z = gridPointsCount.z > 1 ? gridSize.z / (gridPointsCount.z - 1) : 0f,
            };

            Vector3 zeroPoint = new Vector3
            {
                x = -(gridPointsCount.x - 1) / 2f * gridStep.x + gridOffset.x,
                y = -(gridPointsCount.y - 1) / 2f * gridStep.y + gridOffset.y,
                z = -(gridPointsCount.z - 1) / 2f * gridStep.z + gridOffset.z,
            };

            float offset = 0f;
            float deltaOffset = 1f / Mathf.Max(1, gridPointsCount.x * gridPointsCount.y * gridPointsCount.z);

            for (int z = 0; z < gridPointsCount.z; z++)
            for (int y = 0; y < gridPointsCount.y; y++)
            for (int x = 0; x < gridPointsCount.x; x++)
            {
                var position = zeroPoint + new Vector3(gridStep.x * x, gridStep.y * y, gridStep.z * z);

                Vector3 worldPosition = transform.TransformPoint(position);

                Color fieldColor;
                float fieldPower;

                switch (sourceType)
                {
                    case SourceType.Field:
                        fieldPower = field.GetPowerAndColor(worldPosition, out fieldColor);
                        break;

                    case SourceType.FieldsSpace:
                        fieldPower = fieldsSpace.GetPowerAndColor(worldPosition, offset, out fieldColor);
                        break;
                    
                    default:
                        return;
                }

                if (showColor)
                {
                    if (!colorAllowTransparent)
                        fieldColor = ColorBlend.AlphaBlend(Color.black, fieldColor);

                    float dotSize = 0.1f * colorSize;
                    if (powerImpactOnColorSize)
                        dotSize *= fieldPower;

                    Handles.color = fieldColor;
                    Handles.DotHandleCap(0, worldPosition, transform.rotation, dotSize, EventType.Repaint);
                }

                if (showPower)
                {
                    if (powerDotsVisible)
                    {
                        if (Dust.IsNotNull(powerDotsColor))
                            Handles.color = powerDotsColor.Evaluate(fieldPower);
                        else
                            Handles.color = Color.Lerp(Color.black, Color.white, fieldPower);

                        float dotSize = 0.1f * powerDotsSize;
                        if (powerImpactOnDotsSize)
                            dotSize *= fieldPower;

                        Handles.DotHandleCap(0, worldPosition, transform.rotation, dotSize, EventType.Repaint);
                    }

                    // Draw values last!
                    if (powerVisible)
                    {
                        GUIStyle style = new GUIStyle("Label");
                        style.fontSize = Mathf.RoundToInt(style.fontSize * powerSize * (powerImpactOnDotsSize ? fieldPower : 1f));
                        style.fontSize = Mathf.Max(style.fontSize, 3); // Cannot be 0!

                        Handles.Label(worldPosition, fieldPower.ToString("F2"), style);
                    }
                }

                offset += deltaOffset;
            }
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeSize(float value)
        {
            return Mathf.Clamp(value, 0.1f, float.MaxValue);
        }

        public static Vector3Int NormalizeGridPointsCount(Vector3Int value)
        {
            return DuVector3Int.Clamp(value, Vector3Int.one, Vector3Int.one * 1000);
        }
    }
}
