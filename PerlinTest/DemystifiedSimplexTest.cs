using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perlin;
using System;

namespace PerlinTest
{
    [TestClass]
    public class DemystifiedSimplexTest
    {
        [TestMethod]
        public void NoisedTest1()
        {
            Assert.AreEqual(1E-3, 0.001);
            Assert.AreEqual(1E-4, 0.0001);

            double eps = 1E-10;
            Random random = new Random();
            Simplex simplex = new Simplex();
            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    for (int k = -10; k < 10; k++)
                    {
                        double c = random.NextDouble();

                        float expect1 = simplex.Evaluate(i, j, k);
                        float actual1 = (float) SimplexNoise.noised(i, j, k).x;

                        float expect2 = simplex.Evaluate(i+eps, j+eps, k+eps);
                        float actual2 = (float) SimplexNoise.noised(i+eps, j+eps, k+eps).x;

                        float expect3 = simplex.Evaluate(-i, -j, -k);
                        float actual3 = (float) SimplexNoise.noised(-i, -j, -k).x;

                        float expect4 = simplex.Evaluate(-i-eps, -j-eps, -k-eps);
                        float actual4 = (float) SimplexNoise.noised(-i-eps, -j-eps, -k-eps).x;

                        float expect5 = simplex.Evaluate(c, c, c);
                        float actual5 = (float) SimplexNoise.noised(c, c, c).x;

                        float expect6 = simplex.Evaluate(c+eps, c+eps, c+eps);
                        float actual6 = (float) SimplexNoise.noised(c+eps, c+eps, c+eps).x;

                        float expect7 = simplex.Evaluate(-c, -c, -c);
                        float actual7 = (float) SimplexNoise.noised(-c, -c, -c).x;

                        float expect8 = simplex.Evaluate(-c-eps, -c-eps, -c-eps);
                        float actual8 = (float) SimplexNoise.noised(-c-eps, -c-eps, -c-eps).x;

                        Assert.AreEqual(expect1, actual1, eps);
                        Assert.AreEqual(expect2, actual2, eps);
                        Assert.AreEqual(expect3, actual3, eps);
                        Assert.AreEqual(expect4, actual4, eps);
                        Assert.AreEqual(expect5, actual5, eps);
                        Assert.AreEqual(expect6, actual6, eps);
                        Assert.AreEqual(expect7, actual7, eps);
                        Assert.AreEqual(expect8, actual8, eps);
                    }
                }
            }
        }

        [TestMethod]
        public void NoisedTest2()
        {
            double eps = 1E-5;
            Simplex simplex = new Simplex();
            for (double i = -10; i < 10; i+=0.5)
            {
                for (double j = -10; j < 10; j+=0.5)
                {
                    for (double k = -10; k < 10; k+=0.5)
                    {
                        double expect = simplex.Evaluate(i,j,k);
                        double actual = SimplexNoise.noised(i, j, k).x;

                        Assert.AreEqual(expect, actual, eps);
                    }
                }
            }
        }
    }
}
