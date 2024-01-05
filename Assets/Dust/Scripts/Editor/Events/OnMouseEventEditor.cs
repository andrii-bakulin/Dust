using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(OnMouseEvent))]
    [CanEditMultipleObjects]
    public class OnMouseEventEditor : OnAbstractEventEditor
    {
        protected DuProperty m_MouseButtonIndex;

        protected DuProperty m_OnMouseButtonDown;
        protected DuProperty m_OnMouseButtonHold;
        protected DuProperty m_OnMouseButtonUp;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Events/On Mouse")]
        [MenuItem("GameObject/Dust/Events/On Mouse")]
        public static void AddComponent()
        {
            AddComponentToSelectedOrNewObject("DuMouse", typeof(OnMouseEvent));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_MouseButtonIndex = FindProperty("m_MouseButtonIndex", "Button Index");

            m_OnMouseButtonDown = FindProperty("m_OnMouseButtonDown", "On Mouse Button Down Callbacks");
            m_OnMouseButtonHold = FindProperty("m_OnMouseButtonHold", "On Mouse Button Hold Callbacks");
            m_OnMouseButtonUp = FindProperty("m_OnMouseButtonUp", "On Mouse Button Up Callbacks");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_MouseButtonIndex);

            Space();

            var titleOnKeyDown = "Button Down" + (m_OnMouseButtonDown.valUnityEvent.arraySize > 0 ? " (" + m_OnMouseButtonDown.valUnityEvent.arraySize + ")" : "");
            var titleOnKeyHold = "Button Hold" + (m_OnMouseButtonHold.valUnityEvent.arraySize > 0 ? " (" + m_OnMouseButtonHold.valUnityEvent.arraySize + ")" : "");
            var titleOnKeyUp   = "Button Up"   + (m_OnMouseButtonUp.valUnityEvent.arraySize   > 0 ? " (" + m_OnMouseButtonUp.valUnityEvent.arraySize   + ")" : "");

            var tabIndex = DustGUI.Toolbar("OnKeyEvent.Events", new[] {titleOnKeyDown, titleOnKeyHold, titleOnKeyUp});

            switch (tabIndex)
            {
                case 0: PropertyField(m_OnMouseButtonDown); break;
                case 1: PropertyField(m_OnMouseButtonHold); break;
                case 2: PropertyField(m_OnMouseButtonUp); break;
            }

            Space();
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
