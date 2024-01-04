using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Factory Machines/Clamp Machine")]
    public class ClampFactoryMachine : BasicFactoryMachine
    {
        [SerializeField]
        private ClampMode m_PositionMode = ClampMode.NoClamp;
        public ClampMode positionMode
        {
            get => m_PositionMode;
            set => m_PositionMode = value;
        }

        [SerializeField]
        private Vector3 m_PositionMin = Vector3.zero;
        public Vector3 positionMin
        {
            get => m_PositionMin;
            set => m_PositionMin = value;
        }

        [SerializeField]
        private Vector3 m_PositionMax = Vector3.one;
        public Vector3 positionMax
        {
            get => m_PositionMax;
            set => m_PositionMax = value;
        }

        [SerializeField]
        private bool m_PositionClampX = true;
        public bool positionClampX
        {
            get => m_PositionClampX;
            set => m_PositionClampX = value;
        }

        [SerializeField]
        private bool m_PositionClampY = true;
        public bool positionClampY
        {
            get => m_PositionClampY;
            set => m_PositionClampY = value;
        }

        [SerializeField]
        private bool m_PositionClampZ = true;
        public bool positionClampZ
        {
            get => m_PositionClampZ;
            set => m_PositionClampZ = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private ClampMode m_RotationMode = ClampMode.NoClamp;
        public ClampMode rotationMode
        {
            get => m_RotationMode;
            set => m_RotationMode = value;
        }

        [SerializeField]
        private Vector3 m_RotationMin = Vector3.one * -90f;
        public Vector3 rotationMin
        {
            get => m_RotationMin;
            set => m_RotationMin = value;
        }

        [SerializeField]
        private Vector3 m_RotationMax = Vector3.one * +90f;
        public Vector3 rotationMax
        {
            get => m_RotationMax;
            set => m_RotationMax = value;
        }

        [SerializeField]
        private bool m_RotationClampX = true;
        public bool rotationClampX
        {
            get => m_RotationClampX;
            set => m_RotationClampX = value;
        }

        [SerializeField]
        private bool m_RotationClampY = true;
        public bool rotationClampY
        {
            get => m_RotationClampY;
            set => m_RotationClampY = value;
        }

        [SerializeField]
        private bool m_RotationClampZ = true;
        public bool rotationClampZ
        {
            get => m_RotationClampZ;
            set => m_RotationClampZ = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private ClampMode m_ScaleMode = ClampMode.NoClamp;
        public ClampMode scaleMode
        {
            get => m_ScaleMode;
            set => m_ScaleMode = value;
        }

        [SerializeField]
        private Vector3 m_ScaleMin = Vector3.zero;
        public Vector3 scaleMin
        {
            get => m_ScaleMin;
            set => m_ScaleMin = value;
        }

        [SerializeField]
        private Vector3 m_ScaleMax = Vector3.one;
        public Vector3 scaleMax
        {
            get => m_ScaleMax;
            set => m_ScaleMax = value;
        }

        [SerializeField]
        private bool m_ScaleClampX = true;
        public bool scaleClampX
        {
            get => m_ScaleClampX;
            set => m_ScaleClampX = value;
        }

        [SerializeField]
        private bool m_ScaleClampY = true;
        public bool scaleClampY
        {
            get => m_ScaleClampY;
            set => m_ScaleClampY = value;
        }

        [SerializeField]
        private bool m_ScaleClampZ = true;
        public bool scaleClampZ
        {
            get => m_ScaleClampZ;
            set => m_ScaleClampZ = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "Clamp";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            float intensityByMachine = intensity;

            UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            var instanceState = factoryInstanceState.instance.stateDynamic;

            // @notice: here fieldPower also involve to transferPower (not like in PRS-Factory)
            float transferPower = factoryInstanceState.intensityByFactory
                                  * factoryInstanceState.intensityByMachine
                                  * factoryInstanceState.fieldPower;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Position

            if (positionMode != ClampMode.NoClamp && (positionClampX || positionClampY || positionClampZ))
            {
                Vector3 endPosition = instanceState.position;

                if (positionMode == ClampMode.MinOnly || positionMode == ClampMode.MinAndMax)
                {
                    if (positionClampX) endPosition.x = Mathf.Max(endPosition.x, positionMin.x);
                    if (positionClampY) endPosition.y = Mathf.Max(endPosition.y, positionMin.y);
                    if (positionClampZ) endPosition.z = Mathf.Max(endPosition.z, positionMin.z);
                }

                if (positionMode == ClampMode.MaxOnly || positionMode == ClampMode.MinAndMax)
                {
                    if (positionClampX) endPosition.x = Mathf.Min(endPosition.x, positionMax.x);
                    if (positionClampY) endPosition.y = Mathf.Min(endPosition.y, positionMax.y);
                    if (positionClampZ) endPosition.z = Mathf.Min(endPosition.z, positionMax.z);
                }

                instanceState.position = Vector3.LerpUnclamped(instanceState.position, endPosition, transferPower);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Rotation

            if (rotationMode != ClampMode.NoClamp && (rotationClampX || rotationClampY || rotationClampZ))
            {
                Vector3 endRotation = instanceState.rotation;

                if (rotationMode == ClampMode.MinOnly || rotationMode == ClampMode.MinAndMax)
                {
                    if (rotationClampX) endRotation.x = Mathf.Max(endRotation.x, rotationMin.x);
                    if (rotationClampY) endRotation.y = Mathf.Max(endRotation.y, rotationMin.y);
                    if (rotationClampZ) endRotation.z = Mathf.Max(endRotation.z, rotationMin.z);
                }

                if (rotationMode == ClampMode.MaxOnly || rotationMode == ClampMode.MinAndMax)
                {
                    if (rotationClampX) endRotation.x = Mathf.Min(endRotation.x, rotationMax.x);
                    if (rotationClampY) endRotation.y = Mathf.Min(endRotation.y, rotationMax.y);
                    if (rotationClampZ) endRotation.z = Mathf.Min(endRotation.z, rotationMax.z);
                }

                instanceState.rotation = Vector3.LerpUnclamped(instanceState.rotation, endRotation, transferPower);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Scale

            if (scaleMode != ClampMode.NoClamp && (scaleClampX || scaleClampY || scaleClampZ))
            {
                Vector3 endScale = instanceState.scale;

                if (scaleMode == ClampMode.MinOnly || scaleMode == ClampMode.MinAndMax)
                {
                    if (scaleClampX) endScale.x = Mathf.Max(endScale.x, scaleMin.x);
                    if (scaleClampY) endScale.y = Mathf.Max(endScale.y, scaleMin.y);
                    if (scaleClampZ) endScale.z = Mathf.Max(endScale.z, scaleMin.z);
                }

                if (scaleMode == ClampMode.MaxOnly || scaleMode == ClampMode.MinAndMax)
                {
                    if (scaleClampX) endScale.x = Mathf.Min(endScale.x, scaleMax.x);
                    if (scaleClampY) endScale.y = Mathf.Min(endScale.y, scaleMax.y);
                    if (scaleClampZ) endScale.z = Mathf.Min(endScale.z, scaleMax.z);
                }

                instanceState.scale = Vector3.LerpUnclamped(instanceState.scale, endScale, transferPower);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, positionMode);
            if (positionMode != ClampMode.NoClamp)
            {
                DynamicState.Append(ref dynamicState, ++seq, positionMin);
                DynamicState.Append(ref dynamicState, ++seq, positionMax);
                DynamicState.Append(ref dynamicState, ++seq, positionClampX);
                DynamicState.Append(ref dynamicState, ++seq, positionClampY);
                DynamicState.Append(ref dynamicState, ++seq, positionClampZ);
            }

            DynamicState.Append(ref dynamicState, ++seq, rotationMode);
            if (rotationMode != ClampMode.NoClamp)
            {
                DynamicState.Append(ref dynamicState, ++seq, rotationMin);
                DynamicState.Append(ref dynamicState, ++seq, rotationMax);
                DynamicState.Append(ref dynamicState, ++seq, rotationClampX);
                DynamicState.Append(ref dynamicState, ++seq, rotationClampY);
                DynamicState.Append(ref dynamicState, ++seq, rotationClampZ);
            }

            DynamicState.Append(ref dynamicState, ++seq, scaleMode);
            if (scaleMode != ClampMode.NoClamp)
            {
                DynamicState.Append(ref dynamicState, ++seq, scaleMin);
                DynamicState.Append(ref dynamicState, ++seq, scaleMax);
                DynamicState.Append(ref dynamicState, ++seq, scaleClampX);
                DynamicState.Append(ref dynamicState, ++seq, scaleClampY);
                DynamicState.Append(ref dynamicState, ++seq, scaleClampZ);
            }

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        void Reset()
        {
            ResetToDefaults();
        }
    }
}
