using System;
using System.Numerics;

using static ProceduralNoises.NoiseHardwarePerlin;

namespace ProceduralNoises
{
    public static class SimplexNoiseGustavson
    {
        public static Vector4 noise(double x, double y, double z)
        {
            const double F3 = 1.0 / 3.0;
            double s = (x + y + z) * F3;

            int i = (int)Math.Floor(x + s);
            int j = (int)Math.Floor(y + s);
            int k = (int)Math.Floor(z + s);

            const double G3 = 1.0 / 6.0;
            double t = (i + j + k) * G3;
            
            double x0 = x - (i - t);
            double y0 = y - (j - t);
            double z0 = z - (k - t);

            int i1, j1, k1;
            int i2, j2, k2;

            if (x0 >= y0)
            {
                if (y0 >= z0)
                {
                    // X Y Z order
                    i1 = 1;
                    j1 = 0;
                    k1 = 0;
                    i2 = 1;
                    j2 = 1;
                    k2 = 0;
                }
                else if (x0 >= z0)
                {
                    // X Z Y order
                    i1 = 1;
                    j1 = 0;
                    k1 = 0;
                    i2 = 1;
                    j2 = 0;
                    k2 = 1;
                }
                else
                {
                    // Z X Y order
                    i1 = 0;
                    j1 = 0;
                    k1 = 1;
                    i2 = 1;
                    j2 = 0;
                    k2 = 1;
                }
            }
            else
            {
                // x0 < y0
                if (y0 < z0)
                {
                    // Z Y X order
                    i1 = 0;
                    j1 = 0;
                    k1 = 1;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                }
                else if (x0 < z0)
                {
                    // Y Z X order
                    i1 = 0;
                    j1 = 1;
                    k1 = 0;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                }
                else
                {
                    // Y X Z order
                    i1 = 0;
                    j1 = 1;
                    k1 = 0;
                    i2 = 1;
                    j2 = 1;
                    k2 = 0;
                }
            }

            double x1 = x0 - i1 + G3;
            double y1 = y0 - j1 + G3;
            double z1 = z0 - k1 + G3;

            double x2 = x0 - i2 + F3;
            double y2 = y0 - j2 + F3;
            double z2 = z0 - k2 + F3;

            double x3 = x0 - 0.5;
            double y3 = y0 - 0.5;
            double z3 = z0 - 0.5;

            int gi0 = shuffle(i,   j,   k)    & 63;
            int gi1 = shuffle(i+i1,j+j1,k+k1) & 63;
            int gi2 = shuffle(i+i2,j+j2,k+k2) & 63;
            int gi3 = shuffle(i+1, j+1, k+1)  & 63;

            Vector3 g0;
            double val = 0, dx = 0, dy = 0, dz = 0;
            double t0, dot0, pow02, pow04;
            Vector4 v1 = Vector4.Zero, v2 = Vector4.Zero, v3 = Vector4.Zero, v4 = Vector4.Zero;

            g0 = grad3[gi0];
            t0 = 0.6 - x0*x0 - y0*y0 - z0*z0;
            if (t0 > 0)
            {
                dot0 = g0.X * x0 + g0.Y * y0 + g0.Z * z0;
                pow02 = t0 * t0;
                pow04 = pow02 * pow02;

                val = 8 * dot0 * pow04;
                dx = -64 * pow02 * t0 * x0 * dot0 + 8 * pow04 * g0.X;
                dy = -64 * pow02 * t0 * y0 * dot0 + 8 * pow04 * g0.Y;
                dz = -64 * pow02 * t0 * z0 * dot0 + 8 * pow04 * g0.Z;
                v1 = new Vector4((float)val, (float)dx, (float)dy, (float)dz);
            }
            
            
            g0 = grad3[gi1];
            t0 = 0.6 - x1*x1 - y1*y1 - z1*z1;
            if (t0 > 0)
            {
                dot0 = g0.X * x1 + g0.Y * y1 + g0.Z * z1;
                pow02 = t0 * t0;
                pow04 = pow02 * pow02;
                val = 8 * dot0 * pow04;
                dx = -64 * pow02 * t0 * x1 * dot0 + 8 * pow04 * g0.X;
                dy = -64 * pow02 * t0 * y1 * dot0 + 8 * pow04 * g0.Y;
                dz = -64 * pow02 * t0 * z1 * dot0 + 8 * pow04 * g0.Z;
                v2 = new Vector4((float)val, (float)dx, (float)dy, (float)dz);
            }

            g0 = grad3[gi2];
            t0 = 0.6 - x2*x2 - y2*y2 - z2*z2;
            if (t0 > 0)
            {
                dot0 = g0.X * x2 + g0.Y * y2 + g0.Z * z2;
                pow02 = t0 * t0;
                pow04 = pow02 * pow02;
                val = 8 * dot0 * pow04;
                dx = -64 * pow02 * t0 * x2 * dot0 + 8 * pow04 * g0.X;
                dy = -64 * pow02 * t0 * y2 * dot0 + 8 * pow04 * g0.Y;
                dz = -64 * pow02 * t0 * z2 * dot0 + 8 * pow04 * g0.Z;
                v3 = new Vector4((float)val, (float)dx, (float)dy, (float)dz);
            }

            g0 = grad3[gi3];
            t0 = 0.6 - x3*x3 - y3*y3 - z3*z3;
            if (t0 > 0)
            {
                dot0 = g0.X * x3 + g0.Y * y3 + g0.Z * z3;
                pow02 = t0 * t0;
                pow04 = pow02 * pow02;
                val = 8 * dot0 * pow04;
                dx = -64 * pow02 * t0 * x3 * dot0 + 8 * pow04 * g0.X;
                dy = -64 * pow02 * t0 * y3 * dot0 + 8 * pow04 * g0.Y;
                dz = -64 * pow02 * t0 * z3 * dot0 + 8 * pow04 * g0.Z;
                v4 = new Vector4((float)val, (float)dx, (float)dy, (float)dz);
            }

            return v1 + v2 + v3 + v4;
            //return new Vector4( (float)val, (float)dx, (float)dy, (float)dz );
        }

        public static Vector3[] grad3 =
        {
            new(-1,1,-1),  // 00
            new(-1,-1,0),  // 01
            new(0,-1,-1),  // 02
            new(-1,0,-1),  // 03
            new(-1,1,-1),  // 04
            new(-1,0,1),   // 05
            new(1,-1,0),   // 06
            new(0,1,-1),   // 07
                           
            new(-1,-1,1),  // 08
            new(1,-1,0),   // 09
            new(0,1,-1),   // 10
            new(-1,0,1),   // 11
            new(-1,-1,1),  // 12
            new(1,0,-1),   // 13
            new(-1,1,0),   // 14
            new(0,-1,1),   // 15
                           
            new(1,-1,-1),  // 16
            new(-1,1,0),   // 17
            new(0,-1,1),   // 18
            new(1,0,-1),   // 19
            new(1,-1,-1),  // 20
            new(-1,0,-1),  // 21
            new(-1,-1,0),  // 22
            new(0,-1,-1),  // 23
                           
            new(1,1,1),    // 24
            new(1,1,0),    // 25
            new(0,1,1),    // 26
            new(1,0,1),    // 27
            new(1,1,1),    // 28
            new(1,0,1),    // 29
            new(1,1,0),    // 30
            new(0,1,1),    // 31
                           
            new(1,-1,1),   // 32
            new(1,1,0),    // 33
            new(0,1,1),    // 34
            new(1,0,1),    // 35
            new(1,-1,1),   // 36
            new(1,0,-1),   // 37
            new(-1,1,0),   // 38
            new(0,-1,1),   // 39
                           
            new(1,1,-1),   // 40
            new(-1,1,0),   // 41
            new(0,-1,1),   // 42
            new(1,0,-1),   // 43
            new(1,1,-1),   // 44
            new(-1,0,1),   // 45
            new(1,-1,0),   // 46
            new(0,1,-1),   // 47

            new(-1,1,1),   // 48
            new(1,-1,0),   // 49
            new(0,1,-1),   // 50
            new(-1,0,1),   // 51
            new(-1,1,1),   // 52
            new(1,0,1),    // 53
            new(1,1,0),    // 54
            new(0,1,1),    // 55

            new(-1,-1,-1), // 56
            new(-1,-1,0),  // 57
            new(0,-1,-1),  // 58
            new(-1,0,-1),  // 59
            new(-1,-1,-1), // 60
            new(-1,0,-1),  // 61
            new(-1,-1,0),  // 62
            new(0,-1,-1),  // 63
        };

        /*
        private static Vector3[] Grad3()
        {
            Vector3[] grads = new Vector3[64];
            for (int h = 0; h < grads.Length; h++)
            {
                grads[h].X = grad(h, 1, 0, 0);
                grads[h].Y = grad(h, 0, 1, 0);
                grads[h].Z = grad(h, 0, 0, 1);
            }
            return grads;

            static float grad(int h, float x, float y, float z)
            {
                int b5 = h>>5 & 1, b4 = h>>4 & 1, b3 = h>>3 & 1, b2= h>>2 & 1, b = h & 3;
                float p = b==1?x:b==2?y:z, q = b==1?y:b==2?z:x, r = b==1?z:b==2?x:y;
                p = (b5==b3 ? -p : p); q = (b5==b4 ? -q : q); r = (b5!=(b4^b3) ? -r : r);
                return p + (b==0 ? q+r : b2==0 ? q : r);
            }
        }
        */
    }
}
