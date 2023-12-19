using UnityEngine;

namespace DustEngine
{
    public static class DuVector2
    {
        public static Vector2 Abs(Vector2 value)
        {
            value.x = Mathf.Abs(value.x);
            value.y = Mathf.Abs(value.y);
            return value;
        }

        // Thank for https://www.habrador.com/tutorials/math/5-line-line-intersection/
        public static bool IsIntersecting(Vector2 line1point1, Vector2 line1point2, Vector2 line2point1, Vector2 line2point2, out Vector2 intersectPoint)
        {
            intersectPoint = Vector2.zero;

            // Direction of the lines
            Vector2 line1direction = (line1point2 - line1point1).normalized;
            Vector2 line2direction = (line2point2 - line2point1).normalized;

            // If we know the direction we can get the normal vector to each line
            Vector2 line1normal = new Vector2(-line1direction.y, line1direction.x);
            Vector2 line2normal = new Vector2(-line2direction.y, line2direction.x);

            // Step 2: are the lines parallel? -> no solutions
            if (IsParallel(line1normal, line2normal))
                return false;

            // Step 3: are the lines the same line? -> infinite amount of solutions
            // Pick one point on each line and test if the vector between the points is orthogonal to one of the normals
            if (IsOrthogonal(line1point1 - line2point1, line1normal))
                return false;

            // Step 1: Rewrite the lines to a general form: Ax + By = k1 and Cx + Dy = k2
            // The normal vector is the A, B
            float A = line1normal.x;
            float B = line1normal.y;

            float C = line2normal.x;
            float D = line2normal.y;

            // To get k we just use one point on the line
            float k1 = (A * line1point1.x) + (B * line1point1.y);
            float k2 = (C * line2point1.x) + (D * line2point1.y);

            // Step 4: calculate the intersection point -> one solution
            intersectPoint.x = (+ D * k1 - B * k2) / (A * D - B * C);
            intersectPoint.y = (- C * k1 + A * k2) / (A * D - B * C);

            // Step 5: but we have line segments so we have to check if the intersection point is within the segment
            return IsBetween(line1point1, line1point2, intersectPoint) && IsBetween(line2point1, line2point2, intersectPoint);
        }

        // Are 2 vectors parallel?
        public static bool IsParallel(Vector2 v1, Vector2 v2)
        {
            // 2 vectors are parallel if the angle between the vectors are 0 or 180 degrees
            return Mathf.Approximately(Vector2.Angle(v1, v2), 0f) || Mathf.Approximately(Vector2.Angle(v1, v2), 180f);
        }

        // Are 2 vectors orthogonal?
        public static bool IsOrthogonal(Vector2 v1, Vector2 v2)
        {
            // 2 vectors are orthogonal is the dot product is 0
            // We have to check if close to 0 because of floating numbers
            return DuMath.IsZero(Vector2.Dot(v1, v2));
        }

        // Is a point c between 2 other points a and b?
        public static bool IsBetween(Vector2 a, Vector2 b, Vector2 c)
        {
            // Entire line segment
            Vector2 ab = b - a;

            // The intersection and the first point
            Vector2 ac = c - a;

            // Need to check 2 things:
            // 1. If the vectors are pointing in the same direction = if the dot product is positive
            // 2. If the length of the vector between the intersection and the first point is smaller than the entire line
            return Vector2.Dot(ab, ac) > 0f && ab.sqrMagnitude >= ac.sqrMagnitude;
        }
    }
}
