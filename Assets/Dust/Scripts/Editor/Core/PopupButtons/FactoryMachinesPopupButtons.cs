using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public class FactoryMachinesPopupButtons : PopupButtons
    {
        private FactoryEditor m_Factory;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddMachine(System.Type type, string title)
        {
            AddEntity("FactoryMachines", type, title);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static PopupButtons Popup(FactoryEditor factory)
        {
            var popup = new FactoryMachinesPopupButtons();
            popup.m_Factory = factory;

            GenerateColumn(popup, "FactoryMachines", "Machines");

            return popup;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override bool OnButtonClicked(CellRecord cellRecord)
        {
            FactoryMachineEditor.AddFactoryMachineComponentByType((m_Factory.target as DuMonoBehaviour).gameObject, cellRecord.type);
            return true;
        }
    }
}
