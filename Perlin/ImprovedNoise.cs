using System;
using System.Numerics;

using static Perlin.ImprovedNoisePerlin;

namespace Perlin
{
    public static class ImprovedNoise
    {
        public static double noise(double x, double y, double z) {
            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;
            int Z = (int)Math.Floor(z) & 255;

            x -= Math.Floor(x);
            y -= Math.Floor(y);
            z -= Math.Floor(z);

            double u = fade(x);
            double v = fade(y);
            double w = fade(z);

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

            float a = Vector3.Dot(c000, p000);
            float b = Vector3.Dot(c100, p100);
            float c = Vector3.Dot(c010, p010);
            float e = Vector3.Dot(c001, p001);
            float f = Vector3.Dot(c101, p101);
            float g = Vector3.Dot(c011, p011);
            float d = Vector3.Dot(c110, p110);
            float h = Vector3.Dot(c111, p111);

            return lerp(w,
                        lerp(v,
                            lerp(u, a, b),
                            lerp(u, c, d)),
                        lerp(v,
                            lerp(u, e, f),
                            lerp(u, g, h)));
        }

        public static int hash(int x, int y, int z)
        {
            return p[ p[ p[x] + y] + z] & 15;
        }

        public static Vector3[] grad3 =
        {
            new(1,1,0), new(-1,1,0), new(1,-1,0), new(-1,-1,0),
            new(1,0,1), new(-1,0,1), new(1,0,-1), new(-1,0,-1),
            new(0,1,1), new(0,-1,1), new(0,1,-1), new(0,-1,-1),
            new(1,1,0), new(0,-1,1), new(-1,1,0), new(0,-1,-1),
        };

        public static Vector3 float3(double x, double y, double z)
        {
            return new Vector3((float)x, (float)y, (float)z);
        }
    }
}