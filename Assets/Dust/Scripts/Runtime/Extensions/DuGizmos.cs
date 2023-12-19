using UnityEngine;

namespace DustEngine
{
    public static class DuGizmos
    {
        //--------------------------------------------------------------------------------------------------------------
        // All Gizmos by default draw elements in [X+]-axis-direction

        public static void DrawWireCylinder(float radius, float height, Vector3 center, Axis3xDirection direction, int circlePoints, int edgesCount)
        {
            DrawWireCylinder(radius, height, center, AxisDirection.ConvertToAxis6(direction), circlePoints, edgesCount);
        }

        public static void DrawWireCylinder(float radius, float height, Vector3 center, Axis6xDirection direction, int circlePoints, int edgesCount)
        {
            Vector3 pHeight = new Vector3(height / 2f, 0f, 0f);
            pHeight = AxisDirection.ConvertFromAxisXPlusToDirection(direction, pHeight);

            for (int i = 0; i < circlePoints; i++)
            {
                float offset0 = (float) i / circlePoints;
                float offset1 = (float) (i + 1) / circlePoints;

                Vector3 p0 = GetCirclePointByOffset(offset0, direction) * radius;
                Vector3 p1 = GetCirclePointByOffset(offset1, direction) * radius;

                Gizmos.DrawLine(center + p0 + pHeight, center + p1 + pHeight);
                Gizmos.DrawLine(center + p0 - pHeight, center + p1 - pHeight);
            }

            for (int i = 0; i < edgesCount; i++)
            {
                float offset0 = (float) i / edgesCount;

                Vector3 p0 = GetCirclePointByOffset(offset0, direction) * radius;

                Gizmos.DrawLine(center + p0 + pHeight, center + p0 - pHeight);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void DrawWireCone(float radius, float height, Vector3 center, Axis6xDirection direction, int circlePoints, int edgesCount)
        {
            Vector3 pHeight = new Vector3(height / 2f, 0f, 0f);
            pHeight = AxisDirection.ConvertFromAxisXPlusToDirection(direction, pHeight);

            for (int i = 0; i < circlePoints; i++)
            {
                float offset0 = (float) i / circlePoints;
                float offset1 = (float) (i + 1) / circlePoints;

                Vector3 p0 = GetCirclePointByOffset(offset0, direction);
                Vector3 p1 = GetCirclePointByOffset(offset1, direction);

                // Middle
                Gizmos.DrawLine(center + p0 * radius / 2f, center + p1 * radius / 2f);

                // Base
                Gizmos.DrawLine(center + p0 * radius - pHeight, center + p1 * radius - pHeight);
            }

            for (int i = 0; i < edgesCount; i++)
            {
                float offset0 = (float) i / edgesCount;

                Vector3 p0 = GetCirclePointByOffset(offset0, direction) * radius;

                Gizmos.DrawLine(center + pHeight, center + p0 - pHeight);
            }
        }

        public static void DrawWirePyramid(float radius, float height, Vector3 center, Axis6xDirection direction, int sidesCount)
        {
            sidesCount = Mathf.Max(3, sidesCount);

            Vector3 pHeight = new Vector3(height / 2f, 0f, 0f);
            pHeight = AxisDirection.ConvertFromAxisXPlusToDirection(direction, pHeight);

            for (int i = 0; i < sidesCount; i++)
            {
                float offset0 = (float) i / sidesCount;
                float offset1 = (float) (i + 1) / sidesCount;

                Vector3 p0 = GetCirclePointByOffset(offset0, direction);
                Vector3 p1 = GetCirclePointByOffset(offset1, direction);

                Gizmos.DrawLine(center + p0 * radius - pHeight, center + p1 * radius - pHeight);
            }

            for (int i = 0; i < sidesCount; i++)
            {
                float offset0 = (float) i / sidesCount;

                Vector3 p0 = GetCirclePointByOffset(offset0, direction) * radius;

                Gizmos.DrawLine(center + pHeight, center + p0 - pHeight);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void DrawWireTorus(float radius, float thickness, Vector3 center, Axis6xDirection direction, int circlePoints, int smallCirclePoints)
        {
            DrawWireTorus(radius, thickness, center, AxisDirection.ConvertToAxis3(direction), circlePoints, smallCirclePoints);
        }

        public static void DrawWireTorus(float radius, float thickness, Vector3 center, Axis3xDirection direction, int circlePoints, int smallCirclePoints)
        {
            Vector3 offset0;
            Vector3 offsetA;
            Vector3 offsetB;
            Axis3xDirection directionA;
            Axis3xDirection directionB;

            switch (direction)
            {
                default:
                case Axis3xDirection.X:
                    offset0 = new Vector3(1f, 0f, 0f);

                    offsetA = new Vector3(0f, 0f, 1f);
                    offsetB = new Vector3(0f, 1f, 0f);
                    directionA = Axis3xDirection.Y;
                    directionB = Axis3xDirection.Z;
                    break;

                case Axis3xDirection.Y:
                    offset0 = new Vector3(0f, 1f, 0f);

                    offsetA = new Vector3(0f, 0f, 1f);
                    offsetB = new Vector3(1f, 0f, 0f);
                    directionA = Axis3xDirection.X;
                    directionB = Axis3xDirection.Z;
                    break;

                case Axis3xDirection.Z:
                    offset0 = new Vector3(0f, 0f, 1f);

                    offsetA = new Vector3(0f, 1f, 0f);
                    offsetB = new Vector3(1f, 0f, 0f);
                    directionA = Axis3xDirection.X;
                    directionB = Axis3xDirection.Y;
                    break;
            }

            DrawCircle(radius + thickness, center, direction, 64);
            DrawCircle(radius - thickness, center, direction, 64);
            DrawCircle(radius, center + (offset0 * +thickness), direction, 64);
            DrawCircle(radius, center + (offset0 * -thickness), direction, 64);

            DrawCircle(thickness, center + radius * offsetA, directionA, 32);
            DrawCircle(thickness, center - radius * offsetA, directionA, 32);

            DrawCircle(thickness, center + radius * offsetB, directionB, 32);
            DrawCircle(thickness, center - radius * offsetB, directionB, 32);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void DrawRect(float width, float height, Vector3 center, Axis3xDirection direction)
            => DrawRect(width, height, center, AxisDirection.ConvertToAxis6(direction));

        public static void DrawRect(float width, float height, Vector3 center, Axis6xDirection direction)
            => DrawRect(new Vector2(width, height), center, direction);

        public static void DrawRect(Vector2 size, Vector3 center, Axis3xDirection direction)
            => DrawRect(size, center, AxisDirection.ConvertToAxis6(direction));

        public static void DrawRect(Vector2 size, Vector3 center, Axis6xDirection direction)
        {
            Vector3 p0 = new Vector3(0f, -size.y / 2f, -size.x / 2f);
            Vector3 p1 = new Vector3(0f, +size.y / 2f, -size.x / 2f);
            Vector3 p2 = new Vector3(0f, -size.y / 2f, +size.x / 2f);
            Vector3 p3 = new Vector3(0f, +size.y / 2f, +size.x / 2f);

            p0 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, p0);
            p1 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, p1);
            p2 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, p2);
            p3 = AxisDirection.ConvertFromAxisXPlusToDirection(direction, p3);

            Gizmos.DrawLine(center + p0, center + p1);
            Gizmos.DrawLine(center + p1, center + p3);
            Gizmos.DrawLine(center + p3, center + p2);
            Gizmos.DrawLine(center + p2, center + p0);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void DrawCircle(float radius, Vector3 center, Axis3xDirection direction, int circlePoints)
        {
            DrawCircle(radius, center, AxisDirection.ConvertToAxis6(direction), circlePoints);
        }

        public static void DrawCircle(float radius, Vector3 center, Axis6xDirection direction, int circlePoints)
        {
            for (int i = 0; i < circlePoints; i++)
            {
                float offset0 = (float) i / circlePoints;
                float offset1 = (float) (i + 1) / circlePoints;

                Vector3 p0 = GetCirclePointByOffset(offset0, direction) * radius;
                Vector3 p1 = GetCirclePointByOffset(offset1, direction) * radius;

                Gizmos.DrawLine(center + p0, center + p1);
                Gizmos.DrawLine(center + p0, center + p1);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Vector3 GetCirclePointByAngle(float angle, Axis3xDirection direction)
        {
            return GetCirclePointByOffset(angle / 360f, AxisDirection.ConvertToAxis6(direction));
        }

        public static Vector3 GetCirclePointByOffset(float offset, Axis3xDirection direction)
        {
            return GetCirclePointByOffset(offset, AxisDirection.ConvertToAxis6(direction));
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Vector3 GetCirclePointByAngle(float angle, Axis6xDirection direction)
        {
            return GetCirclePointByOffset(angle / 360f, direction);
        }

        public static Vector3 GetCirclePointByOffset(float offset, Axis6xDirection direction)
        {
            Vector3 point = new Vector3(0f, Mathf.Sin(Constants.PI2 * offset), Mathf.Cos(Constants.PI2 * offset));

            return AxisDirection.ConvertFromAxisXPlusToDirection(direction, point);
        }
    }
}
