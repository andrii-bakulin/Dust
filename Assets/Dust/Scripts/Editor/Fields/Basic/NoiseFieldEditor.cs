﻿using System;
using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(NoiseField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class NoiseFieldEditor : SpaceFieldEditor
    {
        protected DuProperty m_NoiseMode;
        protected DuProperty m_Seed;

        protected DuProperty m_NoiseSpace;
        protected DuProperty m_NoiseScale;
        protected DuProperty m_NoisePower;

        protected DuProperty m_AnimationSpeed;
        protected DuProperty m_AnimationOffset;

        protected DuProperty m_IgnoreAxisX;
        protected DuProperty m_IgnoreAxisY;
        protected DuProperty m_IgnoreAxisZ;

        //--------------------------------------------------------------------------------------------------------------

        static NoiseFieldEditor()
        {
            FieldsPopupButtons.AddBasicField(typeof(NoiseField), "Noise");
        }

        [MenuItem("Dust/Fields/Basic Fields/Noise")]
        [MenuItem("GameObject/Dust/Fields/Basic Fields/Noise")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(NoiseField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_NoiseMode = FindProperty("m_NoiseMode", "Noise Mode");
            m_Seed = FindProperty("m_Seed", "Seed");

            m_NoiseSpace = FindProperty("m_NoiseSpace", "Noise Space");
            m_NoiseScale = FindProperty("m_NoiseScale", "Noise Scale");
            m_NoisePower = FindProperty("m_NoisePower", "Noise Power");

            m_AnimationSpeed = FindProperty("m_AnimationSpeed", "Animation Speed");
            m_AnimationOffset = FindProperty("m_AnimationOffset", "Animation Offset");

            m_IgnoreAxisX = FindProperty("m_IgnoreAxisX", "Ignore Axis X");
            m_IgnoreAxisY = FindProperty("m_IgnoreAxisY", "Ignore Axis Y");
            m_IgnoreAxisZ = FindProperty("m_IgnoreAxisZ", "Ignore Axis Z");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            PropertyExtendedSlider(m_Power, 0f, 5f, 0.01f);
            Space();

            PropertyField(m_NoiseMode);
            PropertySeedFixed(m_Seed);
            Space();

            switch ((NoiseField.NoiseMode) m_NoiseMode.valInt)
            {
                case NoiseField.NoiseMode.Random:
                    // Ignore
                    break;

                case NoiseField.NoiseMode.Perlin:
                    PropertyField(m_NoiseSpace);
                    PropertyExtendedSlider(m_NoiseScale, 0.01f, 16f, 0.01f, 0.01f);
                    PropertyExtendedSlider(m_NoisePower, 0.01f, 3f, 0.01f, 0.01f);
                    Space();

                    PropertyExtendedSlider(m_AnimationSpeed, 0f, 10f, 0.01f);
                    PropertyExtendedSlider(m_AnimationOffset, -5f, 5f, 0.01f);
                    Space();

                    PropertyField(m_IgnoreAxisX);
                    PropertyField(m_IgnoreAxisY);
                    PropertyField(m_IgnoreAxisZ);
                    Space();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            PropertyField(m_Hint);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_ColoringBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_NoiseScale.isChanged)
                m_NoiseScale.valFloat = NoiseField.NormalizeNoiseScale(m_NoiseScale.valFloat);

            if (m_NoisePower.isChanged)
                m_NoisePower.valFloat = NoiseField.NormalizeNoisePower(m_NoisePower.valFloat);

            if (m_Seed.isChanged)
                m_Seed.valInt = NoiseField.NormalizeSeed(m_Seed.valInt);

            InspectorCommitUpdates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            foreach (var subTarget in targets)
            {
                var origin = subTarget as NoiseField;

                if (m_Seed.isChanged || DustGUI.IsUndoRedoPerformed())
                    origin.ResetStates();
            }
        }
    }
}
