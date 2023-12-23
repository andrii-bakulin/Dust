using System;
using UnityEngine;

namespace DustEngine
{
    public abstract partial class Field
    {
        // @notice: I create point only once and will be use it for any future calculations
        private Field.Point m_CalcFieldPoint = new Field.Point();

        //--------------------------------------------------------------------------------------------------------------

        public float GetPower(Vector3 worldPosition)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = 0f;
#if DUST_NEW_FEATURE_FACTORY
            m_CalcFieldPoint.inFactoryInstanceState = null;
#endif

            this.Calculate(m_CalcFieldPoint, out var result, false);

            return result.fieldPower;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Color GetColor(Vector3 worldPosition)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = 0f;
#if DUST_NEW_FEATURE_FACTORY
            m_CalcFieldPoint.inFactoryInstanceState = null;
#endif

            this.Calculate(m_CalcFieldPoint, out var result, true);

            return result.fieldColor;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public float GetPowerAndColor(Vector3 worldPosition, out Color color)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = 0f;
#if DUST_NEW_FEATURE_FACTORY
            m_CalcFieldPoint.inFactoryInstanceState = null;
#endif

            this.Calculate(m_CalcFieldPoint, out var result, true);

            color = result.fieldColor;
            return result.fieldPower;
        }
    }
}