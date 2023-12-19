using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public static class Dust
    {
        public static bool IsNull(System.Object obj)
        {
            return obj is null;
        }

        public static bool IsNotNull(System.Object obj)
        {
            return !(obj is null);
        }

        public static bool IsNull(Object obj)
        {
            return obj == null;
        }

        public static bool IsNotNull(Object obj)
        {
            return obj != null;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        public static bool IsPrefab(GameObject obj)
        {
            return PrefabUtility.IsPartOfPrefabAsset(obj);
        }
#endif

        public static void DestroyObjectWhenReady(GameObject obj)
        {
#if UNITY_EDITOR
            Object.DestroyImmediate(obj);
#else
            Object.Destroy(obj);
#endif
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        public static class Debug
        {
            public static void Notice(object message)
            {
                UnityEngine.Debug.Log("Dust: " + message);
            }

            public static void Notice(object message, Object context)
            {
                UnityEngine.Debug.Log("Dust: " + message, context);
            }

            public static void Warning(object message)
            {
                UnityEngine.Debug.LogWarning("Dust: " + message);
            }

            public static void Warning(object message, Object context)
            {
                UnityEngine.Debug.LogWarning("Dust: " + message, context);
            }

            public static void Error(object message)
            {
                UnityEngine.Debug.LogError("Dust: " + message);
            }

            public static void Error(object message, Object context)
            {
                UnityEngine.Debug.LogError("Dust: " + message, context);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            public static void Checkpoint(string scope, string eventId) => Checkpoint(scope, eventId, "");

            public static void Checkpoint(string scope, string eventId, object message)
            {
                UnityEngine.Debug.Log($"Dust: {scope}: {eventId}: {message}");
            }

            public static void CheckpointWarning(string scope, string eventId) => CheckpointWarning(scope, eventId, "");

            public static void CheckpointWarning(string scope, string eventId, object message)
            {
                UnityEngine.Debug.LogWarning($"Dust: {scope}: {eventId}: {message}");
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            public static void StrangeState(string scope, object message)
            {
                UnityEngine.Debug.LogError("Dust: STRANGE STATE [" + scope + "]" + message);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            public static DuStopwatch StartTimer()
            {
                var duStopwatch = new DuStopwatch();
                duStopwatch.Start();
                return duStopwatch;
            }

            public class DuStopwatch
            {
                private System.Diagnostics.Stopwatch _timer;

                public void Start()
                {
                    _timer = new System.Diagnostics.Stopwatch();
                    _timer.Start();
                }

                public float Stop()
                {
                    _timer.Stop();

                    double millisecond = _timer.ElapsedMilliseconds;

                    return (float) millisecond / 1000f;
                }

                public void StopAndLog(string scope)
                {
                    _timer.Stop();

                    double millisecond = _timer.ElapsedMilliseconds;
                    string second = (millisecond / 1000).ToString("F3");

                    UnityEngine.Debug.LogWarning("Dust: Stopwatch [" + scope + "] = " + millisecond + " (" + second + " sec)");
                }
            }
        }
#endif
    }
}
