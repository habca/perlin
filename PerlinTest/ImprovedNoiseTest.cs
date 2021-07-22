using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perlin;
using System;
using System.Numerics;

namespace PerlinTest
{
    [TestClass]
    public class ImprovedNoiseTest
    {
        [TestMethod]
        public void NoisedTest1()
        {
            Assert.AreEqual(1E-3, 0.001);
            Assert.AreEqual(1E-4, 0.0001);

            double eps = 1E-5;
            Random random = new Random();
            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    for (int k = -10; k < 10; k++)
                    {
                        double c = random.NextDouble();

                        double e1 = ImprovedNoise.noise(i, j, k);
                        double a1 = ImprovedNoise.noised(i, j, k).X;

                        double e2 = ImprovedNoise.noise(i+eps, j+eps, k+eps);
                        double a2 = ImprovedNoise.noised(i+eps, j+eps, k+eps).X;

                        double e3 = ImprovedNoise.noise(-i, -j, -k);
                        double a3 = ImprovedNoise.noised(-i, -j, -k).X;

                        double e4 = ImprovedNoise.noise(-i-eps, -j-eps, -k-eps);
                        double a4 = ImprovedNoise.noised(-i-eps, -j-eps, -k-eps).X;

                        /*
                        double e5 = ImprovedNoise.noise(c, c, c);
                        double a5 = ImprovedNoise.noised(c, c, c).x;

                        double e6 = ImprovedNoise.noise(c+eps, c+eps, c+eps);
                        double a6 = ImprovedNoise.noised(c+eps, c+eps, c+eps).x;

                        double e7 = ImprovedNoise.noise(-c, -c, -c);
                        double a7 = ImprovedNoise.noised(-c, -c, -c).x;

                        double e8 = ImprovedNoise.noise(-c-eps, -c-eps, -c-eps);
                        double a8 = ImprovedNoise.noised(-c-eps, -c-eps, -c-eps).x;
                        */

                        Assert.AreEqual(e1, a1, eps);
                        Assert.AreEqual(e2, a2, eps);
                        Assert.AreEqual(e3, a3, eps);
                        Assert.AreEqual(e4, a4, eps);

                        /*
                        Assert.AreEqual(e5, a5, eps);
                        Assert.AreEqual(e6, a6, eps);
                        Assert.AreEqual(e7, a7, eps);
                        Assert.AreEqual(e8, a8, eps);
                        */
                    }
                }
            }
        }

        [TestMethod]
        public void NoisedTest5()
        {
            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    for (int k = -10; k < 10; k++)
                    {
                        double a1 = ImprovedNoise.noise(i, j, k);
                        double a2 = ImprovedNoise.noised(i, j, k).X;

                        Assert.AreEqual(a1, 0);
                        Assert.AreEqual(a2, 0);
                        Assert.AreEqual(a1, a2);
                    }
                }
            }
        }

        [TestMethod]
        public void NoisedTest7()
        {
            double eps = 1E-10;
            for (double i = -10; i < 10; i+=0.1)
            {
                for (double j = -10; j < 10; j+=0.1)
                {
                    for (double k = -10; k < 10; k+=0.1)
                    {
                        double a1 = ImprovedNoise.noise(i, j, k);
                        double a2 = ImprovedNoise.noise(i+256, j+256, k+256);

                        Assert.AreEqual(a1, a2, eps);
                    }
                }
            }
        }

        [TestMethod]
        public void NoisedBitmanTest()
        {
            Assert.AreEqual(1E-3, 0.001);
            Assert.AreEqual(1E-4, 0.0001);

            double eps = 1E-5;
            Random random = new Random();
            for (double i = -1; i < 1; i+=0.05)
            {
                for (double j = -1; j < 1; j+=0.05)
                {
                    for (double k = -1; k < 1; k+=0.05)
                    {
                        double c = random.NextDouble();

                        double e1 = ImprovedNoise.noise(i, j, k);
                        double a1 = ImprovedNoiseDerivatives.noisedBitman(i, j, k).X;

                        double e2 = ImprovedNoise.noise(i+eps, j+eps, k+eps);
                        double a2 = ImprovedNoiseDerivatives.noisedBitman(i+eps, j+eps, k+eps).X;

                        double e3 = ImprovedNoise.noise(-i, -j, -k);
                        double a3 = ImprovedNoiseDerivatives.noisedBitman(-i, -j, -k).X;

                        double e4 = ImprovedNoise.noise(-i-eps, -j-eps, -k-eps);
                        double a4 = ImprovedNoiseDerivatives.noisedBitman(-i-eps, -j-eps, -k-eps).X;

                        Assert.AreEqual(e1, a1, eps);
                        Assert.AreEqual(e2, a2, eps);
                        Assert.AreEqual(e3, a3, eps);
                        Assert.AreEqual(e4, a4, eps);
                    }
                }
            }
        }
    }
}
