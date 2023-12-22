using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DestroyAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DestroyActionEditor : InstantActionEditor
    {
        private DuProperty m_ApplyToSelf;
        private DuProperty m_GameObjects;
        private DuProperty m_Components;

        //--------------------------------------------------------------------------------------------------------------

        static DestroyActionEditor()
        {
            ActionsPopupButtons.AddActionOthers(typeof(DestroyAction), "Destroy");
        }

        [MenuItem("Dust/Actions/Destroy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Destroy Action", typeof(DestroyAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ApplyToSelf = FindProperty("m_ApplyToSelf", "Apply To Self");
            m_GameObjects = FindProperty("m_GameObjects", "Game Objects");
            m_Components = FindProperty("m_Components", "Components");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            Space();
                
            PropertyField(m_ApplyToSelf);
            PropertyField(m_GameObjects);
            PropertyField(m_Components);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("DestroyAction");
            OnInspectorGUI_Extended("DestroyAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
