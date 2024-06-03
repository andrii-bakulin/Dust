using UnityEngine;

namespace Dust
{
    public interface ICalcFieldValues
    {
        public float GetPowerAtPoint(Vector3 worldPosition);
        public float GetPowerAtPoint(Vector3 worldPosition, float offset);
        
        public Color GetColorAtPoint(Vector3 worldPosition);
        public Color GetColorAtPoint(Vector3 worldPosition, float offset);
        
        public Field.Result GetPowerAndColorAtPoint(Vector3 worldPosition);
        public Field.Result GetPowerAndColorAtPoint(Vector3 worldPosition, float offset);
    }
}
