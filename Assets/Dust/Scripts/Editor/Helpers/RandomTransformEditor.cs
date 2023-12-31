﻿using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(RandomTransform))]
    [CanEditMultipleObjects]
    public class RandomTransformEditor : DuEditor
    {
        protected DuProperty m_PositionEnabled;
        protected DuProperty m_PositionRangeMin;
        protected DuProperty m_PositionRangeMax;

        protected DuProperty m_RotationEnabled;
        protected DuProperty m_RotationRangeMin;
        protected DuProperty m_RotationRangeMax;

        protected DuProperty m_ScaleEnabled;
        protected DuProperty m_ScaleRangeMin;
        protected DuProperty m_ScaleRangeMax;
        protected DuProperty m_ScaleUniform;

        protected DuProperty m_ActivateMode;
        protected DuProperty m_TransformMode;
        protected DuProperty m_Space;

        protected DuProperty m_Seed;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Helpers/Random Transform")]
        [MenuItem("GameObject/Dust/Helpers/Random Transform")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Random Transform", typeof(RandomTransform));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_PositionEnabled = FindProperty("m_PositionEnabled", "Random Position");
            m_PositionRangeMin = FindProperty("m_PositionRangeMin", "Position Range Min");
            m_PositionRangeMax = FindProperty("m_PositionRangeMax", "Position Range Max");

            m_RotationEnabled = FindProperty("m_RotationEnabled", "Random Rotation");
            m_RotationRangeMin = FindProperty("m_RotationRangeMin", "Rotation Range Min");
            m_RotationRangeMax = FindProperty("m_RotationRangeMax", "Rotation Range Max");

            m_ScaleEnabled = FindProperty("m_ScaleEnabled", "Random Scale");
            m_ScaleRangeMin = FindProperty("m_ScaleRangeMin", "Scale Range Min");
            m_ScaleRangeMax = FindProperty("m_ScaleRangeMax", "Scale Range Max");
            m_ScaleUniform = FindProperty("m_ScaleUniform", "Uniform");

            m_ActivateMode = FindProperty("m_ActivateMode", "Activate On");
            m_TransformMode = FindProperty("m_TransformMode", "Transform Mode",
                "Add - add random values to current transform" + "\n" +
                "Set - set random values as new transform");
            m_Space = FindProperty("m_Space", "Space");

            m_Seed = FindProperty("m_Seed");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_PositionEnabled);
            PropertyField(m_RotationEnabled);
            PropertyField(m_ScaleEnabled);

            Space();

            if (m_PositionEnabled.IsTrue)
            {
                PropertyField(m_PositionRangeMin);
                PropertyField(m_PositionRangeMax);
                Space();
            }

            if (m_RotationEnabled.IsTrue)
            {
                PropertyField(m_RotationRangeMin);
                PropertyField(m_RotationRangeMax);
                Space();
            }

            if (m_ScaleEnabled.IsTrue)
            {
                PropertyField(m_ScaleRangeMin);
                PropertyField(m_ScaleRangeMax);
                PropertyField(m_ScaleUniform);
                Space();
            }

            PropertyField(m_ActivateMode);
            PropertyField(m_TransformMode);
            PropertyField(m_Space);

            PropertySeedRandomOrFixed(m_Seed);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
