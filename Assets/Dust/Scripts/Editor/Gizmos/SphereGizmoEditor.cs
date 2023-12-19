using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(SphereGizmo))]
    [CanEditMultipleObjects]
    public class SphereGizmoEditor : AbstractGizmoEditor
    {
        private DuProperty m_Radius;
        private DuProperty m_Center;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Sphere")]
        public static void AddComponentToSelectedObjects()
        {
            AddGizmoToSelectedOrNewObject(typeof(SphereGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Radius = FindProperty("m_Radius", "Radius");
            m_Center = FindProperty("m_Center", "Center");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyExtendedSlider(m_Radius, 0f, 10f, 0.01f);
            PropertyField(m_Center);
            Space();
            PropertyField(m_Color);
            PropertyField(m_GizmoVisibility);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
