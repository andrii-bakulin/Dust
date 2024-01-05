using UnityEngine;

namespace Dust
{
    public abstract class InstantAction : SequencedAction
    {
        protected override void ActionInnerUpdate(float deltaTime)
        {
            OnActionUpdate(deltaTime);

            ActionInnerStop(false);
        }
    }
}
