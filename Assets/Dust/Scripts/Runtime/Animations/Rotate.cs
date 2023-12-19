using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Animations/Rotate")]
    public class Rotate : DuMonoBehaviour
    {
        public enum Space
        {
            World = 0,
            Local = 1,
            Self = 2,
            AroundObject = 3,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_Axis = Vector3.up;
        public Vector3 axis
        {
            get => m_Axis;
            set => m_Axis = value;
        }

        [SerializeField]
        private float m_Speed = 45f;
        public float speed
        {
            get => m_Speed;
            set => m_Speed = value;
        }

        [SerializeField]
        private Space m_Space = Space.Self;
        public Space space
        {
            get => m_Space;
            set => m_Space = value;
        }

        [SerializeField]
        private GameObject m_RotateAroundObject = null;
        public GameObject rotateAroundObject
        {
            get => m_RotateAroundObject;
            set => m_RotateAroundObject = value;
        }

        [SerializeField]
        private UpdateMode m_UpdateMode = UpdateMode.Update;
        public UpdateMode updateMode
        {
            get => m_UpdateMode;
            set => m_UpdateMode = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            if (updateMode != UpdateMode.Update)
                return;

            UpdateState(Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (updateMode != UpdateMode.LateUpdate)
                return;

            UpdateState(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (updateMode != UpdateMode.FixedUpdate)
                return;

            UpdateState(Time.fixedDeltaTime);
        }

        private void UpdateState(float deltaTime)
        {
            switch (space)
            {
                case Space.World:
                    transform.Rotate(axis, speed * deltaTime, UnityEngine.Space.World);
                    break;

                case Space.Local:
                    var localAxis = transform.parent.TransformDirection(axis);
                    transform.Rotate(localAxis, speed * deltaTime, UnityEngine.Space.World);
                    break;

                case Space.Self:
                    var selfAxis = transform.TransformDirection(axis);
                    transform.Rotate(selfAxis, speed * deltaTime, UnityEngine.Space.World);
                    break;

                case Space.AroundObject:
                    if (Dust.IsNull(rotateAroundObject) || rotateAroundObject == this.gameObject)
                        break;

                    transform.RotateAround(rotateAroundObject.transform.position, axis, speed * deltaTime);
                    break;
            }
        }
    }
}
