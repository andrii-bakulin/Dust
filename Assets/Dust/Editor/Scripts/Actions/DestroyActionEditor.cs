using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(DestroyAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DestroyActionEditor : InstantActionEditor
    {
        protected DuProperty m_ApplyToTarget;
        protected DuProperty m_GameObjects;
        protected DuProperty m_Components;

        //--------------------------------------------------------------------------------------------------------------

        static DestroyActionEditor()
        {
            ActionsPopupButtons.AddActionGameObject(typeof(DestroyAction), "Destroy");
        }

        [MenuItem("Dust/Actions/Destroy")]
        [MenuItem("GameObject/Dust/Actions/Destroy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Destroy Action", typeof(DestroyAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ApplyToTarget = FindProperty("m_ApplyToTarget", "Apply To Target");
            m_GameObjects = FindProperty("m_GameObjects", "Game Objects");
            m_Components = FindProperty("m_Components", "Components");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();
            Space();
                
            PropertyField(m_ApplyToTarget);
            Space();
            
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
