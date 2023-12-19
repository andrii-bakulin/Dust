using UnityEngine;

namespace DustEngine
{
    //
    // Agreement:
    // All methods return values between 0..+1
    // Only Wide methods return values between -1..+1
    //
    public class DuNoise
    {
        private readonly float _perlinNoise_XX;
        private readonly float _perlinNoise_XY;
        private readonly float _perlinNoise_YX;
        private readonly float _perlinNoise_YY;
        private readonly float _perlinNoise_ZX;
        private readonly float _perlinNoise_ZY;

        public DuNoise(int seed)
        {
            if (seed <= 0)
                seed = Random.Range(Constants.RANDOM_SEED_MIN, Constants.RANDOM_SEED_MAX);

            DuRandom duRandom = new DuRandom(seed);

            _perlinNoise_XX = duRandom.Range(-9999f, +9999f);
            _perlinNoise_XY = duRandom.Range(-9999f, +9999f);

            _perlinNoise_YX = duRandom.Range(-9999f, +9999f);
            _perlinNoise_YY = duRandom.Range(-9999f, +9999f);

            _perlinNoise_ZX = duRandom.Range(-9999f, +9999f);
            _perlinNoise_ZY = duRandom.Range(-9999f, +9999f);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Notice: Nov 2, 2020 in Unity.2020.1.6f1 I found that globally total avarage for unity Perlin noise move
        //         to average value 0.465f, so I forced try to move it to 0.5f

        public static float PerlinNoise(float x, float y)
        {
            return Mathf.Clamp01(Mathf.PerlinNoise(x, y) * 1.075268817204301f);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Perlin 1D noise
        // - input: float
        // - return: float

        public float Perlin1D(float v)
            => Perlin1D(v, 0f, 1f);

        public float Perlin1D(float v, float offset)
            => Perlin1D(v, offset, 1f);

        public float Perlin1D(float v, float offset, float power)
        {
            return ImproveResult(PerlinNoise(_perlinNoise_XX + v, _perlinNoise_XY + offset), power);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public float Perlin1D_asWide(float v)
            => Perlin1D_asWide(v, 0f, 1f);

        public float Perlin1D_asWide(float v, float offset)
            => Perlin1D_asWide(v, offset, 1f);

        public float Perlin1D_asWide(float v, float offset, float power)
            => Perlin1D(v, offset, power) * 2f - 1f;

        //--------------------------------------------------------------------------------------------------------------
        // Perlin 1D noise
        // - input: float
        // - return: Vector3

        public Vector3 Perlin1D_asVector3(float v)
            => Perlin1D_asVector3(v, 0f, 1f);

        public Vector3 Perlin1D_asVector3(float v, float offset)
            => Perlin1D_asVector3(v, offset, 1f);

        public Vector3 Perlin1D_asVector3(float v, float offset, float power)
        {
            Vector3 result;
            result.x = ImproveResult(PerlinNoise(_perlinNoise_XX + v, _perlinNoise_XY + offset), power);
            result.y = ImproveResult(PerlinNoise(_perlinNoise_YX + v, _perlinNoise_YY + offset), power);
            result.z = ImproveResult(PerlinNoise(_perlinNoise_ZX + v, _perlinNoise_ZY + offset), power);
            return result;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Vector3 Perlin1D_asWideVector3(float v)
            => Perlin1D_asVector3(v) * 2f - Vector3.one;

        public Vector3 Perlin1D_asWideVector3(float v, float offset)
            => Perlin1D_asVector3(v, offset, 1f) * 2f - Vector3.one;

        public Vector3 Perlin1D_asWideVector3(float v, float offset, float power)
            => Perlin1D_asVector3(v, offset, power) * 2f - Vector3.one;

        //--------------------------------------------------------------------------------------------------------------
        // Perlin 2D noise
        // - input: Vector2
        // - return: float

        public float Perlin2D(Vector2 v)
            => Perlin2D(v.x, v.y, 0f, 1f);

        public float Perlin2D(Vector2 v, float offset, float power)
            => Perlin2D(v.x, v.y, offset, power);

        public float Perlin2D(float x, float y)
            => Perlin2D(x, y, 0f, 1f);

        public float Perlin2D(float x, float y, float offset)
            => Perlin2D(x, y, offset, 1f);

        public float Perlin2D(float x, float y, float offset, float power)
        {
            return ImproveResult(PerlinNoise(_perlinNoise_XX + x + offset, _perlinNoise_YY + y + offset), power);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Perlin 2D noise
        // - input: Vector2
        // - return: Vector2

        public Vector2 Perlin2D_asVector2(Vector2 v)
            => Perlin2D_asVector2(v.x, v.y, 0f, 1f);

        public Vector2 Perlin2D_asVector2(Vector2 v, float offset)
            => Perlin2D_asVector2(v.x, v.y, offset, 1f);

        public Vector2 Perlin2D_asVector2(Vector2 v, float offset, float power)
            => Perlin2D_asVector2(v.x, v.y, offset, power);

        public Vector2 Perlin2_asVector2D(float x, float y)
            => Perlin2D_asVector2(x, y, 0f, 1f);

        public Vector2 Perlin2_asVector2D(float x, float y, float offset)
            => Perlin2D_asVector2(x, y, offset, 1f);

        public Vector2 Perlin2D_asVector2(float x, float y, float offset, float power)
            => new Vector2(
                Perlin2D(x, y, +offset, power),
                Perlin2D(y, x, -offset, power)
            );

        //--------------------------------------------------------------------------------------------------------------
        // Perlin 3D noise
        // - input: Vector3
        // - return: float

        public float Perlin3D(Vector3 v)
            => Perlin3D(v.x, v.y, v.z, 0f, 1f);

        public float Perlin3D(Vector3 v, float offset)
            => Perlin3D(v.x, v.y, v.z, offset, 1f);

        public float Perlin3D(Vector3 v, float offset, float power)
            => Perlin3D(v.x, v.y, v.z, offset, power);

        public float Perlin3D(float x, float y, float z)
            => Perlin3D(x, y, z, 0f, 1f);

        public float Perlin3D(float x, float y, float z, float offset)
            => Perlin3D(x, y, z, offset, 1f);

        public float Perlin3D(float x, float y, float z, float offset, float power)
        {
            float x1 = PerlinNoise(_perlinNoise_XX + x + offset, _perlinNoise_XY + y + offset);
            float x2 = PerlinNoise(_perlinNoise_XX + x + offset, _perlinNoise_XY + z + offset);

            float y1 = PerlinNoise(_perlinNoise_YX + y + offset, _perlinNoise_YY + x + offset);
            float y2 = PerlinNoise(_perlinNoise_YX + y + offset, _perlinNoise_YY + z + offset);

            float z1 = PerlinNoise(_perlinNoise_ZX + z + offset, _perlinNoise_ZY + x + offset);
            float z2 = PerlinNoise(_perlinNoise_ZX + z + offset, _perlinNoise_ZY + y + offset);

            return ImproveResult((x1 + x2 + y1 + y2 + z1 + z2) / 6f, power);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Perlin 3D noise
        // - input: Vector3
        // - return: Vector3

        public Vector3 Perlin3D_asVector3(Vector3 v)
            => Perlin3D_asVector3(v.x, v.y, v.z, 0f, 1f);

        public Vector3 Perlin3D_asVector3(Vector3 v, float offset)
            => Perlin3D_asVector3(v.x, v.y, v.z, offset, 1f);

        public Vector3 Perlin3D_asVector3(Vector3 v, float offset, float power)
            => Perlin3D_asVector3(v.x, v.y, v.z, offset, power);

        public Vector3 Perlin3_asVector3D(float x, float y, float z)
            => Perlin3D_asVector3(x, y, z, 0f, 1f);

        public Vector3 Perlin3_asVector3D(float x, float y, float z, float offset)
            => Perlin3D_asVector3(x, y, z, offset, 1f);

        public Vector3 Perlin3D_asVector3(float x, float y, float z, float offset, float power)
            => new Vector3(
                Perlin3D(x, y, z, offset, power),
                Perlin3D(y, z, x, offset, power),
                Perlin3D(z, x, y, offset, power)
            );

        //--------------------------------------------------------------------------------------------------------------

        // Idea of this improvement is to make perlin noise more spread to 0f..1f, not only in the middle around 0.5f
        //
        // - input: value is 0..1 (but may be slightly lower/higger then limits)
        // - result: improved value
        //
        protected float ImproveResult(float result, float power)
        {
            if (Mathf.Approximately(power, 1f))
                return Mathf.Clamp01(result);

            return Mathf.PingPong((result - 0.5f) * power + 0.5f, 1f);
        }
    }
}
