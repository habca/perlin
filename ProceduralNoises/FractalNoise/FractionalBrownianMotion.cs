using System;
using System.Numerics;

namespace ProceduralNoises
{
    public static class FractionalBrownianMotion
    {
        public static Vector4 fbm(
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
                double xx = x * freq;
                double yy = y * freq;
                double zz = z * freq;

                Vector4 n = f(xx, yy, zz);
                dsum.X += n.Y;
                dsum.Y += n.Z;
                dsum.Z += n.W;

                float fval = (float) n.X;
                fval = (1f + fval) * 0.5f;
                sum += amp * fval;
                freq *= 2f;
                amp *= 0.5f;
            }

            return new Vector4(
                (float)sum,
                (float)dsum.X,
                (float)dsum.Y,
                (float)dsum.Z);
        }

        /*
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
        */

        public delegate double Warp(Fract h, Sumf g, Noise f, double x, double y, double z, out Vector3 derva);
        public delegate double Fract(Sumf g, Noise f, double x, double y, double z, out Vector3 derva);
        public delegate double Noise(double x, double y, double z, out Vector3 derva);
        public delegate double Sumf(double x);

        public static double Dnoise(Sumf g, Noise f, double x, double y, double z, out Vector3 derva)
        {
            double sum = 0;
            double freq = 1;
            double m = 0.1;
            derva = Vector3.Zero;

            int octaves = 0;
            while (freq < 256)
            {
                double n = f(freq * x, freq * y, freq * z, out Vector3 d) / freq;
                derva += d;
                // TODO: korvaa skaalaus Sumf kutsulla
                double tmp = (n * Math.PI * 0.5 + 0.5) / (1f + m * Vector3.Dot(derva, derva));

                // clamp values between [0, 1]
                if (tmp < 0) tmp = 0;
                if (tmp > 1) tmp = 1;

                sum += tmp;
                freq *= 2;
                octaves += 1;
            }
            if (octaves < 1) return 0;
            return sum / octaves;
        }

        public static double RWarp(Fract h, Sumf g, Noise f, double x, double y, double z, out Vector3 d)
        {
            int depth = offset.Length / 3; // divisible by three
            return RWarp(h, g, f, x, y, z, out d, depth);
        }

        private static double RWarp(Fract h, Sumf g, Noise f, double x, double y, double z, out Vector3 d, int n)
        {
            if (n <= 0) return h(g, f, x, y, z, out d);
            double m = 0.1;
            int i = n * 3 - 1, j = i - 1, k = j - 1;
            int depth = n - 1;
            double xx = RWarp(h, g, f, x + offset[i].X, y + offset[i].Y, z + offset[i].Z, out d, depth);
            double yy = RWarp(h, g, f, x + offset[j].X, y + offset[j].Y, z + offset[j].Z, out d, depth);
            double zz = RWarp(h, g, f, x + offset[k].X, y + offset[k].Y, z + offset[k].Z, out d, depth);
            return h(g, f, x + m * xx, y + m * yy, z + m * zz, out d);
        }

        private static Vector3[] offset =
        {
            new Vector3(5.2f, 1.3f, 3.1f),
            new Vector3(1.7f, 9.2f, 8.3f),
            new Vector3(8.3f, 2.8f, 5.1f),
        };

        public static double Billow(double n)
        {
            return Math.Abs(n);
        }

        public static double Normalize(double n)
        {
            // TODO: kohinan skaalaus välille [0, 1]
            throw new NotImplementedException();
        }
    }
}
