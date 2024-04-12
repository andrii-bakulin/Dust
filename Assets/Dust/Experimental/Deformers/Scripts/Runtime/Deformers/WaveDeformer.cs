using UnityEngine;
using UnityEditor;

namespace Dust.Experimental.Deformers
{
    [AddComponentMenu("Dust/* Experimental/Deformers/Wave Deformer")]
    public class WaveDeformer : AbstractDeformer
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
        private float m_Frequency = 1f;
        public float frequency
        {
            get => m_Frequency;
            set => m_Frequency = value;
        }

        [SerializeField]
        private bool m_UseFalloff;
        public bool useFalloff
        {
            get => m_UseFalloff;
            set => m_UseFalloff = value;
        }

        [SerializeField]
        private float m_LinearFalloff;
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
        private float m_GizmoOffset;
        public float gizmoOffset
        {
            get => m_GizmoOffset;
            set => m_GizmoOffset = value;
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

                // @todo: fix that!! need to call
                // DustGUIRuntime.ForcedRedrawSceneView();
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

        public override string DeformerName()
        {
            return "Wave";
        }

        public override string DeformerDynamicHint()
        {
            return "";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected override bool IsRequireDeformPoint(Vector3 localPosition)
        {
            if (!useFalloff)
                return true;

            // xp = x+
            var xpAxisPosition = AxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);
            
            float distance = DuMath.Length(xpAxisPosition.y, xpAxisPosition.z);

            return distance < linearFalloff;
        }

        protected override bool DeformPoint(ref Vector3 localPosition, float strength)
        {
            // xp = x+
            var xpAxisPosition = AxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Deform logic (in X+ direction)

            float distance = DuMath.Length(xpAxisPosition.y, xpAxisPosition.z);

            if (useFalloff && distance >= linearFalloff)
                return false;

            float sinOffset = distance * frequency - (offset + 0.75f) - m_OffsetDynamic;
            float waveOffset = Mathf.Sin(Constants.PI2 * sinOffset) * amplitude / 2f;

            if (useFalloff)
                waveOffset *= Mathf.Clamp01((linearFalloff - distance) / linearFalloff);

            xpAxisPosition.x += waveOffset * strength;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            localPosition = AxisDirection.ConvertFromAxisXPlusToDirection(direction, xpAxisPosition);
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, amplitude);
            DynamicState.Append(ref dynamicState, ++seq, frequency);
            DynamicState.Append(ref dynamicState, ++seq, useFalloff);
            DynamicState.Append(ref dynamicState, ++seq, linearFalloff);

            DynamicState.Append(ref dynamicState, ++seq, offset);
            DynamicState.Append(ref dynamicState, ++seq, animationSpeed);
            DynamicState.Append(ref dynamicState, ++seq, direction);

            DynamicState.Append(ref dynamicState, ++seq, m_OffsetDynamic);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawDeformerGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = enabled ? k_GizmosColorActive : k_GizmosColorDisabled;

            int segments;

            switch (gizmoQuality)
            {
                default:
                case GizmoQuality.Low:       segments = 12; break;
                case GizmoQuality.Medium:    segments = 24; break;
                case GizmoQuality.High:      segments = 48; break;
                case GizmoQuality.ExtraHigh: segments = 96; break;
            }

            Vector3 zeroPoint = new Vector3(gizmoOffset, -gizmoSize / 2f, -gizmoSize / 2f);

            float delta = gizmoSize / segments;

            for (int s0 = 0; s0 <= segments; s0++)
            for (int s1 = 0; s1 < segments; s1++)
            {
                Vector3 pointY0, pointY1;
                Vector3 pointZ0, pointZ1;

                pointY0 = zeroPoint + new Vector3(0f, (s1 + 0) * delta, s0 * delta);
                pointY1 = zeroPoint + new Vector3(0f, (s1 + 1) * delta, s0 * delta);
                pointZ0 = new Vector3(pointY0.x, pointY0.z, pointY0.y); // invert Y-Z
                pointZ1 = new Vector3(pointY1.x, pointY1.z, pointY1.y); // invert Y-Z

                pointY0 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, pointY0);
                pointY1 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, pointY1);
                pointZ0 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, pointZ0);
                pointZ1 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, pointZ1);

                DeformPoint(ref pointY0, 1f);
                DeformPoint(ref pointY1, 1f);
                DeformPoint(ref pointZ0, 1f);
                DeformPoint(ref pointZ1, 1f);

                Gizmos.DrawLine(pointY0, pointY1);
                Gizmos.DrawLine(pointZ0, pointZ1);
            }

            if (useFalloff)
            {
                var offset = AxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(gizmoOffset, 0f, 0f));
                DuGizmos.DrawCircle(linearFalloff, offset, direction, 32);
            }
        }
#endif
    }
}
