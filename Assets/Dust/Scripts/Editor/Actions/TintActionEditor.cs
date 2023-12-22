using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(TintAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TintActionEditor : IntervalWithRollbackActionEditor
    {
        private DuProperty m_TintColor;
        private DuProperty m_TintMode;
        private DuProperty m_PropertyName;

        protected TintAction.TintMode tintMode => (TintAction.TintMode) m_TintMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static TintActionEditor()
        {
            ActionsPopupButtons.AddActionAnimate(typeof(TintAction), "Tint");
        }

        [MenuItem("Dust/Actions/Tint")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Tint Action", typeof(TintAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_TintColor = FindProperty("m_TintColor", "Tint Color");
            m_TintMode = FindProperty("m_TintMode", "Tint Mode");
            m_PropertyName = FindProperty("m_PropertyName", "Property Name");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_TintColor);
            PropertyField(m_TintMode);
                
            if (tintMode == TintAction.TintMode.MeshRenderer)
            {
                PropertyField(m_PropertyName);
            }
                
            Space();
                
            OnInspectorGUI_Duration();

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("TintAction");
            OnInspectorGUI_Extended("TintAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
