using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(OnTrigger2DEvent))]
    [CanEditMultipleObjects]
    public class OnTrigger2DEventEditor : OnColliderEventEditor
    {
        [MenuItem("Dust/Events/On Trigger 2D")]
        public static void AddComponent()
        {
            AddComponentToSelectedOrNewObject("DuTrigger2D", typeof(OnTrigger2DEvent));
        }
    }
}
