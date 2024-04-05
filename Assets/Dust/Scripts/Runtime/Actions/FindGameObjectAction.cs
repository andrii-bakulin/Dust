using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Find GameObject")]
    public class FindGameObjectAction : InstantAction
    {
        public enum Scope
        {
            Scene = 0,
            Parents = 1,
            Children = 2
        }
        
        [Serializable]
        public class FoundEvent : UnityEvent<GameObject>
        {
        }

        [Serializable]
        public class NotFoundEvent : UnityEvent
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Scope m_Scope = Scope.Scene;
        public Scope scope
        {
            get => m_Scope;
            set => m_Scope = value;
        }

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

        [SerializeField]
        protected NotFoundEvent m_OnNotFound;
        public NotFoundEvent onNotFound => m_OnNotFound;

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionExecute()
        {
            GameObject gameObj = m_Scope switch
            {
                Scope.Scene => SearchGameObjectInScene(),
                Scope.Parents => SearchGameObjectInParents(activeTargetTransform.parent),
                Scope.Children => SearchGameObjectInChildren(activeTargetTransform),
                _ => null
            };

            if (Dust.IsNotNull(gameObj))
                onFound?.Invoke(gameObj);
            else
                onNotFound?.Invoke();
        }

        protected GameObject SearchGameObjectInScene()
        {
            if (tagName == "" && objectName == "")
                return null;

            if (objectName == "")
                return GameObject.FindWithTag(tagName); // Find by Tag only

            if (tagName == "")
                return GameObject.Find(objectName); // Find by Name only

            // Find by both - Tag + Name 
            var foundObjects = GameObject.FindGameObjectsWithTag(tagName);

            foreach (var foundObject in foundObjects)
            {
                if (foundObject.name != objectName)
                    continue;

                return foundObject;
            }

            return null;
        }

        protected GameObject SearchGameObjectInParents(Transform tr)
        {
            while (Dust.IsNotNull(tr))
            {
                if (IsGameObjectMatchRules(tr.gameObject))
                    return tr.gameObject;
                
                tr = tr.parent;
            }

            return null;
        }

        protected GameObject SearchGameObjectInChildren(Transform tr)
        {
            for (int i = 0; i < tr.childCount; i++)
            {
                var gameObj = tr.GetChild(i).gameObject;

                if (IsGameObjectMatchRules(gameObj))
                    return gameObj;
            }
            
            for (int i = 0; i < tr.childCount; i++)
            {
                var gameObj = SearchGameObjectInChildren(tr.GetChild(i));

                if (Dust.IsNotNull(gameObj))
                    return gameObj;
            }

            return null;
        }

        protected bool IsGameObjectMatchRules(GameObject gameObj)
        {
            if (tagName != "" && objectName != "")
                return gameObj.CompareTag(tagName) && gameObj.name == objectName;

            if (tagName != "")
                return gameObj.CompareTag(tagName);

            if (objectName != "")
                return gameObj.name == objectName;

            return true;
        }
    }
}
