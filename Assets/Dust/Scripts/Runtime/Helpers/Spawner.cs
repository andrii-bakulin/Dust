using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DustEngine
{
    [AddComponentMenu("Dust/Instances/Spawner")]
    public class Spawner : DuMonoBehaviour
    {
        public enum SpawnEvent
        {
            Manual = 0,
            FixedInterval = 1,
            IntervalInRange = 2,
        }

        public enum IterateMode
        {
            Iterate = 0,
            Random = 1,
        }

        public enum SpawnPointMode
        {
            Self = 0,
            Points = 1,
        }

        public enum SpawnParentMode
        {
            Spawner = 0,
            SpawnPoint = 1,
            World = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [System.Serializable]
        public class SpawnerEvent : UnityEvent<GameObject>
        {
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
        private int m_SpawnPointsIteration = 0;
        public int spawnPointsIteration
        {
            get => m_SpawnPointsIteration;
            set => m_SpawnPointsIteration = value;
        }

        [SerializeField]
        private int m_SpawnPointsSeed = 0;
        public int spawnPointsSeed
        {
            get => m_SpawnPointsSeed;
            set => m_SpawnPointsSeed = value;
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
        private int m_SpawnObjectsIteration = 0;
        public int spawnObjectsIteration
        {
            get => m_SpawnObjectsIteration;
            set => m_SpawnObjectsIteration = value;
        }

        [SerializeField]
        private int m_SpawnObjectsSeed = 0;
        public int spawnObjectsSeed
        {
            get => m_SpawnObjectsSeed;
            set => m_SpawnObjectsSeed = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private SpawnEvent m_SpawnEvent = SpawnEvent.FixedInterval;
        public SpawnEvent spawnEvent
        {
            get => m_SpawnEvent;
            set => m_SpawnEvent = value;
        }

        [SerializeField]
        private float m_Interval = 1f;
        public float interval
        {
            get => m_Interval;
            set => m_Interval = NormalizeIntervalValue(value);
        }

        [SerializeField]
        private DuRange m_IntervalRange = DuRange.oneToTwo;
        public DuRange intervalRange
        {
            get => m_IntervalRange;
            set
            {
                value.min = NormalizeIntervalValue(value.min);
                value.max = NormalizeIntervalValue(value.max);
                m_IntervalRange = value;
            } 
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_MultipleSpawnEnabled = false;
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
        private int m_MultipleSpawnSeed = 0;
        public int multipleSpawnSeed
        {
            get => m_MultipleSpawnSeed;
            set => m_MultipleSpawnSeed = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private SpawnParentMode m_ParentMode = SpawnParentMode.Spawner;
        public SpawnParentMode parentMode
        {
            get => m_ParentMode;
            set => m_ParentMode = value;
        }

        [SerializeField]
        private int m_Limit = 0;
        public int limit
        {
            get => m_Limit;
            set => m_Limit = NormalizeLimit(value);
        }

        [SerializeField]
        private bool m_SpawnOnAwake = false;
        public bool spawnOnAwake
        {
            get => m_SpawnOnAwake;
            set => m_SpawnOnAwake = value;
        }

        [SerializeField]
        private bool m_ResetTransform = true;
        public bool resetTransform
        {
            get => m_ResetTransform;
            set => m_ResetTransform = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private SpawnerEvent m_OnSpawn = null;
        public SpawnerEvent onSpawn => m_OnSpawn;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Count = 0;
        public int count => m_Count;

        private DuRandom m_SpawnPointsRandom;
        private DuRandom spawnPointsRandom => m_SpawnPointsRandom ??= new DuRandom(spawnPointsSeed);

        private DuRandom m_SpawnObjectsRandom;
        private DuRandom spawnObjectsRandom => m_SpawnObjectsRandom ??= new DuRandom(spawnObjectsSeed);

        private DuRandom m_MultipleSpawnRandom;
        private DuRandom multipleSpawnRandom => m_MultipleSpawnRandom ??= new DuRandom(multipleSpawnSeed);

        private DuRandom m_SpawnIntervalRandom;
        private DuRandom spawnIntervalRandom => m_SpawnIntervalRandom ??= new DuRandom((int)(intervalRange.min*123.45f + intervalRange.max*456.78f));

        private float m_SpawnTimer;
        private float m_SpawnDelay;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            if (spawnOnAwake)
                Spawn();

            m_SpawnDelay = GetDelayLimit();
        }

        private void Update()
        {
            if (spawnEvent == SpawnEvent.Manual || m_SpawnDelay < 0f)
                return;

            m_SpawnTimer += Time.deltaTime;

            if (m_SpawnTimer < m_SpawnDelay)
                return;

            Spawn();

            m_SpawnTimer = 0f;
            m_SpawnDelay = GetDelayLimit();
        }

        public void Spawn()
        {
            if (m_Limit > 0 && m_Count >= m_Limit)
                return;

            if (multipleSpawnEnabled)
            {
                int spawnCount = multipleSpawnRandom.Range(multipleSpawnCount.min, multipleSpawnCount.max + 1);

                if (m_Limit > 0)
                    spawnCount = Mathf.Min(spawnCount, m_Limit - m_Count);

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

        public GameObject SpawnSingleObject()
        {
            GameObject useSpawnPoint = null;
            GameObject useSpawnObject = null;

            // Detect spawn point

            switch (spawnPointMode)
            {
                case SpawnPointMode.Self:
                    useSpawnPoint = this.gameObject;
                    break;

                case SpawnPointMode.Points:
                    if (Dust.IsNull(spawnPoints) || spawnPoints.Count == 0)
                        break;

                    switch (spawnPointsIterate)
                    {
                        case IterateMode.Iterate:
                            useSpawnPoint = spawnPoints[(spawnPointsIteration++) % spawnPoints.Count];
                            break;

                        case IterateMode.Random:
                            useSpawnPoint = spawnPoints[spawnPointsRandom.Range(0, spawnPoints.Count)];
                            break;
                        
                        default:
                            return null;
                    }
                    break;

                default:
                    return null;
            }

            if (Dust.IsNull(useSpawnPoint))
                return null;

            // Detect GameObject to spawn

            if (Dust.IsNotNull(spawnObjects) && spawnObjects.Count > 0)
            {
                switch (spawnObjectsIterate)
                {
                    case IterateMode.Iterate:
                        useSpawnObject = spawnObjects[(spawnObjectsIteration++) % spawnObjects.Count];
                        break;

                    case IterateMode.Random:
                        useSpawnObject = spawnObjects[spawnObjectsRandom.Range(0, spawnObjects.Count)];
                        break;

                    default:
                        return null;
                }
            }

            if (Dust.IsNull(useSpawnObject) || Dust.IsNull(useSpawnPoint))
                return null;

            // Spawn

            GameObject obj = Instantiate(useSpawnObject, useSpawnPoint.transform);

            if (resetTransform)
                DuTransform.Reset(obj.transform);

            switch (parentMode)
            {
                case SpawnParentMode.Spawner:
                    obj.transform.parent = transform;
                    break;

                case SpawnParentMode.SpawnPoint:
                    obj.transform.parent = useSpawnPoint.transform;
                    break;

                case SpawnParentMode.World:
                    obj.transform.parent = null;
                    break;
            }

            m_Count++;

            if (Dust.IsNotNull(onSpawn) && onSpawn.GetPersistentEventCount() > 0)
                onSpawn.Invoke(obj);

            return obj;
        }

        private float GetDelayLimit()
        {
            float delay;

            switch (spawnEvent)
            {
                case SpawnEvent.Manual:
                    return 0f;

                case SpawnEvent.FixedInterval:
                    delay = interval;
                    break;

                case SpawnEvent.IntervalInRange:
                    delay = spawnIntervalRandom.Range(intervalRange.min, intervalRange.max);
                    break;

                default:
                    delay = -1f;
                    break;
            }

            return delay;
        }

        public void ResetCounter()
        {
            m_Count = 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static int NormalizeLimit(int value)
        {
            return Mathf.Max(0, value);
        }

        public static DuIntRange NormalizeMultipleSpawnCount(DuIntRange range)
        {
            range.min = Mathf.Max(range.min, 0);
            range.max = Mathf.Max(range.max, range.min);
            return range;
        }

        public static float NormalizeIntervalValue(float value)
        {
            return Mathf.Max(0f, value);
        }
    }
}
