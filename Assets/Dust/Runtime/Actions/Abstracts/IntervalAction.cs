using UnityEngine;

namespace Dust
{
    public abstract class IntervalAction : SequencedAction
    {
        public enum RepeatMode
        {
            PlayOnce = 0,
            Repeat = 1,
            RepeatForever = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected float m_Duration = 1f;
        public float duration
        {
            get => m_Duration;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Duration = NormalizeDuration(value);
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected RepeatMode m_RepeatMode = RepeatMode.PlayOnce;
        public RepeatMode repeatMode
        {
            get => m_RepeatMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_RepeatMode = value;
            }
        }

        [SerializeField]
        protected int m_RepeatTimes = 1;
        public int repeatTimes
        {
            get => m_RepeatTimes;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_RepeatTimes = NormalizeRepeatTimes(value);
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected int m_PlaybackIndex;
        public int playbackIndex => m_PlaybackIndex;

        protected float m_PlaybackState;
        public float playbackState => m_PlaybackState;

        protected float m_PreviousState;
        protected float previousState => m_PreviousState;

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            if (!isPlaying)
                return;

            ActionInnerUpdate(Time.deltaTime);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void ActionInnerStart(Action previousAction)
        {
            m_PlaybackIndex = 0;

            ActionPlaybackInitialize();

            base.ActionInnerStart(previousAction);

            if (IsInstantAction())
            {
                // This is instance action!
                // Required execute it right now!
                
                ActionInnerUpdate(0f);
            }
        }

        protected virtual void ActionPlaybackInitialize()
        {
            m_PreviousState = 0f;
            m_PlaybackState = 0f;
        }

        protected virtual bool IsInstantAction()
        {
            return DuMath.IsZero(duration);
        }
            
        protected virtual void ActionInnerUpdate(float deltaTime)
        {
            if (duration > 0f)
            {
                m_PreviousState = m_PlaybackState;
                m_PlaybackState = Mathf.Min(m_PlaybackState + deltaTime / duration, 1f);
            }
            else
            {
                m_PreviousState = 0f;
                m_PlaybackState = 1f;
            }

            OnActionUpdate(deltaTime);

            if (m_PlaybackState >= 1f)
                ActionPlaybackComplete();
        }

        protected virtual void ActionPlaybackComplete()
        {
            m_PlaybackIndex++;

            bool isActionFullyCompleted = repeatMode switch
            {
                RepeatMode.PlayOnce => true,
                RepeatMode.Repeat => m_PlaybackIndex >= repeatTimes,
                RepeatMode.RepeatForever => false,
                _ => true, // For undefined state -> forced finish action
            };

            if (!isActionFullyCompleted)
            {
                ActionPlaybackInitialize(); // ReInitialize and Replay
                return;
            }
            
            ActionInnerStop(false);
        }

        protected override void ActionInnerStop(bool isTerminated)
        {
            m_PlaybackIndex = 0;
            m_PreviousState = 0f;
            m_PlaybackState = 0f;

            base.ActionInnerStop(isTerminated);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Abstract methods to implement

        protected abstract void OnActionUpdate(float deltaTime);

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeDuration(float value)
        {
            return Mathf.Max(value, 0f);
        }

        public static int NormalizeRepeatTimes(int value)
        {
            return Mathf.Max(value, 1);
        }
    }
}
