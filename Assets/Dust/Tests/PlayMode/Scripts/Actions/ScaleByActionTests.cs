using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.Actions.Scale
{
    public class ScaleByActionTests : ScaleActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Scale_WorldSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var scaleBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = Vector3.Scale(testObject.transform.lossyScale, scaleBy);
            
            var sut = testObject.AddComponent<ScaleByAction>();
            sut.duration = Sec(duration);
            sut.space = ScaleByAction.Space.World;
            sut.scaleBy = scaleBy;
            sut.Play();

            yield return ScaleInWorldSpaceTest(testObject, duration, endInWorld);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Scale_LocalSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var scaleBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInLocal = Vector3.Scale(testObject.transform.localScale, scaleBy);
            
            var sut = testObject.AddComponent<ScaleByAction>();
            sut.duration = Sec(duration);
            sut.space = ScaleByAction.Space.Local;
            sut.scaleBy = scaleBy;
            sut.Play();

            yield return ScaleInLocalSpaceTest(testObject, duration, endInLocal);
        }

        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator ScaleAndRollback_WorldSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var scaleBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = testObject.transform.lossyScale;
            
            var sut = testObject.AddComponent<ScaleByAction>();
            sut.duration = Sec(duration * 0.5f);
            sut.playRollback = true;
            sut.rollbackDuration = Sec(duration * 0.5f);
            sut.space = ScaleByAction.Space.World;
            sut.scaleBy = scaleBy;
            sut.Play();

            yield return ScaleInWorldSpaceTest(testObject, duration, endInWorld);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator ScaleAndRollback_LocalSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var scaleBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInLocal = testObject.transform.localScale;
            
            var sut = testObject.AddComponent<ScaleByAction>();
            sut.duration = Sec(duration * 0.5f);
            sut.playRollback = true;
            sut.rollbackDuration = Sec(duration * 0.5f);
            sut.space = ScaleByAction.Space.Local;
            sut.scaleBy = scaleBy;
            sut.Play();

            yield return ScaleInLocalSpaceTest(testObject, duration, endInLocal);
        }
    }
}
