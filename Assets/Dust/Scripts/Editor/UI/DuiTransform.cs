using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(Transform))]
    [CanEditMultipleObjects]
    public class DuiTransform : DuEditor
    {
        // @Thanks for:
        // https://forum.unity.com/threads/extending-instead-of-replacing-built-in-inspectors.407612/

        //Unity's built-in editor
        private Editor _defaultEditor;

        private bool _meshInfoRead = false;
        private string _meshInfoMessage = "";

        void OnEnable()
        {
            //When this inspector is created, also create the built-in inspector

            _defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
        }

        void OnDisable()
        {
            //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
            //Also, make sure to call any required methods like OnDisable

            MethodInfo disableMethod = _defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (disableMethod != null)
                disableMethod.Invoke(_defaultEditor,null);

            DestroyImmediate(_defaultEditor);
        }

        public override void OnInspectorGUI()
        {
            _defaultEditor.OnInspectorGUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Extend UI

            bool isRequireShowMeshInfo = SessionState.GetBool("DuiTransform.ShowMeshInfo", false);

            DustGUI.BeginHorizontal();
            {
                if (DustGUI.IconButton(UI.Icons.TRANSFORM_RESET))
                {
                    foreach (var subTarget in targets)
                    {
                        Undo.RecordObject(subTarget, "Reset Transform");
                        DuTransform.Reset(subTarget as Transform);
                    }
                }

                if (DustGUI.IconButton(UI.Icons.GAME_OBJECT_STATS, isRequireShowMeshInfo ? DustGUI.ButtonState.Pressed : DustGUI.ButtonState.Normal))
                {
                    isRequireShowMeshInfo = !isRequireShowMeshInfo;
                    SessionState.SetBool("DuiTransform.ShowMeshInfo", isRequireShowMeshInfo);
                }
            }
            DustGUI.EndHorizontal();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (isRequireShowMeshInfo)
            {
                if (!_meshInfoRead)
                {
                    var total = new DuGameObject.Data();

                    foreach (var subTarget in targets)
                    {
                        total += DuGameObject.GetStats((subTarget as Transform).gameObject, true);
                    }

                    if (total.meshesCount > 0)
                    {
                        _meshInfoMessage += "Meshes Count: " + total.meshesCount.ToString() + "\n";
                        _meshInfoMessage += "Vertex Count: " + total.vertexCount.ToString() + "\n";

                        if (total.unreadableCount > 0)
                        {
                            _meshInfoMessage += "Triangles Count: " + total.triangleCount.ToString() + "*\n";

                            if (total.unreadableCount == 1)
                                _meshInfoMessage += "* 1 mesh is unreadable, so value is not exact";
                            else
                                _meshInfoMessage += "* " + total.unreadableCount + " meshes are unreadable, so value is not exact";
                        }
                        else
                        {
                            _meshInfoMessage += "Triangles Count: " + total.triangleCount.ToString();
                        }
                    }
                    else
                    {
                        _meshInfoMessage += "Meshes not found";
                    }

                    _meshInfoRead = true;
                }

                DustGUI.HelpBoxInfo(_meshInfoMessage);
            }
        }
    }
}
