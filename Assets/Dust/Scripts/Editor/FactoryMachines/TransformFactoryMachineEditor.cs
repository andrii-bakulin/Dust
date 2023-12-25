using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(TransformFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TransformFactoryMachineEditor : PRSFactoryMachineEditor
    {
        //--------------------------------------------------------------------------------------------------------------

        static TransformFactoryMachineEditor()
        {
            FactoryMachinesPopupButtons.AddMachine(typeof(TransformFactoryMachine), "Transform");
        }

        [MenuItem("Dust/Factory/Machines/Transform")]
        [MenuItem("GameObject/Dust/Factory/Machines/Transform")]
        public new static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(TransformFactoryMachine));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseParameters();
            OnInspectorGUI_TransformBlock();
            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();
            OnInspectorGUI_FieldsMap();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
