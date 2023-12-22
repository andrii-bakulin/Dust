using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(UpdateHierarchyAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class UpdateHierarchyActionEditor : InstantActionEditor
    {
        private DuProperty m_UpdateMode;
        private DuProperty m_OrderMode;
        private DuProperty m_ReferenceObject;

        private UpdateHierarchyAction.UpdateMode updateMode
            => (UpdateHierarchyAction.UpdateMode) m_UpdateMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static UpdateHierarchyActionEditor()
        {
            ActionsPopupButtons.AddActionTransform(typeof(UpdateHierarchyAction), "Update Hierarchy");
        }

        [MenuItem("Dust/Actions/Update Hierarchy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Update Hierarchy Action", typeof(UpdateHierarchyAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
            m_OrderMode = FindProperty("m_OrderMode", "Order Mode");
            m_ReferenceObject = FindProperty("m_ReferenceObject", "Reference Object");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_UpdateMode);

            if (updateMode == UpdateHierarchyAction.UpdateMode.SetTargetAsChildOfReferenceObject ||
                updateMode == UpdateHierarchyAction.UpdateMode.SetReferenceObjectAsChildOfTarget)
            {
                PropertyField(m_OrderMode);
            }

            PropertyField(m_ReferenceObject);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("UpdateHierarchyAction");
            OnInspectorGUI_Extended("UpdateHierarchyAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
