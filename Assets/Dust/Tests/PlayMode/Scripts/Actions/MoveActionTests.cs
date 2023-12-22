using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace DustEngine.Test.Actions.Move
{
    public abstract class MoveActionTests : TransformActionTests
    {
        protected static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                var objLevelIds = new[] {ObjectTopLevel, ObjectSubLevel};
                var durations = new[] {1.0f, 0.0f};
                var xList = new[] {0f, 1f,  -5.00f, -1234.56f};
                var yList = new[] {0f, 1f,   7.50f,  3456.78f};
                var zList = new[] {0f, 1f, -12.25f, -9999.99f};

                if (Constants.MINIMIZE_ACTIONS_TESTS)
                {
                    durations = new[] {1.0f};
                    xList = new[] {0f,  -5.00f};
                    yList = new[] {0f,   7.50f};
                    zList = new[] {0f, -12.25f};
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

        protected Vector3 ConvertLocalToWorld(GameObject baseObject, Vector3 pointInLocal)
        {
            if (baseObject.transform.parent == null)
                return pointInLocal;
            
            return baseObject.transform.parent.TransformPoint(pointInLocal);
        }

        protected Vector3 ConvertWorldToLocal(GameObject baseObject, Vector3 pointInWorld)
        {
            if (baseObject.transform.parent == null)
                return pointInWorld;
            
            return baseObject.transform.parent.InverseTransformPoint(pointInWorld);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected IEnumerator MoveTest(GameObject testObject, float duration, Vector3 endInWorld, Vector3 endInLocal)
        {
            DebugLog($"Start At [WORLD]: {testObject.transform.position.ToString(Constants.FLOAT_ACCURACY_MASK)}");
            DebugLog($"Start At [LOCAL]: {testObject.transform.localPosition.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            DebugLog($"Expect At [WORLD]: {endInWorld.ToString(Constants.FLOAT_ACCURACY_MASK)}");
            DebugLog($"Expect At [LOCAL]: {endInLocal.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            if (duration > 0f)
            {
                var moveByCmp = testObject.GetComponent<MoveByAction>();
                if (moveByCmp != null && !moveByCmp.moveBy.Equals(Vector3.zero))
                {
                    Assert_NotEqual(testObject.transform.position, endInWorld, "Check middle in World space");
                    Assert_NotEqual(testObject.transform.localPosition, endInLocal, "Check middle in Local space");
                }
            }

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            DebugLog($"Result At [WORLD]: {testObject.transform.position.ToString(Constants.FLOAT_ACCURACY_MASK)}");
            DebugLog($"Result At [LOCAL]: {testObject.transform.localPosition.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            Assert_Equal(testObject.transform.position, endInWorld, "Check end in World space");
            Assert_Equal(testObject.transform.localPosition, endInLocal, "Check end in Local space");
        }
    }
}
