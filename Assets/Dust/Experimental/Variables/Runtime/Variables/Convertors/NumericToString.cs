using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

namespace Dust.Experimental.Variables
{
    public class NumericToString : MonoBehaviour
    {
        [System.Serializable]
        public class UpdateEvent : UnityEvent<string>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private UpdateEvent m_OnConvert;
        public UpdateEvent onConvert => m_OnConvert;

        //--------------------------------------------------------------------------------------------------------------

        public void Convert(int value)
        {
            string result = value.ToString();
            onConvert?.Invoke(result);
        }

        public void Convert(float value)
        {
            string result = value.ToString(CultureInfo.InvariantCulture);
            onConvert?.Invoke(result);
        }

        public void Convert(double value)
        {
            string result = value.ToString(CultureInfo.InvariantCulture);
            onConvert?.Invoke(result);
        }
    }
}
