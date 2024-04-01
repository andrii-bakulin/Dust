using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(OnCollisionEvent))]
    [CanEditMultipleObjects]
    public class OnCollisionEventEditor : OnCollideEventEditor
    {
        [MenuItem("Dust/Events/On Collision")]
        [MenuItem("GameObject/Dust/Events/On Collision")]
        public static void AddComponent()
        {
            AddComponentToSelectedOrNewObject("DuCollision", typeof(OnCollisionEvent));
        }
    }
}
