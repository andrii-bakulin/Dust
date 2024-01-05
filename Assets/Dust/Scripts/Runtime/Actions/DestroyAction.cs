﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Destroy Action")]
    public class DestroyAction : InstantAction
    {
        [SerializeField]
        private bool m_ApplyToTarget = true;
        public bool applyToTarget
        {
            get => m_ApplyToTarget;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ApplyToTarget = value;
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
            foreach (var comp in components.Where(comp => Dust.IsNotNull(comp)))
            {
                Destroy(comp);
            }

            foreach (var go in gameObjects.Where(go => Dust.IsNotNull(go)))
            {
                Destroy(go);
            }

            if (applyToTarget && Dust.IsNotNull(activeTargetObject))
                Destroy(activeTargetObject);
        }
    }
}
