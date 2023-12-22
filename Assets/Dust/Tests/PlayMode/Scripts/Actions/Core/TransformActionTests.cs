using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.Actions
{
    public class TransformActionTests : ActionTests
    {
        protected const string ObjectTopLevel = "Top-Level";
        protected const string ObjectSubLevel = "Sub-Level";

        protected GameObject topLevelObj;
        protected GameObject middleLevelObj;
        protected GameObject lastLevelObj;
        
        //--------------------------------------------------------------------------------------------------------------

        [UnitySetUp]
        protected IEnumerator SetupScene()
        {
            topLevelObj = new GameObject("TopLevelObj");
            topLevelObj.transform.localPosition = new Vector3(1f, 2f, 3f);
            topLevelObj.transform.localRotation = Quaternion.Euler(45f, 0f, 0f);
            //topLevelObj.transform.localScale = new Vector3(1f, 2f, 3f);

            middleLevelObj = new GameObject("MiddleLevelObj");
            middleLevelObj.transform.parent = topLevelObj.transform;
            middleLevelObj.transform.localPosition = new Vector3(-1.5f, -2.5f, -3.5f);
            middleLevelObj.transform.localRotation = Quaternion.Euler(0f, 0f, 45f);
            //middleLevelObj.transform.localScale = new Vector3(2f, 1f, 0.5f);

            lastLevelObj = new GameObject("LastLevelObj");
            lastLevelObj.transform.parent = middleLevelObj.transform;
            lastLevelObj.transform.localPosition = new Vector3(1.25f, 2.35f, 3.45f);
            lastLevelObj.transform.localRotation = Quaternion.Euler(10f, 20f, 30f);
            //lastLevelObj.transform.localScale = new Vector3(1f, 1.5f, 1f);
            
            yield break;
        }

        [UnityTearDown]
        protected IEnumerator ReleaseScene()
        {
            UnityEngine.Object.DestroyImmediate(lastLevelObj);   
            UnityEngine.Object.DestroyImmediate(middleLevelObj);   
            UnityEngine.Object.DestroyImmediate(topLevelObj);

            yield break;
        }

        protected GameObject GetTestGameObject(string objLevelId)
        {
            switch (objLevelId)
            {
                case ObjectTopLevel: return topLevelObj;
                case ObjectSubLevel: return lastLevelObj;
            }

            throw new Exception("Undefined ObjLevelId");
        }
    }
}
