// 

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Numerics;

using ProceduralNoises;
using static ProceduralNoises.NoiseHardwarePerlin;
using static ProceduralNoises.SimplexNoiseGustavson;

namespace ProceduralNoisesTest
{
    [TestClass]
    public class NoiseHardwareTest
    {
        private readonly double eps = 0.01;
        private readonly double step = 0.05;
        private readonly int min = -5;
        private readonly int max = 5;

        [TestMethod]
        public void SimplexNoisePerlinImplementationTest()
        {
            for (double i = min; i < max; i += step)
            {
                for (double j = min; j < max; j += step)
                {
                    for (double k = min; k < max; k += step)
                    {
                        double perlin = NoiseHardwarePerlin.noise(i, j, k);
                        double custom = NoiseHardware.noise(i, j, k);

                        Assert.AreEqual(expected: perlin, actual: custom, delta: eps);
                    }
                }
            }
        }

        [TestMethod]
        public void SimplexNoiseGustavsonImplementationTest()
        {
            for (double i = min; i < max; i += step)
            {
                for (double j = min; j < max; j += step)
                {
                    for (double k = min; k < max; k += step)
                    {
                        double perlin = NoiseHardwarePerlin.noise(i, j, k);
                        Vector4 gustavson = SimplexNoiseGustavson.noise(i, j, k);

                        Assert.AreEqual(expected: perlin, actual: gustavson.X, delta: eps);
                    }
                }
            }
        }

        [TestMethod]
        public void SimplexNoiseDerivativesImplementationTest()
        {
            for (double i = min; i < max; i += step)
            {
                for (double j = min; j < max; j += step)
                {
                    for (double k = min; k < max; k += step)
                    {
                        double perlin = NoiseHardwarePerlin.noise(i, j, k);
                        double custom = NoiseHardwareDerivatives.noise(i, j, k, out Vector3 _);

                        Assert.AreEqual(expected: perlin, actual: custom, delta: eps);
                    }
                }
            }
        }

        [TestMethod]
        public void SimplexNoiseGradientVectorTest()
        {
            for (double i = min; i < max; i += step)
            {
                for (double j = min; j < max; j += step)
                {
                    for (double k = min; k < max; k += step)
                    {
                        Vector4 perlin = NoiseHardwareDerivativesTest.noise(i, j, k);
                        Vector4 gustavson = SimplexNoiseGustavsonTest.noise(i, j, k);

                        Assert.AreEqual(expected: perlin.X, actual: gustavson.X, delta: eps);
                        Assert.AreEqual(expected: perlin.Y, actual: gustavson.Y, delta: eps);
                        Assert.AreEqual(expected: perlin.Z, actual: gustavson.Z, delta: eps);
                        Assert.AreEqual(expected: perlin.W, actual: gustavson.W, delta: eps);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Test class for SimplexNoiseGradientVectorTest
    /// </summary>
    public static class NoiseHardwareDerivativesTest
    {
        static int[] sum(int[] a, int[] b)
        {
            return new int[] { a[0] + b[0], a[1] + b[1], a[2] + b[2] };
        }
        public static double skew(double x, double y, double z)
        {
            return (x + y + z) / 3.0;
        }
        public static double unskew(double si, double sj, double sk)
        {
            // simplex noise demystified
            // (0,0,0) sama kuin x0,y0,z0
            // (1,0,0) = (i1,j1,k1) --> 1/6 = G3
            // (1,1,0) = (i2,j2,k2) --> 2/6 = 2.0*G3
            // (1,1,1) = (1.0,1.0,1.0) --> 3/6 = 3.0*G3
            return (si + sj + sk) / 6.0;
        }
        public static Vector4 noise(double x, double y, double z)
        {

            double s = skew(x, y, z);

            int si = (int)Math.Floor(x + s),
                sj = (int)Math.Floor(y + s),
                sk = (int)Math.Floor(z + s);

            double t = unskew(si, sj, sk);
            double u = x - si + t;
            double v = y - sj + t;
            double w = z - sk + t;

            // simplex vertice traversal order
            int[,][] simplex = new int[6, 4][]
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
                        u >= v ? 2 : // Z X Y
                        u < v && v < w ? 3 : // Z Y X
                        u < v && u < w ? 4 : // Y Z X
                        u < v ? 5 : // Y X Z
                                          -1; // error*/

            int i1, j1, k1, i2, j2, k2;
            if (u >= v)
            {
                if (v >= w)
                {
                    // X Y Z order
                    i1 = 1;
                    j1 = 0;
                    k1 = 0;
                    i2 = 1;
                    j2 = 1;
                    k2 = 0;
                }
                // z > y
                else if (u >= w)
                {
                    // X Z Y order
                    i1 = 1;
                    j1 = 0;
                    k1 = 0;
                    i2 = 1;
                    j2 = 0;
                    k2 = 1;
                }
                // z > y && z > x
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
                // x < y
                if (v <= w)
                {
                    // Z Y X order
                    i1 = 0;
                    j1 = 0;
                    k1 = 1;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                }
                // x < y && z < y
                else if (u <= w)
                {
                    // Y Z X order
                    i1 = 0;
                    j1 = 1;
                    k1 = 0;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                }
                // x < y && z < y && z < x
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

            int[] A = simplex[index, 0],
                    B = simplex[index, 1],
                    C = simplex[index, 2],
                    D = simplex[index, 3];

            /*
            return new Vector4(
                   K(si, sj, sk, A),
                   K(si, sj, sk, sum(A, B)),
                   K(si, sj, sk, sum(sum(A, B), C)),
                   K(si, sj, sk, sum(sum(sum(A, B), C), D)));
            */

            /*
            return new Vector4(
                shuffle(si + (A[0]),                      sj + (A[1]),                      sk + (A[2])                     ) & 63,
                shuffle(si + (A[0] + B[0]),               sj + (A[1] + B[1]),               sk + (A[2] + B[2])              ) & 63,
                shuffle(si + (A[0] + B[0] + C[0]),        sj + (A[1] + B[1] + C[1]),        sk + (A[2] + B[2] + C[2])       ) & 63,
                shuffle(si + (A[0] + B[0] + C[0] + D[0]), sj + (A[1] + B[1] + C[1] + D[1]), sk + (A[2] + B[2] + C[2] + D[2])) & 63
            );
            */

            // OK TOIMII
            //return new Vector4(si, sj, sk, 0);
            //return new Vector4((float)u, (float)v, (float)w, 0);
            //return new Vector4(A[0], A[1], A[2], 0);

            // EI TOIMI!
            return new Vector4(i1, j1, k1, 0);
        }

        static int K(
            int si, int sj, int sk,
            int[] A
        )
        {
            int h = shuffle(si + A[0], sj + A[1], sk + A[2]);
            return grad(h);
        }

        public static int grad(int h)
        {
            // 6-bit gradient index
            int b5 = b(h, 5);
            int b4 = b(h, 4);
            int b3 = b(h, 3);

            int b2 = b(h, 2);
            int b1 = b(h, 1);
            int b0 = b(h, 0);

            return 32 * b5 + 16 * b4 + 8 * b3 + 4 * b2 + 2 * b1 + b0;
        }
    }

    /// <summary>
    /// Test class for SimplexNoiseGradientVectorTest
    /// </summary>
    public static class SimplexNoiseGustavsonTest
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
                // z > y
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
                // z > y && z > x
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
                // x < y
                if (y0 <= z0)
                {
                    // Z Y X order
                    i1 = 0;
                    j1 = 0;
                    k1 = 1;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                }
                // x < y && z < y
                else if (x0 <= z0)
                {
                    // Y Z X order
                    i1 = 0;
                    j1 = 1;
                    k1 = 0;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                }
                // x < y && z < y && z < x
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

            int gi0 = NoiseHardwareDerivativesTest.grad(shuffle(i, j, k));
            int gi1 = NoiseHardwareDerivativesTest.grad(shuffle(i + i1, j + j1, k + k1));
            int gi2 = NoiseHardwareDerivativesTest.grad(shuffle(i + i2, j + j2, k + k2));
            int gi3 = NoiseHardwareDerivativesTest.grad(shuffle(i + 1, j + 1, k + 1));

            //return new Vector4(gi0, gi1, gi2, gi3);

            // OK TOIMII
            //return new Vector4(i, j, k, 0);
            //return new Vector4((float)x0, (float)y0, (float)z0, 0);
            //return new Vector4(0, 0, 0, 0);

            // EI TOIMI!
            return new Vector4(i1, j1, k1, 0);
        }
    }
}
