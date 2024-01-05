using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(TransformUpdateAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TransformUpdateActionEditor : InstantActionEditor
    {
        protected DuProperty m_PositionEnabled;
        protected DuProperty m_RotationEnabled;
        protected DuProperty m_ScaleEnabled;

        protected DuProperty m_Position;
        protected DuProperty m_Rotation;
        protected DuProperty m_Scale;

        protected DuProperty m_TransformMode;
        
        protected DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static TransformUpdateActionEditor()
        {
            ActionsPopupButtons.AddActionTransform(typeof(TransformUpdateAction), "Transform Update");
        }

        [MenuItem("Dust/Actions/Transform Update")]
        [MenuItem("GameObject/Dust/Actions/Transform Update")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Transform Update Action", typeof(TransformUpdateAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_PositionEnabled = FindProperty("m_PositionEnabled", "Update Position");
            m_RotationEnabled = FindProperty("m_RotationEnabled", "Update Rotation");
            m_ScaleEnabled = FindProperty("m_ScaleEnabled", "Update Scale");

            m_Position = FindProperty("m_Position", "Position");
            m_Rotation = FindProperty("m_Rotation", "Rotation");
            m_Scale = FindProperty("m_Scale", "Scale");

            m_TransformMode = FindProperty("m_TransformMode", "Transform Mode");

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

            PropertyField(m_TransformMode);
            Space();
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("TransformUpdateAction");
            OnInspectorGUI_Extended("TransformUpdateAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
