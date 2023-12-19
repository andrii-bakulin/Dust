using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(Rotate))]
    [CanEditMultipleObjects]
    public class RotateEditor : DuEditor
    {
        private DuProperty m_Axis;
        private DuProperty m_Speed;
        private DuProperty m_Space;

        private DuProperty m_RotateAroundObject;

        private DuProperty m_UpdateMode;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private Rotate.Space space => (Rotate.Space) m_Space.valInt;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Animations/Rotate")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Rotate", typeof(Rotate));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Axis = FindProperty("m_Axis", "Axis");
            m_Speed = FindProperty("m_Speed", "Speed");
            m_Space = FindProperty("m_Space", "Space");

            m_RotateAroundObject = FindProperty("m_RotateAroundObject", "Rotate Around Object");

            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_Axis);
            PropertyExtendedSlider(m_Speed, -180f, +180f, 1f);
            PropertyField(m_Space);

            if (space == Rotate.Space.AroundObject)
            {
                PropertyField(m_RotateAroundObject);
            }

            Space();

            PropertyField(m_UpdateMode);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
