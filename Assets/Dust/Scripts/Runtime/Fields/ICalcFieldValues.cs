using UnityEngine;

namespace DustEngine
{
    public interface ICalcFieldValues
    {
        public float GetPower(Vector3 worldPosition);
        public float GetPower(Vector3 worldPosition, float offset);
        
        public Color GetColor(Vector3 worldPosition);
        public Color GetColor(Vector3 worldPosition, float offset);
        
        public float GetPowerAndColor(Vector3 worldPosition, out Color color);
        public float GetPowerAndColor(Vector3 worldPosition, float offset, out Color color);

        // public Field.Result GetPowerAndColor(Vector3 worldPosition);
    }
}
