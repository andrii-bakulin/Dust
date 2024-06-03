using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Factory/Radial Factory")]
    [ExecuteInEditMode]
    public class RadialFactory : Factory
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
        private float m_Radius = 1.0f;
        public float radius
        {
            get => m_Radius;
            set
            {
                if (!UpdatePropertyValue(ref m_Radius, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private Orientation m_Orientation = Orientation.XZ;
        public Orientation orientation
        {
            get => m_Orientation;
            set
            {
                if (m_Orientation == value)
                    return;

                m_Orientation = value;
                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private bool m_Align = true;
        public bool align
        {
            get => m_Align;
            set
            {
                if (!UpdatePropertyValue(ref m_Align, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_LevelsCount = 1;
        public int levelsCount
        {
            get => m_LevelsCount;
            set
            {
                if (!UpdatePropertyValue(ref m_LevelsCount, NormalizeLevelsCount(value)))
                    return;

                RebuildInstances();
            }
        }

        [SerializeField]
        private float m_LevelRadiusOffset = 2.0f;
        public float levelRadiusOffset
        {
            get => m_LevelRadiusOffset;
            set
            {
                if (!UpdatePropertyValue(ref m_LevelRadiusOffset, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private int m_DeltaCountPerLevel;
        public int deltaCountPerLevel
        {
            get => m_DeltaCountPerLevel;
            set
            {
                if (!UpdatePropertyValue(ref m_DeltaCountPerLevel, value))
                    return;

                RebuildInstances();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_StartAngle;
        public float startAngle
        {
            get => m_StartAngle;
            set
            {
                if (!UpdatePropertyValue(ref m_StartAngle, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private float m_EndAngle = 360f;
        public float endAngle
        {
            get => m_EndAngle;
            set
            {
                if (!UpdatePropertyValue(ref m_EndAngle, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_Offset;
        public float offset
        {
            get => m_Offset;
            set
            {
                if (!UpdatePropertyValue(ref m_Offset, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private float m_OffsetVariation;
        public float offsetVariation
        {
            get => m_OffsetVariation;
            set
            {
                if (!UpdatePropertyValue(ref m_OffsetVariation, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private int m_OffsetSeed = Constants.RANDOM_SEED_DEFAULT;
        public int offsetSeed
        {
            get => m_OffsetSeed;
            set
            {
                value = DuRandom.NormalizeSeedToNonRandom(value);

                if (!UpdatePropertyValue(ref m_OffsetSeed, NormalizeOffsetSeed(value)))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryName()
        {
            return "Radial";
        }

        internal override FactoryBuilder GetFactoryBuilder()
        {
            return new FactoryRadialBuilder();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static int NormalizeCount(int value)
        {
            return Mathf.Max(0, value);
        }

        public static int NormalizeOffsetSeed(int value)
        {
            return DuRandom.NormalizeSeedToNonRandom(value);
        }

        public static int NormalizeLevelsCount(int value)
        {
            return Mathf.Max(1, value);
        }
    }
}
