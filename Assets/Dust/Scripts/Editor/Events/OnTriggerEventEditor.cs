using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(OnTriggerEvent))]
    [CanEditMultipleObjects]
    public class OnTriggerEventEditor : OnCollideEventEditor
    {
        [MenuItem("Dust/Events/On Trigger")]
        [MenuItem("GameObject/Dust/Events/On Trigger")]
        public static void AddComponent()
        {
            AddComponentToSelectedOrNewObject("DuTrigger", typeof(OnTriggerEvent));
        }
    }
}
