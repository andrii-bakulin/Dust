using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace DustEngine.Test.Actions.Rotate
{
    public class RotateActionTests : TransformActionTests
    {
        protected static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                var objLevelIds = new[] {ObjectTopLevel, ObjectSubLevel};
                var durations = new[] {1.0f, 0.0f};
                var xList = new[] {0f, 90f, -90f,  -5.00f, -999.99f};
                var yList = new[] {0f, 90f, -90f,   7.50f,  888.88f};
                var zList = new[] {0f, 90f, -90f, -12.25f,  360.00f};

                if (Constants.MINIMIZE_ACTIONS_TESTS)
                {
                    durations = new[] {1.0f};
                    xList = new[] {0f, 30.00f};
                    yList = new[] {0f, 45.00f};
                    zList = new[] {0f, 90.25f};
                }

                foreach (var objLevelId in objLevelIds)
                foreach (var duration in durations)
                foreach (var x in xList)
                foreach (var y in yList)
                foreach (var z in zList)
                {
                    yield return new TestCaseData(objLevelId, duration, x, y, z).Returns(null);                    
                }
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------
        // Helpers

        protected Quaternion ConvertLocalToWorld(GameObject baseObject, Quaternion angleInLocal)
        {
            if (baseObject.transform.parent == null)
                return angleInLocal;
            
            return baseObject.transform.parent.rotation * angleInLocal;
        }

        protected Quaternion ConvertWorldToLocal(GameObject baseObject, Quaternion pointInWorld)
        {
            if (baseObject.transform.parent == null)
                return pointInWorld;

            return Quaternion.Inverse(baseObject.transform.parent.rotation) * pointInWorld;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected IEnumerator RotateTest(GameObject testObject, float duration, Quaternion endInWorld, Quaternion endInLocal)
        {
            DebugLog($"Start At [WORLD]: {testObject.transform.rotation.eulerAngles.ToString(Constants.FLOAT_ACCURACY_MASK)}");
            DebugLog($"Start At [LOCAL]: {testObject.transform.localRotation.eulerAngles.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            DebugLog($"Expect At [WORLD]: {endInWorld.eulerAngles.ToString(Constants.FLOAT_ACCURACY_MASK)}");
            DebugLog($"Expect At [LOCAL]: {endInLocal.eulerAngles.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            /*
            if (duration > 0f)
            {
                var rotateByCmp = testObject.GetComponent<RotateByAction>();
                if (rotateByCmp != null && !rotateByCmp.rotateBy.Equals(Vector3.zero))
                {
                    Assert_NotEqual(testObject.transform.rotation, endInWorld, "Check middle point in World space");
                    Assert_NotEqual(testObject.transform.localRotation, endInLocal, "Check middle point in Local space");
                }
            }
            */

            yield return new WaitForSeconds(Sec(duration * 0.75f));
 
            DebugLog($"Result At [WORLD]: {testObject.transform.rotation.eulerAngles.ToString(Constants.FLOAT_ACCURACY_MASK)}");
            DebugLog($"Result At [LOCAL]: {testObject.transform.localRotation.eulerAngles.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            Assert_Equal(testObject.transform.rotation, endInWorld, "Check end point in World space");
            Assert_Equal(testObject.transform.localRotation, endInLocal, "Check end point in Local space");
        }
    }
}
