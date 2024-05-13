using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(InstantiateAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class InstantiateActionEditor : InstantActionEditor
    {
        protected DuProperty m_Original;
        protected DuProperty m_InstantiateSpace;
        protected DuProperty m_SpaceGameObject;

        protected DuProperty m_PositionEnabled;
        protected DuProperty m_RotationEnabled;
        protected DuProperty m_ScaleEnabled;

        protected DuProperty m_Position;
        protected DuProperty m_Rotation;
        protected DuProperty m_Scale;

        protected DuProperty m_ParentMode;
        protected DuProperty m_ParentGameObject;

        //--------------------------------------------------------------------------------------------------------------

        static InstantiateActionEditor()
        {
            ActionsPopupButtons.AddActionGameObject(typeof(InstantiateAction), "Instantiate");
        }

        [MenuItem("Dust/Actions/Instantiate")]
        [MenuItem("GameObject/Dust/Actions/Instantiate")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Instantiate Action", typeof(InstantiateAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Original = FindProperty("m_Original", "Original");
            m_InstantiateSpace = FindProperty("m_InstantiateSpace", "Instantiate Space");
            m_SpaceGameObject = FindProperty("m_SpaceGameObject", "Space Object");

            m_PositionEnabled = FindProperty("m_PositionEnabled", "Update Position");
            m_RotationEnabled = FindProperty("m_RotationEnabled", "Update Rotation");
            m_ScaleEnabled = FindProperty("m_ScaleEnabled", "Update Scale");
            
            m_Position = FindProperty("m_Position", "Local Position");
            m_Rotation = FindProperty("m_Rotation", "Local Rotation");
            m_Scale = FindProperty("m_Scale", "Local Scale");
            
            m_ParentMode = FindProperty("m_ParentMode", "Assign Parent");
            m_ParentGameObject = FindProperty("m_ParentGameObject", "Parent Object");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_Original);
            PropertyField(m_InstantiateSpace);

            if ((InstantiateAction.Space)m_InstantiateSpace.valInt == InstantiateAction.Space.GameObject)
            {
                PropertyField(m_SpaceGameObject);
            }
            
            Space();

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

            PropertyField(m_ParentMode);

            if ((InstantiateAction.ParentMode)m_ParentMode.valInt == InstantiateAction.ParentMode.GameObject)
            {
                PropertyField(m_ParentGameObject);
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("InstantiateAction");
            OnInspectorGUI_Extended("InstantiateAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
