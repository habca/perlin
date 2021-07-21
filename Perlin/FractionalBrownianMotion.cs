using System;
using Vector3 = Perlin.ImprovedNoise.Vector3;
using Vector4 = Perlin.ImprovedNoise.Vector4;
using System.Numerics;

namespace Perlin
{
    static class FBM
    {
        public static Vector4 fbm(
            Func<double, double, double, Vector4> f,
            double x,
            double y,
            double z,
            int it
        )
        {
            float sum = 0f;
            float freq = 1f;
            float amplitude = 1f;

            Vector3 dsum = new Vector3(0f, 0f, 0f);
            for (int i = 0; i < it; i++)
            {
                double xx = x * freq;
                double yy = y * freq;
                double zz = z * freq;

                Vector4 n = f(xx, yy, zz);
                dsum.x += n.z;
                dsum.y += n.w;
                dsum.z += n.w;

                float fval = (float) n.x;
                fval = (1f + fval) * 0.5f;
                sum += amplitude * fval;
                freq *= 2f;
                amplitude *= 0.5f;
            }

            return new Vector4(sum, dsum.x, dsum.y, dsum.z);
        }

        public static Vector4 hurst(
            Func<double, double, double, Vector4> f,
            double x,
            double y,
            double z,
            int it
        )
        {
            float sum = 0f;
            float freq = 0.01f;
            float amplitude = 0.48f;
            float lacunarity = 2.0f;

            float H = 1f;
            float gain = (float) Math.Pow(lacunarity, -H);

            Vector3 dsum = new Vector3(0f, 0f, 0f);
            for (int i = 0; i < it; i++)
            {
                double xx = x * freq;
                double yy = y * freq;
                double zz = z * freq;

                Vector4 n = f(xx, yy, zz);
                dsum.x += n.z;
                dsum.y += n.w;
                dsum.z += n.w;

                double value = n.x * amplitude / (1f + ImprovedNoise.dot(dsum, dsum));
                sum += (float)value;
                freq *= lacunarity;
                amplitude *= gain;

            }
            return new Vector4(sum, dsum.x, dsum.y, dsum.z);
        }

        public static Vector4 billow(
            Func<double, double, double, Vector4> f,
            double x,
            double y,
            double z,
            int it
        )
        {
            float sum = 0f;
            float freq = 0.01f;
            float amplitude = 0.48f;

            Vector3 dsum = new Vector3(0f, 0f, 0f);
            for (int i = 0; i < it; i++)
            {
                double xx = x * freq;
                double yy = y * freq;
                double zz = z * freq;

                Vector4 n = f(xx, yy, zz);
                dsum.x += n.z;
                dsum.y += n.w;
                dsum.z += n.w;

                float fval = (float) n.x;
                sum += amplitude * Math.Abs(fval);
                freq *= 2f;
                amplitude *= 0.5f;
            }

            return new Vector4(sum, dsum.x, dsum.y, dsum.z);
        }

        public static Vector4 turbulence(
            Func<double, double, double, Vector4> f,
            double x,
            double y,
            double z,
            int it
        )
        {
            float sum = 0f;
            float freq = 0.01f;
            float amplitude = 0.48f;

            Vector3 dsum = new Vector3(0f, 0f, 0f);
            for (int i = 0; i < it; i++)
            {
                double xx = x * freq;
                double yy = y * freq;
                double zz = z * freq;

                Vector4 n = f(xx, yy, zz);
                dsum.x += n.z;
                dsum.y += n.w;
                dsum.z += n.w;

                float fval = (float) n.x;
                sum += Math.Abs(amplitude * fval);
                freq *= 2f;
                amplitude *= 0.5f;
            }

            return new Vector4(sum, dsum.x, dsum.y, dsum.z);
        }


        public static Vector4 ridge(
            Func<double, double, double, Vector4> f,
            double x,
            double y,
            double z,
            int it
        )
        {
            float sum = 0f;
            float freq = 0.01f;
            float amplitude = 1.001f;

            Vector3 dsum = new Vector3(0f, 0f, 0f);
            for (int i = 0; i < it; i++)
            {
                double xx = x * freq;
                double yy = y * freq;
                double zz = z * freq;

                Vector4 n = f(xx, yy, zz);
                dsum.x += n.z;
                dsum.y += n.w;
                dsum.z += n.w;

                float fval = (float) n.x;
                fval = 1f - Math.Abs(fval);
                sum += amplitude * fval * fval;
                freq *= 2f;
                amplitude *= 0.5f;
            }

            return new Vector4(sum, dsum.x, dsum.y, dsum.z);
        }

        public static Vector4 nms(
            Func<double, double, double, Vector4> f,
            double x,
            double y,
            double z,
            int it
        )
        {
            double sum = 0f;
            double freq = 0.005f;
            double amp = 1f;

            Vector3 dsum = new Vector3(0f, 0f, 0f);
            for (int i = 0; i < it; i++)
            {
                Vector4 n = f(freq*x, freq*y, freq*z);
                dsum.x += n.y;
                dsum.y += n.z;
                dsum.z = 0;

                sum += amp * n.x / (1f + ImprovedNoise.dot(dsum, dsum));
                amp *= 0.5;
                freq *= 2.0;

                // cos(36.8) = 0.8. sin(36.8) = 0.6
                var tmpx = x * 1 + y * 0;
                var tmpy = x * 0 + y * -1;

                x = tmpx;
                y = tmpy;
            }
            return new Vector4(sum, dsum.x, dsum.y, dsum.z);
        }

            public static Vector4 warp(
            Func<double, double, double, Vector4> f,
            double x,
            double y,
            double z,
            int it
        )
        {
            float k = 48f;
            
            float n1v = 0f, n1dx = 0f, n1dy = 0f;
            float n2v = 0f, n2dx = 0f, n2dy = 0f;

            for (int i = 0; i < it; i++)
            {
                Vector4 n1 = nms( f, x + n1v + n1dx, y + n2v + n1dy, z, 8 );
                Vector4 n2 = nms( f, x + n1v + n2dx, y + n2v + n2dy, z, 8 );

                n1dx = (float)n1.y;
                n1dy = (float)n1.z;

                n2dx = (float)n2.y;
                n2dy = (float)n2.z;

                n1v = k * (float)n1.x;
                n2v = k * (float)n2.x;
            }
            return nms( f, x + n1v, y + n2v, z, it );
        }
    }
}