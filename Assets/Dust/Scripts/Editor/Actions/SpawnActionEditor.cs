using System;
using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(SpawnAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class SpawnActionEditor : InstantActionEditor
    {
        protected DuProperty m_SpawnObjects;
        protected DuProperty m_SpawnObjectsIterate;
        protected DuProperty m_SpawnObjectsSeed;

        protected DuProperty m_SpawnPointMode;
        protected DuProperty m_SpawnPoints;
        protected DuProperty m_SpawnPointsIterate;
        protected DuProperty m_SpawnPointsSeed;

        protected DuProperty m_SphereVolumeRadius;
        protected DuProperty m_BoxVolumeSize;

        protected DuProperty m_MultipleSpawnEnabled;
        protected DuProperty m_MultipleSpawnCount;
        protected DuProperty m_MultipleSpawnSeed;

        protected DuProperty m_ActivateInstance;
        protected DuProperty m_ParentMode;
        protected DuProperty m_ResetRotation;
        protected DuProperty m_ResetScale;

        //--------------------------------------------------------------------------------------------------------------

        static SpawnActionEditor()
        {
            ActionsPopupButtons.AddActionGameObject(typeof(SpawnAction), "Spawn");
        }

        [MenuItem("Dust/Actions/Spawn")]
        [MenuItem("GameObject/Dust/Actions/Spawn")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Spawn Action", typeof(SpawnAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_SpawnObjects = FindProperty("m_SpawnObjects", "Objects To Spawn");
            m_SpawnObjectsIterate = FindProperty("m_SpawnObjectsIterate", "Spawn Objects Iterate");
            m_SpawnObjectsSeed = FindProperty("m_SpawnObjectsSeed", "Seed");

            m_SpawnPointMode = FindProperty("m_SpawnPointMode", "Spawn At Point");
            m_SpawnPoints = FindProperty("m_SpawnPoints", "Spawn Points");
            m_SpawnPointsIterate = FindProperty("m_SpawnPointsIterate", "Spawn Points Iterate");
            m_SpawnPointsSeed = FindProperty("m_SpawnPointsSeed", "Seed");

            m_SphereVolumeRadius = FindProperty("m_SphereVolumeRadius", "Volume Radius");
            m_BoxVolumeSize = FindProperty("m_BoxVolumeSize", "Volume Size");

            m_MultipleSpawnEnabled = FindProperty("m_MultipleSpawnEnabled", "Enabled");
            m_MultipleSpawnCount = FindProperty("m_MultipleSpawnCount", "Spawn Count");
            m_MultipleSpawnSeed = FindProperty("m_MultipleSpawnSeed", "Seed");

            m_ActivateInstance = FindProperty("m_ActivateInstance", "Activate Instance", "If TRUE, all new GameObjects will be forcibly set to active.");
            m_ParentMode = FindProperty("m_ParentMode", "Assign Parent As");
            m_ResetRotation = FindProperty("m_ResetRotation", "Reset Rotation");
            m_ResetScale = FindProperty("m_ResetScale", "Reset Scale");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            OnInspectorGUI_BaseControlUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_SpawnObjects);
                
            Space();

            if (m_SpawnObjects.property.arraySize > 1)
            {
                PropertyField(m_SpawnObjectsIterate);

                if ((SpawnAction.IterateMode) m_SpawnObjectsIterate.valInt == SpawnAction.IterateMode.Random)
                    PropertySeedRandomOrFixed(m_SpawnObjectsSeed);

                Space();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            DustGUI.Header("Spawn At");

            PropertyField(m_SpawnPointMode);

            switch ((SpawnAction.SpawnPointMode) m_SpawnPointMode.valInt)
            {
                case SpawnAction.SpawnPointMode.Self:
                    break;

                case SpawnAction.SpawnPointMode.Points:
                    PropertyField(m_SpawnPoints);

                    if (m_SpawnPoints.property.arraySize > 1)
                    {
                        PropertyField(m_SpawnPointsIterate);

                        if ((SpawnAction.IterateMode) m_SpawnPointsIterate.valInt == SpawnAction.IterateMode.Random)
                            PropertySeedRandomOrFixed(m_SpawnPointsSeed);
                    }
                    break;

                case SpawnAction.SpawnPointMode.SphereVolume:
                    PropertyExtendedSlider(m_SphereVolumeRadius, 0f, 5f, 0.01f, 0f);
                    PropertySeedRandomOrFixed(m_SpawnPointsSeed);
                    break;

                case SpawnAction.SpawnPointMode.BoxVolume:
                    PropertyField(m_BoxVolumeSize);
                    PropertySeedRandomOrFixed(m_SpawnPointsSeed);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            DustGUI.Header("Spawn Multiple Objects");

            PropertyField(m_MultipleSpawnEnabled);

            if (m_MultipleSpawnEnabled.IsTrue)
            {
                PropertyFieldRange(m_MultipleSpawnCount, 0, 16, 1, 0, int.MaxValue);
                PropertySeedRandomOrFixed(m_MultipleSpawnSeed);
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("On Spawn Instance", "SpawnAction.OnSpawnInstance", true))
            {
                PropertyField(m_ActivateInstance);
                PropertyField(m_ParentMode);

                Space();

                PropertyField(m_ResetRotation);
                PropertyField(m_ResetScale);

                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("SpawnAction");
            OnInspectorGUI_Extended("SpawnAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_SphereVolumeRadius.isChanged)
                m_SphereVolumeRadius.valFloat = SpawnAction.NormalizeSphereVolumeRadius(m_SphereVolumeRadius.valFloat);

            if (m_BoxVolumeSize.isChanged)
                m_BoxVolumeSize.valVector3 = SpawnAction.NormalizeBoxVolumeSize(m_BoxVolumeSize.valVector3);

            // @notice: no need to NormalizeMultipleSpawnCount for m_MultipleSpawnCount.
            // It auto-normalized in PropertyFieldRange() method

            InspectorCommitUpdates();
        }
    }
}
