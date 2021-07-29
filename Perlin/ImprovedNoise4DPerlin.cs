// https://mrl.cs.nyu.edu/~perlin/noise/ImprovedNoise4D.java

using System;

using static Perlin.ImprovedNoisePerlin;

namespace Perlin
{
    public static class ImprovedNoise4DPerlin {
        public static double noise(double x, double y, double z, double w) {
            int X = (int)Math.Floor(x) & 255,
                Y = (int)Math.Floor(y) & 255,
                Z = (int)Math.Floor(z) & 255,
                W = (int)Math.Floor(w) & 255;

            x -= Math.Floor(x);
            y -= Math.Floor(y);
            z -= Math.Floor(z);
            w -= Math.Floor(w);

            double a = fade(x),
                   b = fade(y),
                   c = fade(z),
                   d = fade(w);
            
            int A = p[X  ]+Y, AA = p[A]+Z, AB = p[A+1]+Z,
                B = p[X+1]+Y, BA = p[B]+Z, BB = p[B+1]+Z,
                AAA = p[AA]+W, AAB = p[AA+1]+W,
                ABA = p[AB]+W, ABB = p[AB+1]+W,
                BAA = p[BA]+W, BAB = p[BA+1]+W,
                BBA = p[BB]+W, BBB = p[BB+1]+W;

            return lerp(d,
                lerp(c,lerp(b,lerp(a,grad(p[AAA  ], x  , y  , z  , w), 
                                     grad(p[BAA  ], x-1, y  , z  , w)),
                              lerp(a,grad(p[ABA  ], x  , y-1, z  , w), 
                                     grad(p[BBA  ], x-1, y-1, z  , w))),

                       lerp(b,lerp(a,grad(p[AAB  ], x  , y  , z-1, w), 
                                     grad(p[BAB  ], x-1, y  , z-1, w)),
                              lerp(a,grad(p[ABB  ], x  , y-1, z-1, w),
                                     grad(p[BBB  ], x-1, y-1, z-1, w)))),

                lerp(c,lerp(b,lerp(a,grad(p[AAA+1], x  , y  , z  , w-1), 
                                     grad(p[BAA+1], x-1, y  , z  , w-1)),
                              lerp(a,grad(p[ABA+1], x  , y-1, z  , w-1), 
                                     grad(p[BBA+1], x-1, y-1, z  , w-1))),

                       lerp(b,lerp(a,grad(p[AAB+1], x  , y  , z-1, w-1), 
                                     grad(p[BAB+1], x-1, y  , z-1, w-1)),
                              lerp(a,grad(p[ABB+1], x  , y-1, z-1, w-1),
                                     grad(p[BBB+1], x-1, y-1, z-1, w-1)))));
        }
        public static double grad(int hash, double x, double y, double z, double w) {
            int h = hash & 31;
            double a=y,b=z,c=w;            // X,Y,Z
            switch (h >> 3) {
            case 1: a=w;b=x;c=y;break;     // W,X,Y
            case 2: a=z;b=w;c=x;break;     // Z,W,X
            case 3: a=y;b=z;c=w;break;     // Y,Z,W
            }
            return ((h&4)==0 ? -a:a) + ((h&2)==0 ? -b:b) + ((h&1)==0 ? -c:c);
        }
    }
}