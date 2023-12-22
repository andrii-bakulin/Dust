using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DustEngine.Test
{
    public static class Constants
    {
        // false | true
        internal static readonly bool MINIMIZE_ACTIONS_TESTS = true;

        // 0.2f | 1.0f
        internal static readonly float TIME_SCALE = 0.2f;

        internal static readonly float VECTOR_DISTANCE_DELTA = 0.005f;
        internal static readonly float QUATERNION_ANGLE_DELTA = 1.0f;
        internal static readonly string FLOAT_ACCURACY_MASK = "F3";

        // true : false
        internal static bool DEBUG_LOG = true;
    }
}
