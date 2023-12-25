using System;
using UnityEngine;

namespace DustEngine
{
    public abstract partial class Field : ICalcFieldValues
    {
        // @notice: I create point only once and will be use it for any future calculations
        private Field.Point m_CalcFieldPoint = new Field.Point();

        //--------------------------------------------------------------------------------------------------------------

        public float GetPower(Vector3 worldPosition)
            => GetPower(worldPosition, 0f);

        public float GetPower(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = 0f;
            m_CalcFieldPoint.inFactoryInstanceState = null;

            Calculate(m_CalcFieldPoint, out var result, false);

            return result.power;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Color GetColor(Vector3 worldPosition)
            => GetColor(worldPosition, 0f);

        public Color GetColor(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = 0f;
            m_CalcFieldPoint.inFactoryInstanceState = null;

            Calculate(m_CalcFieldPoint, out var result, true);

            return result.color;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Field.Result GetPowerAndColor(Vector3 worldPosition)
            => GetPowerAndColor(worldPosition, 0f);

        public Field.Result GetPowerAndColor(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = 0f;
            m_CalcFieldPoint.inFactoryInstanceState = null;

            Calculate(m_CalcFieldPoint, out var result, true);

            return result;
        }
    }
}
