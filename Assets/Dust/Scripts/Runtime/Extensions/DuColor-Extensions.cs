using UnityEngine;

namespace DustEngine
{
    public static class DuColor_Extensions
    {
        public static void duClamp(ref this Color self, float min, float max)
        {
            self = DuColor.Clamp(self, min, max);
        }

        public static void duClamp(ref this Color self, Color min, Color max)
        {
            self = DuColor.Clamp(self, min, max);
        }

        public static void duClamp01(ref this Color self)
        {
            self.duClamp(0f, 1f);
        }

        public static Color duClamped01(this Color self)
        {
            self.duClamp01();
            return self;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void duInvertRGB(ref this Color self)
        {
            self.r = 1f - self.r;
            self.g = 1f - self.g;
            self.b = 1f - self.b;
        }

        public static void duInvertRGBA(ref this Color self)
        {
            self.r = 1f - self.r;
            self.g = 1f - self.g;
            self.b = 1f - self.b;
            self.a = 1f - self.a;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Color duToRGBWithoutAlpha(this Color self)
        {
            self.r *= self.a;
            self.g *= self.a;
            self.b *= self.a;
            self.a = 1f;
            return self;
        }

        public static Gradient duToGradient(this Color self)
        {
            var gradient = new Gradient();

            gradient.SetKeys(
                new[] {new GradientColorKey(self, 0f)},
                new[] {new GradientAlphaKey(1f, 0f)});

            return gradient;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Vector3 duToVector3(this Color self)
            => duToVector3(self, 2);

        public static Vector3 duToVector3(this Color self, int roundToDigits)
        {
            var v = new Vector3();
            v.x = DuMath.Round(self.r, roundToDigits);
            v.y = DuMath.Round(self.g, roundToDigits);
            v.z = DuMath.Round(self.b, roundToDigits);
            return v;
        }

        public static Vector3Int duToVector3Int(this Color self)
        {
            var v = new Vector3Int();
            v.x = Mathf.RoundToInt(self.r);
            v.y = Mathf.RoundToInt(self.g);
            v.z = Mathf.RoundToInt(self.b);
            return v;
        }
    }
}
