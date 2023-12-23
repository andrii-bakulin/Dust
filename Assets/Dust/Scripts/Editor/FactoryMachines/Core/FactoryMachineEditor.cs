using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class FactoryMachineEditor : DuEditor
    {
        protected DuProperty m_CustomHint;
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

                if (Dust.IsNotNull(selectedFactory))
                {
                    selectedFactory.AddFactoryMachine(factoryMachine);

                    if (Dust.IsNotNull(selectedFactory.factoryMachinesHolder))
                    {
                        factoryMachine.transform.parent = selectedFactory.factoryMachinesHolder.transform;
                    }
                    else if(Dust.IsNotNull(selectedFactory.instancesHolder) && selectedFactory.instancesHolder != selectedFactory.gameObject)
                    {
                        // If has instancesHolder and it's not same object as factory
                        // then add factoryMachine as a child for factory object
                        factoryMachine.transform.parent = selectedFactory.transform;
                    }
                    else
                    {
                        // Add factoryMachine on the same level as factory, and right after factory
                        factoryMachine.transform.parent = selectedFactory.transform.parent;
                        factoryMachine.transform.SetSiblingIndex(selectedFactory.transform.GetSiblingIndex() + 1);
                    }
                }

                gameObject.name = factoryMachine.FactoryMachineName() + " Machine";
                DuTransform.Reset(gameObject.transform);
            }

            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            return gameObject;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_CustomHint = FindProperty("m_CustomHint", "Hint for Machine");
            m_Intensity = FindProperty("m_Intensity", "Intensity");
        }

        // WARNING!
        // On change logic/structure here, also required to check changes in parent/children methods
        // with same method name/links
        protected virtual void OnInspectorGUI_BaseParameters()
        {
            if (DustGUI.FoldoutBegin("Parameters", "FactoryMachine.Parameters"))
            {
                PropertyField(m_CustomHint);
                PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
                Space();
            }
            DustGUI.FoldoutEnd();
        }
    }
}
