using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [InitializeOnLoad]
    public static class FactoryEditorController
    {
        private static HashSet<Factory> m_QueueUpdateFactory;

        static FactoryEditorController()
        {
            m_QueueUpdateFactory = new HashSet<Factory>();

            PrefabUtility.prefabInstanceUpdated += OnPrefabInstanceUpdated;
            EditorApplication.update += EditorUpdate;
        }

        static void OnPrefabInstanceUpdated(GameObject instance)
        {
            // Notice:
            // Yes. FactoryInstance has [DisallowMultipleComponent] flag, but if user ?SOMEHOW? added few components
            // to a single prefab, then require analyze of all this components!
            var factoryInstances = instance.GetComponents<FactoryInstance>();

            foreach (var factoryInstance in factoryInstances)
            {
                if (Dust.IsNull(factoryInstance) || Dust.IsNull(factoryInstance.parentFactory))
                    continue;

                var factory = factoryInstance.parentFactory;

                if (m_QueueUpdateFactory.Contains(factory))
                    continue;

                if (factory.autoRebuildOnPrefabUpdated == false)
                {
#if DUST_DEBUG_FACTORY_BUILDER
                    Dust.Debug.CheckpointWarning("Factory.Controller", "OnPrefabInstanceUpdated",
                        string.Format("Skip factory rebuilding [{0} (#{1})]", factory.gameObject.name, factory.transform.GetInstanceID()));
#endif
                    continue;
                }

                m_QueueUpdateFactory.Add(factory);

#if DUST_DEBUG_FACTORY_BUILDER
                Dust.Debug.CheckpointWarning("Factory.Controller", "OnPrefabInstanceUpdated",
                    string.Format("Require rebuild factory [{0} (#{1})]", factory.gameObject.name, factory.transform.GetInstanceID()));
#endif
            }
        }

        static void EditorUpdate()
        {
            if (m_QueueUpdateFactory.Count == 0)
                return;

            // Why need this?
            // I had situation when I edit prefab but Factory object already not existed in the Scene.
            // As a result, I tried to call RebuildInstances() method for non-existed objects and had Error in console.
            // After that m_QueueUpdateFactory.Clear() not called (it was after the loop).
            // So, I decide to move Clear() call before the loop.
            //
            // Hm... Hello to Garbage Collector?!

            Factory[] factories = m_QueueUpdateFactory.ToArray();
            m_QueueUpdateFactory.Clear();

            foreach (var factory in factories)
            {
#if DUST_DEBUG_FACTORY_BUILDER
                Dust.Debug.CheckpointWarning("Factory.Controller", "UpdateParentFactory",
                    string.Format("Rebuild [{0} (#{1})]", factory.gameObject.name, factory.transform.GetInstanceID()));
#endif

                factory.RebuildInstances();
            }
        }
    }
}
