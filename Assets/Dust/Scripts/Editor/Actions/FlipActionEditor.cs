using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(FlipAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class FlipActionEditor : InstantActionEditor
    {
        private DuProperty m_FlipX;
        private DuProperty m_FlipY;
        private DuProperty m_FlipZ;

        //--------------------------------------------------------------------------------------------------------------

        static FlipActionEditor()
        {
            ActionsPopupButtons.AddActionTransform(typeof(FlipAction), "Flip");
        }

        [MenuItem("Dust/Actions/Flip")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Flip Action", typeof(FlipAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_FlipX = FindProperty("m_FlipX", "FlipX");
            m_FlipY = FindProperty("m_FlipY", "FlipY");
            m_FlipZ = FindProperty("m_FlipZ", "FlipZ");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_FlipX);
            PropertyField(m_FlipY);
            PropertyField(m_FlipZ);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("FlipAction");
            OnInspectorGUI_Extended("FlipAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
