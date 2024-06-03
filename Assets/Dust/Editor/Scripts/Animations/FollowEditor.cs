using System;
using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(Follow))]
    [CanEditMultipleObjects]
    public class FollowEditor : DuEditor
    {
        protected DuProperty m_FollowObject;
        protected DuProperty m_FollowOffset;

        protected DuProperty m_SpeedMode;
        protected DuProperty m_SpeedLimit;

        protected DuProperty m_UseSmoothDamp;
        protected DuProperty m_SmoothTime;

        protected DuProperty m_UpdateMode;
        protected DuProperty m_UpdateInEditor;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected Follow.SpeedMode speedMode => (Follow.SpeedMode) m_SpeedMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Animations/Follow")]
        [MenuItem("GameObject/Dust/Animations/Follow")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Follow", typeof(Follow));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_FollowObject = FindProperty("m_FollowObject", "Follow Object");
            m_FollowOffset = FindProperty("m_FollowOffset", "Follow Offset");

            m_SpeedMode = FindProperty("m_SpeedMode", "Speed");
            m_SpeedLimit = FindProperty("m_SpeedLimit", "Speed Limit");

            m_UseSmoothDamp = FindProperty("m_UseSmoothDamp", "Use Smooth Damp");
            m_SmoothTime = FindProperty("m_SmoothTime", "Smooth Time");

            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
            m_UpdateInEditor = FindProperty("m_UpdateInEditor", "Update In Editor");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_FollowObject);
            PropertyField(m_FollowOffset);

            Space();

            PropertyField(m_SpeedMode);

            switch (speedMode)
            {
                case Follow.SpeedMode.Unlimited:
                    // Nothing to show
                    break;

                case Follow.SpeedMode.Limited:
                    PropertyField(m_SpeedLimit);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Space();

            PropertyField(m_UseSmoothDamp);
            PropertyFieldOrHide(m_SmoothTime, !m_UseSmoothDamp.IsTrue);

            Space();

            PropertyField(m_UpdateMode);
            PropertyFieldOrLock(m_UpdateInEditor, Application.isPlaying);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_SpeedLimit.isChanged)
                m_SpeedLimit.valFloat = Follow.NormalizeSpeedLimit(m_SpeedLimit.valFloat);

            if (m_SmoothTime.isChanged)
                m_SmoothTime.valVector3 = Follow.NormalizeSmoothTime(m_SmoothTime.valVector3);

            InspectorCommitUpdates();
        }
    }
}
