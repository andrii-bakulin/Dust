using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Start Action")]
    public class StartAction : InstantAction
    {
        public static bool Play(GameObject source)
        {
            if (Dust.IsNull(source))
                return false;

            var startAction = source.GetComponent<StartAction>();

            if (Dust.IsNull(startAction))
                return false;

            startAction.Play();
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionExecute()
        {
            // Nothing need to do...
        }

        private void Reset()
        {
            m_AutoStart = true;
        }
    }
}
