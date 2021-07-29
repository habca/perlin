// https://mrl.cs.nyu.edu/~perlin/noise/INoise.java

using System;

using static ProceduralNoises.ImprovedNoisePerlin;

namespace ProceduralNoises
{
    public static class ImprovedNoise3DPerlin
    {
        static public int noise(int x, int y, int z) {
            int X = x>>16 & 255, Y = y>>16 & 255, Z = z>>16 & 255, N = 1<<16;
            x &= N-1; y &= N-1; z &= N-1;
            int u=Fade(x),v=Fade(y),w=Fade(z), A=p[X  ]+Y, AA=p[A]+Z, AB=p[A+1]+Z,
                                                B=p[X+1]+Y, BA=p[B]+Z, BB=p[B+1]+Z;
            return lerp(w, lerp(v, lerp(u, grad(p[AA  ], x   , y   , z   ),  
                                            grad(p[BA  ], x-N , y   , z   )), 
                                    lerp(u, grad(p[AB  ], x   , y-N , z   ),  
                                            grad(p[BB  ], x-N , y-N , z   ))),
                            lerp(v, lerp(u, grad(p[AA+1], x   , y   , z-N ),  
                                            grad(p[BA+1], x-N , y   , z-N )), 
                                    lerp(u, grad(p[AB+1], x   , y-N , z-N ),
                                            grad(p[BB+1], x-N , y-N , z-N ))));
        }
        static int lerp(int t, int a, int b) { return a + (t * (b - a) >> 12); }
        public static int grad(int hash, int x, int y, int z) {
            int h = hash&15, u = h<8?x:y, v = h<4?y:h==12||h==14?x:z;
            return ((h&1) == 0 ? u : -u) + ((h&2) == 0 ? v : -v);
        }
        static int Fade(int t) {
            int t0 = fade[t >> 8], t1 = fade[Math.Min(255, (t >> 8) + 1)];
            return t0 + ( (t & 255) * (t1 - t0) >> 8 );
        }
        static int[] fade = new int[256];
        static ImprovedNoise3DPerlin() { for (int i=0; i < 256 ; i++) fade[i] = (int)((1<<12)*f(i/256.0)); }
        static double f(double t) { return t * t * t * (t * (t * 6 - 15) + 10); }
    }
}