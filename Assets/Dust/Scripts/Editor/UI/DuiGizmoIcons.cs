using System.Reflection;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [InitializeOnLoad]
    public class DuiGizmoIcons
    {
        static DuiGizmoIcons()
        {
            // Animations
            SetGizmoIconEnabled<Follow>(false);
            SetGizmoIconEnabled<LookAt>(false);
            SetGizmoIconEnabled<Move>(false);
            SetGizmoIconEnabled<Pulsate>(false);
            SetGizmoIconEnabled<Rotate>(false);
            SetGizmoIconEnabled<Scale>(false);
            SetGizmoIconEnabled<Shake>(false);
            
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
        }
        
        //--------------------------------------------------------------------------------------------------------------
        // Thanks to https://stackoverflow.com/a/74942431
        
        private static MethodInfo setIconEnabled = null;
            
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
