using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DirectionalField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DirectionalFieldEditor : SpaceFieldEditor
    {
        private DuProperty m_Length;
        private DuProperty m_Direction;

        private DuProperty m_GizmoWidth;
        private DuProperty m_GizmoHeight;

        //--------------------------------------------------------------------------------------------------------------

        static DirectionalFieldEditor()
        {
            FieldsPopupButtons.Add2DField(typeof(DirectionalField), "Directional");
        }

        [MenuItem("Dust/Fields/2D Fields/Directional")]
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

            if (DustGUI.FoldoutBegin("Field Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                PropertyExtendedSlider(m_Length, 0f, 20f, 0.01f);
                PropertyField(m_Direction);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();

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
