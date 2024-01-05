using System;
using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Tint Action")]
    public class TintAction : IntervalWithRollbackAction
    {
        public abstract class TintUpdater
        {
            private TintAction m_TintAction;
            protected TintAction tintAction => m_TintAction;
            
            protected Color m_StartColor;
            public Color startColor => m_StartColor;

            public virtual void Init(TintAction parentTintAction)
            {
                m_TintAction = parentTintAction;
            }

            public abstract void Update(float deltaTime, Color color);

            public virtual void Release(bool isActionTerminated)
            {
                m_TintAction = null;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public enum TintMode
        {
            Auto = 0,
            
            MeshRenderer = 1,

            UIImage = 101,
            UIText = 102,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Color m_TintColor = Color.white;
        public Color tintColor
        {
            get => m_TintColor;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TintColor = value;
            }
        }

        [SerializeField]
        private TintMode m_TintMode = TintMode.Auto;
        public TintMode tintMode
        {
            get => m_TintMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TintMode = value;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Used by: { MeshRenderer } 

        [SerializeField]
        private string m_PropertyName = "_Color";
        public string propertyName
        {
            get => m_PropertyName;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_PropertyName = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected TintUpdater m_ActiveTintUpdater;

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionStart()
        {
            base.OnActionStart();
            
            m_ActiveTintUpdater = UniversalTintUpdater(tintMode);
        }

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(m_ActiveTintUpdater))
                return;

            Color color = playingPhase switch
            {
                PlayingPhase.Idle     => m_ActiveTintUpdater.startColor,
                PlayingPhase.Main     => Color.Lerp(m_ActiveTintUpdater.startColor, tintColor, playbackState),
                PlayingPhase.Rollback => Color.Lerp(tintColor, m_ActiveTintUpdater.startColor, playbackState),
                _ => throw new ArgumentOutOfRangeException()
            };

            m_ActiveTintUpdater.Update(deltaTime, color);
        }

        protected override void OnActionStop(bool isTerminated)
        {
            if (Dust.IsNotNull(m_ActiveTintUpdater))
            {
                m_ActiveTintUpdater.Release(isTerminated);
                m_ActiveTintUpdater = null;
            }

            base.OnActionStop(isTerminated);
        }

        //--------------------------------------------------------------------------------------------------------------
        
        protected TintUpdater UniversalTintUpdater(TintMode inTintMode)
        {
            if (inTintMode == TintMode.Auto)
            {
                TintMode[] autoDetectTintsSequence =
                {
                    TintMode.MeshRenderer,

                    TintMode.UIImage,
                    TintMode.UIText,
                };
                    
                foreach (var tryTintMode in autoDetectTintsSequence)
                {
                    TintUpdater updater = UniversalTintUpdater(tryTintMode);

                    if (Dust.IsNotNull(updater))
                        return updater;
                }

                return null;
            }
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            switch (inTintMode)
            {
                case TintMode.MeshRenderer:
                    return MeshRendererTintUpdater.Create(this);

                case TintMode.UIImage:
                    return UIImageTintUpdater.Create(this);
                case TintMode.UIText:
                    return UITextTintUpdater.Create(this);
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
