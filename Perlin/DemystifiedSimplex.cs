using System;
using System.Numerics;

namespace Perlin
{
    public class SimplexNoise
    {
        public static double skew(double x, double y, double z)
        {
            return (x + y + z) / 3.0;
        }

        public static double unskew(double si, double sj, double sk)
        {
            return (si + sj + sk) / 6.0;
        }

        public static Vector4 noised(double x, double y, double z)
        {

            // skew to simplex (i,j,k)
            const double F3 = 1.0 / 3.0;
            double s = (x + y + z) * F3;

            // cell origin as (i,j,k)
            int i = FastFloor(x + s);
            int j = FastFloor(y + s);
            int k = FastFloor(z + s);

            // unskew back to (x,y,z)
            const double G3 = 1.0 / 6.0;
            double t = (i + j + k) * G3;
            
            // first corner offset in (x,y,z)
            double x0 = x - (i - t);
            double y0 = y - (j - t);
            double z0 = z - (k - t);

            // second corner offsets in (i,j,k)
            int i1, j1, k1;
            
            // third corner offsets in (i,j,k)
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

            // second corner offset in (x,y,z)
            double x1 = x0 - i1 + G3;
            double y1 = y0 - j1 + G3;
            double z1 = z0 - k1 + G3;

            // third corner offset in (x,y,z)
            double x2 = x0 - i2 + F3;
            double y2 = y0 - j2 + F3;
            double z2 = z0 - k2 + F3;

            // fourth corner offset in (x,y,z)
            double x3 = x0 - 0.5;
            double y3 = y0 - 0.5;
            double z3 = z0 - 0.5;

            // gradient index
            int ii = i & 255;
            int jj = j & 255;
            int kk = k & 255;

            // four corners hashed gradient indices
            int gi0 = perm[ii + perm[jj + perm[kk]]] % 12;
            int gi1 = perm[ii + i1 + perm[jj + j1 + perm[kk + k1]]] % 12;
            int gi2 = perm[ii + i2 + perm[jj + j2 + perm[kk + k2]]] % 12;
            int gi3 = perm[ii + 1 + perm[jj + 1 + perm[kk + 1]]] % 12;

            Vector3 g0;
            double val = 0, dx = 0, dy = 0, dz = 0;
            double t0 = 0, dot0 = 0, pow02 = 0, pow04 = 0, ddot0 = 0;

            g0 = grad3[gi0];
            t0 = 0.5 - x0*x0 - y0*y0 - z0*z0;
            if (t0 > 0) 
            {
                dot0 = g0.X * x0 + g0.Y * y0 + g0.Z * z0;
                pow02 = t0 * t0;
                pow04 = pow02 * pow02;
                val += 32 * dot0 * pow04;
                ddot0 = -8.0 * dot0 * pow02 * t0;
                dx += ddot0 * x0 + pow04 * g0.X;
                dy += ddot0 * y0 + pow04 * g0.Y;
                dz += ddot0 * z0 + pow04 * g0.Z;
            }
            
            g0 = grad3[gi1];
            t0 = 0.5 - x1*x1 - y1*y1 - z1*z1;
            if (t0 > 0)
            {
                dot0 = g0.X * x1 + g0.Y * y1 + g0.Z * z1;
                pow02 = t0 * t0;
                pow04 = pow02 * pow02;
                val += 32 * dot0 * pow04;
                ddot0 = -8.0 * dot0 * pow02 * t0;
                dx += ddot0 * x1 + pow04 * g0.X;
                dy += ddot0 * y1 + pow04 * g0.Y;
                dz += ddot0 * z1 + pow04 * g0.Z;
            }

            g0 = grad3[gi2];
            t0 = 0.5 - x2*x2 - y2*y2 - z2*z2;
            if (t0 > 0)
            {
                dot0 = g0.X * x2 + g0.Y * y2 + g0.Z * z2;
                pow02 = t0 * t0;
                pow04 = pow02 * pow02;
                val += 32 * dot0 * pow04;
                ddot0 = -8.0 * dot0 * pow02 * t0;
                dx += ddot0 * x2 + pow04 * g0.X;
                dy += ddot0 * y2 + pow04 * g0.Y;
                dz += ddot0 * z2 + pow04 * g0.Z;
            }

            g0 = grad3[gi3];
            t0 = 0.5 - x3*x3 - y3*y3 - z3*z3;
            if (t0 > 0)
            {
                dot0 = g0.X * x3 + g0.Y * y3 + g0.Z * z3;
                pow02 = t0 * t0;
                pow04 = pow02 * pow02;
                val += 32 * dot0 * pow04;
                ddot0 = -8.0 * dot0 * pow02 * t0;
                dx += ddot0 * x3 + pow04 * g0.X;
                dy += ddot0 * y3 + pow04 * g0.Y;
                dz += ddot0 * z3 + pow04 * g0.Z;
            }

            return new Vector4(
                (float)val,
                (float)dx,
                (float)dy,
                (float)dz);
        }

        static int FastFloor(double x)
        {
            return x >= 0 ? (int)x : (int)x - 1;
        }

        private static Vector3[] grad3 = new Vector3[12]
        {
            new (1,1,0), new (-1,1,0), new (1,-1,0), new (-1,-1,0),
            new (1,0,1), new (-1,0,1), new (1,0,-1), new (-1,0,-1),
            new (0,1,1), new (0,-1,1), new (0,1,-1), new (0,-1,-1),
        };

        private static int[] p = 
        {
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };

        private static int[] perm = new int[512];
        static SimplexNoise() 
        { 
            for (int i=0; i < 256; i++)
            {
                perm[256+i] = perm[i] = p[i];
            }
        }


    }
}