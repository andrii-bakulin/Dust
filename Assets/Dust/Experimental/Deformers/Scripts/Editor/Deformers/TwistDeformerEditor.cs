using UnityEngine;
using UnityEditor;

namespace Dust.Experimental.Deformers.Editor
{
    [CustomEditor(typeof(TwistDeformer))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TwistDeformerEditor : AbstractDeformerEditor
    {
        private DuProperty m_DeformMode;
        private DuProperty m_Size;
        private DuProperty m_Angle;
        private DuProperty m_Direction;

        //--------------------------------------------------------------------------------------------------------------

        static TwistDeformerEditor()
        {
            DeformersPopupButtons.AddDeformer(typeof(TwistDeformer), "Twist");
        }

        [MenuItem("Dust/* Experimental/Deformers/Twist Deformer")]
        [MenuItem("GameObject/Dust/* Experimental/Deformers/Twist Deformer")]
        public static void AddComponent()
        {
            AddDeformerComponentByType(typeof(TwistDeformer));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_DeformMode = FindProperty("m_DeformMode", "Deform Mode");
            m_Size = FindProperty("m_Size", "Size");
            m_Angle = FindProperty("m_Angle", "Angle");
            m_Direction = FindProperty("m_Direction", "Direction");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_Size);
            PropertyExtendedSlider(m_Angle, -360f, 360f, 1f);
            PropertyField(m_DeformMode);
            PropertyField(m_Direction);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_FieldsMap();
            OnInspectorGUI_GizmoBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Size.experimentalIsChanged)
                m_Size.valVector3 = TwistDeformer.Normalizer.Size(m_Size.valVector3);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Require forced redraw scene view

            DustGUI.ForcedRedrawSceneView();
        }
    }
}
