using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(WaveField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class WaveFieldEditor : SpaceFieldEditor
    {
        private DuProperty m_Amplitude;
        private DuProperty m_Size;
        private DuProperty m_LinearFalloff;
        private DuProperty m_Offset;
        private DuProperty m_AnimationSpeed;
        private DuProperty m_Direction;

        private DuProperty m_GizmoSize;
        private DuProperty m_GizmoQuality;
        private DuProperty m_GizmoAnimated;

        //--------------------------------------------------------------------------------------------------------------

        static WaveFieldEditor()
        {
            FieldsPopupButtons.Add2DField(typeof(WaveField), "Wave");
        }

        [MenuItem("Dust/Fields/2D Fields/Wave")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(WaveField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Amplitude = FindProperty("m_Amplitude", "Amplitude");
            m_Size = FindProperty("m_Size", "Size");
            m_LinearFalloff = FindProperty("m_LinearFalloff", "Linear Falloff");
            m_Offset = FindProperty("m_Offset", "Offset");
            m_AnimationSpeed = FindProperty("m_AnimationSpeed", "Animation Speed");
            m_Direction = FindProperty("m_Direction", "Direction");

            m_GizmoSize = FindProperty("m_GizmoSize", "Size");
            m_GizmoQuality = FindProperty("m_GizmoQuality", "Quality");
            m_GizmoAnimated = FindProperty("m_GizmoAnimated", "Animate in Editor");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                PropertyExtendedSlider(m_Amplitude, 0f, 10f, 0.01f);
                PropertyExtendedSlider(m_Size, 0f, 10f, 0.01f);
                PropertyExtendedSlider(m_LinearFalloff, 0f, 10f, 0.01f);
                PropertyField(m_Direction);
                Space();

                PropertyExtendedSlider(m_AnimationSpeed, -2f, +2f, 0.01f);
                PropertyExtendedSlider(m_Offset, -1f, +1f, 0.01f);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_RemappingBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // @ignore: OnInspectorGUI_GizmoBlock();

            if (DustGUI.FoldoutBegin("Gizmo", "DuAnyField.Gizmo"))
            {
                PropertyExtendedSlider(m_GizmoSize, 0.1f, 10f, 0.1f);
                PropertyField(m_GizmoQuality);
                PropertyField(m_GizmoVisibility);
                PropertyField(m_GizmoFieldColor);
                PropertyFieldOrLock(m_GizmoAnimated, DuMath.IsZero(m_AnimationSpeed.valFloat));
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();

            if (m_GizmoAnimated.isChanged)
                DustGUI.ForcedRedrawSceneView();
        }
    }
}
