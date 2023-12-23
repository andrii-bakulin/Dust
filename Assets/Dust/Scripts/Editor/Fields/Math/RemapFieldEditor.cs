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
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(RemapField));
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
            }
            DustGUI.FoldoutEnd();

            m_RemappingEditor.OnInspectorGUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
