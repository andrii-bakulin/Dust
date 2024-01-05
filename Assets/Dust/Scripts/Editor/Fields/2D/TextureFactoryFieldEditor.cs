using System;
using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(TextureFactoryField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TextureFactoryFieldEditor : BasicFieldEditor
    {
        protected DuProperty m_Texture;
        protected DuProperty m_SpaceUVW;

        protected DuProperty m_FlipU;
        protected DuProperty m_FlipV;
        protected DuProperty m_FlipW;

        protected DuProperty m_PowerSource;
        protected DuProperty m_ApplyPowerToAlpha;

        //--------------------------------------------------------------------------------------------------------------

        static TextureFactoryFieldEditor()
        {
            FieldsPopupButtons.Add2DField(typeof(TextureFactoryField), "Texture Factory");
        }

        [MenuItem("Dust/Fields/2D Fields/Texture Factory")]
        [MenuItem("GameObject/Dust/Fields/2D Fields/Texture Factory")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(TextureFactoryField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Texture = FindProperty("m_Texture", "Texture");
            m_SpaceUVW = FindProperty("m_SpaceUVW", "Space");

            m_FlipU = FindProperty("m_FlipU", "Flip U");
            m_FlipV = FindProperty("m_FlipV", "Flip V");
            m_FlipW = FindProperty("m_FlipW", "Flip W");

            m_PowerSource = FindProperty("m_PowerSource", "Power Source");

            m_ApplyPowerToAlpha = FindProperty("m_ApplyPowerToAlpha", "Apply Power To Alpha");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForField(this);

            PropertyExtendedSlider(m_Power, 0f, 5f, 0.01f);
            Space();

            PropertyField(m_Texture);
            PropertyField(m_SpaceUVW);

            switch ((TextureFactoryField.SpaceUVW) m_SpaceUVW.valInt)
            {
                case TextureFactoryField.SpaceUVW.UV:
                    PropertyField(m_FlipU);
                    PropertyField(m_FlipV);
                    break;

                case TextureFactoryField.SpaceUVW.UW:
                    PropertyField(m_FlipU);
                    PropertyField(m_FlipW);
                    break;

                case TextureFactoryField.SpaceUVW.VW:
                    PropertyField(m_FlipV);
                    PropertyField(m_FlipW);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            Space();

            DustGUI.Header("Power Impact");
            PropertyField(m_PowerSource);
            Space();

            DustGUI.Header("Color Impact");
            PropertyField(m_ApplyPowerToAlpha);
            Space();

            PropertyField(m_Hint);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if ((TextureFactoryField.ColorComponent) m_PowerSource.valInt != TextureFactoryField.ColorComponent.Ignore)
            {
                OnInspectorGUI_RemappingBlock();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
