using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(RemapField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class RemapFieldEditor : FieldEditor
    {
        protected RemappingEditor m_RemappingEditor;

        static RemapFieldEditor()
        {
            FieldsPopupButtons.AddMathField(typeof(RemapField), "Remap");
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_RemappingEditor = new RemappingEditor((target as RemapField).remapping, serializedObject.FindProperty("m_Remapping"));
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

            PropertyField(m_CustomHint);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            m_RemappingEditor.OnInspectorGUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
