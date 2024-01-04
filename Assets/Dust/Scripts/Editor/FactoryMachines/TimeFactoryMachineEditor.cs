using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
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

        [MenuItem("Dust/Factory Machines/Time")]
        [MenuItem("GameObject/Dust/Factory Machines/Time")]
        public new static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(TimeFactoryMachine));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorBreadcrumbsForFactoryMachine(this);

            PropertyField(m_Hint);
            PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_TransformBlock();

            OnInspectorGUI_FieldsMap();
            
            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
