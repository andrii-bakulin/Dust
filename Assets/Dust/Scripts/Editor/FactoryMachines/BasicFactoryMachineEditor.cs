using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(BasicFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class BasicFactoryMachineEditor : FactoryMachineEditor
    {
        protected DuProperty m_ValueImpactEnabled;
        protected DuProperty m_ValueImpactIntensity;
        protected DuProperty m_ValueBlendMode;
        protected DuProperty m_ValueClampEnabled;
        protected DuProperty m_ValueClampMin;
        protected DuProperty m_ValueClampMax;

        protected DuProperty m_ColorImpactEnabled;
        protected DuProperty m_ColorImpactIntensity;
        protected DuProperty m_ColorBlendMode;

        protected FieldsMapEditor m_FieldsMapEditor;

        //--------------------------------------------------------------------------------------------------------------

        static BasicFactoryMachineEditor()
        {
            FactoryMachinesPopupButtons.AddMachine(typeof(BasicFactoryMachine), "Basic");
        }

        [MenuItem("Dust/Factory Machines/Basic")]
        [MenuItem("GameObject/Dust/Factory Machines/Basic")]
        public static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(BasicFactoryMachine));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ValueImpactEnabled = FindProperty("m_ValueImpactEnabled", "Enabled");
            m_ValueImpactIntensity = FindProperty("m_ValueImpactIntensity", "Intensity");
            m_ValueBlendMode = FindProperty("m_ValueBlendMode", "Blend Mode");
            m_ValueClampEnabled = FindProperty("m_ValueClampEnabled", "Final Clamp");
            m_ValueClampMin = FindProperty("m_ValueClampMin", "Min");
            m_ValueClampMax = FindProperty("m_ValueClampMax", "Max");

            m_ColorImpactEnabled = FindProperty("m_ColorImpactEnabled", "Enabled");
            m_ColorImpactIntensity = FindProperty("m_ColorImpactIntensity", "Intensity");
            m_ColorBlendMode = FindProperty("m_ColorBlendMode", "Blend Mode");

            m_FieldsMapEditor = new FieldsMapEditor(this, serializedObject.FindProperty("m_FieldsMap"), (target as BasicFactoryMachine).fieldsMap);
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForFactoryMachine(this);

            PropertyField(m_Hint);
            PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_FieldsMap();
            
            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InspectorCommitUpdates()
        {
            if (m_ValueImpactIntensity.isChanged)
                m_ValueImpactIntensity.valFloat = BasicFactoryMachine.NormalizeImpactIntensity(m_ValueImpactIntensity.valFloat);

            if (m_ColorImpactIntensity.isChanged)
                m_ColorImpactIntensity.valFloat = BasicFactoryMachine.NormalizeImpactIntensity(m_ColorImpactIntensity.valFloat);

            base.InspectorCommitUpdates();
        }

        //--------------------------------------------------------------------------------------------------------------

        // WARNING!
        // On change logic/structure here, also required to check changes in parent/children methods
        // with same method name/links
        protected void OnInspectorGUI_ImpactOnValueBlock()
        {
            if (DustGUI.FoldoutBegin("Impact on the Value of Instances", "FactoryMachine.ImpactOnValue"))
            {
                PropertyField(m_ValueImpactEnabled);

                if (m_ValueImpactEnabled.IsTrue)
                {
                    PropertyField(m_ValueBlendMode);
                    PropertyExtendedSlider(m_ValueImpactIntensity, 0f, +1f, 0.01f);

                    Space();

                    PropertyField(m_ValueClampEnabled);

                    if (m_ValueClampEnabled.IsTrue)
                    {
                        DustGUI.IndentLevelInc();
                        PropertyExtendedSlider(m_ValueClampMin, -1f, +1f, 0.01f);
                        PropertyExtendedSlider(m_ValueClampMax, -1f, +1f, 0.01f);
                        DustGUI.IndentLevelDec();
                    }
                }

                Space();
            }
            DustGUI.FoldoutEnd();
        }

        // WARNING!
        // On change logic/structure here, also required to check changes in parent/children methods
        // with same method name/links
        protected void OnInspectorGUI_ImpactOnColorBlock()
        {
            if (DustGUI.FoldoutBegin("Impact on the Color of Instances", "FactoryMachine.ImpactOnColor"))
            {
                PropertyField(m_ColorImpactEnabled);

                if (m_ColorImpactEnabled.IsTrue)
                {
                    PropertyField(m_ColorBlendMode);
                    PropertyExtendedSlider(m_ColorImpactIntensity, 0f, +1f, 0.01f);
                }
                Space();
            }
            DustGUI.FoldoutEnd();
        }

        // WARNING!
        // On change logic/structure here, also required to check changes in parent/children methods
        // with same method name/links
        protected void OnInspectorGUI_FieldsMap()
        {
            var showColumnPower = FieldsMapEditor.ColumnVisibility.Auto;
            var showColumnColor = FieldsMapEditor.ColumnVisibility.Auto;

            if (!m_ColorImpactEnabled.IsTrue)
                showColumnColor = FieldsMapEditor.ColumnVisibility.ForcedHide;

            m_FieldsMapEditor.OnInspectorGUI(showColumnPower, showColumnColor);
        }
    }
}
