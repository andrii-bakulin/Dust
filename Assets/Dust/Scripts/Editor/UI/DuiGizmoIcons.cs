using System;
using System.Reflection;
using UnityEditor;

namespace Dust.DustEditor
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
            SetGizmoIconEnabled<FindGameObjectAction>(false);
            SetGizmoIconEnabled<FlipAction>(false);
            SetGizmoIconEnabled<FlowRandomAction>(false);
            SetGizmoIconEnabled<MoveByAction>(false);
            SetGizmoIconEnabled<MoveToAction>(false);
            SetGizmoIconEnabled<RotateByAction>(false);
            SetGizmoIconEnabled<RotateToAction>(false);
            SetGizmoIconEnabled<ScaleByAction>(false);
            SetGizmoIconEnabled<ScaleToAction>(false);
            SetGizmoIconEnabled<SpawnAction>(false);
            SetGizmoIconEnabled<StartAction>(false);
            SetGizmoIconEnabled<TintAction>(false);
            SetGizmoIconEnabled<TransformCopyAction>(false);
            SetGizmoIconEnabled<TransformRandomAction>(false);
            SetGizmoIconEnabled<TransformUpdateAction>(false);
            SetGizmoIconEnabled<UpdateHierarchyAction>(false);

            // Actions.Rigidbody
            SetGizmoIconEnabled<RigidbodyAddForceAction>(false);
            SetGizmoIconEnabled<RigidbodyAddTorqueAction>(false);
            
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
            
            // Factory
            SetGizmoIconEnabled<FactoryInstance>(false);
            SetGizmoIconEnabled<GridFactory>(false);
            SetGizmoIconEnabled<LinearFactory>(false);
            SetGizmoIconEnabled<RadialFactory>(false);
            // FactoryMachines
            SetGizmoIconEnabled<BasicFactoryMachine>(false);
            SetGizmoIconEnabled<ClampFactoryMachine>(false);
            SetGizmoIconEnabled<LookAtFactoryMachine>(false);
            SetGizmoIconEnabled<MaterialFactoryMachine>(false);
            SetGizmoIconEnabled<NoiseFactoryMachine>(false);
            SetGizmoIconEnabled<TimeFactoryMachine>(false);
            SetGizmoIconEnabled<TransformFactoryMachine>(false);
            
            // Fields:2D
            SetGizmoIconEnabled<RadialField>(false);
            SetGizmoIconEnabled<TextureField>(false);
            SetGizmoIconEnabled<TextureFactoryField>(false);
            SetGizmoIconEnabled<WaveField>(false);
            // Fields:3D
            SetGizmoIconEnabled<ConeField>(false);
            SetGizmoIconEnabled<CubeField>(false);
            SetGizmoIconEnabled<CylinderField>(false);
            SetGizmoIconEnabled<DirectionalField>(false);
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
            SetGizmoIconEnabled<ClampPosition>(false);
            SetGizmoIconEnabled<ClampScale>(false);
            SetGizmoIconEnabled<Debugger>(false);
            SetGizmoIconEnabled<Destroyer>(false);
            SetGizmoIconEnabled<LockTransform>(false);
            SetGizmoIconEnabled<RandomTransform>(false);
            SetGizmoIconEnabled<Spawner>(false);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        // Thanks to https://stackoverflow.com/a/74942431

        public static void SetGizmoIconEnabled<T>(bool isEnabled)
        {
            const int MONO_BEHAVIOR_CLASS_ID = 114; // https://docs.unity3d.com/Manual/ClassIDReference.html

            var editor = Assembly.GetAssembly(typeof(Editor));
            if (editor == null)
                return;

            var annotationUtility = editor.GetType("UnityEditor.AnnotationUtility");
            if (annotationUtility == null)
                return;

            // Find solution here:
            // https://github.com/Unity-Technologies/com.unity.probuilder/pull/355/files
            //
            // Seems that getting the annotation array remove the warning
            // Might be initializing something that is missing otherwise
            MethodInfo getAnnotations = annotationUtility.GetMethod("GetAnnotations",
                BindingFlags.Static | BindingFlags.NonPublic,
                null,
                new Type[] { },
                null);
            var annotations = getAnnotations.Invoke(null, new object[] {});

            MethodInfo setIconEnabled = annotationUtility.GetMethod("SetIconEnabled",
                BindingFlags.Static | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(int), typeof(string), typeof(int) },
                null);

            if (setIconEnabled == null)
                return;

            setIconEnabled.Invoke(null, new object[] { MONO_BEHAVIOR_CLASS_ID, typeof(T).Name, isEnabled ? 1 : 0 });
        }
    }
}
