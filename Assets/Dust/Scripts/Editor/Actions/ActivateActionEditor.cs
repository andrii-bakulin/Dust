using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(ActivateAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ActivateActionEditor : InstantActionEditor
    {
        private DuProperty m_ActivationMode;

        private DuProperty m_ApplyToSelf;
        private DuProperty m_GameObjects;
        private DuProperty m_Components;

        private DuProperty m_Seed;

        private ActivateAction.ActivationMode activationMode => (ActivateAction.ActivationMode) m_ActivationMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static ActivateActionEditor()
        {
            ActionsPopupButtons.AddActionTransform(typeof(ActivateAction), "Activate");
        }

        [MenuItem("Dust/Actions/Activate")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Activate Action", typeof(ActivateAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ActivationMode = FindProperty("m_ActivationMode", "Activation Mode");

            m_ApplyToSelf = FindProperty("m_ApplyToSelf", "Apply To Self");
            m_GameObjects = FindProperty("m_GameObjects", "Game Objects");
            m_Components = FindProperty("m_Components", "Components");

            m_Seed = FindProperty("m_Seed");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_ActivationMode);

            if (activationMode == ActivateAction.ActivationMode.ToggleRandom)
                PropertySeedRandomOrFixed(m_Seed);

            Space();
                
            PropertyField(m_ApplyToSelf);
            PropertyField(m_GameObjects);
            PropertyField(m_Components);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("ActivateAction");
            OnInspectorGUI_Extended("ActivateAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
