using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(TriggerGizmo))]
    [CanEditMultipleObjects]
    public class TriggerGizmoEditor : AbstractGizmoEditor
    {
        private DuProperty m_Size;

        private DuProperty m_TriggeredColor;
        private DuProperty m_TriggeredSize;

        private DuProperty m_ShowMessage;
        private DuProperty m_HideMessageOnIdleState;
        private DuProperty m_Message;
        private DuProperty m_MessagePosition;
        private DuProperty m_MessageOffset;
        private DuProperty m_MessageSize;
        private DuProperty m_MessageSizeInDepth;

        private DuProperty m_FalloffDuration;
        private DuProperty m_Center;

        private DuProperty m_DebugLog;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Trigger")]
        public static void AddComponentToSelectedObjects()
        {
            AddGizmoToSelectedOrNewObject(typeof(TriggerGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Size = FindProperty("m_Size", "Size");

            m_TriggeredColor = FindProperty("m_TriggeredColor", "Triggered Color");
            m_TriggeredSize = FindProperty("m_TriggeredSize", "Triggered Size");

            m_ShowMessage = FindProperty("m_ShowMessage", "Show Message");
            m_HideMessageOnIdleState = FindProperty("m_HideMessageOnIdleState", "Hide On Idle State");
            m_Message = FindProperty("m_Message", "Message");
            m_MessagePosition = FindProperty("m_MessagePosition", "Position");
            m_MessageOffset = FindProperty("m_MessageOffset", "Offset");
            m_MessageSize = FindProperty("m_MessageSize", "Size");
            m_MessageSizeInDepth = FindProperty("m_MessageSizeInDepth", "Size depend on depth");

            m_FalloffDuration = FindProperty("m_FalloffDuration", "Falloff Duration");
            m_Center = FindProperty("m_Center", "Center");

            m_DebugLog = FindProperty("m_DebugLog", "Write To Console As");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyExtendedSlider(m_Size, 0.01f, 5.0f, +0.01f, 0.01f);
            PropertyField(m_Color);

            Space();

            PropertyExtendedSlider(m_TriggeredSize, 0.01f, 5.0f, +0.01f, 0.01f);
            PropertyField(m_TriggeredColor);

            Space();

            PropertyField(m_ShowMessage);

            if (m_ShowMessage.IsTrue)
            {
                PropertyField(m_HideMessageOnIdleState);
                PropertyField(m_Message);

                PropertyField(m_MessagePosition);

                if ((TriggerGizmo.MessagePosition) m_MessagePosition.valInt != TriggerGizmo.MessagePosition.Center)
                {
                    PropertyExtendedSlider(m_MessageOffset, 0f, +1f, 0.01f, 0f, +1f);
                }

                PropertyExtendedSlider(m_MessageSize, 0.5f, 3.0f, 0.01f, 0.01f, +1000f);
                PropertyField(m_MessageSizeInDepth);
            }

            Space();

            PropertyExtendedSlider(m_FalloffDuration, 0.01f, 1.0f, +0.01f, 0.01f);
            PropertyField(m_Center);

            Space();

            PropertyField(m_DebugLog);

            Space();

            if (DustGUI.Button("Test Trigger"))
            {
                foreach (var subTarget in targets)
                {
                    (subTarget as TriggerGizmo).Trigger();
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_Size.isChanged)
                m_Size.valFloat = TriggerGizmo.NormalizeSize(m_Size.valFloat);

            if (m_TriggeredSize.isChanged)
                m_TriggeredSize.valFloat = TriggerGizmo.NormalizeSize(m_TriggeredSize.valFloat);

            if (m_FalloffDuration.isChanged)
                m_FalloffDuration.valFloat = TriggerGizmo.NormalizeFalloffDuration(m_FalloffDuration.valFloat);

            if (m_MessageOffset.isChanged)
                m_MessageOffset.valFloat = TriggerGizmo.NormalizeMessageOffset(m_MessageOffset.valFloat);

            if (m_MessageSize.isChanged)
                m_MessageSize.valFloat = TriggerGizmo.NormalizeMessageSize(m_MessageSize.valFloat);

            InspectorCommitUpdates();
        }
    }
}
