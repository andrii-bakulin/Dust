using UnityEngine;

namespace DustEngine
{
    public static class DuGradient
    {
        public static Gradient CreateBlackToRed()
            => CreateBetweenColors(Color.black, Color.red);

        public static Gradient CreateBlackToColor(Color color)
            => CreateBetweenColors(Color.black, color);

        public static Gradient CreateBetweenColors(Color color1, Color color2)
        {
            var gradient = new Gradient();

            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(color1, 0f),
                    new GradientColorKey(color2, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            );

            return gradient;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Gradient CreateRainbow()
            => CreateRainbow(0f, 1f);

        public static Gradient CreateRainbow(float minRange, float maxRange)
        {
            int blocksCount = 8;

            var gradient = new Gradient();
            gradient.mode = GradientMode.Blend;

            var colorKeys = new GradientColorKey[blocksCount];

            for (int i = 0; i < blocksCount; i++)
            {
                float offsetCol = minRange + (maxRange - minRange) / (blocksCount - 1f) * i;
                offsetCol = Mathf.Repeat(offsetCol, 1f);

                float offsetPos = 1f / (blocksCount - 1f) * i;

                colorKeys[i] = new GradientColorKey(Color.HSVToRGB(offsetCol, 1f, 1f), offsetPos);
            }

            gradient.SetKeys(
                colorKeys,
                new[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            );

            return gradient;
        }

        public static Gradient CreateRandomSet()
        {
            int blocksCount = 8;

            var gradient = new Gradient();
            gradient.mode = GradientMode.Fixed;

            var colorKeys = new GradientColorKey[blocksCount];

            for (int i = 0; i < blocksCount; i++)
            {
                float offset = 1f / (blocksCount - 1) * i;
                colorKeys[i] = new GradientColorKey(Color.HSVToRGB(offset, offset * 2.79f % 1f, offset * 3.37f % 1f), offset);
            }

            gradient.SetKeys(
                colorKeys,
                new[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            );

            return gradient;
        }
    }
}
