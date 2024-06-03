using UnityEngine;

namespace Dust
{
    public static class Constants
    {
        public static readonly float PI = Mathf.PI;
        public static readonly float PI2 = Mathf.PI * 2f;
        public static readonly float EDITOR_UPDATE_TIMEOUT = 0.0166f;

        public static readonly int ROUND_DIGITS_COUNT = 6;

        public static readonly int RANDOM_SEED_MIN = 1;
        public static readonly int RANDOM_SEED_MAX = int.MaxValue;
        public static readonly int RANDOM_SEED_DEFAULT = 12345;
        public static readonly int RANDOM_SEED_MIN_IN_EDITOR = 1;
        public static readonly int RANDOM_SEED_MAX_IN_EDITOR = 99999;
    }
}
