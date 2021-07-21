using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Simplex;

namespace SimplexTest
{
    [TestClass]
    public class NoiseHardwareTest
    {
        [TestMethod]
        public void NoiseTest()
        {
            double eps = 1E-1;
            
            for (double i = -10; i < 10; i+=0.5)
            {
                for (double j = -10; j < 10; j+=0.5)
                {
                    for (double k = -10; k < 10; k+=0.5)
                    {
                        double expect = Noise3.noise(i,j,k);
                        double actual = NoiseHardware.noise(i,j,k);

                        Assert.AreEqual(expect, actual, eps);
                    }
                }
            }
        }
    }
}
