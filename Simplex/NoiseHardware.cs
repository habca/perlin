using System;

namespace Simplex
{
    public static class NoiseHardware
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
        }
        
        static int[] sum(int[] a, int[] b)
        {
            return new int[]{a[0]+b[0],a[1]+b[1],a[2]+b[2]};
        }
        public static (int,int,int) floor(double sx, double sy, double sz)
        {
            return ((int)Math.Floor(sx),(int)Math.Floor(sy),(int)Math.Floor(sz));
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
            (int    si, int    sj, int    sk) = floor ( sx,  sy,  sz);

            (double  i, double  j, double  k) = unskew(si, sj, sk);
            // artikkelissa simplex noise demystified x0,y0,z0
            (double  u, double  v, double  w) = (x-i, y-j, z-k);

            // simplex vertice traversal order
            int[,][] simplex = new int[6,4][]
            {
                { new int[]{0,0,0}, new int[]{1,0,0}, new int[]{0,1,0}, new int[]{0,0,1} }, // X Y Z
                { new int[]{0,0,0}, new int[]{1,0,0}, new int[]{0,0,1}, new int[]{0,1,0} }, // X Z Y
                { new int[]{0,0,0}, new int[]{0,0,1}, new int[]{1,0,0}, new int[]{0,1,0} }, // Z X Y
                { new int[]{0,0,0}, new int[]{0,0,1}, new int[]{0,1,0}, new int[]{1,0,0} }, // Z Y X
                { new int[]{0,0,0}, new int[]{0,1,0}, new int[]{0,0,1}, new int[]{1,0,0} }, // Y Z X
                { new int[]{0,0,0}, new int[]{0,1,0}, new int[]{1,0,0}, new int[]{0,0,1} }, // Y X Z
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
            // artikkelissa simplex noise demystified
            // x0 - (i1 - G3) sama kuin x0 - i1 + G3
            (double i, double j, double k) = unskew(A[0], A[1], A[2]);
            (double u, double v, double w) = (x-i, y-j, z-k);

            double t = 0.6 - u*u - v*v - w*w;
            if (t < 0) return 0;

            // hash pseudorandom 6-bit gradient index 
            int h = shuffle(si + A[0], sj + A[1], sk + A[2]);

            // artikkelissa demystifying simplex noise
            // h = gradientIndex ja x0,y0,z0 = u,v,w
            // return 8 * t*t*t*t * dot(gradients[gradientIndex], x0, y0, z0);
            return 8 * t*t*t*t * gradient(h, u, v, w);

            // float x = point.x - ix;
		    // float f = 1f - x * x;
		    // float f2 = f * f;
		    // float f3 = f * f2;
            // Vector3 g = gradients3D[hash];
			// float v = Dot(g, x, y, z);
			// float v6f2 = -6f * v * f2;
			// sample.value = v * f3;
			// sample.derivative.x = g.x * f3 + v6f2 * x;
			// sample.derivative.y = g.y * f3 + v6f2 * y;
			// sample.derivative.z = g.z * f3 + v6f2 * z;
        }

        static double gradient(int h, double x, double y, double z)
        {
            // 6-bit gradient index
            int b5 = bit(h,5);
            int b4 = bit(h,4);
            int b3 = bit(h,3);

            int b2 = bit(h,2);
            int b1 = bit(h,1);
            int b0 = bit(h,0);

            double[,] magnitude = new double[8,3]
            {
                {x,y,z},
                {y,z,0},
                {z,x,0},
                {x,y,0},

                {x,y,z},
                {y,0,x},
                {z,0,y},
                {x,0,z},
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
        static int shuffle(int i, int j, int k) {
            return b(i,j,k,0) + b(j,k,i,1) + b(k,i,j,2) + b(i,j,k,3) +
                   b(j,k,i,4) + b(k,i,j,5) + b(i,j,k,6) + b(j,k,i,7);
        }
        static int b(int i, int j, int k, int B)
        {
            int patternIndex = 4 * bit(i,B) + 2 * bit(j,B) + bit(k,B);
            return bitPatterns[patternIndex];
        }
        static int bit(int N, int B)
        {
            return (N >> B) & 1;
        }
        static int[] bitPatterns = {0x15,0x38,0x32,0x2c,0x0d,0x13,0x07,0x2a};
    }
}
