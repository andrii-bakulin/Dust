using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(RigidbodyAddForceAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class RigidbodyAddForceEditor : InstantActionEditor
    {
        protected DuProperty m_ForceVector;
        protected DuProperty m_ForceMode;
        protected DuProperty m_ForceSpace;

        //--------------------------------------------------------------------------------------------------------------

        static RigidbodyAddForceEditor()
        {
            ActionsPopupButtons.AddActionPhysics(typeof(RigidbodyAddForceAction), "AddForce");
        }

        [MenuItem("Dust/Actions/Physics/AddForce")]
        [MenuItem("GameObject/Dust/Actions/Physics/AddForce")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Rigidbody AddForce Action", typeof(RigidbodyAddForceAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ForceVector = FindProperty("m_ForceVector", "Force Vector");
            m_ForceMode = FindProperty("m_ForceMode", "ForceMode");
            m_ForceSpace = FindProperty("m_ForceSpace", "ForceSpace");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_ForceVector);
            PropertyField(m_ForceMode);
            PropertyField(m_ForceSpace);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("RigidbodyAddForceAction");
            OnInspectorGUI_Extended("RigidbodyAddForceAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
