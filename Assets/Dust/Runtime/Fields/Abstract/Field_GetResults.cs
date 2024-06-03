using UnityEngine;

namespace Dust
{
    public abstract partial class Field : ICalcFieldValues
    {
        // @notice: I create point only once and will be use it for any future calculations
        private Field.Point m_CalcFieldPoint = new Field.Point();

        //--------------------------------------------------------------------------------------------------------------

        public float GetPowerAtPoint(Vector3 worldPosition)
            => GetPowerAtPoint(worldPosition, 0f);

        public float GetPowerAtPoint(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = 0f;
            m_CalcFieldPoint.inFactoryInstanceState = null;

            Calculate(m_CalcFieldPoint, out var result, false);

            return result.power;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Color GetColorAtPoint(Vector3 worldPosition)
            => GetColorAtPoint(worldPosition, 0f);

        public Color GetColorAtPoint(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = 0f;
            m_CalcFieldPoint.inFactoryInstanceState = null;

            Calculate(m_CalcFieldPoint, out var result, true);

            return result.color;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Field.Result GetPowerAndColorAtPoint(Vector3 worldPosition)
            => GetPowerAndColorAtPoint(worldPosition, 0f);

        public Field.Result GetPowerAndColorAtPoint(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = 0f;
            m_CalcFieldPoint.inFactoryInstanceState = null;

            Calculate(m_CalcFieldPoint, out var result, true);

            return result;
        }
    }
}
