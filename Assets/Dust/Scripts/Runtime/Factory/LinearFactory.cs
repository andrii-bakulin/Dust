using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Factory/Linear Factory")]
    [ExecuteInEditMode]
    public class LinearFactory : Factory
    {
        [SerializeField]
        private int m_Count = 5;
        public int count
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
        private int m_Offset;
        public int offset
        {
            get => m_Offset;
            set
            {
                if (!UpdatePropertyValue(ref m_Offset, NormalizeOffset(value)))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private float m_Amount = 1f;
        public float amount
        {
            get => m_Amount;
            set
            {
                if (!UpdatePropertyValue(ref m_Amount, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private Vector3 m_Position = Vector3.right * 5f;
        public Vector3 position
        {
            get => m_Position;
            set
            {
                if (!UpdatePropertyValue(ref m_Position, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private Vector3 m_Rotation = Vector3.zero;
        public Vector3 rotation
        {
            get => m_Rotation;
            set
            {
                if (!UpdatePropertyValue(ref m_Rotation, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private Vector3 m_Scale = Vector3.one;
        public Vector3 scale
        {
            get => m_Scale;
            set
            {
                if (!UpdatePropertyValue(ref m_Scale, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private Vector3 m_StepRotation = Vector3.zero;
        public Vector3 stepRotation
        {
            get => m_StepRotation;
            set
            {
                if (!UpdatePropertyValue(ref m_StepRotation, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryName()
        {
            return "Linear";
        }

        internal override FactoryBuilder GetFactoryBuilder()
        {
            return new FactoryLinearBuilder();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static int NormalizeCount(int value)
        {
            return Mathf.Max(0, value);
        }

        public static int NormalizeOffset(int value)
        {
            return Mathf.Max(0, value);
        }
    }
}
