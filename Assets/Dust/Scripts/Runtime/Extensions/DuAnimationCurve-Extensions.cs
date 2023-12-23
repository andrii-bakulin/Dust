using UnityEngine;

namespace DustEngine
{
    public static class DuAnimationCurve_Extensions
    {
        /**
         * t - should be in [0..1] range
         */
        public static float duEvaluateIfTimeInRange01(this AnimationCurve curve, float t)
        {
            if (curve.keys.Length == 0)
                return 0f;

            if (curve.keys.Length == 1)
                return curve.keys[0].value;

            float timeMin = curve.keys[0].time;
            float timeMax = curve.keys[curve.length - 1].time;
            float timeLen = timeMax - timeMin;
            float tNormalized = timeMin + timeLen * t;

            return curve.Evaluate(tNormalized);
        }

        public static void duClampTime(this AnimationCurve curve, float min, float max)
            => duClampTime(curve, min, max, false);

        public static void duClampTime(this AnimationCurve curve, float min, float max, bool forceStretch)
        {
            if (curve.keys.Length == 0)
                return;

            Keyframe[] keys = curve.keys;

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].time = Mathf.Clamp(keys[i].time, min, max);
            }

            if (forceStretch)
            {
                if (keys.Length > 1)
                {
                    if (keys[0].time > min)
                        keys[0].time = min;

                    if (keys[keys.Length - 1].time < max)
                        keys[keys.Length - 1].time = max;
                }
                else // curve.keys.Length == 1
                {
                    if (keys[0].time > min)
                        keys[0].time = min;
                }
            }

            curve.keys = keys;

            if (forceStretch && keys.Length == 1)
                curve.AddKey(max, keys[0].value);
        }

        public static void duClampValues(this AnimationCurve curve, float min, float max)
        {
            if (curve.keys.Length == 0)
                return;

            Keyframe[] keys = curve.keys;

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].value = Mathf.Clamp(keys[i].value, min, max);
            }

            curve.keys = keys;
        }

        public static void duClamp01TimeAndValues(this AnimationCurve curve)
            => duClamp01TimeAndValues(curve, true);

        public static void duClamp01TimeAndValues(this AnimationCurve curve, bool forceStretch)
        {
            curve.duClampTime(0f, 1f, forceStretch);
            curve.duClampValues(0f, 1f);
        }
    }
}
