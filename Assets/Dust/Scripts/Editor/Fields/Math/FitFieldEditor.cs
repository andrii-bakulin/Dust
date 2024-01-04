using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(FitField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class FitFieldEditor : MathFieldEditor
    {
        protected DuProperty m_MinInput;
        protected DuProperty m_MaxInput;

        protected DuProperty m_MinOutput;
        protected DuProperty m_MaxOutput;

        //--------------------------------------------------------------------------------------------------------------

        static FitFieldEditor()
        {
            FieldsPopupButtons.AddMathField(typeof(FitField), "Fit");
        }

        [MenuItem("Dust/Fields/Math Fields/Fit")]
        [MenuItem("GameObject/Dust/Fields/Math Fields/Fit")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(FitField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_MinInput = FindProperty("m_MinInput", "Min Input");
            m_MaxInput = FindProperty("m_MaxInput", "Max Input");

            m_MinOutput = FindProperty("m_MinOutput", "Min Output");
            m_MaxOutput = FindProperty("m_MaxOutput", "Max Output");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            PropertyExtendedSlider(m_MinInput, -1f, +2f, 0.01f);
            PropertyExtendedSlider(m_MaxInput, -1f, +2f, 0.01f);
            Space();

            PropertyExtendedSlider(m_MinOutput, -1f, +2f, 0.01f);
            PropertyExtendedSlider(m_MaxOutput, -1f, +2f, 0.01f);
            Space();

            PropertyField(m_Hint);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
