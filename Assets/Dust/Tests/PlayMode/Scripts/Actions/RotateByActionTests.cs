using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.Actions.Rotate
{
    public class RotateByActionTests : RotateActionTests
    {
        protected float THRESHOLD = 0.02f;

        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Rotate_InWorldSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var tmp = new GameObject();
            tmp.transform.parent = testObject.transform.parent;
            tmp.transform.localPosition = testObject.transform.localPosition;
            tmp.transform.localRotation = testObject.transform.localRotation;
            tmp.transform.localScale = testObject.transform.localScale;

            int iterations = Mathf.Max(1, Mathf.CeilToInt(rotateBy.magnitude / THRESHOLD));
            var rotateByStep = rotateBy / iterations;
            
            for (int i = 0; i < iterations; i++)
            {
                tmp.transform.Rotate(rotateByStep, Space.World);
            }

            Quaternion endInWorld = tmp.transform.rotation;
            Quaternion endInLocal = tmp.transform.localRotation;
            
            Object.DestroyImmediate(tmp);
            
            yield return RotateTest(testObject, duration, RotateByAction.Space.World, rotateBy, endInWorld, endInLocal);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Rotate_InLocalSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var tmp = new GameObject();
            tmp.transform.parent = testObject.transform.parent;
            tmp.transform.localPosition = testObject.transform.localPosition;
            tmp.transform.localRotation = testObject.transform.localRotation;
            tmp.transform.localScale = testObject.transform.localScale;

            var rotateByStep = rotateBy;
            
            if (tmp.transform.parent != null)
                rotateByStep = tmp.transform.parent.TransformDirection(rotateByStep);

            int iterations = Mathf.Max(1, Mathf.CeilToInt(rotateBy.magnitude / THRESHOLD));
            rotateByStep = rotateByStep / iterations;
            
            for (int i = 0; i < iterations; i++)
            {
                tmp.transform.Rotate(rotateByStep, Space.World);
            }

            Quaternion endInWorld = tmp.transform.rotation;
            Quaternion endInLocal = tmp.transform.localRotation;
            
            Object.DestroyImmediate(tmp);
            
            yield return RotateTest(testObject, duration, RotateByAction.Space.Local, rotateBy, endInWorld, endInLocal);
        }

        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Rotate_InSelfSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var tmp = new GameObject();
            tmp.transform.parent = testObject.transform.parent;
            tmp.transform.localPosition = testObject.transform.localPosition;
            tmp.transform.localRotation = testObject.transform.localRotation;
            tmp.transform.localScale = testObject.transform.localScale;

            int iterations = Mathf.Max(1, Mathf.CeilToInt(rotateBy.magnitude / THRESHOLD));
            var rotateByStep = rotateBy / iterations;
            
            for (int i = 0; i < iterations; i++)
            {
                tmp.transform.Rotate(rotateByStep, Space.Self);
            }

            Quaternion endInWorld = tmp.transform.rotation;
            Quaternion endInLocal = tmp.transform.localRotation;
            
            Object.DestroyImmediate(tmp);
            
            yield return RotateTest(testObject, duration, RotateByAction.Space.Self, rotateBy, endInWorld, endInLocal);
        }
        
        protected IEnumerator RotateTest(GameObject testObject, float duration,
            RotateByAction.Space space, Vector3 rotateBy, 
            Quaternion endInWorld, Quaternion endInLocal)
        {
            var sut = testObject.AddComponent<RotateByAction>();
            sut.duration = Sec(duration);
            sut.space = space;
            sut.rotateBy = rotateBy;
            
            sut.improveAccuracy = true;
            sut.improveAccuracyThreshold = 0.1f;
            sut.improveAccuracyMaxIterations = 1000;

            sut.Play();

            yield return RotateTest(testObject, duration, endInWorld, endInLocal);            
        }

        //--------------------------------------------------------------------------------------------------------------
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator RotateAndRollback_InWorldSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            yield return RotateAndRollbackTest(testObject, duration, RotateByAction.Space.World, rotateBy);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator RotateAndRollback_InLocalSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            yield return RotateAndRollbackTest(testObject, duration, RotateByAction.Space.Local, rotateBy);
        }

        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator RotateAndRollback_InSelfSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            yield return RotateAndRollbackTest(testObject, duration, RotateByAction.Space.Self, rotateBy);
        }

        protected IEnumerator RotateAndRollbackTest(GameObject testObject, float duration,
            RotateByAction.Space space, Vector3 rotateBy)
        {
            Quaternion endInWorld = testObject.transform.rotation;
            Quaternion endInLocal = testObject.transform.localRotation;
            
            var sut = testObject.AddComponent<RotateByAction>();
            sut.duration = Sec(duration * 0.5f);
            sut.playRollback = true;
            sut.rollbackDuration = Sec(duration * 0.5f);
            sut.space = space;
            sut.rotateBy = rotateBy;
            
            sut.improveAccuracy = true;
            sut.improveAccuracyThreshold = 0.1f;
            sut.improveAccuracyMaxIterations = 1000;
            sut.Play();

            yield return RotateTest(testObject, duration, endInWorld, endInLocal);            
        }
    }
}
