using System;
using NUnit.Framework;
using UnityEngine;

namespace Dust.Test.EditMode
{
    public class Performance
    {
        const int TRY_COUNT = 100_000_000;

        public struct StructInt
        {
            public float val;
        }
        
        [Test]
        public void TestFuncCallsInt()
        {
            float checkSum1 = 0, checkSum2 = 0, checkSum3 = 0;
            
            long time0 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            for (int i = 0; i < TRY_COUNT; i++)
            {
                StructInt r1 = FuncInt_Res(i, i);
                checkSum1 += r1.val;
            }
            
            long time1 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            for (int i = 0; i < TRY_COUNT; i++)
            {
                FuncInt_Out(i, i, out StructInt r2);
                checkSum2 += r2.val;
            }
            
            long time2 = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            StructInt r3;
            r3.val = 0f;
            for (int i = 0; i < TRY_COUNT; i++)
            {
                FuncInt_Ref(i, i, ref r3);
                checkSum3 += r3.val;
            }
            
            long time3 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            Debug.Log("Time Res: " + (time1 - time0) + " / CheckSum = " + checkSum1);
            Debug.Log("Time Out: " + (time2 - time1) + " / CheckSum = " + checkSum2);
            Debug.Log("Time Ref: " + (time3 - time2) + " / CheckSum = " + checkSum3);

            Assert.That(true, Is.True);
        }

        StructInt FuncInt_Res(int a, int b)
        {
            StructInt res;
            res.val = a + b;
            return res;
        }
        
        void FuncInt_Out(int a, int b, out StructInt res)
        {
            res.val = a + b;
        }
        
        void FuncInt_Ref(int a, int b, ref StructInt res)
        {
            res.val = a + b;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        public struct StructIntColor
        {
            public float val;
            public Color col;
        }

        [Test]
        public void TestFuncCallsIntColor()
        {
            float checkSum1 = 0, checkSum2 = 0, checkSum3 = 0;
            
            long time0 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            for (int i = 0; i < TRY_COUNT; i++)
            {
                StructIntColor r1 = FuncIntColor_Res(i, i);
                checkSum1 += r1.val;
            }
            
            long time1 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            for (int i = 0; i < TRY_COUNT; i++)
            {
                FuncIntColor_Out(i, i, out StructIntColor r2);
                checkSum2 += r2.val;
            }
            
            long time2 = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            StructIntColor r3;
            r3.val = 0f;
            r3.col = Color.clear;
            for (int i = 0; i < TRY_COUNT; i++)
            {
                FuncIntColor_Ref(i, i, ref r3);
                checkSum3 += r3.val;
            }
            
            long time3 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            Debug.Log("Time Res: " + (time1 - time0) + " / CheckSum = " + checkSum1);
            Debug.Log("Time Out: " + (time2 - time1) + " / CheckSum = " + checkSum2);
            Debug.Log("Time Ref: " + (time3 - time2) + " / CheckSum = " + checkSum3);

            Assert.That(true, Is.True);
        }

        StructIntColor FuncIntColor_Res(int a, int b)
        {
            StructIntColor res;
            res.val = a + b;
            res.col = Color.clear;
            return res;
        }
        
        void FuncIntColor_Out(int a, int b, out StructIntColor res)
        {
            res.val = a + b;
            res.col = Color.clear;
        }
        
        void FuncIntColor_Ref(int a, int b, ref StructIntColor res)
        {
            res.val = a + b;
            res.col = Color.clear;
        }
    }
}
