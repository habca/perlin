using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Numerics;

using Perlin;

namespace PerlinTest
{
    [TestClass]
    public class ImprovedNoiseTest
    {
        static double eps = 1E-5;
        static double step = 5E-2;
        static int min = -5;
        static int max = 5;

        [TestMethod]
        public void NoiseTest()
        {
            for (double i = min; i < max; i+=step)
            {
                for (double j = min; j < max; j+=step)
                {
                    for (double k = min; k < max; k+=step)
                    {
                        double perlin = ImprovedNoisePerlin.noise(i, j, k);
                        Vector4 bourke = ImprovedNoiseBourke.noise(i, j, k);
                        Vector4 quilez = ImprovedNoiseQuilez.noise(i, j, k);

                        Assert.AreEqual(perlin, bourke.X, eps);
                        Assert.AreEqual(perlin, quilez.X, eps);
                    }
                }
            }
        }

        [TestMethod]
        public void NoiseDerivativesTest()
        {
            for (double i = min; i < max; i+=step)
            {
                for (double j = min; j < max; j+=step)
                {
                    for (double k = min; k < max; k+=step)
                    {
                        Vector4 bourke = ImprovedNoiseBourke.noise(i, j, k);
                        Vector4 quilez = ImprovedNoiseQuilez.noise(i, j, k);

                        Assert.AreEqual(quilez.Y, bourke.Y, eps);
                        Assert.AreEqual(quilez.Z, bourke.Z, eps);
                        Assert.AreEqual(quilez.W, bourke.W, eps);
                    }
                }
            }
        }
    }
}
