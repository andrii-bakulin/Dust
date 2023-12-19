using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(Shake))]
    [CanEditMultipleObjects]
    public class ShakeEditor : DuEditor
    {
        private DuProperty m_Power;
        private DuProperty m_WarmUpTime;
        private DuProperty m_Freeze;
        private DuProperty m_Seed;

        private DuProperty m_PositionEnabled;
        private DuProperty m_PositionAmplitude;
        private DuProperty m_PositionSpeed;

        private DuProperty m_RotationEnabled;
        private DuProperty m_RotationAmplitude;
        private DuProperty m_RotationSpeed;

        private DuProperty m_ScaleEnabled;
        private DuProperty m_ScaleAmplitude;
        private DuProperty m_ScaleSpeed;
        private DuProperty m_ScaleUniform;

        private DuProperty m_TransformMode;
        private DuProperty m_UpdateMode;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Animations/Shake")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Shake", typeof(Shake));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Power = FindProperty("m_Power", "Power");
            m_WarmUpTime = FindProperty("m_WarmUpTime", "Warm Up Time");
            m_Freeze = FindProperty("m_Freeze", "Freeze");
            m_Seed = FindProperty("m_Seed", "Seed");

            m_PositionEnabled = FindProperty("m_PositionEnabled", "Enable");
            m_PositionAmplitude = FindProperty("m_PositionAmplitude", "Amplitude");
            m_PositionSpeed = FindProperty("m_PositionSpeed", "Speed");

            m_RotationEnabled = FindProperty("m_RotationEnabled", "Enable");
            m_RotationAmplitude = FindProperty("m_RotationAmplitude", "Amplitude");
            m_RotationSpeed = FindProperty("m_RotationSpeed", "Speed");

            m_ScaleEnabled = FindProperty("m_ScaleEnabled", "Enable");
            m_ScaleAmplitude = FindProperty("m_ScaleAmplitude", "Amplitude");
            m_ScaleSpeed = FindProperty("m_ScaleSpeed", "Speed");
            m_ScaleUniform = FindProperty("m_ScaleUniform", "Uniform");

            m_TransformMode = FindProperty("m_TransformMode", "Transform Mode");
            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Position", "Shake.Position"))
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


            if (DustGUI.FoldoutBegin("Rotation", "Shake.Rotation"))
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


            if (DustGUI.FoldoutBegin("Scale", "Shake.Scale"))
            {
                PropertyField(m_ScaleEnabled);

                if (!m_ScaleEnabled.IsTrue)
                    DustGUI.Lock();

                PropertyField(m_ScaleAmplitude);
                PropertyExtendedSlider(m_ScaleSpeed, 0f, 10f, 0.01f);
                PropertyField(m_ScaleUniform);

                DustGUI.Unlock();
                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Shake Parameters", "Shake.Parameters"))
            {
                PropertyExtendedSlider(m_Power, 0f, 1f, 0.01f, 0f, 1f);
                PropertyExtendedSlider(m_WarmUpTime, 0f, 5f, 0.01f, 0f);
                PropertyField(m_Freeze);
                PropertySeedRandomOrFixed(m_Seed);

                Space();

                PropertyField(m_TransformMode);
                PropertyField(m_UpdateMode);

                if ((Shake.TransformMode) m_TransformMode.valInt == Shake.TransformMode.AppendToAnimation)
                {
                    DustGUI.HelpBoxInfo("This mode need to use when object animated by keyframes or manually in Update method."
                                        + " Then you may apply shake in LastUpdate calls");
                }

                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_Power.isChanged)
                m_Power.valFloat = Shake.NormalizePower(m_Power.valFloat);

            if (m_WarmUpTime.isChanged)
                m_WarmUpTime.valFloat = Shake.NormalizeWarmUpTime(m_WarmUpTime.valFloat);

            if (m_ScaleAmplitude.isChanged)
                m_ScaleAmplitude.valVector3 = Shake.NormalizeScaleAmplitude(m_ScaleAmplitude.valVector3);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            foreach (var subTarget in targets)
            {
                var origin = subTarget as Shake;
                
                if (origin == null)
                    continue;

                if (m_Seed.isChanged || DustGUI.IsUndoRedoPerformed())
                    origin.ResetStates();
            }
        }
    }
}
