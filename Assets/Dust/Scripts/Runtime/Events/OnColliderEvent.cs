using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DustEngine
{
    public abstract class OnColliderEvent : OnAbstractEvent
    {
        public enum TagProcessingMode
        {
            Ignore = 0,
            Contains = 1,
            NotContains = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [Serializable]
        public class CollisionEvent : UnityEvent<Collision>
        {
        }

        [Serializable]
        public class TriggerEvent : UnityEvent<Collider>
        {
        }

        [Serializable]
        public class Collision2DEvent : UnityEvent<Collision2D>
        {
        }

        [Serializable]
        public class Trigger2DEvent : UnityEvent<Collider2D>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected List<string> m_ObjectTags = new List<string>();
        public List<string> objectTags => m_ObjectTags;

        [SerializeField]
        private TagProcessingMode m_TagProcessingMode = TagProcessingMode.Ignore;
        public TagProcessingMode tagProcessingMode
        {
            get => m_TagProcessingMode;
            set => m_TagProcessingMode = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected bool IsRequireSendEvent(GameObject otherGameObject)
        {
            if (tagProcessingMode == TagProcessingMode.Ignore)
                return true;

            if (Dust.IsNull(objectTags) || objectTags.Count == 0)
                return true;

            if (tagProcessingMode == TagProcessingMode.NotContains)
                return !objectTags.Contains(otherGameObject.tag);

            // TagProcessingMode.Contains [or] default
            return objectTags.Contains(otherGameObject.tag);
        }
    }
}
