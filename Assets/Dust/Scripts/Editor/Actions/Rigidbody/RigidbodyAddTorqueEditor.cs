using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(RigidbodyAddTorqueAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class RigidbodyAddTorqueEditor : InstantActionEditor
    {
        protected DuProperty m_TorqueVector;
        protected DuProperty m_TorqueMode;
        protected DuProperty m_TorqueSpace;

        //--------------------------------------------------------------------------------------------------------------

        static RigidbodyAddTorqueEditor()
        {
            ActionsPopupButtons.AddActionPhysics(typeof(RigidbodyAddTorqueAction), "AddTorque");
        }

        [MenuItem("Dust/Actions/Physics/AddTorque")]
        [MenuItem("GameObject/Dust/Actions/Physics/AddTorque")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Rigidbody AddTorque Action", typeof(RigidbodyAddTorqueAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_TorqueVector = FindProperty("m_TorqueVector", "Torque Vector");
            m_TorqueMode = FindProperty("m_TorqueMode", "TorqueMode");
            m_TorqueSpace = FindProperty("m_TorqueSpace", "TorqueSpace");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_TorqueVector);
            PropertyField(m_TorqueMode);
            PropertyField(m_TorqueSpace);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("RigidbodyAddTorqueAction");
            OnInspectorGUI_Extended("RigidbodyAddTorqueAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
