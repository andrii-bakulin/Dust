using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(CubeField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class CubeFieldEditor : SpaceFieldEditor
    {
        private DuProperty m_Size;

        //--------------------------------------------------------------------------------------------------------------

        static CubeFieldEditor()
        {
            FieldsPopupButtons.Add3DField(typeof(CubeField), "Cube");
        }

        [MenuItem("Dust/Fields/3D Fields/Cube")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(CubeField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Size = FindProperty("m_Size", "Size");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                PropertyField(m_Size);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_GizmoBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Size.isChanged)
                m_Size.valVector3 = CubeField.NormalizeSize(m_Size.valVector3);

            InspectorCommitUpdates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            foreach (var subTarget in targets)
            {
                var origin = subTarget as CubeField;

                if (m_Size.isChanged || DustGUI.IsUndoRedoPerformed())
                    origin.ResetCalcData();
            }
        }
    }
}
