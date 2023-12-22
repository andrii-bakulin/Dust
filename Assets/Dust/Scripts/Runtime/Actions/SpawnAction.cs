﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DustEngine
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

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ResetPosition = false;
        public bool resetPosition
        {
            get => m_ResetPosition;
            set => m_ResetPosition = value;
        }

        [SerializeField]
        private bool m_ResetRotation = false;
        public bool resetRotation
        {
            get => m_ResetRotation;
            set => m_ResetRotation = value;
        }

        [SerializeField]
        private bool m_ResetScale = false;
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

        protected override void OnActionUpdate(float deltaTime)
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

            // Detect spawn point  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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
                    }
                    break;
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
                }
            }

            if (Dust.IsNull(objectToSpawn))
                return null;

            // Spawn - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            // 1. Create object and make parent as spawn-object
            GameObject obj = Instantiate(objectToSpawn, spawnAtPoint.transform);

            // 2. Change parent if need
            obj.transform.parent = parentMode switch
            {
                SpawnParentMode.Spawner    => transform,
                SpawnParentMode.SpawnPoint => spawnAtPoint.transform,
                SpawnParentMode.World      => null,
                _                          => spawnAtPoint.transform
            };

            // 3. Reset transform if need only after change parent!
            if (resetPosition)
                obj.transform.localPosition = Vector3.zero;

            if (resetRotation)
                obj.transform.localRotation = Quaternion.identity;
            
            if (resetScale)
                obj.transform.localScale = Vector3.one;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            return obj;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static DuIntRange NormalizeMultipleSpawnCount(DuIntRange range)
        {
            range.min = Mathf.Max(range.min, 0);
            range.max = Mathf.Max(range.max, range.min);
            return range;
        }
    }
}
