namespace DustEngine
{
    public abstract class InstantAction : ActionWithCallbacks
    {
        protected override void ActionInnerUpdate(float deltaTime)
        {
            OnActionUpdate(deltaTime);

            ActionInnerStop(false);
        }
    }
}
