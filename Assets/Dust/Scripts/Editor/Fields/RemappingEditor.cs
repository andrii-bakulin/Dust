using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public class RemappingEditor
    {
        private Remapping m_Remapping;
        private Material m_DrawerMaterial;

        protected DuEditor.DuProperty m_RemapPowerEnabled;

        protected DuEditor.DuProperty m_InMin;
        protected DuEditor.DuProperty m_InMax;
        protected DuEditor.DuProperty m_OutMin;
        protected DuEditor.DuProperty m_OutMax;

        protected DuEditor.DuProperty m_Strength;
        protected DuEditor.DuProperty m_Invert;

        protected DuEditor.DuProperty m_ClampInMode;

        protected DuEditor.DuProperty m_ClampOutMode;
        protected DuEditor.DuProperty m_ClampOutMin;
        protected DuEditor.DuProperty m_ClampOutMax;

        protected DuEditor.DuProperty m_PostPower;
        protected DuEditor.DuProperty m_PostReshapeMode;
        protected DuEditor.DuProperty m_PostStepsCount;
        protected DuEditor.DuProperty m_PostCurve;

        protected DuEditor.DuProperty m_ColorMode;
        protected DuEditor.DuProperty m_Color;
        protected DuEditor.DuProperty m_Gradient;

        protected DuEditor.DuProperty m_RainbowMinOffset;
        protected DuEditor.DuProperty m_RainbowMaxOffset;
        protected DuEditor.DuProperty m_RainbowRepeat;

        protected DuEditor.DuProperty m_RandomMinColor;
        protected DuEditor.DuProperty m_RandomMaxColor;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private ClampMode clampInMode => (ClampMode) m_ClampInMode.valInt;
        private ClampMode clampOutMode => (ClampMode) m_ClampOutMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        public RemappingEditor(Remapping remapping, SerializedProperty remappingProperty)
        {
            m_Remapping = remapping;
            m_DrawerMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));

            m_RemapPowerEnabled = DuEditor.FindProperty(remappingProperty, "m_RemapPowerEnabled", "Enabled");

            m_InMin = DuEditor.FindProperty(remappingProperty, "m_InMin", "In Min");
            m_InMax = DuEditor.FindProperty(remappingProperty, "m_InMax", "In Max");
            m_OutMin = DuEditor.FindProperty(remappingProperty, "m_OutMin", "Out Min");
            m_OutMax = DuEditor.FindProperty(remappingProperty, "m_OutMax", "Out Max");
        
            m_Strength = DuEditor.FindProperty(remappingProperty, "m_Strength", "Strength");
            m_Invert = DuEditor.FindProperty(remappingProperty, "m_Invert", "Invert");

            m_ClampInMode = DuEditor.FindProperty(remappingProperty, "m_ClampInMode", "Clamp In Mode");

            m_ClampOutMode = DuEditor.FindProperty(remappingProperty, "m_ClampOutMode", "Clamp Out Mode");
            m_ClampOutMin = DuEditor.FindProperty(remappingProperty, "m_ClampOutMin", "Clamp Out Min");
            m_ClampOutMax = DuEditor.FindProperty(remappingProperty, "m_ClampOutMax", "Clamp Out Max");

            m_PostPower = DuEditor.FindProperty(remappingProperty, "m_PostPower", "Post Power");
            m_PostReshapeMode = DuEditor.FindProperty(remappingProperty, "m_PostReshapeMode", "Post Reshape");
            m_PostStepsCount = DuEditor.FindProperty(remappingProperty, "m_PostStepsCount", "Steps Count");
            m_PostCurve = DuEditor.FindProperty(remappingProperty, "m_PostCurve", "Curve Shape");

            m_ColorMode = DuEditor.FindProperty(remappingProperty, "m_ColorMode", "Mode");
            m_Color = DuEditor.FindProperty(remappingProperty, "m_Color", "Color");
            m_Gradient = DuEditor.FindProperty(remappingProperty, "m_Gradient", "Gradient");

            m_RainbowMinOffset = DuEditor.FindProperty(remappingProperty, "m_RainbowMinOffset", "Min Offset");
            m_RainbowMaxOffset = DuEditor.FindProperty(remappingProperty, "m_RainbowMaxOffset", "Max Offset");
            m_RainbowRepeat = DuEditor.FindProperty(remappingProperty, "m_RainbowRepeat", "Repeat");

            m_RandomMinColor = DuEditor.FindProperty(remappingProperty, "m_RandomMinColor", "Min Color");
            m_RandomMaxColor = DuEditor.FindProperty(remappingProperty, "m_RandomMaxColor", "Max Color");
        }

        public void OnInspectorGUI()
            => OnInspectorGUI(true);

        public void OnInspectorGUI(bool showColorBlock)
        {
            if (DustGUI.FoldoutBegin("Remap Power", "Remapping.Power"))
            {
                DuEditor.PropertyField(m_RemapPowerEnabled);

                PropertyMappingGraph(m_Remapping, m_Color.valColor.duToRGBWithoutAlpha());

                if (m_RemapPowerEnabled.IsTrue)
                {
                    DuEditor.PropertyExtendedSlider01(m_InMin);
                    DuEditor.PropertyExtendedSlider01(m_InMax);
                    DuEditor.PropertyField(m_ClampInMode);

                    DuEditor.Space();

                    DuEditor.PropertyExtendedSlider(m_Strength, 0f, 1f, 0.01f);

                    DuEditor.Space();

                    DuEditor.PropertyExtendedSlider(m_OutMin, 0f, 1f, 0.01f);
                    DuEditor.PropertyExtendedSlider(m_OutMax, 0f, 1f, 0.01f);
                    DuEditor.PropertyField(m_ClampOutMode);
                    {
                        if (clampOutMode == ClampMode.MinOnly || clampOutMode == ClampMode.MinAndMax)
                            DuEditor.PropertyExtendedSlider(m_ClampOutMin, 0f, 1f, 0.01f);
                        if (clampOutMode == ClampMode.MaxOnly || clampOutMode == ClampMode.MinAndMax)
                            DuEditor.PropertyExtendedSlider(m_ClampOutMax, 0f, 1f, 0.01f);
                    }
                    DuEditor.Space();

                    DuEditor.PropertyField(m_Invert);
                    DuEditor.Space();

                    DustGUI.Header("Post Update");
                    DuEditor.PropertyExtendedSlider(m_PostPower, 0f, 1f, 0.01f);
                    DuEditor.PropertyField(m_PostReshapeMode);

                    switch ((Remapping.PostReshapeMode) m_PostReshapeMode.valInt)
                    {
                        case Remapping.PostReshapeMode.None:
                            break;

                        case Remapping.PostReshapeMode.Curve:
                            DuEditor.PropertyFieldCurve(m_PostCurve);
                            break;

                        case Remapping.PostReshapeMode.Steps:
                            DuEditor.PropertyExtendedIntSlider(m_PostStepsCount, 1, 25, 1, 1);
                            break;
                    }
                }

                DuEditor.Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (showColorBlock)
            {
                if (DustGUI.FoldoutBegin("Color", "Remapping.Color"))
                {
                    DuEditor.PropertyField(m_ColorMode);

                    switch ((Remapping.ColorMode) m_ColorMode.valInt)
                    {
                        case Remapping.ColorMode.Ignore:
                        case Remapping.ColorMode.RandomColor:
                            break;

                        case Remapping.ColorMode.Color:
                            DuEditor.PropertyField(m_Color);
                            break;

                        case Remapping.ColorMode.Gradient:
                            DuEditor.PropertyField(m_Gradient);
                            break;

                        case Remapping.ColorMode.Rainbow:
                            DuEditor.PropertyExtendedSlider(m_RainbowMinOffset, 0f, 1f, 0.01f);
                            DuEditor.PropertyExtendedSlider(m_RainbowMaxOffset, 0f, 1f, 0.01f);
                            DuEditor.PropertyField(m_RainbowRepeat);

                            DustGUI.Lock();
                            DustGUI.Field("Preview", DuGradient.CreateRainbow(m_RainbowMinOffset.valFloat, m_RainbowMaxOffset.valFloat));
                            DustGUI.Unlock();
                            break;

                        case Remapping.ColorMode.RandomColorInRange:
                            DuEditor.PropertyField(m_RandomMinColor);
                            DuEditor.PropertyField(m_RandomMaxColor);

                            DustGUI.Lock();
                            DustGUI.Field("Preview", DuGradient.CreateBetweenColors(m_RandomMinColor.valColor, m_RandomMaxColor.valColor));
                            DustGUI.Unlock();
                            break;
                    }

                    DuEditor.Space();
                }
                DustGUI.FoldoutEnd();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_InMin.isChanged)
                m_InMin.valFloat = Remapping.NormalizeInMinMax(m_InMin.valFloat);

            if (m_InMax.isChanged)
                m_InMax.valFloat = Remapping.NormalizeInMinMax(m_InMax.valFloat);

            if (m_PostStepsCount.isChanged)
                m_PostStepsCount.valInt = Remapping.NormalizePostStepsCount(m_PostStepsCount.valInt);

            if (m_PostCurve.isChanged)
                m_PostCurve.valAnimationCurve = Remapping.NormalizePostCurve(m_PostCurve.valAnimationCurve);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void PropertyMappingGraph(Remapping remapping, Color color)
        {
            // Begin to draw a horizontal layout, using the helpBox EditorStyle
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            float kPaddingH = 30f;
            float kPaddingV = 15f;

            Color bgColor = new Color(0.15f, 0.15f, 0.15f);
            float kHeight = 140;

            // Reserve GUI space with a width from 10 to 10000, and a fixed height of 200, and
            // cache it as a rectangle.
            Rect layoutRectangle = GUILayoutUtility.GetRect(10, 10000, kHeight, kHeight);

            float kWidth = layoutRectangle.width;

            if (Event.current.type == EventType.Repaint)
            {
                // If we are currently in the Repaint event, begin to draw a clip of the size of
                // previously reserved rectangle, and push the current matrix for drawing.
                GUI.BeginClip(layoutRectangle);
                GL.PushMatrix();

                // Clear the current render buffer, setting a new background colour, and set our
                // material for rendering.
                GL.Clear(true, false, bgColor);
                m_DrawerMaterial.SetPass(0);

                // Start drawing in OpenGL Quads, to draw the background canvas. Set the
                // colour black as the current OpenGL drawing colour, and draw a quad covering
                // the dimensions of the layoutRectangle.
                GL.Begin(GL.QUADS);
                GL.Color(bgColor);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(layoutRectangle.width, 0, 0);
                GL.Vertex3(layoutRectangle.width, layoutRectangle.height, 0);
                GL.Vertex3(0, layoutRectangle.height, 0);
                GL.End();

                // Start drawing in OpenGL Lines, to draw the lines of the grid.
                GL.Begin(GL.LINES);
                GL.Color(color);

                float rangePaddingH = (kWidth / (kWidth - 2f * kPaddingH) - 1f) / 2f;
                float rangePaddingV = (kHeight / (kHeight - 2f * kPaddingV) - 1f) / 2f;

                float rangeMin = 0f - rangePaddingH;
                float rangeMax = 1f + rangePaddingH;

                for (int i = 0; i <= kWidth; i++)
                {
                    float offset = Mathf.LerpUnclamped(rangeMin, rangeMax, i / kWidth);

                    float value = remapping.MapValue(offset);
                    value = 1f - value; // To draw upside down
                    value = Mathf.Clamp(value, 0f - rangePaddingV, 1f + rangePaddingV);

                    float yDrawFrom = DuMath.Fit(0f - rangePaddingV, 1f + rangePaddingV, 0, kHeight, value);

                    DrawLine(i, yDrawFrom, i, kHeight);
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Show over grid

                GL.Color(Color.black);

                DrawLine(kPaddingH, 0, kPaddingH, kHeight);
                DrawLine(kWidth - kPaddingH, 0, kWidth - kPaddingH, kHeight);

                DrawLine(0, kPaddingV, kWidth, kPaddingV);
                DrawLine(0, kHeight - kPaddingV + 1f, kWidth, kHeight - kPaddingV + 1f);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                // End lines drawing.
                GL.End();

                // Pop the current matrix for rendering, and end the drawing clip.
                GL.PopMatrix();
                GUI.EndClip();
            }

            // End our horizontal
            GUILayout.EndHorizontal();
        }

        private void DrawLine(float x0, float y0, float x1, float y1)
        {
            GL.Vertex3(x0, y0, 0);
            GL.Vertex3(x1, y1, 0);

            if (EditorGUIUtility.pixelsPerPoint > 1.0f && !y0.Equals(y1))
            {
                // This require for retina display
                GL.Vertex3(x0 + 0.5f, y0, 0);
                GL.Vertex3(x1 + 0.5f, y1, 0);
            }
        }
    }
}
