using UnityEngine;

namespace Dust
{
    public abstract class FlowAction : Action
    {
        protected override void ActionInnerUpdate(float deltaTime)
        {
            OnActionUpdate(deltaTime);

            ActionInnerStop(false);
        }
    }
}
