using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(StartAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class StartActionEditor : InstantActionEditor
    {
        static StartActionEditor()
        {
            ActionsPopupButtons.AddActionOthers(typeof(StartAction), "Start");
        }

        [MenuItem("Dust/Actions/Start")]
        [MenuItem("GameObject/Dust/Actions/Start")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Start Action", typeof(StartAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("StartAction");
            OnInspectorGUI_Extended("StartAction", true);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
