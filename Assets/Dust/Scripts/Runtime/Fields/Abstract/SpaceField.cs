using UnityEngine;
using UnityEditor;

namespace Dust
{
    public abstract class SpaceField : BasicField
    {
        public static readonly Color k_GizmosColorRangeZero = new Color(0.0f, 0.3f, 0.6f);
        public static readonly Color k_GizmosColorRangeOne = new Color(0.0f, 0.5f, 1.0f);
        public static readonly Color k_GizmosColorDefaultShape = new Color(0.1f, 0.1f, 0.1f);

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private GizmoVisibility m_GizmoVisibility = GizmoVisibility.DrawOnSelect;
        public GizmoVisibility gizmoVisibility
        {
            get => m_GizmoVisibility;
            set => m_GizmoVisibility = value;
        }

        [SerializeField]
        private bool m_GizmoFieldColor = true;
        public bool gizmoFieldColor
        {
            get => m_GizmoFieldColor;
            set => m_GizmoFieldColor = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, transform);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (Selection.activeGameObject == this.gameObject)
                return;

            if (gizmoVisibility != GizmoVisibility.AlwaysDraw)
                return;

            DrawFieldGizmos();
        }

        void OnDrawGizmosSelected()
        {
            if (gizmoVisibility == GizmoVisibility.Hidden)
                return;

            DrawFieldGizmos();
        }

        protected abstract void DrawFieldGizmos();

        protected Color GetGizmoColorRange0()
        {
            return gizmoFieldColor ? coloring.color * 0.66f : k_GizmosColorRangeZero;
        }

        protected Color GetGizmoColorRange1()
        {
            return gizmoFieldColor ? coloring.color : k_GizmosColorRangeOne;
        }

        protected Color GetGizmoColorDefaultShape()
        {
            return k_GizmosColorDefaultShape;
        }
#endif
    }
}
