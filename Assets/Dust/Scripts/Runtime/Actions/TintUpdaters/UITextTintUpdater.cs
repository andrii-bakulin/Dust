using UnityEngine;
using UnityEngine.UI;

namespace DustEngine
{
    public class UITextTintUpdater : TintAction.TintUpdater
    {
        protected Text m_Target;

        //----------------------------------------------------------------------------------------------------------

        public static TintAction.TintUpdater Create(TintAction parentTintAction)
        {
            var target = parentTintAction.activeTargetObject.GetComponent<Text>();

            if (Dust.IsNull(target))
                return null;

            var tintUpdater = new UITextTintUpdater();
            tintUpdater.m_Target = target;
            tintUpdater.Init(parentTintAction);
            return tintUpdater;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override void Init(TintAction parentTintAction)
        {
            base.Init(parentTintAction);
                
            m_StartColor = m_Target.color;
        }

        public override void Update(float deltaTime, Color color)
        {
            m_Target.color = color;
        }

        public override void Release(bool isActionTerminated)
        {
            m_Target = null;

            base.Release(isActionTerminated);
        }
    }
}
