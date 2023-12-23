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
            
#if DUST_NEW_FEATURE_DEFORMER
            // Deformers
#endif
            
            // Events
            SetGizmoIconEnabled<OnCollision2DEvent>(false);
            SetGizmoIconEnabled<OnCollisionEvent>(false);
            SetGizmoIconEnabled<OnKeyEvent>(false);
            SetGizmoIconEnabled<OnMouseEvent>(false);
            SetGizmoIconEnabled<OnTimerEvent>(false);
            SetGizmoIconEnabled<OnTrigger2DEvent>(false);
            SetGizmoIconEnabled<OnTriggerEvent>(false);
            
#if DUST_NEW_FEATURE_FACTORY
            // Factory
#endif
            
            // Fields:2D
            SetGizmoIconEnabled<DirectionalField>(false);
            SetGizmoIconEnabled<RadialField>(false);
            SetGizmoIconEnabled<TextureSpaceField>(false);
            SetGizmoIconEnabled<WaveField>(false);
            // Fields:3D
            SetGizmoIconEnabled<ConeField>(false);
            SetGizmoIconEnabled<CubeField>(false);
            SetGizmoIconEnabled<CylinderField>(false);
            SetGizmoIconEnabled<SphereField>(false);
            SetGizmoIconEnabled<TorusField>(false);
            // Fields:Basic
            SetGizmoIconEnabled<ConstantField>(false);
            SetGizmoIconEnabled<CoordinatesField>(false);
            SetGizmoIconEnabled<NoiseField>(false);
            SetGizmoIconEnabled<TimeField>(false);
            // Fields:Math
            SetGizmoIconEnabled<ClampField>(false);
            SetGizmoIconEnabled<CurveField>(false);
            SetGizmoIconEnabled<FitField>(false);
            SetGizmoIconEnabled<InvertField>(false);
            SetGizmoIconEnabled<RemapField>(false);
            SetGizmoIconEnabled<RoundField>(false);
            // Fields
            SetGizmoIconEnabled<FieldsSpace>(false);
            
            // Gizmos
            SetGizmoIconEnabled<ArrowGizmo>(false);
            SetGizmoIconEnabled<ConeGizmo>(false);
            SetGizmoIconEnabled<CubeGizmo>(false);
            SetGizmoIconEnabled<CylinderGizmo>(false);
            SetGizmoIconEnabled<FieldGizmo>(false);
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
            
        public static void SetGizmoIconEnabled<T>(bool isEnabled)
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
