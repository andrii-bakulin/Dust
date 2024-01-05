using System;
using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Factory Machines/Noise Machine")]
    public class NoiseFactoryMachine : PRSFactoryMachine
    {
        public enum AxisRemapping
        {
            Off = 0,
            XyzToXzy = 1,
            XyzToYxz = 2,
            XyzToYzx = 3,
            XyzToZxy = 4,
            XyzToZyx = 5,
        }

        public enum NoiseDimension
        {
            Noise1D = 0,
            Noise3D = 1,
        }

        public enum NoiseMode
        {
            Random = 0,
            Perlin = 1,
        }

        public enum NoiseSpace
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected float m_Min;
        public float min
        {
            get => m_Min;
            set => m_Min = value;
        }

        [SerializeField]
        protected float m_Max = 1.0f;
        public float max
        {
            get => m_Max;
            set => m_Max = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private NoiseMode m_NoiseMode = NoiseMode.Random;
        public NoiseMode noiseMode
        {
            get => m_NoiseMode;
            set => m_NoiseMode = value;
        }

        [SerializeField]
        private NoiseDimension m_NoiseDimension = NoiseDimension.Noise3D;
        public NoiseDimension noiseDimension
        {
            get => m_NoiseDimension;
            set => m_NoiseDimension = value;
        }

        [SerializeField]
        private float m_AnimationSpeed;
        public float animationSpeed
        {
            get => m_AnimationSpeed;
            set => m_AnimationSpeed = value;
        }

        [SerializeField]
        private float m_AnimationOffset;
        public float animationOffset
        {
            get => m_AnimationOffset;
            set => m_AnimationOffset = value;
        }

        [SerializeField]
        private bool m_Synchronized;
        public bool synchronized
        {
            get => m_Synchronized;
            set => m_Synchronized = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected AxisRemapping m_PositionAxisRemapping = AxisRemapping.Off;
        public AxisRemapping positionAxisRemapping
        {
            get => m_PositionAxisRemapping;
            set => m_PositionAxisRemapping = value;
        }

        [SerializeField]
        protected AxisRemapping m_RotationAxisRemapping = AxisRemapping.Off;
        public AxisRemapping rotationAxisRemapping
        {
            get => m_RotationAxisRemapping;
            set => m_RotationAxisRemapping = value;
        }

        [SerializeField]
        protected AxisRemapping m_ScaleAxisRemapping = AxisRemapping.Off;
        public AxisRemapping scaleAxisRemapping
        {
            get => m_ScaleAxisRemapping;
            set => m_ScaleAxisRemapping = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private NoiseSpace m_NoiseSpace;
        public NoiseSpace noiseSpace
        {
            get => m_NoiseSpace;
            set => m_NoiseSpace = value;
        }

        [SerializeField]
        private float m_NoiseForce = 1f;
        public float noiseForce
        {
            get => m_NoiseForce;
            set => m_NoiseForce = NormalizeNoiseForce(value);
        }

        [SerializeField]
        private float m_NoiseScale = 1f;
        public float noiseScale
        {
            get => m_NoiseScale;
            set => m_NoiseScale = NormalizeNoiseScale(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Seed = Constants.RANDOM_SEED_DEFAULT;
        public int seed
        {
            get => m_Seed;
            set
            {
                value = NormalizeSeed(value);

                if (m_Seed == value)
                    return;

                m_Seed = value;
                ResetStates();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuNoise m_DuNoisePos;
        private DuNoise m_DuNoiseRot;
        private DuNoise m_DuNoiseScl;

        private DuNoise duNoisePos => m_DuNoisePos ??= new DuNoise(seed);
        private DuNoise duNoiseRot => m_DuNoiseRot ??= new DuNoise(seed + 1235);
        private DuNoise duNoiseScl => m_DuNoiseScl ??= new DuNoise(seed - 1235);

        private float m_OffsetDynamic;

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "Noise";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            m_OffsetDynamic += Time.deltaTime * animationSpeed;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            Vector3 noisePowerPos;
            Vector3 noisePowerRot;
            Vector3 noisePowerScl;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            var factoryInstance = factoryInstanceState.instance;

            switch (noiseMode)
            {
                case NoiseMode.Random:
                    var randomVector = factoryInstance.randomVector * 100f;

                    if (noiseDimension == NoiseDimension.Noise1D)
                    {
                        noisePowerPos = DuVector3.New(duNoisePos.Perlin1D(randomVector.x));

                        if (synchronized)
                        {
                            noisePowerRot = noisePowerScl = noisePowerPos;
                        }
                        else
                        {
                            noisePowerRot = DuVector3.New(duNoiseRot.Perlin1D(randomVector.y));
                            noisePowerScl = DuVector3.New(duNoiseScl.Perlin1D(randomVector.z));
                        }
                    }
                    else // NoiseDimension.Noise3D
                    {
                        noisePowerPos = duNoisePos.Perlin1D_asVector3(randomVector.x);

                        if (synchronized)
                        {
                            noisePowerRot = noisePowerScl = noisePowerPos;
                        }
                        else
                        {
                            noisePowerRot = duNoiseRot.Perlin1D_asVector3(randomVector.y);
                            noisePowerScl = duNoiseScl.Perlin1D_asVector3(randomVector.z);
                        }
                    }
                    break;

                case NoiseMode.Perlin:
                    float animTotalOffset = m_OffsetDynamic + animationOffset;
                    Vector3 inSpaceOffset;

                    switch (noiseSpace)
                    {
                        case NoiseSpace.World:
                            inSpaceOffset = factoryInstanceState.factory.GetPositionInWorldSpace(factoryInstance);
                            break;

                        case NoiseSpace.Local:
                        default:
                            inSpaceOffset = factoryInstance.stateDynamic.position;
                            break;
                    }

                    if (DuMath.IsNotZero(noiseScale))
                        inSpaceOffset /= noiseScale;

                    if (noiseDimension == NoiseDimension.Noise1D)
                    {
                        noisePowerPos = DuVector3.New(duNoisePos.Perlin3D(inSpaceOffset, animTotalOffset));
                        noisePowerPos *= noiseForce;
                        noisePowerPos.duClamp01();

                        if (synchronized)
                        {
                            noisePowerRot = noisePowerScl = noisePowerPos;
                        }
                        else
                        {
                            noisePowerRot = DuVector3.New(duNoiseRot.Perlin3D(inSpaceOffset, animTotalOffset));
                            noisePowerRot *= noiseForce;
                            noisePowerRot.duClamp01();

                            noisePowerScl = DuVector3.New(duNoiseScl.Perlin3D(inSpaceOffset, animTotalOffset));
                            noisePowerScl *= noiseForce;
                            noisePowerScl.duClamp01();
                        }
                    }
                    else // NoiseDimension.Noise3D
                    {
                        noisePowerPos = duNoisePos.Perlin3D_asVector3(inSpaceOffset, animTotalOffset);
                        noisePowerPos *= noiseForce;
                        noisePowerPos.duClamp01();

                        if (synchronized)
                        {
                            noisePowerRot = noisePowerScl = noisePowerPos;
                        }
                        else
                        {
                            noisePowerRot = duNoiseRot.Perlin3D_asVector3(inSpaceOffset, animTotalOffset);
                            noisePowerRot *= noiseForce;
                            noisePowerRot.duClamp01();

                            noisePowerScl = duNoiseScl.Perlin3D_asVector3(inSpaceOffset, animTotalOffset);
                            noisePowerScl *= noiseForce;
                            noisePowerScl.duClamp01();
                        }
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            noisePowerPos.duFit01To(min, max);
            noisePowerRot.duFit01To(min, max);
            noisePowerScl.duFit01To(min, max);

            if (synchronized && noiseDimension == NoiseDimension.Noise3D)
            {
                if (positionAxisRemapping != AxisRemapping.Off)
                    noisePowerPos = AxisRemap(noisePowerPos, positionAxisRemapping);

                if (rotationAxisRemapping != AxisRemapping.Off)
                    noisePowerRot = AxisRemap(noisePowerRot, rotationAxisRemapping);

                if (scaleAxisRemapping != AxisRemapping.Off)
                    noisePowerScl = AxisRemap(noisePowerScl, scaleAxisRemapping);
            }

            factoryInstanceState.extraPowerEnabled = true;
            factoryInstanceState.extraPowerPosition = noisePowerPos;
            factoryInstanceState.extraPowerRotation = noisePowerRot;
            factoryInstanceState.extraPowerScale = noisePowerScl;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            UpdateInstanceDynamicState(factoryInstanceState, intensity);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Vector3 AxisRemap(Vector3 v, AxisRemapping axisRemapping)
        {
            switch (axisRemapping)
            {
                case AxisRemapping.Off: break;
                case AxisRemapping.XyzToXzy: v = new Vector3(v.x, v.z, v.y); break;
                case AxisRemapping.XyzToYxz: v = new Vector3(v.y, v.x, v.z); break;
                case AxisRemapping.XyzToYzx: v = new Vector3(v.y, v.z, v.x); break;
                case AxisRemapping.XyzToZxy: v = new Vector3(v.z, v.x, v.y); break;
                case AxisRemapping.XyzToZyx: v = new Vector3(v.z, v.y, v.x); break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return v;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, min);
            DynamicState.Append(ref dynamicState, ++seq, max);

            DynamicState.Append(ref dynamicState, ++seq, noiseMode);

            DynamicState.Append(ref dynamicState, ++seq, noiseDimension);
            DynamicState.Append(ref dynamicState, ++seq, seed);
            DynamicState.Append(ref dynamicState, ++seq, synchronized);

            if (synchronized)
            {
                DynamicState.Append(ref dynamicState, ++seq, positionAxisRemapping);
                DynamicState.Append(ref dynamicState, ++seq, rotationAxisRemapping);
                DynamicState.Append(ref dynamicState, ++seq, scaleAxisRemapping);
            }

            switch (noiseMode)
            {
                case NoiseMode.Random:
                    // Nothing need to do
                    break;

                case NoiseMode.Perlin:
                    DynamicState.Append(ref dynamicState, ++seq, noiseSpace);
                    DynamicState.Append(ref dynamicState, ++seq, noiseForce);
                    DynamicState.Append(ref dynamicState, ++seq, noiseScale);

                    DynamicState.Append(ref dynamicState, ++seq, animationSpeed);
                    DynamicState.Append(ref dynamicState, ++seq, animationOffset);

                    DynamicState.Append(ref dynamicState, ++seq, m_OffsetDynamic);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        void Reset()
        {
            ResetToDefaults();
            ResetStates();

            min = -1f;
        }

        public void ResetStates()
        {
            m_DuNoisePos = null;
            m_DuNoiseRot = null;
            m_DuNoiseScl = null;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeNoiseForce(float value)
        {
            return Mathf.Clamp(value, 0f, 10f);
        }

        public static float NormalizeNoiseScale(float value)
        {
            return Mathf.Clamp(value, 0.0001f, float.MaxValue);
        }

        public static int NormalizeSeed(int value)
        {
            return DuRandom.NormalizeSeedToNonRandom(value);
        }
    }
}
