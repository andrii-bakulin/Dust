using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(LookAtFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class LookAtFactoryMachineEditor : BasicFactoryMachineEditor
    {
        protected DuProperty m_TargetMode;
        protected DuProperty m_TargetObject;

        protected DuProperty m_UpVectorMode;
        protected DuProperty m_UpVectorObject;

        protected DuProperty m_LockAxisX;
        protected DuProperty m_LockAxisY;
        protected DuProperty m_LockAxisZ;

        //--------------------------------------------------------------------------------------------------------------

        static LookAtFactoryMachineEditor()
        {
            FactoryMachinesPopupButtons.AddMachine(typeof(LookAtFactoryMachine), "LookAt");
        }

        [MenuItem("Dust/Factory Machines/LookAt")]
        [MenuItem("GameObject/Dust/Factory Machines/LookAt")]
        public new static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(LookAtFactoryMachine));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_TargetMode = FindProperty("m_TargetMode", "Target Mode");
            m_TargetObject = FindProperty("m_TargetObject", "Target Object");

            m_UpVectorMode = FindProperty("m_UpVectorMode", "Up Vector Mode");
            m_UpVectorObject = FindProperty("m_UpVectorObject", "Up Vector Object");

            m_LockAxisX = FindProperty("m_LockAxisX", "Lock Axis X");
            m_LockAxisY = FindProperty("m_LockAxisY", "Lock Axis Y");
            m_LockAxisZ = FindProperty("m_LockAxisZ", "Lock Axis Z");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForFactoryMachine(this);

            PropertyField(m_Hint);
            PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_TargetMode);
            PropertyFieldOrHide(m_TargetObject, (LookAtFactoryMachine.TargetMode) m_TargetMode.valInt
                                                != LookAtFactoryMachine.TargetMode.ObjectTarget);
            Space();

            PropertyField(m_UpVectorMode);
            PropertyFieldOrHide(m_UpVectorObject, (LookAtFactoryMachine.UpVectorMode) m_UpVectorMode.valInt
                                                  != LookAtFactoryMachine.UpVectorMode.Object);
            Space();

            PropertyField(m_LockAxisX);
            PropertyField(m_LockAxisY);
            PropertyField(m_LockAxisZ);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_FieldsMap();

            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
