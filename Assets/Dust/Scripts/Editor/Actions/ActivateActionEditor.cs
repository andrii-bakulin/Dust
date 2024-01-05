using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ActivateAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ActivateActionEditor : InstantActionEditor
    {
        protected DuProperty m_ActivationMode;

        protected DuProperty m_ApplyToTarget;
        protected DuProperty m_GameObjects;
        protected DuProperty m_Components;

        protected DuProperty m_Seed;

        protected ActivateAction.ActivationMode activationMode => (ActivateAction.ActivationMode) m_ActivationMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static ActivateActionEditor()
        {
            ActionsPopupButtons.AddActionTransform(typeof(ActivateAction), "Activate");
        }

        [MenuItem("Dust/Actions/Activate")]
        [MenuItem("GameObject/Dust/Actions/Activate")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Activate Action", typeof(ActivateAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ActivationMode = FindProperty("m_ActivationMode", "Activation Mode");

            m_ApplyToTarget = FindProperty("m_ApplyToTarget", "Apply To Target");
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
                
            PropertyField(m_ApplyToTarget);
            Space();

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
