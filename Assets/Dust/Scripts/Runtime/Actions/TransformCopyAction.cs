using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Transform Copy Action")]
    public class TransformCopyAction : InstantAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_Position = false;
        public bool position
        {
            get => m_Position;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Position = value;
            }
        }

        [SerializeField]
        private bool m_Rotation = false;
        public bool rotation
        {
            get => m_Rotation;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Rotation = value;
            }
        }

        [SerializeField]
        private bool m_Scale = false;
        public bool scale
        {
            get => m_Scale;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Scale = value;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private GameObject m_SourceObject;
        public GameObject sourceObject
        {
            get => m_SourceObject;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_SourceObject = value;
            }
        }

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Space = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(sourceObject) || sourceObject.Equals(gameObject))
                return;

            if (Dust.IsNull(activeTargetTransform))
                return;

            if (space == Space.World)
            {
                if (position)
                    activeTargetTransform.position = sourceObject.transform.position;

                if (rotation)
                    activeTargetTransform.rotation = sourceObject.transform.rotation;

                if (scale)
                    DuTransform.SetGlobalScale(activeTargetTransform, sourceObject.transform.lossyScale);
            }
            else if (space == Space.Local)
            {
                if (position)
                    activeTargetTransform.localPosition = sourceObject.transform.localPosition;

                if (rotation)
                    activeTargetTransform.localRotation = sourceObject.transform.localRotation;

                if (scale)
                    activeTargetTransform.localScale = sourceObject.transform.localScale;
            }
        }
    }
}
