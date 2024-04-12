using UnityEngine;
using UnityEditor;
#if !DUST_IGNORE_EXPERIMENTAL_DEFORMERS
using AbstractDeformer = Dust.Experimental.Deformers.AbstractDeformer;
#endif

namespace Dust.DustEditor
{
    public abstract class FieldEditor : DuEditor
    {
        protected DuProperty m_Hint;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddFieldComponentByType(System.Type duFieldType)
        {
            Selection.activeGameObject = AddFieldComponentByType(Selection.activeGameObject, duFieldType);
        }

        public static GameObject AddFieldComponentByType(GameObject activeGameObject, System.Type duFieldType)
        {
#if !DUST_IGNORE_EXPERIMENTAL_DEFORMERS
            AbstractDeformer selectedDeformer = null;
#endif
            FieldsSpace selectedFieldsSpace = null;
            BasicFactoryMachine selectedFactoryMachine = null;

            if (Dust.IsNotNull(activeGameObject))
            {
#if !DUST_IGNORE_EXPERIMENTAL_DEFORMERS
                selectedDeformer = activeGameObject.GetComponent<AbstractDeformer>();

                if (Dust.IsNull(selectedDeformer) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedDeformer = activeGameObject.transform.parent.GetComponent<AbstractDeformer>();
#endif

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                selectedFieldsSpace = activeGameObject.GetComponent<FieldsSpace>();

                if (Dust.IsNull(selectedFieldsSpace) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedFieldsSpace = activeGameObject.transform.parent.GetComponent<FieldsSpace>();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                selectedFactoryMachine = activeGameObject.GetComponent<BasicFactoryMachine>();

                if (Dust.IsNull(selectedFactoryMachine) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedFactoryMachine = activeGameObject.transform.parent.GetComponent<BasicFactoryMachine>();
            }

            var gameObject = new GameObject();
            {
                var field = gameObject.AddComponent(duFieldType) as Field;

                if (Dust.IsNull(field))
                    return null;

#if !DUST_IGNORE_EXPERIMENTAL_DEFORMERS
                if (Dust.IsNotNull(selectedDeformer))
                {
                    field.transform.parent = selectedDeformer.transform;
                    selectedDeformer.fieldsMap.AddField(field);
                }
                else
#endif
                if (Dust.IsNotNull(selectedFieldsSpace))
                {
                    field.transform.parent = selectedFieldsSpace.transform;
                    selectedFieldsSpace.fieldsMap.AddField(field);
                }
                else if (Dust.IsNotNull(selectedFactoryMachine))
                {
                    field.transform.parent = selectedFactoryMachine.transform;
                    selectedFactoryMachine.fieldsMap.AddField(field);
                }

                gameObject.name = field.FieldName() + " Field";
                DuTransform.Reset(gameObject.transform);
            }

            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            return gameObject;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Hint = FindProperty("m_Hint", "Hint for Field");
        }
    }
}
