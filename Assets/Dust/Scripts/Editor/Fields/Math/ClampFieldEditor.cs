using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ClampField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ClampFieldEditor : MathFieldEditor
    {
        protected DuProperty m_PowerClampMode;
        protected DuProperty m_PowerClampMin;
        protected DuProperty m_PowerClampMax;

        protected DuProperty m_ColorClampMode;
        protected DuProperty m_ColorClampMin;
        protected DuProperty m_ColorClampMax;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private ClampMode powerClampMode => (ClampMode) m_PowerClampMode.valInt;
        private ClampMode colorClampMode => (ClampMode) m_ColorClampMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static ClampFieldEditor()
        {
            FieldsPopupButtons.AddMathField(typeof(ClampField), "Clamp");
        }

        [MenuItem("Dust/Fields/Math Fields/Clamp")]
        [MenuItem("GameObject/Dust/Fields/Math Fields/Clamp")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(ClampField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_PowerClampMode = FindProperty("m_PowerClampMode", "Clamp Mode");
            m_PowerClampMin = FindProperty("m_PowerClampMin", "Min");
            m_PowerClampMax = FindProperty("m_PowerClampMax", "Max");

            m_ColorClampMode = FindProperty("m_ColorClampMode", "Clamp Mode");
            m_ColorClampMin = FindProperty("m_ColorClampMin", "Min");
            m_ColorClampMax = FindProperty("m_ColorClampMax", "Max");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Power", "DuAnyField.Power"))
            {
                PropertyField(m_PowerClampMode);

                bool isMinEnabled = powerClampMode == ClampMode.MinAndMax || powerClampMode == ClampMode.MinOnly;
                bool isMaxEnabled = powerClampMode == ClampMode.MinAndMax || powerClampMode == ClampMode.MaxOnly;

                if (!isMinEnabled) DustGUI.Lock();
                PropertyExtendedSlider(m_PowerClampMin, 0f, 1f, 0.01f);
                if (!isMinEnabled) DustGUI.Unlock();

                if (!isMaxEnabled) DustGUI.Lock();
                PropertyExtendedSlider(m_PowerClampMax, 0f, 1f, 0.01f);
                if (!isMaxEnabled) DustGUI.Unlock();

                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Color", "DuAnyField.Color"))
            {
                PropertyField(m_ColorClampMode);

                bool isMinEnabled = colorClampMode == ClampMode.MinAndMax || colorClampMode == ClampMode.MinOnly;
                bool isMaxEnabled = colorClampMode == ClampMode.MinAndMax || colorClampMode == ClampMode.MaxOnly;

                if (!isMinEnabled) DustGUI.Lock();
                PropertyField(m_ColorClampMin);
                if (!isMinEnabled) DustGUI.Unlock();

                if (!isMaxEnabled) DustGUI.Lock();
                PropertyField(m_ColorClampMax);
                if (!isMaxEnabled) DustGUI.Unlock();

                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_Hint);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
