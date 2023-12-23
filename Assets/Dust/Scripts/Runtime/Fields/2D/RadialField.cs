using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/2D Fields/Radial Field")]
    public class RadialField : SpaceField
    {
        [SerializeField]
        private float m_StartAngle = 0.0f;
        public float startAngle
        {
            get => m_StartAngle;
            set => m_StartAngle = value;
        }

        [SerializeField]
        private float m_EndAngle = 360.0f;
        public float endAngle
        {
            get => m_EndAngle;
            set => m_EndAngle = value;
        }

        [SerializeField]
        private float m_FadeInOffset = 45.0f;
        public float fadeInOffset
        {
            get => m_FadeInOffset;
            set => m_FadeInOffset = NormalizeFadeOffset(value);
        }

        [SerializeField]
        private float m_FadeOutOffset = 45.0f;
        public float fadeOutOffset
        {
            get => m_FadeOutOffset;
            set => m_FadeOutOffset = NormalizeFadeOffset(value);
        }

        [SerializeField]
        private float m_Iterations = 1.0f;
        public float iterations
        {
            get => m_Iterations;
            set => m_Iterations = NormalizeIterations(value);
        }

        [SerializeField]
        private float m_Offset = 0.0f;
        public float offset
        {
            get => m_Offset;
            set => m_Offset = value;
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
        private float m_GizmoRadius = 1.0f;
        public float gizmoRadius
        {
            get => m_GizmoRadius;
            set => m_GizmoRadius = NormalizeGizmoRadius(value);
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, startAngle);
            DynamicState.Append(ref dynamicState, ++seq, endAngle);
            DynamicState.Append(ref dynamicState, ++seq, fadeInOffset);
            DynamicState.Append(ref dynamicState, ++seq, fadeOutOffset);
            DynamicState.Append(ref dynamicState, ++seq, iterations);
            DynamicState.Append(ref dynamicState, ++seq, offset);
            DynamicState.Append(ref dynamicState, ++seq, direction);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Radial";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------
        //
        //               *--------------*                    A. Start Angle
        //             /                 \                   B. Start Angle + FadeIn
        //           /                    \                  C. End Angle - FadeOut
        //         /                       \                 D. End Angle
        // *-----*                          *------*
        // 0'    A       B              C   D      360'
        //
        public override void Calculate(Field.Point fieldPoint, out Field.Result result, bool calculateColor)
        {
            Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

            // Convert to [X+]-axis-space by direction
            localPosition = AxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            float angle = Vector2.SignedAngle(Vector2.down, new Vector2(localPosition.y, localPosition.z));
            angle = DuMath.NormalizeAngle360(angle - offset);        // step 1
            angle = DuMath.NormalizeAngle360(angle * iterations);    // step 2

            float power = 0f;

            if (DuMath.Between(angle, startAngle, endAngle))
            {
                float pointA = startAngle;
                float pointB = startAngle + fadeInOffset;
                float pointC = endAngle - fadeOutOffset;
                float pointD = endAngle;

                if (DuMath.Between(angle, pointB, pointC))
                {
                    power = 1f;
                }
                else // somewhere in fade-in-out. But if in both in+out then get min value
                {
                    bool inRangeAB = DuMath.Between(angle, pointA, pointB);
                    bool inRangeCD = DuMath.Between(angle, pointC, pointD);

                    if (inRangeAB && inRangeCD)
                    {
                        power = Mathf.Min(Mathf.InverseLerp(pointA, pointB, angle), 1f - Mathf.InverseLerp(pointC, pointD, angle));
                    }
                    else if (inRangeAB)
                    {
                        power = Mathf.InverseLerp(pointA, pointB, angle);
                    }
                    else if (inRangeCD)
                    {
                        power = 1f - Mathf.InverseLerp(pointC, pointD, angle);
                    }
                }
            }

            result.fieldPower = remapping.MapValue(power);
            result.fieldColor = GetFieldColorFromRemapping(remapping, result.fieldPower, calculateColor);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Color colorRange0 = GetGizmoColorRange0();
            Color colorRange1 = GetGizmoColorRange1();

            if (remapping.remapPowerEnabled)
            {
                Gizmos.color = !remapping.invert ? colorRange1 : colorRange0;
                DrawRadialGizmo(startAngle + fadeInOffset, endAngle - fadeOutOffset, 0.98f);

                Gizmos.color = !remapping.invert ? colorRange0 : colorRange1;
                DrawRadialGizmo(startAngle, endAngle, 1f);
            }
            else
            {
                Gizmos.color = colorRange0;
                DrawRadialGizmo(startAngle, endAngle, 1f);
            }
        }

        protected void DrawRadialGizmo(float angleStart, float angleEnd, float scale)
        {
            int points = 64;

            float angleDelta = (angleEnd - angleStart) / points;

            for (int i = 0; i < points; i++)
            {
                float angle0 = angleStart + i * angleDelta;
                float angle1 = angleStart + (i + 1) * angleDelta;

                Vector3 p0 = DuGizmos.GetCirclePointByAngle(180f - angle0, direction) * gizmoRadius * scale;
                Vector3 p1 = DuGizmos.GetCirclePointByAngle(180f - angle1, direction) * gizmoRadius * scale;

                Gizmos.DrawLine(p0, p1);
            }

            Vector3 pMid = Vector3.zero;
            Vector3 pBeg = DuGizmos.GetCirclePointByAngle(180f - angleStart, direction) * gizmoRadius * scale;
            Vector3 pEnd = DuGizmos.GetCirclePointByAngle(180f - angleEnd, direction) * gizmoRadius * scale;

            Gizmos.DrawLine(pMid, pBeg);
            Gizmos.DrawLine(pMid, pEnd);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeFadeOffset(float value)
        {
            return Mathf.Clamp(value, 0f, 360f);
        }

        public static float NormalizeIterations(float value)
        {
            return Mathf.Max(1f, value);
        }

        public static float NormalizeGizmoRadius(float value)
        {
            return Mathf.Abs(value);
        }
    }
}
