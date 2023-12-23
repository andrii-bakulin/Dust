using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class SpaceField : Field
    {
        [SerializeField]
        private Remapping m_Remapping = new Remapping();
        public Remapping remapping => m_Remapping;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

        public override bool IsAllowCalculateFieldColor()
        {
            return remapping.colorMode != Remapping.ColorMode.Ignore;
        }

#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return true;
        }

        public override Gradient GetFieldColorPreview(out float colorPower)
        {
            return GetFieldColorPreview(remapping, out colorPower);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, transform);
            DynamicState.Append(ref dynamicState, ++seq, remapping);

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
            return gizmoFieldColor ? remapping.color * 0.66f : k_GizmosColorRangeZero;
        }

        protected Color GetGizmoColorRange1()
        {
            return gizmoFieldColor ? remapping.color : k_GizmosColorRangeOne;
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        private void Reset()
        {
            ResetDefaults();
        }

        protected void ResetDefaults()
        {
            // Use this method to reset values for default to remapping object
        }
    }
}
