using Dust.Experimental.Variables;
using UnityEngine;

namespace Dust.Experimental
{
    public abstract class VariableAction : InstantAction
    {
        public enum Scope
        {
            Self = 0,
            Object = 1,
            Scene = 10,
            // App,
            // Saved,
        }

        [SerializeField]
        protected Scope m_Scope;
        public Scope scope
        {
            get => m_Scope;
            set => m_Scope = value;
        }

        [SerializeField]
        protected GameObject m_ReferenceObject;
        public GameObject referenceObject
        {
            get => m_ReferenceObject;
            set => m_ReferenceObject = value;
        }

        public AbstractVariablesManager GetManager(Scope getInScope)
        {
            return getInScope switch
            {
                Scope.Self => this.gameObject.GetComponent<ObjectVariablesManager>(),
                Scope.Object => Dust.IsNotNull(referenceObject) ? referenceObject.GetComponent<ObjectVariablesManager>() : null,
                Scope.Scene => GameObject.FindObjectOfType<SceneVariablesManager>(),
                _ => null
            };
        }
    }
}
