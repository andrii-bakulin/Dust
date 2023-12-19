using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(Follow))]
    [CanEditMultipleObjects]
    public class FollowEditor : DuEditor
    {
        private DuProperty m_FollowObject;
        private DuProperty m_FollowOffset;

        private DuProperty m_SpeedMode;
        private DuProperty m_SpeedLimit;

        private DuProperty m_UseSmoothDamp;
        private DuProperty m_SmoothTime;

        private DuProperty m_UpdateMode;
        private DuProperty m_UpdateInEditor;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private Follow.SpeedMode speedMode => (Follow.SpeedMode) m_SpeedMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Animations/Follow")]
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
            }

            Space();

            PropertyField(m_UseSmoothDamp);
            PropertyFieldOrHide(m_SmoothTime, !m_UseSmoothDamp.IsTrue);

            Space();

            PropertyField(m_UpdateMode);
            PropertyFieldOrLock(m_UpdateInEditor, Application.isPlaying);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_FollowObject.isChanged)
            {
                GameObject followObject = m_FollowObject.GameObjectReference;

                if (Dust.IsNotNull(followObject))
                {
                    foreach (var entity in GetSerializedEntitiesByTargets())
                    {
                        Vector3 followOffset = (entity.target as Follow).transform.position - followObject.transform.position;

                        entity.serializedObject.FindProperty("m_FollowOffset").vector3Value = DuVector3.Round(followOffset);
                        entity.serializedObject.ApplyModifiedProperties();
                    }
                }
            }

            if (m_SpeedLimit.isChanged)
                m_SpeedLimit.valFloat = Follow.NormalizeSpeedLimit(m_SpeedLimit.valFloat);

            if (m_SmoothTime.isChanged)
                m_SmoothTime.valVector3 = Follow.NormalizeSmoothTime(m_SmoothTime.valVector3);

            InspectorCommitUpdates();
        }
    }
}
