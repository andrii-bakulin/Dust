using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(TorusGizmo))]
    [CanEditMultipleObjects]
    public class TorusGizmoEditor : AbstractGizmoEditor
    {
        private DuProperty m_Radius;
        private DuProperty m_Thickness;
        private DuProperty m_Center;
        private DuProperty m_Direction;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Torus")]
        public static void AddComponentToSelectedObjects()
        {
            AddGizmoToSelectedOrNewObject(typeof(TorusGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Radius = FindProperty("m_Radius", "Radius");
            m_Thickness = FindProperty("m_Thickness", "Thickness");
            m_Center = FindProperty("m_Center", "Center");
            m_Direction = FindProperty("m_Direction", "Direction");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyExtendedSlider(m_Radius, 0f, 10f, 0.01f);
            PropertyExtendedSlider(m_Thickness, 0f, 10f, 0.01f);
            PropertyField(m_Center);
            PropertyField(m_Direction);
            Space();
            PropertyField(m_Color);
            PropertyField(m_GizmoVisibility);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
