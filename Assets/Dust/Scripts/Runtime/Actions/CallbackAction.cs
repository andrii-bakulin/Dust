using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Callback Action")]
    public class CallbackAction : InstantAction
    {
        [Serializable]
        public class ActionCallback : UnityEvent<Action>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected ActionCallback m_OnCompleteCallback;
        public ActionCallback onCompleteCallback => m_OnCompleteCallback;

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            onCompleteCallback?.Invoke(this);
        }
    }
}
