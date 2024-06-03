using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(InvertField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class InvertFieldEditor : MathFieldEditor
    {
        protected DuProperty m_ColorInvertAlpha;

        //--------------------------------------------------------------------------------------------------------------

        static InvertFieldEditor()
        {
            FieldsPopupButtons.AddMathField(typeof(InvertField), "Invert");
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ColorInvertAlpha = FindProperty("m_ColorInvertAlpha", "Invert Alpha");
        }

        [MenuItem("Dust/Fields/Math Fields/Invert")]
        [MenuItem("GameObject/Dust/Fields/Math Fields/Invert")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(InvertField));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            DustGUI.Header("Color");
            PropertyField(m_ColorInvertAlpha);
            Space();

            PropertyField(m_Hint);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
