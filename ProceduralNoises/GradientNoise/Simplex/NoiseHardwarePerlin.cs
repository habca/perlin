﻿using System;

namespace ProceduralNoises
{
    public static class NoiseHardwarePerlin {
        static int i,j,k;
        static int[] A = {0,0,0};
        static double u,v,w;
        public static double noise(double x, double y, double z) {
            double s = (x+y+z)/3.0;
            i=(int)Math.Floor(x+s); j=(int)Math.Floor(y+s); k=(int)Math.Floor(z+s);
            s = (i+j+k)/6.0; u = x-i+s; v = y-j+s; w = z-k+s;
            A[0]=A[1]=A[2]=0;
            int hi = u>=w ? u>=v ? 0 : 1 : v>=w ? 1 : 2;
            int lo = u< w ? u< v ? 0 : 1 : v< w ? 1 : 2;
            return K(hi) + K(3-hi-lo) + K(lo) + K(0);
        }
        static double K(int a) {
            double s = (A[0]+A[1]+A[2])/6.0;
            double x = u-A[0]+s, y = v-A[1]+s, z = w-A[2]+s, t = .6-x*x-y*y-z*z;
            int h = shuffle(i+A[0],j+A[1],k+A[2]);
            A[a]++;
            if (t < 0)
            return 0;
            int b5 = h>>5 & 1, b4 = h>>4 & 1, b3 = h>>3 & 1, b2= h>>2 & 1, b = h & 3;
            double p = b==1?x:b==2?y:z, q = b==1?y:b==2?z:x, r = b==1?z:b==2?x:y;
            p = (b5==b3 ? -p : p); q = (b5==b4 ? -q : q); r = (b5!=(b4^b3) ? -r : r);
            t *= t;
            return 8 * t * t * (p + (b==0 ? q+r : b2==0 ? q : r));
        }
        public static int shuffle(int i, int j, int k) {
            return b(i,j,k,0) + b(j,k,i,1) + b(k,i,j,2) + b(i,j,k,3) +
            b(j,k,i,4) + b(k,i,j,5) + b(i,j,k,6) + b(j,k,i,7) ;
        }
        public static int b(int i, int j, int k, int B) { return T[b(i,B)<<2 | b(j,B)<<1 | b(k,B)]; }
        public static int b(int N, int B) { return N>>B & 1; }
        public static int[] T = {0x15,0x38,0x32,0x2c,0x0d,0x13,0x07,0x2a};
    }
}
