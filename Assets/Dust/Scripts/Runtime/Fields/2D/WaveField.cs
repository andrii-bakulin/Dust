using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/2D Fields/Wave Field")]
    [ExecuteInEditMode]
    public class WaveField : SpaceField
    {
        public enum GizmoQuality
        {
            Low = 0,
            Medium = 1,
            High = 2,
            ExtraHigh = 3
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private float m_Amplitude = 0.5f;
        public float amplitude
        {
            get => m_Amplitude;
            set => m_Amplitude = value;
        }

        [SerializeField]
        private float m_Size = 1f;
        public float size
        {
            get => m_Size;
            set => m_Size = value;
        }

        [SerializeField]
        private float m_LinearFalloff = 0f;
        public float linearFalloff
        {
            get => m_LinearFalloff;
            set => m_LinearFalloff = value;
        }

        [SerializeField]
        private float m_Offset = 0f;
        public float offset
        {
            get => m_Offset;
            set => m_Offset = value;
        }

        [SerializeField]
        private float m_AnimationSpeed = 0f;
        public float animationSpeed
        {
            get => m_AnimationSpeed;
            set => m_AnimationSpeed = value;
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
        private float m_GizmoSize = 3f;
        public float gizmoSize
        {
            get => m_GizmoSize;
            set => m_GizmoSize = value;
        }

        [SerializeField]
        private GizmoQuality m_GizmoQuality = GizmoQuality.Medium;
        public GizmoQuality gizmoQuality
        {
            get => m_GizmoQuality;
            set => m_GizmoQuality = value;
        }

        [SerializeField]
        private bool m_GizmoAnimated = false;
        public bool gizmoAnimated
        {
            get => m_GizmoAnimated;
            set => m_GizmoAnimated = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private float m_OffsetDynamic;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        void OnEnable()
        {
            if (isInEditorMode)
            {
                EditorUpdateReset();

                EditorApplication.update -= EditorUpdate;
                EditorApplication.update += EditorUpdate;
            }
        }

        void OnDisable()
        {
            if (isInEditorMode)
            {
                EditorApplication.update -= EditorUpdate;
            }
        }

        void EditorUpdate()
        {
            float deltaTime;

            if (!EditorUpdateTick(out deltaTime))
                return;

            if (gizmoAnimated && DuMath.IsNotZero(animationSpeed))
            {
                m_OffsetDynamic += deltaTime * animationSpeed;

                // ForcedRedrawSceneView
                SceneView.lastActiveSceneView.Repaint();
            }
        }
#endif

        void Update()
        {
#if UNITY_EDITOR
            if (isInEditorMode) return;
#endif

            m_OffsetDynamic += Time.deltaTime * animationSpeed;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, amplitude);
            DynamicState.Append(ref dynamicState, ++seq, size);
            DynamicState.Append(ref dynamicState, ++seq, linearFalloff);
            DynamicState.Append(ref dynamicState, ++seq, direction);

            DynamicState.Append(ref dynamicState, ++seq, offset);
            DynamicState.Append(ref dynamicState, ++seq, animationSpeed);

            DynamicState.Append(ref dynamicState, ++seq, m_OffsetDynamic);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Wave";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

            // Convert to Axis X+ (xp) space
            var xpLocalPosition = AxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

            result.fieldPower = GetPowerForLocalPositionInAxisXPlus(xpLocalPosition);
            result.fieldColor = GetFieldColorFromRemapping(remapping, result.fieldPower, calculateColor);
        }

        internal float GetPowerForLocalPositionInAxisXPlus(Vector3 xpLocalPosition)
        {
            if (DuMath.IsZero(size))
                return remapping.MapValue(0.5f);

            float distance = DuMath.Length(xpLocalPosition.y, xpLocalPosition.z);

            if (DuMath.IsNotZero(linearFalloff) && distance >= linearFalloff)
                return remapping.MapValue(0.5f);

            float sinOffset = distance / size - (offset + 0.75f) - m_OffsetDynamic;
            float waveOffset = Mathf.Sin(Constants.PI2 * sinOffset) * amplitude;

            if (DuMath.IsNotZero(linearFalloff))
                waveOffset *= Mathf.Clamp01((linearFalloff - distance) / linearFalloff);

            // Convert waveOffset [-1..+1] to [0..1]))
            waveOffset = DuMath.Fit(-1f, +1f, 0f, 1f, waveOffset);

            return remapping.MapValue(waveOffset);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = GetGizmoColorRange1();

            int segments;

            switch (gizmoQuality)
            {
                default:
                case GizmoQuality.Low:       segments = 12; break;
                case GizmoQuality.Medium:    segments = 24; break;
                case GizmoQuality.High:      segments = 48; break;
                case GizmoQuality.ExtraHigh: segments = 96; break;
            }

            Vector3 zeroOffset = new Vector3(0, -gizmoSize / 2f, -gizmoSize / 2f);

            float delta = gizmoSize / segments;

            for (int s0 = 0; s0 <= segments; s0++)
            for (int s1 = 0; s1 < segments; s1++)
            {
                Vector3 pointY0, pointY1;
                Vector3 pointZ0, pointZ1;

                pointY0 = zeroOffset + new Vector3(0, (s1 + 0) * delta, s0 * delta);
                pointY1 = zeroOffset + new Vector3(0, (s1 + 1) * delta, s0 * delta);
                pointZ0 = new Vector3(pointY0.x, pointY0.z, pointY0.y);
                pointZ1 = new Vector3(pointY1.x, pointY1.z, pointY1.y);

                if (DuMath.IsNotZero(linearFalloff) && (pointY0.magnitude > linearFalloff || pointY1.magnitude > linearFalloff))
                    continue;

                pointY0.x = GetPowerForLocalPositionInAxisXPlus(pointY0);
                pointY1.x = GetPowerForLocalPositionInAxisXPlus(pointY1);
                pointZ0.x = GetPowerForLocalPositionInAxisXPlus(pointZ0);
                pointZ1.x = GetPowerForLocalPositionInAxisXPlus(pointZ1);

                pointY0 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, pointY0);
                pointY1 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, pointY1);
                pointZ0 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, pointZ0);
                pointZ1 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, pointZ1);

                Gizmos.DrawLine(pointY0, pointY1);
                Gizmos.DrawLine(pointZ0, pointZ1);
            }

            if (DuMath.IsNotZero(linearFalloff))
            {
                Vector3 zeroPoint = Vector3.zero;
                zeroPoint.x = GetPowerForLocalPositionInAxisXPlus(new Vector3(0f, linearFalloff, linearFalloff)); // this point will be always outside falloff range
                zeroPoint = AxisDirection.ConvertFromAxisXPlusToDirection(direction, zeroPoint);

                DuGizmos.DrawCircle(linearFalloff, zeroPoint, direction, 32);
            }
        }
#endif

        private void Reset()
        {
            ResetDefaults();
        }
    }
}
