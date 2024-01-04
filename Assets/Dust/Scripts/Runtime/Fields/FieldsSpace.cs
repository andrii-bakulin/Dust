using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Fields/Fields Space")]
    public class FieldsSpace : DuMonoBehaviour, ICalcFieldValues
    {
        [SerializeField]
        private FieldsMap m_FieldsMap = FieldsMap.FieldsSpace();
        public FieldsMap fieldsMap => m_FieldsMap;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        // @notice: I create point only once and will be use it for any future calculations
        private Field.Point m_CalcFieldPoint = new Field.Point();

        //--------------------------------------------------------------------------------------------------------------

        public float GetPowerAtPoint(Vector3 worldPosition)
            => GetPowerAtPoint(worldPosition, 0f);

        public float GetPowerAtPoint(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = offset;
            m_CalcFieldPoint.inFactoryInstanceState = null;

            fieldsMap.Calculate(m_CalcFieldPoint);

            return m_CalcFieldPoint.result.power;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Color GetColorAtPoint(Vector3 worldPosition)
            => GetColorAtPoint(worldPosition, 0f);

        public Color GetColorAtPoint(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = offset;
            m_CalcFieldPoint.inFactoryInstanceState = null;

            fieldsMap.Calculate(m_CalcFieldPoint);

            return m_CalcFieldPoint.result.color;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Field.Result GetPowerAndColorAtPoint(Vector3 worldPosition)
            => GetPowerAndColorAtPoint(worldPosition, 0f);

        public Field.Result GetPowerAndColorAtPoint(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = offset;
            m_CalcFieldPoint.inFactoryInstanceState = null;

            fieldsMap.Calculate(m_CalcFieldPoint);

            return m_CalcFieldPoint.result;
        }
    }
}
