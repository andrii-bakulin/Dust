using UnityEngine;

namespace Dust
{
    public abstract class InstantAction : SequencedAction
    {
        protected override void ActionInnerStart(Action previousAction)
        {
            base.ActionInnerStart(previousAction);

            OnActionExecute();
            
            ActionInnerStop(false);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        // Abstract methods to implement

        protected abstract void OnActionExecute();
    }
}
