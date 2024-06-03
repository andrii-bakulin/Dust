using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(FactoryInstance))]
    // [CanEditMultipleObjects] -> Cannot!
    public class FactoryInstanceEditor : DuEditor
    {
        protected DuProperty m_UpdatePosition;
        protected DuProperty m_UpdateRotation;
        protected DuProperty m_UpdateScale;

        protected DuProperty m_Index;
        protected DuProperty m_Offset;
        protected DuProperty m_RandomScalar;
        protected DuProperty m_RandomVector;
        protected DuProperty m_MaterialReferences;

        protected DuProperty m_ParentFactory;
        protected DuProperty m_PrevInstance;
        protected DuProperty m_NextInstance;

        protected DuProperty m_OnInstanceUpdate;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Factory/Support/Factory Instance")]
        [MenuItem("GameObject/Dust/Factory/Support/Factory Instance")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedObjects(typeof(FactoryInstance));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_UpdatePosition = FindProperty("m_UpdatePosition", "Update Position");
            m_UpdateRotation = FindProperty("m_UpdateRotation", "Update Rotation");
            m_UpdateScale = FindProperty("m_UpdateScale", "Update Scale");

            m_Index = FindProperty("m_Index", "Index");
            m_Offset = FindProperty("m_Offset", "Offset");
            m_RandomScalar = FindProperty("m_RandomScalar", "Random Scalar");
            m_RandomVector = FindProperty("m_RandomVector", "Random Vector");
            m_MaterialReferences = FindProperty("m_MaterialReferences");

            m_ParentFactory = FindProperty("m_ParentFactory", "Parent Factory");
            m_PrevInstance = FindProperty("m_PrevInstance", "Previous Instance");
            m_NextInstance = FindProperty("m_NextInstance", "Next Instance");

            m_OnInstanceUpdate = FindProperty("m_OnInstanceUpdate", "On Instance Update");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            FactoryInstance mainScript = target as FactoryInstance;

            bool IsFreeInstance = Dust.IsNull(mainScript.parentFactory);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_UpdatePosition);
            PropertyField(m_UpdateRotation);
            PropertyField(m_UpdateScale);
            Space();

            PropertyField(m_OnInstanceUpdate);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (!IsFreeInstance)
            {
                PropertyFieldOrLock(m_Index, true);
                PropertyFieldOrLock(m_Offset, true);
                PropertyFieldOrLock(m_RandomScalar, true);
                PropertyFieldOrLock(m_RandomVector, true);
                Space();

                PropertyFieldOrLock(m_ParentFactory, true);
                PropertyFieldOrLock(m_PrevInstance, true);
                PropertyFieldOrLock(m_NextInstance, true);
                Space();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // @DUST.todo: make control more then one element

            if (IsFreeInstance)
            {
                if (m_MaterialReferences.property.arraySize == 0)
                {
                    if (DustGUI.Button("Add Material Reference"))
                    {
                        m_MaterialReferences.property.InsertArrayElementAtIndex(0);

                        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                        // Set default values

                        var matRef = mainScript.GetDefaultMaterialReference();

                        var item = m_MaterialReferences.property.GetArrayElementAtIndex(0);

                        DuProperty m_MeshRenderer      = FindProperty(item, "m_MeshRenderer");
                        DuProperty m_ValuePropertyName = FindProperty(item, "m_ValuePropertyName");
                        DuProperty m_ColorPropertyName = FindProperty(item, "m_ColorPropertyName");
                        DuProperty m_UvwPropertyName   = FindProperty(item, "m_UvwPropertyName");

                        m_MeshRenderer.property.objectReferenceValue = matRef.meshRenderer;
                        m_ValuePropertyName.property.stringValue = matRef.valuePropertyName;
                        m_ColorPropertyName.property.stringValue = matRef.colorPropertyName;
                        m_UvwPropertyName.property.stringValue = matRef.uvwPropertyName;
                    }
                }
                else
                {
                    if (DustGUI.Button("Remove Material Reference"))
                    {
                        m_MaterialReferences.property.DeleteArrayElementAtIndex(0);
                    }
                }
            }

            for (int i = 0; i < m_MaterialReferences.property.arraySize; i++)
            {
                var item = m_MaterialReferences.property.GetArrayElementAtIndex(i);

                DuProperty m_MeshRenderer      = FindProperty(item, "m_MeshRenderer", "Mesh Renderer");
                DuProperty m_ValuePropertyName = FindProperty(item, "m_ValuePropertyName", "Value");
                DuProperty m_ColorPropertyName = FindProperty(item, "m_ColorPropertyName", "Color");
                DuProperty m_UvwPropertyName   = FindProperty(item, "m_UvwPropertyName", "UVW");
                DuProperty m_OriginalMaterial  = FindProperty(item, "m_OriginalMaterial", "Original Material");

                DustGUI.Header("Reference #" + (i + 1));
                PropertyFieldOrLock(m_MeshRenderer, !IsFreeInstance);
                PropertyFieldOrLock(m_ColorPropertyName, !IsFreeInstance);
                PropertyFieldOrLock(m_ValuePropertyName, !IsFreeInstance);
                PropertyFieldOrLock(m_UvwPropertyName, !IsFreeInstance);

                if (!IsFreeInstance)
                {
                    Space();
                    PropertyFieldOrLock(m_OriginalMaterial, true);
                }

                Space();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (!IsFreeInstance && Dust.IsNotNull(mainScript.stateDynamic))
            {
                if (DustGUI.FoldoutBegin("Dynamic Final State", "FactoryInstance.DynamicStates"))
                {
                    DustGUI.Lock();
                    DustGUI.Field("Position", mainScript.stateDynamic.position.duToRound(3));
                    DustGUI.Field("Rotation", mainScript.stateDynamic.rotation.duToRound(3));
                    DustGUI.Field("Scale", mainScript.stateDynamic.scale.duToRound(3));
                    Space();
                    DustGUI.Field("Value", mainScript.stateDynamic.value);
                    DustGUI.Field("Color", mainScript.stateDynamic.color);
                    DustGUI.Field("Color Values", mainScript.stateDynamic.color.duToVector3(2));
                    DustGUI.Field("Color RGB", (mainScript.stateDynamic.color * 255).duToVector3Int());
                    DustGUI.Field("UVW", mainScript.stateDynamic.uvw.duToRound(3));
                    DustGUI.Unlock();
                }
                DustGUI.FoldoutEnd();
            }

            if (!IsFreeInstance && Dust.IsNotNull(mainScript.stateDynamicPrevious))
            {
                if (DustGUI.FoldoutBegin("Dynamic Previous State", "FactoryInstance.DynamicStatesPrevious", false))
                {
                    DustGUI.Lock();
                    DustGUI.Field("Position", mainScript.stateDynamicPrevious.position.duToRound(3));
                    DustGUI.Field("Rotation", mainScript.stateDynamicPrevious.rotation.duToRound(3));
                    DustGUI.Field("Scale", mainScript.stateDynamicPrevious.scale.duToRound(3));
                    Space();
                    DustGUI.Field("Value", mainScript.stateDynamicPrevious.value);
                    DustGUI.Field("Color", mainScript.stateDynamicPrevious.color);
                    DustGUI.Field("Color Values", mainScript.stateDynamicPrevious.color.duToVector3(2));
                    DustGUI.Field("Color RGB", (mainScript.stateDynamicPrevious.color * 255).duToVector3Int());
                    DustGUI.Field("UVW", mainScript.stateDynamicPrevious.uvw.duToRound(3));
                    DustGUI.Unlock();
                }
                DustGUI.FoldoutEnd();
            }

            if (!IsFreeInstance && Dust.IsNotNull(mainScript.stateZero))
            {
                if (DustGUI.FoldoutBegin("Default State", "FactoryInstance.DefaultStates", false))
                {
                    DustGUI.Lock();
                    DustGUI.Field("Position", mainScript.stateZero.position.duToRound(3));
                    DustGUI.Field("Rotation", mainScript.stateZero.rotation.duToRound(3));
                    DustGUI.Field("Scale", mainScript.stateZero.scale.duToRound(3));
                    Space();
                    DustGUI.Field("Value", mainScript.stateZero.value);
                    DustGUI.Field("Color", mainScript.stateZero.color);
                    DustGUI.Field("Color Values", mainScript.stateZero.color.duToVector3(2));
                    DustGUI.Field("Color RGB", (mainScript.stateZero.color * 255).duToVector3Int());
                    DustGUI.Field("UVW", mainScript.stateZero.uvw.duToRound(3));
                    DustGUI.Unlock();
                }
                DustGUI.FoldoutEnd();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
