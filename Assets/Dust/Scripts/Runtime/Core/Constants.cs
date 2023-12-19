using UnityEngine;

namespace DustEngine
{
    public static class Constants
    {
        public const float PI = Mathf.PI;
        public const float PI2 = Mathf.PI * 2f;
        public const float EDITOR_UPDATE_TIMEOUT = 0.0166f;

        public const int ROUND_DIGITS_COUNT = 6;

        public const int RANDOM_SEED_MIN = 1;
        public const int RANDOM_SEED_MAX = int.MaxValue;
        public const int RANDOM_SEED_DEFAULT = 12345;
        public const int RANDOM_SEED_MIN_IN_EDITOR = 1;
        public const int RANDOM_SEED_MAX_IN_EDITOR = 99999;
    }
}
