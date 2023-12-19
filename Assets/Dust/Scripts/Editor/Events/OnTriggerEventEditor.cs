using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(OnTriggerEvent))]
    [CanEditMultipleObjects]
    public class OnTriggerEventEditor : OnColliderEventEditor
    {
        [MenuItem("Dust/Events/On Trigger")]
        public static void AddComponent()
        {
            AddComponentToSelectedOrNewObject("DuTrigger", typeof(OnTriggerEvent));
        }
    }
}
