using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(Scale))]
    [CanEditMultipleObjects]
    public class ScaleEditor : DuEditor
    {
        protected DuProperty m_DeltaScale;
        protected DuProperty m_Speed;

        protected DuProperty m_Freeze;

        protected DuProperty m_UpdateMode;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Animations/Scale")]
        [MenuItem("GameObject/Dust/Animations/Scale")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Scale", typeof(Scale));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_DeltaScale = FindProperty("m_DeltaScale", "Delta Scale", "Support only local scale");
            m_Speed = FindProperty("m_Speed", "Speed");

            m_Freeze = FindProperty("m_Freeze", "Freeze");

            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_DeltaScale);
            PropertyExtendedSlider(m_Speed, 0f, 10f, 0.1f);

            Space();

            PropertyField(m_Freeze);

            Space();

            PropertyField(m_UpdateMode);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
