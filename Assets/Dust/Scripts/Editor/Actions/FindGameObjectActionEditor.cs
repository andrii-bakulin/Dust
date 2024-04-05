using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(FindGameObjectAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class FindGameObjectActionEditor : InstantActionEditor
    {
        protected DuProperty m_Scope;
        protected DuProperty m_TagName;
        protected DuProperty m_ObjectName;
        protected DuProperty m_OnFound;
        protected DuProperty m_OnNotFound;

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

            m_Scope = FindProperty("m_Scope", "Lookup Scope");
            m_TagName = FindProperty("m_TagName", "Tag Name");
            m_ObjectName = FindProperty("m_ObjectName", "Object Name");
            m_OnFound = FindProperty("m_OnFound", "OnFound");
            m_OnNotFound = FindProperty("m_OnNotFound", "OnNotFound");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_Scope);
            PropertyField(m_TagName);
            PropertyField(m_ObjectName);
            
            Space();

            var titleOnFound = "OnFound" + (m_OnFound.valUnityEvent.arraySize > 0 ? " (" + m_OnFound.valUnityEvent.arraySize + ")" : "");
            var titleOnNotFound = "OnNotFound" + (m_OnNotFound.valUnityEvent.arraySize > 0 ? " (" + m_OnNotFound.valUnityEvent.arraySize + ")" : "");

            var tabIndex = DustGUI.Toolbar("FindGameObjectAction.Events", new[] {titleOnFound, titleOnNotFound});

            switch (tabIndex)
            {
                case 0: PropertyField(m_OnFound); break;
                case 1: PropertyField(m_OnNotFound); break;
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("FindGameObject");
            OnInspectorGUI_Extended("FindGameObject");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
