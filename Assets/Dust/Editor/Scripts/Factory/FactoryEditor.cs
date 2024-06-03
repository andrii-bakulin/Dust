using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    public abstract class FactoryEditor : DuEditor
    {
        protected DuProperty m_SourceObjects;
        protected DuProperty m_IterateMode;
        protected DuProperty m_Seed;

        protected DuProperty m_DefaultValue;
        protected DuProperty m_DefaultColor;
        protected DuProperty m_FactoryMachines;

        protected DuProperty m_TransformSpace;
        protected DuProperty m_TransformSequence;
        protected DuProperty m_TransformPosition;
        protected DuProperty m_TransformRotation;
        protected DuProperty m_TransformScale;
        protected DuProperty m_TransformApplyDefaultScale;

        protected DuProperty m_InstanceAccessMode;
        protected DuProperty m_InstanceTypeMode;
        protected DuProperty m_InstancesHolder;
        protected DuProperty m_InstancesFillRate;
        protected DuProperty m_InstancesFillSeed;
        protected DuProperty m_ForcedSetActive;
        protected DuProperty m_ForcedUpdateEachFrame;

        protected DuProperty m_InspectorDisplay;
        protected DuProperty m_InspectorScale;

        protected DuProperty m_AutoRebuildOnPrefabUpdated;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private readonly Dictionary<string, Rect> m_RectsUI = new Dictionary<string, Rect>();

        protected bool m_IsRequireRebuildInstances;
        protected bool m_IsRequireResetupInstances;

        //--------------------------------------------------------------------------------------------------------------

        public static void CreateFactoryByType(System.Type factoryType)
        {
            var gameObject = new GameObject();

            var factory = gameObject.AddComponent(factoryType) as Factory;

            gameObject.name = factory.FactoryName() + " Factory";
            DuTransform.Reset(gameObject.transform);

            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            Selection.activeGameObject = gameObject;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_SourceObjects = FindProperty("m_SourceObjects", "Source Objects");
            m_IterateMode = FindProperty("m_IterateMode", "Iterate");
            m_Seed = FindProperty("m_Seed", "Forced Set Active");

            m_DefaultValue = FindProperty("m_DefaultValue", "Default Value");
            m_DefaultColor = FindProperty("m_DefaultColor", "Default Color");
            m_FactoryMachines = FindProperty("m_FactoryMachines", "Factory Machines");

            m_TransformSpace = FindProperty("m_TransformSpace", "Transform Space");
            m_TransformSequence = FindProperty("m_TransformSequence", "Transform Sequence");
            m_TransformPosition = FindProperty("m_TransformPosition", "Position");
            m_TransformRotation = FindProperty("m_TransformRotation", "Rotation");
            m_TransformScale = FindProperty("m_TransformScale", "Scale");
            m_TransformApplyDefaultScale = FindProperty("m_TransformApplyDefaultScale", "Apply Source Object Scale");

            m_InstanceAccessMode = FindProperty("m_InstanceAccessMode", "Access Mode");
            m_InstanceTypeMode = FindProperty("m_InstanceTypeMode", "Type Mode");
            m_InstancesHolder = FindProperty("m_InstancesHolder", "Instances Holder");
            m_InstancesFillRate = FindProperty("m_InstancesFillRate", "Fill Rate");
            m_InstancesFillSeed = FindProperty("m_InstancesFillSeed", "Fill Seed");
            m_ForcedSetActive = FindProperty("m_ForcedSetActive", "Forced Set Active");

            m_ForcedUpdateEachFrame = FindProperty("m_ForcedUpdateEachFrame", "Forced Updates", "If TRUE, then the calculation for all instances will be execute forced each frame, even if nothing changed. Otherwise, Factory try to optimize calculations.");

            m_InspectorDisplay = FindProperty("m_InspectorDisplay", "Display");
            m_InspectorScale = FindProperty("m_InspectorScale", "Scale");

            m_AutoRebuildOnPrefabUpdated = FindProperty("m_AutoRebuildOnPrefabUpdated", "Auto Rebuild", "Auto Rebuild all factory instances on any of prefab params updated");
        }

        //--------------------------------------------------------------------------------------------------------------

        protected Factory.IterateMode iterateMode
            => (Factory.IterateMode) m_IterateMode.valInt;

        protected Factory.TransformSpace transformSpace
            => (Factory.TransformSpace) m_TransformSpace.valInt;

        //--------------------------------------------------------------------------------------------------------------

        protected override void InspectorInitStates()
        {
            base.InspectorInitStates();

            m_IsRequireRebuildInstances = m_IsRequireResetupInstances = DustGUI.IsUndoRedoPerformed();
        }

        protected override void InspectorCommitUpdates()
        {
            base.InspectorCommitUpdates();

            if (m_IsRequireRebuildInstances || m_IsRequireResetupInstances)
            {
                foreach (var subTarget in targets)
                {
                    var origin = subTarget as Factory;

                    if (m_IsRequireRebuildInstances)
                        origin.RebuildInstances();

                    if (m_IsRequireResetupInstances)
                        origin.UpdateInstancesZeroStates();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void OnInspectorGUI_SourceObjects()
        {
            PropertyField(m_SourceObjects);
            PropertyField(m_IterateMode);

            if (iterateMode == Factory.IterateMode.Random)
                PropertySeedFixed(m_Seed);

            Space();

            m_IsRequireRebuildInstances |= m_SourceObjects.isChanged;
            m_IsRequireRebuildInstances |= m_IterateMode.isChanged;
            m_IsRequireRebuildInstances |= m_Seed.isChanged;

            if (m_Seed.isChanged)
                m_Seed.valInt = Factory.NormalizeSeed(m_Seed.valInt);
        }

        protected void OnInspectorGUI_Instances()
        {
            if (DustGUI.FoldoutBegin("Instances", "Factory.Instances"))
            {
                PropertyExtendedSlider01(m_InstancesFillRate);
                if (m_InstancesFillRate.valFloat < 1f)
                    PropertySeedFixed(m_InstancesFillSeed);
                Space();

                PropertyField(m_InstanceTypeMode);
                PropertyField(m_ForcedSetActive);
                Space();

                PropertyFieldOrLock(m_InstancesHolder, targets.Length > 1);
                PropertyField(m_InstanceAccessMode);
                Space();

                PropertyField(m_ForcedUpdateEachFrame);
                Space();
            }
            DustGUI.FoldoutEnd();

            m_IsRequireRebuildInstances |= m_InstancesFillRate.isChanged;
            m_IsRequireRebuildInstances |= m_InstancesFillSeed.isChanged;
            m_IsRequireRebuildInstances |= m_InstanceTypeMode.isChanged;
            m_IsRequireRebuildInstances |= m_ForcedSetActive.isChanged;
            m_IsRequireRebuildInstances |= m_InstanceAccessMode.isChanged;
            m_IsRequireRebuildInstances |= m_InstancesHolder.isChanged;

            if (m_InstancesFillRate.isChanged)
                m_InstancesFillRate.valFloat = Factory.NormalizeInstancesFillRate(m_InstancesFillRate.valFloat);

            if (m_InstancesFillSeed.isChanged)
                m_InstancesFillSeed.valInt = Factory.NormalizeInstancesFillSeed(m_InstancesFillSeed.valInt);
        }

        protected void OnInspectorGUI_FactoryMachines()
        {
            if (DustGUI.FoldoutBegin("Factory Machines", "Factory.FactoryMachines"))
            {
                PropertyExtendedSlider(m_DefaultValue, 0f, 1f, 0.01f);
                PropertyField(m_DefaultColor);
                Space();

                DrawFactoryMachinesBlock();
            }
            DustGUI.FoldoutEnd();

            m_IsRequireResetupInstances |= m_DefaultValue.isChanged;
            m_IsRequireResetupInstances |= m_DefaultColor.isChanged;
        }

        protected void OnInspectorGUI_Transform()
        {
            if (DustGUI.FoldoutBegin("Default Transform Updates of Instances", "Factory.Transform"))
            {
                PropertyField(m_TransformPosition);
                PropertyField(m_TransformRotation);
                PropertyField(m_TransformScale);
                PropertyField(m_TransformSpace);
                PropertyField(m_TransformApplyDefaultScale);

                if (transformSpace == Factory.TransformSpace.Instance)
                    PropertyField(m_TransformSequence);

                Space();
            }
            DustGUI.FoldoutEnd();

            m_IsRequireResetupInstances |= m_TransformPosition.isChanged;
            m_IsRequireResetupInstances |= m_TransformRotation.isChanged;
            m_IsRequireResetupInstances |= m_TransformScale.isChanged;
            m_IsRequireResetupInstances |= m_TransformSpace.isChanged;
            m_IsRequireResetupInstances |= m_TransformSequence.isChanged;
            m_IsRequireResetupInstances |= m_TransformApplyDefaultScale.isChanged;
        }

        protected void OnInspectorGUI_Gizmos()
        {
            if (DustGUI.FoldoutBegin("Gizmos", "Factory.Gizmos"))
            {
                PropertyField(m_InspectorDisplay);
                PropertyExtendedSlider(m_InspectorScale, 0.5f, 3f, 0.01f, 0.01f);
                Space();
            }
            DustGUI.FoldoutEnd();
        }

        protected void OnInspectorGUI_Tools()
        {
            if (DustGUI.FoldoutBegin("Tools", "Factory.Tools", false))
            {
                var main = target as Factory;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Stats

                string statsInfo = "STATS:" + "\n";
                statsInfo += "Updates count: " + main.stats.updatesCount + "\n";
                statsInfo += "Last update: " + main.stats.lastUpdateTime + " sec";

                DustGUI.HelpBoxWarning(statsInfo);
                this.Repaint();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (DustGUI.Button("Rebuild Instances"))
                    m_IsRequireRebuildInstances |= true;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                PropertyField(m_AutoRebuildOnPrefabUpdated);
            }
            DustGUI.FoldoutEnd();
        }

        //--------------------------------------------------------------------------------------------------------------

        void DrawFactoryMachinesBlock()
        {
            if (targets.Length > 1)
            {
                DustGUI.BeginVerticalBox(0, 32 * 3f);
                DustGUI.Label("Cannot edit machines for multiple factories", 0, DustGUI.Config.ICON_BUTTON_HEIGHT, Color.gray);
                DustGUI.EndVertical();
                return;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OptimizeFactoryMachinesArray();

            Vector2 scrollPosition = SessionState.GetVector3("Factory.FactoryMachine.ScrollPosition", target, Vector2.zero);
            float totalHeight = 36 * Mathf.Clamp(m_FactoryMachines.property.arraySize + 1, 4, 8) + 16;

            int indentLevel = DustGUI.IndentLevelReset(); // Because it'll try to add left-spacing when draw fields
            Rect rect = DustGUI.BeginVerticalBox();
            DustGUI.BeginScrollView(ref scrollPosition, 0, totalHeight);
            {
                for (int i = 0; i < m_FactoryMachines.property.arraySize; i++)
                {
                    SerializedProperty item = m_FactoryMachines.property.GetArrayElementAtIndex(i);

                    if (DrawFactoryMachineItem(item, i, m_FactoryMachines.property.arraySize))
                    {
                        m_FactoryMachines.isChanged = true;
                        EditorUtility.SetDirty(this);
                        break; // stop update anything... in next update it will redraw real state
                    }
                }

                DrawAddFactoryMachineButton();
            }
            DustGUI.EndScrollView();
            DustGUI.EndVertical();
            DustGUI.IndentLevelReset(indentLevel);

            Space();

            // PropertyField(m_FactoryMachines);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (rect.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.DragPerform)
                {
                    AddFactoryMachineFromObjectsList(DragAndDrop.objectReferences);
                    Event.current.Use();
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            SessionState.SetVector3("Factory.FactoryMachine.ScrollPosition", target, scrollPosition);
        }

        private FactoryMachine.Record UnpackFactoryMachineRecord(SerializedProperty item)
        {
            var record = new FactoryMachine.Record();
            record.factoryMachine = item.FindPropertyRelative("m_FactoryMachine").objectReferenceValue as FactoryMachine;
            record.intensity = item.FindPropertyRelative("m_Intensity").floatValue;
            record.enabled = item.FindPropertyRelative("m_Enabled").boolValue;
            return record;
        }

        private bool DrawFactoryMachineItem(SerializedProperty item, int itemIndex, int itemsCount)
        {
            var curRecord = UnpackFactoryMachineRecord(item); // just to save previous state
            var newRecord = UnpackFactoryMachineRecord(item); // this record will fix changes

            if (Dust.IsNull(newRecord.factoryMachine))
                return false; // Notice: but it should never be this way!

            bool clickOnDelete;
            bool clickOnMoveUp;
            bool clickOnMoveDw;

            DustGUI.BeginHorizontal();
            {
                var machineEnabledInScene = newRecord.factoryMachine.enabled &&
                                            newRecord.factoryMachine.gameObject.activeInHierarchy;

                var machineIcon = UI.Icons.GetTextureByComponent(newRecord.factoryMachine, !machineEnabledInScene ? "Disabled" : "");

                if (DustGUI.IconButton(machineIcon, UI.CELL_WIDTH_ICON, UI.CELL_WIDTH_ICON, ExtraList.styleMiniButton))
                    Selection.activeGameObject = newRecord.factoryMachine.gameObject;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                var btnStateIcon = newRecord.enabled ? UI.Icons.STATE_ENABLED : UI.Icons.STATE_DISABLED;

                if (DustGUI.IconButton(btnStateIcon, UI.CELL_WIDTH_STATE, 32f, ExtraList.styleMiniButton))
                    newRecord.enabled = !newRecord.enabled;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (!newRecord.enabled)
                    DustGUI.Lock();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                var machineName = newRecord.factoryMachine.gameObject.name;
                var machineHint = newRecord.factoryMachine.hint;
                var dynamicHint = newRecord.factoryMachine.FactoryMachineDynamicHint();

                if (dynamicHint != "")
                    machineHint += (machineHint != "" ? ", " : "" ) + dynamicHint;

                if (machineHint != "")
                {
                    DustGUI.BeginVertical();
                    {
                        DustGUI.SimpleLabel(machineName, 0, 14);
                        DustGUI.SimpleLabel(machineHint, 0, 10, ExtraList.styleHintLabel);
                    }
                    DustGUI.EndVertical();
                }
                else
                {
                    DustGUI.SimpleLabel(machineName, 0, DustGUI.Config.ICON_BUTTON_HEIGHT);
                }

                DustGUI.SpaceExpand();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                string intensityValue = newRecord.intensity.ToString("F2");

                if (DustGUI.Button(intensityValue, UI.CELL_WIDTH_INTENSITY, 20f, ExtraList.styleIntensityButton, DustGUI.ButtonState.Pressed))
                {
                    Rect buttonRect = m_RectsUI["item" + itemIndex.ToString()];
                    buttonRect.y += 5f;

                    PopupWindow.Show(buttonRect, PopupExtraSlider.Create(serializedObject, "Intensity", item.FindPropertyRelative("m_Intensity")));
                }

                if (Event.current.type == EventType.Repaint)
                    m_RectsUI["item" + itemIndex.ToString()] = GUILayoutUtility.GetLastRect();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (!newRecord.enabled)
                    DustGUI.Unlock();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                clickOnDelete = DustGUI.IconButton(UI.Icons.DELETE, 20, 32, ExtraList.styleMiniButton);

                DustGUI.BeginVertical(20);
                {
                    DustGUI.ButtonState stateUp = itemIndex > 0 ? DustGUI.ButtonState.Normal : DustGUI.ButtonState.Locked;
                    DustGUI.ButtonState stateDw = itemIndex < itemsCount - 1 ? DustGUI.ButtonState.Normal : DustGUI.ButtonState.Locked;

                    clickOnMoveUp = DustGUI.IconButton(DustGUI.Config.RESOURCE_ICON_ARROW_UP, 20, 16, ExtraList.styleMiniButton, stateUp);
                    clickOnMoveDw = DustGUI.IconButton(DustGUI.Config.RESOURCE_ICON_ARROW_DOWN, 20, 16, ExtraList.styleMiniButton, stateDw);
                }
                DustGUI.EndVertical();
            }
            DustGUI.EndHorizontal();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Actions

            if (curRecord.enabled != newRecord.enabled) {
                item.FindPropertyRelative("m_Enabled").boolValue = newRecord.enabled;
                return true;
            }

            if (!curRecord.intensity.Equals(newRecord.intensity)) {
                item.FindPropertyRelative("m_Intensity").floatValue = newRecord.intensity;
                return true;
            }

            if (clickOnDelete) {
                m_FactoryMachines.property.DeleteArrayElementAtIndex(itemIndex);
                return true;
            }

            if (clickOnMoveUp) {
                m_FactoryMachines.property.MoveArrayElement(itemIndex, itemIndex - 1);
                return true;
            }

            if (clickOnMoveDw) {
                m_FactoryMachines.property.MoveArrayElement(itemIndex, itemIndex + 1);
                return true;
            }

            return false;
        }

        private void DrawAddFactoryMachineButton()
        {
            DustGUI.BeginHorizontal();
            {
                if (DustGUI.IconButton(UI.Icons.ADD_FACTORY_MACHINE, UI.CELL_WIDTH_ICON, UI.CELL_WIDTH_ICON, ExtraList.styleMiniButton))
                    PopupWindow.Show(m_RectsUI["Add"], FactoryMachinesPopupButtons.Popup(this));

                if (Event.current.type == EventType.Repaint)
                    m_RectsUI["Add"] = GUILayoutUtility.GetLastRect();

                DustGUI.Label("Add Factory Machine", 0, DustGUI.Config.ICON_BUTTON_HEIGHT);
            }
            DustGUI.EndHorizontal();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private void AddFactoryMachineFromObjectsList(Object[] objectReferences)
        {
            if (Dust.IsNull(objectReferences) || objectReferences.Length == 0)
                return;

            foreach (Object obj in objectReferences)
            {
                if (obj is GameObject)
                {
                    FactoryMachine[] factoryMachines = (obj as GameObject).GetComponents<FactoryMachine>();

                    foreach (var factoryMachine in factoryMachines)
                    {
                        AddFactoryMachine(factoryMachine);
                    }
                }
                else if (obj is FactoryMachine)
                {
                    AddFactoryMachine(obj as FactoryMachine);
                }
            }
        }

        private void AddFactoryMachine(FactoryMachine factoryMachine)
        {
            int count = m_FactoryMachines.property.arraySize;

            for (int i = 0; i < count; i++)
            {
                SerializedProperty record = m_FactoryMachines.property.GetArrayElementAtIndex(i);

                if (Dust.IsNull(record))
                    continue;

                SerializedProperty refObject = record.FindPropertyRelative("m_FactoryMachine");

                if (Dust.IsNull(refObject) || !refObject.objectReferenceValue.Equals(factoryMachine))
                    continue;

                return; // No need to insert 2nd time
            }

            m_FactoryMachines.property.InsertArrayElementAtIndex(count);

            var defaultRec = new FactoryMachine.Record();

            SerializedProperty newRecord = m_FactoryMachines.property.GetArrayElementAtIndex(count);
            newRecord.FindPropertyRelative("m_FactoryMachine").objectReferenceValue = factoryMachine;
            newRecord.FindPropertyRelative("m_Intensity").floatValue = defaultRec.intensity;
            newRecord.FindPropertyRelative("m_Enabled").boolValue = defaultRec.enabled;

            serializedObject.ApplyModifiedProperties();
        }

        private void OptimizeFactoryMachinesArray()
        {
            EditorHelper.OptimizeObjectReferencesArray(ref m_FactoryMachines, "m_FactoryMachine");
        }
    }
}
