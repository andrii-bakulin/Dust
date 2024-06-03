using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    public class ActionsPopupButtons : PopupButtons
    {
        protected Action m_Action;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddActionAnimate(System.Type type, string title)
        {
            AddEntity("Actions.Animate", type, title);
        }

        public static void AddActionGameObject(System.Type type, string title)
        {
            AddEntity("Actions.GameObject", type, title);
        }

        public static void AddActionTransform(System.Type type, string title)
        {
            AddEntity("Actions.Transform", type, title);
        }

        public static void AddActionPhysics(System.Type type, string title)
        {
            AddEntity("Actions.Physics", type, title);
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
            GenerateColumn(popup, "Actions.Physics", "Physics");
            GenerateColumn(popup, "Actions.GameObject", "GameObject");
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
