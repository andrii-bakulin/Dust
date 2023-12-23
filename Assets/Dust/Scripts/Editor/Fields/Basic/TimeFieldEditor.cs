using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(TimeField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class TimeFieldEditor : FieldEditor
    {
        protected DuProperty m_TimeMode;
        protected DuProperty m_TimeScale;
        protected DuProperty m_Offset;

        protected RemappingEditor m_RemappingEditor;

        //--------------------------------------------------------------------------------------------------------------

        static TimeFieldEditor()
        {
            FieldsPopupButtons.AddBasicField(typeof(TimeField), "Time");
        }

        [MenuItem("Dust/Fields/Basic Fields/Time")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(TimeField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_TimeMode = FindProperty("m_TimeMode", "Mode");
            m_TimeScale = FindProperty("m_TimeScale", "Time Scale");
            m_Offset = FindProperty("m_Offset", "Offset");

            m_RemappingEditor = new RemappingEditor((target as TimeField).remapping, serializedObject.FindProperty("m_Remapping"));
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                PropertyField(m_TimeMode);
                PropertyExtendedSlider(m_TimeScale, 0f, 10f, 0.01f);
                PropertyExtendedSlider(m_Offset, 0f, 1f, 0.01f);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Generate preview

                var timeMode = (TimeField.TimeMode) m_TimeMode.valInt;
                var timeScale = m_TimeScale.valFloat;
                var offset = m_Offset.valFloat;

                var previewDynamic = true;
                var previewLength = SessionState.GetFloat("TimeField.Preview.Length", 3f);
                var previewTitle = "Preview (" + previewLength.ToString("F1") + " sec)";

                // Small trick for "None" timeMode: it's always show be "Linear"
                if (timeMode == TimeField.TimeMode.Linear)
                {
                    timeScale = 1f;
                    offset = 0f;

                    previewDynamic = false;
                    previewLength = 1f;
                    previewTitle = "Preview";
                }

                AnimationCurve curve = new AnimationCurve();

                for (int i = 0; i < 200; i++)
                {
                    float innerOffset = i / 200f;
                    float globalOffset = (innerOffset + offset) * timeScale * previewLength;
                    float value = (target as TimeField).GetPowerByTimeMode(timeMode, globalOffset);

                    curve.AddKey(new Keyframe(innerOffset, value) {weightedMode = WeightedMode.Both});
                }

                Space();

                DustGUI.Lock();
                DustGUI.Field(previewTitle, curve, 0, 30, Color.green, new Rect(0f, 0f, 1f, 1f));
                DustGUI.Unlock();

                if (previewDynamic)
                {
                    previewLength = DustGUI.ExtraSlider.Create(1f, 5f, 0.1f, 1f, 100f).Draw("Preview Length", previewLength);
                    SessionState.SetFloat("TimeField.Preview.Length", previewLength);
                }

                Space();
            }
            DustGUI.FoldoutEnd();

            m_RemappingEditor.OnInspectorGUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
