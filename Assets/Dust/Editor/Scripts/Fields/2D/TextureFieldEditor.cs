using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(TextureField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TextureFieldEditor : SpaceFieldEditor
    {
        protected DuProperty m_Texture;
        protected DuProperty m_WrapMode;

        protected DuProperty m_Width;
        protected DuProperty m_Height;
        protected DuProperty m_Direction;

        protected DuProperty m_FlipX;
        protected DuProperty m_FlipY;

        protected DuProperty m_PowerSource;
        protected DuProperty m_ApplyPowerToAlpha;

        //--------------------------------------------------------------------------------------------------------------

        static TextureFieldEditor()
        {
            FieldsPopupButtons.Add2DField(typeof(TextureField), "Texture");
        }

        [MenuItem("Dust/Fields/2D Fields/Texture")]
        [MenuItem("GameObject/Dust/Fields/2D Fields/Texture")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(TextureField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Texture = FindProperty("m_Texture", "Texture");
            m_WrapMode = FindProperty("m_WrapMode", "Wrap Mode");

            m_Width = FindProperty("m_Width", "Width");
            m_Height = FindProperty("m_Height", "Height");
            m_Direction = FindProperty("m_Direction", "Direction");

            m_FlipX = FindProperty("m_FlipX", "Flip X");
            m_FlipY = FindProperty("m_FlipY", "Flip Y");

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
            PropertyField(m_WrapMode);
            PropertyExtendedSlider(m_Width, 0f, 50f, 0.01f);
            PropertyExtendedSlider(m_Height, 0f, 50f, 0.01f);
            PropertyField(m_Direction);
            PropertyField(m_FlipX);
            PropertyField(m_FlipY);
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
            // OnInspectorGUI_RemappingBlock();

            if ((TextureField.ColorComponent) m_PowerSource.valInt != TextureField.ColorComponent.Ignore)
            {
                OnInspectorGUI_RemappingBlock();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Width.isChanged)
                m_Width.valFloat = TextureField.NormalizeWidth(m_Width.valFloat);

            if (m_Height.isChanged)
                m_Height.valFloat = TextureField.NormalizeHeight(m_Height.valFloat);

            InspectorCommitUpdates();
        }
    }
}
