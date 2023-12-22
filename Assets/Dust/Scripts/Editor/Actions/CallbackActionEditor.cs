using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(CallbackAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class CallbackActionEditor : InstantActionEditor
    {
        protected DuProperty m_OnCompleteCallback;

        //--------------------------------------------------------------------------------------------------------------

        static CallbackActionEditor()
        {
            ActionsPopupButtons.AddActionOthers(typeof(CallbackAction), "Callback");
        }

        [MenuItem("Dust/Actions/Callback")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Callback Action", typeof(CallbackAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_OnCompleteCallback = FindProperty("m_OnCompleteCallback", "Callback");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_OnCompleteCallback);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("CallbackAction");
            OnInspectorGUI_Extended("CallbackAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
