using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Animations/Scale")]
    public class Scale : DuMonoBehaviour
    {
        [SerializeField]
        private Vector3 m_DeltaScale = Vector3.one;
        public Vector3 deltaScale
        {
            get => m_DeltaScale;
            set => m_DeltaScale = value;
        }

        [SerializeField]
        private float m_Speed = 1f;
        public float speed
        {
            get => m_Speed;
            set => m_Speed = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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
            if (freeze)
                return;

            transform.localScale += deltaScale * (speed * deltaTime);
        }
    }
}
