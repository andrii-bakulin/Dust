using UnityEngine;

namespace DustEngine
{
    // https://en.wikipedia.org/wiki/Alpha_compositing#Alpha_blending
    // PS: "SRC over DST" [or] "colorOVER over colorBASE"
    public static class ColorBlend
    {
        public static Color AlphaBlend(Color colorBase, Color colorOver)
        {
            Color res = new Color(0f, 0f, 0f, 0f);

            res.a = colorOver.a + colorBase.a * (1 - colorOver.a);

            if (DuMath.IsZero(res.a))
                return res;

            res.r = (colorBase.r * colorBase.a * (1 - colorOver.a) + colorOver.r * colorOver.a) / res.a;
            res.g = (colorBase.g * colorBase.a * (1 - colorOver.a) + colorOver.g * colorOver.a) / res.a;
            res.b = (colorBase.b * colorBase.a * (1 - colorOver.a) + colorOver.b * colorOver.a) / res.a;

            return res;
        }

        public static Color Add(Color colorBase, Color colorOver)
        {
            Color res = new Color(0f, 0f, 0f, 0f);

            res.a = colorOver.a + colorBase.a * (1 - colorOver.a);

            if (DuMath.IsZero(res.a))
                return res;

            res.r = (colorBase.r * colorBase.a + colorOver.r * colorOver.a) / res.a;
            res.g = (colorBase.g * colorBase.a + colorOver.g * colorOver.a) / res.a;
            res.b = (colorBase.b * colorBase.a + colorOver.b * colorOver.a) / res.a;

            return res;
        }

        public static Color Subtract(Color colorBase, Color colorOver)
        {
            Color res = new Color(0f, 0f, 0f, 0f);

            res.a = colorOver.a + colorBase.a * (1 - colorOver.a);

            if (DuMath.IsZero(res.a))
                return res;

            res.r = (colorBase.r * colorBase.a - colorOver.r * colorOver.a) / res.a;
            res.g = (colorBase.g * colorBase.a - colorOver.g * colorOver.a) / res.a;
            res.b = (colorBase.b * colorBase.a - colorOver.b * colorOver.a) / res.a;

            return res;
        }

        public static Color Multiply(Color colorBase, Color colorOver)
        {
            if (DuMath.IsZero(colorOver.a))
                return colorBase;

            // Calculate multiplication and base to colorOver.alpha
            Color res;
            res.r = (colorBase.r * colorBase.a * colorOver.r * colorOver.a) / colorOver.a;
            res.g = (colorBase.g * colorBase.a * colorOver.g * colorOver.a) / colorOver.a;
            res.b = (colorBase.b * colorBase.a * colorOver.b * colorOver.a) / colorOver.a;
            res.a = colorOver.a;

            // LERP to required value
            return Color.Lerp(colorBase, res, colorOver.a);
        }

        public static Color Min(Color colorBase, Color colorOver)
        {
            if (colorOver.Equals(colorBase))
                return colorOver;

            Color res = new Color(0f, 0f, 0f, 0f);

            res.a = colorOver.a + colorBase.a * (1 - colorOver.a);

            if (DuMath.IsZero(res.a))
                return res;

            res.r = Mathf.Min(colorBase.r * colorBase.a, colorOver.r * colorOver.a) / res.a;
            res.g = Mathf.Min(colorBase.g * colorBase.a, colorOver.g * colorOver.a) / res.a;
            res.b = Mathf.Min(colorBase.b * colorBase.a, colorOver.b * colorOver.a) / res.a;

            return res;
        }

        public static Color Max(Color colorBase, Color colorOver)
        {
            if (colorOver.Equals(colorBase))
                return colorOver;

            Color res = new Color(0f, 0f, 0f, 0f);

            res.a = colorOver.a + colorBase.a * (1 - colorOver.a);

            if (DuMath.IsZero(res.a))
                return res;

            res.r = Mathf.Max(colorBase.r * colorBase.a, colorOver.r * colorOver.a) / res.a;
            res.g = Mathf.Max(colorBase.g * colorBase.a, colorOver.g * colorOver.a) / res.a;
            res.b = Mathf.Max(colorBase.b * colorBase.a, colorOver.b * colorOver.a) / res.a;

            return res;
        }

        public static Color MinAfterBlend(Color colorBase, Color colorOver)
        {
            return Min(colorBase, AlphaBlend(colorBase, colorOver));
        }

        public static Color MaxAfterBlend(Color colorBase, Color colorOver)
        {
            return Max(colorBase, AlphaBlend(colorBase, colorOver));
        }
    }
}
