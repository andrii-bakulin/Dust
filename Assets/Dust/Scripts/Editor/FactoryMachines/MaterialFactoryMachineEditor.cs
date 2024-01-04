using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
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

            InspectorBreadcrumbsForFactoryMachine(this);

            PropertyField(m_Hint);
            PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
