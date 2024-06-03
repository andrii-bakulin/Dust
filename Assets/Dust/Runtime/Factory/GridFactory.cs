using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Factory/Grid Factory")]
    [ExecuteInEditMode]
    public class GridFactory : Factory
    {
        public enum OffsetDirection
        {
            Disabled = 0,
            X = 1,
            Y = 2,
            Z = 3,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3Int m_Count = new Vector3Int(5, 1, 5);
        public Vector3Int count
        {
            get => m_Count;
            set
            {
                if (!UpdatePropertyValue(ref m_Count, NormalizeCount(value)))
                    return;

                RebuildInstances();
            }
        }

        [SerializeField]
        private Vector3 m_Step = new Vector3(2f, 2f, 2f);
        public Vector3 step
        {
            get => m_Step;
            set
            {
                if (!UpdatePropertyValue(ref m_Step, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private OffsetDirection m_OffsetDirection = OffsetDirection.Disabled;
        public OffsetDirection offsetDirection
        {
            get => m_OffsetDirection;
            set
            {
                if (m_OffsetDirection == value)
                    return;

                m_OffsetDirection = value;
                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private float m_OffsetWidth;
        public float offsetWidth
        {
            get => m_OffsetWidth;
            set
            {
                if (!UpdatePropertyValue(ref m_OffsetWidth, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private float m_OffsetHeight;
        public float offsetHeight
        {
            get => m_OffsetHeight;
            set
            {
                if (!UpdatePropertyValue(ref m_OffsetHeight, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryName()
        {
            return "Grid";
        }

        internal override FactoryBuilder GetFactoryBuilder()
        {
            return new FactoryGridBuilder();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static Vector3Int NormalizeCount(Vector3Int value)
        {
            return DuVector3Int.Clamp(value, Vector3Int.one, Vector3Int.one * 1000);
        }
    }
}
