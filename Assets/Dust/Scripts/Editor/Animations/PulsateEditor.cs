using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(Pulsate))]
    [CanEditMultipleObjects]
    public class PulsateEditor : DuEditor
    {
        private DuProperty m_Power;
        private DuProperty m_SleepTime;
        private DuProperty m_Freeze;

        private DuProperty m_PositionEnabled;
        private DuProperty m_PositionAmplitude;
        private DuProperty m_PositionSpeed;

        private DuProperty m_RotationEnabled;
        private DuProperty m_RotationAmplitude;
        private DuProperty m_RotationSpeed;

        private DuProperty m_ScaleEnabled;
        private DuProperty m_ScaleAmplitude;
        private DuProperty m_ScaleSpeed;

        private DuProperty m_EaseMode;
        private DuProperty m_TransformMode;
        private DuProperty m_UpdateMode;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Animations/Pulsate")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Pulsate", typeof(Pulsate));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Power = FindProperty("m_Power", "Power");
            m_SleepTime = FindProperty("m_SleepTime", "Sleep Time");
            m_Freeze = FindProperty("m_Freeze", "Freeze");

            m_PositionEnabled = FindProperty("m_PositionEnabled", "Enable");
            m_PositionAmplitude = FindProperty("m_PositionAmplitude", "Amplitude");
            m_PositionSpeed = FindProperty("m_PositionSpeed", "Speed");

            m_RotationEnabled = FindProperty("m_RotationEnabled", "Enable");
            m_RotationAmplitude = FindProperty("m_RotationAmplitude", "Amplitude");
            m_RotationSpeed = FindProperty("m_RotationSpeed", "Speed");

            m_ScaleEnabled = FindProperty("m_ScaleEnabled", "Enable");
            m_ScaleAmplitude = FindProperty("m_ScaleAmplitude", "Amplitude");
            m_ScaleSpeed = FindProperty("m_ScaleSpeed", "Speed");

            m_EaseMode = FindProperty("m_EaseMode", "Ease");
            m_TransformMode = FindProperty("m_TransformMode", "Transform Mode");
            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Position", "Pulsate.Position"))
            {
                PropertyField(m_PositionEnabled);

                if (!m_PositionEnabled.IsTrue)
                    DustGUI.Lock();

                PropertyField(m_PositionAmplitude);
                PropertyExtendedSlider(m_PositionSpeed, 0f, 10f, 0.01f);

                DustGUI.Unlock();
                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Rotation", "Pulsate.Rotation"))
            {
                PropertyField(m_RotationEnabled);

                if (!m_RotationEnabled.IsTrue)
                    DustGUI.Lock();

                PropertyField(m_RotationAmplitude);
                PropertyExtendedSlider(m_RotationSpeed, 0f, 10f, 0.01f);

                DustGUI.Unlock();
                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Scale", "Pulsate.Scale"))
            {
                PropertyField(m_ScaleEnabled);

                if (!m_ScaleEnabled.IsTrue)
                    DustGUI.Lock();

                PropertyField(m_ScaleAmplitude);
                PropertyExtendedSlider(m_ScaleSpeed, 0f, 10f, 0.01f);

                DustGUI.Unlock();
                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Pulsate Parameters", "Pulsate.Parameters"))
            {
                PropertyField(m_EaseMode);
                PropertyExtendedSlider(m_Power, 0f, 1f, 0.01f, 0f, 1f);
                PropertyExtendedSlider(m_SleepTime, 0f, 10f, 0.01f, 0f);
                PropertyField(m_Freeze);

                Space();

                PropertyField(m_TransformMode);
                PropertyField(m_UpdateMode);

                if ((Pulsate.TransformMode) m_TransformMode.valInt == Pulsate.TransformMode.AppendToAnimation)
                {
                    DustGUI.HelpBoxInfo("This mode need to use when object animated by keyframes or manually in Update method."
                                        + " Then you may apply pulsate in LastUpdate calls");
                }

                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_Power.isChanged)
                m_Power.valFloat = Pulsate.NormalizePower(m_Power.valFloat);

            if (m_SleepTime.isChanged)
                m_SleepTime.valFloat = Pulsate.NormalizeSleepTime(m_SleepTime.valFloat);

            if (m_ScaleAmplitude.isChanged)
                m_ScaleAmplitude.valVector3 = Pulsate.NormalizeScaleAmplitude(m_ScaleAmplitude.valVector3);

            InspectorCommitUpdates();
        }
    }
}
