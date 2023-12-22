using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(TransformCopyAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TransformCopyActionEditor : InstantActionEditor
    {
        private DuProperty m_Position;
        private DuProperty m_Rotation;
        private DuProperty m_Scale;

        private DuProperty m_SourceObject;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static TransformCopyActionEditor()
        {
            ActionsPopupButtons.AddActionTransform(typeof(TransformCopyAction), "Transform Copy");
        }

        [MenuItem("Dust/Actions/Transform Copy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Transform Copy Action", typeof(TransformCopyAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Position = FindProperty("m_Position", "Copy Position");
            m_Rotation = FindProperty("m_Rotation", "Copy Rotation");
            m_Scale = FindProperty("m_Scale", "Copy Scale");

            m_SourceObject = FindProperty("m_SourceObject", "Source Object");
            m_Space = FindProperty("m_Space", "Copy In Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_Position);
            PropertyField(m_Rotation);
            PropertyField(m_Scale);

            Space();
                
            PropertyField(m_SourceObject);
            PropertyField(m_Space);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("TransformCopyAction");
            OnInspectorGUI_Extended("TransformCopyAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
