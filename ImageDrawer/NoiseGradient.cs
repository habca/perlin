using System;
using System.Numerics;

using Perlin;
using Simplex;

namespace ImageDrawer
{
    public static class NoiseGradients
    {
        public static void Run()
        {
            Console.WriteLine("Classic noise 3D:");
            Vector3[] perlin = TestPerlin();
            foreach (var grad in perlin)
            {
                Console.WriteLine(grad);
            }

            Console.WriteLine("Simplex noise 3D:");
            Vector3[] simplex = TestSimplex();
            foreach (var grad in simplex)
            {
                Console.WriteLine(grad);
            }

            Console.WriteLine("Classic noise 4D:");
            Vector4[] perlin4 = TestPerlin4();
            foreach (var grad in perlin4)
            {
                Console.WriteLine(grad);
            }
        }

        private static Vector3[] TestPerlin()
        {
            Vector3[] grads = new Vector3[16];
            for (int h = 0; h < grads.Length; h++)
            {
                grads[h].X = (float)ImprovedNoisePerlin.grad(h, 1, 0, 0);
                grads[h].Y = (float)ImprovedNoisePerlin.grad(h, 0, 1, 0);
                grads[h].Z = (float)ImprovedNoisePerlin.grad(h, 0, 0, 1);
            }
            return grads;
        }

        private static Vector3[] TestSimplex()
        {
            Vector3[] grads = new Vector3[64];
            for (int h = 0; h < grads.Length; h++)
            {
                grads[h].X = (float)NoiseHardware.grad(h, 1, 0, 0);
                grads[h].Y = (float)NoiseHardware.grad(h, 0, 1, 0);
                grads[h].Z = (float)NoiseHardware.grad(h, 0, 0, 1);
            }
            return grads;
        }

        private static Vector4[] TestPerlin4()
        {
            Vector4[] grads = new Vector4[32];
            for (int h = 0; h < grads.Length; h++)
            {
                grads[h].X = (float)ImprovedNoise4DPerlin.grad(h, 1, 0, 0, 0);
                grads[h].Y = (float)ImprovedNoise4DPerlin.grad(h, 0, 1, 0, 0);
                grads[h].Z = (float)ImprovedNoise4DPerlin.grad(h, 0, 0, 1, 0);
                grads[h].W = (float)ImprovedNoise4DPerlin.grad(h, 0, 0, 0, 1);
            }
            return grads;
        }
    }
}