using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(OnCollision2DEvent))]
    [CanEditMultipleObjects]
    public class OnCollision2DEventEditor : OnColliderEventEditor
    {
        [MenuItem("Dust/Events/On Collision 2D")]
        public static void AddComponent()
        {
            AddComponentToSelectedOrNewObject("DuCollision2D", typeof(OnCollision2DEvent));
        }
    }
}
