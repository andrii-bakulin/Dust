using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Helpers/Lock Transform")]
    [ExecuteAlways]
    public class LockTransform : MonoBehaviour
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_LockPosition = true;
        public bool lockPosition
        {
            get => m_LockPosition;
            set
            {
                if (m_LockPosition == value)
                    return;

                m_LockPosition = value;
                FixLockStates();
            }
        }

        [SerializeField]
        private bool m_LockRotation = true;
        public bool lockRotation
        {
            get => m_LockRotation;
            set
            {
                if (m_LockRotation == value)
                    return;

                m_LockRotation = value;
                FixLockStates();
            }
        }

        [SerializeField]
        private bool m_LockScale = true;
        public bool lockScale
        {
            get => m_LockScale;
            set
            {
                if (m_LockScale == value)
                    return;

                m_LockScale = value;
                FixLockStates();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Vector3 m_Position;
        public Vector3 position
        {
            get => m_Position;
            set => m_Position = value;
        }

        [SerializeField]
        private Quaternion m_Rotation;
        public Quaternion rotation
        {
            get => m_Rotation;
            set => m_Rotation = value;
        }

        [SerializeField]
        private Vector3 m_Scale;
        public Vector3 scale
        {
            get => m_Scale;
            set => m_Scale = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Space m_Space;
        public Space space
        {
            get => m_Space;
            set
            {
                if (m_Space == value)
                    return;

                m_Space = value;
                FixLockStates();
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            FixLockStates();
        }

        private void OnDisable()
        {
            ReleaseLockStates();
        }

        private void LateUpdate()
        {
            UpdateTransformState();
        }

        private void UpdateTransformState()
        {
            if (lockPosition)
            {
                if (space == Space.Local)
                    transform.localPosition = m_Position;
                else
                    transform.position = m_Position;
            }

            if (lockRotation)
            {
                if (space == Space.Local)
                    transform.localRotation = m_Rotation;
                else
                    transform.rotation = m_Rotation;
            }

            if (lockScale)
            {
                transform.localScale = m_Scale;
            }
        }

        public void FixLockStates()
        {
            if (lockPosition)
                m_Position = space == Space.Local ? transform.localPosition : transform.position;

            if (lockRotation)
                m_Rotation = space == Space.Local ? transform.localRotation : transform.rotation;

            if (lockScale)
                m_Scale = transform.localScale;
        }

        private void ReleaseLockStates()
        {
            m_Position = Vector3.zero;
            m_Rotation = Quaternion.identity;
            m_Scale = Vector3.one;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Reset()
        {
            FixLockStates();
        }
    }
}
