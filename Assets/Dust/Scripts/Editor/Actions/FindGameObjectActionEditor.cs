using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(FindGameObjectAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class FindGameObjectActionEditor : InstantActionEditor
    {
        protected DuProperty m_TagName;
        protected DuProperty m_ObjectName;
        protected DuProperty m_OnFound;

        //--------------------------------------------------------------------------------------------------------------

        static FindGameObjectActionEditor()
        {
            ActionsPopupButtons.AddActionGameObject(typeof(FindGameObjectAction), "Find GameObject");
        }

        [MenuItem("Dust/Actions/Find GameObject")]
        [MenuItem("GameObject/Dust/Actions/Find GameObject")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Find GameObject", typeof(FindGameObjectAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_TagName = FindProperty("m_TagName", "Tag Name");
            m_ObjectName = FindProperty("m_ObjectName", "Object Name");
            m_OnFound = FindProperty("m_OnFound", "OnFound");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_TagName);
            PropertyField(m_ObjectName);
            
            Space();

            PropertyField(m_OnFound);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("FindGameObject");
            OnInspectorGUI_Extended("FindGameObject");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
