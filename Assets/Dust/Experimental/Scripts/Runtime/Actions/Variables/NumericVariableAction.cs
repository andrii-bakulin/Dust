using Dust.Experimental.Variables;
using UnityEngine;

namespace Dust.Experimental
{
    [AddComponentMenu("Dust/* Experimental/Actions/Numeric Variable")]
    public class NumericVariableAction : VariableAction
    {
        [SerializeField]
        private string m_VariableName;
        public string variableName
        {
            get => m_VariableName;
            set => m_VariableName = value;
        }
        
        [SerializeField]
        private NumericVariable.Action m_Action;
        public NumericVariable.Action action
        {
            get => m_Action;
            set => m_Action = value;
        }
        
        [SerializeField]
        private string m_Value;
        public string value
        {
            get => m_Value;
            set => m_Value = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionExecute()
        {
            AbstractVariablesManager manager = GetManager(this.scope);

            if (Dust.IsNull(manager))
            {
#if UNITY_EDITOR
                Debug.LogError($"Cannot find manager with scope `{scope}`");
#endif
                return;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            NumericVariable variable = manager.FindVariableByName<NumericVariable>(variableName);

            if (Dust.IsNull(variable))
            {
#if UNITY_EDITOR
                Debug.LogError($"Cannot find variable `{variableName}` in `{scope}` manager");
#endif
                return;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            variable.Execute(action, value);
        }
    }
}
