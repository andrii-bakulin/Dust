using UnityEngine;

namespace Dust
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
        protected bool m_AutoStart;
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
        protected GameObject m_TargetObject;
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

        //--------------------------------------------------------------------------------------------------------------

        public void Play() => Play(null);

        internal void Play(Action previousAction)
        {
            if (this.enabled == false)
                return; // if component not enabled -> don't start action

            if (isPlaying)
                return; // no need restart playing action

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

            if (Dust.IsNull(m_ActiveTargetObject))
            {
                Debug.LogError("Cannot start action, because failed to detect target object");
                return;
            }
            
            m_ActiveTargetTransform = m_ActiveTargetObject.transform;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            m_IsPlaying = true;

            OnActionStart();
        }

        protected virtual void ActionInnerStop(bool isTerminated)
        {
            m_IsPlaying = false;

            OnActionStop(isTerminated);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle for custom logic of each actions

        protected virtual void OnActionStart()
        {
            // Nothing need to do
        }

        protected virtual void OnActionStop(bool isTerminated)
        {
            // Nothing need to do
        }
    }
}
