using System;
using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(Move))]
    [CanEditMultipleObjects]
    public class MoveEditor : DuEditor
    {
        protected DuProperty m_TranslateType;

        protected DuProperty m_LinearSpeed;

        protected DuProperty m_WaveAmplitude;
        protected DuProperty m_WaveSpeed;
        protected DuProperty m_WaveOffset;

        protected DuProperty m_Space;
        protected DuProperty m_Freeze;

        protected DuProperty m_UpdateMode;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Animations/Move")]
        [MenuItem("GameObject/Dust/Animations/Move")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Move", typeof(Move));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_TranslateType = FindProperty("m_TranslateType", "Translate Kind");

            m_LinearSpeed = FindProperty("m_LinearSpeed", "Speed");

            m_WaveAmplitude = FindProperty("m_WaveAmplitude", "Amplitude");
            m_WaveSpeed = FindProperty("m_WaveSpeed", "Speed in Degrees");
            m_WaveOffset = FindProperty("m_WaveOffset", "Offset");

            m_Space = FindProperty("m_Space", "Space");
            m_Freeze = FindProperty("m_Freeze", "Freeze");

            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_TranslateType);

            switch ((Move.TranslateType) m_TranslateType.valInt)
            {
                case Move.TranslateType.Linear:
                    PropertyField(m_LinearSpeed);
                    break;

                case Move.TranslateType.Wave:
                    PropertyField(m_WaveAmplitude);
                    PropertyField(m_WaveSpeed);
                    PropertyField(m_WaveOffset);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Space();

            PropertyField(m_Space);
            PropertyField(m_Freeze);

            Space();

            PropertyField(m_UpdateMode);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (GUI.changed)
                m_WaveOffset.valVector3 = DuVector3.Clamp01(m_WaveOffset.valVector3);

            InspectorCommitUpdates();
        }
    }
}
