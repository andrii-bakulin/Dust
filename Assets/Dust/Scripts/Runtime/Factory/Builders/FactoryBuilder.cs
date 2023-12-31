﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dust
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
                instanceGameObject = UnityEngine.Object.Instantiate(prefab, parent);
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

        internal void ObjectsQueue_Initialize()
        {
            switch (m_Factory.iterateMode)
            {
                case Factory.IterateMode.Iterate:
                    m_ObjectsQueue_index = 0;
                    break;

                case Factory.IterateMode.Random:
                    m_ObjectsQueue_duRandom = new DuRandom( DuRandom.NormalizeSeedToNonRandom(m_Factory.seed) );
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private GameObject ObjectsQueue_GetNextPrefab()
        {
            if (m_Factory.sourceObjects.Count == 0)
                return null;

            switch (m_Factory.iterateMode)
            {
                case Factory.IterateMode.Iterate:
                    return m_Factory.sourceObjects[m_ObjectsQueue_index++ % m_Factory.sourceObjects.Count];

                case Factory.IterateMode.Random:
                    return m_Factory.sourceObjects[m_ObjectsQueue_duRandom.Range(0, m_Factory.sourceObjects.Count)];

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void ObjectsQueue_Release()
        {
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
