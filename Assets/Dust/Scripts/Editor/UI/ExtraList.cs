using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor.UI
{
    public static class ExtraList
    {
        private static GUIStyle m_StyleMiniButton;
        public static GUIStyle styleMiniButton => m_StyleMiniButton;

        private static GUIStyle m_StyleHintLabel;
        public static GUIStyle styleHintLabel => m_StyleHintLabel;

        private static GUIStyle m_StyleIntensityButton;
        public static GUIStyle styleIntensityButton => m_StyleIntensityButton;

        private static GUIStyle m_StyleDropDownList;
        public static GUIStyle styleDropDownList => m_StyleDropDownList;

        static ExtraList()
        {
            m_StyleMiniButton = DustGUI.NewStyleButton().Padding(2, 0).Margin(0).Build();

            m_StyleHintLabel = DustGUI.NewStyleLabel()
                .PaddingTop(0).PaddingBottom(0)
                .MarginTop(0).MarginBottom(0)
                .FontSizeScaled(0.8f)
                .NormalTextColor(Color.gray)
                .Build();

            m_StyleIntensityButton = DustGUI.NewStyleButton().MarginTop(6).Build();

            m_StyleDropDownList = DustGUI.NewStylePopup().MarginTop(7).Build();
        }
    }
}
