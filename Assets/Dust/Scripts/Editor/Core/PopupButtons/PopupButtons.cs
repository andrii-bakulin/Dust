using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class PopupButtons : PopupWindowContent
    {
        private const float BUTTON_WIDTH = 140f;
        private const float BUTTON_HEIGHT = 28f;

        //--------------------------------------------------------------------------------------------------------------

        protected class ColumnRecord
        {
            public string title;
            public List<CellRecord> cells = new List<CellRecord>();
        }

        protected struct CellRecord : IEquatable<CellRecord>
        {
            public string title;
            public Type type;

            public bool Equals(CellRecord other)
            {
                throw new NotImplementedException();
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private static Dictionary<string, Dictionary<Type, string>> m_Entities;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private List<ColumnRecord> m_ColumnRecords = new List<ColumnRecord>();
        private int m_ColsCount;
        private int m_RowsCount;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddEntity(string groupId, Type type, string title)
        {
            if (Dust.IsNull(m_Entities))
                m_Entities = new Dictionary<string, Dictionary<Type, string>>();

            if (!m_Entities.ContainsKey(groupId))
                m_Entities[groupId] = new Dictionary<Type, string>();

            m_Entities[groupId][type] = title;
        }

        protected static void GenerateColumn(PopupButtons popup, string groupId, string title)
        {
            if (Dust.IsNull(m_Entities))
                return;

            var column = new ColumnRecord
            {
                title = title
            };

            if (m_Entities.ContainsKey(groupId))
            {
                var sortedEntities = from entry in m_Entities[groupId]
                    orderby entry.Value ascending
                    select entry;

                foreach (var pair in sortedEntities)
                {
                    CellRecord button;
                    button.title = pair.Value;
                    button.type = pair.Key;
                    column.cells.Add(button);

                    popup.m_RowsCount = Mathf.Max(popup.m_RowsCount, column.cells.Count);
                }
            }

            popup.m_ColumnRecords.Add(column);
            popup.m_ColsCount = popup.m_ColumnRecords.Count;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override Vector2 GetWindowSize()
        {
            var padding = 4f;
            var titleHeight = 16f;
            var buttonPadding = 1f;

            return new Vector2(m_ColsCount * (BUTTON_WIDTH + 2f * buttonPadding) + 2f * padding,
                titleHeight + m_RowsCount * (BUTTON_HEIGHT + 2f * buttonPadding) + 2f * padding);
        }

        public override void OnGUI(Rect rect)
        {
            GUIStyle btnStyle = DustGUI.iconButtonStyle;
            btnStyle.alignment = TextAnchor.MiddleLeft;

            DustGUI.BeginHorizontal();

            foreach (var columnRecord in m_ColumnRecords)
            {
                DustGUI.BeginVertical();

                DustGUI.Header(columnRecord.title, BUTTON_WIDTH);

                foreach (var cellRecord in columnRecord.cells)
                {
                    GUIContent btnContent = new GUIContent();
                    btnContent.image = UI.Icons.GetTextureByClassName(cellRecord.type.ToString());
                    btnContent.text = cellRecord.title;

                    if (DustGUI.IconButton(btnContent, BUTTON_WIDTH, BUTTON_HEIGHT, btnStyle))
                    {
                        if (OnButtonClicked(cellRecord))
                            editorWindow.Close();
                    }
                }

                DustGUI.EndVertical();
            }

            DustGUI.EndHorizontal();
        }

        protected abstract bool OnButtonClicked(CellRecord cellRecord);
    }
}
