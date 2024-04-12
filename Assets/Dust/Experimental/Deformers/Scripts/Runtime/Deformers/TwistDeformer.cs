using System;
using UnityEngine;

namespace Dust.Experimental.Deformers
{
    [AddComponentMenu("Dust/* Experimental/Deformers/Twist Deformer")]
    public class TwistDeformer : AbstractDeformer
    {
        public enum DeformMode
        {
            Limited = 0,
            Unlimited = 1,
            WithinBox = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private DeformMode m_DeformMode = DeformMode.Limited;
        public DeformMode deformMode
        {
            get => m_DeformMode;
            set => m_DeformMode = value;
        }

        [SerializeField]
        private Vector3 m_Size = Vector3.one * 2f;
        public Vector3 size
        {
            get => m_Size;
            set => m_Size = Normalizer.Size(value);
        }

        [SerializeField]
        private float m_Angle = 0f;
        public float angle
        {
            get => m_Angle;
            set => m_Angle = value;
        }

        [SerializeField]
        private Axis6xDirection m_Direction = Axis6xDirection.YPlus;
        public Axis6xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string DeformerName()
        {
            return "Twist";
        }

        public override string DeformerDynamicHint()
        {
            return angle.ToString("F2") + "° in " + AxisDirection.ToString(direction);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected override bool IsRequireDeformPoint(Vector3 localPosition)
        {
            if (deformMode == DeformMode.WithinBox && !IsPointInsideDeformBox(localPosition, size))
                return false;
            
            // @todo: can also detect true/false for DeformMode.Limited mode 

            return true;
        }

        protected override bool DeformPoint(ref Vector3 localPosition, float strength)
        {
            var xpAxisPosition = AxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Deform logic (in X+ direction)

            float twistHalfSize = direction switch
            {
                Axis6xDirection.XPlus => size.x / 2f,
                Axis6xDirection.XMinus => size.x / 2f,

                Axis6xDirection.YPlus => size.y / 2f,
                Axis6xDirection.YMinus => size.y / 2f,

                Axis6xDirection.ZPlus => size.z / 2f,
                Axis6xDirection.ZMinus => size.z / 2f,

                _ => throw new ArgumentOutOfRangeException()
            };

            float deformPower;

            switch (deformMode)
            {
                default:
                case DeformMode.Limited:
                case DeformMode.WithinBox:
                    deformPower = DuMath.Fit(-twistHalfSize, +twistHalfSize, 0f, 1f, xpAxisPosition.x, true);
                    break;

                case DeformMode.Unlimited:
                    deformPower = DuMath.Fit(-twistHalfSize, +twistHalfSize, 0f, 1f, xpAxisPosition.x);
                    break;
            }

            DuMath.RotatePoint(ref xpAxisPosition.y, ref xpAxisPosition.z, deformPower * angle * strength);

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

            DynamicState.Append(ref dynamicState, ++seq, deformMode);
            DynamicState.Append(ref dynamicState, ++seq, size);
            DynamicState.Append(ref dynamicState, ++seq, angle);
            DynamicState.Append(ref dynamicState, ++seq, direction);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawDeformerGizmos()
        {
            Vector3 halfSize = size / 2f;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = enabled ? k_GizmosColorActive : k_GizmosColorDisabled;

            // Bottom
            DrawGizmosLine(new Vector3(+halfSize.x, -halfSize.y, +halfSize.z), new Vector3(+halfSize.x, -halfSize.y, -halfSize.z));
            DrawGizmosLine(new Vector3(+halfSize.x, -halfSize.y, -halfSize.z), new Vector3(-halfSize.x, -halfSize.y, -halfSize.z));
            DrawGizmosLine(new Vector3(-halfSize.x, -halfSize.y, -halfSize.z), new Vector3(-halfSize.x, -halfSize.y, +halfSize.z));
            DrawGizmosLine(new Vector3(-halfSize.x, -halfSize.y, +halfSize.z), new Vector3(+halfSize.x, -halfSize.y, +halfSize.z));

            // Top
            DrawGizmosLine(new Vector3(+halfSize.x, +halfSize.y, +halfSize.z), new Vector3(+halfSize.x, +halfSize.y, -halfSize.z));
            DrawGizmosLine(new Vector3(+halfSize.x, +halfSize.y, -halfSize.z), new Vector3(-halfSize.x, +halfSize.y, -halfSize.z));
            DrawGizmosLine(new Vector3(-halfSize.x, +halfSize.y, -halfSize.z), new Vector3(-halfSize.x, +halfSize.y, +halfSize.z));
            DrawGizmosLine(new Vector3(-halfSize.x, +halfSize.y, +halfSize.z), new Vector3(+halfSize.x, +halfSize.y, +halfSize.z));

            // Side lines
            int steps = Mathf.Abs(Mathf.FloorToInt(angle / 10)) + 10;

            for (int i = 0; i < steps; i++)
            {
                float y0 = Mathf.Lerp(-halfSize.y, +halfSize.y, (float) (i + 0) / steps);
                float y1 = Mathf.Lerp(-halfSize.y, +halfSize.y, (float) (i + 1) / steps);

                DrawGizmosLine(new Vector3(+halfSize.x, y0, +halfSize.z), new Vector3(+halfSize.x, y1, +halfSize.z));
                DrawGizmosLine(new Vector3(+halfSize.x, y0, -halfSize.z), new Vector3(+halfSize.x, y1, -halfSize.z));
                DrawGizmosLine(new Vector3(-halfSize.x, y0, +halfSize.z), new Vector3(-halfSize.x, y1, +halfSize.z));
                DrawGizmosLine(new Vector3(-halfSize.x, y0, -halfSize.z), new Vector3(-halfSize.x, y1, -halfSize.z));
            }
        }

        private void DrawGizmosLine(Vector3 point0, Vector3 point1)
        {
            DeformPoint(ref point0, 1f);
            DeformPoint(ref point1, 1f);
            Gizmos.DrawLine(point0, point1);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static Vector3 Size(Vector3 value)
            {
                return DuVector3.Abs(value);
            }
        }
    }
}
