using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Update Hierarchy Action")]
    public class UpdateHierarchyAction : InstantAction
    {
        public enum UpdateMode
        {
            SetTargetAsChildOfReferenceObject = 0,
            SetReferenceObjectAsChildOfTarget = 1,

            CopyParentFromReferenceObject = 2,
            
            SetTargetBeforeReferenceObject = 3,
            SetTargetAfterReferenceObject = 4,
        }

        public enum OrderMode
        {
            First = 0,
            Last = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private UpdateMode m_UpdateMode = UpdateMode.SetTargetAsChildOfReferenceObject;
        public UpdateMode updateMode
        {
            get => m_UpdateMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_UpdateMode = value;
            }
        }

        [SerializeField]
        private OrderMode m_OrderMode = OrderMode.Last;
        public OrderMode orderMode
        {
            get => m_OrderMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_OrderMode = value;
            }
        }

        [SerializeField]
        private GameObject m_ReferenceObject;
        public GameObject referenceObject
        {
            get => m_ReferenceObject;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ReferenceObject = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(activeTargetObject) || referenceObject.Equals(activeTargetObject))
                return;

            if (updateMode == UpdateMode.SetTargetAsChildOfReferenceObject)
            {
                if (UpdateParent(activeTargetObject, referenceObject) == false)
                    return;

                switch (orderMode)
                {
                    case OrderMode.First:
                        activeTargetObject.transform.SetAsFirstSibling();
                        break;

                    case OrderMode.Last:
                        activeTargetObject.transform.SetAsLastSibling();
                        break;
                }
            }
            else if (updateMode == UpdateMode.SetReferenceObjectAsChildOfTarget)
            {
                if (UpdateParent(referenceObject, activeTargetObject) == false)
                    return;

                switch (orderMode)
                {
                    case OrderMode.First:
                        referenceObject.transform.SetAsFirstSibling();
                        break;

                    case OrderMode.Last:
                        referenceObject.transform.SetAsLastSibling();
                        break;
                }
            }
            else if (updateMode == UpdateMode.CopyParentFromReferenceObject ||
                     updateMode == UpdateMode.SetTargetBeforeReferenceObject ||
                     updateMode == UpdateMode.SetTargetAfterReferenceObject)
            {
                if (Dust.IsNull(referenceObject))
                    return;

                var curSiblingIndex = referenceObject.transform.GetSiblingIndex();
                
                activeTargetObject.transform.parent = referenceObject.transform.parent;

                if (Dust.IsNotNull(referenceObject.transform.parent))
                    if (!activeTargetObject.transform.parent.Equals(referenceObject.transform.parent))
                        return;

                if (updateMode == UpdateMode.SetTargetBeforeReferenceObject)
                {
                    activeTargetObject.transform.SetSiblingIndex(curSiblingIndex);
                }
                else if (updateMode == UpdateMode.SetTargetAfterReferenceObject)
                {
                    activeTargetObject.transform.SetSiblingIndex(curSiblingIndex+1);
                }
            }
        }

        protected bool UpdateParent(GameObject newChild, GameObject newParent)
        {
            if (Dust.IsNull(newChild))
                return false;

            if (Dust.IsNull(newParent))
            {
                newChild.transform.parent = null;
                return true;
            }
            
            newChild.transform.parent = newParent.transform;

            return newChild.transform.parent.Equals(newParent.transform);
        }
    }
}
