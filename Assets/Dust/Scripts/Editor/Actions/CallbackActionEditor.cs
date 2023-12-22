using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(CallbackAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class CallbackActionEditor : InstantActionEditor
    {
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

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            OnInspectorGUI_Callbacks("CallbackAction", callbackExpanded:true);
            OnInspectorGUI_Extended("CallbackAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
