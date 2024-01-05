using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(FieldsSpace))]
    public class FieldsSpaceEditor : DuEditor
    {
        protected DuProperty m_CalculatePower;
        protected DuProperty m_CalculateColor;

        private FieldsMapEditor m_FieldsMapEditor;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Fields/Fields Space")]
        [MenuItem("GameObject/Dust/Fields/Fields Space")]
        protected static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Fields Space", typeof(FieldsSpace));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            SerializedProperty propertyFieldsMap = serializedObject.FindProperty("m_FieldsMap");

            m_CalculatePower = FindProperty(propertyFieldsMap, "m_CalculatePower", "Calculate Power");
            m_CalculateColor = FindProperty(propertyFieldsMap, "m_CalculateColor", "Calculate Color");

            m_FieldsMapEditor = new FieldsMapEditor(this, propertyFieldsMap, (target as FieldsSpace).fieldsMap);
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Parameters", "FieldsSpace.Parameters"))
            {
                PropertyField(m_CalculatePower);
                PropertyField(m_CalculateColor);
                Space();
            }
            DustGUI.FoldoutEnd();

            m_FieldsMapEditor.OnInspectorGUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
