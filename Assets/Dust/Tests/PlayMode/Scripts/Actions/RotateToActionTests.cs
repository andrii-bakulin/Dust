using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.Actions.Rotate
{
    public class RotateToActionTests : RotateActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Rotate_InWorldSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);
            
            Quaternion endInWorld = Quaternion.Euler(rotateTo);
            Quaternion endInLocal = ConvertWorldToLocal(testObject, endInWorld);

            yield return RotateTest(testObject, duration, RotateToAction.Space.World, rotateTo, endInWorld, endInLocal);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Rotate_InLocalSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            Quaternion endInLocal = Quaternion.Euler(rotateTo);
            Quaternion endInWorld = ConvertLocalToWorld(testObject, endInLocal);

            yield return RotateTest(testObject, duration, RotateToAction.Space.Local, rotateTo, endInWorld, endInLocal);
        }

        protected IEnumerator RotateTest(GameObject testObject, float duration, 
            RotateToAction.Space space, Vector3 rotateTo,
            Quaternion endInWorld, Quaternion endInLocal)
        {
            var sut = testObject.AddComponent<RotateToAction>();
            sut.duration = Sec(duration);
            sut.space = space;
            sut.rotateTo = rotateTo;
            sut.Play();

            yield return RotateTest(testObject, duration, endInWorld, endInLocal);
        }
        
        //--------------------------------------------------------------------------------------------------------------

        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator RotateAndRollback_InWorldSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            yield return RotateAndRollbackTest(testObject, duration, RotateToAction.Space.World, rotateTo);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator RotateAndRollback_InLocalSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            yield return RotateAndRollbackTest(testObject, duration, RotateToAction.Space.Local, rotateTo);
        }

        protected IEnumerator RotateAndRollbackTest(GameObject testObject, float duration,
            RotateToAction.Space space, Vector3 rotateTo)
        {
            Quaternion endInWorld = testObject.transform.rotation;
            Quaternion endInLocal = testObject.transform.localRotation;

            var sut = testObject.AddComponent<RotateToAction>();
            sut.duration = Sec(duration * 0.5f);
            sut.playRollback = true;
            sut.rollbackDuration = Sec(duration * 0.5f);
            sut.space = space;
            sut.rotateTo = rotateTo;
            sut.Play();

            yield return RotateTest(testObject, duration, endInWorld, endInLocal);
        }
    }
}
