using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(GridFactory))]
    [CanEditMultipleObjects]
    public class GridFactoryEditor : FactoryEditor
    {
        protected DuProperty m_Count;
        protected DuProperty m_Step;

        protected DuProperty m_OffsetDirection;
        protected DuProperty m_OffsetWidth;
        protected DuProperty m_OffsetHeight;

        //--------------------------------------------------------------------------------------------------------------

        private GridFactory.OffsetDirection offsetDirection
            => (GridFactory.OffsetDirection) m_OffsetDirection.valInt;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Factory/Grid Factory")]
        [MenuItem("GameObject/Dust/Factory/Grid Factory")]
        public static void AddComponent()
        {
            CreateFactoryByType(typeof(GridFactory));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Count = FindProperty("m_Count", "Count");
            m_Step = FindProperty("m_Step", "Step");

            m_OffsetDirection = FindProperty("m_OffsetDirection", "Offset Direction");
            m_OffsetWidth = FindProperty("m_OffsetWidth", "Offset Width");
            m_OffsetHeight = FindProperty("m_OffsetHeight", "Offset Height");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_Count);
            PropertyField(m_Step);

            Space();

            PropertyField(m_OffsetDirection);

            if (offsetDirection != GridFactory.OffsetDirection.Disabled)
            {
                PropertyExtendedSlider(m_OffsetWidth, -1f, +1f, 0.01f);
                PropertyExtendedSlider(m_OffsetHeight, -1f, +1f, 0.01f);
            }
            
            Space();

            m_IsRequireRebuildInstances |= m_Count.isChanged;
            m_IsRequireResetupInstances |= m_Step.isChanged;

            m_IsRequireResetupInstances |= m_OffsetDirection.isChanged;
            m_IsRequireResetupInstances |= m_OffsetWidth.isChanged;
            m_IsRequireResetupInstances |= m_OffsetHeight.isChanged;

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
                m_Count.valVector3Int = GridFactory.NormalizeCount(m_Count.valVector3Int);

            InspectorCommitUpdates();
        }
    }
}
