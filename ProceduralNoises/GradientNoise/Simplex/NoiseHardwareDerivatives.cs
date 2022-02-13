using System;
using System.Numerics;

using static ProceduralNoises.NoiseHardwarePerlin;
using static ProceduralNoises.SimplexNoiseGustavson;

namespace ProceduralNoises
{
    public static class NoiseHardwareDerivatives
    {
        static int i, j, k;
        static int[] A = { 0, 0, 0 };
        static double u, v, w;
        static double dx, dy, dz;
        public static double noise(double x, double y, double z, out Vector3 derva)
        {
            double s = (x + y + z) / 3.0;
            i = (int)Math.Floor(x + s); j = (int)Math.Floor(y + s); k = (int)Math.Floor(z + s);
            s = (i + j + k) / 6.0; u = x - i + s; v = y - j + s; w = z - k + s;
            A[0] = A[1] = A[2] = 0;
            int hi = u >= w ? u >= v ? 0 : 1 : v >= w ? 1 : 2;
            int lo = u < w ? u < v ? 0 : 1 : v < w ? 1 : 2;
            dx = dy = dz = .0;
            double val = K(hi) + K(3 - hi - lo) + K(lo) + K(0);
            derva = new Vector3((float)dx, (float)dy, (float)dz);
            return val;
        }
        static double K(int a)
        {
            double s = (A[0] + A[1] + A[2]) / 6.0;
            double x = u - A[0] + s, y = v - A[1] + s, z = w - A[2] + s, t = .6 - x * x - y * y - z * z;
            int h = shuffle(i + A[0], j + A[1], k + A[2]);
            A[a]++;
            if (t < 0)
                return 0;
            int b5 = h >> 5 & 1, b4 = h >> 4 & 1, b3 = h >> 3 & 1, b2 = h >> 2 & 1, b = h & 3;
            double p = b == 1 ? x : b == 2 ? y : z, q = b == 1 ? y : b == 2 ? z : x, r = b == 1 ? z : b == 2 ? x : y;
            p = (b5 == b3 ? -p : p); q = (b5 == b4 ? -q : q); r = (b5 != (b4 ^ b3) ? -r : r);
            double t0 = t; t *= t;
            double tmp1 = -64 * t * t0 * (p + (b == 0 ? q + r : b2 == 0 ? q : r)), tmp2 = 8 * t * t;
            dx += tmp1 * x + tmp2 * grad3[h & 63].X;
            dy += tmp1 * y + tmp2 * grad3[h & 63].Y;
            dz += tmp1 * z + tmp2 * grad3[h & 63].Z;
            return tmp2 * (p + (b == 0 ? q + r : b2 == 0 ? q : r));
        }
    }
}
