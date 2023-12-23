using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(RadialFactory))]
    [CanEditMultipleObjects]
    public class RadialFactoryEditor : FactoryEditor
    {
        private DuProperty m_Count;
        private DuProperty m_Radius;
        private DuProperty m_Orientation;
        private DuProperty m_Align;

        private DuProperty m_LevelsCount;
        private DuProperty m_LevelRadiusOffset;
        private DuProperty m_DeltaCountPerLevel;

        private DuProperty m_StartAngle;
        private DuProperty m_EndAngle;

        private DuProperty m_Offset;
        private DuProperty m_OffsetVariation;
        private DuProperty m_OffsetSeed;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Factory/Radial Factory")]
        public static void AddComponent()
        {
            CreateFactoryByType(typeof(RadialFactory));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Count = FindProperty("m_Count", "Count");
            m_Radius = FindProperty("m_Radius", "Radius");
            m_Orientation = FindProperty("m_Orientation", "Orientation");
            m_Align = FindProperty("m_Align", "Align");

            m_LevelsCount = FindProperty("m_LevelsCount", "Levels Count");
            m_LevelRadiusOffset = FindProperty("m_LevelRadiusOffset", "Radius Offset");
            m_DeltaCountPerLevel = FindProperty("m_DeltaCountPerLevel", "Delta Count");

            m_StartAngle = FindProperty("m_StartAngle", "Start Angle");
            m_EndAngle = FindProperty("m_EndAngle", "End Angle");

            m_Offset = FindProperty("m_Offset", "Offset");
            m_OffsetVariation = FindProperty("m_OffsetVariation", "Offset Variation");
            m_OffsetSeed = FindProperty("m_OffsetSeed", "Offset Seed");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyExtendedIntSlider(m_Count, 1, 100, 1, 1);
            PropertyExtendedSlider(m_Radius, 0f, 10f, 0.01f);
            PropertyField(m_Orientation);
            PropertyField(m_Align);

            Space();

            PropertyExtendedIntSlider(m_LevelsCount, 1, 16, 1, 1);

            if (m_LevelsCount.valInt > 1)
            {
                DustGUI.IndentLevelInc();
                PropertyExtendedSlider(m_LevelRadiusOffset, 0f, 10f, 0.01f);
                PropertyExtendedIntSlider(m_DeltaCountPerLevel, 0, 50, 1);
                DustGUI.IndentLevelDec();
            }
            
            Space();

            PropertyExtendedSlider(m_StartAngle, 0f, 360f, 1f);
            PropertyExtendedSlider(m_EndAngle, 0f, 360f, 1f);
            
            Space();

            PropertyExtendedSlider(m_Offset, 0f, 360f, 1f);
            PropertyExtendedSlider(m_OffsetVariation, 0f, 1f, 0.01f);
            PropertySeedFixed(m_OffsetSeed);
            
            Space();

            m_IsRequireRebuildInstances |= m_Count.isChanged;
            m_IsRequireRebuildInstances |= m_LevelsCount.isChanged;
            m_IsRequireRebuildInstances |= m_DeltaCountPerLevel.isChanged;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            m_IsRequireResetupInstances |= m_Radius.isChanged;
            m_IsRequireResetupInstances |= m_Orientation.isChanged;
            m_IsRequireResetupInstances |= m_Align.isChanged;

            m_IsRequireResetupInstances |= m_LevelRadiusOffset.isChanged;

            m_IsRequireResetupInstances |= m_StartAngle.isChanged;
            m_IsRequireResetupInstances |= m_EndAngle.isChanged;

            m_IsRequireResetupInstances |= m_Offset.isChanged;
            m_IsRequireResetupInstances |= m_OffsetVariation.isChanged;
            m_IsRequireResetupInstances |= m_OffsetSeed.isChanged;

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
                m_Count.valInt = RadialFactory.NormalizeCount(m_Count.valInt);

            if (m_OffsetSeed.isChanged)
                m_OffsetSeed.valInt = RadialFactory.NormalizeOffsetSeed(m_OffsetSeed.valInt);

            if (m_LevelsCount.isChanged)
                m_LevelsCount.valInt = RadialFactory.NormalizeLevelsCount(m_LevelsCount.valInt);

            InspectorCommitUpdates();
        }
    }
}
