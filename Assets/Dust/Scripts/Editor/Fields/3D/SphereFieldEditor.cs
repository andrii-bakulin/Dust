using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(SphereField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class SphereFieldEditor : SpaceFieldEditor
    {
        private DuProperty m_Radius;

        //--------------------------------------------------------------------------------------------------------------

        static SphereFieldEditor()
        {
            FieldsPopupButtons.Add3DField(typeof(SphereField), "Sphere");
        }

        [MenuItem("Dust/Fields/3D Fields/Sphere")]
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

            if (DustGUI.FoldoutBegin("Field Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                PropertyExtendedSlider(m_Radius, 0f, 20f, 0.01f);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_GizmoBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Radius.isChanged)
                m_Radius.valFloat = SphereField.NormalizeRadius(m_Radius.valFloat);

            InspectorCommitUpdates();
        }
    }
}
