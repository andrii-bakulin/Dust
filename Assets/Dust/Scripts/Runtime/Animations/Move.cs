using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Animations/Move")]
    public class Move : DuMonoBehaviour
    {
        public enum Space
        {
            World = 0,
            Local = 1,
            Self = 2,
        }

        public enum TranslateType
        {
            Linear = 0,
            Wave = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private TranslateType m_TranslateType = TranslateType.Linear;
        public TranslateType translateType
        {
            get => m_TranslateType;
            set => m_TranslateType = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Vector3 m_LinearSpeed = Vector3.forward;
        public Vector3 linearSpeed
        {
            get => m_LinearSpeed;
            set => m_LinearSpeed = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Vector3 m_WaveAmplitude = Vector3.forward;
        public Vector3 waveAmplitude
        {
            get => m_WaveAmplitude;
            set => m_WaveAmplitude = value;
        }

        [SerializeField]
        private Vector3 m_WaveSpeed = Vector3.forward * 360;
        public Vector3 waveSpeed
        {
            get => m_WaveSpeed;
            set => m_WaveSpeed = value;
        }

        [SerializeField]
        private Vector3 m_WaveOffset = Vector3.zero;
        public Vector3 waveOffset
        {
            get => m_WaveOffset;
            set => m_WaveOffset = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set => m_Space = value;
        }

        [SerializeField]
        private bool m_Freeze = false;
        public bool freeze
        {
            get => m_Freeze;
            set => m_Freeze = value;
        }

        [SerializeField]
        private UpdateMode m_UpdateMode = UpdateMode.Update;
        public UpdateMode updateMode
        {
            get => m_UpdateMode;
            set => m_UpdateMode = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private Vector3 m_LastDeltaPosition;
        public Vector3 lastDeltaPosition => m_LastDeltaPosition;

        private float m_TimeSinceStart;

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            if (updateMode != UpdateMode.Update)
                return;

            UpdateState(Time.deltaTime);
        }

        void LateUpdate()
        {
            if (updateMode != UpdateMode.LateUpdate)
                return;

            UpdateState(Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (updateMode != UpdateMode.FixedUpdate)
                return;

            UpdateState(Time.fixedDeltaTime);
        }

        void UpdateState(float deltaTime)
        {
            if (freeze)
                return;

            m_TimeSinceStart += deltaTime;

            Vector3 deltaPosition;
            bool requireRollbackLastTranslate = false;

            switch (translateType)
            {
                case TranslateType.Linear:
                    deltaPosition = linearSpeed * deltaTime;
                    break;

                case TranslateType.Wave:
                    deltaPosition = new Vector3(
                        CalculateWaveOffset(waveAmplitude.x, waveSpeed.x, waveOffset.x),
                        CalculateWaveOffset(waveAmplitude.y, waveSpeed.y, waveOffset.y),
                        CalculateWaveOffset(waveAmplitude.z, waveSpeed.z, waveOffset.z));
                    requireRollbackLastTranslate = true;
                    break;

                default:
                    return;
            }

            if (space == Space.Self)
                deltaPosition = DuMath.RotatePoint(deltaPosition, this.transform.localEulerAngles);

            Vector3 extDeltaPosition = deltaPosition;

            if (requireRollbackLastTranslate)
                extDeltaPosition -= m_LastDeltaPosition;

            switch (space)
            {
                case Space.World:
                    this.transform.position += extDeltaPosition;
                    break;

                case Space.Local:
                case Space.Self:
                    this.transform.localPosition += extDeltaPosition;
                    break;
            }

            m_LastDeltaPosition = deltaPosition;
        }

        float CalculateWaveOffset(float amplitude, float speed, float offset)
        {
            if (DuMath.IsZero(amplitude))
                return 0f;

            return Mathf.Sin(Constants.PI2 * (speed / 360f) * m_TimeSinceStart + Constants.PI2 * offset) * amplitude;
        }
    }
}
