using UnityEngine;

namespace DustEngine
{
    public static class DuColor
    {
        public static Color RandomColor()
        {
            return Color.HSVToRGB(Random.Range(0f, 1f), 1f, 1f);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Color InvertRGB(Color color)
        {
            color.duInvertRGB();
            return color;
        }

        public static Color InvertRGBA(Color color)
        {
            color.duInvertRGBA();
            return color;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Color Min(Color a, Color b)
        {
            Color r;
            r.r = Mathf.Min(a.r, b.r);
            r.g = Mathf.Min(a.g, b.g);
            r.b = Mathf.Min(a.b, b.b);
            r.a = Mathf.Min(a.a, b.a);
            return r;
        }

        public static Color Max(Color a, Color b)
        {
            Color r;
            r.r = Mathf.Max(a.r, b.r);
            r.g = Mathf.Max(a.g, b.g);
            r.b = Mathf.Max(a.b, b.b);
            r.a = Mathf.Max(a.a, b.a);
            return r;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Color Clamp(Color color, float min, float max)
        {
            color.r = Mathf.Clamp(color.r, min, max);
            color.g = Mathf.Clamp(color.g, min, max);
            color.b = Mathf.Clamp(color.b, min, max);
            color.a = Mathf.Clamp(color.a, min, max);
            return color;
        }

        public static Color Clamp(Color color, Color min, Color max)
        {
            color.r = Mathf.Clamp(color.r, min.r, max.r);
            color.g = Mathf.Clamp(color.g, min.g, max.g);
            color.b = Mathf.Clamp(color.b, min.b, max.b);
            color.a = Mathf.Clamp(color.a, min.a, max.a);
            return color;
        }
    }
}
