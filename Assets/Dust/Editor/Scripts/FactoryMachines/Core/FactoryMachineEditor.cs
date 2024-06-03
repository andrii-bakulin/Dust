using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    public abstract class FactoryMachineEditor : DuEditor
    {
        protected DuProperty m_Hint;
        protected DuProperty m_Intensity;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddFactoryMachineComponentByType(System.Type factoryType)
        {
            Selection.activeGameObject = AddFactoryMachineComponentByType(Selection.activeGameObject, factoryType);
        }

        public static GameObject AddFactoryMachineComponentByType(GameObject activeGameObject, System.Type factoryMachineType)
        {
            Factory selectedFactory = null;

            if (Dust.IsNotNull(activeGameObject))
            {
                selectedFactory = activeGameObject.GetComponent<Factory>();

                if (Dust.IsNull(selectedFactory) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedFactory = activeGameObject.transform.parent.GetComponent<Factory>();
            }

            var gameObject = new GameObject();
            {
                var factoryMachine = gameObject.AddComponent(factoryMachineType) as FactoryMachine;

                if (Dust.IsNull(factoryMachine))
                {
                    Debug.LogError("Cannot add FactoryMachine component");
                    return null;
                }

                gameObject.name = factoryMachine.FactoryMachineName() + " Machine";
                
                if (Dust.IsNotNull(selectedFactory))
                {
                    selectedFactory.AddFactoryMachine(factoryMachine);

                    factoryMachine.transform.parent = selectedFactory.transform;
                    factoryMachine.transform.SetSiblingIndex(0);

                    // Need put factory-machine in special place in tree.
                    // - if selectedFactory already has one or more factoryMachines -> require add after last one
                    // - if not (default) -> add it as a first child
                    
                    for (int i = selectedFactory.transform.childCount - 1; i >= 0; i--)
                    {
                        var fm = selectedFactory.transform.GetChild(i).GetComponent<FactoryMachine>();
                        
                        if (Dust.IsNotNull(fm) && fm != factoryMachine)
                        {
                            factoryMachine.transform.SetSiblingIndex(i);
                            break;
                        }
                    }
                }

                DuTransform.Reset(gameObject.transform);
            }

            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            return gameObject;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Hint = FindProperty("m_Hint", "Hint for Machine");
            m_Intensity = FindProperty("m_Intensity", "Intensity");
        }
    }
}
