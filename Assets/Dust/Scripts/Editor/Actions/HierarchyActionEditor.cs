using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(HierarchyAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class HierarchyActionEditor : InstantActionEditor
    {
        protected DuProperty m_UpdateMode;
        protected DuProperty m_OrderMode;
        protected DuProperty m_ReferenceObject;

        protected HierarchyAction.UpdateMode updateMode
            => (HierarchyAction.UpdateMode) m_UpdateMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static HierarchyActionEditor()
        {
            ActionsPopupButtons.AddActionTransform(typeof(HierarchyAction), "Hierarchy");
        }

        [MenuItem("Dust/Actions/Hierarchy")]
        [MenuItem("GameObject/Dust/Actions/Hierarchy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Hierarchy Action", typeof(HierarchyAction));
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

            if (updateMode == HierarchyAction.UpdateMode.SetTargetAsChildOfReferenceObject ||
                updateMode == HierarchyAction.UpdateMode.SetReferenceObjectAsChildOfTarget)
            {
                PropertyField(m_OrderMode);
            }

            PropertyField(m_ReferenceObject);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("HierarchyAction");
            OnInspectorGUI_Extended("HierarchyAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
