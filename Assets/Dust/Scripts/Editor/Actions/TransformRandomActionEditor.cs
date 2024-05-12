using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(TransformRandomAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TransformRandomActionEditor : InstantActionEditor
    {
        protected DuProperty m_PositionEnabled;
        protected DuProperty m_PositionRangeMin;
        protected DuProperty m_PositionRangeMax;
        protected DuProperty m_PositionTransformMode;

        protected DuProperty m_RotationEnabled;
        protected DuProperty m_RotationRangeMin;
        protected DuProperty m_RotationRangeMax;
        protected DuProperty m_RotationTransformMode;

        protected DuProperty m_ScaleEnabled;
        protected DuProperty m_ScaleRangeMin;
        protected DuProperty m_ScaleRangeMax;
        protected DuProperty m_ScaleTransformMode;
        protected DuProperty m_ScaleUniform;

        protected DuProperty m_Space;

        protected DuProperty m_Seed;

        //--------------------------------------------------------------------------------------------------------------

        static TransformRandomActionEditor()
        {
            ActionsPopupButtons.AddActionTransform(typeof(TransformRandomAction), "Transform Random");
        }

        [MenuItem("Dust/Actions/Transform Random")]
        [MenuItem("GameObject/Dust/Actions/Transform Random")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Transform Random Action", typeof(TransformRandomAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_PositionEnabled = FindProperty("m_PositionEnabled", "Random Position");
            m_PositionRangeMin = FindProperty("m_PositionRangeMin", "Range Min");
            m_PositionRangeMax = FindProperty("m_PositionRangeMax", "Range Max");
            m_PositionTransformMode = FindProperty("m_PositionTransformMode", "Transform Mode");

            m_RotationEnabled = FindProperty("m_RotationEnabled", "Random Rotation");
            m_RotationRangeMin = FindProperty("m_RotationRangeMin", "Range Min");
            m_RotationRangeMax = FindProperty("m_RotationRangeMax", "Range Max");
            m_RotationTransformMode = FindProperty("m_RotationTransformMode", "Transform Mode");

            m_ScaleEnabled = FindProperty("m_ScaleEnabled", "Random Scale");
            m_ScaleRangeMin = FindProperty("m_ScaleRangeMin", "Range Min");
            m_ScaleRangeMax = FindProperty("m_ScaleRangeMax", "Range Max");
            m_ScaleTransformMode = FindProperty("m_ScaleTransformMode", "Transform Mode");
            m_ScaleUniform = FindProperty("m_ScaleUniform", "Uniform", "If TRUE all values for each axis will be the same");

            m_Space = FindProperty("m_Space", "Space");

            m_Seed = FindProperty("m_Seed");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_PositionEnabled);

            if (m_PositionEnabled.IsTrue)
            {
                DustGUI.IndentLevelInc();
                PropertyField(m_PositionRangeMin);
                PropertyField(m_PositionRangeMax);
                PropertyField(m_PositionTransformMode);
                DustGUI.IndentLevelDec();
            }

            Space();
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_RotationEnabled);

            if (m_RotationEnabled.IsTrue)
            {
                DustGUI.IndentLevelInc();
                PropertyField(m_RotationRangeMin);
                PropertyField(m_RotationRangeMax);
                PropertyField(m_RotationTransformMode);
                DustGUI.IndentLevelDec();
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_ScaleEnabled);

            if (m_ScaleEnabled.IsTrue)
            {
                DustGUI.IndentLevelInc();
                PropertyField(m_ScaleRangeMin);
                PropertyField(m_ScaleRangeMax);
                PropertyField(m_ScaleTransformMode);
                PropertyField(m_ScaleUniform);
                DustGUI.IndentLevelDec();
            }

            Space();

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
