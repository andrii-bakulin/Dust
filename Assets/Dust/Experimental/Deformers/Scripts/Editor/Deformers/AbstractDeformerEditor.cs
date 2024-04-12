using Dust.DustEditor;
using UnityEngine;
using UnityEditor;

namespace Dust.Experimental.Deformers.Editor
{
    public abstract class AbstractDeformerEditor : DustEditor.DuEditor
    {
        protected DuProperty m_GizmoVisibility;

        protected FieldsMapEditor m_FieldsMapEditor;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddDeformerComponentByType(System.Type type)
        {
            Selection.activeGameObject = AddDeformerComponentByType(Selection.activeGameObject, type);
        }

        public static GameObject AddDeformerComponentByType(GameObject activeGameObject, System.Type type)
        {
            MeshDeformer selectedDeformMesh = null;

            if (Dust.IsNotNull(activeGameObject))
            {
                selectedDeformMesh = activeGameObject.GetComponent<MeshDeformer>();

                if (Dust.IsNull(selectedDeformMesh) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedDeformMesh = activeGameObject.transform.parent.GetComponent<MeshDeformer>();
            }

            var gameObject = new GameObject();
            {
                var deformer = gameObject.AddComponent(type) as AbstractDeformer;

                if (Dust.IsNotNull(selectedDeformMesh))
                {
                    deformer.transform.parent = selectedDeformMesh.transform;
                    selectedDeformMesh.AddDeformer(deformer);
                }

                gameObject.name = deformer.DeformerName() + " Deformer";
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
            }

            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            return gameObject;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_GizmoVisibility = FindProperty("m_GizmoVisibility", "Visibility");

            m_FieldsMapEditor = new FieldsMapEditor(this, serializedObject.FindProperty("m_FieldsMap"), (target as AbstractDeformer).fieldsMap);
        }

        public override void OnInspectorGUI()
        {
            // Hide default OnInspectorGUI() call
            // Extend all-deformers-view if need in future...
        }

        protected void OnInspectorGUI_FieldsMap()
        {
            m_FieldsMapEditor.OnInspectorGUI();
        }

        protected void OnInspectorGUI_GizmoBlock()
        {
            if (DustGUI.FoldoutBegin("Gizmo", "AbstractDeformer.Gizmo"))
            {
                PropertyField(m_GizmoVisibility);
                Space();
            }
            DustGUI.FoldoutEnd();
        }
    }
}
