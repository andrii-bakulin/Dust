using UnityEngine;
using UnityEngine.Events;

namespace DustEngine
{
    [AddComponentMenu("Dust/Instances/Destroyer")]
    public class Destroyer : DuMonoBehaviour
    {
        public enum DestroyMode
        {
            Manual = 0,
            Time = 1,
            TimeRange = 2,
            AliveZone = 3,
            DeadZone = 4,
        };

        public enum VolumeCenterMode
        {
            StartPosition = 0,
            FixedWorldPosition = 1,
            SourceObject = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [System.Serializable]
        public class DestroyerEvent : UnityEvent<GameObject>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private DestroyMode m_DestroyMode = DestroyMode.Time;
        public DestroyMode destroyMode
        {
            get => m_DestroyMode;
            set => m_DestroyMode = value;
        }

        [SerializeField]
        private float m_Timeout = 1f;
        public float timeout
        {
            get => m_Timeout;
            set => m_Timeout = value;
        }

        [SerializeField]
        private DuRange m_TimeoutRange = DuRange.zeroToOne;
        public DuRange timeoutRange
        {
            get => m_TimeoutRange;
            set => m_TimeoutRange = value;
        }

        [SerializeField]
        private VolumeCenterMode m_VolumeCenterMode = VolumeCenterMode.StartPosition;
        public VolumeCenterMode volumeCenterMode
        {
            get => m_VolumeCenterMode;
            set => m_VolumeCenterMode = value;
        }

        [SerializeField]
        private Vector3 m_VolumeCenter = Vector3.zero;
        public Vector3 volumeCenter
        {
            get => m_VolumeCenter;
            set => m_VolumeCenter = value;
        }

        [SerializeField]
        private Vector3 m_VolumeOffset = Vector3.zero;
        public Vector3 volumeOffset
        {
            get => m_VolumeOffset;
            set => m_VolumeOffset = value;
        }

        [SerializeField]
        private Vector3 m_VolumeSize = Vector3.one;
        public Vector3 volumeSize
        {
            get => m_VolumeSize;
            set => m_VolumeSize = NormalizeVolumeSize(value);
        }

        [SerializeField]
        private GameObject m_VolumeSourceCenter = null;
        public GameObject volumeSourceCenter
        {
            get => m_VolumeSourceCenter;
            set => m_VolumeSourceCenter = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_DisableColliders = true;
        public bool disableColliders
        {
            get => m_DisableColliders;
            set => m_DisableColliders = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private DestroyerEvent m_OnDestroy = null;
        public DestroyerEvent onDestroy => m_OnDestroy;

        //--------------------------------------------------------------------------------------------------------------

        private float m_TimeAlive;
        public float timeAlive => m_TimeAlive;

        private float m_TimeLimit;
        public float timeLimit => m_TimeLimit;

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            m_TimeAlive = 0f;
            m_TimeLimit = 0f;

            switch (destroyMode)
            {
                default:
                case DestroyMode.Manual:
                case DestroyMode.Time:
                    // Nothing need to do...
                    m_TimeLimit = timeout;
                    break;

                case DestroyMode.TimeRange:
                    m_TimeLimit = Random.Range(timeoutRange.min, timeoutRange.max);
                    break;

                case DestroyMode.AliveZone:
                case DestroyMode.DeadZone:
                    if (volumeCenterMode == VolumeCenterMode.StartPosition)
                        volumeCenter = this.transform.position;
                    break;
            }
        }

        private void Update()
        {
            switch (destroyMode)
            {
                default:
                case DestroyMode.Manual:
                    // Nothing need to do...
                    break;

                case DestroyMode.Time:
                case DestroyMode.TimeRange:
                    m_TimeAlive += Time.deltaTime;

                    if (m_TimeAlive >= m_TimeLimit)
                        DestroySelf();
                    break;

                case DestroyMode.AliveZone:
                    if (!IsInsideVolume())
                        DestroySelf();
                    break;

                case DestroyMode.DeadZone:
                    if (IsInsideVolume())
                        DestroySelf();
                    break;
            }
        }

        protected bool IsInsideVolume()
        {
            Vector3 selfPos = transform.position;

            Vector3 volCenter = volumeCenter;
            Vector3 halfSize = volumeSize / 2f;

            if (volumeCenterMode == VolumeCenterMode.SourceObject)
            {
                if (Dust.IsNull(volumeSourceCenter))
                    return false;

                volCenter = volumeSourceCenter.transform.position;
            }

            volCenter += volumeOffset;

            if (volCenter.x - halfSize.x > selfPos.x) return false;
            if (volCenter.x + halfSize.x < selfPos.x) return false;

            if (volCenter.y - halfSize.y > selfPos.y) return false;
            if (volCenter.y + halfSize.y < selfPos.y) return false;

            if (volCenter.z - halfSize.z > selfPos.z) return false;
            if (volCenter.z + halfSize.z < selfPos.z) return false;

            return true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            switch (destroyMode)
            {
                default:
                case DestroyMode.Manual:
                case DestroyMode.Time:
                case DestroyMode.TimeRange:
                    return;

                case DestroyMode.AliveZone:
                    Gizmos.color = Color.green;
                    break;

                case DestroyMode.DeadZone:
                    Gizmos.color = Color.red;
                    break;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            Vector3 gizmoCenter;

            switch (volumeCenterMode)
            {
                case VolumeCenterMode.StartPosition:
                    gizmoCenter = Application.isPlaying ? volumeCenter : transform.position;
                    break;

                case VolumeCenterMode.FixedWorldPosition:
                    gizmoCenter = volumeCenter;
                    break;

                case VolumeCenterMode.SourceObject:
                    if (Dust.IsNull(volumeSourceCenter))
                        return;

                    gizmoCenter = volumeSourceCenter.transform.position;
                    break;

                default:
                    return;
            }

            Gizmos.DrawWireCube(gizmoCenter + volumeOffset, volumeSize);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public void DestroySelf()
        {
            DestroyGameObject(this.gameObject);
        }

        public void DestroyTarget(GameObject gameObjectToDestroy)
        {
            DestroyGameObject(gameObjectToDestroy);
        }
        
        public void DestroyTarget(Component component)
        {
            DestroyGameObject(component.gameObject);
        }

        public void DestroyTarget(Collision other)
        {
            DestroyGameObject(other.gameObject);
        }

        public void DestroyTarget(Collider other)
        {
            DestroyGameObject(other.gameObject);
        }

        public void DestroyTarget(Collision2D other)
        {
            DestroyGameObject(other.gameObject);
        }

        public void DestroyTarget(Collider2D other)
        {
            DestroyGameObject(other.gameObject);
        }

        protected void DestroyGameObject(GameObject gameObjectToDestroy)
        {
            if (Dust.IsNotNull(onDestroy) && onDestroy.GetPersistentEventCount() > 0)
                onDestroy.Invoke(gameObjectToDestroy);

            if (disableColliders)
            {
                var colliders = gameObjectToDestroy.GetComponents<Collider>();

                foreach (var col in colliders)
                {
                    col.enabled = false;
                }
            }

            this.enabled = false;

            Destroy(gameObjectToDestroy);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static Vector3 NormalizeVolumeSize(Vector3 value)
        {
            return DuVector3.Abs(value);
        }
    }
}
