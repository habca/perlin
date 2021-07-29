// http://paulbourke.net/miscellaneous/interpolation/

using System;
using System.Numerics;

using static ProceduralNoises.ImprovedNoisePerlin;
using static ProceduralNoises.ValueNoiseQuilez;

namespace ProceduralNoises
{
    public static class ValueNoiseBourke
    {
        static public Vector4 noise(double x, double y, double z) {
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

            double a = grad(p[AA  ], x  , y  , z   );
            double b = grad(p[BA  ], x-1, y  , z   );
            double c = grad(p[AB  ], x  , y-1, z   );
            double e = grad(p[AA+1], x  , y  , z-1 );
            double f = grad(p[BA+1], x-1, y  , z-1 );
            double g = grad(p[AB+1], x  , y-1, z-1 );
            double d = grad(p[BB  ], x-1, y-1, z   );
            double h = grad(p[BB+1], x-1, y-1, z-1 );

            double val = a * (1 - u) * (1 - v) * (1 - w) +
                         b * u * (1 - v) * (1 - w) +
                         c * (1 - u) * v * (1 - w) +
                         e * (1 - u) * (1 - v) * w +
                         f * u * (1 - v) * w +
                         g * (1 - u) * v * w +
                         d * u * v * (1 - w) +
                         h * u * v * w;

            double dx = a * (-du) * (1 - v) * (1 - w) +
                         b * du * (1 - v) * (1 - w) +
                         c * (-du) * v * (1 - w) +
                         e * (-du) * (1 - v) * w +
                         f * du * (1 - v) * w +
                         g * (-du) * v * w +
                         d * du * v * (1 - w) +
                         h * du * v * w;

            double dy = a * (1 - u) * (-dv) * (1 - w) +
                         b * u * (-dv) * (1 - w) +
                         c * (1 - u) * dv * (1 - w) +
                         e * (1 - u) * (-dv) * w +
                         f * u * (-dv) * w +
                         g * (1 - u) * dv * w +
                         d * u * dv * (1 - w) +
                         h * u * dv * w;

            double dz = a * (1 - u) * (1 - v) * (-dw) +
                         b * u * (1 - v) * (-dw) +
                         c * (1 - u) * v * (-dw) +
                         e * (1 - u) * (1 - v) * dw +
                         f * u * (1 - v) * dw +
                         g * (1 - u) * v * dw +
                         d * u * v * (-dw) +
                         h * u * v * dw;

            return new Vector4( (float)val, (float)dx, (float)dy, (float)dz );
        }

        public static Vector4 noise(Vector3 vec)
        {
            return noise(vec.X, vec.Y, vec.Z);
        }
    }
}