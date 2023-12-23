using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(ConstantField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ConstantFieldEditor : FieldEditor
    {
        private DuProperty m_Power;
        private DuProperty m_Color;

        //--------------------------------------------------------------------------------------------------------------

        static ConstantFieldEditor()
        {
            FieldsPopupButtons.AddBasicField(typeof(ConstantField), "Constant");
        }

        [MenuItem("Dust/Fields/Basic Fields/Constant")]
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

            if (DustGUI.FoldoutBegin("Field Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                PropertyExtendedSlider(m_Power, 0f, 1f, 0.01f);
                PropertyField(m_Color);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
