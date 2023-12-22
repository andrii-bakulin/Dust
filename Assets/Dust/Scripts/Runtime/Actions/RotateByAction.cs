using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/RotateBy Action")]
    public class RotateByAction : IntervalWithRollbackAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
            Self = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_RotateBy = Vector3.zero;
        public Vector3 rotateBy
        {
            get => m_RotateBy;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_RotateBy = value;
            }
        }

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Space = value;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ImproveAccuracy = false;
        public bool improveAccuracy
        {
            get => m_ImproveAccuracy;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ImproveAccuracy = value;
            }
        }

        [SerializeField]
        private float m_ImproveAccuracyThreshold = 0.5f;
        public float improveAccuracyThreshold
        {
            get => m_ImproveAccuracyThreshold;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ImproveAccuracyThreshold = NormalizeAccuracyThreshold(value);
            }
        }

        [SerializeField]
        private int m_ImproveAccuracyMaxIterations = 16;
        public int improveAccuracyMaxIterations
        {
            get => m_ImproveAccuracyMaxIterations;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ImproveAccuracyMaxIterations = NormalizeAccuracyMaxIterations(value);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(activeTargetTransform))
                return;

            float signRotate = playingPhase == PlayingPhase.Main ? +1f : -1f;

            Vector3 deltaRotate = rotateBy * (signRotate * (playbackState - previousState));

            if (deltaRotate.Equals(Vector3.zero))
                return;
            
            if (space == Space.Local && Dust.IsNotNull(activeTargetTransform.parent))
                deltaRotate = activeTargetTransform.parent.TransformDirection(deltaRotate);
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            int iterationsCount = 1;

            if (improveAccuracy)
            {
                iterationsCount = Mathf.CeilToInt(deltaRotate.magnitude / improveAccuracyThreshold);
                iterationsCount = Mathf.Min(iterationsCount, improveAccuracyMaxIterations); 
                deltaRotate /= iterationsCount;
            }

            Quaternion quaternion = Quaternion.Euler(deltaRotate);

            for (int i = 0; i < iterationsCount; i++)
            {
                if (space == Space.World || space == Space.Local)
                {
                    // m_TargetTransform.Rotate(deltaRotate, UnityEngine.Space.World);
                    activeTargetTransform.rotation *= 
                        Quaternion.Inverse(activeTargetTransform.rotation) * quaternion * activeTargetTransform.rotation;
                }
                else if (space == Space.Self)
                {
                    // m_TargetTransform.Rotate(deltaRotate, UnityEngine.Space.Self);
                    activeTargetTransform.localRotation *= quaternion;
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static float NormalizeAccuracyThreshold(float value)
        {
            return Mathf.Max(value, 0.01f);
        }

        public static int NormalizeAccuracyMaxIterations(int value)
        {
            return Mathf.Clamp(value, 1, 1000);
        }
    }
}
