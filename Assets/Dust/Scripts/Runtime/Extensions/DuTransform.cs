using System;
using UnityEngine;

namespace Dust
{
    public static class DuTransform
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        public enum Mode
        {
            Set = 0,
            Add = 1,
        }
        
        //--------------------------------------------------------------------------------------------------------------

        public static void SetGlobalScale(Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;

            Vector3 curScale = transform.lossyScale;

            transform.localScale = new Vector3(
                DuMath.IsNotZero(curScale.x) ? globalScale.x / curScale.x : 0f, 
                DuMath.IsNotZero(curScale.y) ? globalScale.y / curScale.y : 0f, 
                DuMath.IsNotZero(curScale.z) ? globalScale.z / curScale.z : 0f);
        }

        public static void Reset(Transform tr)
        {
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
            tr.localScale = Vector3.one;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        public static void UpdatePosition(Transform transform, Vector3 value, Space space, Mode mode)
        {
            if (Dust.IsNull(transform))
                return;

            var position = space switch
            {
                DuTransform.Space.World => transform.position,
                DuTransform.Space.Local => transform.localPosition,
                _ => transform.localPosition,
            };

            position = mode switch
            {
                DuTransform.Mode.Set => value,
                DuTransform.Mode.Add => position + value,
                _ => position
            };

            switch (space)
            {
                case DuTransform.Space.World:
                    transform.position = position;
                    break;
                
                case DuTransform.Space.Local:
                    transform.localPosition = position;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void UpdateRotation(Transform transform, Vector3 value, Space space, Mode mode)
        {
            if (Dust.IsNull(transform))
                return;

            var rotation = space switch
            {
                DuTransform.Space.World => transform.eulerAngles,
                DuTransform.Space.Local => transform.localEulerAngles,
                _ => transform.localEulerAngles,
            };

            rotation = mode switch
            {
                DuTransform.Mode.Set => value,
                DuTransform.Mode.Add => rotation + value,
                _ => rotation
            };

            switch (space)
            {
                case DuTransform.Space.World:
                    transform.eulerAngles = rotation;
                    break;
                
                case DuTransform.Space.Local:
                    transform.localEulerAngles = rotation;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void UpdateScale(Transform transform, Vector3 value, Space space, Mode mode)
        {
            if (Dust.IsNull(transform))
                return;

            var scale = space switch
            {
                DuTransform.Space.World => transform.lossyScale,
                DuTransform.Space.Local => transform.localScale,
                _ => transform.localScale,
            };

            scale = mode switch
            {
                DuTransform.Mode.Set => value,
                DuTransform.Mode.Add => scale + value,
                _ => scale
            };

            switch (space)
            {
                case DuTransform.Space.World:
                    DuTransform.SetGlobalScale(transform, scale);
                    break;
                
                case DuTransform.Space.Local:
                    transform.localScale = scale;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
