using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Destroy Action")]
    public class DestroyAction : InstantAction
    {
        [SerializeField]
        private bool m_ApplyToSelf = true;
        public bool applyToSelf
        {
            get => m_ApplyToSelf;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ApplyToSelf = value;
            }
        }

        [SerializeField]
        private List<GameObject> m_GameObjects = new List<GameObject>();
        public List<GameObject> gameObjects => m_GameObjects;

        [SerializeField]
        private List<Component> m_Components = new List<Component>();
        public List<Component> components => m_Components;

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            // Cannot call DestroyNow() here, because then callbacks will be ignored
            // So require call it as a last instruction in ActionInnerStop method! 
        }

        protected override void ActionInnerStop(bool isTerminated)
        {
            base.ActionInnerStop(isTerminated);

            DestroyNow();
        }

        public void DestroyNow()
        {
            if (Dust.IsNull(activeTargetObject))
                return;

            foreach (var comp in components)
            {
                if (Dust.IsNull(comp))
                    continue;

                Destroy(comp);
            }

            foreach (var go in gameObjects)
            {
                if (Dust.IsNull(go))
                    continue;

                Destroy(go);
            }

            if (applyToSelf)
                Destroy(this.gameObject);
        }
    }
}
