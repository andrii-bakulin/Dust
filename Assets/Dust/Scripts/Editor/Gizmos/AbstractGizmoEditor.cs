using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class AbstractGizmoEditor : DuEditor
    {
        protected DuProperty m_Color;
        protected DuProperty m_GizmoVisibility;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddGizmoToSelectedOrNewObject(System.Type duComponentType)
        {
            if (Selection.gameObjects.Length > 0)
            {
                foreach (var gameObject in Selection.gameObjects)
                {
                    Undo.AddComponent(gameObject, duComponentType);
                }
            }
            else
            {
                AddGizmoToNewObject(duComponentType);
            }
        }

        public static Component AddGizmoToNewObject(System.Type duComponentType)
            => AddGizmoToNewObject(duComponentType, true);

        public static Component AddGizmoToNewObject(System.Type duComponentType, bool fixUndoState)
        {
            var gameObject = new GameObject();

            if (Dust.IsNotNull(Selection.activeGameObject))
                gameObject.transform.parent = Selection.activeGameObject.transform;

            var component = gameObject.AddComponent(duComponentType) as AbstractGizmo;

            if (component == null)
                return null;

            gameObject.name = component.GizmoName() + " Gizmo";
            DuTransform.Reset(gameObject.transform);

            if (fixUndoState)
                Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            Selection.activeGameObject = gameObject;
            return component;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Color = FindProperty("m_Color", "Color");
            m_GizmoVisibility = FindProperty("m_GizmoVisibility", "Visibility");
        }
    }
}
