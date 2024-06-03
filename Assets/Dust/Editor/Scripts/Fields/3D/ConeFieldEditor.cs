using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ConeField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ConeFieldEditor : SpaceObjectFieldEditor
    {
        protected DuProperty m_Height;
        protected DuProperty m_Radius;
        protected DuProperty m_Direction;

        //--------------------------------------------------------------------------------------------------------------

        static ConeFieldEditor()
        {
            FieldsPopupButtons.Add3DField(typeof(ConeField), "Cone");
        }

        [MenuItem("Dust/Fields/3D Fields/Cone")]
        [MenuItem("GameObject/Dust/Fields/3D Fields/Cone")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(ConeField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Height = FindProperty("m_Height", "Height");
            m_Radius = FindProperty("m_Radius", "Radius");
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
            PropertyExtendedSlider(m_Height, 0f, 20f, 0.01f);
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

            if (m_Height.isChanged)
                m_Height.valFloat = ConeField.NormalizeHeight(m_Height.valFloat);

            if (m_Radius.isChanged)
                m_Radius.valFloat = ConeField.NormalizeRadius(m_Radius.valFloat);

            InspectorCommitUpdates();
        }
    }
}
