using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    public abstract class SequencedAction : Action
    {
        [SerializeField]
        protected List<Action> m_OnCompleteActions = null;
        public List<Action> onCompleteActions
        {
            get
            {
                if (Dust.IsNull(m_OnCompleteActions))
                    m_OnCompleteActions = new List<Action>();

                return m_OnCompleteActions;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void ActionInnerStop(bool isTerminated)
        {
            base.ActionInnerStop(isTerminated);
            
            if (!isTerminated)
            {
                if (Dust.IsNotNull(onCompleteActions))
                {
                    foreach (var action in onCompleteActions)
                    {
                        if (Dust.IsNull(action))
                            continue;
                        
                        // Situation:
                        //      I have 1st action "Reset Transform" which starts 2 actions "Move" (2s) + "Rotate" (2s).
                        //      Then "Move" restart "Reset Transform" and repeat forever.
                        // Sometimes have situation when "Move" action finished and "Rotate" action needs
                        //      few updates more to finish.
                        //      But "Move" action start "Reset Transform" and then the last one restarts "Move"+"Rotate"
                        // In case if just call Play() for "Move" and "Rotate" then "Rotate" will not restart
                        //      and stop playing in a few Updates.
                        // Solution: need call Stop() method for all actions before I re-start them!
                        action.Stop();
                        
                        action.Play(this);
                    }
                }
            }
        }
    }
}
