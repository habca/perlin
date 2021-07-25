using System;
using System.Numerics;

using static Simplex.NoiseHardwarePerlin;

namespace Simplex
{
    public static class NoiseHardware
    {        
        static int[] sum(int[] a, int[] b)
        {
            return new int[]{a[0]+b[0],a[1]+b[1],a[2]+b[2]};
        }
        public static (double,double,double) skew(double x, double y, double z)
        {
            double s = (x+y+z) / 3.0;
            return (x+s, y+s, z+s);
        }
        public static (double,double,double) unskew(double si, double sj, double sk)
        {
            // simplex noise demystified
            // (0,0,0) sama kuin x0,y0,z0
            // (1,0,0) = (i1,j1,k1) --> 1/6 = G3
            // (1,1,0) = (i2,j2,k2) --> 2/6 = 2.0*G3
            // (1,1,1) = (1.0,1.0,1.0) --> 3/6 = 3.0*G3
            double ss = (si+sj+sk) / 6.0;
            return (si-ss, sj-ss, sk-ss);
        }
        public static double noise(double x, double y, double z) {

            (double sx, double sy, double sz) = skew  (  x,   y,   z);
            
            int si = (int)Math.Floor(sx),
                sj = (int)Math.Floor(sy),
                sk = (int)Math.Floor(sz);

            (double  i, double  j, double  k) = unskew(si, sj, sk);
            (double  u, double  v, double  w) = (x-i, y-j, z-k);

            // simplex vertice traversal order
            int[,][] simplex = new int[6,4][]
            {
                { new[]{0,0,0}, new[]{1,0,0}, new[]{0,1,0}, new[]{0,0,1} }, // X Y Z
                { new[]{0,0,0}, new[]{1,0,0}, new[]{0,0,1}, new[]{0,1,0} }, // X Z Y
                { new[]{0,0,0}, new[]{0,0,1}, new[]{1,0,0}, new[]{0,1,0} }, // Z X Y
                { new[]{0,0,0}, new[]{0,0,1}, new[]{0,1,0}, new[]{1,0,0} }, // Z Y X
                { new[]{0,0,0}, new[]{0,1,0}, new[]{0,0,1}, new[]{1,0,0} }, // Y Z X
                { new[]{0,0,0}, new[]{0,1,0}, new[]{1,0,0}, new[]{0,0,1} }, // Y X Z
            };

            // find the simplex since there are 6 in 3D
            int index = u >= v && v >= w ? 0 : // X Y Z
                        u >= v && u >= w ? 1 : // X Z Y
                        u >= v           ? 2 : // Z X Y
                        u <  v && v <  w ? 3 : // Z Y X
                        u <  v && u <  w ? 4 : // Y Z X
                        u <  v           ? 5 : // Y X Z
                                          -1 ; // error

            int[] Start = simplex[index,0],
                      A = simplex[index,1],
                      B = simplex[index,2],
                      C = simplex[index,3];

            return K(si,sj,sk, u,v,w, Start) +
                   K(si,sj,sk, u,v,w, sum(Start,A)) +
                   K(si,sj,sk, u,v,w, sum(sum(Start,A),B)) +
                   K(si,sj,sk, u,v,w, sum(sum(sum(Start,A),B),C));
        }
        static double K(
            int   si, int   sj, int   sk,
            double x, double y, double z,
            int[] A
        )
        {
            (double i, double j, double k) = unskew(A[0], A[1], A[2]);
            (double u, double v, double w) = (x-i, y-j, z-k);

            double t = 0.6 - u*u - v*v - w*w;
            if (t < 0) return 0;

            // hash pseudorandom 6-bit gradient index 
            int h = shuffle(si + A[0], sj + A[1], sk + A[2]);
            return 8f * t*t*t*t * gradient(h, u, v, w);
        }

        static double gradient(int h, double x, double y, double z)
        {
            // 6-bit gradient index
            int b5 = b(h,5);
            int b4 = b(h,4);
            int b3 = b(h,3);

            int b2 = b(h,2);
            int b1 = b(h,1);
            int b0 = b(h,0);

            double[,] magnitude = new double[8,3]
            {
                {z,x,y},
                {x,y,0},
                {y,z,0},
                {z,x,0},

                {z,x,y},
                {x,0,z},
                {y,0,x},
                {z,0,y},
            };

            // 3-bit: bit2,bit1,bit0
            int b210 = 4 * b2 + 2 * b1 + b0;

            double p = magnitude[b210,0];
            double q = magnitude[b210,1];
            double r = magnitude[b210,2];

            double[,] octant = new double[8,3]
            {
                {-p,-q, r},
                { p,-q,-r},
                {-p, q,-r},
                { p, q, r},

                { p, q,-r},
                {-p, q, r},
                { p,-q, r},
                {-p,-q,-r},
            };

            // 3-bit: bit5,bit4,bit3
            int b543 = 4 * b5 + 2 * b4 + b3;

            p = octant[b543,0];
            q = octant[b543,1];
            r = octant[b543,2];
            
            return p + q + r;
        }
    }
}
