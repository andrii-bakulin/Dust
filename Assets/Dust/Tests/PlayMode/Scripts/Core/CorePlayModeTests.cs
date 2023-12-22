using NUnit.Framework;
using UnityEngine;

namespace DustEngine.Test
{
    public abstract class CorePlayModeTests
    {
        protected float Sec(float sec)
        {
            return sec * Constants.TIME_SCALE;
        }

        protected void Assert_Equal(Vector3 testValue, Vector3 baseValue, string message = "")
        {
            float distance = Vector3.Distance(testValue, baseValue);

            message += (string.IsNullOrEmpty(message) ? "" : "\n") +
                       $"Expected: Vector3({baseValue.ToString("F3")})\n" +
                       $"But was:  Vector3({testValue.ToString("F3")})\n" +
                       $"Distance: {distance} is greater than delta {Constants.VECTOR_DISTANCE_DELTA}";

            Assert.That(distance, Is.LessThanOrEqualTo(Constants.VECTOR_DISTANCE_DELTA), message);
        }

        protected void Assert_NotEqual(Vector3 testValue, Vector3 baseValue, string message = "")
        {
            float distance = Vector3.Distance(testValue, baseValue);

            message += (string.IsNullOrEmpty(message) ? "" : "\n") +
                       $"NOT Expected: Vector3({baseValue.ToString("F3")})\n" +
                       $"But was:      Vector3({testValue.ToString("F3")})\n" +
                       $"Distance:     {distance} is less than delta {Constants.VECTOR_DISTANCE_DELTA} so vectors are EQUAL";

            Assert.That(distance, Is.Not.LessThanOrEqualTo(Constants.VECTOR_DISTANCE_DELTA), message);
        }

        protected void Assert_Equal(Quaternion testValue, Quaternion baseValue, string message = "")
        {
            float angle = Quaternion.Angle(testValue, baseValue);

            message += (string.IsNullOrEmpty(message) ? "" : "\n") +
                       $"Expected: Quaternion.Euler({baseValue.eulerAngles.ToString("F3")})\n" +
                       $"But was:  Quaternion.Euler({testValue.eulerAngles.ToString("F3")})\n" +
                       $"Angle:    {angle} is greater than delta {Constants.QUATERNION_ANGLE_DELTA}";

            Assert.That(angle, Is.LessThanOrEqualTo(Constants.QUATERNION_ANGLE_DELTA), message);
        }

        protected void Assert_NotEqual(Quaternion testValue, Quaternion baseValue, string message = "")
        {
            float angle = Quaternion.Angle(testValue, baseValue);

            message += (string.IsNullOrEmpty(message) ? "" : "\n") +
                       $"NOT Expected: Quaternion.Euler({baseValue.eulerAngles.ToString("F3")})\n" + 
                       $"But was:      Quaternion.Euler({testValue.eulerAngles.ToString("F3")})\n" +
                       $"Angle:        {angle} is less than delta {Constants.QUATERNION_ANGLE_DELTA} so Quaternions are EQUAL";

            Assert.That(angle, Is.Not.LessThanOrEqualTo(Constants.QUATERNION_ANGLE_DELTA), message);
        }
        
        //--------------------------------------------------------------------------------------------------------------

        public static void DebugLog(object message)
        {
            if (!Constants.DEBUG_LOG)
                return;

            Debug.Log(message);
        }
    }
}
