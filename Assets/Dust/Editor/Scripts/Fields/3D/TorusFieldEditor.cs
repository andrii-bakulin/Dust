using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(TorusField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TorusFieldEditor : SpaceObjectFieldEditor
    {
        protected DuProperty m_Radius;
        protected DuProperty m_Thickness;
        protected DuProperty m_Direction;

        //--------------------------------------------------------------------------------------------------------------

        static TorusFieldEditor()
        {
            FieldsPopupButtons.Add3DField(typeof(TorusField), "Torus");
        }

        [MenuItem("Dust/Fields/3D Fields/Torus")]
        [MenuItem("GameObject/Dust/Fields/3D Fields/Torus")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(TorusField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Radius = FindProperty("m_Radius", "Radius");
            m_Thickness = FindProperty("m_Thickness", "Thickness");
            m_Direction = FindProperty("m_Direction", "Direction");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            PropertyExtendedSlider(m_Power, 0f, 5f, 0.01f);
            Space();

            PropertyExtendedSlider(m_Radius, 0f, 20f, 0.01f);
            PropertyExtendedSlider(m_Thickness, 0f, 20f, 0.01f);
            PropertyField(m_Direction);
            Space();

            PropertyField(m_Unlimited);
            PropertyField(m_Hint);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            
            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_ColoringBlock();
            OnInspectorGUI_GizmoBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Radius.isChanged)
                m_Radius.valFloat = TorusField.NormalizeRadius(m_Radius.valFloat);

            if (m_Thickness.isChanged)
                m_Thickness.valFloat = TorusField.NormalizeThickness(m_Thickness.valFloat);

            InspectorCommitUpdates();
        }
    }
}
