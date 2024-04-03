using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(Spawner))]
    [CanEditMultipleObjects]
    public class SpawnerEditor : DuEditor
    {
        protected DuProperty m_SpawnEvent;
        protected DuProperty m_Interval;
        protected DuProperty m_IntervalRange;

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

        protected DuProperty m_ParentMode;
        protected DuProperty m_Limit;
        protected DuProperty m_SpawnOnAwake;

        protected DuProperty m_ActivateInstance;
        protected DuProperty m_ResetRotation;
        protected DuProperty m_ResetScale;

        protected DuProperty m_OnSpawn;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Helpers/Spawner")]
        [MenuItem("GameObject/Dust/Helpers/Spawner")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Spawner", typeof(Spawner));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_SpawnEvent = FindProperty("m_SpawnEvent", "Spawn Event");
            m_Interval = FindProperty("m_Interval", "Interval");
            m_IntervalRange = FindProperty("m_IntervalRange", "Interval Range");

            m_SpawnObjects = FindProperty("m_SpawnObjects", "Objects To Spawn");
            m_SpawnObjectsIterate = FindProperty("m_SpawnObjectsIterate", "Objects Iterate");
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

            m_ParentMode = FindProperty("m_ParentMode", "Assign Parent As");
            m_Limit = FindProperty("m_Limit", "Total Limit");
            m_SpawnOnAwake = FindProperty("m_SpawnOnAwake", "Spawn On Awake");

            m_ActivateInstance = FindProperty("m_ActivateInstance", "Activate Instance", "If TRUE, all new GameObjects will be forcibly set to active.");
            m_ResetRotation = FindProperty("m_ResetRotation", "Reset Rotation");
            m_ResetScale = FindProperty("m_ResetScale", "Reset Scale");
            
            m_OnSpawn = FindProperty("m_OnSpawn", "On Spawn");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_SpawnEvent);

            switch ((Spawner.SpawnEvent) m_SpawnEvent.valInt)
            {
                case Spawner.SpawnEvent.Manual:
                    DustGUI.HelpBoxInfo("Call Spawn() method to spawn object(s)");
                    break;

                case Spawner.SpawnEvent.FixedInterval:
                    PropertyDurationField(m_Interval);
                    break;

                case Spawner.SpawnEvent.IntervalInRange:
                    PropertyFieldDurationRange(m_IntervalRange);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_SpawnObjects);

            Space();

            if (m_SpawnObjects.property.arraySize > 1)
            {
                PropertyField(m_SpawnObjectsIterate);

                if ((Spawner.IterateMode) m_SpawnObjectsIterate.valInt == Spawner.IterateMode.Random)
                    PropertySeedRandomOrFixed(m_SpawnObjectsSeed);
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            DustGUI.Header("Spawn At");

            PropertyField(m_SpawnPointMode);

            switch ((Spawner.SpawnPointMode) m_SpawnPointMode.valInt)
            {
                case Spawner.SpawnPointMode.Self:
                    break;

                case Spawner.SpawnPointMode.Points:
                    PropertyField(m_SpawnPoints);

                    if (m_SpawnPoints.property.arraySize > 1)
                    {
                        PropertyField(m_SpawnPointsIterate);

                        if ((Spawner.IterateMode) m_SpawnPointsIterate.valInt == Spawner.IterateMode.Random)
                            PropertySeedRandomOrFixed(m_SpawnPointsSeed);
                    }
                    break;
                
                case Spawner.SpawnPointMode.SphereVolume:
                    PropertyExtendedSlider(m_SphereVolumeRadius, 0f, 5f, 0.01f, 0f);
                    PropertySeedRandomOrFixed(m_SpawnPointsSeed);
                    break;

                case Spawner.SpawnPointMode.BoxVolume:
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

            PropertyField(m_Limit);
            PropertyField(m_SpawnOnAwake);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("On Spawn Instance", "Spawner.OnSpawnInstance", true))
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

            if (DustGUI.FoldoutBegin("Events", "Spawner.Events", false))
            {
                PropertyField(m_OnSpawn);
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (Application.isPlaying)
            {
                if (DustGUI.FoldoutBegin("Debug", "Spawner.Debug"))
                {
                    DustGUI.BeginHorizontal();
                    {
                        if (DustGUI.Button("Spawn"))
                        {
                            foreach (var subTarget in targets)
                                (subTarget as Spawner).Spawn();
                        }

                        if (DustGUI.Button("Spawn Single Object"))
                        {
                            foreach (var subTarget in targets)
                                (subTarget as Spawner).SpawnSingleObject();
                        }
                    }
                    DustGUI.EndHorizontal();

                    if (targets.Length == 1)
                    {
                        var main = target as Spawner;

                        Space();

                        DustGUI.Header("Stats");
                        DustGUI.StaticTextField("Spawned", main.count.ToString());
                        DustGUI.StaticTextField("Total Limit", main.limit.ToString());
                    }
                }
                DustGUI.FoldoutEnd();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_SphereVolumeRadius.isChanged)
                m_SphereVolumeRadius.valFloat = Spawner.NormalizeSphereVolumeRadius(m_SphereVolumeRadius.valFloat);

            if (m_BoxVolumeSize.isChanged)
                m_BoxVolumeSize.valVector3 = Spawner.NormalizeBoxVolumeSize(m_BoxVolumeSize.valVector3);

            if (m_Interval.isChanged)
                m_Interval.valFloat = Spawner.NormalizeIntervalValue(m_Interval.valFloat);

            if (m_Limit.isChanged)
                m_Limit.valInt = Spawner.NormalizeLimit(m_Limit.valInt);

            // @notice: no need to NormalizeIntervalValue for m_IntervalRange.
            // It auto-normalized in PropertyFieldDurationRange() method

            // @notice: no need to NormalizeMultipleSpawnCount for m_MultipleSpawnCount.
            // It auto-normalized in PropertyFieldRange() method

            InspectorCommitUpdates();
        }
    }
}
