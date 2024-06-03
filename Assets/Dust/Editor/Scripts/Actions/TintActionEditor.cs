using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(TintAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TintActionEditor : IntervalWithRollbackActionEditor
    {
        protected DuProperty m_TintColor;
        protected DuProperty m_TintMode;
        protected DuProperty m_PropertyName;

        protected TintAction.TintMode tintMode => (TintAction.TintMode) m_TintMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static TintActionEditor()
        {
            ActionsPopupButtons.AddActionAnimate(typeof(TintAction), "Tint");
        }

        [MenuItem("Dust/Actions/Tint")]
        [MenuItem("GameObject/Dust/Actions/Tint")]
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
            m_PropertyName = FindProperty("m_PropertyName", "Property Name", 
                "To change color for Material use next color property names:" + "\n" +
                "- _BaseColor for URP and HDRP" + "\n" +
                "- _Color for Standard Render Pipeline");
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
