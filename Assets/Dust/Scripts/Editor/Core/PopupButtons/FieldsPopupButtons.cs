using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    public class FieldsPopupButtons : PopupButtons
    {
        protected FieldsMapEditor m_FieldsMap;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddBasicField(System.Type type, string title)
        {
            AddEntity("Fields.Basic", type, title);
        }

        public static void Add2DField(System.Type type, string title)
        {
            AddEntity("Fields.2D", type, title);
        }

        public static void Add3DField(System.Type type, string title)
        {
            AddEntity("Fields.3D", type, title);
        }

        public static void AddMathField(System.Type type, string title)
        {
            AddEntity("Fields.Math", type, title);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static PopupButtons Popup(FieldsMapEditor fieldsMap)
        {
            var popup = new FieldsPopupButtons();
            popup.m_FieldsMap = fieldsMap;

            GenerateColumn(popup, "Fields.Basic", "Basic Fields");
            GenerateColumn(popup, "Fields.2D", "2D Fields");
            GenerateColumn(popup, "Fields.3D", "3D Fields");
            GenerateColumn(popup, "Fields.Math", "Math Fields");

            return popup;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override bool OnButtonClicked(CellRecord cellRecord)
        {
            FieldEditor.AddFieldComponentByType(m_FieldsMap.GetParentGameObject(), cellRecord.type);
            return true;
        }
    }
}
