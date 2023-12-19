using UnityEngine;

namespace DustEngine
{
    public static class DuVector3_Extension
    {
        public static void duAbs(ref this Vector3 self)
        {
            self = DuVector3.Abs(self);
        }

        public static Vector3 duToAbs(this Vector3 self)
        {
            return DuVector3.Abs(self);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void duClamp01(ref this Vector3 self)
        {
            self = DuVector3.Clamp01(self);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void duFit01To(ref this Vector3 self, float min, float max)
            => duFit01To(ref self, min, max, false);

        public static void duFit01To(ref this Vector3 self, float min, float max, bool clamped)
        {
            self.x = DuMath.Fit01To(min, max, self.x, clamped);
            self.y = DuMath.Fit01To(min, max, self.y, clamped);
            self.z = DuMath.Fit01To(min, max, self.z, clamped);
        }

        public static void duFit01To(ref this Vector3 self, Vector3 min, Vector3 max)
            => duFit01To(ref self, min, max, false);

        public static void duFit01To(ref this Vector3 self, Vector3 min, Vector3 max, bool clamped)
        {
            self.x = DuMath.Fit01To(min.x, max.x, self.x, clamped);
            self.y = DuMath.Fit01To(min.y, max.y, self.y, clamped);
            self.z = DuMath.Fit01To(min.z, max.z, self.z, clamped);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void duInverseScale(ref this Vector3 self, Vector3 scale)
        {
            self.x /= scale.x;
            self.y /= scale.y;
            self.z /= scale.z;
        }

        public static Vector3 duToRound(this Vector3 self, int roundToDigits)
        {
            self.x = DuMath.Round(self.x, roundToDigits);
            self.y = DuMath.Round(self.y, roundToDigits);
            self.z = DuMath.Round(self.z, roundToDigits);
            return self;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Color duToColor(this Vector3 self)
            => duToColor(self, 1.0f);

        public static Color duToColor(this Vector3 self, float alpha)
        {
            return new Color(self.x, self.y, self.z, alpha);
        }
    }
}
