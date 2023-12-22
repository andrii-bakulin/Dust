using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(TransformRandomAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TransformRandomActionEditor : InstantActionEditor
    {
        private DuProperty m_PositionEnabled;
        private DuProperty m_PositionRangeMin;
        private DuProperty m_PositionRangeMax;

        private DuProperty m_RotationEnabled;
        private DuProperty m_RotationRangeMin;
        private DuProperty m_RotationRangeMax;

        private DuProperty m_ScaleEnabled;
        private DuProperty m_ScaleRangeMin;
        private DuProperty m_ScaleRangeMax;
        private DuProperty m_ScaleUniform;

        private DuProperty m_TransformMode;
        private DuProperty m_Space;

        private DuProperty m_Seed;

        //--------------------------------------------------------------------------------------------------------------

        static TransformRandomActionEditor()
        {
            ActionsPopupButtons.AddActionTransform(typeof(TransformRandomAction), "Transform Random");
        }

        [MenuItem("Dust/Actions/Transform Random")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Transform Random Action", typeof(TransformRandomAction));
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

            OnInspectorGUI_BaseControlUI();

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

            PropertyField(m_TransformMode);
            PropertyField(m_Space);

            PropertySeedRandomOrFixed(m_Seed);

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("TransformRandomAction");
            OnInspectorGUI_Extended("TransformRandomAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
