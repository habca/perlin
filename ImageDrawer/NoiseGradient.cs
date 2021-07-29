using System;
using System.Numerics;

using ProceduralNoises;

namespace ImageDrawer
{
    public static class NoiseGradients
    {
        public static void Run(Func<Vector3[]> gradientNoise)
        {
            foreach (var gradientVector in gradientNoise())
            {
                Console.WriteLine(gradientVector);
            }
        }

        public static void Run(Func<Vector4[]> gradientNoise)
        {
            foreach (var gradientVector in gradientNoise())
            {
                Console.WriteLine(gradientVector);
            }
        }

        public static Vector3[] GradientsPerlin()
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

        public static Vector3[] GradientsSimplex()
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

        public static Vector4[] GradientsPerlin4D()
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

        public static Vector3[] GradientsPerlin3D()
        {
            Vector3[] grads = new Vector3[16];
            for (int h = 0; h < grads.Length; h++)
            {
                grads[h].X = (float)ImprovedNoise3DPerlin.grad(h, 1, 0, 0);
                grads[h].Y = (float)ImprovedNoise3DPerlin.grad(h, 0, 1, 0);
                grads[h].Z = (float)ImprovedNoise3DPerlin.grad(h, 0, 0, 1);
            }
            return grads;
        }
    }
}