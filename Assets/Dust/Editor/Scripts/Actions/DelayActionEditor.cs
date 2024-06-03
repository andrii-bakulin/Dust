using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(DelayAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DelayActionEditor : IntervalActionEditor
    {
        protected DuProperty m_DelayMode;
        protected DuProperty m_DurationRange;
        protected DuProperty m_Seed;

        protected DelayAction.DelayMode delayMode => (DelayAction.DelayMode) m_DelayMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static DelayActionEditor()
        {
            ActionsPopupButtons.AddActionOthers(typeof(DelayAction), "Delay");
        }

        [MenuItem("Dust/Actions/Delay")]
        [MenuItem("GameObject/Dust/Actions/Delay")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Delay Action", typeof(DelayAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_DelayMode = FindProperty("m_DelayMode", "Mode");
            m_DurationRange = FindProperty("m_DurationRange");
            m_Seed = FindProperty("m_Seed", "Seed");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_DelayMode);

            if (delayMode == DelayAction.DelayMode.Fixed)
            {
                OnInspectorGUI_Duration();
            }
            else if (delayMode == DelayAction.DelayMode.Range)
            {
                PropertyFieldDurationRange(m_DurationRange); 
                PropertySeedRandomOrFixed(m_Seed);
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Callbacks("DelayAction");
            OnInspectorGUI_Extended("DelayAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
