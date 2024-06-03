using UnityEditor;

namespace Dust.DustEditor
{
    public abstract class ClampTransformEditor : DuEditor
    {
        protected DuProperty m_ClampModeX;
        protected DuProperty m_ClampMinX;
        protected DuProperty m_ClampMaxX;

        protected DuProperty m_ClampModeY;
        protected DuProperty m_ClampMinY;
        protected DuProperty m_ClampMaxY;

        protected DuProperty m_ClampModeZ;
        protected DuProperty m_ClampMinZ;
        protected DuProperty m_ClampMaxZ;

        //--------------------------------------------------------------------------------------------------------------
        
        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ClampModeX = FindProperty("m_ClampModeX", "Axis X");
            m_ClampMinX = FindProperty("m_ClampMinX", "Min");
            m_ClampMaxX = FindProperty("m_ClampMaxX", "Max");

            m_ClampModeY = FindProperty("m_ClampModeY", "Axis Y");
            m_ClampMinY = FindProperty("m_ClampMinY", "Min");
            m_ClampMaxY = FindProperty("m_ClampMaxY", "Max");

            m_ClampModeZ = FindProperty("m_ClampModeZ", "Axis Z");
            m_ClampMinZ = FindProperty("m_ClampMinZ", "Min");
            m_ClampMaxZ = FindProperty("m_ClampMaxZ", "Max");
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void OnInspectorGUI_Main()
        {
            var clampModeX = (ClampMode)m_ClampModeX.valInt;
            var clampModeY = (ClampMode)m_ClampModeY.valInt;
            var clampModeZ = (ClampMode)m_ClampModeZ.valInt;

            PropertyField(m_ClampModeX);

            if (clampModeX != ClampMode.NoClamp)
            {
                DustGUI.IndentLevelInc();
                    
                if (clampModeX is ClampMode.MinOnly or ClampMode.MinAndMax)
                    PropertyField(m_ClampMinX);

                if (clampModeX is ClampMode.MaxOnly or ClampMode.MinAndMax)
                    PropertyField(m_ClampMaxX);
        
                DustGUI.IndentLevelDec();
                Space();
            }

            PropertyField(m_ClampModeY);

            if (clampModeY != ClampMode.NoClamp)
            {
                DustGUI.IndentLevelInc();
                    
                if (clampModeY is ClampMode.MinOnly or ClampMode.MinAndMax)
                    PropertyField(m_ClampMinY);

                if (clampModeY is ClampMode.MaxOnly or ClampMode.MinAndMax)
                    PropertyField(m_ClampMaxY);
        
                DustGUI.IndentLevelDec();
                Space();
            }

            PropertyField(m_ClampModeZ);

            if (clampModeZ != ClampMode.NoClamp)
            {
                DustGUI.IndentLevelInc();
                    
                if (clampModeZ is ClampMode.MinOnly or ClampMode.MinAndMax)
                    PropertyField(m_ClampMinZ);

                if (clampModeZ is ClampMode.MaxOnly or ClampMode.MinAndMax)
                    PropertyField(m_ClampMaxZ);
        
                DustGUI.IndentLevelDec();
            }
        }
    }
}