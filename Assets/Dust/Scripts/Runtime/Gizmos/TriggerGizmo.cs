using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Trigger Gizmo")]
    [ExecuteInEditMode]
    public class TriggerGizmo : AbstractGizmo
    {
        public enum MessagePosition
        {
            Top = 0,
            Bottom = 1,
            Left = 2,
            Right = 3,
            Center = 4,
        }

        public enum DebugLog
        {
            None = 0,
            Notice = 1,
            Warning = 2,
            Error = 3,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private float m_Size = 1.0f;
        public float size
        {
            get => m_Size;
            set => m_Size = NormalizeSize(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Color m_TriggeredColor = Color.red;
        public Color triggeredColor
        {
            get => m_TriggeredColor;
            set => m_TriggeredColor = value;
        }

        [SerializeField]
        private float m_TriggeredSize = 1.0f;
        public float triggeredSize
        {
            get => m_TriggeredSize;
            set => m_TriggeredSize = NormalizeSize(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ShowMessage = false;
        public bool showMessage
        {
            get => m_ShowMessage;
            set => m_ShowMessage = value;
        }

        [SerializeField]
        private bool m_HideMessageOnIdleState = false;
        public bool hideMessageOnIdleState
        {
            get => m_HideMessageOnIdleState;
            set => m_HideMessageOnIdleState = value;
        }

        [SerializeField]
        private string m_Message = "Trigger Message";
        public string message
        {
            get => m_Message;
            set => m_Message = value;
        }

        [SerializeField]
        private MessagePosition m_MessagePosition = MessagePosition.Top;
        public MessagePosition messagePosition
        {
            get => m_MessagePosition;
            set => m_MessagePosition = value;
        }

        [SerializeField]
        private float m_MessageOffset = 0.25f;
        public float messageOffset
        {
            get => m_MessageOffset;
            set => m_MessageOffset = NormalizeMessageOffset(value);
        }

        [SerializeField]
        private float m_MessageSize = 1.0f;
        public float messageSize
        {
            get => m_MessageSize;
            set => m_MessageSize = NormalizeMessageSize(value);
        }

        [SerializeField]
        private bool m_MessageSizeInDepth = false;
        public bool messageSizeInDepth
        {
            get => m_MessageSizeInDepth;
            set => m_MessageSizeInDepth = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_FalloffDuration = 0.2f;
        public float falloffDuration
        {
            get => m_FalloffDuration;
            set => m_FalloffDuration = NormalizeFalloffDuration(value);
        }

        [SerializeField]
        private Vector3 m_Center = Vector3.zero;
        public Vector3 center
        {
            get => m_Center;
            set => m_Center = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private DebugLog m_DebugLog = DebugLog.None;
        public DebugLog debugLog
        {
            get => m_DebugLog;
            set => m_DebugLog = value;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

        private float m_TriggerState;

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            if (isInEditorMode)
            {
                EditorUpdateReset();

                EditorApplication.update -= EditorUpdate;
                EditorApplication.update += EditorUpdate;
            }
        }

        private void OnDisable()
        {
            if (isInEditorMode)
            {
                EditorApplication.update -= EditorUpdate;
            }
        }

        private void EditorUpdate()
        {
            float deltaTime;

            if (!EditorUpdateTick(out deltaTime))
                return;

            UpdateState(deltaTime);
        }

        private void Update()
        {
            if (isInEditorMode) return;

            UpdateState(Time.deltaTime);
        }

        private void UpdateState(float deltaTime)
        {
            m_TriggerState = Mathf.Clamp01(m_TriggerState - deltaTime / falloffDuration);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        public void Trigger()
        {
            m_TriggerState = 1.0f;
        }

        public void Trigger(string newMessage)
        {
            m_TriggerState = 1.0f;
            m_Message = newMessage;

            switch (debugLog)
            {
                default:
                case DebugLog.None:
                    // Ignore:
                    break;

                case DebugLog.Notice:
                    Debug.Log(gameObject.name + ": " + newMessage);
                    break;

                case DebugLog.Warning:
                    Debug.LogWarning(gameObject.name + ": " + newMessage);
                    break;

                case DebugLog.Error:
                    Debug.LogError(gameObject.name + ": " + newMessage);
                    break;
            }
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public override string GizmoName()
        {
            return "Trigger";
        }

#if UNITY_EDITOR
        protected override void DrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.Lerp(color, triggeredColor, m_TriggerState);
            Gizmos.DrawSphere(center, 1.0f * Mathf.Lerp(size, triggeredSize, m_TriggerState));

            if (showMessage && !(hideMessageOnIdleState && DuMath.IsZero(m_TriggerState)))
            {
                Vector3 worldPosition = transform.TransformPoint(center);

                GUIStyle style = new GUIStyle("Label");
                style.fixedWidth = 1000;
                style.fixedHeight = 1000;

                if (messageSizeInDepth)
                    style.fontSize = Mathf.RoundToInt(style.fontSize * messageSize * 3f / HandleUtility.GetHandleSize(worldPosition));
                else
                    style.fontSize = Mathf.RoundToInt(style.fontSize * messageSize);

                if (hideMessageOnIdleState)
                    style.normal.textColor = Color.white * m_TriggerState;

                Vector3 offset = Vector3.zero;

                switch (messagePosition)
                {
                    case MessagePosition.Top:
                        offset = Camera.current.transform.up;
                        style.alignment = TextAnchor.LowerCenter;
                        break;

                    case MessagePosition.Bottom:
                        offset = -Camera.current.transform.up;
                        style.alignment = TextAnchor.UpperCenter;
                        break;

                    case MessagePosition.Left:
                        offset = -Camera.current.transform.right;
                        style.alignment = TextAnchor.MiddleRight;
                        break;

                    case MessagePosition.Right:
                        offset = Camera.current.transform.right;
                        style.alignment = TextAnchor.MiddleLeft;
                        break;

                    case MessagePosition.Center:
                        offset = Vector3.zero;
                        style.alignment = TextAnchor.MiddleCenter;
                        break;
                }

                Handles.Label(worldPosition + offset * (1f + messageOffset), message, style);
            }

            // ForcedRedrawSceneView
            SceneView.lastActiveSceneView.Repaint();
        }
#endif

        private void Reset()
        {
            color = Color.yellow;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeSize(float value)
        {
            return Mathf.Abs(value);
        }

        public static float NormalizeFalloffDuration(float value)
        {
            return Mathf.Clamp(value, 0.01f, float.MaxValue);
        }

        public static float NormalizeMessageOffset(float value)
        {
            return Mathf.Clamp(value, 0f, +1f);
        }

        public static float NormalizeMessageSize(float value)
        {
            return Mathf.Clamp(value, 0.01f, +1000f);
        }
    }
}
