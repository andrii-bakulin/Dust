using Dust.DustEditor;
using UnityEngine;
using UnityEditor;

namespace Dust.Experimental.Deformers.Editor
{
    public class DeformersPopupButtons : PopupButtons
    {
        private MeshDeformerEditor m_DeformMesh;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddDeformer(System.Type type, string title)
        {
            AddEntity("Deformers", type, title);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static PopupButtons Popup(MeshDeformerEditor deformMesh)
        {
            var popup = new DeformersPopupButtons();
            popup.m_DeformMesh = deformMesh;

            GenerateColumn(popup, "Deformers", "Deformers");

            return popup;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override bool OnButtonClicked(CellRecord cellRecord)
        {
            AbstractDeformerEditor.AddDeformerComponentByType((m_DeformMesh.target as DuMonoBehaviour).gameObject, cellRecord.type);
            return true;
        }
    }
}
