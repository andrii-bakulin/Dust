using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ClampPosition))]
    [CanEditMultipleObjects]
    public class ClampPositionEditor : ClampTransformEditor
    {
        protected DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Helpers/Clamp Position")]
        [MenuItem("GameObject/Dust/Helpers/Clamp Position")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedObjects(typeof(ClampPosition));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            
            OnInspectorGUI_Main();

            Space();

            PropertyField(m_Space);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
