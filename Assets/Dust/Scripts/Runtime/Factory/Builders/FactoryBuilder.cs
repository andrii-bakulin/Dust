using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class FactoryBuilder
    {
        protected Factory m_Factory;

        protected List<FactoryInstance.State> m_InstancesStates;

        public virtual void Initialize(Factory factory)
        {
            m_Factory = factory;

            m_InstancesStates = new List<FactoryInstance.State>();
        }

        // Alias for Initialize, but in some cases logic maybe much simpler
        public virtual void Reinitialize(Factory factory)
        {
            Initialize(factory);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Instance Manager

        // WARNING!
        //     If some logic change here, then require to do same changes in CreateFactoryFakeInstance() method !!!
        //
        internal FactoryInstance CreateFactoryInstance(int instanceIndex, int instancesCount, float randomScalar, Vector3 randomVector)
        {
            GameObject prefab = ObjectsQueue_GetNextPrefab();

            if (Dust.IsNull(prefab))
                return null;

            Transform parent = m_Factory.GetInstancesHolderTransform();

            GameObject instanceGameObject = null;

#if UNITY_EDITOR
            if (m_Factory.instanceTypeMode == Factory.InstanceTypeMode.Inherit)
            {
                instanceGameObject = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
            }
#endif

            // Notice: When I create prefab by PrefabUtility.InstantiatePrefab() call sometimes it cannot be created
            // and instanceGameObject is NULL. In that cases I forced create instance as object!
            if (Dust.IsNull(instanceGameObject))
            {
                instanceGameObject = Object.Instantiate(prefab, parent);
                instanceGameObject.name = instanceGameObject.name.Replace("(Clone)", "");
            }

            if (Dust.IsNull(instanceGameObject))
                return null;

            if (m_Factory.forcedSetActive)
                instanceGameObject.SetActive(true);

            FactoryInstance factoryInstance = instanceGameObject.GetComponent<FactoryInstance>();

            if (Dust.IsNull(factoryInstance))
                factoryInstance = instanceGameObject.AddComponent<FactoryInstance>();

            float instanceOffset = instancesCount > 1 ? (float) instanceIndex / (instancesCount - 1) : 0f;

            factoryInstance.Initialize(m_Factory, instanceIndex, instanceOffset, randomScalar, randomVector);

            return factoryInstance;
        }

        // WARNING! Why need this method?
        //     This method will be call when fill-rate is less then 1.0f.
        //     So when need to skip instance creating Builder will call this method.
        //     It should not create anything, but it should "call sub-method" to make offset in random generators
        //     or others data
        internal FactoryInstance CreateFactoryFakeInstance(int instanceIndex, int instancesCount, float randomScalar, Vector3 randomVector)
        {
            ObjectsQueue_GetNextPrefab();
            return null;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Objects Queue

        private int m_ObjectsQueue_index;
        private DuRandom m_ObjectsQueue_duRandom;
        private List<GameObject> m_FinalSourceObjects;

        internal void ObjectsQueue_Initialize()
        {
            m_FinalSourceObjects = new List<GameObject>();

            if (m_Factory.sourceObjectsMode == Factory.SourceObjectsMode.Holder ||
                m_Factory.sourceObjectsMode == Factory.SourceObjectsMode.HolderAndList)
            {
                // Make SourceObject list. Step 1:
                if (Dust.IsNotNull(m_Factory.sourceObjectsHolder))
                {
                    var holder = m_Factory.sourceObjectsHolder.transform;
                    for (int i = 0; i < holder.childCount; i++)
                    {
                        m_FinalSourceObjects.Add(holder.GetChild(i).gameObject);
                    }
                }
            }

            if (m_Factory.sourceObjectsMode == Factory.SourceObjectsMode.List ||
                m_Factory.sourceObjectsMode == Factory.SourceObjectsMode.HolderAndList)
            {
                // Make SourceObject list. Step 2:
                foreach (var sourceObject in m_Factory.sourceObjects)
                {
                    m_FinalSourceObjects.Add(sourceObject);
                }
            }

            switch (m_Factory.iterateMode)
            {
                case Factory.IterateMode.Iterate:
                    m_ObjectsQueue_index = 0;
                    break;

                case Factory.IterateMode.Random:
                    m_ObjectsQueue_duRandom = new DuRandom( DuRandom.NormalizeSeedToNonRandom(m_Factory.seed) );
                    break;

                default:
                    break;
            }
        }

        private GameObject ObjectsQueue_GetNextPrefab()
        {
            if (m_FinalSourceObjects.Count == 0)
                return null;

            switch (m_Factory.iterateMode)
            {
                case Factory.IterateMode.Iterate:
                default:
                    return m_FinalSourceObjects[m_ObjectsQueue_index++ % m_FinalSourceObjects.Count];

                case Factory.IterateMode.Random:
                    return m_FinalSourceObjects[m_ObjectsQueue_duRandom.Range(0, m_FinalSourceObjects.Count)];
            }
        }

        internal void ObjectsQueue_Release()
        {
            m_FinalSourceObjects = null;
            m_ObjectsQueue_duRandom = null;
        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual int GetTotalInstancesCount()
        {
            return m_InstancesStates.Count;
        }

        // This method should calculate:
        // - position, rotation, scale
        // - UVW
        // Values for (next) params will be defined by instance:
        // - value
        // - color
        public virtual FactoryInstance.State GetDefaultInstanceState(FactoryInstance factoryInstance)
        {
            return m_InstancesStates[factoryInstance.index].Clone();
        }
    }
}
