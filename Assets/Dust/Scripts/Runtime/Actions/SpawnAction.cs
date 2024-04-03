using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Spawn Action")]
    public class SpawnAction : InstantAction
    {
        public enum IterateMode
        {
            Iterate = 0,
            Random = 1,
        }

        public enum SpawnPointMode
        {
            Self = 0,
            Points = 1,
            SphereVolume = 2,
            BoxVolume = 3
        }

        public enum SpawnParentMode
        {
            Spawner = 0,
            SpawnPoint = 1,
            World = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private SpawnPointMode m_SpawnPointMode = SpawnPointMode.Self;
        public SpawnPointMode spawnPointMode
        {
            get => m_SpawnPointMode;
            set => m_SpawnPointMode = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private List<GameObject> m_SpawnPoints = new List<GameObject>();
        public List<GameObject> spawnPoints => m_SpawnPoints;

        [SerializeField]
        private IterateMode m_SpawnPointsIterate = IterateMode.Iterate;
        public IterateMode spawnPointsIterate
        {
            get => m_SpawnPointsIterate;
            set => m_SpawnPointsIterate = value;
        }

        [SerializeField]
        private int m_SpawnPointsIteration;
        public int spawnPointsIteration
        {
            get => m_SpawnPointsIteration;
            set => m_SpawnPointsIteration = value;
        }

        [SerializeField]
        private int m_SpawnPointsSeed;
        public int spawnPointsSeed
        {
            get => m_SpawnPointsSeed;
            set => m_SpawnPointsSeed = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        
        [SerializeField]
        private float m_SphereVolumeRadius = 0.5f;
        public float sphereVolumeRadius
        {
            get => m_SphereVolumeRadius;
            set => m_SphereVolumeRadius = NormalizeSphereVolumeRadius(value);
        }
        
        [SerializeField]
        private Vector3 m_BoxVolumeSize = Vector3.one;
        public Vector3 boxVolumeSize
        {
            get => m_BoxVolumeSize;
            set => m_BoxVolumeSize = NormalizeBoxVolumeSize(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private List<GameObject> m_SpawnObjects = new List<GameObject>();
        public List<GameObject> spawnObjects => m_SpawnObjects;

        [SerializeField]
        private IterateMode m_SpawnObjectsIterate = IterateMode.Iterate;
        public IterateMode spawnObjectsIterate
        {
            get => m_SpawnObjectsIterate;
            set => m_SpawnObjectsIterate = value;
        }

        [SerializeField]
        private int m_SpawnObjectsIteration;
        public int spawnObjectsIteration
        {
            get => m_SpawnObjectsIteration;
            set => m_SpawnObjectsIteration = value;
        }

        [SerializeField]
        private int m_SpawnObjectsSeed;
        public int spawnObjectsSeed
        {
            get => m_SpawnObjectsSeed;
            set => m_SpawnObjectsSeed = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_MultipleSpawnEnabled;
        public bool multipleSpawnEnabled
        {
            get => m_MultipleSpawnEnabled;
            set => m_MultipleSpawnEnabled = value;
        }

        [SerializeField]
        private DuIntRange m_MultipleSpawnCount = DuIntRange.oneToFive;
        public DuIntRange multipleSpawnCount
        {
            get => m_MultipleSpawnCount;
            set => m_MultipleSpawnCount = NormalizeMultipleSpawnCount(value);
        }

        [SerializeField]
        private int m_MultipleSpawnSeed;
        public int multipleSpawnSeed
        {
            get => m_MultipleSpawnSeed;
            set => m_MultipleSpawnSeed = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ActivateInstance;
        public bool activateInstance
        {
            get => m_ActivateInstance;
            set => m_ActivateInstance = value;
        }

        [SerializeField]
        private SpawnParentMode m_ParentMode = SpawnParentMode.Spawner;
        public SpawnParentMode parentMode
        {
            get => m_ParentMode;
            set => m_ParentMode = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ResetRotation;
        public bool resetRotation
        {
            get => m_ResetRotation;
            set => m_ResetRotation = value;
        }

        [SerializeField]
        private bool m_ResetScale;
        public bool resetScale
        {
            get => m_ResetScale;
            set => m_ResetScale = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuRandom m_SpawnPointsRandom;
        private DuRandom spawnPointsRandom => m_SpawnPointsRandom ??= new DuRandom(spawnPointsSeed);

        private DuRandom m_SpawnObjectsRandom;
        private DuRandom spawnObjectsRandom => m_SpawnObjectsRandom ??= new DuRandom(spawnObjectsSeed);

        private DuRandom m_MultipleSpawnRandom;
        private DuRandom multipleSpawnRandom => m_MultipleSpawnRandom ??= new DuRandom(multipleSpawnSeed);

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnActionExecute()
        {
            if (multipleSpawnEnabled)
            {
                var spawnCount = multipleSpawnRandom.Range(multipleSpawnCount.min, multipleSpawnCount.max + 1);

                for (int i = 0; i < spawnCount; i++)
                {
                    SpawnSingleObject();
                }
            }
            else
            {
                SpawnSingleObject();
            }
        }

        protected GameObject SpawnSingleObject()
        {
            GameObject spawnAtPoint = null;
            GameObject objectToSpawn = null;

            Vector3 spawnOffsetPosition = Vector3.zero;

            // Detect spawn point & offset - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            switch (spawnPointMode)
            {
                case SpawnPointMode.Self:
                    spawnAtPoint = this.gameObject;
                    break;

                case SpawnPointMode.Points:
                    if (Dust.IsNull(spawnPoints) || spawnPoints.Count == 0)
                        break;

                    switch (spawnPointsIterate)
                    {
                        case IterateMode.Iterate:
                            spawnAtPoint = spawnPoints[(spawnPointsIteration++) % spawnPoints.Count];
                            break;

                        case IterateMode.Random:
                            spawnAtPoint = spawnPoints[spawnPointsRandom.Range(0, spawnPoints.Count)];
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;

                case SpawnPointMode.SphereVolume:
                    spawnAtPoint = this.gameObject;

                    spawnOffsetPosition = spawnPointsRandom.Range(-Vector3.one, Vector3.one);
                    spawnOffsetPosition.Normalize();
                    spawnOffsetPosition *= spawnPointsRandom.Range(0f, sphereVolumeRadius);
                    break;
                    
                case SpawnPointMode.BoxVolume:
                    spawnAtPoint = this.gameObject;

                    spawnOffsetPosition = spawnPointsRandom.Range(-boxVolumeSize, boxVolumeSize) / 2f;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Dust.IsNull(spawnAtPoint))
                return null;

            // Detect GameObject to spawn  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (Dust.IsNotNull(spawnObjects) && spawnObjects.Count > 0)
            {
                switch (spawnObjectsIterate)
                {
                    case IterateMode.Iterate:
                        objectToSpawn = spawnObjects[(spawnObjectsIteration++) % spawnObjects.Count];
                        break;

                    case IterateMode.Random:
                        objectToSpawn = spawnObjects[spawnObjectsRandom.Range(0, spawnObjects.Count)];
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (Dust.IsNull(objectToSpawn))
                return null;

            // Spawn - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            // 1. Create object and make parent as spawn-object
            GameObject obj = Instantiate(objectToSpawn, spawnAtPoint.transform);
            {
                obj.transform.localPosition = spawnOffsetPosition;
            }

            // 2. Change parent if need
            obj.transform.parent = parentMode switch
            {
                SpawnParentMode.Spawner    => transform,
                SpawnParentMode.SpawnPoint => spawnAtPoint.transform,
                SpawnParentMode.World      => null,
                _                          => spawnAtPoint.transform
            };

            // 3. Reset transform if need only after change parent!
            if (resetRotation)
                obj.transform.localRotation = Quaternion.identity;
            
            if (resetScale)
                obj.transform.localScale = Vector3.one;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (activateInstance)
                obj.SetActive(true);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            return obj;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.yellow;

            switch (spawnPointMode)
            {
                case SpawnPointMode.Self:
                case SpawnPointMode.Points:
                    return;

                case SpawnPointMode.SphereVolume:
                    Gizmos.DrawWireSphere(Vector3.zero, sphereVolumeRadius);
                    break;

                case SpawnPointMode.BoxVolume:
                    Gizmos.DrawWireCube(Vector3.zero, boxVolumeSize);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeSphereVolumeRadius(float radius)
        {
            return Mathf.Abs(radius);
        }

        public static Vector3 NormalizeBoxVolumeSize(Vector3 value)
        {
            return DuVector3.Abs(value);
        }

        public static DuIntRange NormalizeMultipleSpawnCount(DuIntRange range)
        {
            range.min = Mathf.Max(range.min, 0);
            range.max = Mathf.Max(range.max, range.min);
            return range;
        }
    }
}
