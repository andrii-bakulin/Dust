using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ConstantField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ConstantFieldEditor : FieldEditor
    {
        protected DuProperty m_Power;
        protected DuProperty m_Color;

        //--------------------------------------------------------------------------------------------------------------

        static ConstantFieldEditor()
        {
            FieldsPopupButtons.AddBasicField(typeof(ConstantField), "Constant");
        }

        [MenuItem("Dust/Fields/Basic Fields/Constant")]
        [MenuItem("GameObject/Dust/Fields/Basic Fields/Constant")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(ConstantField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Power = FindProperty("m_Power", "Power");
            m_Color = FindProperty("m_Color", "Color");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            PropertyExtendedSlider(m_Power, 0f, 5f, 0.01f);
            PropertyField(m_Color);
            Space();

            PropertyField(m_Hint);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
