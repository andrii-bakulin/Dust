using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dust.Experimental.Deformers
{
    [AddComponentMenu("Dust/* Experimental/Deformers/Mesh Deformer")]
    [RequireComponent(typeof(MeshFilter))]
    [ExecuteInEditMode]
    public class MeshDeformer : DuMonoBehaviour, IDynamicState
    {
        [System.Serializable]
        public class Record
        {
            [SerializeField]
            private AbstractDeformer m_Deformer = null;
            public AbstractDeformer deformer
            {
                get => m_Deformer;
                set => m_Deformer = value;
            }

            [SerializeField]
            private float m_Intensity = 1f;
            public float intensity
            {
                get => m_Intensity;
                set => m_Intensity = value;
            }

            [SerializeField]
            private bool m_Enabled = true;
            public bool enabled
            {
                get => m_Enabled;
                set => m_Enabled = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        public class Stats
        {
            public int updatesCount { get; internal set; }
            public float lastUpdateTime { get; internal set; }
        }

        public readonly Stats stats = new Stats();
#endif

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Mesh m_MeshOriginal;
        public Mesh meshOriginal => m_MeshOriginal;

        [SerializeField]
        private List<Record> m_Deformers = new();
        public List<Record> deformers => m_Deformers;

        [SerializeField]
        private bool m_RecalculateBounds = true;
        public bool recalculateBounds
        {
            get => m_RecalculateBounds;
            set => m_RecalculateBounds = value;
        }

        [SerializeField]
        private bool m_RecalculateNormals = true;
        public bool recalculateNormals
        {
            get => m_RecalculateNormals;
            set => m_RecalculateNormals = value;
        }

        [SerializeField]
        private bool m_RecalculateTangents = true;
        public bool recalculateTangents
        {
            get => m_RecalculateTangents;
            set => m_RecalculateTangents = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private int m_LastDynamicStateHashCode = 0;
        public int lastDynamicStateHashCode => m_LastDynamicStateHashCode;

        //--------------------------------------------------------------------------------------------------------------

        private MeshFilter m_MeshFilter;
        public MeshFilter meshFilter
        {
            get
            {
                if (Dust.IsNull(m_MeshFilter))
                    m_MeshFilter = GetComponent<MeshFilter>();

                return m_MeshFilter;
            }
        }

        private Vector3[] m_MeshPointsOriginal = null;
        private Vector3[] m_MeshPointsDeformed = null;

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
#if UNITY_EDITOR
            if (isInEditorMode)
            {
                EditorUpdateReset();

                EditorApplication.update -= EditorUpdate;
                EditorApplication.update += EditorUpdate;
            }
#endif
            EnableMeshForDeformer();
            UpdateMeshPoints(0f);
        }

        void OnDisable()
        {
#if UNITY_EDITOR
            if (isInEditorMode)
            {
                EditorApplication.update -= EditorUpdate;
            }
#endif
            DisableMeshForDeformer();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void EnableMeshForDeformer()
        {
            bool hasMeshAtFilter = Dust.IsNotNull(meshFilter.sharedMesh);
            bool hasMeshAtDeformer = Dust.IsNotNull(m_MeshOriginal);

            if (!hasMeshAtFilter && !hasMeshAtDeformer)
                return; // Both links have no meshes at all...

            // Save link to original mesh
            if (hasMeshAtFilter && !hasMeshAtDeformer)
            {
                if (!meshFilter.sharedMesh.isReadable)
                {
#if UNITY_EDITOR
                    Dust.Debug.Warning("GameObject [" + gameObject.name + "] has not readable Mesh");
#endif
                    return;
                }

                m_MeshOriginal = meshFilter.sharedMesh;

                meshFilter.hideFlags = HideFlags.NotEditable;
            }

            // @notice: after next call I can modify sharedMesh without involve to real sharedMesh
            //          and will no have any error in console in editor mode
            Mesh meshClone = Instantiate(m_MeshOriginal);
            meshClone.hideFlags = HideFlags.DontSave;
            meshClone.name = m_MeshOriginal.name + " (Deformed)";
            meshClone.MarkDynamic();
            meshFilter.sharedMesh = meshClone;

            m_MeshPointsOriginal = m_MeshOriginal.vertices;
            m_MeshPointsDeformed = new Vector3[m_MeshPointsOriginal.Length];

            UpdateMeshPoints(0f);
        }

        private void DisableMeshForDeformer()
        {
            m_MeshPointsDeformed = null;
            m_MeshPointsOriginal = null;

            if (Dust.IsNotNull(meshFilter))
            {
                meshFilter.hideFlags = HideFlags.None;

                if (Dust.IsNotNull(m_MeshOriginal))
                {
                    meshFilter.sharedMesh = m_MeshOriginal;
                    m_MeshOriginal = null;
                }

                m_MeshFilter = null;
            }

            m_LastDynamicStateHashCode = 0;
        }

        public void ReEnableMeshForDeformer()
        {
            DisableMeshForDeformer();
            EnableMeshForDeformer();
            UpdateMeshPoints(0f);
        }

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
#if UNITY_EDITOR
            if (isInEditorMode) return;
#endif

            UpdateMeshPoints(Time.deltaTime);
        }

#if UNITY_EDITOR
        void EditorUpdate()
        {
            float deltaTime;

            if (!EditorUpdateTick(out deltaTime))
                return;

            UpdateMeshPoints(deltaTime);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public Record AddDeformer(AbstractDeformer deformer)
        {
            var deformerRecord = new Record
            {
                deformer = deformer,
                enabled = true,
                intensity = 1f,
            };

            deformers.Add(deformerRecord);

            return deformerRecord;
        }

        //--------------------------------------------------------------------------------------------------------------
        // IDynamicState

        public int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DynamicState.Append(ref dynamicState, ++seq, transform);
            DynamicState.Append(ref dynamicState, ++seq, meshOriginal);
            DynamicState.Append(ref dynamicState, ++seq, recalculateBounds);
            DynamicState.Append(ref dynamicState, ++seq, recalculateNormals);
            DynamicState.Append(ref dynamicState, ++seq, recalculateTangents);

            if (Dust.IsNotNull(m_Deformers))
            {
                foreach (var deformerRecord in m_Deformers)
                {
                    // @WARNING!!! require sync code in: GetDynamicStateHashCode() + UpdateMeshPoints()

                    if (Dust.IsNull(deformerRecord) || !deformerRecord.enabled || DuMath.IsZero(deformerRecord.intensity))
                        continue;

                    if (Dust.IsNull(deformerRecord.deformer))
                        continue;

                    if (!deformerRecord.deformer.enabled || !deformerRecord.deformer.gameObject.activeInHierarchy)
                        continue;

                    // @END

                    DynamicState.Append(ref dynamicState, ++seq, deformerRecord.intensity);
                    DynamicState.Append(ref dynamicState, ++seq, deformerRecord.deformer);
                }
            }

            return DynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void UpdateMeshPoints(float deltaTime)
        {
            if (Dust.IsNull(m_MeshPointsOriginal) || m_MeshPointsOriginal.Length == 0)
                return;

            int newDynamicStateHash = GetDynamicStateHashCode();

            if (newDynamicStateHash != 0 && m_LastDynamicStateHashCode == newDynamicStateHash)
                return;

#if UNITY_EDITOR
            var timer = Dust.Debug.StartTimer();
#endif

            m_MeshPointsOriginal.CopyTo(m_MeshPointsDeformed, 0);

            if (Dust.IsNotNull(m_Deformers))
            {
                foreach (var deformerRecord in m_Deformers)
                {
                    // @WARNING!!! require sync code in: GetDynamicStateHashCode() + UpdateMeshPoints()

                    if (Dust.IsNull(deformerRecord) || !deformerRecord.enabled || DuMath.IsZero(deformerRecord.intensity))
                        continue;

                    if (Dust.IsNull(deformerRecord.deformer))
                        continue;

                    if (!deformerRecord.deformer.enabled || !deformerRecord.deformer.gameObject.activeInHierarchy)
                        continue;

                    // @END

                    deformerRecord.deformer.UpdateMeshPointsCloud(ref m_MeshPointsDeformed, transform, deformerRecord.intensity);
                }
            }

            meshFilter.sharedMesh.vertices = m_MeshPointsDeformed;

            if (recalculateBounds)
                meshFilter.sharedMesh.RecalculateBounds();

            if (recalculateNormals)
                meshFilter.sharedMesh.RecalculateNormals();

            if (recalculateTangents)
                meshFilter.sharedMesh.RecalculateTangents();

#if UNITY_EDITOR
            stats.updatesCount++;
            stats.lastUpdateTime = timer.Stop();
#endif

            m_LastDynamicStateHashCode = newDynamicStateHash;
        }

        //--------------------------------------------------------------------------------------------------------------

        public int GetMeshVerticesCount()
        {
            if (Dust.IsNull(m_MeshOriginal))
                return -1;

            return m_MeshOriginal.vertexCount;
        }
    }
}
