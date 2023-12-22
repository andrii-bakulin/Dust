using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public class ActionsPopupButtons : PopupButtons
    {
        private Action m_Action;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddActionAnimate(System.Type type, string title)
        {
            AddEntity("Actions.Animate", type, title);
        }

        public static void AddActionFlow(System.Type type, string title)
        {
            AddEntity("Actions.Flow", type, title);
        }

        public static void AddActionTransform(System.Type type, string title)
        {
            AddEntity("Actions.Transform", type, title);
        }

        public static void AddActionOthers(System.Type type, string title)
        {
            AddEntity("Actions.Others", type, title);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static PopupButtons Popup(Action action)
        {
            var popup = new ActionsPopupButtons();
            popup.m_Action = action;

            GenerateColumn(popup, "Actions.Animate", "Animate");
            GenerateColumn(popup, "Actions.Flow", "Flow");
            GenerateColumn(popup, "Actions.Transform", "Transform");
            GenerateColumn(popup, "Actions.Others", "Others");

            return popup;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override bool OnButtonClicked(CellRecord cellRecord)
        {
            Component newAction = Undo.AddComponent(m_Action.gameObject, cellRecord.type);
            
            if (m_Action as SequencedAction is SequencedAction sequencedAction)
                sequencedAction.onCompleteActions.Add((Action)newAction);

            return true;
        }
    }
}
