using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public class PopupExtraSlider : PopupWindowContent
    {
        internal const float WIDTH = 200f;
        internal const float HEIGHT = 23f;

        private GameObject m_ActiveGameObject;
        private SerializedObject m_SerializedObject;
        private SerializedProperty m_Property;
        private string m_Title;

        //--------------------------------------------------------------------------------------------------------------

        public static PopupExtraSlider Create(SerializedObject serializedObject, SerializedProperty property)
        {
            var popup = new PopupExtraSlider();
            popup.m_ActiveGameObject = Selection.activeGameObject;
            popup.m_SerializedObject = serializedObject;
            popup.m_Property = property;
            popup.m_Title = "";
            return popup;
        }

        public static PopupExtraSlider Create(SerializedObject serializedObject, string title, SerializedProperty property)
        {
            var popup = new PopupExtraSlider();
            popup.m_ActiveGameObject = Selection.activeGameObject;
            popup.m_SerializedObject = serializedObject;
            popup.m_Property = property;
            popup.m_Title = title;
            return popup;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override Vector2 GetWindowSize()
        {
            if (m_Title != "")
                return new Vector2(WIDTH, HEIGHT + 22f);

            return new Vector2(WIDTH, HEIGHT);
        }

        public override void OnGUI(Rect rect)
        {
            if (m_ActiveGameObject != Selection.activeGameObject)
                return;

            if (m_Title != "")
                DustGUI.SimpleLabel(m_Title, DustGUI.NewStyleLabel().PaddingBottom(3).Build());

            if (DustGUI.ExtraSlider.Create01().Draw(m_Property))
                m_SerializedObject.ApplyModifiedProperties();
        }
    }
}
