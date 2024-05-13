using System;
using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Instantiate Action")]
    public class InstantiateAction : InstantAction
    {
        public enum Space
        {
            Self = 0,
            Local = 1,
            World = 2,
            GameObject = 3
        }

        public enum ParentMode
        {
            Keep = 0,
            Self = 1,
            ParentOfSelf = 2,
            World = 3,
            GameObject = 4
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private GameObject m_Original;
        public GameObject original
        {
            get => m_Original;
            set => m_Original = value;
        }

        [SerializeField]
        private Space m_InstantiateSpace;
        public Space instantiateSpace
        {
            get => m_InstantiateSpace;
            set => m_InstantiateSpace = value;
        }

        [SerializeField]
        private GameObject m_SpaceGameObject;
        public GameObject spaceGameObject
        {
            get => m_SpaceGameObject;
            set => m_SpaceGameObject = value;
        }
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_PositionEnabled;
        public bool positionEnabled
        {
            get => m_PositionEnabled;
            set => m_PositionEnabled = value;
        }

        [SerializeField]
        private Vector3 m_Position = Vector3.zero;
        public Vector3 position
        {
            get => m_Position;
            set => m_Position = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_RotationEnabled;
        public bool rotationEnabled
        {
            get => m_RotationEnabled;
            set => m_RotationEnabled = value;
        }

        [SerializeField]
        private Vector3 m_Rotation = Vector3.zero;
        public Vector3 rotation
        {
            get => m_Rotation;
            set => m_Rotation = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ScaleEnabled;
        public bool scaleEnabled
        {
            get => m_ScaleEnabled;
            set => m_ScaleEnabled = value;
        }

        [SerializeField]
        private Vector3 m_Scale = Vector3.one;
        public Vector3 scale
        {
            get => m_Scale;
            set => m_Scale = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private ParentMode m_ParentMode;
        public ParentMode parentMode
        {
            get => m_ParentMode;
            set => m_ParentMode = value;
        }

        [SerializeField]
        private GameObject m_ParentGameObject;
        public GameObject parentGameObject
        {
            get => m_ParentGameObject;
            set => m_ParentGameObject = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionExecute()
        {
            if (Dust.IsNull(original))
                return;

            if (instantiateSpace == Space.GameObject && Dust.IsNull(spaceGameObject))
                return;

            if (parentMode == ParentMode.GameObject && Dust.IsNull(parentGameObject))
                return;

            Transform instantiateOwner = instantiateSpace switch
            {
                Space.Self => transform,
                Space.Local => transform.parent,
                Space.World => null,
                Space.GameObject => spaceGameObject.transform,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            var gameObj = Instantiate(original, instantiateOwner);
            
            if (positionEnabled)
                DuTransform.UpdatePosition(gameObj.transform, position, DuTransform.Space.Local, DuTransform.Mode.Set);

            if (rotationEnabled)
                DuTransform.UpdateRotation(gameObj.transform, rotation, DuTransform.Space.Local, DuTransform.Mode.Set);

            if (scaleEnabled)
                DuTransform.UpdateScale(gameObj.transform, scale, DuTransform.Space.Local, DuTransform.Mode.Set);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (parentMode != ParentMode.Keep)
            {
                gameObj.transform.parent = parentMode switch
                {
                    ParentMode.Self => transform,
                    ParentMode.ParentOfSelf => transform.parent,
                    ParentMode.World => null,
                    ParentMode.GameObject => parentGameObject.transform,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }
}
