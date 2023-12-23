using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(CoordinatesField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class CoordinatesFieldEditor : FieldEditor
    {
        private DuProperty m_PowerEnabled;
        private DuProperty m_PowerUseAxisX;
        private DuProperty m_PowerUseAxisY;
        private DuProperty m_PowerUseAxisZ;
        private DuProperty m_PowerScale;
        private DuProperty m_PowerAggregate;
        private DuProperty m_PowerMin;
        private DuProperty m_PowerMax;
        private DuProperty m_PowerShape;

        private DuProperty m_ColorEnabled;
        private DuProperty m_ColorScale;
        private DuProperty m_ColorShape;

        //--------------------------------------------------------------------------------------------------------------

        static CoordinatesFieldEditor()
        {
            FieldsPopupButtons.AddBasicField(typeof(CoordinatesField), "Coordinates");
        }

        [MenuItem("Dust/Fields/Basic Fields/Coordinates")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(CoordinatesField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_PowerEnabled = FindProperty("m_PowerEnabled", "Enabled");
            m_PowerUseAxisX = FindProperty("m_PowerUseAxisX", "Use Axis X");
            m_PowerUseAxisY = FindProperty("m_PowerUseAxisY", "Use Axis Y");
            m_PowerUseAxisZ = FindProperty("m_PowerUseAxisZ", "Use Axis Z");
            m_PowerScale = FindProperty("m_PowerScale", "Scale");
            m_PowerAggregate = FindProperty("m_PowerAggregate", "Aggregate Func");
            m_PowerMin = FindProperty("m_PowerMin", "Min");
            m_PowerMax = FindProperty("m_PowerMax", "Max");
            m_PowerShape = FindProperty("m_PowerShape", "Shape");

            m_ColorEnabled = FindProperty("m_ColorEnabled", "Enabled");
            m_ColorScale = FindProperty("m_ColorScale", "Scale");
            m_ColorShape = FindProperty("m_ColorShape", "Shape");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Power", "DuAnyField.Power"))
            {
                PropertyField(m_PowerEnabled);
                Space();

                if (!m_PowerEnabled.IsTrue) DustGUI.Lock();

                PropertyField(m_PowerUseAxisX);
                PropertyField(m_PowerUseAxisY);
                PropertyField(m_PowerUseAxisZ);

                if ((m_PowerUseAxisX.IsTrue ? 1 : 0) + (m_PowerUseAxisY.IsTrue ? 1 : 0) + (m_PowerUseAxisZ.IsTrue ? 1 : 0) > 1)
                    PropertyField(m_PowerAggregate);

                Space();

                PropertyExtendedSlider(m_PowerScale, 0.01f, 5f, 0.01f);
                PropertyExtendedSlider(m_PowerMin, 0f, 1f, 0.01f);
                PropertyExtendedSlider(m_PowerMax, 0f, 1f, 0.01f);
                PropertyField(m_PowerShape);
                Space();

                if (!m_PowerEnabled.IsTrue) DustGUI.Unlock();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Color", "DuAnyField.Color"))
            {
                PropertyField(m_ColorEnabled);
                Space();

                if (!m_ColorEnabled.IsTrue) DustGUI.Lock();

                PropertyExtendedSlider(m_ColorScale, 0.01f, 5f, 0.01f);
                PropertyField(m_ColorShape);
                Space();

                if (!m_ColorEnabled.IsTrue) DustGUI.Unlock();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
