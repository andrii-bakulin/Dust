using UnityEngine;

namespace DustEngine
{
    public static class DuVector3
    {
        public static Vector3 New(float xyz)
        {
            return new Vector3(xyz, xyz, xyz);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Vector3 Abs(Vector3 value)
        {
            value.x = Mathf.Abs(value.x);
            value.y = Mathf.Abs(value.y);
            value.z = Mathf.Abs(value.z);
            return value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            value.x = Mathf.Clamp(value.x, min.x, max.x);
            value.y = Mathf.Clamp(value.y, min.y, max.y);
            value.z = Mathf.Clamp(value.z, min.z, max.z);
            return value;
        }

        public static Vector3 Clamp01(Vector3 value)
        {
            return Clamp(value, Vector3.zero, Vector3.one);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Vector3 NormalizeAngle180(Vector3 value)
        {
            value.x = DuMath.NormalizeAngle180(value.x);
            value.y = DuMath.NormalizeAngle180(value.y);
            value.z = DuMath.NormalizeAngle180(value.z);
            return value;
        }

        public static Vector3 NormalizeAngle360(Vector3 value)
        {
            value.x = DuMath.NormalizeAngle360(value.x);
            value.y = DuMath.NormalizeAngle360(value.y);
            value.z = DuMath.NormalizeAngle360(value.z);
            return value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Vector3 Round(Vector3 value)
            => Round(value, Constants.ROUND_DIGITS_COUNT);

        public static Vector3 Round(Vector3 value, int roundToDigits)
        {
            value.x = DuMath.Round(value.x, roundToDigits);
            value.y = DuMath.Round(value.y, roundToDigits);
            value.z = DuMath.Round(value.z, roundToDigits);
            return value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Vector3 Fit(float inMin, float inMax, Vector3 outMin, Vector3 outMax, float inValue)
            => Fit(inMin, inMax, outMin, outMax, inValue, false);

        public static Vector3 Fit(float inMin, float inMax, Vector3 outMin, Vector3 outMax, float inValue, bool clamped)
        {
            Vector3 r;
            r.x = DuMath.Fit(inMin, inMax, outMin.x, outMax.x, inValue, clamped);
            r.y = DuMath.Fit(inMin, inMax, outMin.y, outMax.y, inValue, clamped);
            r.z = DuMath.Fit(inMin, inMax, outMin.z, outMax.z, inValue, clamped);
            return r;
        }

        public static Vector3 Fit01To(Vector3 outMin, Vector3 outMax, float inValue)
            => Fit01To(outMin, outMax, inValue, false);

        public static Vector3 Fit01To(Vector3 outMin, Vector3 outMax, float inValue, bool clamped)
        {
            Vector3 r;
            r.x = DuMath.Fit01To(outMin.x, outMax.x, inValue, clamped);
            r.y = DuMath.Fit01To(outMin.y, outMax.y, inValue, clamped);
            r.z = DuMath.Fit01To(outMin.z, outMax.z, inValue, clamped);
            return r;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime)
            => SmoothDamp(current, target, ref currentVelocity, smoothTime, Mathf.Infinity);

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed)
        {
            Vector3 r;
            r.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed);
            r.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed);
            r.z = Mathf.SmoothDamp(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed);
            return r;
        }

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
        {
            Vector3 r;
            r.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime);
            r.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime);
            r.z = Mathf.SmoothDamp(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime);
            return r;
        }

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, Vector3 smoothTime)
            => SmoothDamp(current, target, ref currentVelocity, smoothTime, Mathf.Infinity);

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, Vector3 smoothTime, float maxSpeed)
        {
            Vector3 r;
            r.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime.x, maxSpeed);
            r.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime.y, maxSpeed);
            r.z = Mathf.SmoothDamp(current.z, target.z, ref currentVelocity.z, smoothTime.z, maxSpeed);
            return r;
        }

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, Vector3 smoothTime, float maxSpeed, float deltaTime)
        {
            Vector3 r;
            r.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime.x, maxSpeed, deltaTime);
            r.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime.y, maxSpeed, deltaTime);
            r.z = Mathf.SmoothDamp(current.z, target.z, ref currentVelocity.z, smoothTime.z, maxSpeed, deltaTime);
            return r;
        }
    }
}
