using DustEngine.DustEditor.UI;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class ActionEditor : DuEditor
    {
        protected DuProperty m_AutoStart;

        protected DuProperty m_TargetMode;
        protected DuProperty m_TargetObject;

        //--------------------------------------------------------------------------------------------------------------

        protected Action.TargetMode targetMode => (Action.TargetMode) m_TargetMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_AutoStart = FindProperty("m_AutoStart", "Auto Start");

            m_TargetMode = FindProperty("m_TargetMode", "Target Mode (?)",
                "If the action has Inherited Target Mode and that action starts the animation then for Target Object will be used SELF object.");
            
            m_TargetObject = FindProperty("m_TargetObject", "Target Object");
        }

        //--------------------------------------------------------------------------------------------------------------

        private Rect m_RectsAddButton;

        protected virtual void OnInspectorGUI_BaseControlUI()
        {
            if (targets.Length != 1)
                return;

            var isAutoStart = m_AutoStart.IsTrue;
            var action = target as Action;

            if (Dust.IsNull(action))
                return;
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            string iconName;
            string iconTitle;

            if (Application.isPlaying)
            {
                iconName  = action.isPlaying ? Icons.ACTION_PLAY : Icons.ACTION_IDLE;
                iconTitle = action.isPlaying ? "Playing" : "Idle";
            }
            else
            {
                iconName  = isAutoStart ? Icons.ACTION_PLAY : Icons.ACTION_IDLE;
                iconTitle = isAutoStart ? "Auto Start" : "Idle";
            }
            
            if (action as IntervalAction is IntervalAction intervalAction)
            {
                if (intervalAction.repeatMode == IntervalAction.RepeatMode.Repeat)
                {
                    if (Application.isPlaying && action.isPlaying)
                        iconTitle += $" ({intervalAction.playbackIndex+1}/{intervalAction.repeatTimes})";
                    else
                        iconTitle += $", Repeat {intervalAction.repeatTimes}x";
                }
                else if (intervalAction.repeatMode == IntervalAction.RepeatMode.RepeatForever)
                {
                    iconTitle += ", Repeat Forever";
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            DustGUI.BeginHorizontal();
            {
                if (DustGUI.IconButton(iconName))
                {
                    if (Application.isPlaying)
                    {
                        if (!action.isPlaying)
                            action.Play();
                        else
                            action.Stop();
                    }
                    else
                    {
                        m_AutoStart.valBool = !m_AutoStart.valBool;
                        m_AutoStart.isChanged = true;
                    }
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                DustGUI.BeginVertical();
                {
                    DustGUI.SimpleLabel(iconTitle);

                    var rect = EditorGUILayout.GetControlRect(false, 8f);
                    EditorGUI.ProgressBar(rect, GetActionPercentsDone(action), "");
                }
                DustGUI.EndVertical();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                DustGUI.Lock();
                DustGUI.IconButton(Icons.ACTION_NEXT, DustGUI.Config.ICON_BUTTON_WIDTH * 0.75f, DustGUI.Config.ICON_BUTTON_HEIGHT);
                DustGUI.Unlock();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (action as SequencedAction is SequencedAction sequencedAction)
                {
                    foreach (var nextAction in sequencedAction.onCompleteActions)
                    {
                        if (Dust.IsNull(nextAction))
                            continue;

                        Texture icon = Icons.GetTextureByComponent(nextAction);
                        DustGUI.IconButton(icon);
                    }
                }

                if (DustGUI.IconButton(Icons.ADD_ACTION))
                    PopupWindow.Show(m_RectsAddButton, ActionsPopupButtons.Popup(action));

                if (Event.current.type == EventType.Repaint)
                    m_RectsAddButton = GUILayoutUtility.GetLastRect();

                // @todo: make in future catch "Drag-n-Drop" event with other action in __this__ object and append it
                //        too the actions list
            }
            DustGUI.EndHorizontal();

            Space();
            
            if (Application.isPlaying)
                DustGUI.ForcedRedrawInspector(this);
        }

        protected float GetActionPercentsDone(Action action)
        {
            if (action as IntervalWithRollbackAction is IntervalWithRollbackAction intervalWithRollbackAction)
            {
                if (intervalWithRollbackAction.playingPhase == IntervalWithRollbackAction.PlayingPhase.Main)
                    return intervalWithRollbackAction.playbackState;
                
                if (intervalWithRollbackAction.playingPhase == IntervalWithRollbackAction.PlayingPhase.Rollback)
                    return 1f - intervalWithRollbackAction.playbackState;

                return 0f;
            }
            
            if (action as IntervalAction is IntervalAction intervalAction)
                return intervalAction.playbackState;

            return 0f; // For InstantAction <or> others > return 0f
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected virtual void OnInspectorGUI_Extended(string actionId)
        {
            if (DustGUI.FoldoutBegin("Extended", actionId + ".Extended", this, false))
            {
                OnInspectorGUI_Extended_BlockFirst();
                OnInspectorGUI_Extended_BlockMiddle();
                OnInspectorGUI_Extended_BlockLast();
            }
            DustGUI.FoldoutEnd();
        }

        protected virtual void OnInspectorGUI_Extended_BlockFirst()
        {
            PropertyField(m_AutoStart);
        }

        protected virtual void OnInspectorGUI_Extended_BlockMiddle()
        {
        }

        protected virtual void OnInspectorGUI_Extended_BlockLast()
        {
            PropertyField(m_TargetMode);
            PropertyFieldOrHide(m_TargetObject, targetMode != Action.TargetMode.GameObject);

            if (targetMode == Action.TargetMode.Inherited && m_AutoStart.valBool)
            {
                DustGUI.HelpBoxInfo("Start Target Object will be used as SELF");
            }
        }
    }
}
