using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(TimeFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TimeFactoryMachineEditor : PRSFactoryMachineEditor
    {
        //--------------------------------------------------------------------------------------------------------------

        static TimeFactoryMachineEditor()
        {
            FactoryMachinesPopupButtons.AddMachine(typeof(TimeFactoryMachine), "Time");
        }

        [MenuItem("Dust/Factory/Machines/Time")]
        [MenuItem("GameObject/Dust/Factory/Machines/Time")]
        public new static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(TimeFactoryMachine));
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
