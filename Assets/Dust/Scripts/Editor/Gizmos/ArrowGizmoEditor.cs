using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(ArrowGizmo))]
    [CanEditMultipleObjects]
    public class ArrowGizmoEditor : AbstractGizmoEditor
    {
        private static string[] s_DirectionOptions =
        {
            "Select",
            "Up (0, +1, 0)",
            "Down (0, -1, 0)",
            "Left (-1, 0, 0)",
            "Right (+1, 0, 0)",
            "Forward (0, 0, +1)",
            "Back (0, 0, -1)",
        };

        private DuProperty m_Direction;
        private DuProperty m_StartPosition;
        private DuProperty m_Size;

        private DuProperty m_AxisColorMode;
        private DuProperty m_ShowStartPoint;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Arrow")]
        public static void AddComponentToSelectedObjects()
        {
            AddGizmoToSelectedOrNewObject(typeof(ArrowGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Direction = FindProperty("m_Direction", "Direction");
            m_StartPosition = FindProperty("m_StartPosition", "Position Offset");
            m_Size = FindProperty("m_Size", "Size");

            m_AxisColorMode = FindProperty("m_AxisColorMode", "Axis Color Mode");
            m_ShowStartPoint = FindProperty("m_ShowStartPoint", "Show Start Point");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_Direction);

            int selectedIndex = Popup("Direction Predefined", 0, s_DirectionOptions);

            if (selectedIndex > 0)
            {
                Vector3 newDirection;

                switch (selectedIndex)
                {
                    default:
                    case 1: newDirection = Vector3.up; break;
                    case 2: newDirection = Vector3.down; break;
                    case 3: newDirection = Vector3.left; break;
                    case 4: newDirection = Vector3.right; break;
                    case 5: newDirection = Vector3.forward; break;
                    case 6: newDirection = Vector3.back; break;
                }

                foreach (var entity in GetSerializedEntitiesByTargets())
                {
                    entity.serializedObject.FindProperty("m_Direction").vector3Value = newDirection;
                    entity.serializedObject.ApplyModifiedProperties();
                }
            }

            if (m_Direction.valVector3.Equals(Vector3.zero))
                DustGUI.HelpBoxInfo("Direction cannot be ZERO vector");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            Space();

            PropertyField(m_StartPosition);
            PropertyExtendedSlider(m_Size, 0.01f, 10f, 0.01f);

            Space();

            PropertyField(m_AxisColorMode);
            PropertyFieldOrHide(m_Color, (ArrowGizmo.AxisColorMode) m_AxisColorMode.valInt != ArrowGizmo.AxisColorMode.Custom);
            PropertyField(m_GizmoVisibility);
            PropertyField(m_ShowStartPoint);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
