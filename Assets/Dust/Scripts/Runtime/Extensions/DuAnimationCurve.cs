using UnityEngine;

namespace DustEngine
{
    public static class DuAnimationCurve
    {
        public static AnimationCurve StraightLine01()
        {
            var curve = new AnimationCurve();

            var key0 = new Keyframe(0f, 0f)
            {
                weightedMode = WeightedMode.Both
            };
            curve.AddKey(key0);

            var key1 = new Keyframe(1f, 1f)
            {
                weightedMode = WeightedMode.Both
            };
            curve.AddKey(key1);

            return curve;
        }
    }
}
