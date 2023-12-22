﻿using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.Actions.Move
{
    public class MoveByActionTests : MoveActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Move_InWorldSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = testObject.transform.position + moveBy;
            var endInLocal = ConvertWorldToLocal(testObject, endInWorld);
            
            var sut = testObject.AddComponent<MoveByAction>();
            sut.duration = Sec(duration);
            sut.space = MoveByAction.Space.World;
            sut.moveBy = moveBy;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Move_InLocalSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInLocal = testObject.transform.localPosition + moveBy;
            var endInWorld = ConvertLocalToWorld(testObject, endInLocal);
            
            var sut = testObject.AddComponent<MoveByAction>();
            sut.duration = Sec(duration);
            sut.space = MoveByAction.Space.Local;
            sut.moveBy = moveBy;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }

        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Move_InSelfSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInLocal = testObject.transform.localPosition + DuMath.RotatePoint(moveBy, testObject.transform.localEulerAngles);
            var endInWorld = ConvertLocalToWorld(testObject, endInLocal);

            var sut = testObject.AddComponent<MoveByAction>();
            sut.duration = Sec(duration);
            sut.space = MoveByAction.Space.SelfFixed;
            sut.moveBy = moveBy;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }
        
        //--------------------------------------------------------------------------------------------------------------

        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator MoveAndRollback_InWorldSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = testObject.transform.position;
            var endInLocal = testObject.transform.localPosition;
            
            var sut = testObject.AddComponent<MoveByAction>();
            sut.duration = Sec(duration * 0.5f);
            sut.playRollback = true;
            sut.rollbackDuration = Sec(duration * 0.5f);
            sut.space = MoveByAction.Space.World;
            sut.moveBy = moveBy;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator MoveAndRollback_InLocalSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = testObject.transform.position;
            var endInLocal = testObject.transform.localPosition;
            
            var sut = testObject.AddComponent<MoveByAction>();
            sut.duration = Sec(duration * 0.5f);
            sut.playRollback = true;
            sut.rollbackDuration = Sec(duration * 0.5f);
            sut.space = MoveByAction.Space.Local;
            sut.moveBy = moveBy;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }

        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator MoveAndRollback_InSelfSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = testObject.transform.position;
            var endInLocal = testObject.transform.localPosition;

            var sut = testObject.AddComponent<MoveByAction>();
            sut.duration = Sec(duration * 0.5f);
            sut.playRollback = true;
            sut.rollbackDuration = Sec(duration * 0.5f);
            sut.space = MoveByAction.Space.SelfFixed;
            sut.moveBy = moveBy;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }
    }
}
