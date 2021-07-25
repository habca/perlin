// 

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Numerics;

using Simplex;

namespace SimplexTest
{
    [TestClass]
    public class NoiseHardwareTest
    {
        static double eps = 1E-2;
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
                        double perlin = NoiseHardwarePerlin.noise(i, j, k);
                        double custom = NoiseHardware.noise(i, j, k);
                        Vector4 gustavson = SimplexNoiseGustavson.noise(i, j, k);
                        
                        Assert.AreEqual(perlin, custom, eps);
                        Assert.AreEqual(perlin, gustavson.X, eps);
                    }
                }
            }
        }
    }
}
