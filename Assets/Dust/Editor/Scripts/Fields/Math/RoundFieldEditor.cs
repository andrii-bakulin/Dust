using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(RoundField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class RoundFieldEditor : MathFieldEditor
    {
        protected DuProperty m_RoundMode;
        protected DuProperty m_Distance;

        //--------------------------------------------------------------------------------------------------------------

        static RoundFieldEditor()
        {
            FieldsPopupButtons.AddMathField(typeof(RoundField), "Round");
        }

        [MenuItem("Dust/Fields/Math Fields/Round")]
        [MenuItem("GameObject/Dust/Fields/Math Fields/Round")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(RoundField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_RoundMode = FindProperty("m_RoundMode", "Round Mode");
            m_Distance = FindProperty("m_Distance", "Distance");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            PropertyField(m_RoundMode);
            PropertyExtendedSlider(m_Distance, 0f, 1f, 0.01f);
            Space();

            PropertyField(m_Hint);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
