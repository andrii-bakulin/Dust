using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dust
{
    [AddComponentMenu("Dust/Factory/Support/Factory Instance")]
    [DisallowMultipleComponent]
    public class FactoryInstance : DuMonoBehaviour
    {
        [Serializable]
        public class InstanceUpdateEvent : UnityEvent<FactoryInstance>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [System.Serializable]
        public class State
        {
            public Vector3 position { get; set; } = Vector3.zero;
            public Vector3 rotation { get; set; } = Vector3.zero;
            public Vector3 scale { get; set; } = Vector3.one;

            public float value { get; set; }
            public Color color { get; set; } = Color.magenta;
            public Vector3 uvw { get; set; } = Vector3.zero;

            public State Clone()
            {
                var clone = new State
                {
                    position = this.position,
                    rotation = this.rotation,
                    scale    = this.scale,

                    value    = this.value,
                    color    = this.color,
                    uvw      = this.uvw,
                };
                return clone;
            }

            public void CopyFrom(State t)
            {
                this.position = t.position;
                this.rotation = t.rotation;
                this.scale    = t.scale;

                this.value    = t.value;
                this.color    = t.color;
                this.uvw      = t.uvw;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        [Serializable]
        public class MaterialReference
        {
            [SerializeField]
            private MeshRenderer m_MeshRenderer;
            public MeshRenderer meshRenderer
            {
                get => m_MeshRenderer;
                set => m_MeshRenderer = value;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            [SerializeField]
            private string m_ValuePropertyName = "";
            public string valuePropertyName
            {
                get => m_ValuePropertyName;
                set => m_ValuePropertyName = value;
            }

            [SerializeField]
            private string m_ColorPropertyName = "_Color";
            public string colorPropertyName
            {
                get => m_ColorPropertyName;
                set => m_ColorPropertyName = value;
            }

            [SerializeField]
            private string m_UvwPropertyName = "";
            public string uvwPropertyName
            {
                get => m_UvwPropertyName;
                set => m_UvwPropertyName = value;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            [SerializeField]
            private Material m_OriginalMaterial;
            public Material originalMaterial
            {
                get => m_OriginalMaterial;
                set
                {
                    if (Dust.IsNotNull(value))
                    {
                        if (Dust.IsNull(m_OriginalMaterial))
                            m_OriginalMaterial = value;
                    }
                    else
                        m_OriginalMaterial = null;
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected bool m_UpdatePosition = true;
        public bool updatePosition
        {
            get => m_UpdatePosition;
            set => m_UpdatePosition = value;
        }

        [SerializeField]
        protected bool m_UpdateRotation = true;
        public bool updateRotation
        {
            get => m_UpdateRotation;
            set => m_UpdateRotation = value;
        }

        [SerializeField]
        protected bool m_UpdateScale = true;
        public bool updateScale
        {
            get => m_UpdateScale;
            set => m_UpdateScale = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        internal Factory m_ParentFactory;
        public Factory parentFactory => m_ParentFactory;

        [SerializeField]
        private FactoryInstance m_PrevInstance;
        public FactoryInstance prevInstance => m_PrevInstance;

        [SerializeField]
        private FactoryInstance m_NextInstance;
        public FactoryInstance nextInstance => m_NextInstance;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Index;
        public int index => m_Index;

        // Instance offset in total instances sequence.
        // Value in range [0..1]
        // First instance is 0.0
        // Last instance is 1.0
        [SerializeField]
        private float m_Offset;
        public float offset => m_Offset;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // This random values generated once on create Factory Instance.
        // If factory will regenerate instances in future with the same inner parameters,
        // then this values will be also the same

        [SerializeField]
        private float m_RandomScalar;
        public float randomScalar => m_RandomScalar;

        [SerializeField]
        private Vector3 m_RandomVector;
        public Vector3 randomVector => m_RandomVector;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private State m_StateZero = new State();
        public State stateZero => m_StateZero;

        private readonly State m_StateDynamic = new State();
        public State stateDynamic => m_StateDynamic;

        private readonly State m_StateDynamicPrevious = new State();
        public State stateDynamicPrevious => m_StateDynamicPrevious;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Material reference(s)

        // Why I use this bool flag and not check ..if (Dust.IsNull(m_MaterialReference))..
        // Because if developer enable debug mode from inspector and click on factory instance
        // Then Inspector auto-create m_MaterialReference object.
        // But it'll have from values and will not be inserted to references array
        private bool m_MaterialReferenceLinked;

        // No need to serialize field.
        // It's just link to m_MaterialReferences[0]
        private MaterialReference m_MaterialReference;
        public MaterialReference materialReference
        {
            get
            {
                if (!m_MaterialReferenceLinked)
                {
                    if (materialReferences.Count == 0)
                        materialReferences.Add(GetDefaultMaterialReference());

                    m_MaterialReference = materialReferences[0];
                    m_MaterialReferenceLinked = true;
                }

                return m_MaterialReference;
            }
        }

        // @DUST.todo: Now I use only 1st element of the array as main reference to material.
        // But in future I can add more references for few MeshRender:Materials + specific params
        [SerializeField]
        private List<MaterialReference> m_MaterialReferences;
        private List<MaterialReference> materialReferences
        {
            get
            {
                if (Dust.IsNull(m_MaterialReferences))
                    m_MaterialReferences = new List<MaterialReference>();

                return m_MaterialReferences;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private InstanceUpdateEvent m_OnInstanceUpdate;
        public InstanceUpdateEvent onInstanceUpdate => m_OnInstanceUpdate;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Vector3 m_DefaultPrefabScale;
        public Vector3 defaultPrefabScale => m_DefaultPrefabScale;

        //--------------------------------------------------------------------------------------------------------------

        public void Initialize(Factory factory, int initIndex, float initOffset, float initRandomScalar, Vector3 initRandomVector)
        {
            m_ParentFactory = factory;
            m_Index = initIndex;
            m_Offset = initOffset;
            m_RandomScalar = initRandomScalar;
            m_RandomVector = initRandomVector;

            m_DefaultPrefabScale = this.transform.localScale;
        }

        internal void SetPrevNextInstances(FactoryInstance prevFactoryInstance, FactoryInstance nextFactoryInstance)
        {
            m_PrevInstance = prevFactoryInstance;
            m_NextInstance = nextFactoryInstance;
        }

        public void SetDefaultState(State state)
        {
            m_StateZero.CopyFrom(state);
        }

        //--------------------------------------------------------------------------------------------------------------

        private bool m_DidApplyMaterialUpdatesBefore;
        private bool m_DidApplyMaterialUpdatesLastIteration;

        internal void ResetDynamicStateToZeroState()
        {
            m_StateDynamicPrevious.CopyFrom(m_StateDynamic);
            m_StateDynamic.CopyFrom(m_StateZero);

            m_DidApplyMaterialUpdatesLastIteration = false;
        }

        internal void ApplyDynamicStateToObject()
        {
            onInstanceUpdate?.Invoke(this);

            if (updatePosition)
                transform.localPosition = m_StateDynamic.position;

            if (updateRotation)
                transform.localEulerAngles = m_StateDynamic.rotation;

            if (updateScale)
                transform.localScale = m_StateDynamic.scale;

            if (m_DidApplyMaterialUpdatesBefore && !m_DidApplyMaterialUpdatesLastIteration)
            {
                var matRef = materialReference;

                if( Dust.IsNotNull(matRef.originalMaterial))
                {
                    matRef.meshRenderer.sharedMaterial = matRef.originalMaterial;
                    matRef.originalMaterial = null;
                }

                m_DidApplyMaterialUpdatesBefore = false;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        internal void ApplyMaterialUpdatesToObject(float intensity)
        {
            var matRef = materialReference;

            if (Dust.IsNull(matRef.meshRenderer))
                return;

            Material material;

            if (Dust.IsNull(matRef.originalMaterial))
            {
                if (Dust.IsNull(matRef.meshRenderer.sharedMaterial))
                    return;

                matRef.originalMaterial = matRef.meshRenderer.sharedMaterial;

                material = new Material(matRef.originalMaterial);
                material.name += " (Clone)";
                material.hideFlags = HideFlags.DontSave;

                // Creating clone of material
                matRef.meshRenderer.sharedMaterial = material;
            }
            else
            {
                material = matRef.meshRenderer.sharedMaterial;
            }

            if (Dust.IsNull(material))
                return; // This may happen when you move factory to the prefab

            if (!string.IsNullOrEmpty(matRef.valuePropertyName))
                material.SetFloat(matRef.valuePropertyName, stateDynamic.value * intensity);

            if (!string.IsNullOrEmpty(matRef.colorPropertyName))
                material.SetColor(matRef.colorPropertyName, stateDynamic.color * intensity);

            if (!string.IsNullOrEmpty(matRef.uvwPropertyName))
                material.SetVector(matRef.uvwPropertyName, stateDynamic.uvw * intensity);

            m_DidApplyMaterialUpdatesBefore = true;
            m_DidApplyMaterialUpdatesLastIteration = true;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public MaterialReference GetDefaultMaterialReference()
        {
            var matRef = new MaterialReference();

            matRef.meshRenderer = GetComponent<MeshRenderer>();

            if (Dust.IsNull(matRef.meshRenderer))
                matRef.meshRenderer = GetComponentInChildren<MeshRenderer>();

            return matRef;
        }
    }
}
