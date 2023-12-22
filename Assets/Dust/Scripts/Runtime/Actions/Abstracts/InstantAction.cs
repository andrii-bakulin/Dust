namespace DustEngine
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
