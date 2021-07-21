using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perlin;
using System;
using Vector3 = Perlin.ImprovedNoise.Vector3;
using Vector4 = Perlin.ImprovedNoise.Vector4;

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

            double eps = 1E-10;
            Random random = new Random();
            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    for (int k = -10; k < 10; k++)
                    {
                        double c = random.NextDouble();

                        double e1 = ImprovedNoise.noise(i, j, k);
                        double a1 = ImprovedNoise.noised(i, j, k).x;

                        double e2 = ImprovedNoise.noise(i+eps, j+eps, k+eps);
                        double a2 = ImprovedNoise.noised(i+eps, j+eps, k+eps).x;

                        double e3 = ImprovedNoise.noise(-i, -j, -k);
                        double a3 = ImprovedNoise.noised(-i, -j, -k).x;

                        double e4 = ImprovedNoise.noise(-i-eps, -j-eps, -k-eps);
                        double a4 = ImprovedNoise.noised(-i-eps, -j-eps, -k-eps).x;

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
        public void NoisedTest2()
        {
            Perlin.ImprovedNoise.Vector4 noise;
            noise = Perlin.ImprovedNoise.noised(1.5,2.5,3.5);

            Perlin.ImprovedNoise.Vector3 normal;
            normal = new Perlin.ImprovedNoise.Vector3(noise.y, -1, noise.w);

            Perlin.ImprovedNoise.Vector3 Tx = new Perlin.ImprovedNoise.Vector3(1, noise.y, 0);
            Perlin.ImprovedNoise.Vector3 Tz = new Perlin.ImprovedNoise.Vector3(0, noise.w, 1);
            Perlin.ImprovedNoise.Vector3 cross = Perlin.ImprovedNoise.cross(Tx, Tz);

            Assert.AreEqual(cross.x, normal.x);
            Assert.AreEqual(cross.y, normal.y);
            Assert.AreEqual(cross.z, normal.z);
        }

        /*
        [TestMethod]
        public void NoisedTest3()
        {
            Perlin.ImprovedNoise.Vector4 noise;
            noise = Perlin.ImprovedNoise.noised(1.5,2.5,3.5);

            Perlin.ImprovedNoise.Vector3 normal;
            normal = new Perlin.ImprovedNoise.Vector3(noise.y, -1.0, noise.w);
            normal = Perlin.ImprovedNoise.normalize(normal);

            Assert.AreEqual(1, Perlin.ImprovedNoise.magnitude(normal));
        }

        [TestMethod]
        public void NoisedTest4()
        {
            Perlin.ImprovedNoise.Vector4 noise;
            noise = Perlin.ImprovedNoise.noised(1.5,2.5,3.5);

            Perlin.ImprovedNoise.Vector3 normal;
            normal = new Perlin.ImprovedNoise.Vector3(noise.y, -1.0, noise.w);
            normal = Perlin.ImprovedNoise.normalize(normal);

            Perlin.ImprovedNoise.Vector3 Tx = new Perlin.ImprovedNoise.Vector3(1, noise.y, 0);
            Perlin.ImprovedNoise.Vector3 Tz = new Perlin.ImprovedNoise.Vector3(0, noise.w, 1);

            Perlin.ImprovedNoise.Vector3 cross;
            cross = Perlin.ImprovedNoise.cross(Tx, Tz);
            cross = Perlin.ImprovedNoise.normalize(cross);

            Assert.AreEqual(cross.x, normal.x);
            Assert.AreEqual(cross.y, normal.y);
            Assert.AreEqual(cross.z, normal.z);

            Assert.AreEqual(1, Perlin.ImprovedNoise.magnitude(normal));
            Assert.AreEqual(1, Perlin.ImprovedNoise.magnitude(cross));
        }
        */

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
                        double a2 = ImprovedNoise.noised(i, j, k).x;

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

        /*
        [TestMethod]
        public void DerivativeTest()
        {
            double diff = 0.01;
            double step = 0.25;

            for (double i = -10; i < 10; i+=step)
            {
                for (double j = -10; j < 10; j+=step)
                {
                    Vector4 x0y0 = Perlin.ImprovedNoise.noised(i, j, 0);
                    Vector4 x1y0 = Perlin.ImprovedNoise.noised(i+diff, j, 0);
                    Vector4 x0y1 = Perlin.ImprovedNoise.noised(i, j+diff, 0);

                    double gradx = Perlin.ImprovedNoise.gradient(i, i+diff, x1y0.x, x0y0.x);
                    double grady = Perlin.ImprovedNoise.gradient(j, j+diff, x0y1.x, x0y0.x);

                    Assert.AreEqual(x0y0.x, x1y0.x, 0.1);
                    Assert.AreEqual(x0y0.x, x0y1.x, 0.1);

                    Assert.AreEqual(x0y0.y, gradx, 0.1);
                    Assert.AreEqual(x0y0.z, grady, 0.1);
                }
            }
        }
        */
    }
}
