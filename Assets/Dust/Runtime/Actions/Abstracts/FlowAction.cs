using UnityEngine;

namespace Dust
{
    public abstract class FlowAction : Action
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
