using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Factory Machines/LookAt Machine")]
    public class LookAtFactoryMachine : BasicFactoryMachine
    {
        public enum TargetMode
        {
            ObjectTarget = 0,
            NextInstance = 1,
            PreviousInstance = 2,
        }

        public enum UpVectorMode
        {
            XPlus = 0,
            XMinus = 1,
            YPlus = 2,
            YMinus = 3,
            ZPlus = 4,
            ZMinus = 5,
            Object = 6,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private TargetMode m_TargetMode = TargetMode.ObjectTarget;
        public TargetMode targetMode
        {
            get => m_TargetMode;
            set => m_TargetMode = value;
        }

        [SerializeField]
        private GameObject m_TargetObject;
        public GameObject targetObject
        {
            get => m_TargetObject;
            set => m_TargetObject = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private UpVectorMode m_UpVectorMode = UpVectorMode.YPlus;
        public UpVectorMode upVectorMode
        {
            get => m_UpVectorMode;
            set => m_UpVectorMode = value;
        }

        [SerializeField]
        private GameObject m_UpVectorObject;
        public GameObject upVectorObject
        {
            get => m_UpVectorObject;
            set => m_UpVectorObject = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_LockAxisX;
        public bool lockAxisX
        {
            get => m_LockAxisX;
            set => m_LockAxisX = value;
        }

        [SerializeField]
        private bool m_LockAxisY;
        public bool lockAxisY
        {
            get => m_LockAxisY;
            set => m_LockAxisY = value;
        }

        [SerializeField]
        private bool m_LockAxisZ;
        public bool lockAxisZ
        {
            get => m_LockAxisZ;
            set => m_LockAxisZ = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "LookAt";
        }

        public override string FactoryMachineDynamicHint()
        {
            switch (targetMode)
            {
                default:
                case TargetMode.ObjectTarget:
                    return Dust.IsNotNull(targetObject) ? targetObject.gameObject.name : "";

                case TargetMode.NextInstance:
                    return "Next Instance";

                case TargetMode.PreviousInstance:
                    return "Previous Instance";
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            //----------------------------------------------------------------------------------------------------------
            // [1] Base logic

            float intensityByMachine = factoryInstanceState.intensityByFactory
                                       * intensity;

            UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);

            //----------------------------------------------------------------------------------------------------------
            // [2] Apply LookAt logic

            if (DuMath.IsZero(intensityByMachine))
                return; // no need to continue execute next logic

            if (targetMode == TargetMode.ObjectTarget && Dust.IsNull(targetObject))
                return;

            if (upVectorMode == UpVectorMode.Object && Dust.IsNull(upVectorObject))
                return;

            Vector3 lookDirection;

            // @short links
            var factory = factoryInstanceState.factory;
            var factoryInstance = factoryInstanceState.instance;

            switch (targetMode)
            {
                default:
                case TargetMode.ObjectTarget:
                {
                    Vector3 targetLocalPosition = factory.GetPositionInLocalSpace(targetObject.transform.position);

                    lookDirection = targetLocalPosition - factoryInstance.stateDynamic.position;
                    break;
                }

                case TargetMode.NextInstance:
                    if (Dust.IsNotNull(factoryInstance.nextInstance))
                    {
                        lookDirection = factoryInstance.nextInstance.stateDynamic.position - factoryInstance.stateDynamic.position;
                    }
                    else
                    {
                        if (Dust.IsNull(factoryInstance.prevInstance))
                            return;

                        // calculate value as on prev-state
                        lookDirection = factoryInstance.stateDynamic.position - factoryInstance.prevInstance.stateDynamic.position;
                    }
                    break;

                case TargetMode.PreviousInstance:
                    if (Dust.IsNotNull(factoryInstance.prevInstance))
                    {
                        lookDirection = factoryInstance.prevInstance.stateDynamic.position - factoryInstance.stateDynamic.position;
                    }
                    else
                    {
                        if (Dust.IsNull(factoryInstance.nextInstance))
                            return;

                        // calculate value as on prev-state
                        lookDirection = factoryInstance.stateDynamic.position - factoryInstance.nextInstance.stateDynamic.position;
                    }
                    break;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            Vector3 upVector;

            switch (upVectorMode)
            {
                case UpVectorMode.XPlus:  upVector = Vector3.right; break;
                case UpVectorMode.XMinus: upVector = Vector3.left; break;
                case UpVectorMode.YPlus:  upVector = Vector3.up; break;
                case UpVectorMode.YMinus: upVector = Vector3.down; break;
                case UpVectorMode.ZPlus:  upVector = Vector3.forward; break;
                case UpVectorMode.ZMinus: upVector = Vector3.back; break;

                default:
                case UpVectorMode.Object:
                    upVector = factory.GetPositionInLocalSpace(upVectorObject.transform.position);
                    upVector = upVector - factoryInstance.stateDynamic.position;
                    break;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Warning! Quaternion.LookRotation works in local space

            Quaternion rotationCur = Quaternion.Euler(factoryInstance.stateDynamic.rotation);
            Quaternion rotationNew = Quaternion.LookRotation(lookDirection, upVector);

            var endRotation = Quaternion.LerpUnclamped(rotationCur, rotationNew, intensityByMachine).eulerAngles;

            if (lockAxisX) endRotation.x = factoryInstance.stateDynamic.rotation.x;
            if (lockAxisY) endRotation.y = factoryInstance.stateDynamic.rotation.y;
            if (lockAxisZ) endRotation.z = factoryInstance.stateDynamic.rotation.z;

            factoryInstance.stateDynamic.rotation = endRotation;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DynamicState.Append(ref dynamicState, ++seq, targetMode);
            if (targetMode == TargetMode.ObjectTarget)
                DynamicState.Append(ref dynamicState, ++seq, targetObject);

            DynamicState.Append(ref dynamicState, ++seq, upVectorMode);
            if (upVectorMode == UpVectorMode.Object)
                DynamicState.Append(ref dynamicState, ++seq, upVectorObject);

            DynamicState.Append(ref dynamicState, ++seq, lockAxisX);
            DynamicState.Append(ref dynamicState, ++seq, lockAxisY);
            DynamicState.Append(ref dynamicState, ++seq, lockAxisZ);

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        void Reset()
        {
            ResetToDefaults();
        }
    }
}
