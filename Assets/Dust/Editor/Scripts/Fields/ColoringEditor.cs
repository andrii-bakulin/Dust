using System;
using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    public class ColoringEditor
    {
        protected DuEditor.DuProperty m_ColorMode;
        protected DuEditor.DuProperty m_Color;
        protected DuEditor.DuProperty m_Gradient;

        protected DuEditor.DuProperty m_RainbowMinOffset;
        protected DuEditor.DuProperty m_RainbowMaxOffset;
        protected DuEditor.DuProperty m_RainbowRepeat;

        protected DuEditor.DuProperty m_RandomMinColor;
        protected DuEditor.DuProperty m_RandomMaxColor;

        //--------------------------------------------------------------------------------------------------------------

        public ColoringEditor(SerializedProperty coloring)
        {
            m_ColorMode = DuEditor.FindProperty(coloring, "m_ColorMode", "Mode");
            m_Color = DuEditor.FindProperty(coloring, "m_Color", "Color");
            m_Gradient = DuEditor.FindProperty(coloring, "m_Gradient", "Gradient");

            m_RainbowMinOffset = DuEditor.FindProperty(coloring, "m_RainbowMinOffset", "Min Offset");
            m_RainbowMaxOffset = DuEditor.FindProperty(coloring, "m_RainbowMaxOffset", "Max Offset");
            m_RainbowRepeat = DuEditor.FindProperty(coloring, "m_RainbowRepeat", "Repeat");

            m_RandomMinColor = DuEditor.FindProperty(coloring, "m_RandomMinColor", "Min Color");
            m_RandomMaxColor = DuEditor.FindProperty(coloring, "m_RandomMaxColor", "Max Color");
        }
        
        public void OnInspectorGUI()
        {
            if (DustGUI.FoldoutBegin("Color", "Coloring.Color"))
            {
                DuEditor.PropertyField(m_ColorMode);

                switch ((Coloring.ColorMode) m_ColorMode.valInt)
                {
                    case Coloring.ColorMode.Ignore:
                    case Coloring.ColorMode.RandomColor:
                        break;

                    case Coloring.ColorMode.Color:
                        DuEditor.PropertyField(m_Color);
                        break;

                    case Coloring.ColorMode.Gradient:
                        DuEditor.PropertyField(m_Gradient);
                        break;

                    case Coloring.ColorMode.Rainbow:
                        DuEditor.PropertyExtendedSlider(m_RainbowMinOffset, 0f, 1f, 0.01f);
                        DuEditor.PropertyExtendedSlider(m_RainbowMaxOffset, 0f, 1f, 0.01f);
                        DuEditor.PropertyField(m_RainbowRepeat);

                        DustGUI.Lock();
                        DustGUI.Field("Preview", DuGradient.CreateRainbow(m_RainbowMinOffset.valFloat, m_RainbowMaxOffset.valFloat));
                        DustGUI.Unlock();
                        break;

                    case Coloring.ColorMode.RandomColorInRange:
                        DuEditor.PropertyField(m_RandomMinColor);
                        DuEditor.PropertyField(m_RandomMaxColor);

                        DustGUI.Lock();
                        DustGUI.Field("Preview", DuGradient.CreateBetweenColors(m_RandomMinColor.valColor, m_RandomMaxColor.valColor));
                        DustGUI.Unlock();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                DuEditor.Space();
            }
            DustGUI.FoldoutEnd();
        }
    }
}
