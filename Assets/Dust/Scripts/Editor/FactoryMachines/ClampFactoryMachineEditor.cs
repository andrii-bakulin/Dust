using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(ClampFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ClampFactoryMachineEditor : BasicFactoryMachineEditor
    {
        private DuProperty m_PositionMode;
        private DuProperty m_PositionMin;
        private DuProperty m_PositionMax;
        private DuProperty m_PositionClampX;
        private DuProperty m_PositionClampY;
        private DuProperty m_PositionClampZ;

        private DuProperty m_RotationMode;
        private DuProperty m_RotationMin;
        private DuProperty m_RotationMax;
        private DuProperty m_RotationClampX;
        private DuProperty m_RotationClampY;
        private DuProperty m_RotationClampZ;

        private DuProperty m_ScaleMode;
        private DuProperty m_ScaleMin;
        private DuProperty m_ScaleMax;
        private DuProperty m_ScaleClampX;
        private DuProperty m_ScaleClampY;
        private DuProperty m_ScaleClampZ;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private ClampMode positionMode => (ClampMode) m_PositionMode.valInt;
        private ClampMode rotationMode => (ClampMode) m_RotationMode.valInt;
        private ClampMode scaleMode => (ClampMode) m_ScaleMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static ClampFactoryMachineEditor()
        {
            FactoryMachinesPopupButtons.AddMachine(typeof(ClampFactoryMachine), "Clamp");
        }

        [MenuItem("Dust/Factory/Machines/Clamp")]
        [MenuItem("GameObject/Dust/Factory/Machines/Clamp")]
        public new static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(ClampFactoryMachine));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_PositionMode = FindProperty("m_PositionMode", "Position");
            m_PositionMin = FindProperty("m_PositionMin", "Min");
            m_PositionMax = FindProperty("m_PositionMax", "Max");
            m_PositionClampX = FindProperty("m_PositionClampX", "Clamp Axis X");
            m_PositionClampY = FindProperty("m_PositionClampY", "Clamp Axis Y");
            m_PositionClampZ = FindProperty("m_PositionClampZ", "Clamp Axis Z");

            m_RotationMode = FindProperty("m_RotationMode", "Rotation");
            m_RotationMin = FindProperty("m_RotationMin", "Min");
            m_RotationMax = FindProperty("m_RotationMax", "Max");
            m_RotationClampX = FindProperty("m_RotationClampX", "Clamp Axis X");
            m_RotationClampY = FindProperty("m_RotationClampY", "Clamp Axis Y");
            m_RotationClampZ = FindProperty("m_RotationClampZ", "Clamp Axis Z");

            m_ScaleMode = FindProperty("m_ScaleMode", "Scale");
            m_ScaleMin = FindProperty("m_ScaleMin", "Min");
            m_ScaleMax = FindProperty("m_ScaleMax", "Max");
            m_ScaleClampX = FindProperty("m_ScaleClampX", "Clamp Axis X");
            m_ScaleClampY = FindProperty("m_ScaleClampY", "Clamp Axis Y");
            m_ScaleClampZ = FindProperty("m_ScaleClampZ", "Clamp Axis Z");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // OnInspectorGUI_BaseParameters();

            if (DustGUI.FoldoutBegin("Parameters", "FactoryMachine.Parameters"))
            {
                PropertyField(m_CustomHint);
                PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
                Space();

                PropertyField(m_PositionMode);
                {
                    bool showMin = false;
                    bool showMax = false;

                    showMin |= positionMode == ClampMode.MinOnly;
                    showMin |= positionMode == ClampMode.MinAndMax;

                    showMax |= positionMode == ClampMode.MaxOnly;
                    showMax |= positionMode == ClampMode.MinAndMax;

                    PropertyFieldOrHide(m_PositionMin, !showMin);
                    PropertyFieldOrHide(m_PositionMax, !showMax);

                    if (showMin || showMax)
                    {
                        PropertyField(m_PositionClampX);
                        PropertyField(m_PositionClampY);
                        PropertyField(m_PositionClampZ);
                    }
                }
                Space();

                PropertyField(m_RotationMode);
                {
                    bool showMin = false;
                    bool showMax = false;

                    showMin |= rotationMode == ClampMode.MinOnly;
                    showMin |= rotationMode == ClampMode.MinAndMax;

                    showMax |= rotationMode == ClampMode.MaxOnly;
                    showMax |= rotationMode == ClampMode.MinAndMax;

                    PropertyFieldOrHide(m_RotationMin, !showMin);
                    PropertyFieldOrHide(m_RotationMax, !showMax);

                    if (showMin || showMax)
                    {
                        PropertyField(m_RotationClampX);
                        PropertyField(m_RotationClampY);
                        PropertyField(m_RotationClampZ);
                    }
                }
                Space();

                PropertyField(m_ScaleMode);
                {
                    bool showMin = false;
                    bool showMax = false;

                    showMin |= scaleMode == ClampMode.MinOnly;
                    showMin |= scaleMode == ClampMode.MinAndMax;

                    showMax |= scaleMode == ClampMode.MaxOnly;
                    showMax |= scaleMode == ClampMode.MinAndMax;

                    PropertyFieldOrHide(m_ScaleMin, !showMin);
                    PropertyFieldOrHide(m_ScaleMax, !showMax);

                    if (showMin || showMax)
                    {
                        PropertyField(m_ScaleClampX);
                        PropertyField(m_ScaleClampY);
                        PropertyField(m_ScaleClampZ);
                    }
                }
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();
            OnInspectorGUI_FieldsMap();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
