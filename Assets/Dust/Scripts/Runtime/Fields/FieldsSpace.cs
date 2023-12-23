using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Fields Space")]
    public class FieldsSpace : DuMonoBehaviour
    {
        [SerializeField]
        private FieldsMap m_FieldsMap = FieldsMap.FieldsSpace();
        public FieldsMap fieldsMap => m_FieldsMap;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        // @notice: I create point only once and will be use it for any future calculations
        private Field.Point m_CalcFieldPoint = new Field.Point();

        //--------------------------------------------------------------------------------------------------------------

        public float GetPower(Vector3 worldPosition)
            => GetPower(worldPosition, 0f);

        public float GetPower(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = offset;

#if DUST_NEW_FEATURE_FACTORY
            m_CalcFieldPoint.inFactoryInstanceState = null;
#endif

            fieldsMap.Calculate(m_CalcFieldPoint);

            return m_CalcFieldPoint.endPower;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Color GetColor(Vector3 worldPosition)
            => GetColor(worldPosition, 0f);

        public Color GetColor(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = offset;

#if DUST_NEW_FEATURE_FACTORY
            m_CalcFieldPoint.inFactoryInstanceState = null;
#endif

            fieldsMap.Calculate(m_CalcFieldPoint);

            return m_CalcFieldPoint.endColor;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public float GetPowerAndColor(Vector3 worldPosition, out Color color)
            => GetPowerAndColor(worldPosition, 0f, out color);

        public float GetPowerAndColor(Vector3 worldPosition, float offset, out Color color)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = offset;

#if DUST_NEW_FEATURE_FACTORY
            m_CalcFieldPoint.inFactoryInstanceState = null;
#endif

            fieldsMap.Calculate(m_CalcFieldPoint);

            color = m_CalcFieldPoint.endColor;
            return m_CalcFieldPoint.endPower;
        }
    }
}
