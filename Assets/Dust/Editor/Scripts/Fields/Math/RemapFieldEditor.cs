using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(RemapField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class RemapFieldEditor : MathFieldEditor
    {
        protected RemappingEditor m_RemappingEditor;
        protected ColoringEditor m_ColoringEditor;

        static RemapFieldEditor()
        {
            FieldsPopupButtons.AddMathField(typeof(RemapField), "Remap");
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_RemappingEditor = new RemappingEditor((target as RemapField).remapping, serializedObject.FindProperty("m_Remapping"));
            m_ColoringEditor = new ColoringEditor(serializedObject.FindProperty("m_Coloring"));
        }

        [MenuItem("Dust/Fields/Math Fields/Remap")]
        [MenuItem("GameObject/Dust/Fields/Math Fields/Remap")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(RemapField));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            PropertyField(m_Hint);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            m_RemappingEditor.OnInspectorGUI();
            m_ColoringEditor.OnInspectorGUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
