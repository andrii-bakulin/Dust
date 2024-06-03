using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ClampScale))]
    [CanEditMultipleObjects]
    public class ClampScaleEditor : ClampTransformEditor
    {
        [MenuItem("Dust/Helpers/Clamp Scale")]
        [MenuItem("GameObject/Dust/Helpers/Clamp Scale")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedObjects(typeof(ClampScale));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            
            OnInspectorGUI_Main();

            Space();
            
            DustGUI.Label("Local Space Only");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
