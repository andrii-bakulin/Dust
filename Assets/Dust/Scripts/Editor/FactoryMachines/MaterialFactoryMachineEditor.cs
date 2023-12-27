using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(MaterialFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class MaterialFactoryMachineEditor : FactoryMachineEditor
    {
        //--------------------------------------------------------------------------------------------------------------

        static MaterialFactoryMachineEditor()
        {
            FactoryMachinesPopupButtons.AddMachine(typeof(MaterialFactoryMachine), "Material");
        }

        [MenuItem("Dust/Factory Machines/Material")]
        [MenuItem("GameObject/Dust/Factory Machines/Material")]
        public static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(MaterialFactoryMachine));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_CustomHint);
            PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
