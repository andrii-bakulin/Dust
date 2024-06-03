using System;
using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(Destroyer))]
    [CanEditMultipleObjects]
    public class DestroyerEditor : DuEditor
    {
        protected DuProperty m_DestroyMode;

        protected DuProperty m_Timeout;
        protected DuProperty m_TimeoutRange;

        protected DuProperty m_VolumeCenterMode;
        protected DuProperty m_VolumeCenter;
        protected DuProperty m_VolumeOffset;
        protected DuProperty m_VolumeSize;
        protected DuProperty m_VolumeSourceCenter;

        protected DuProperty m_SelfDestroy;
        protected DuProperty m_GameObjects;
        protected DuProperty m_Components;

        protected DuProperty m_OnDestroy;

        private Destroyer.DestroyMode destroyMode => (Destroyer.DestroyMode) m_DestroyMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Helpers/Destroyer")]
        [MenuItem("GameObject/Dust/Helpers/Destroyer")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Destroyer", typeof(Destroyer));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_DestroyMode = FindProperty("m_DestroyMode", "Destroy Mode");

            m_Timeout = FindProperty("m_Timeout", "Timeout");
            m_TimeoutRange = FindProperty("m_TimeoutRange", "Timeout Range");

            m_VolumeCenterMode = FindProperty("m_VolumeCenterMode", "Volume Center At");
            m_VolumeCenter = FindProperty("m_VolumeCenter", "Center");
            m_VolumeOffset = FindProperty("m_VolumeOffset", "Offset");
            m_VolumeSize = FindProperty("m_VolumeSize", "Size");
            m_VolumeSourceCenter = FindProperty("m_VolumeSourceCenter", "Center Source Object");

            m_SelfDestroy = FindProperty("m_SelfDestroy", "Self Destroy");
            m_GameObjects = FindProperty("m_GameObjects", "Destroy Game Objects");
            m_Components = FindProperty("m_Components", "Destroy Components");

            m_OnDestroy = FindProperty("m_OnDestroy", "On Destroy");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_DestroyMode);

            switch (destroyMode)
            {
                case Destroyer.DestroyMode.Manual:
                    // Nothing need to show here
                    break;

                case Destroyer.DestroyMode.Time:
                    PropertyDurationField(m_Timeout);
                    break;

                case Destroyer.DestroyMode.TimeRange:
                    PropertyFieldDurationRange(m_TimeoutRange);
                    break;

                case Destroyer.DestroyMode.AliveZone:
                case Destroyer.DestroyMode.DeadZone:
                    PropertyField(m_VolumeCenterMode);

                    switch ((Destroyer.VolumeCenterMode) m_VolumeCenterMode.valInt)
                    {
                        case Destroyer.VolumeCenterMode.StartPosition:
                            if (Application.isPlaying)
                                PropertyFieldOrLock(m_VolumeCenter, true);
                            else
                                DustGUI.StaticTextField("Center", "Self position when object appears in scene");
                            PropertyField(m_VolumeOffset);
                            PropertyField(m_VolumeSize, "Size");
                            break;

                        case Destroyer.VolumeCenterMode.FixedWorldPosition:
                            PropertyField(m_VolumeCenter);
                            PropertyField(m_VolumeOffset);
                            PropertyField(m_VolumeSize);
                            break;

                        case Destroyer.VolumeCenterMode.SourceObject:
                            PropertyField(m_VolumeSourceCenter);
                            PropertyField(m_VolumeOffset);
                            PropertyField(m_VolumeSize);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_SelfDestroy);
            Space();
            
            PropertyField(m_GameObjects);
            PropertyField(m_Components);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Events", "Destroyer.Events", false))
            {
                PropertyField(m_OnDestroy);
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            switch (destroyMode)
            {
                case Destroyer.DestroyMode.Manual:
                    DustGUI.HelpBoxInfo("To destroy GameObject or Component call one of the methods:" + "\n" +
                        "- Destroy()" + "\n" +
                        "- DestroySelf()" + "\n" + 
                        "- DestroyGameObject( .. )" + "\n" + 
                        "- DestroyComponent( .. )");
                    break;

                case Destroyer.DestroyMode.Time:
                case Destroyer.DestroyMode.TimeRange:
                    // Nothing need to show here
                    break;

                case Destroyer.DestroyMode.AliveZone:
                    DustGUI.HelpBoxInfo("GameObject will be destroyed\nwhen it will leave Alive Zone");
                    break;

                case Destroyer.DestroyMode.DeadZone:
                    DustGUI.HelpBoxInfo("GameObject will be destroyed\nwhen it will get inside Dead Zone");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (Application.isPlaying && targets.Length == 1)
            {
                if (destroyMode == Destroyer.DestroyMode.Time || destroyMode == Destroyer.DestroyMode.TimeRange)
                {
                    var main = target as Destroyer;

                    if (main.timeLimit > 0)
                    {
                        var progressBarState = Mathf.Max(1f - main.timeAlive / main.timeLimit, 0f);
                        var progressBarTitle = Mathf.Max(main.timeLimit - main.timeAlive, 0f).ToString("F1") + " sec";

                        var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                        EditorGUI.ProgressBar(rect, progressBarState, progressBarTitle);

                        DustGUI.ForcedRedrawInspector(this);
                    }
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Post Update

            if (m_VolumeSize.isChanged)
                m_VolumeSize.valVector3 = Destroyer.NormalizeVolumeSize(m_VolumeSize.valVector3);

            InspectorCommitUpdates();
        }
    }
}
