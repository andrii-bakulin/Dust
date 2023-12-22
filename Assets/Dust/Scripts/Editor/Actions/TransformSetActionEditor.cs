using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(TransformSetAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TransformSetActionEditor : InstantActionEditor
    {
        private DuProperty m_PositionEnabled;
        private DuProperty m_RotationEnabled;
        private DuProperty m_ScaleEnabled;

        private DuProperty m_Position;
        private DuProperty m_Rotation;
        private DuProperty m_Scale;

        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static TransformSetActionEditor()
        {
            ActionsPopupButtons.AddActionTransform(typeof(TransformSetAction), "Transform Set");
        }

        [MenuItem("Dust/Actions/Transform Set")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Transform Set Action", typeof(TransformSetAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_PositionEnabled = FindProperty("m_PositionEnabled", "Set Position");
            m_RotationEnabled = FindProperty("m_RotationEnabled", "Set Rotation");
            m_ScaleEnabled = FindProperty("m_ScaleEnabled", "Set Scale");

            m_Position = FindProperty("m_Position", "Position");
            m_Rotation = FindProperty("m_Rotation", "Rotation");
            m_Scale = FindProperty("m_Scale", "Scale");

            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_PositionEnabled);
            PropertyField(m_RotationEnabled);
            PropertyField(m_ScaleEnabled);
                
            Space();

            if (m_PositionEnabled.IsTrue || m_RotationEnabled.IsTrue || m_ScaleEnabled.IsTrue)
            {
                PropertyFieldOrHide(m_Position, !m_PositionEnabled.IsTrue);
                PropertyFieldOrHide(m_Rotation, !m_RotationEnabled.IsTrue);
                PropertyFieldOrHide(m_Scale, !m_ScaleEnabled.IsTrue);

                Space();
            }

            PropertyField(m_Space);
                
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("TransformSetAction");
            OnInspectorGUI_Extended("TransformSetAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
