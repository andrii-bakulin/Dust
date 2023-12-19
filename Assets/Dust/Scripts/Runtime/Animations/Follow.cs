using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Animations/Follow")]
    [ExecuteInEditMode]
    public class Follow : DuMonoBehaviour
    {
        public enum SpeedMode
        {
            Unlimited = 0,
            Limited = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private GameObject m_FollowObject = null;
        public GameObject followObject
        {
            get => m_FollowObject;
            set => m_FollowObject = value;
        }

        [SerializeField]
        private Vector3 m_FollowOffset = Vector3.zero;
        public Vector3 followOffset
        {
            get => m_FollowOffset;
            set => m_FollowOffset = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private SpeedMode m_SpeedMode = SpeedMode.Unlimited;
        public SpeedMode speedMode
        {
            get => m_SpeedMode;
            set => m_SpeedMode = value;
        }

        [SerializeField]
        private float m_SpeedLimit = 1f;
        public float speedLimit
        {
            get => m_SpeedLimit;
            set => m_SpeedLimit = NormalizeSpeedLimit(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_UseSmoothDamp = false;
        public bool useSmoothDamp
        {
            get => m_UseSmoothDamp;
            set => m_UseSmoothDamp = value;
        }

        [SerializeField]
        private Vector3 m_SmoothTime = Vector3.one;
        public Vector3 smoothTime
        {
            get => m_SmoothTime;
            set => m_SmoothTime = NormalizeSmoothTime(value);
        }

        [SerializeField]
        private UpdateMode m_UpdateMode = UpdateMode.Update;
        public UpdateMode updateMode
        {
            get => m_UpdateMode;
            set => m_UpdateMode = value;
        }

        [SerializeField]
        private bool m_UpdateInEditor = true;
        public bool updateInEditor
        {
            get => m_UpdateInEditor;
            set => m_UpdateInEditor = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private Vector3 m_SmoothVelocity;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        void OnEnable()
        {
            if (isInEditorMode)
            {
                EditorUpdateReset();

                EditorApplication.update -= EditorUpdate;
                EditorApplication.update += EditorUpdate;
            }
        }

        void OnDisable()
        {
            if (isInEditorMode)
            {
                EditorApplication.update -= EditorUpdate;
            }
        }
#endif

        void Update()
        {
#if UNITY_EDITOR
            if (isInEditorMode && !updateInEditor) return;
#endif

            if (updateMode != UpdateMode.Update)
                return;

            UpdateState(Time.deltaTime);
        }

        void LateUpdate()
        {
#if UNITY_EDITOR
            if (isInEditorMode && !updateInEditor) return;
#endif

            if (updateMode != UpdateMode.LateUpdate)
                return;

            UpdateState(Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (updateMode != UpdateMode.FixedUpdate)
                return;

            UpdateState(Time.fixedDeltaTime);
        }

#if UNITY_EDITOR
        private void EditorUpdate()
        {
            if (!EditorUpdateTick(out var deltaTime))
                return;

#if UNITY_EDITOR
            if (!updateInEditor) return;
#endif

            UpdateState(deltaTime);
        }
#endif

        private void UpdateState(float deltaTime)
        {
            if (Dust.IsNull(followObject) || followObject == this.gameObject)
                return;

            Vector3 newPosition = followObject.transform.position + followOffset;

            if (useSmoothDamp)
            {
                float maxSpeed = speedMode == SpeedMode.Limited ? speedLimit : Mathf.Infinity;

                newPosition = DuVector3.SmoothDamp(transform.position, newPosition, ref m_SmoothVelocity, smoothTime, maxSpeed, deltaTime);
            }
            else
            {
                if (speedMode == SpeedMode.Limited)
                {
                    Vector3 deltaMove = newPosition - transform.position;
                    float speedInFrame = speedLimit * deltaTime;

                    if (deltaMove.sqrMagnitude > speedInFrame * speedInFrame)
                        deltaMove = deltaMove.normalized * (speedLimit * deltaTime);

                    newPosition = transform.position + deltaMove;
                }
                // else -> for Unlimited -> just move to required newPosition
            }

            transform.position = newPosition;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeSpeedLimit(float value)
        {
            return Mathf.Max(value, 0f);
        }

        public static Vector3 NormalizeSmoothTime(Vector3 value)
        {
            return Vector3.Max(DuVector3.New(0.01f), value);
        }
    }
}
