using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(SphereField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class SphereFieldEditor : SpaceObjectFieldEditor
    {
        protected DuProperty m_Radius;

        //--------------------------------------------------------------------------------------------------------------

        static SphereFieldEditor()
        {
            FieldsPopupButtons.Add3DField(typeof(SphereField), "Sphere");
        }

        [MenuItem("Dust/Fields/3D Fields/Sphere")]
        [MenuItem("GameObject/Dust/Fields/3D Fields/Sphere")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(SphereField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Radius = FindProperty("m_Radius", "Radius");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            PropertyExtendedSlider(m_Power, 0f, 5f, 0.01f);
            Space();

            PropertyExtendedSlider(m_Radius, 0f, 20f, 0.01f);
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
                m_Radius.valFloat = SphereField.NormalizeRadius(m_Radius.valFloat);

            InspectorCommitUpdates();
        }
    }
}
