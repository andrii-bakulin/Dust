using UnityEngine;

namespace DustEngine
{
    public class MeshRendererTintUpdater : TintAction.TintUpdater
    {
        protected MeshRenderer m_Target;

        protected Material m_OriginalMaterial;
        protected Material m_TintMaterial;

        //----------------------------------------------------------------------------------------------------------

        public static TintAction.TintUpdater Create(TintAction parentTintAction)
        {
            var target = parentTintAction.activeTargetObject.GetComponent<MeshRenderer>();

            if (Dust.IsNull(target))
                return null;

            var tintUpdater = new MeshRendererTintUpdater();
            tintUpdater.m_Target = target;
            tintUpdater.Init(parentTintAction);
            return tintUpdater;
        }
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override void Init(TintAction parentTintAction)
        {
            base.Init(parentTintAction);
            
            m_OriginalMaterial = m_Target.sharedMaterial;
        
            m_TintMaterial = new Material(m_OriginalMaterial);
            m_TintMaterial.hideFlags = HideFlags.DontSave;

            if (!m_TintMaterial.name.Contains("(Clone)"))
                m_TintMaterial.name += " (Clone)";

            m_Target.sharedMaterial = m_TintMaterial;

            m_StartColor = m_TintMaterial.GetColor(tintAction.propertyName);
        }

        public override void Update(float deltaTime, Color color)
        {
            m_TintMaterial.SetColor(tintAction.propertyName, color);
        }

        public override void Release(bool isActionTerminated)
        {
            if (!isActionTerminated && tintAction.playRollback)
                m_Target.sharedMaterial = m_OriginalMaterial;
        
            m_OriginalMaterial = null;
            m_TintMaterial = null;
            
            base.Release(isActionTerminated);
        }
    }
}
