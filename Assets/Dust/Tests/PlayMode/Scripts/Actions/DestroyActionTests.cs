using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace DustEngine.Test.Actions.Destroy
{
    public class DestroyActionTests : ActionTests
    {
        protected const int MAX_TEST_OBJECTS_COUNT = 2;
        protected string TestId = "";

        [UnitySetUp]
        protected IEnumerator SetupScene()
        {
            TestId = "id" + DateTime.Now.Ticks;
            yield break;
        }

        [UnityTearDown]
        protected IEnumerator ReleaseScene()
        {
            for (int index = 1; index <= MAX_TEST_OBJECTS_COUNT; index++)
            {
                var testObject = GetTestObject(index);
                if (testObject != null)
                    Object.DestroyImmediate(testObject);   
            }
            yield break;
        }
        
        protected GameObject CreateTestObject(int index)
        {
            var testObject = new GameObject($"TestObject_{TestId}_{index}");
            testObject.transform.localPosition = new Vector3(index, 0f, 0f);
            return testObject;
        }

        protected GameObject GetTestObject(int index)
        {
            return GameObject.Find($"/TestObject_{TestId}_{index}");
        }

        protected bool IsTestObjectExists(int index)
        {
            return GetTestObject(index) != null;
        }

        protected void IntroduceTestId(string testName)
        {
            DebugLog($"Test ID[{TestId}] = {testName}");
        }

        //--------------------------------------------------------------------------------------------------------------
        
        [UnityTest]
        public IEnumerator DoNotDestroyObject_BecauseActionNotPlayed()
        {
            IntroduceTestId("DoNotDestroyObject_BecauseActionNotPlayed");

            var testObject1 = CreateTestObject(1);

            var actDestroy = testObject1.AddComponent<DestroyAction>();
            // actDestroy.Play();

            Assert.That(IsTestObjectExists(1), Is.True);

            yield return new WaitForSeconds(Sec(0.25f)); 
            
            Assert.That(IsTestObjectExists(1), Is.True);
        }

        [UnityTest]
        public IEnumerator DestroyObject_Immediately()
        {
            IntroduceTestId("DestroyObject_Immediately");

            var testObject1 = CreateTestObject(1);

            var actDestroy = testObject1.AddComponent<DestroyAction>();
            actDestroy.applyToSelf = true;

            Assert.That(IsTestObjectExists(1), Is.True);

            actDestroy.Play();

            // wait for 3 frames :)
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            Assert.That(IsTestObjectExists(1), Is.False);
        }

        [UnityTest]
        public IEnumerator DestroyObject_AfterDelay()
        {
            IntroduceTestId("DestroyObject_AfterDelay");

            var testObject1 = CreateTestObject(1);

            var actDelay = testObject1.AddComponent<DelayAction>();
            actDelay.duration = Sec(0.5f);

            var actDestroy = testObject1.AddComponent<DestroyAction>();
            actDestroy.applyToSelf = true;
            actDelay.onCompleteActions.Add(actDestroy);
            
            Assert.That(IsTestObjectExists(1), Is.True);

            actDelay.Play();
            
            yield return new WaitForSeconds(Sec(0.25f));
            
            Assert.That(IsTestObjectExists(1), Is.True);
            
            yield return new WaitForSeconds(Sec(0.5f));
            
            Assert.That(IsTestObjectExists(1), Is.False);
        }

        [UnityTest]
        public IEnumerator DestroyObject_AndNextCompleteActionIsPlaying()
        {
            IntroduceTestId("DestroyObject_AndNextCompleteActionIsPlaying");

            var testObject1 = CreateTestObject(1);
            var testObject2 = CreateTestObject(2);

            var actMoving = testObject1.AddComponent<MoveByAction>();
            actMoving.duration = Sec(3.0f);
            actMoving.moveBy = Vector3.one;
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            
            var actDelay = testObject2.AddComponent<DelayAction>();
            actDelay.duration = Sec(0.25f);

            var actDestroy = testObject2.AddComponent<DestroyAction>();
            actDestroy.applyToSelf = true;
            actDestroy.onCompleteActions.Add(actMoving);

            actDelay.onCompleteActions.Add(actDestroy);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            actDelay.Play();
            
            Assert.That(actDelay.isPlaying, Is.True);
            Assert.That(actDestroy.isPlaying, Is.False);
            Assert.That(actMoving.isPlaying, Is.False);

            yield return new WaitForSeconds(Sec(0.5f));
            
            Assert.That(IsTestObjectExists(1), Is.True);
            Assert.That(IsTestObjectExists(2), Is.False);

            Assert.That(actMoving.isPlaying, Is.True);
        }
    }
}
