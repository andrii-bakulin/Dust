using System;
using UnityEngine;

namespace DustEngine
{
    public abstract class Action : DuMonoBehaviour
    {
        public enum TargetMode
        {
            Inherited = 0,
            Self = 1,
            Parent = 2,
            GameObject = 3,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected bool m_AutoStart = false;
        public bool autoStart
        {
            get => m_AutoStart;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_AutoStart = value;
            }
        }

        [SerializeField]
        protected TargetMode m_TargetMode = TargetMode.Inherited;
        public TargetMode targetMode
        {
            get => m_TargetMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TargetMode = value;
            }
        }

        [SerializeField]
        protected GameObject m_TargetObject = null;
        public GameObject targetObject
        {
            get => m_TargetObject;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TargetObject = value;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private GameObject m_ActiveTargetObject;
        public GameObject activeTargetObject => m_ActiveTargetObject;

        private Transform m_ActiveTargetTransform;
        public Transform activeTargetTransform => m_ActiveTargetTransform;

        protected bool m_IsPlaying;
        public bool isPlaying => m_IsPlaying;

        //--------------------------------------------------------------------------------------------------------------

        protected bool IsAllowUpdateProperty()
        {
            if (isPlaying)
            {
#if UNITY_EDITOR
                Dust.Debug.Warning("Cannot update property for action while it playing");
#endif
                return false;
            }

            return true;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            if (autoStart)
                Play();
        }

        private void Update()
        {
            if (!isPlaying)
                return;

            ActionInnerUpdate(Time.deltaTime);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Play() => Play(null);

        public void Play(Action previousAction)
        {
            if (isPlaying)
                return;

            ActionInnerStart(previousAction);
        }

        public void Stop()
        {
            if (!isPlaying)
                return;

            ActionInnerStop(true);
        }

        public void StopAllActionsAndPlay()
        {
            StopAllActions();
            Play();
        }

        public void StopAllActions()
        {
            StopAllActions(this.gameObject);
        }

        public static void StopAllActions(GameObject target)
        {
            var actions = target.GetComponents<Action>();

            foreach (var action in actions)
            {
                if (action.isPlaying)
                    action.Stop();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle INNER

        protected virtual void ActionInnerStart(Action previousAction)
        {
            if (Dust.IsNull(previousAction))
            {
                // Start 1st Action
                m_ActiveTargetObject = targetMode switch
                {
                    TargetMode.Inherited    => gameObject,
                    TargetMode.Self         => gameObject,
                    TargetMode.Parent       => Dust.IsNotNull(transform.parent) ? transform.parent.gameObject : null,
                    TargetMode.GameObject   => targetObject,
                    _                       => null
                };
            }
            else
            {
                // Continue to next Action
                m_ActiveTargetObject = targetMode switch
                {
                    TargetMode.Inherited    => previousAction.activeTargetObject,
                    TargetMode.Self         => gameObject,
                    TargetMode.Parent       => Dust.IsNotNull(transform.parent) ? transform.parent.gameObject : null,
                    TargetMode.GameObject   => targetObject,
                    _                       => null
                };
            }

            if (Dust.IsNull(activeTargetObject))
            {
                Debug.LogError("Cannot start action, because failed to detect target object");
                return;
            }
            
            m_ActiveTargetTransform = activeTargetObject.transform;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            m_IsPlaying = true;

            OnActionStart();
        }

        protected abstract void ActionInnerUpdate(float deltaTime);

        protected virtual void ActionInnerStop(bool isTerminated)
        {
            m_IsPlaying = false;

            OnActionStop(isTerminated);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected virtual void OnActionStart()
        {
            // Nothing need to do
        }

        protected abstract void OnActionUpdate(float deltaTime);

        protected virtual void OnActionStop(bool isTerminated)
        {
            // Nothing need to do
        }
    }
}
