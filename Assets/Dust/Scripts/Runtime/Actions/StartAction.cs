using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Start Action")]
    public class StartAction : InstantAction
    {
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
