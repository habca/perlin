using System;
using System.Numerics;

using static Perlin.ImprovedNoisePerlin;
using static Perlin.ImprovedNoise;

namespace Perlin
{
    public static class ImprovedNoiseGustavson
    {
        public static Vector4 noise(double x, double y, double z) {
            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;
            int Z = (int)Math.Floor(z) & 255;

            double x1 = (x -= Math.Floor(x)) - 1;
            double y1 = (y -= Math.Floor(y)) - 1;
            double z1 = (z -= Math.Floor(z)) - 1;

            double u = fade(x);
            double v = fade(y);
            double w = fade(z);

            double du = derva(x);
            double dv = derva(y);
            double dw = derva(z);

            Vector3 c000 = grad3[hash(X,   Y,   Z  )];
            Vector3 c100 = grad3[hash(X+1, Y,   Z  )];
            Vector3 c010 = grad3[hash(X,   Y+1, Z  )];
            Vector3 c001 = grad3[hash(X,   Y,   Z+1)];
            Vector3 c101 = grad3[hash(X+1, Y,   Z+1)];
            Vector3 c011 = grad3[hash(X,   Y+1, Z+1)];
            Vector3 c110 = grad3[hash(X+1, Y+1, Z  )];
            Vector3 c111 = grad3[hash(X+1, Y+1, Z+1)];

            Vector3 p000 = float3(x  , y  , z  );
            Vector3 p100 = float3(x-1, y  , z  );
            Vector3 p010 = float3(x  , y-1, z  );
            Vector3 p001 = float3(x  , y  , z-1);
            Vector3 p101 = float3(x-1, y  , z-1);
            Vector3 p011 = float3(x  , y-1, z-1);
            Vector3 p110 = float3(x-1, y-1, z  );
            Vector3 p111 = float3(x-1, y-1, z-1);

            float v000 = Vector3.Dot(c000, p000);
            float v100 = Vector3.Dot(c100, p100);
            float v010 = Vector3.Dot(c010, p010);
            float v001 = Vector3.Dot(c001, p001);
            float v101 = Vector3.Dot(c101, p101);
            float v011 = Vector3.Dot(c011, p011);
            float v110 = Vector3.Dot(c110, p110);
            float v111 = Vector3.Dot(c111, p111);

            double k0 = v000;
            double k1 = v100 - v000;
            double k2 = v010 - v000;
            double k3 = v001 - v000;
            double k4 = v000 - v100 - v010 + v110;
            double k5 = v000 - v010 - v001 + v011;
            double k6 = v000 - v100 - v001 + v101;
            double k7 = -v000 + v100 + v010 - v110 + v001 - v101 - v011 + v111;

            Vector3 d0 = c000;
            Vector3 d1 = c100 - c000;
            Vector3 d2 = c010 - c000;
            Vector3 d3 = c001 - c000;
            Vector3 d4 = c000 - c100 - c010 + c110;
            Vector3 d5 = c000 - c010 - c001 + c011;
            Vector3 d6 = c000 - c100 - c001 + c101;
            Vector3 d7 = -c000 + c100 + c010 - c110 + c001 - c101 - c011 + c111;

            double val = k0 + k1 * u + k2 * v + k3 * w + k4 * u * v + k5 * v * w + k6 * w * u + k7 * u * v * w;

            double dx = du * (k1 + k4 * v + k6 * w + k7 * v * w) + d0.X + u * d1.X + v * d2.X + w * d3.X + u * v * d4.X + v * w * d5.X + w * u * d6.X + u * v * w * d7.X;
            
            double dy = dv * (k2 + k4 * u + k5 * w + k7 * u * w) + d0.Y + u * d1.Y + v * d2.Y + w * d3.Y + u * v * d4.Y + v * w * d5.Y + w * u * d6.Y + u * v * w * d7.Y;
            
            double dz = dw * (k3 + k5 * v + k6 * u + k7 * u * v) + d0.Z + u * d1.Z + v * d2.Z + w * d3.Z + u * v * d4.Z + v * w * d5.Z + w * u * d6.Z + u * v * w * d7.Z;

            return new Vector4((float)val, (float)dx, (float)dy, (float)dz);
        }

        public static double derva(double t)
        {
            return 30 * t * t * (t - 1) * (t - 1);
        }

        public static Vector4 noise(Vector3 vec)
        {
            return noise(vec.X, vec.Y, vec.Z);
        }
    }
}