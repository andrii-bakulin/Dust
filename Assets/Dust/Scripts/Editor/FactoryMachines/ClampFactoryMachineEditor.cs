using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(ClampFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class ClampFactoryMachineEditor : BasicFactoryMachineEditor
    {
        protected DuProperty m_PositionMode;
        protected DuProperty m_PositionMin;
        protected DuProperty m_PositionMax;
        protected DuProperty m_PositionClampX;
        protected DuProperty m_PositionClampY;
        protected DuProperty m_PositionClampZ;

        protected DuProperty m_RotationMode;
        protected DuProperty m_RotationMin;
        protected DuProperty m_RotationMax;
        protected DuProperty m_RotationClampX;
        protected DuProperty m_RotationClampY;
        protected DuProperty m_RotationClampZ;

        protected DuProperty m_ScaleMode;
        protected DuProperty m_ScaleMin;
        protected DuProperty m_ScaleMax;
        protected DuProperty m_ScaleClampX;
        protected DuProperty m_ScaleClampY;
        protected DuProperty m_ScaleClampZ;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private ClampMode positionMode => (ClampMode) m_PositionMode.valInt;
        private ClampMode rotationMode => (ClampMode) m_RotationMode.valInt;
        private ClampMode scaleMode => (ClampMode) m_ScaleMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static ClampFactoryMachineEditor()
        {
            FactoryMachinesPopupButtons.AddMachine(typeof(ClampFactoryMachine), "Clamp");
        }

        [MenuItem("Dust/Factory Machines/Clamp")]
        [MenuItem("GameObject/Dust/Factory Machines/Clamp")]
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

            InspectorBreadcrumbsForFactoryMachine(this);

            PropertyField(m_Hint);
            PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_FieldsMap();

            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
