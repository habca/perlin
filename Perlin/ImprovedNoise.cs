using System;

namespace Perlin
{
    public class ImprovedNoise
    {
        public struct Vector4 {
            public double x, y, z, w;

            public Vector4(double x, double y, double z, double w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }
        };

        public struct Vector3 {
            public double x, y, z;
            public Vector3(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            public Vector3(double a)
            {
                this.x = this.y = this.z = a;
            }
            public static Vector3 operator +(Vector3 a, Vector3 b)
            {
                return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
            }
            public static Vector3 zero = new Vector3(0,0,0);
            public static Vector3 operator- (Vector3 a, Vector3 b)
            {
                Vector3 c = new Vector3();
                c.x = b.x - a.x;
                c.y = b.y - a.x;
                c.z = b.z - a.z;
                return c;
            }
            public static Vector3 operator+ (Vector3 a, double b) 
            {
                Vector3 c = new Vector3();
                c.x = a.x + b;
                c.y = a.y + b;
                c.z = a.z + b;
                return c;
            }
        }

        public struct Vector2 {
            public double x, y;
            public Vector2(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public static double magnitude(Vector3 vector)
        {
            double a = vector.x * vector.x;
            double b = vector.y * vector.y;
            double c = vector.z * vector.z;
            return Math.Sqrt(a + b + c); 
        }

        public static Vector3 normalize(Vector4 vector)
        {
            return normalize(new Vector3(vector.y, -1, vector.w));
        }

        public static Vector3 normalize(Vector3 vector)
        {
            double dist = magnitude(vector);
            double a = vector.x / dist;
            double b = vector.y / dist;
            double c = vector.z / dist;
            return new Vector3(a,b,c);
        }

        public static Vector3 cross(Vector3 vec1, Vector3 vec2)
        {
            double a = vec1.y * vec2.z - vec1.z * vec2.y;
            double b = vec1.z * vec2.x - vec1.x * vec2.z;
            double c = vec1.x * vec2.y - vec1.y * vec2.x;
            return new Vector3(a,b,c);
        }

        public static double dot(Vector3 vec1, Vector3 vec2)
        {
            double a = vec1.x * vec2.x;
            double b = vec1.y * vec2.y;
            double c = vec1.z * vec2.z;
            return a + b + c;
        }

        public static int floor(double x)
        {
            return x > 0 ? (int) x : (int) (x - 1);
        }

        public static double gradient(double x1, double x2, double y1, double y2)
        {
            return (y2 - y1) / (x2 - x1);
        }

        public static int hash(int x, int y, int z)
        {
            return p[ p[ p[x] + y] + z] & 15;
        }

        public static Vector3[] gradients = new Vector3[16]
        {
            new (1,1,0), new (-1,1,0), new (1,-1,0), new (-1,-1,0),
            new (1,0,1), new (-1,0,1), new (1,0,-1), new (-1,0,-1),
            new (0,1,1), new (0,-1,1), new (0,1,-1), new (0,-1,-1),
            new (1,1,0), new (-1,1,0), new (0,-1,1), new (0,-1,-1),
        };

        public static Vector4 noised(double x, double y, double z)
        {
            int xi = floor(x) & 255;
            int yi = floor(y) & 255;
            int zi = floor(z) & 255;

            double tx = x - floor(x);
            double ty = y - floor(y);
            double tz = z - floor(z);

            double u = fade(tx);
            double v = fade(ty);
            double w = fade(tz);

            double du = derva(tx);
            double dv = derva(ty);
            double dw = derva(tz);

            /*
            int A = p[xi0]+yi0, AA = p[A]+zi0, AB = p[A+1]+zi0;
            int B = p[xi1]+yi0, BA = p[B]+zi0, BB = p[B+1]+zi0;

            double va = grad(p[AA], tx  , ty, tz);
            double vb = grad(p[BA], tx-1, ty, tz);
            double vc = grad(p[AB], tx, ty-1, tz);
            double vd = grad(p[BB], tx-1, ty-1, tz);
            double ve = grad(p[AA+1], tx, ty, tz-1);
            double vf = grad(p[BA+1], tx-1, ty, tz-1);
            double vg = grad(p[AB+1], tx, ty-1, tz-1);
            double vh = grad(p[BB+1], tx-1, ty-1, tz-1);
            */

            Vector3 c000 = gradients[hash(xi,   yi,   zi)];
            Vector3 c100 = gradients[hash(xi+1, yi,   zi)];
            Vector3 c010 = gradients[hash(xi,   yi+1, zi)];
            Vector3 c110 = gradients[hash(xi+1, yi+1, zi)];
    
            Vector3 c001 = gradients[hash(xi,   yi,   zi+1)];
            Vector3 c101 = gradients[hash(xi+1, yi,   zi+1)];
            Vector3 c011 = gradients[hash(xi,   yi+1, zi+1)];
            Vector3 c111 = gradients[hash(xi+1, yi+1, zi+1)];

            Vector3 p000 = new Vector3(tx,   ty,   tz);
            Vector3 p100 = new Vector3(tx-1, ty,   tz);
            Vector3 p010 = new Vector3(tx,   ty-1, tz);
            Vector3 p110 = new Vector3(tx-1, ty-1, tz);
    
            Vector3 p001 = new Vector3(tx,   ty,   tz-1);
            Vector3 p101 = new Vector3(tx-1, ty,   tz-1);
            Vector3 p011 = new Vector3(tx,   ty-1, tz-1);
            Vector3 p111 = new Vector3(tx-1, ty-1, tz-1);

            double va = dot(c000, p000);
            double vb = dot(c100, p100);
            double vc = dot(c010, p010);
            double vd = dot(c110, p110);

            double ve = dot(c001, p001);
            double vf = dot(c101, p101);
            double vg = dot(c011, p011);
            double vh = dot(c111, p111);

            double k0 = va;
            double k1 = vb - va;
            double k2 = vc - va;
            double k3 = ve - va;
            double k4 = va - vb - vc + vd;
            double k5 = va - vc - ve + vg;
            double k6 = va - vb - ve + vf;
            double k7 = -va + vb + vc - vd + ve - vf - vg + vh;

            double dx = du * (k1 + k4 * v + k6 * w + k7 * v * w);
            double dy = dv * (k2 + k4 * u + k5 * w + k7 * u * w);
            double dz = dw * (k3 + k5 * v + k6 * u + k7 * u * v);

            double val = k0 + k1 * u + k2 * v + k3 * w + k4 * u * v + k5 * v * w + k6 * w * u + k7 * u * v * w;

            /*
            double val = lerp(w,
                        lerp(v,
                            lerp(u, va, vb),
                            lerp(u, vc, vd)),
                        lerp(v, 
                            lerp(u, ve, vf),
                            lerp(u, vg, vh)));
            */

            return new Vector4(val, dx, dy, dz);
        }

        static public double noise(double x, double y, double z) {
            int X = (int)Math.Floor(x) & 255;                  // FIND UNIT CUBE THAT
            int Y = (int)Math.Floor(y) & 255;                  // CONTAINS POINT.
            int Z = (int)Math.Floor(z) & 255;
            
            x -= Math.Floor(x);                                // FIND RELATIVE X,Y,Z
            y -= Math.Floor(y);                                // OF POINT IN CUBE.
            z -= Math.Floor(z);
            
            double u = fade(x);                                // COMPUTE FADE CURVES
            double v = fade(y);                                // FOR EACH OF X,Y,Z.
            double w = fade(z);
            
            int A = p[X  ]+Y, AA = p[A]+Z, AB = p[A+1]+Z;      // HASH COORDINATES OF
            int B = p[X+1]+Y, BA = p[B]+Z, BB = p[B+1]+Z;      // THE 8 CUBE CORNERS,

            return lerp(w,
                        lerp(v,
                            lerp(u, grad(p[AA  ], x  , y  , z   ),  // AND ADD
                                    grad(p[BA  ], x-1, y  , z   )), // BLENDED
                            lerp(u, grad(p[AB  ], x  , y-1, z   ),  // RESULTS
                                    grad(p[BB  ], x-1, y-1, z   ))),// FROM  8
                        lerp(v, 
                            lerp(u, grad(p[AA+1], x  , y  , z-1 ),  // CORNERS
                                    grad(p[BA+1], x-1, y  , z-1 )), // OF CUBE
                            lerp(u, grad(p[AB+1], x  , y-1, z-1 ),
                                    grad(p[BB+1], x-1, y-1, z-1 ))));
        }

        static double fade(double t)
        { 
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        static double derva(double t)
        {
            return 30 * t * t * (t * (t - 2) + 1);
        }

        static double lerp(double t, double a, double b)
        {
            return a + t * (b - a);
            //return (1-t)*a + t*b;
        }

        static double grad(int hash, double x, double y, double z) {
            int h = hash & 15;
            double u = h<8 ? x : y,
                    v = h<4 ? y : h==12||h==14 ? x : z;
            return ((h&1) == 0 ? u : -u) + ((h&2) == 0 ? v : -v);
        }

        public static int[] p = new int[512];
        public static int[] permutation = { 151,160,137,91,90,15,
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

        static ImprovedNoise()
        { 
            for (int i=0; i < 256; i++)
            {
                p[256+i] = p[i] = permutation[i];
            }
        }
    }
}
