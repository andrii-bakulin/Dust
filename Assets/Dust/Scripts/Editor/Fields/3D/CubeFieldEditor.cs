using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(CubeField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class CubeFieldEditor : SpaceObjectFieldEditor
    {
        protected DuProperty m_Size;

        //--------------------------------------------------------------------------------------------------------------

        static CubeFieldEditor()
        {
            FieldsPopupButtons.Add3DField(typeof(CubeField), "Cube");
        }

        [MenuItem("Dust/Fields/3D Fields/Cube")]
        [MenuItem("GameObject/Dust/Fields/3D Fields/Cube")]
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

            InspectorBreadcrumbsForField(this);

            PropertyExtendedSlider(m_Power, 0f, 5f, 0.01f);
            Space();

            PropertyField(m_Size);
            Space();

            PropertyField(m_Unlimited);
            PropertyField(m_Hint);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_ColoringBlock();
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
