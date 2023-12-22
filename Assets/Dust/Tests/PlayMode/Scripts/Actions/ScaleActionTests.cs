using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace DustEngine.Test.Actions.Scale
{
    public abstract class ScaleActionTests : TransformActionTests
    {
        protected static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                var objLevelIds = new[] {ObjectTopLevel, ObjectSubLevel};
                var durations = new[] {1.0f, 0.0f};
                var xList = new[] {0f, 0.5f, 1f, -1f, 0.75f};
                var yList = new[] {0f, 0.5f, 1f, -1f, 1.25f};
                var zList = new[] {0f, 0.5f, 1f, -1f, 3.75f};

                if (Constants.MINIMIZE_ACTIONS_TESTS)
                {
                    durations = new[] {1.0f};
                    xList = new[] {0.00f, -0.50f};
                    yList = new[] {0.75f, -1.00f};
                    zList = new[] {1.33f, -1.33f};
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

        protected Vector3 ConvertLocalToWorld(GameObject baseObject, Vector3 scaleInLocal)
        {
            if (baseObject.transform.parent == null)
                return scaleInLocal;
            
            return baseObject.transform.parent.TransformPoint(scaleInLocal);
        }

        protected Vector3 ConvertWorldToLocal(GameObject baseObject, Vector3 scaleInWorld)
        {
            if (baseObject.transform.parent == null)
                return scaleInWorld;
            
            return baseObject.transform.parent.InverseTransformPoint(scaleInWorld);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected IEnumerator ScaleTest(GameObject testObject, float duration, Vector3 endInWorld, Vector3 endInLocal)
        {
            DebugLog($"Start At [WORLD]: {testObject.transform.lossyScale.ToString(Constants.FLOAT_ACCURACY_MASK)}");
            DebugLog($"Start At [LOCAL]: {testObject.transform.localScale.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            DebugLog($"Expect At [WORLD]: {endInWorld.ToString(Constants.FLOAT_ACCURACY_MASK)}");
            DebugLog($"Expect At [LOCAL]: {endInLocal.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            if (duration > 0f)
            {
                var scaleByCmp = testObject.GetComponent<ScaleByAction>();
                if (scaleByCmp != null && !scaleByCmp.scaleBy.Equals(Vector3.zero))
                {
                    Assert_NotEqual(testObject.transform.lossyScale, endInWorld, "Check middle in World space");
                    Assert_NotEqual(testObject.transform.localScale, endInLocal, "Check middle in Local space");
                }
            }

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            DebugLog($"Result At [WORLD]: {testObject.transform.lossyScale.ToString(Constants.FLOAT_ACCURACY_MASK)}");
            DebugLog($"Result At [LOCAL]: {testObject.transform.localScale.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            Assert_Equal(testObject.transform.lossyScale, endInWorld, "Check end in World space");
            Assert_Equal(testObject.transform.localScale, endInLocal, "Check end in Local space");
        }

        //--------------------------------------------------------------------------------------------------------------

        protected IEnumerator ScaleInWorldSpaceTest(GameObject testObject, float duration, Vector3 endInWorld)
        {
            DebugLog($"Start At [WORLD]: {testObject.transform.lossyScale.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            DebugLog($"Expect At [WORLD]: {endInWorld.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            if (duration > 0f)
            {
                var scaleByCmp = testObject.GetComponent<ScaleByAction>();
                if (scaleByCmp != null && !scaleByCmp.scaleBy.Equals(Vector3.zero))
                {
                    Assert_NotEqual(testObject.transform.lossyScale, endInWorld, "Check middle in World space");
                }
            }

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            DebugLog($"Result At [WORLD]: {testObject.transform.lossyScale.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            Assert_Equal(testObject.transform.lossyScale, endInWorld, "Check end in World space");
        }
        
        protected IEnumerator ScaleInLocalSpaceTest(GameObject testObject, float duration, Vector3 endInLocal)
        {
            DebugLog($"Start At [LOCAL]: {testObject.transform.localScale.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            DebugLog($"Expect At [LOCAL]: {endInLocal.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            if (duration > 0f)
            {
                var scaleByCmp = testObject.GetComponent<ScaleByAction>();
                if (scaleByCmp != null && !scaleByCmp.scaleBy.Equals(Vector3.zero))
                {
                    Assert_NotEqual(testObject.transform.localScale, endInLocal, "Check middle in Local space");
                }
            }

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            DebugLog($"Result At [LOCAL]: {testObject.transform.localScale.ToString(Constants.FLOAT_ACCURACY_MASK)}");

            Assert_Equal(testObject.transform.localScale, endInLocal, "Check end in Local space");
        }
    }
}
