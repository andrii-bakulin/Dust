using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(DirectionalField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DirectionalFieldEditor : SpaceObjectFieldEditor
    {
        protected DuProperty m_Length;
        protected DuProperty m_Direction;

        protected DuProperty m_GizmoWidth;
        protected DuProperty m_GizmoHeight;

        //--------------------------------------------------------------------------------------------------------------

        static DirectionalFieldEditor()
        {
            FieldsPopupButtons.Add3DField(typeof(DirectionalField), "Directional");
        }

        [MenuItem("Dust/Fields/3D Fields/Directional")]
        [MenuItem("GameObject/Dust/Fields/3D Fields/Directional")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DirectionalField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Length = FindProperty("m_Length", "Length");
            m_Direction = FindProperty("m_Direction", "Direction");

            m_GizmoWidth = FindProperty("m_GizmoWidth", "Width");
            m_GizmoHeight = FindProperty("m_GizmoHeight", "Height");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            PropertyExtendedSlider(m_Power, 0f, 5f, 0.01f);
            Space();

            PropertyExtendedSlider(m_Length, 0f, 20f, 0.01f);
            PropertyField(m_Direction);
            Space();

            PropertyField(m_Unlimited);
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
                PropertyExtendedSlider(m_GizmoWidth, 0f, 10f, 0.1f, 0f);
                PropertyExtendedSlider(m_GizmoHeight, 0f, 10f, 0.1f, 0f);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Length.isChanged)
                m_Length.valFloat = DirectionalField.NormalizeLength(m_Length.valFloat);

            if (m_GizmoWidth.isChanged)
                m_GizmoWidth.valFloat = DirectionalField.NormalizeGizmoWidth(m_GizmoWidth.valFloat);

            if (m_GizmoHeight.isChanged)
                m_GizmoHeight.valFloat = DirectionalField.NormalizeGizmoHeight(m_GizmoHeight.valFloat);

            InspectorCommitUpdates();
        }
    }
}
