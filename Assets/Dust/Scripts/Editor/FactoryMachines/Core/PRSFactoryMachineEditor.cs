using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class PRSFactoryMachineEditor : BasicFactoryMachineEditor
    {
        protected DuProperty m_Min;
        protected DuProperty m_Max;

        protected DuProperty m_PositionEnabled;
        protected DuProperty m_Position;
        protected DuProperty m_PositionTransformSpace;
        protected DuProperty m_PositionTransformMode;

        protected DuProperty m_RotationEnabled;
        protected DuProperty m_Rotation;
        protected DuProperty m_RotationTransformMode;

        protected DuProperty m_ScaleEnabled;
        protected DuProperty m_Scale;
        protected DuProperty m_ScaleTransformMode;

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Min = FindProperty("m_Min", "Min");
            m_Max = FindProperty("m_Max", "Max");

            m_PositionEnabled = FindProperty("m_PositionEnabled", "Position");
            m_Position = FindProperty("m_Position", "Offset");
            m_PositionTransformSpace = FindProperty("m_PositionTransformSpace", "Transform Space");
            m_PositionTransformMode = FindProperty("m_PositionTransformMode", "Transform Mode");

            m_RotationEnabled = FindProperty("m_RotationEnabled", "Rotation");
            m_Rotation = FindProperty("m_Rotation", "Angle");
            m_RotationTransformMode = FindProperty("m_RotationTransformMode", "Transform Mode");

            m_ScaleEnabled = FindProperty("m_ScaleEnabled", "Scale");
            m_Scale = FindProperty("m_Scale", "Value");
            m_ScaleTransformMode = FindProperty("m_ScaleTransformMode", "Transform Mode");
        }

        // WARNING!
        // On change logic/structure here, also required to check changes in parent/children methods
        // with same method name/links
        protected override void OnInspectorGUI_BaseParameters()
        {
            if (DustGUI.FoldoutBegin("Parameters", "FactoryMachine.Parameters"))
            {
                PropertyField(m_CustomHint);
                PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
                Space();

                PropertyExtendedSlider(m_Max, -1f, +1f, 0.01f);
                PropertyExtendedSlider(m_Min, -1f, +1f, 0.01f);
                Space();
            }
            DustGUI.FoldoutEnd();
        }

        // WARNING!
        // On change logic/structure here, also required to check changes in parent/children methods
        // with same method name/links
        protected void OnInspectorGUI_TransformBlock()
        {
            if (DustGUI.FoldoutBegin("Transform", "FactoryMachine.Transform"))
            {
                PropertyField(m_PositionEnabled);
                PropertyFieldOrLock(m_Position, !m_PositionEnabled.IsTrue);
                PropertyFieldOrLock(m_PositionTransformSpace, !m_PositionEnabled.IsTrue);
                PropertyFieldOrLock(m_PositionTransformMode, !m_PositionEnabled.IsTrue);
                Space();

                PropertyField(m_RotationEnabled);
                PropertyFieldOrLock(m_Rotation, !m_RotationEnabled.IsTrue);
                PropertyFieldOrLock(m_RotationTransformMode, !m_RotationEnabled.IsTrue);
                Space();

                PropertyField(m_ScaleEnabled);
                PropertyFieldOrLock(m_Scale, !m_ScaleEnabled.IsTrue);
                PropertyFieldOrLock(m_ScaleTransformMode, !m_ScaleEnabled.IsTrue);
                Space();
            }
            DustGUI.FoldoutEnd();
        }
    }
}
