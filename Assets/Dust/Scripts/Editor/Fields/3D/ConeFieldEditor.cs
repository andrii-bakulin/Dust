using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(ConeField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ConeFieldEditor : SpaceFieldEditor
    {
        private DuProperty m_Height;
        private DuProperty m_Radius;
        private DuProperty m_Direction;

        //--------------------------------------------------------------------------------------------------------------

        static ConeFieldEditor()
        {
            FieldsPopupButtons.Add3DField(typeof(ConeField), "Cone");
        }

        [MenuItem("Dust/Fields/3D Fields/Cone")]
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

            if (DustGUI.FoldoutBegin("Field Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                PropertyExtendedSlider(m_Radius, 0f, 20f, 0.01f);
                PropertyExtendedSlider(m_Height, 0f, 20f, 0.01f);
                PropertyField(m_Direction);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();
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
