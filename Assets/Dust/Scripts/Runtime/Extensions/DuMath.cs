using UnityEngine;

namespace DustEngine
{
    public static class DuMath
    {
        public static bool IsZero(float value)
        {
            return Mathf.Approximately(value, 0f);
        }

        public static bool IsNotZero(float value)
        {
            return !IsZero(value);
        }

        public static bool Between(float value, float a, float b)
        {
            return a <= value && value <= b;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static float Round(float value)
            => Round(value, Constants.ROUND_DIGITS_COUNT);

        public static float Round(float value, int roundToDigits)
        {
            if (roundToDigits == 0)
                return value;

            return (float) System.Math.Round(value, roundToDigits);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static float Length(float a, float b)
        {
            return Mathf.Sqrt(a * a + b * b);
        }

        public static float NormalizeAngle180(float value)
        {
            value -= FloorToZero(value / 360f) * 360f;
            while (value > +180f) value -= 360f;
            while (value < -180f) value += 360f;
            return value;
        }

        public static float NormalizeAngle360(float value)
        {
            value -= FloorToZero(value / 360f) * 360f;
            while (value > +360f) value -= 360f;
            while (value <    0f) value += 360f;
            return value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static float FloorToZero(float value)
        {
            return value >= 0f ? Mathf.Floor(value) : Mathf.Ceil(value);
        }

        public static int FloorToZeroToInt(float value)
        {
            return value >= 0f ? Mathf.FloorToInt(value) : Mathf.CeilToInt(value);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static float Repeat(float t, float length)
        {
            // Why need this?
            // Because for example if length = 1f, then Repeat(1f, 1f) will be 0f, but should be 1f
            if (t < 0f || t > length)
                t = Mathf.Repeat(t, length);

            return t;
        }

        public static float Step01(float value, int stepsCount)
        {
            if (stepsCount < 1)
                return 0.0f;

            float stepDelta = 1f / stepsCount;
            float stepIndex = value / stepDelta;

            return Mathf.RoundToInt(stepIndex) * stepDelta;
        }

        public static float Step(float value, int stepsCount, float min, float max)
        {
            if (stepsCount < 1)
                return 0.0f;

            float valueNormalized = Fit(min, max, 0f, 1f, value);

            valueNormalized = Step01(valueNormalized, stepsCount);

            return Fit01To(min, max, valueNormalized);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static float Fit(float inMin, float inMax, float outMin, float outMax, float inValue)
            => Fit(inMin, inMax, outMin, outMax, inValue, false);

        public static float Fit(float inMin, float inMax, float outMin, float outMax, float inValue, bool clamped)
        {
            if (clamped)
            {
                if (inValue <= inMin) return outMin;
                if (inValue >= inMax) return outMax;
            }

            float inRange = inMax - inMin;
            float outRange = outMax - outMin;

            if (IsZero(inRange))
                return outMin + outRange / 2f; // just to prevent divide by ZERO -> return middle value from [min..max]-out

            return outMin + (inValue - inMin) / inRange * outRange;
        }

        public static float Fit01To(float outMin, float outMax, float inValue)
            => Fit01To(outMin, outMax, inValue, false);

        public static float Fit01To(float outMin, float outMax, float inValue, bool clamped)
        {
            if (clamped)
            {
                if (inValue <= 0f) return outMin;
                if (inValue >= 1f) return outMax;
            }

            return outMin + (outMax - outMin) * inValue;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void RotatePoint(ref float x, ref float y, float angle)
        {
            float sin = Mathf.Sin(Mathf.Deg2Rad * angle);
            float cos = Mathf.Cos(Mathf.Deg2Rad * angle);

            float xNew = x * cos - y * sin;
            float yNew = x * sin + y * cos;

            x = xNew;
            y = yNew;
        }

        public static Vector2 RotatePoint(float x, float y, float angle)
        {
            RotatePoint(ref x, ref y, angle);
            return new Vector2(x, y);
        }

        public static Vector2 RotatePoint(Vector2 point, float angle)
        {
            return RotatePoint(point.x, point.y, angle);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        // @DUST.optimize: make it better!
        public static Vector3 RotatePoint(Vector3 pointToRotate, Vector3 eulerRotation)
        {
            if (eulerRotation.Equals(Vector3.zero))
                return pointToRotate;

            Quaternion q = Quaternion.Euler(eulerRotation);
            Matrix4x4 m4r = Matrix4x4.Rotate(q);
            return m4r.MultiplyPoint(pointToRotate);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Cone

        // Math for this cone use fixed orientation along [X+]-axis
        // Ray outgoing from center of cone to endPoint
        public static class Cone
        {
            public static float DistanceToEdge(float radius, float height, Vector3 endPoint)
            {
                if (IsZero(radius) || IsZero(height) || endPoint.Equals(Vector3.zero))
                    return 0f;

                if (IsZero(endPoint.y) && IsZero(endPoint.z))
                    return height / 2f;

                // conePoint1 .. conePoint2 = edge
                // conePoint2 .. conePoint3 = base

                Vector2 conePoint1 = new Vector2(+height / 2f, 0f);
                Vector2 conePoint2 = new Vector2(-height / 2f, radius);

                Vector2 linePoint1 = Vector2.zero;

                // Convert 3D point to 2D (x&z; y) -> (x; y)
                Vector2 linePoint2 = new Vector2(endPoint.x, Length(endPoint.y, endPoint.z));
                linePoint2.y = Mathf.Abs(linePoint2.y);
                linePoint2 *= 1000f;

                if (DuVector2.IsIntersecting(conePoint1, conePoint2, linePoint1, linePoint2, out var intersectPoint) == false)
                {
                    Vector2 conePoint3 = new Vector2(-height / 2f, 0f);

                    if (DuVector2.IsIntersecting(conePoint2, conePoint3, linePoint1, linePoint2, out intersectPoint) == false)
                        return 0f;
                }

                return intersectPoint.magnitude;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Cylinder

        // Math for this cylinder use fixed orientation along [X+]-axis
        // Ray outgoing from center of cylinder to endPoint
        public static class Cylinder
        {
            public static Vector3 IntersectionPoint(float radius, float height, Vector3 endPoint)
            {
                if (IsZero(radius) || IsZero(height) || endPoint.Equals(Vector3.zero))
                    return Vector3.zero;

                float h2 = height / 2f;

                Vector3 point = Vector3.zero;

                if (IsZero(endPoint.y) && IsZero(endPoint.z))
                    return new Vector3(h2 * Mathf.Sign(endPoint.z), 0f, 0f);

                float rP2 = radius * radius;
                float yP2 = endPoint.y * endPoint.y;
                float zP2 = endPoint.z * endPoint.z;

                point.y = Mathf.Sqrt(rP2 * yP2 / (yP2 + zP2)) * Mathf.Sign(endPoint.y);
                point.z = Mathf.Sqrt(rP2 * zP2 / (yP2 + zP2)) * Mathf.Sign(endPoint.z);

                point.x = endPoint.x * (IsNotZero(endPoint.y) ? point.y / endPoint.y : point.z / endPoint.z);

                if (Mathf.Abs(point.x) > h2)
                    point *= h2 / Mathf.Abs(point.x);

                return point;
            }

            public static float DistanceToEdge(float radius, float height, Vector3 endPoint)
            {
                return IntersectionPoint(radius, height, endPoint).magnitude;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
    }
}
