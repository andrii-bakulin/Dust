using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    public abstract class SpaceObjectFieldEditor : SpaceFieldEditor
    {
        protected DuProperty m_Unlimited;
        
        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Unlimited = FindProperty("m_Unlimited", "Unlimited");
        }
    }
}
