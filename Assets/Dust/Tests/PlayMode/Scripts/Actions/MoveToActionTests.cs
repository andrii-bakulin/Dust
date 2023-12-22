using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.Actions.Move
{
    public class MoveToActionTests : MoveActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Move_InWorldSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var moveTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = moveTo;
            var endInLocal = ConvertWorldToLocal(testObject, endInWorld);

            var sut = testObject.AddComponent<MoveToAction>();
            sut.duration = Sec(duration);
            sut.space = MoveToAction.Space.World;
            sut.moveTo = moveTo;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Move_InLocalSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var moveTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInLocal = moveTo;
            var endInWorld = ConvertLocalToWorld(testObject, endInLocal);

            var sut = testObject.AddComponent<MoveToAction>();
            sut.duration = Sec(duration);
            sut.space = MoveToAction.Space.Local;
            sut.moveTo = moveTo;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator MoveAndRollback_InWorldSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var moveTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = testObject.transform.position;
            var endInLocal = testObject.transform.localPosition;

            var sut = testObject.AddComponent<MoveToAction>();
            sut.duration = Sec(duration * 0.5f);
            sut.playRollback = true;
            sut.rollbackDuration = Sec(duration * 0.5f); 
            sut.space = MoveToAction.Space.World;
            sut.moveTo = moveTo;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator MoveAndRollback_InLocalSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var moveTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = testObject.transform.position;
            var endInLocal = testObject.transform.localPosition;

            var sut = testObject.AddComponent<MoveToAction>();
            sut.duration = Sec(duration * 0.5f);
            sut.playRollback = true;
            sut.rollbackDuration = Sec(duration * 0.5f); 
            sut.space = MoveToAction.Space.Local;
            sut.moveTo = moveTo;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }
    }
}
