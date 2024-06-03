using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Transform Copy Action")]
    public class TransformCopyAction : InstantAction
    {
        [SerializeField]
        private bool m_Position;
        public bool position
        {
            get => m_Position;
            set => m_Position = value;
        }

        [SerializeField]
        private bool m_Rotation;
        public bool rotation
        {
            get => m_Rotation;
            set => m_Rotation = value;
        }

        [SerializeField]
        private bool m_Scale;
        public bool scale
        {
            get => m_Scale;
            set => m_Scale = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private GameObject m_SourceObject;
        public GameObject sourceObject
        {
            get => m_SourceObject;
            set => m_SourceObject = value;
        }

        [SerializeField]
        private DuTransform.Space m_Space = DuTransform.Space.Local;
        public DuTransform.Space space
        {
            get => m_Space;
            set => m_Space = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionExecute()
        {
            if (Dust.IsNull(sourceObject) || sourceObject.Equals(gameObject))
                return;

            if (Dust.IsNull(activeTargetTransform))
                return;

            if (space == DuTransform.Space.World)
            {
                if (position)
                    activeTargetTransform.position = sourceObject.transform.position;

                if (rotation)
                    activeTargetTransform.rotation = sourceObject.transform.rotation;

                if (scale)
                    DuTransform.SetGlobalScale(activeTargetTransform, sourceObject.transform.lossyScale);
            }
            else if (space == DuTransform.Space.Local)
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
