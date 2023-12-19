using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(OnCollisionEvent))]
    [CanEditMultipleObjects]
    public class OnCollisionEventEditor : OnColliderEventEditor
    {
        [MenuItem("Dust/Events/On Collision")]
        public static void AddComponent()
        {
            AddComponentToSelectedOrNewObject("DuCollision", typeof(OnCollisionEvent));
        }
    }
}
