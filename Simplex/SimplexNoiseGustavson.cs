using System;
using System.Numerics;

using static Simplex.NoiseHardwarePerlin;

namespace Simplex
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
            double t0 = 0, dot0 = 0, pow02 = 0, pow04 = 0, ddot0 = 0;

            g0 = grad3[gi0];
            t0 = 0.6 - x0*x0 - y0*y0 - z0*z0;
            if (t0 > 0) 
            {
                dot0 = g0.X * x0 + g0.Y * y0 + g0.Z * z0;
                pow02 = t0 * t0;
                pow04 = pow02 * pow02;
                val += 8 * dot0 * pow04;
                ddot0 = -8.0 * dot0 * pow02 * t0;
                
                //dx += ddot0 * x0 + pow04 * g0.X;
                //dy += ddot0 * y0 + pow04 * g0.Y;
                //dz += ddot0 * z0 + pow04 * g0.Z;

                dx += 8*(ddot0 * x0 + pow04 * g0.X);
                dy += 8*(ddot0 * y0 + pow04 * g0.Y);
                dz += 8*(ddot0 * z0 + pow04 * g0.Z);
            }
            
            g0 = grad3[gi1];
            t0 = 0.6 - x1*x1 - y1*y1 - z1*z1;
            if (t0 > 0)
            {
                dot0 = g0.X * x1 + g0.Y * y1 + g0.Z * z1;
                pow02 = t0 * t0;
                pow04 = pow02 * pow02;
                val += 8 * dot0 * pow04;
                ddot0 = -8.0 * dot0 * pow02 * t0;

                //dx += ddot0 * x1 + pow04 * g0.X;
                //dy += ddot0 * y1 + pow04 * g0.Y;
                //dz += ddot0 * z1 + pow04 * g0.Z;
                
                dx += 8*(ddot0 * x1 + pow04 * g0.X);
                dy += 8*(ddot0 * y1 + pow04 * g0.Y);
                dz += 8*(ddot0 * z1 + pow04 * g0.Z);
            }

            g0 = grad3[gi2];
            t0 = 0.6 - x2*x2 - y2*y2 - z2*z2;
            if (t0 > 0)
            {
                dot0 = g0.X * x2 + g0.Y * y2 + g0.Z * z2;
                pow02 = t0 * t0;
                pow04 = pow02 * pow02;
                val += 8 * dot0 * pow04;
                ddot0 = -8.0 * dot0 * pow02 * t0;
                
                //dx += ddot0 * x2 + pow04 * g0.X;
                //dy += ddot0 * y2 + pow04 * g0.Y;
                //dz += ddot0 * z2 + pow04 * g0.Z;

                dx += 8*(ddot0 * x2 + pow04 * g0.X);
                dy += 8*(ddot0 * y2 + pow04 * g0.Y);
                dz += 8*(ddot0 * z2 + pow04 * g0.Z);
            }

            g0 = grad3[gi3];
            t0 = 0.6 - x3*x3 - y3*y3 - z3*z3;
            if (t0 > 0)
            {
                dot0 = g0.X * x3 + g0.Y * y3 + g0.Z * z3;
                pow02 = t0 * t0;
                pow04 = pow02 * pow02;
                val += 8 * dot0 * pow04;
                ddot0 = -8.0 * dot0 * pow02 * t0;

                //dx += ddot0 * x3 + pow04 * g0.X;
                //dy += ddot0 * y3 + pow04 * g0.Y;
                //dz += ddot0 * z3 + pow04 * g0.Z;

                dx += 8*(ddot0 * x3 + pow04 * g0.X);
                dy += 8*(ddot0 * y3 + pow04 * g0.Y);
                dz += 8*(ddot0 * z3 + pow04 * g0.Z);
            }

            return new Vector4( (float)val, (float)dx, (float)dy, (float)dz );
        }

        public static Vector3[] grad3 = new Vector3[64]
        {
            new(-1,1,-1),
            new(-1,-1,0),
            new(0,-1,-1),
            new(-1,0,-1),
            new(-1,1,-1),
            new(-1,0,1),
            new(1,-1,0),
            new(0,1,-1),

            new(-1,-1,1),
            new(1,-1,0),
            new(0,1,-1),
            new(-1,0,1),
            new(-1,-1,1),
            new(1,0,-1),
            new(-1,1,0),
            new(0,-1,1),

            new(1,-1,-1),
            new(-1,1,0),
            new(0,-1,1),
            new(1,0,-1),
            new(1,-1,-1),
            new(-1,0,-1),
            new(-1,-1,0),
            new(0,-1,-1),

            new(1,1,1),
            new(1,1,0),
            new(0,1,1),
            new(1,0,1),
            new(1,1,1),
            new(1,0,1),
            new(1,1,0),
            new(0,1,1),

            new(1,-1,1),
            new(1,1,0),
            new(0,1,1),
            new(1,0,1),
            new(1,-1,1),
            new(1,0,-1),
            new(-1,1,0),
            new(0,-1,1),

            new(1,1,-1),
            new(-1,1,0),
            new(0,-1,1),
            new(1,0,-1),
            new(1,1,-1),
            new(-1,0,1),
            new(1,-1,0),
            new(0,1,-1),

            new(-1,1,1),
            new(1,-1,0),
            new(0,1,-1),
            new(-1,0,1),
            new(-1,1,1),
            new(1,0,1),
            new(1,1,0),
            new(0,1,1),

            new(-1,-1,-1),
            new(-1,-1,0),
            new(0,-1,-1),
            new(-1,0,-1),
            new(-1,-1,-1),
            new(-1,0,-1),
            new(-1,-1,0),
            new(0,-1,-1),
        };
    }
}
