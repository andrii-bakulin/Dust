using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(RadialField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class RadialFieldEditor : SpaceFieldEditor
    {
        protected DuProperty m_StartAngle;
        protected DuProperty m_EndAngle;

        protected DuProperty m_FadeInOffset;
        protected DuProperty m_FadeOutOffset;

        protected DuProperty m_Iterations;
        protected DuProperty m_Offset;

        protected DuProperty m_Direction;

        protected DuProperty m_GizmoRadius;

        //--------------------------------------------------------------------------------------------------------------

        static RadialFieldEditor()
        {
            FieldsPopupButtons.Add2DField(typeof(RadialField), "Radial");
        }

        [MenuItem("Dust/Fields/2D Fields/Radial")]
        [MenuItem("GameObject/Dust/Fields/2D Fields/Radial")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(RadialField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_StartAngle = FindProperty("m_StartAngle", "Start Angle");
            m_EndAngle = FindProperty("m_EndAngle", "End Angle");

            m_FadeInOffset = FindProperty("m_FadeInOffset", "Fade In Offset");
            m_FadeOutOffset = FindProperty("m_FadeOutOffset", "Fade Out Offset");

            m_Iterations = FindProperty("m_Iterations", "Iterations");
            m_Offset = FindProperty("m_Offset", "Offset");

            m_Direction = FindProperty("m_Direction", "Direction");

            m_GizmoRadius = FindProperty("m_GizmoRadius", "Radius");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            PropertyExtendedSlider(m_Power, 0f, 5f, 0.01f);
            Space();

            PropertyExtendedSlider(m_StartAngle, 0f, 360f, 1f);
            PropertyExtendedSlider(m_EndAngle, 0f, 360f, 1f);
            Space();

            PropertyExtendedSlider(m_FadeInOffset, 0f, 360f, 1f);
            PropertyExtendedSlider(m_FadeOutOffset, 0f, 360f, 1f);
            Space();

            PropertyExtendedSlider(m_Iterations, 1f, 10f, 0.01f, 1f);
            PropertyExtendedSlider(m_Offset, 0f, 360f, 1f);
            Space();

            PropertyField(m_Direction);
            Space();

            PropertyField(m_Hint);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_ColoringBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // OnInspectorGUI_GizmoBlock();

            if (DustGUI.FoldoutBegin("Gizmo", "DuAnyField.Gizmo"))
            {
                PropertyField(m_GizmoVisibility);
                PropertyField(m_GizmoFieldColor);
                PropertyExtendedSlider(m_GizmoRadius, 0f, 5f, 0.1f, 0f);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_FadeInOffset.isChanged)
                m_FadeInOffset.valFloat = RadialField.NormalizeFadeOffset(m_FadeInOffset.valFloat);

            if (m_FadeOutOffset.isChanged)
                m_FadeOutOffset.valFloat = RadialField.NormalizeFadeOffset(m_FadeOutOffset.valFloat);

            if (m_Iterations.isChanged)
                m_Iterations.valFloat = RadialField.NormalizeIterations(m_Iterations.valFloat);

            if (m_GizmoRadius.isChanged)
                m_GizmoRadius.valFloat = RadialField.NormalizeGizmoRadius(m_GizmoRadius.valFloat);

            InspectorCommitUpdates();
        }
    }
}
