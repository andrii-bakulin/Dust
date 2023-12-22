using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Activate Action")]
    public class ActivateAction : InstantAction
    {
        public enum ActivationMode
        {
            Disable = 0,
            Enable = 1,
            Toggle = 2,
            ToggleRandom = 3,
        }
        
        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private ActivationMode m_ActivationMode = ActivationMode.Enable;
        public ActivationMode activationMode
        {
            get => m_ActivationMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ActivationMode = value;
            }
        }

        [SerializeField]
        private bool m_ApplyToSelf = false;
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
        private int m_Seed = 0;
        public int seed
        {
            get => m_Seed;
            set
            {
                if (!IsAllowUpdateProperty()) return;

                if (m_Seed == value)
                    return;

                m_Seed = value;
                m_DuRandom = null;
            }
        }

        [SerializeField]
        private List<GameObject> m_GameObjects = new List<GameObject>();
        public List<GameObject> gameObjects => m_GameObjects;

        [SerializeField]
        private List<Component> m_Components = new List<Component>();
        public List<Component> components => m_Components;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuRandom m_DuRandom;
        private DuRandom duRandom => m_DuRandom ??= new DuRandom(seed);

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(activeTargetTransform))
                return;
            
            if (applyToSelf)
                gameObject.SetActive(GetNewState(gameObject.activeSelf));

            foreach (var go in gameObjects)
            {
                if (Dust.IsNull(go))
                    continue;
                
                go.SetActive(GetNewState(go.activeSelf));
            }

            foreach (var comp in components)
            {
                if (Dust.IsNull(comp))
                    continue;

                if (comp as Behaviour is Behaviour compBehaviour)
                {
                    compBehaviour.enabled = GetNewState(compBehaviour.enabled);
                }
                else if (comp as Collider is Collider compCollider)
                {
                    compCollider.enabled = GetNewState(compCollider.enabled);
                }
                else if (comp as Renderer is Renderer compRenderer)
                {
                    compRenderer.enabled = GetNewState(compRenderer.enabled);
                }
                else if (comp as Cloth is Cloth compCloth)
                {
                    compCloth.enabled = GetNewState(compCloth.enabled);
                }
                else if (comp as LODGroup is LODGroup compLODGroup)
                {
                    compLODGroup.enabled = GetNewState(compLODGroup.enabled);
                }
            }
        }

        protected bool GetNewState(bool currentState)
        {
            return activationMode switch
            {
                ActivationMode.Disable      => false,
                ActivationMode.Enable       => true,
                ActivationMode.Toggle       => !currentState,
                ActivationMode.ToggleRandom => duRandom.Next() >= 0.5f,
                _                           => currentState
            };
        }
    }
}
