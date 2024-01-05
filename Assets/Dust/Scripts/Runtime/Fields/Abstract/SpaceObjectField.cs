using UnityEngine;
using UnityEditor;

namespace Dust
{
    public abstract class SpaceObjectField : SpaceField
    {
        [SerializeField]
        private bool m_Unlimited;
        public bool unlimited
        {
            get => m_Unlimited;
            set => m_Unlimited = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, unlimited);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Color colorRange0 = GetGizmoColorRange0();
            Color colorRange1 = GetGizmoColorRange1();

            float innerMinScale = 1f - remapping.inMin;
            float innerMaxScale = 1f - remapping.inMax;

            if (remapping.remapPowerEnabled)
            {
                if (innerMinScale < 1f && innerMaxScale < 1f)
                {
                    Gizmos.color = GetGizmoColorDefaultShape();
                    DrawFieldGizmo(1f);
                }

                if (innerMaxScale > 0f || !Mathf.Approximately(innerMinScale, innerMaxScale))
                {
                    Gizmos.color = !remapping.invert ? colorRange1 : colorRange0;
                    DrawFieldGizmo(innerMaxScale);
                }

                if (innerMinScale > 0f)
                {
                    Gizmos.color = !remapping.invert ? colorRange0 : colorRange1;
                    DrawFieldGizmo(innerMinScale);
                }
            }
            else
            {
                Gizmos.color = colorRange1;
                DrawFieldGizmo(1f);
            }
        }

        protected abstract void DrawFieldGizmo(float scale);
#endif
    }
}
