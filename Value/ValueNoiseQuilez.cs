// https://www.iquilezles.org/www/articles/morenoise/morenoise.htm

using System;
using System.Numerics;

using static Perlin.ImprovedNoisePerlin;

namespace Value
{
    public static class ValueNoiseQuilez
    {
        public static Vector4 noise(double x, double y, double z)
        {
            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;
            int Z = (int)Math.Floor(z) & 255;

            x -= Math.Floor(x);
            y -= Math.Floor(y);
            z -= Math.Floor(z);

            double u = fade(x);
            double v = fade(y);
            double w = fade(z);

            double du = derva(x);
            double dv = derva(y);
            double dw = derva(z);

            int A = p[X  ]+Y, AA = p[A]+Z, AB = p[A+1]+Z;
            int B = p[X+1]+Y, BA = p[B]+Z, BB = p[B+1]+Z;

            double a = grad(p[AA  ], x,   y,   z );
            double b = grad(p[BA  ], x-1, y,   z );
            double c = grad(p[AB  ], x,   y-1, z );
            double e = grad(p[AA+1], x,   y,   z-1);
            double f = grad(p[BA+1], x-1, y,   z-1);
            double g = grad(p[AB+1], x,   y-1, z-1);
            double d = grad(p[BB  ], x-1, y-1, z );
            double h = grad(p[BB+1], x-1, y-1, z-1);

            double k0 = a;
            double k1 = b - a;
            double k2 = c - a;
            double k3 = e - a;
            double k4 = a - b - c + d;
            double k5 = a - c - e + g;
            double k6 = a - b - e + f;
            double k7 = -a + b + c - d + e - f - g + h;

            double val = k0 + k1 * u + k2 * v + k3 * w + k4 * u * v + k5 * v * w + k6 * w * u + k7 * u * v * w;

            double dx = du * (k1 + k4 * v + k6 * w + k7 * v * w);
            double dy = dv * (k2 + k4 * u + k5 * w + k7 * u * w);
            double dz = dw * (k3 + k5 * v + k6 * u + k7 * u * v);

            return new Vector4( (float)val, (float)dx, (float)dy, (float)dz );
        }

        public static double derva(double t)
        {
            return 30 * t * t * (t * (t - 2) + 1);
        }

        public static Vector4 noise(Vector3 vec)
        {
            return noise(vec.X, vec.Y, vec.Z);
        }
    }
}
