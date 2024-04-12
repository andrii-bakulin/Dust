using Dust.DustEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SessionState = Dust.DustEditor.SessionState;

namespace Dust.Experimental.Deformers.Editor
{
    [CustomEditor(typeof(MeshDeformer))]
    public class MeshDeformerEditor : DustEditor.DuEditor
    {
        private const float CELL_WIDTH_ICON = 32f;
        private const float CELL_WIDTH_STATE = 20f;
        private const float CELL_WIDTH_INTENSITY = 54f;

        //--------------------------------------------------------------------------------------------------------------

        private DuProperty m_Deformers;
        private DuProperty m_MeshOriginal;

        private DuProperty m_RecalculateBounds;
        private DuProperty m_RecalculateNormals;
        private DuProperty m_RecalculateTangents;
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private readonly Dictionary<string, Rect> m_RectsUI = new();

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/* Experimental/Deformers/Mesh Deformer")]
        [MenuItem("GameObject/Dust/* Experimental/Deformers/Mesh Deformer")]
        public static void AddComponent()
        {
            AddComponentToSelectedOrNewObject("Mesh Deformer", typeof(MeshDeformer));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Deformers = FindProperty("m_Deformers", "Deformers");
            m_MeshOriginal = FindProperty("m_MeshOriginal", "Mesh (Original)");
            
            m_RecalculateBounds = FindProperty("m_RecalculateBounds", "Recalculate Bounds");
            m_RecalculateNormals = FindProperty("m_RecalculateNormals", "Recalculate Normals");
            m_RecalculateTangents = FindProperty("m_RecalculateTangents", "Recalculate Tangents");
        }

        public override void OnInspectorGUI()
        {
            var main = target as MeshDeformer;

            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (Dust.IsNull(main.meshOriginal))
            {
                if (Dust.IsNotNull(main.meshFilter.sharedMesh) && !main.meshFilter.sharedMesh.isReadable)
                {
                    DustGUI.HelpBoxError("Mesh is not readable." + "\n" + "Enabled \"Read/Write Enabled\" flag for this mesh.");
                }
                else if (main.enabled)
                {
                    main.ReEnableMeshForDeformer();
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Deformers", "MeshDeformer.Deformers"))
            {
                OptimizeDeformersArray();

                Vector2 scrollPosition = SessionState.GetVector3("MeshDeformer.Deformers.ScrollPosition", target, Vector2.zero);
                float totalHeight = 24 + 36 * Mathf.Clamp(m_Deformers.experimentalProperty.arraySize + 1, 4, 8) + 16;

                int indentLevel = DustGUI.IndentLevelReset(); // Because it'll try to add left-spacing when draw deformers
                Rect rect = DustGUI.BeginVerticalBox();
                DustGUI.BeginScrollView(ref scrollPosition, 0, totalHeight);
                {
                    for (int i = 0; i < m_Deformers.experimentalProperty.arraySize; i++)
                    {
                        SerializedProperty item = m_Deformers.experimentalProperty.GetArrayElementAtIndex(i);

                        if (DrawDeformerItem(item, i, m_Deformers.experimentalProperty.arraySize))
                        {
                            m_Deformers.experimentalIsChanged = true;
                            EditorUtility.SetDirty(this);
                            break; // stop update anything... in next update it will redraw real state
                        }
                    }

                    DrawAddDeformerButton();
                }
                DustGUI.EndScrollView();
                DustGUI.EndVertical();
                DustGUI.IndentLevelReset(indentLevel);

                Space();

                // PropertyField(m_Deformers);
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
                        AddDeformersFromObjectsList(DragAndDrop.objectReferences);
                        Event.current.Use();
                    }
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                SessionState.SetVector3("MeshDeformer.Deformers.ScrollPosition", target, scrollPosition);
            }
            DustGUI.FoldoutEnd();

            if (m_MeshOriginal.ObjectReferenceExists)
            {
                DustGUI.Lock();
                PropertyField(m_MeshOriginal);
                DustGUI.Unlock();
                Space();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_RecalculateBounds);
            PropertyField(m_RecalculateNormals);
            PropertyField(m_RecalculateTangents);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            int verticesCount = main.GetMeshVerticesCount();

            if (verticesCount >= 0)
            {
                Space();
                DustGUI.HelpBoxInfo("Mesh has " + verticesCount + " vertices");
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            string statsInfo = "STATS:" + "\n";
            statsInfo += "Updates count: " + main.stats.updatesCount + "\n";
            statsInfo += "Last update: " + main.stats.lastUpdateTime + " sec";

            DustGUI.HelpBoxWarning(statsInfo);
            this.Repaint();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Require forced redraw scene view

            DustGUI.ForcedRedrawSceneView();
        }

        private MeshDeformer.Record UnpackDeformerRecord(SerializedProperty item)
        {
            var record = new MeshDeformer.Record();
            record.enabled = item.FindPropertyRelative("m_Enabled").boolValue;
            record.deformer = item.FindPropertyRelative("m_Deformer").objectReferenceValue as AbstractDeformer;
            record.intensity = item.FindPropertyRelative("m_Intensity").floatValue;
            return record;
        }

        private bool DrawDeformerItem(SerializedProperty item, int itemIndex, int itemsCount)
        {
            var curRecord = UnpackDeformerRecord(item); // just to save previous state
            var newRecord = UnpackDeformerRecord(item); // this record will fix changes

            if (Dust.IsNull(newRecord.deformer))
                return false; // Notice: but it should never be this way!

            bool clickOnDelete;
            bool clickOnMoveUp;
            bool clickOnMoveDw;

            DustGUI.BeginHorizontal();
            {
                var deformerEnabledInScene = newRecord.deformer.enabled &&
                                             newRecord.deformer.gameObject.activeInHierarchy;

                var deformerIcon = UI.Icons.GetTextureByComponent(newRecord.deformer, !deformerEnabledInScene ? "Disabled" : "");

                if (DustGUI.IconButton(deformerIcon, CELL_WIDTH_ICON, CELL_WIDTH_ICON, ExtraList.styleMiniButton))
                    Selection.activeGameObject = newRecord.deformer.gameObject;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                var btnStateIcon = newRecord.enabled ? UI.Icons.STATE_ENABLED : UI.Icons.STATE_DISABLED;

                if (DustGUI.IconButton(btnStateIcon, CELL_WIDTH_STATE, 32f, ExtraList.styleMiniButton))
                    newRecord.enabled = !newRecord.enabled;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (!newRecord.enabled)
                    DustGUI.Lock();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                var deformerName = new GUIContent(newRecord.deformer.DeformerName(), newRecord.deformer.gameObject.name);
                var deformerHint = newRecord.deformer.DeformerDynamicHint();

                if (deformerHint != "")
                {
                    DustGUI.BeginVertical();
                    {
                        DustGUI.SimpleLabel(deformerName, 0, 14);
                        DustGUI.SimpleLabel(deformerHint, 0, 10, ExtraList.styleHintLabel);
                    }
                    DustGUI.EndVertical();
                }
                else
                {
                    DustGUI.SimpleLabel(deformerName, 0, DustGUI.Config.ICON_BUTTON_HEIGHT);
                }

                DustGUI.SpaceExpand();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                string intensityValue = newRecord.intensity.ToString("F2");

                if (DustGUI.Button(intensityValue, CELL_WIDTH_INTENSITY, 20f, ExtraList.styleIntensityButton, DustGUI.ButtonState.Pressed))
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
                m_Deformers.experimentalProperty.DeleteArrayElementAtIndex(itemIndex);
                return true;
            }

            if (clickOnMoveUp) {
                m_Deformers.experimentalProperty.MoveArrayElement(itemIndex, itemIndex - 1);
                return true;
            }

            if (clickOnMoveDw) {
                m_Deformers.experimentalProperty.MoveArrayElement(itemIndex, itemIndex + 1);
                return true;
            }

            return false;
        }

        private void DrawAddDeformerButton()
        {
            DustGUI.BeginHorizontal();
            {
                if (DustGUI.IconButton(UI.Icons.ADD_DEFORMER, CELL_WIDTH_ICON, CELL_WIDTH_ICON, ExtraList.styleMiniButton))
                    PopupWindow.Show(m_RectsUI["Add"], DeformersPopupButtons.Popup(this));

                if (Event.current.type == EventType.Repaint)
                    m_RectsUI["Add"] = GUILayoutUtility.GetLastRect();

                DustGUI.Label("Add Deformer", 0, DustGUI.Config.ICON_BUTTON_HEIGHT);
            }
            DustGUI.EndHorizontal();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void AddDeformersFromObjectsList(Object[] objectReferences)
        {
            if (Dust.IsNull(objectReferences) || objectReferences.Length == 0)
                return;

            foreach (Object obj in objectReferences)
            {
                if (obj is GameObject)
                {
                    AbstractDeformer[] deformers = (obj as GameObject).GetComponents<AbstractDeformer>();

                    foreach (var deformer in deformers)
                    {
                        AddDeformer(deformer);
                    }
                }
                else if (obj is AbstractDeformer)
                {
                    AddDeformer(obj as AbstractDeformer);
                }
            }
        }

        private void AddDeformer(AbstractDeformer deformer)
        {
            int count = m_Deformers.experimentalProperty.arraySize;

            for (int i = 0; i < count; i++)
            {
                SerializedProperty record = m_Deformers.experimentalProperty.GetArrayElementAtIndex(i);

                if (Dust.IsNull(record))
                    continue;

                SerializedProperty refObject = record.FindPropertyRelative("m_Deformer");

                if (Dust.IsNull(refObject) || !refObject.objectReferenceValue.Equals(deformer))
                    continue;

                return; // No need to insert 2nd time
            }

            m_Deformers.experimentalProperty.InsertArrayElementAtIndex(count);

            var defaultRec = new FieldsMap.FieldRecord();

            SerializedProperty newRecord = m_Deformers.experimentalProperty.GetArrayElementAtIndex(count);

            newRecord.FindPropertyRelative("m_Enabled").boolValue = defaultRec.enabled;
            newRecord.FindPropertyRelative("m_Deformer").objectReferenceValue = deformer;
            newRecord.FindPropertyRelative("m_Intensity").floatValue = defaultRec.intensity;

            serializedObject.ApplyModifiedProperties();
        }

        private void OptimizeDeformersArray()
        {
            EditorHelper.OptimizeObjectReferencesArray(ref m_Deformers, "m_Deformer");
        }
    }
}
