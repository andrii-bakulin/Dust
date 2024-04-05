using UnityEngine;
using UnityEngine.Events;

namespace Dust.Experimental.Variables
{
    public class FloatVariable : NumericVariable
    {
        [System.Serializable]
        public class UpdateEvent : UnityEvent<float>
        {
        }
        
        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private float m_Value;
        public float value => m_Value;
        
        [SerializeField]
        private bool m_UseMinLimit;
        public bool useMinLimit => m_UseMinLimit;

        [SerializeField]
        private bool m_UseMaxLimit;
        public bool useMaxLimit => m_UseMaxLimit;

        [SerializeField]
        private float m_MinLimit;
        public float minLimit => m_MinLimit;

        [SerializeField]
        private float m_MaxLimit;
        public float maxLimit => m_MaxLimit;

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private UpdateEvent m_OnChange;
        public UpdateEvent onChange => m_OnChange;

        [SerializeField]
        private UpdateEvent m_OnHitMinLimit;
        public UpdateEvent onHitMinLimit => m_OnHitMinLimit;

        [SerializeField]
        private UpdateEvent m_OnHitMaxLimit;
        public UpdateEvent onHitMaxLimit => m_OnHitMaxLimit;

        //--------------------------------------------------------------------------------------------------------------

        public override bool Execute(Action action, string param)
        {
            if (float.TryParse(param, out float val) == false)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Cannot convert '{param}' to float value");
#endif
                return false;
            }

            return action switch
            {
                Action.Set => Set(val),
                Action.Add => Add(val),
                Action.Subtract => Subtract(val),
                Action.Multiply => Multiply(val),
                Action.Divide => Divide(val),
                _ => false
            };
       }

        public bool Set(float val)
        {
            return UpdateValue(val);
        }

        public bool Add(float val)
        {
            return UpdateValue(value + val);
        }

        public bool Subtract(float val)
        {
            return UpdateValue(value - val);
        }

        public bool Multiply(float val)
        {
            return UpdateValue(value * val);
        }

        public bool Divide(float val)
        {
            if (DuMath.IsZero(val))
                return false;
            
            return UpdateValue(value / val);
        }

        private bool UpdateValue(float newValue)
        {
            if (Mathf.Approximately(m_Value, newValue))
                return false;

            m_Value = newValue;

            if (useMinLimit && m_Value < minLimit)
                m_Value = minLimit;

            if (useMaxLimit && m_Value > maxLimit)
                m_Value = maxLimit;
            
            onChange?.Invoke(this.value);
            
            if (useMinLimit && Mathf.Approximately(m_Value, minLimit))
                onHitMinLimit?.Invoke(this.value);
            
            if (useMaxLimit && Mathf.Approximately(m_Value, maxLimit))
                onHitMaxLimit?.Invoke(this.value);
            
            return true;
        }
    }
}
