using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(LinearFactory))]
    [CanEditMultipleObjects]
    public class LinearFactoryEditor : FactoryEditor
    {
        protected DuProperty m_Count;
        protected DuProperty m_Offset;
        protected DuProperty m_Amount;
        protected DuProperty m_Position;
        protected DuProperty m_Rotation;
        protected DuProperty m_Scale;
        protected DuProperty m_StepRotation;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Factory/Linear Factory")]
        [MenuItem("GameObject/Dust/Factory/Linear Factory")]
        public static void AddComponent()
        {
            CreateFactoryByType(typeof(LinearFactory));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Count = FindProperty("m_Count", "Count");
            m_Offset = FindProperty("m_Offset", "Offset");
            m_Amount = FindProperty("m_Amount", "Amount");
            m_Position = FindProperty("m_Position", "Position");
            m_Rotation = FindProperty("m_Rotation", "Rotation");
            m_Scale = FindProperty("m_Scale", "Scale");
            m_StepRotation = FindProperty("m_StepRotation", "Step Rotation");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyExtendedIntSlider(m_Count, 0, 100, 1, 0);
            PropertyExtendedIntSlider(m_Offset, 0, 100, 1, 0);
            
            Space();
            
            PropertyField(m_Position);
            PropertyField(m_Rotation);
            PropertyField(m_Scale);
            PropertyExtendedSlider(m_Amount, 0f, 1f, 0.01f);
            
            Space();

            PropertyField(m_StepRotation);
            
            Space();

            m_IsRequireRebuildInstances |= m_Count.isChanged;

            m_IsRequireResetupInstances |= m_Offset.isChanged;
            m_IsRequireResetupInstances |= m_Amount.isChanged;
            m_IsRequireResetupInstances |= m_Position.isChanged;
            m_IsRequireResetupInstances |= m_Rotation.isChanged;
            m_IsRequireResetupInstances |= m_Scale.isChanged;
            m_IsRequireResetupInstances |= m_StepRotation.isChanged;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_SourceObjects();
            OnInspectorGUI_Instances();
            OnInspectorGUI_FactoryMachines();
            OnInspectorGUI_Transform();
            OnInspectorGUI_Gizmos();
            OnInspectorGUI_Tools();

            //----------------------------------------------------------------------------------------------------------
            // Validate & Normalize Data

            if (m_Count.isChanged)
                m_Count.valInt = LinearFactory.NormalizeCount(m_Count.valInt);

            if (m_Offset.isChanged)
                m_Offset.valInt = LinearFactory.NormalizeOffset(m_Offset.valInt);

            InspectorCommitUpdates();
        }
    }
}
