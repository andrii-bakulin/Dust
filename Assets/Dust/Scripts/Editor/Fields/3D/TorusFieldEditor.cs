﻿using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(TorusField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TorusFieldEditor : Space3DFieldEditor
    {
        private DuProperty m_Radius;
        private DuProperty m_Thickness;
        private DuProperty m_Direction;

        //--------------------------------------------------------------------------------------------------------------

        static TorusFieldEditor()
        {
            FieldsPopupButtons.Add3DField(typeof(TorusField), "Torus");
        }

        [MenuItem("Dust/Fields/3D Fields/Torus")]
        [MenuItem("GameObject/Dust/Fields/3D Fields/Torus")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(TorusField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Radius = FindProperty("m_Radius", "Radius");
            m_Thickness = FindProperty("m_Thickness", "Thickness");
            m_Direction = FindProperty("m_Direction", "Direction");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            PropertyField(m_CustomHint);
            Space();

            PropertyExtendedSlider(m_Radius, 0f, 20f, 0.01f);
            PropertyExtendedSlider(m_Thickness, 0f, 20f, 0.01f);
            PropertyField(m_Direction);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            
            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_GizmoBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Radius.isChanged)
                m_Radius.valFloat = TorusField.NormalizeRadius(m_Radius.valFloat);

            if (m_Thickness.isChanged)
                m_Thickness.valFloat = TorusField.NormalizeThickness(m_Thickness.valFloat);

            InspectorCommitUpdates();
        }
    }
}
