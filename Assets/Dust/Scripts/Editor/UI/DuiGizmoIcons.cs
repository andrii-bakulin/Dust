using System.Reflection;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [InitializeOnLoad]
    public class DuiGizmoIcons
    {
        static DuiGizmoIcons()
        {
            // Actions
            SetGizmoIconEnabled<ActivateAction>(false);
            SetGizmoIconEnabled<CallbackAction>(false);
            SetGizmoIconEnabled<DelayAction>(false);
            SetGizmoIconEnabled<DestroyAction>(false);
            SetGizmoIconEnabled<FlipAction>(false);
            SetGizmoIconEnabled<FlowRandomAction>(false);
            SetGizmoIconEnabled<MoveByAction>(false);
            SetGizmoIconEnabled<MoveToAction>(false);
            SetGizmoIconEnabled<RotateByAction>(false);
            SetGizmoIconEnabled<RotateToAction>(false);
            SetGizmoIconEnabled<ScaleByAction>(false);
            SetGizmoIconEnabled<ScaleToAction>(false);
            SetGizmoIconEnabled<SpawnAction>(false);
            SetGizmoIconEnabled<TintAction>(false);
            SetGizmoIconEnabled<TransformCopyAction>(false);
            SetGizmoIconEnabled<TransformRandomAction>(false);
            SetGizmoIconEnabled<TransformSetAction>(false);
            SetGizmoIconEnabled<UpdateHierarchyAction>(false);
            
            // Animations
            SetGizmoIconEnabled<Follow>(false);
            SetGizmoIconEnabled<LookAt>(false);
            SetGizmoIconEnabled<Move>(false);
            SetGizmoIconEnabled<Pulsate>(false);
            SetGizmoIconEnabled<Rotate>(false);
            SetGizmoIconEnabled<Scale>(false);
            SetGizmoIconEnabled<Shake>(false);
            
            // Events
            SetGizmoIconEnabled<OnCollision2DEvent>(false);
            SetGizmoIconEnabled<OnCollisionEvent>(false);
            SetGizmoIconEnabled<OnKeyEvent>(false);
            SetGizmoIconEnabled<OnMouseEvent>(false);
            SetGizmoIconEnabled<OnTimerEvent>(false);
            SetGizmoIconEnabled<OnTrigger2DEvent>(false);
            SetGizmoIconEnabled<OnTriggerEvent>(false);
            
            // Gizmos
            SetGizmoIconEnabled<ArrowGizmo>(false);
            SetGizmoIconEnabled<ConeGizmo>(false);
            SetGizmoIconEnabled<CubeGizmo>(false);
            SetGizmoIconEnabled<CylinderGizmo>(false);
            SetGizmoIconEnabled<MeshGizmo>(false);
            SetGizmoIconEnabled<PyramidGizmo>(false);
            SetGizmoIconEnabled<SphereGizmo>(false);
            SetGizmoIconEnabled<TorusGizmo>(false);
            SetGizmoIconEnabled<TriggerGizmo>(false);
            
            // Helpers
            SetGizmoIconEnabled<Debugger>(false);
            SetGizmoIconEnabled<Destroyer>(false);
            SetGizmoIconEnabled<LockTransform>(false);
            SetGizmoIconEnabled<RandomTransform>(false);
            SetGizmoIconEnabled<Spawner>(false);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        // Thanks to https://stackoverflow.com/a/74942431

        private static MethodInfo setIconEnabled;
            
        private static void SetGizmoIconEnabled<T>(bool isEnabled)
        {
            const int MONO_BEHAVIOR_CLASS_ID = 114; // https://docs.unity3d.com/Manual/ClassIDReference.html

            if (setIconEnabled == null)
            {
                var editor = Assembly.GetAssembly(typeof(Editor));
                if (editor == null)
                    return;

                var annotationUtility = editor.GetType("UnityEditor.AnnotationUtility");
                if (annotationUtility == null)
                    return;

                setIconEnabled = annotationUtility.GetMethod("SetIconEnabled",
                    BindingFlags.Static | BindingFlags.NonPublic);

                if (setIconEnabled == null)
                    return;
            }

            setIconEnabled.Invoke(null, new object[] { MONO_BEHAVIOR_CLASS_ID, typeof(T).Name, isEnabled ? 1 : 0 });
        }
    }
}
