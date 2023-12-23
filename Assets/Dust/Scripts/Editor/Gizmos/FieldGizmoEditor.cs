using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(FieldGizmo))]
    [CanEditMultipleObjects]
    public class FieldGizmoEditor : AbstractGizmoEditor
    {
        private DuProperty m_SourceType;
        private DuProperty m_Field;
        private DuProperty m_FieldsSpace;

        private DuProperty m_GridSize;
        private DuProperty m_GridOffset;
        private DuProperty m_GridPointsCount;

        private DuProperty m_PowerVisible;
        private DuProperty m_PowerSize;
        private DuProperty m_PowerDotsVisible;
        private DuProperty m_PowerDotsSize;
        private DuProperty m_PowerDotsColor;
        private DuProperty m_PowerImpactOnDotsSize;

        private DuProperty m_ColorVisible;
        private DuProperty m_ColorSize;
        private DuProperty m_PowerImpactOnColorSize;
        private DuProperty m_ColorAllowTransparent;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Field")]
        public static void AddComponentToSelectedObjects()
        {
            AddGizmoToSelectedOrNewObject(typeof(FieldGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_SourceType = FindProperty("m_SourceType", "Source Type");
            m_Field = FindProperty("m_Field", "Field");
            m_FieldsSpace = FindProperty("m_FieldsSpace", "Fields Space");

            m_GridSize = FindProperty("m_GridSize", "Size");
            m_GridOffset = FindProperty("m_GridOffset", "Offset");
            m_GridPointsCount = FindProperty("m_GridPointsCount", "Points Count");

            m_PowerVisible = FindProperty("m_PowerVisible", "Visible");
            m_PowerSize = FindProperty("m_PowerSize", "Size");
            m_PowerImpactOnDotsSize = FindProperty("m_PowerImpactOnDotsSize", "Change Size by Power");
            m_PowerDotsVisible = FindProperty("m_PowerDotsVisible", "Dots Visible");
            m_PowerDotsSize = FindProperty("m_PowerDotsSize", "Dots Size");
            m_PowerDotsColor = FindProperty("m_PowerDotsColor", "Dots Color");

            m_ColorVisible = FindProperty("m_ColorVisible", "Visible");
            m_ColorSize = FindProperty("m_ColorSize", "Size");
            m_PowerImpactOnColorSize = FindProperty("m_PowerImpactOnColorSize", "Change Size by Power");
            m_ColorAllowTransparent = FindProperty("m_ColorAllowTransparent", "Allow Transparent");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_SourceType);

            switch ((FieldGizmo.SourceType)m_SourceType.valInt)
            {
                case FieldGizmo.SourceType.Field:
                    PropertyField(m_Field);
                    break;

                case FieldGizmo.SourceType.FieldsSpace:
                    PropertyField(m_FieldsSpace);
                    break;
            }

            Space();

            if (DustGUI.FoldoutBegin("Grid", "FieldGizmo.Grid"))
            {
                PropertyField(m_GridSize);
                PropertyField(m_GridOffset);
                Space();
                PropertyField(m_GridPointsCount);
                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Power", "FieldGizmo.Power"))
            {
                PropertyField(m_PowerVisible);
                PropertyExtendedSlider(m_PowerSize, 0.1f, 2.0f, +0.1f, 0.1f);
                Space();
                PropertyField(m_PowerDotsVisible);
                PropertyExtendedSlider(m_PowerDotsSize, 0.1f, 2.0f, +0.1f, 0.1f);
                PropertyField(m_PowerDotsColor);
                Space();
                PropertyField(m_PowerImpactOnDotsSize);
                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Color", "FieldGizmo.Color"))
            {
                PropertyField(m_ColorVisible);
                PropertyExtendedSlider(m_ColorSize, 0.1f, 5.0f, +0.1f, 0.1f);
                PropertyField(m_PowerImpactOnColorSize);
                PropertyField(m_ColorAllowTransparent);
                Space();
            }
            DustGUI.FoldoutEnd();

            PropertyField(m_GizmoVisibility);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_GridPointsCount.isChanged)
                m_GridPointsCount.valVector3Int = FieldGizmo.NormalizeGridPointsCount(m_GridPointsCount.valVector3Int);

            if (m_PowerSize.isChanged)
                m_PowerSize.valFloat = FieldGizmo.NormalizeSize(m_PowerSize.valFloat);

            if (m_PowerDotsSize.isChanged)
                m_PowerDotsSize.valFloat = FieldGizmo.NormalizeSize(m_PowerDotsSize.valFloat);

            if (m_ColorSize.isChanged)
                m_ColorSize.valFloat = FieldGizmo.NormalizeSize(m_ColorSize.valFloat);

            InspectorCommitUpdates();
        }
    }
}
