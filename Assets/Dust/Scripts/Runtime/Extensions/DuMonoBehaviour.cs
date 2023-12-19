using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuMonoBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        public static void AddComponent(System.Type duComponentType)
        {
            if (Selection.gameObjects.Length == 0)
                return;

            foreach (var gameObject in Selection.gameObjects)
            {
                Undo.AddComponent(gameObject, duComponentType);
            }
        }

        public static void AddComponentToSelectedOrNewObject(string gameObjectName, System.Type duComponentType)
        {
            if (Selection.gameObjects.Length > 0)
            {
                foreach (var gameObject in Selection.gameObjects)
                {
                    Undo.AddComponent(gameObject, duComponentType);
                }
            }
            else
            {
                AddComponentToNewObject(gameObjectName, duComponentType);
            }
        }

        public static Component AddComponentToNewObject(string gameObjectName, System.Type duComponentType)
            => AddComponentToNewObject(gameObjectName, duComponentType, true);

        public static Component AddComponentToNewObject(string gameObjectName, System.Type duComponentType, bool fixUndoState)
        {
            var gameObject = new GameObject
            {
                name = gameObjectName
            };

            if (Dust.IsNotNull(Selection.activeGameObject))
                gameObject.transform.parent = Selection.activeGameObject.transform;

            DuTransform.Reset(gameObject.transform);

            var component = gameObject.AddComponent(duComponentType);

            if (fixUndoState)
                Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            Selection.activeGameObject = gameObject;
            return component;
        }

        protected static bool IsAllowExecMenuCommandOnce(MenuCommand menuCommand)
        {
            return Selection.objects.Length == 0 || menuCommand.context == Selection.objects[0];
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        protected static bool UpdatePropertyValue(ref bool originValue, bool newValue)
        {
            if (originValue.Equals(newValue))
                return false;

            originValue = newValue;
            return true;
        }

        protected static bool UpdatePropertyValue(ref int originValue, int newValue)
        {
            if (originValue.Equals(newValue))
                return false;

            originValue = newValue;
            return true;
        }

        protected static bool UpdatePropertyValue(ref float originValue, float newValue)
        {
            if (originValue.Equals(newValue))
                return false;

            originValue = newValue;
            return true;
        }

        protected static bool UpdatePropertyValue(ref Color originValue, Color newValue)
        {
            if (originValue.Equals(newValue))
                return false;

            originValue = newValue;
            return true;
        }

        protected static bool UpdatePropertyValue(ref Vector3 originValue, Vector3 newValue)
        {
            if (originValue.Equals(newValue))
                return false;

            originValue = newValue;
            return true;
        }

        protected static bool UpdatePropertyValue(ref Vector3Int originValue, Vector3Int newValue)
        {
            if (originValue.Equals(newValue))
                return false;

            originValue = newValue;
            return true;
        }

        protected static bool UpdatePropertyValue(ref GameObject originValue, GameObject newValue)
        {
            if (Dust.IsNotNull(originValue) && Dust.IsNotNull(newValue) && originValue.Equals(newValue))
                return false;

            originValue = newValue;
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void print_war(object message) => Debug.LogWarning(message);

        public static void print_err(object message) => Debug.LogError(message);

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        private float _editorUpdate_deltaTime;
        private double _editorUpdate_lastTimeStamp;

        protected bool isInEditorMode => !Application.isPlaying;

        protected void EditorUpdateReset()
        {
            _editorUpdate_deltaTime = 0f;
            _editorUpdate_lastTimeStamp = -1;
        }

        protected bool EditorUpdateTick(out float deltaTime)
        {
            deltaTime = 0f;

            if (_editorUpdate_lastTimeStamp <= 0)
            {
                _editorUpdate_lastTimeStamp = EditorApplication.timeSinceStartup;
                return false;
            }

            _editorUpdate_deltaTime += (float) (EditorApplication.timeSinceStartup - _editorUpdate_lastTimeStamp);
            _editorUpdate_lastTimeStamp = EditorApplication.timeSinceStartup;

            if (_editorUpdate_deltaTime < Constants.EDITOR_UPDATE_TIMEOUT)
                return false;

            deltaTime = _editorUpdate_deltaTime;
            _editorUpdate_deltaTime = 0f;

            return true;
        }
#endif
    }
}
