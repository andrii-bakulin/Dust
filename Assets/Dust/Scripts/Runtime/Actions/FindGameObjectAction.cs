using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Find GameObject")]
    public class FindGameObjectAction : InstantAction
    {
        [Serializable]
        public class FoundEvent : UnityEvent<GameObject>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private string m_TagName = "";
        public string tagName
        {
            get => m_TagName;
            set => m_TagName = value;
        }

        [SerializeField]
        private string m_ObjectName = "";
        public string objectName
        {
            get => m_ObjectName;
            set => m_ObjectName = value;
        }

        [SerializeField]
        protected FoundEvent m_OnFound;
        public FoundEvent onFound => m_OnFound;

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionExecute()
        {
            if (tagName == "" && objectName == "")
                return;
            
            if (Dust.IsNull(onFound) || onFound.GetPersistentEventCount() == 0)
                return;
            
            if (objectName == "")
            {
                // Find by Tag only
                var foundObject = GameObject.FindWithTag(tagName);
                
                if (Dust.IsNotNull(foundObject))
                    onFound.Invoke(foundObject);
            }
            else if (tagName == "")
            {
                // Find by Name only
                var foundObject = GameObject.Find(objectName);
                
                if (Dust.IsNotNull(foundObject))
                    onFound.Invoke(foundObject);
            }
            else
            {
                // Find by both - Tag+Name 
                var foundObjects = GameObject.FindGameObjectsWithTag(tagName);

                foreach (var foundObject in foundObjects)
                {
                    if (foundObject.name != objectName)
                        continue;

                    onFound.Invoke(foundObject);
                    break;
                }
            }
        }
    }
}
