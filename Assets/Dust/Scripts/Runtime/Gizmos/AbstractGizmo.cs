using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class AbstractGizmo : DuMonoBehaviour
    {
        protected static readonly Color k_GizmosDefaultColor = new Color(1.00f, 0.66f, 0.33f);

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Color m_Color = k_GizmosDefaultColor;
        public Color color
        {
            get => m_Color;
            set => m_Color = value;
        }

        [SerializeField]
        private GizmoVisibility m_GizmoVisibility = GizmoVisibility.AlwaysDraw;

        public GizmoVisibility gizmoVisibility
        {
            get => m_GizmoVisibility;
            set => m_GizmoVisibility = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public abstract string GizmoName();

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Selection.activeGameObject == this.gameObject)
                return;

            if (gizmoVisibility == GizmoVisibility.Hidden || gizmoVisibility == GizmoVisibility.DrawOnSelect)
                return; // No need to draw

            DrawGizmos();
        }

        private void OnDrawGizmosSelected()
        {
            if (gizmoVisibility == GizmoVisibility.Hidden)
                return; // No need to draw

            DrawGizmos();
        }

        protected abstract void DrawGizmos();
#endif
    }
}
