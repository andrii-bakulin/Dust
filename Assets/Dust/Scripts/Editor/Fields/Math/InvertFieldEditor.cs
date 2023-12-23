using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(InvertField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class InvertFieldEditor : FieldEditor
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
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(InvertField));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                DustGUI.Header("Color");
                PropertyField(m_ColorInvertAlpha);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
