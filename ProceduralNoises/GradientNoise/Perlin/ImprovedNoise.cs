using System;
using System.Numerics;

using static ProceduralNoises.ImprovedNoisePerlin;

namespace ProceduralNoises
{
    public static class ImprovedNoise
    {
        public static Vector4 noise(double x, double y, double z) {
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

            Vector3 a = grad3[hash(X,   Y,   Z  )];
            Vector3 b = grad3[hash(X+1, Y,   Z  )];
            Vector3 c = grad3[hash(X,   Y+1, Z  )];
            Vector3 e = grad3[hash(X,   Y,   Z+1)];
            Vector3 f = grad3[hash(X+1, Y,   Z+1)];
            Vector3 g = grad3[hash(X,   Y+1, Z+1)];
            Vector3 d = grad3[hash(X+1, Y+1, Z  )];
            Vector3 h = grad3[hash(X+1, Y+1, Z+1)];

            Vector3 p000 = float3(x  , y  , z  );
            Vector3 p100 = float3(x-1, y  , z  );
            Vector3 p010 = float3(x  , y-1, z  );
            Vector3 p001 = float3(x  , y  , z-1);
            Vector3 p101 = float3(x-1, y  , z-1);
            Vector3 p011 = float3(x  , y-1, z-1);
            Vector3 p110 = float3(x-1, y-1, z  );
            Vector3 p111 = float3(x-1, y-1, z-1);

            float va = Vector3.Dot(a, p000);
            float vb = Vector3.Dot(b, p100);
            float vc = Vector3.Dot(c, p010);
            float ve = Vector3.Dot(e, p001);
            float vf = Vector3.Dot(f, p101);
            float vg = Vector3.Dot(g, p011);
            float vd = Vector3.Dot(d, p110);
            float vh = Vector3.Dot(h, p111);

            double xyz = lerp(w,
                            lerp(v,
                                lerp(u, va, vb),
                                lerp(u, vc, vd)),
                            lerp(v,
                                lerp(u, ve, vf),
                                lerp(u, vg, vh)));

            Vector3 drva = lerp3(w,
                            lerp3(v,
                                lerp3(u, a, b),
                                lerp3(u, c, d)),
                            lerp3(v,
                                lerp3(u, e, f),
                                lerp3(u, g, h)));

            //      g       h
            //  c   +----d--+
            //  +--------+  |
            //  |   |    |  |
            //  |   |    |  |
            //  | e +----|--+ f
            //  +--------+
            //  a        b
            
            drva.X += (float)du * lerp(w,
                                lerp(v, (vb - va), (vd - vc)),
                                lerp(v, (vf - ve), (vh - vg)));

            drva.Y += (float)dv * lerp(w,
                                lerp(u, (vc - va), (vd - vb)),
                                lerp(u, (vg - ve), (vh - vf)));

            drva.Z += (float)dw * lerp(v,
                                lerp(u, (ve - va), (vf - vb)),
                                lerp(u, (vg - vc), (vh - vd)));

            return new Vector4((float)xyz, drva.X, drva.Y, drva.Z);
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

        public static double derva(double t)
        {
            return 30 * t * t * (t - 1) * (t - 1);
        }

        public static Vector3 float3(double x, double y, double z)
        {
            return new Vector3((float)x, (float)y, (float)z);
        }

        public static Vector3 lerp3(double t, Vector3 a, Vector3 b)
        {
            return a + (float)t * (b - a);
        }

        public static Vector4 noise(Vector3 vec)
        {
            return noise(vec.X, vec.Y, vec.Z);
        }
    }
}