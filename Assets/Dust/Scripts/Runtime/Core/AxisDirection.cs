using UnityEngine;

namespace DustEngine
{
    public static class AxisDirection
    {
        public static string ToString(Axis3xDirection direction)
        {
            switch (direction)
            {
                default:
                case Axis3xDirection.X: return "X";
                case Axis3xDirection.Y: return "Y";
                case Axis3xDirection.Z: return "Z";
            }
        }

        public static string ToString(Axis6xDirection direction)
        {
            switch (direction)
            {
                default:
                case Axis6xDirection.XPlus:  return "X+";
                case Axis6xDirection.XMinus: return "X-";

                case Axis6xDirection.YPlus:  return "Y+";
                case Axis6xDirection.YMinus: return "Y-";

                case Axis6xDirection.ZPlus:  return "Z+";
                case Axis6xDirection.ZMinus: return "Z-";
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Axis6xDirection ConvertToAxis6(Axis3xDirection direction)
        {
            switch (direction)
            {
                default:
                case Axis3xDirection.X: return Axis6xDirection.XPlus;
                case Axis3xDirection.Y: return Axis6xDirection.YPlus;
                case Axis3xDirection.Z: return Axis6xDirection.ZPlus;
            }
        }

        public static Axis3xDirection ConvertToAxis3(Axis6xDirection direction)
        {
            switch (direction)
            {
                default:
                case Axis6xDirection.XPlus:
                case Axis6xDirection.XMinus: return Axis3xDirection.X;

                case Axis6xDirection.YPlus:
                case Axis6xDirection.YMinus: return Axis3xDirection.Y;

                case Axis6xDirection.ZPlus:
                case Axis6xDirection.ZMinus: return Axis3xDirection.Z;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Vector3 ConvertFromAxisXPlusToDirection(Axis3xDirection direction, Vector3 originPoint)
        {
            Vector3 convertedPoint = originPoint;

            switch (direction)
            {
                default:
                case Axis3xDirection.X: break;
                case Axis3xDirection.Y: convertedPoint.x = +originPoint.y; convertedPoint.y = +originPoint.x; break;
                case Axis3xDirection.Z: convertedPoint.x = +originPoint.z; convertedPoint.z = +originPoint.x; break;
            }

            return convertedPoint;
        }

        public static Vector3 ConvertFromDirectionToAxisXPlus(Axis3xDirection direction, Vector3 convertedPoint)
        {
            Vector3 originPoint = convertedPoint;

            switch (direction)
            {
                default:
                case Axis3xDirection.X: break;
                case Axis3xDirection.Y: originPoint.y = +convertedPoint.x; originPoint.x = +convertedPoint.y; break;
                case Axis3xDirection.Z: originPoint.z = +convertedPoint.x; originPoint.x = +convertedPoint.z; break;
            }

            return originPoint;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Vector3 ConvertFromAxisXPlusToDirection(Axis6xDirection direction, Vector3 originPoint)
        {
            Vector3 convertedPoint = originPoint;

            switch (direction)
            {
                case Axis6xDirection.XPlus:  break;
                case Axis6xDirection.XMinus: convertedPoint.x = -originPoint.x; break;
                case Axis6xDirection.YPlus:  convertedPoint.x = +originPoint.y; convertedPoint.y = +originPoint.x; break;
                case Axis6xDirection.YMinus: convertedPoint.x = +originPoint.y; convertedPoint.y = -originPoint.x; break;
                case Axis6xDirection.ZPlus:  convertedPoint.x = +originPoint.z; convertedPoint.z = +originPoint.x; break;
                case Axis6xDirection.ZMinus: convertedPoint.x = +originPoint.z; convertedPoint.z = -originPoint.x; break;
            }

            return convertedPoint;
        }

        public static Vector3 ConvertFromDirectionToAxisXPlus(Axis6xDirection direction, Vector3 convertedPoint)
        {
            Vector3 originPoint = convertedPoint;

            switch (direction)
            {
                case Axis6xDirection.XPlus:  break;
                case Axis6xDirection.XMinus: originPoint.x = -convertedPoint.x; break;
                case Axis6xDirection.YPlus:  originPoint.y = +convertedPoint.x; originPoint.x = +convertedPoint.y; break;
                case Axis6xDirection.YMinus: originPoint.y = +convertedPoint.x; originPoint.x = -convertedPoint.y; break;
                case Axis6xDirection.ZPlus:  originPoint.z = +convertedPoint.x; originPoint.x = +convertedPoint.z; break;
                case Axis6xDirection.ZMinus: originPoint.z = +convertedPoint.x; originPoint.x = -convertedPoint.z; break;
            }

            return originPoint;
        }
    }
}
